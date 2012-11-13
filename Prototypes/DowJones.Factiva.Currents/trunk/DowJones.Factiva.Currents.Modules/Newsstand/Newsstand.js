/*!
 * Newsstand Module
 */

    DJ.UI.Newsstand = DJ.UI.CompositeComponent.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "Newsstand" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        }

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_Newsstand', DJ.UI.NewsstandCanvasModule);

    $dj.debug('Registered DJ.UI.Newsstand as Newsstand');
