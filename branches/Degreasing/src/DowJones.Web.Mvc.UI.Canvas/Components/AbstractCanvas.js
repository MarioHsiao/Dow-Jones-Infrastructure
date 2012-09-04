DJ.UI.AbstractCanvas = DJ.UI.CompositeComponent.extend({

    layout: null,
    _moduleSelector: ".dj_module",
    _pubSubManager: null,

    init: function (element, meta) {
        this._debug("Initializing canvas...");

        var $meta = $.extend({ name: 'Canvas' }, meta);
        this._super(element, $meta);

        this._pubSubManager = this._pubSubManager || new DJ.PubSubManager();

        $.extend(this.options, window[this.getId() + "_clientState"]);

        this._initializeModules();
        this.layout = DJ.UI.Canvas.Layout.initialize(this, this.options.layout);

        this.subscribe('RemoveModuleRequest.dj.CanvasModule', this._delegates.fireModuleRemoved);
    },

    addModule: function (module) {
        if (module === null || module === undefined)
            return;

        if (!isNaN(module)) {
            $dj.warn("[OBSOLETE] AbstractCanvas.addModule(moduleId) is obsolete -- use loadModule(moduleId) instead");
            this.loadModule(parseInt(module));
        }

        this.layout.add(module);
    },

    canAddModule: function () {
        return true;
    },

    deleteModule: function (moduleId, onSuccess, onError) {
        var request = { pageId: this.get_canvasId(), moduleId: moduleId };

        var module = this.module(moduleId) || { showLoadingArea: function () { }, showContentArea: function () { } };

        module.showLoadingArea();

        $.ajax({
            url: this.options.deleteModuleUrl + '?' + $.param(request),
            type: 'DELETE',
            success: $dj.delegate(this, function (data) {
                this.layout.remove(module);

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

    getData: function (forceCacheRefresh) {
        var modules = this.getModules();
        _.each(modules, function (module) {
            module.getData(forceCacheRefresh);
        }, this);
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

    loadModule: function (moduleId, onSuccess, onError) {
        this._loadModule(moduleId, onSuccess, onError, this._delegates.addModule);
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


    _fireModuleAdded: function (moduleElementId) {
        this._initializeModules();
        this.publish('addModuleSuccess.dj.Canvas', moduleElementId);
    },

    _fireModuleRemoved: function (args) {
        this.deleteModule(args.moduleId);
    },

    _initializeDelegates: function () {
        this._delegates = {
            addModule: $dj.delegate(this, this.addModule),
            fireModuleAdded: $dj.delegate(this, this._fireModuleAdded),
            fireModuleRemoved: $dj.delegate(this, this._fireModuleRemoved)
        };
    },

    _initializeElements: function () { },

    _initializeEventHandlers: function () { },

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
    },

    _loadModule: function (moduleId, onSuccess, onError, addModule) {
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
                    if (addModule) addModule(xhr.responseText);
                    if (onSuccess) onSuccess(xhr.responseText);
                }
            }),
            dataType: 'html'
        });
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



DJ.UI.Canvas.Layout = DJ.Component.extend({
    init: function (canvas, options) {
        this._super({ options: options || {} });
        
        this.element = canvas.element;

        if (!this.options.canvasId && canvas.options) {
            this.options.canvasId = canvas.options.canvasId;
        }
    },

    add: function (module) {
        $dj.debug('TODO: Implement DJ.UI.Canvas.Layout.add()');
    },

    remove: function (module) {
        $dj.debug('TODO: Implement DJ.UI.Canvas.Layout.remove()');
    },

    save: function () {
        $dj.debug('TODO: Implement DJ.UI.Canvas.Layout.save()');
    },

    EOF: null
});


// "Static" members
$.extend(DJ.UI.Canvas.Layout, {
    _layouts: { },
    
    initialize: function (canvas, options) {
        var name = (options || {}).name || 'zone';

        var layout = DJ.UI.Canvas.Layout._layouts[name];

        if (!layout) {
            $dj.debug('Could not find layout "' + name + '" -- defaulting to "zone" layout');
            layout = DJ.UI.Canvas.Layout._layouts['zone'];
        }

        try {
            $dj.debug("Initializing canvas", canvas, "with layout '" + name + "' and options", options);
            return new layout(canvas, options);
        } catch (e) {
            $dj.error(e);
        } 
        
        $dj.error("Failed to initialized canvas layout for ", canvas, "with options", options);

        return null;
    },
    
    register: function(name, layout) {
        DJ.UI.Canvas.Layout._layouts[name] = layout;
        $dj.debug("Registered layout ", name, layout);
    }
});

$dj.debug('Registered DJ.UI.Canvas.Layout');



DJ.UI.Canvas.ZoneLayout = DJ.UI.Canvas.Layout.extend({
    defaults: {
        dataServiceUrl: null,
        zones: null,
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

    init: function (canvas, options) {
        this._super(canvas, options);

        // Load sane defaults from the canvas options
        if (canvas.options) {
            if (!this.options.zoneCount)
                this.options.zoneCount = canvas.options.NumberOfGroups;
            if (!this.options.dataServiceUrl)
                this.options.dataServiceUrl = canvas.options.webServiceBaseUrl + '/modules/positions/json';
        }

        // Map refactored layout methods for backwards compatability
        canvas.getZones = this._delegates.getZones;
        canvas.moveModule = this._delegates.move;
        canvas.saveCanvasModulePositions = this._delegates.save;

        this._initializeModules();
    },

    add: function (module, zone) {
        var el = module;

        if (module instanceof DJ.UI.Component)
            el = module.element;

        zone = zone || 0;

        if (!isNaN(zone))
            zone = this._getZones()[zone];

        $(zone).prepend(el);
    },

    remove: function (module) {
        var el = module;

        if (typeof (module) == DJ.UI.Component)
            el = module.element;

        $(el).remove();
    },

    save: function () {
        var zones = [];

        _.each(this._getZones(), function (zone) {
            var modules = this._getModules(zone);
            var moduleIds = _.map(modules, function (module) { return module.get_moduleId(); });
            zones[zones.length] = moduleIds;
        }, this);

        var request = { pageId: this.options.canvasId, columns: zones };

        this._debug('Saving positions: ' + JSON.stringify(request));

        $.ajax({
            url: this.options.dataServiceUrl,
            type: 'PUT',
            data: JSON.stringify(request)
        });

        return request;
    },


    _getModules: function (zone) {
        var moduleElements = $(this._moduleSelector, zone || this.element);

        var modules = _.map(moduleElements, function (el) {
            var module = $(el).findComponent(DJ.UI.AbstractCanvasModule);
            return module;
        });

        return _.filter(modules, function (module) { return module !== null; });
    },

    _getZones: function () {
        var zoneCount = this.options.zoneCount || (this.options.zones || [[]]).length;
        var zones = $(this._zoneSelector, this.element).get();

        if (zones.length !== zoneCount) {
            var zoneClass = this._zoneSelector.substring(1);

            zoneClass += " span" + (12 / zoneCount);

            for (var i = 0; i < zoneCount; i++) {
                var zone =
                    $("<div />")
                        .addClass(zoneClass)
                        .attr('id', 'zone-' + i)
                        .appendTo(this.element);

                zones.push(zone);
            }
        }

        return $(zones);
    },

    _initializeEventHandlers: function () { },

    _initializeDelegates: function () {
        this._delegates = {
            getZones: $dj.delegate(this, this._getZones),
            move: $dj.delegate(this, this._move),
            save: $dj.delegate(this, this.save)
        };
    },

    _initializeModules: function () {
        $(this._moduleSelector, this.element).hide();

        var zones = this.options.zones;
        var modules = this._getModules();
        var moduleZones = [];

        // Backwards Compatibility: build zones if none are provided
        if (!zones) {
            var zoneCount = this.options.zoneCount || 1;

            zones = new Array(zoneCount);

            _.each(
                 _.sortBy(modules, function (mod) { return mod.options.position; }),
                 function (mod) {
                     var zoneIndex = Math.ceil(mod.options.position % zoneCount);
                     var zone = zones[zoneIndex] || (zones[zoneIndex] = []);
                     zone.push(mod.get_moduleId());
                 }
             );
        }
        
        for (var zoneIndex = 0; zoneIndex < zones.length; zoneIndex++) {
            var zone = zones[zoneIndex] || [];
            for (var i = 0; i < zone.length; i++) {
                moduleZones.push({ moduleId: zone[i], zone: zoneIndex });
            }
        }

        var zoneElements = this._getZones();

        for (var x = 0; x < moduleZones.length; x++) {
            var moduleZone = moduleZones[x];
            var module = _.find(modules, function (mod) {
                return mod.get_moduleId() == moduleZone.moduleId;
            });

            $(module.element).appendTo(zoneElements[moduleZone.zone]).show();
        }

        this._initializeModuleReordering();

        this._debug('Initialized ' + moduleZones.length + ' modules in ' + zones.length + ' zones');
    },

    _initializeModuleReordering: function () {
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

                zones.enableSelection().sortable('enable');

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

        this._debug('Initialized module sorting');
    },

    _move: function (module, direction) {
        this._debug('Moving module ' + module.toString() + ' ' + direction);

        var $module = $(module._module);
        if (direction === 'up') {
            $module.insertBefore($module.prev());
        }
        else {
            $module.insertAfter($module.next());
        }

        this.save();
    },

    EOF: null
});

DJ.UI.Canvas.Layout.register('zone', DJ.UI.Canvas.ZoneLayout);

$dj.debug('Registered DJ.UI.AbstractCanvas.ZoneLayout');