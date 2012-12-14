/*!
 * CurrentSources Module
 */

    DJ.UI.CurrentSources = DJ.UI.CompositeComponent.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "CurrentSources" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        },
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CurrentSources', DJ.UI.CurrentSources);

    $dj.debug('Registered DJ.UI.CurrentSources as CurrentSources');
