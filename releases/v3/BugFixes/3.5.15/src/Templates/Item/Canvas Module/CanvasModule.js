/*!
 * $safeitemrootname$
 */

    DJ.UI.$safeitemrootname$ = DJ.UI.AbstractCanvasModule.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "$safeitemrootname$" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code
        }

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_$safeitemrootname$', DJ.UI.$safeitemrootname$);

    $dj.debug('Registered DJ.UI.$safeitemrootname$ as dj_$safeitemrootname$');
