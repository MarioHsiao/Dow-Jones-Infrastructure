/*!
 * Newsstand Module
 */

    DJ.UI.CurrentNewsStand = DJ.UI.CompositeComponent.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "CurrentNewsStand" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        },
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CurrentNewsStand', DJ.UI.CurrentNewsStand);

    $dj.debug('Registered DJ.UI.CurrentNewsStand as dj_CurrentNewsStand');
