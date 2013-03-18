/*!
* $safeitemname$
*/

(function ($) {

    DJ.UI.$safeitemname$ = DJ.UI.AbstractCanvas.extend({

        init: function (element, meta) {
            this._super(element, meta);
        }
		
    });

    $.plugin('dj_$safeitemname$', DJ.UI.$safeitemname$);

    $dj.debug('Registered DJ.UI.$safeitemname$ as dj_$safeitemname$');

})(jQuery);
