/*!
* Factiva Canvas
*/

(function ($) {

    DJ.UI.ModuleGardenCanvas = DJ.UI.AbstractCanvas.extend({

        init: function (element, meta) {
            this._super(element, meta);
        },

        EOF: null

    });
    $.plugin('dj_moduleGardenCanvas', DJ.UI.ModuleGardenCanvas);

    $dj.debug('Registered DJ.UI.ModuleGardenCanvas as dj_moduleGardenCanvas');

})(jQuery);