DJ.UI.AbstractCanvas = DJ.UI.CompositeComponent.extend({

    _moduleCount: 0,
    _moduleSelector: ".dj_module",
    _pubSubManager: null,

    init: function (element, meta) {
        this._debug("Initializing canvas...");

        this._pubSubManager = this._pubSubManager || new DJ.PubSubManager();

        this._super(element, meta);

        $.extend(this.options, window[this.getId() + "_clientState"]);

        this._initializeLayout();
        this._initializeModules();

        this.subscribe('RemoveModuleRequest.dj.CanvasModule', this._delegates.fireModuleRemoved);
    },

    // Obsolete
    addModule: function (moduleId) {
        $dj.warn("[OBSOLETE] AbstractCanvas.addModule(moduleId) is obsolete -- use loadModule(moduleId) instead");

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
                    if (onError) onError(err);
                }
                else {
                    addModuleToCanvas(xhr.responseText);
                    if (onSuccess) onSuccess(xhr.responseText);
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

        var module = this.module(moduleId) || { showLoadingArea: function () { }, showContentArea: function () { } };

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

    // Obsolete
    getModuleIds: function (modules) {
        $dj.warn("[OBSOLETE] AbstractCanvas.getModuleIds() is obsolete");

        modules = modules || this.getModules();
        var moduleIds = _.map(modules, function (module) { return module.get_moduleId(); });
        return moduleIds;
    },

    getModules: function () {
        var moduleElements = $(this._moduleSelector, this.$element);

        var modules = _.map(moduleElements, function (el) {
            var module = $(el).findComponent(DJ.UI.AbstractCanvasModule);
            return module;
        });

        return _.filter(modules, function (module) { return module !== null; });
    },

    dispose: function () {
        this._super();
        this.options = null;
    },

    module: function (moduleId) {
        if (!moduleId) return null;

        var modules = this.getModules() || [];

        for (var i = 0; i < modules.length; i++) {
            if (modules[i].get_moduleId == moduleId)
                return modules[i];
        }

        return null;
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

        this._layout.update();
        this._layout.save();
    },

    getData: function (forceCacheRefresh) {
        var modules = this.getModules();
        _.each(modules, function (module) {
            module.getData(forceCacheRefresh);
        }, this);
    },

    // Obsolete
    getZones: function () {
        $dj.warn("[OBSOLETE] AbstractCanvas.getZones() is obsolete");

        if (this._layout._getZones)
            return this._layout._getZones();

        return null;
    },

    // Obsolete
    saveCanvasModulePositions: function () {
        $dj.warn("[OBSOLETE] AbstractCanvas.saveCanvasModulePositions() is obsolete");
        this._layout.save();
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

    _fireModuleRemoved: function (args) {
        this.deleteModule(args.moduleId);
    },

    _fireModuleAdded: function (moduleElementId) {
        this._initializeModules();
        this.publish('addModuleSuccess.dj.Canvas', moduleElementId);
    },

    _initializeDelegates: function () {
        this._delegates = {
            addModuleToCanvas: $dj.delegate(this, this._addModuleToCanvas),
            fireModuleAdded: $dj.delegate(this, this._fireModuleAdded),
            fireModuleRemoved: $dj.delegate(this, this._fireModuleRemoved)
        };
    },

    _initializeElements: function () { },
    _initializeEventHandlers: function () { },

    _initializeLayout: function () {
        // HACK: options.layout doesn't actually exist yet - fake it till you make it
        this.options.layout = this.options.layout || { zoneCount: this.options.NumberOfGroups };
        this.options.layout.dataServiceUrl = this.options.webServiceBaseUrl + '/modules/positions/json';

        // TODO: Layout Factory
        var layoutOptions = this.options.layout;
        layoutOptions.canvasId = this.options.canvasId,
        this._layout = new DJ.UI.AbstractCanvas.ZoneLayout(this.element, layoutOptions);
    },

    _initializeModules: function () {
        var modules = this.getModules();

        _.each(modules, function (module) {
            if (module === null) { return; }

            module.setOwner(this);

            if (module.options.needsClientData) {
                module.getData();
                module.options.needsClientData = false;
            }

            // wire up events
            this.events = this.events || {};
            _.each(module.events, function (value, key) {
                this.events[key] = value;
            }, this);
        }, this);

        this._layout.update();
    },

    EOF: null
});


$dj.debug('Registered DJ.UI.AbstractCanvas (extends DJ.UI.Component)');


DJ.UI.Canvas = DJ.UI.AbstractCanvas.extend({});
DJ.UI.Canvas.find = function (canvasId) {
    var canvases = $('.dj_Canvas');

    if (canvasId) {
        // Try to get a specific one
        for (var i = 0; i < canvases.length; i++) {
            var canvas = $(canvases[i]);
            if (canvas && canvas.data('canvas-id') == canvasId)
                return canvas.findComponent(DJ.UI.AbstractCanvas);
        }
    } else if (canvases.length == 1) {
        // Otherwise, return the first canvas
        return $(canvases).findComponent(DJ.UI.AbstractCanvas);
    }

    return null;
};

$.plugin('dj_Canvas', DJ.UI.Canvas);
$dj.debug('Registered DJ.UI.Canvas');





DJ.UI.CanvasLayout = DJ.Component.extend({
    init: function (element, options) {
        this._super({ options: options });
        this._canvas = element;
    },

    save: function () {
        $dj.debug('TODO: Implement DJ.UI.CanvasLayout.save()');
    },

    update: function () {
        $dj.debug('TODO: Implement DJ.UI.CanvasLayout.update()');
    },

    EOF: null
});

$dj.debug('Registered DJ.UI.CanvasLayout');



DJ.UI.AbstractCanvas.ZoneLayout = DJ.UI.CanvasLayout.extend({
    defaults: {
        dataServiceUrl: null,
        groups: null,
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
        }
    },

    _zoneSelector: '.dj_group',
    _moduleSelector: '.dj_module',

    save: function () {
        var zones = [];

        _.each(this._getZones(), function (zone) {
            var modules = this._getModules(zone);
            var moduleIds = _.map(modules, function (module) { return module.get_moduleId(); });
            zones[zones.length] = moduleIds;
        }, this);

        var request = { pageId: this.options.canvasId, columns: zones };

        $.ajax({
            url: this.options.dataServiceUrl,
            type: 'PUT',
            data: JSON.stringify(request)
        });

        return request;
    },

    update: function () {
        var zones = this._getZones();
        var modules = this._getModules();
        var zoneModuleIds = this.options.groups;

        _.each(modules, function (module) {
            if (module === null) { return; }

            var zoneIndex = -1;

            if (zoneModuleIds) {
                for (var groupId = 0; groupId < zoneModuleIds.length; groupId++) {
                    for (var y = 0; y < zoneModuleIds[groupId].length; y++) {
                        if (zoneModuleIds[groupId][y] == module.get_moduleId())
                            zoneIndex = groupId;
                    }
                }
            }

            if (zoneIndex == -1) {
                zoneIndex = Math.ceil(module.options.position % zones.length);
            }

            var zone = (zoneIndex != -1) ? zones.get(zoneIndex) : null;
            if (zone) {
                $(zone).append(module.element);
            }
            else {
                $dj.debug('hiding module ' + module.toString() + ' because it does not have a Position');
                $(module.element).hide();
            }
        });

        this._initializeModuleReordering();
    },

    _getModules: function (zone) {
        var moduleElements = $(this._moduleSelector, zone || this._canvas);

        var modules = _.map(moduleElements, function (el) {
            var module = $(el).findComponent(DJ.UI.AbstractCanvasModule);
            return module;
        });

        return _.filter(modules, function (module) { return module !== null; });
    },

    _getZones: function () {
        var zoneCount = this.options.zoneCount || (this.options.groups || [[]]).length;
        var zones = $(this._zoneSelector, this._canvas).get();

        if (zones.length !== zoneCount) {
            var zoneClass = this._zoneSelector.substring(1);

            zoneClass += " span" + (12 / zoneCount);

            for (var i = 0; i < zoneCount; i++) {
                var zone =
                    $("<div />")
                        .addClass(zoneClass)
                        .attr('id', 'zone-' + i)
                        .appendTo(this._canvas);

                zones.push(zone);
            }
        }

        return $(zones);
    },

    _initializeEventHandlers: function () { },

    _initializeDelegates: function () {
        this._delegates = {
            save: $dj.delegate(this, this.save)
        };
    },

    _initializeModuleReordering: function () {
        this._debug("Initializing module reordering...");

        // Get a copy of the default sort-able settings
        // and update it with our current items and delegates
        var settings = $.extend({}, this.options.sortableSettings);

        var zones = this._getZones();
        var sortableItems = $(settings.items, zones);
        var tooltipHandles = sortableItems.find(settings.handle + ">H3");

        // Save delegate
        var savePositions = this._delegates.save;

        $.extend(settings, {
            start: function (e, ui) {
                $(ui.helper).addClass(settings.draggingClass);

                if (document.selection) {
                    document.selection.clear();
                }

                zones.disableSelection().sortable('refreshPositions');

                tooltipHandles.data("enableSimpleTooltip", false);
                $("#dj_tooltip").hide();
            },

            stop: function (e, ui) {
                $(ui.item).css({ width: '' }).removeClass(settings.draggingClass);
                zones.sortable('enable');
            },

            deactivate: function (e, ui) {
                zones.enableSelection();

                if (document.selection) {
                    document.selection.clear();
                }

                savePositions();

                tooltipHandles.data("enableSimpleTooltip", true);
            }
        });

        zones.sortable(settings);

        if (zones.length > 1) {
            zones.sortable("option", "connectWith", zones);
        }

        // Initialize Crosshairs
        sortableItems.find(settings.handle).css({ cursor: 'move' });

        this._debug('Module reordering initialized: ' +
                    sortableItems.length + ' modules in '
                    + zones.length + ' zones');
    },

    EOF: null
});

$dj.debug('Registered DJ.UI.AbstractCanvas.ZoneLayout');