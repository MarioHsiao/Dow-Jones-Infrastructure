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
        
        if (!this.$element.attr('data-canvas-id'))
            this.$element.attr('data-canvas-id', this.options.canvasId);

        this._numberOfZones = this.options.NumberOfGroups;

        this._canvasRenderManager = $(this.element).get(0);

        this._initializeZones();
        this._initializeModules();
        this._initializeModuleReordering();

        this.subscribe('CanvasModule.RemoveModuleRequest', this._delegates.fireModuleRemoved);

        this._debug("Done initializing canvas: " + (new Date().getTime() - startDate.getTime()));
    },

    // Obsolete
    addModule: function (moduleId) {
        this.loadModule(moduleId);
    },

    loadModule: function (moduleId, onSuccess, onError) {
        this._loadModule(moduleId, onSuccess, onError, this._delegates.addModuleToCanvas);
    },
    
    _loadModule: function (moduleId, onSuccess, onError, addModuleToCanvas) {
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
                    if(onError) onError(err);
                }
                else {
                    addModuleToCanvas(xhr.responseText);
                    if(onSuccess) onSuccess(xhr.responseText);
                }
            }),
            dataType: 'html'
        });
    },
    
    _addModuleToCanvas: function (html) {
        this.$element.prepend(html);
    },

    reloadModule: function (moduleId) {
        var module = this.module(moduleId);

        if (!module) return;

        module.showLoadingArea();
        
        this._loadModule(
            moduleId,
            null,
            function (err) {
                module.showErrorMessage(err);
            },
            function (html) {
                module.$element.replaceWith(html);
            }
        );
    },

    deleteModule: function (moduleId, onSuccess, onError) {
        var request = { pageId: this.get_canvasId(), moduleId: moduleId };

        var module = this.module(moduleId) || { showLoadingArea: function () {}, showContentArea: function () {} };

        module.showLoadingArea();

        $.ajax({
            url: this.options.deleteModuleUrl + '?' + $.param(request),
            type: 'DELETE',
            success: $dj.delegate(this, function (data) {
                        module.$element.remove();

                        this._initializeModules();

                        if (onSuccess) {
                            onSuccess(data);
                        }
                    }),
            error: $dj.delegate(this, function (data) {
                        module.showContentArea();
                        if (onError) {
                             onError(data);
                        }
                    })
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

    module: function (moduleId) {
        if (!moduleId) return null;

        var moduleEls = $(this._moduleSelector, this.element);
        var dataAttr = '[data-module-id="' + moduleId + '"]';
        var module = moduleEls.filter(dataAttr);
        return module.findComponent(DJ.UI.AbstractCanvasModule);
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

    _fireModuleRemoved: function (args) {
        this.deleteModule(args.moduleId);
    },

    _fireModuleAdded: function (moduleElementId) {
        this._initializeModules();
        this.publish('addModuleSuccess.dj.Canvas', moduleElementId);
    },

    _fireSortableOnActivate: function (e, ui) {
        this.$activate$i++;
        this.$element.addClass('sorting');
    },

    _fireSortableOnDeactivate: function (e, ui) {
        this.$element.removeClass('sorting');
        
        this.$deactivate$i++;

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
            addModuleToCanvas: $dj.delegate(this, this._addModuleToCanvas),
            fireModuleAdded: $dj.delegate(this, this._fireModuleAdded),
            fireModuleRemoved: $dj.delegate(this, this._fireModuleRemoved),
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

            var moduleId = module.get_moduleId();
            module.setOwner(this);

            if (module.options.needsClientData) {
                module.getData();
                module.options.needsClientData = false;
            }

            var zoneIndex = -1;
            var layout = this.options.layout;
            if (layout && layout.groups) {
                var groups = layout.groups;
                for (var groupId = 0; groupId < groups.length; groupId++) {
                    for (var y = 0; y < groups[groupId].length; y++) {
                        if (groups[groupId][y] == moduleId)
                            zoneIndex = groupId;
                    }
                }
            }

            if (zoneIndex <= -1) {
                zoneIndex = Math.ceil(module.options.position % zoneCount);
            }

            var zone = (zoneIndex > -1) ? zones.get(zoneIndex) : null;
            if(zone) {
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

        var zones = this.getZones();

        zones.sortable($.extend(settings));

        if (zones.length > 1) {
            zones.sortable("option", "connectWith", zones);
        }

        // Initialize Crosshairs
        this._sortableItems = $(settings.items, zones);
        this._sortableItems.find(this.sortableSettings.handle)
                    .css({ cursor: 'move' })
                    .mousedown(this._fireCrossHairsCursorOnMousedown)
                    .mouseup(this._fireCrossHairsCursorOnMouseup);

        $(">H3", $(this.sortableSettings.handle, this._sortableItems));

        this._debug('Module reordering nitialized: ' +
                        this._sortableItems.length + ' modules in ' + this._numberOfZones + ' zones');

        this._numberOfZones = zones.length;
    },

    _initializeZones: function () {
        var zones = this.getZones();
        var numberOfGroups = this.options.NumberOfGroups;
        var layoutClass = 'span' + (12 / numberOfGroups);

        if (zones.length === 0) {
            var zoneContainer = $(this._canvasRenderManager);

            for (var i = 0; i < numberOfGroups; i++) {
                $("<div />")
                    .attr('id', 'zone-' + i)
                    .addClass(this._groupSelector.substring(1))
                    .addClass(layoutClass)
                    .appendTo(zoneContainer);
            }
        }
    },
    
    EOF: null
});

$.plugin('dj_Canvas', DJ.UI.AbstractCanvas);
    
$dj.debug('Registered DJ.UI.AbstractCanvas (extends DJ.UI.Component)');

DJ.UI.Canvas = {
    find: function (canvasId) {
        var canvases = $('.dj_Canvas');

        if (canvasId) {
            // Try to get a specific one
            for (var i = 0; i < canvases.length; i++) {
                var canvas = $(canvases[i]);
                if (canvas && canvas.data('canvas-id') == canvasId)
                    return canvas.findComponent(DJ.UI.AbstractCanvas);
            }
        } else if(canvases.length == 1) {
            // Otherwise, return the first canvas
            return $(canvases).findComponent(DJ.UI.AbstractCanvas);
        }

        return null;
    }
}