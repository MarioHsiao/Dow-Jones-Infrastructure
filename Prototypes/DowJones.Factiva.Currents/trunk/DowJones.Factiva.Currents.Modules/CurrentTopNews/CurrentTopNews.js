/*!
 * TopNewsCanvasModule
 */

    DJ.UI.CurrentTopNews = DJ.UI.CompositeComponent.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "CurrentTopNews" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        }

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CurrentTopNews', DJ.UI.TopNewsModule);

    $dj.debug('Registered DJ.UI.CurrentTopNews as dj_CurrentTopNews');
