/*!
 * ShareChart
 */

    DJ.UI.ShareChart = DJ.UI.Component.extend({

        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "ShareChart" }, meta));

            // TODO: Add custom initialization code
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                // TODO: Add delegates
                // e.g.  OnHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
            });
        },

        _initializeElements: function () {
            // TODO: Get references to child elements
            // e.g.  this._headlines = this.$element.find('.clear-filters');
        },

        _initializeEventHandlers: function () {
            // TODO:  Wire up events to delegates
            // e.g.  this._headlines.click(this._delegates.OnHeadlineClick);
        },


        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_ShareChart', DJ.UI.ShareChart);
