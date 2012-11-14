/*!
 * CustomTopicCanvasModule
 */

    DJ.UI.CustomTopics = DJ.UI.CompositeComponent.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "CustomTopics" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        }

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CustomTopics', DJ.UI.CustomTopicCanvasModule);

    $dj.debug('Registered DJ.UI.CustomTopic as dj_CustomTopics');
