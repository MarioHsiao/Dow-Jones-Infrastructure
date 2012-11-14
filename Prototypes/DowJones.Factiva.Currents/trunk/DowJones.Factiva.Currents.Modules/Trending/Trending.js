/*!
 * TrendingCanvasModule
 */

    DJ.UI.TrendingCanvasModule = DJ.UI.CompositeComponent.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "Trending" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        }

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_Trending', DJ.UI.TrendingCanvasModule);

    $dj.debug('Registered DJ.UI.TrendingCanvasModule as dj_Trending');
