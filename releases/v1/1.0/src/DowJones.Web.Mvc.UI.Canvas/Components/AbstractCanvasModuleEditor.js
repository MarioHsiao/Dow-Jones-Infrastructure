/// <reference name="jquery.js" assembly="DowJones.Web.Mvc" />
/// <reference name="common.js" assembly="DowJones.Web.Mvc" />

/*!  AbstractCanvasModuleEditor  */

//(function ($) {

DJ.UI.AbstractCanvasModuleEditor = DJ.UI.Component.extend({


    init: function (element, meta) {
        this._super(element, meta);

        this.$editor = $(".dj_Editor", this.$element);

    },


    resetEditArea: function () {
        throw 'Implement Edit Area reset!';
    },

    buildProperties: function () {
        throw 'Implement buildProperties()!';
    },

    saveProperties: function (props, callback) {
        throw 'Implement saveProperties()!';
    },

    getData: function () {
        throw 'Implement getData()!';
    },

    onShow: function () {
        throw 'Implement onShow()!';
    },


    get_ProxyCredentials: function () {
        if (!this.getCanvas())
            return null;

        return this._canvas._proxyCredentials;
    },

    getSessionId: function () {
        return this.getCanvas().get_ControlData().SessionID;
    },

    getCanvas: function () {
        // if already found, do nothing
        if (this._canvas) return this._canvas;

        // else, try to find one
        this.$parent = this.getModule();

        // maybe outside of a module, try finding a canvas object on the page
        if (!this.$parent) {
            var $canvas = $('.dj_Canvas');
            if (!$canvas || $canvas.length === 0) {
                throw ('Could not locate canvas object. Make sure you include at least one canvas object on the page.');
            }

            this._canvas = $canvas.findComponent(DJ.UI.AbstractCanvas);
        }
        else {
            this._canvas = this.$parent._canvas;
        }

        return this._canvas;
    },

    getModule: function () {
        // if already found, do nothing
        if (this.$parent) return this.$parent;

        // else, try to find one
        var $parent = this.$element.parents('.dj_module').findComponent(DJ.UI.AbstractCanvasModule);

        if (!$parent || $parent.length === 0) {
            $dj.debug('Unable to locate parent module');
            return;
        }

        return $parent;
    },

    _publish: function (/* string */eventName, /* object */data) {
        this.getCanvas().publish(eventName, this, data);
    },


    _subscribe: function (/* string */eventName, /* function() */handler) {
        this.getCanvas().subscribe(eventName, handler);
    }



});

