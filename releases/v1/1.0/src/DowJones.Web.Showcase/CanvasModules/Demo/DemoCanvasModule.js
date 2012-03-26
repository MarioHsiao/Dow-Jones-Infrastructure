/*!
*   Demo Canvas Module Script
*/

(function ($) {

    DJ.UI.DemoCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        data: {
            timestamp: null,
            globalTimestamp: null
        },

        defaults: {
            cssClass: 'demo-module'
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "DemoCanvasModule" }, meta);
            this._super(element, meta);

            this._myLastUpdateTimestampContainer = $('.last-updated', this._moduleBody).get(0);
            this._globalLastUpdateTimestampContainer = $('.global-last-updated', this._moduleBody).get(0);
            this._updateButton = $('.update-button', this._moduleBody).get(0);
            this._clearButton = $('.clear-button', this._moduleBody).get(0);

            this._subscribe("DemoModule.GlobalTimestampUpdated", '_globalTimestampUpdatedHandler');

            $(this._updateButton).click($dj.delegate(this, this.getData));
            $(this._clearButton).click($dj.delegate(this, this.clearTimestamps));

            this._debug('Demo Module initialized');
        },

        clearTimestamps: function () {
            $('span', this._moduleBody).empty();
        },

        _getData: function () {
            this.data = {};
            $.get(this.options.ServiceUrl, $dj.delegate(this, this._getDataCallback));
        },

        _getDataCallback: function (timestamp) {
            this._debug('getData request returned');

            this.data.timestamp = timestamp;

            $(this._myLastUpdateTimestampContainer).effect('highlight');

            this._publish("DemoModule.GlobalTimestampUpdated", timestamp);

            this.paint();
        },

        _globalTimestampUpdatedHandler: function (sender, args) {
            this._debug('_globalTimestampUpdatedHandler: recieved message from ' + (sender || {}).toString() + ' (' + args + ')');

            this.data.globalTimestamp = args;

            $(this._globalLastUpdateTimestampContainer).effect('highlight');

            this.paint();
        },

        _paint: function () {
            $(this._myLastUpdateTimestampContainer).html(this.data.timestamp);
            $(this._globalLastUpdateTimestampContainer).html(this.data.globalTimestamp);
        }
    });

    $.plugin('dj_DemoCanvasModule', DJ.UI.DemoCanvasModule);

})(jQuery);