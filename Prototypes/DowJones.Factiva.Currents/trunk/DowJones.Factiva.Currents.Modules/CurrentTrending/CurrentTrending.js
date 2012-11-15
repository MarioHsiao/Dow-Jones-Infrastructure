/*!
 * TrendingCanvasModule
 */

    DJ.UI.CurrentTrending = DJ.UI.CompositeComponent.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "CurrentTrending" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        }

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CurrentTrending', DJ.UI.TrendingCanvasModule);

    $dj.debug('Registered DJ.UI.CurrentTrending as dj_CurrentTrending');
