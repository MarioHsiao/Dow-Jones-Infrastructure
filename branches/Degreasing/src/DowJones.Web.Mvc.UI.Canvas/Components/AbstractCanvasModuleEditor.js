/// <reference name="jquery.js" assembly="DowJones.Web.Mvc" />
/// <reference name="common.js" assembly="DowJones.Web.Mvc" />

/*!  AbstractCanvasModuleEditor  */

DJ.UI.AbstractCanvasModuleEditor = DJ.UI.Component.extend({


    init: function (element, meta) {
        this._super(element, meta);

        this.$editor = $(".dj_Editor", this.$element);
    },


    resetEditArea: function () {
        this._debug('TODO: Implement resetEditArea!');
    },

    buildProperties: function () {
        this._debug('TODO: Implement buildProperties!');
    },

    saveProperties: function (props, callback) {
        this._debug('Updating module properties: ', props);

        var canvas = this.getCanvas();

        if (!canvas) return;

        var url = canvas.get_webServiceBaseUrl() + this.get_dataServiceUrl();
        var queryParams = { pageId: canvas.get_canvasId(), moduleId: this.get_moduleId() };

        $.ajax({
            url: url + '?' + $.param(queryParams),
            type: this.get_moduleId() ? 'PUT' : 'POST',
            data: JSON.stringify(props),
            success: callback,
            contentType: "application/json"
        });
    },

    getData: function () {
        this._debug('TODO: Implement getData!');
    },

    onShow: function () {
        this._debug('TODO: Implement onShow');
    },

    onShown: function () {
        this._debug('TODO: Implement onShown');
    },


    get_ProxyCredentials: function () {
        return DJ.config.credentials;
    },

    getSessionId: function () {
        return DJ.config.credentials.token;
    },

    getCanvas: function () {
        // if already found, do nothing
        if (this._canvas) return this._canvas;

        // else, try to find one
        this.$parent = this.getModule();

        // maybe outside of a module, try finding a canvas object on the page
        if (this.$parent) {
            this._canvas = this.$parent.get_Canvas();
        }
        else {
            var $canvas = $('.dj_Canvas');
            if (!$canvas || $canvas.length === 0) {
                throw ('Could not locate canvas object. Make sure you include at least one canvas object on the page.');
            }

            this._canvas = $canvas.findComponent(DJ.UI.AbstractCanvas);
        }

        return this._canvas;
    },

    getModule: function () {
        // if already found, do nothing
        if (this.$parent) return this.$parent;

        // else, try to find one
        var $parent = this.$element.parents('.dj_module').findComponent(DJ.UI.AbstractCanvasModule);

        if (!$parent || $parent.length === 0) {
            this._debug('Unable to locate parent module');
            return;
        }

        return $parent;
    },

    _publish: function (/* string */eventName, /* object */data) {
        this.getCanvas().publish(eventName, data);
    },

    _subscribe: function (/* string */eventName, /* function() */handler) {
        this.getCanvas().subscribe(eventName, handler);
    }
});

