/*!
*   Dow Jones Composite Component
*/

(function ($) {

    DJ.UI.CompositeComponent = DJ.UI.Component.extend({

        _pubSubManager: null,

        init: function (element, meta) {
            var $meta = $.extend({ name: "CompositeComponent" }, meta);

            this._pubSubManager = new DJ.Platform.PubSubManager();

            this._super(element, $meta);
        },

        dispose: function () {
            this._super();

            this._refreshTrigger = null;

            this.$contentArea = null;
            this.$editArea = null;
            this._footerArea = null;
            this._messageArea = null;
        },

        hasData: function () {
            return this.data !== null && this.data.length;
        },

        getData: function () {
            this.showLoadingArea();
            this._super();
        },

        _innerPublish: function (/* string */eventName, /* object */args) {
            this._pubSubManager.publish(eventName, args);
        },

        subscribe: function (/* string */eventName, /* function() */handler) {
            return this._pubSubManager.subscribe(eventName, handler);
        },

        showLoadingArea: function () {
            this.$element.showLoading();
        },

        hideLoadingArea: function () {
            this.$element.hideLoading();
        },

        EOF: {}

    });


    $.plugin('dj_Composite', DJ.UI.CompositeComponent);

    $dj.debug('Registered DJ.UI.CompositeComponent as dj_Composite');

} (jQuery));