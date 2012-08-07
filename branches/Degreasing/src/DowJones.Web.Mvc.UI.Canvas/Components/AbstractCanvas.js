/// <reference name="common.js" assembly="DowJones.Web.Mvc" />

    DJ.UI.AbstractCanvas = DJ.UI.CompositeComponent.extend({

        defaults: {
            NumberOfGroups: 1
        },

        sortableSettings: {
            axis: false,
            containment: false,
            delay: 100,
            distance: 30,
            draggingClass: "dj_module-dragging",
            forcePlaceholderSize: true,
            handle: ".dj_module-handle",
            items: "> .dj_module-movable",
            opacity: 0.8,
            placeholder: "dj_module-placeholder",
            revert: 300,
            scroll: true,
            tolerance: "pointer" // pointer || intersect [default]
        },

        _canvasName: null,
        _canvasRenderManager: null,
        _moduleCount: 0,
        _registeredModules: null,
        _moduleSelector: ".dj_module",
        _contentSelector: ".dj_module-body",
        _groupContainerSelector: ".dj_groups",
        _groupSelector: ".dj_group",
        _sortableItems: {},
        _pubSubManager: this._pubSubManager || new DJ.PubSubManager(),

        $activate$i: 0,
        $deactivate$i: 0,
        $subscribedElements: [],

        init: function (element, meta) {
            var startDate = new Date();
            this._debug("Initializing canvas...");

            this._super(element, meta);

            this._selectors = $dj.clone(this._selectors);
            this.sortableSettings = $dj.clone(this.sortableSettings);

            $.extend(this.options, this.getSettings());

            this._numberOfZones = this.options.NumberOfGroups;

            this._canvasRenderManager = $(this.element).get(0);

            this._initializeZones();
            this._initializeModules();
            this._initializeModuleReordering();

            this._debug("Done initializing canvas: " + (new Date().getTime() - startDate.getTime()));
        },

        // Obsolete
        addModule: function (moduleId) {
            this.loadModule(moduleId);
        },

        loadModule: function (moduleId) {
            if (!this.canAddModule(moduleId)) { return; }

            var request = {
                'pageId': this.options.canvasId,
                'id': moduleId
            };

            if (DJ.config && DJ.config.credentials)
                request['SA_FROM'] = DJ.config.credentials.SA_FROM;

            // Pass a callback function to initialize the canvas after 
            // the partial rendering initialization has completed
            request.callback = $dj.callback(this._delegates.fireModuleAdded);

            $.ajax({
                url: this.options.loadModuleUrl + '?' + $.param(request),
                cache: false,
                complete: $dj.delegate(this, function (xhr) {
                    var err = $dj.getError(xhr);
                    if (err !== null) {
                        this.publish('addModuleError.dj.Canvas', err);
                        return;
                    }

                    this.$element.append(xhr.responseText);
                }),
                dataType: 'html'
            });
        },

        deleteModule: function (moduleId, onSuccess, onError) {
            var request = { pageId: this.get_canvasId(), moduleId: moduleId };

            var success =
                $dj.delegate(this,
                    function (obj) {
                        this._initializeModules();
                        if (onSuccess) { onSuccess(obj); }
                    }
                );

            $.ajax({
                url: this.options.deleteModuleUrl + '?' + $.param(request),
                type: 'DELETE',
                success: success,
                error: onError
            });
        },

        getModuleIds: function (modules) {
            modules = modules || this.getModules();
            var moduleIds = _.map(modules, function (module) { return module.get_moduleId(); });
            return moduleIds;
        },

        getModules: function (zone) {
            var context = zone || this.$element;

            var moduleElements = $(this._moduleSelector, context);

            var modules = _.map(moduleElements, function (el) {
                var module = $(el).findComponent(DJ.UI.AbstractCanvasModule);
                return module;
            });

            return _.filter(modules, function (module) { return module !== null; });
        },

        dispose: function () {
            this._super();

            // Clean up common variables
            this._canvasRenderManager = null;
            this._sortableItems = null;
            this._sortableSettings = null;
            this._registeredModules = null;
            this.options = null;
            this.$deactivate$i = null;
            this.$activate$i = null;
            this.$subscribedElements = null;
        },

        getSettings: function () {
            var _temp = window[this.getId() + "_clientState"];
            if (_temp !== null) {
                return _temp;
            }
            return null;
        },

        getZones: function () {
            var zones = $(this._groupSelector, this._canvasRenderManager);
            return zones;
        },

        moveModule: function (module, direction) {
            this._debug('Moving module ' + module.toString() + ' ' + direction);

            var $module = $(module._module);
            if (direction === 'up') {
                $module.insertBefore($module.prev());
            }
            else {
                $module.insertAfter($module.next());
            }

            this._initializeZones();
            this.saveCanvasModulePositions();
        },

        getData: function (forceCacheRefresh) {
            var modules = this.getModules();
            _.each(modules, function (module) {
                module.getData(forceCacheRefresh);
            }, this);
        },

        saveCanvasModulePositions: function () {
            var zones = [];

            _.each(this.getZones(), function (zone) {
                var modules = this.getModules(zone);
                zones[zones.length] = this.getModuleIds(modules);
            }, this);

            var request = { pageId: this.options.canvasId, columns: zones };

            $.ajax({
                url: this.options.webServiceBaseUrl + '/modules/positions/json',
                type: 'PUT',
                data: JSON.stringify(request)
            });

            return request;
        },

        canAddModule: function () {
            this._debug("*** canAddModule == true ***  Override canAddModule function to provide canvas-specific logic");
            return true;
        },

        publish: function (/* string */eventName, /* object */args) {
            this._pubSubManager.publish(eventName, args);
        },


        subscribe: function (/* string */eventName, /* function() */handler) {
            return this._pubSubManager.subscribe(eventName, handler);
        },


        _invokeService: function (params) {
            $.ajax(params);
        },

        _fireModuleAdded: function (moduleElementId) {
            this._initializeModules();
            this.publish('addModuleSuccess.dj.Canvas', moduleElementId);
        },

        _fireSortableOnActivate: function (e, ui) {
            this.$activate$i++;
        },

        _fireSortableOnDeactivate: function (e, ui) {
            this.$deactivate$i++;

            // If all of the zones are not done deactivating,
            // ignore this event and wait for the last one
            if (this.$deactivate$i % this._numberOfZones !== 0) {
                this._debug('Ignoring fireSortableOnDeactivate for ' + e.id +
                            '. Waiting for last zone to deactivate.');
                return;
            }

            this.getZones().enableSelection();

            if (document.selection) {
                document.selection.clear();
            }

            this.saveCanvasModulePositions();

            var _groupCntr = $(this._groupContainerSelector, this._canvasRenderManager).get(0);
            if (_groupCntr) {
                var _children = $(_groupCntr).children();
                if ($.browser.msie && $.browser.version === 6.0) {
                    _children.css({ 'height': '400px' });
                }
                _children.css({ 'min-height': '400px' });
            }

            this._sortableItems.find(this.sortableSettings.handle + ">H3").data("enableSimpleTooltip", true);
        },

        _fireSortableOnStart: function (e, ui) {
            $(ui.helper).addClass(this.sortableSettings.draggingClass);

            if (document.selection) {
                document.selection.clear();
            }

            this.getZones()
                    .disableSelection()
                    .sortable('refreshPositions');
            this._sortableItems.find(this.sortableSettings.handle + ">H3").data("enableSimpleTooltip", false);
            $("#dj_tooltip").hide();
        },

        _fireSortableOnStop: function (e, ui) {
            $(ui.item).css({ width: '' }).removeClass(this.sortableSettings.draggingClass);
            this.getZones().sortable('enable');
        },

        _initializeDelegates: function () {
            this._delegates = {
                fireModuleAdded: $dj.delegate(this, this._fireModuleAdded),
                fireSortableOnActivate: $dj.delegate(this, this._fireSortableOnActivate),
                fireSortableOnDeactivate: $dj.delegate(this, this._fireSortableOnDeactivate),
                fireSortableOnStart: $dj.delegate(this, this._fireSortableOnStart),
                fireSortableOnStop: $dj.delegate(this, this._fireSortableOnStop)
            };
        },

        _initializeElements: function () { },

        _initializeModules: function () {
            var zones = this.getZones();
            var zoneCount = zones.length;

            var modules = this.getModules();

            _.each(modules, function (module) {
                if (module === null) { return; }

                var $el = module.$element;

                module.setOwner(this);

                if (module.options.needsClientData) {
                    module.getData();
                    module.options.needsClientData = false;
                }

                if (module.options.position > -1) {
                    var zoneIndex = Math.ceil(module.options.position % zoneCount);
                    var zone = (zoneIndex > -1) ? zones.get(zoneIndex) : null;
                    $(zone).append($el);
                }
                else {
                    $dj.debug('hiding module ' + module.toString() + ' because it does not have a Position');
                    $(module.element).hide();
                }

                // wire up events
                this.events = this.events || {};
                _.each(module.events, function (value, key) {
                    this.events[key] = value;
                }, this);
            }, this);
        },

        _initializeModuleReordering: function () {
            this._debug("Initializing module reordering...");

            // Get a copy of the default sort-able settings
            // and update it with our current items and delegates
            var settings = $.extend(this.sortableSettings, {
                start: this._delegates.fireSortableOnStart,
                stop: this._delegates.fireSortableOnStop,
                activate: this._delegates.fireSortableOnActivate,
                deactivate: this._delegates.fireSortableOnDeactivate
            });

            var _zones = this.getZones();

            _zones.sortable($.extend(settings));

            if (_zones.length > 1) {
                _zones.sortable("option", "connectWith", _zones);
            }

            // Initialize Crosshairs
            this._sortableItems = $(settings.items, _zones);
            this._sortableItems.find(this.sortableSettings.handle)
                    .css({ cursor: 'move' })
                    .mousedown(this._fireCrossHairsCursorOnMousedown)
                    .mouseup(this._fireCrossHairsCursorOnMouseup);

            $(">H3", $(this.sortableSettings.handle, this._sortableItems));

            this._debug('Module reordering nitialized: ' +
                        this._sortableItems.length + ' modules in ' + this._numberOfZones + ' zones');

            this._numberOfZones = _zones.length;
        },

        _initializeZones: function () {
            var zones = this.getZones();

            if (zones.length === 0) {
                var zoneContainer = $(this._canvasRenderManager);
                var zoneClass = this._groupSelector.substring(1);
                var i;

                for (i = 0; i < this.options.NumberOfGroups; i++) {
                    $("<div />")
                            .addClass(zoneClass)
                            .attr('id', 'zone-' + i)
                            .appendTo(zoneContainer);
                }
            }
        }

    });

    $.plugin('dj_Canvas', DJ.UI.AbstractCanvas);
    
    $dj.debug('Registered DJ.UI.AbstractCanvas (extends DJ.UI.Component)');

