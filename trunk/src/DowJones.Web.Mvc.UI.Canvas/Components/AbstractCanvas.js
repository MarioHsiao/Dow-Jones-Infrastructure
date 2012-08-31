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
        
        this.layout = DJ.UI.Canvas.Layout.initialize(this);

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