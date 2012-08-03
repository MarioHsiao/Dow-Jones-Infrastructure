/*!
* EmbeddedContentCanvasModule
*/

DJ.UI.EmbeddedContentCanvasModule = DJ.UI.AbstractCanvasModule.extend({

    init: function (element, meta) {
        var $meta = $.extend({ name: "EmbeddedContentCanvasModule" }, meta);
        this._super(element, $meta);

        this.showContentArea();
    }

});


// Declare this class as a jQuery plugin
$.plugin('dj_EmbeddedContentCanvasModule', DJ.UI.EmbeddedContentCanvasModule);

$dj.debug('Registered DJ.UI.EmbeddedContentCanvasModule as dj_EmbeddedContentCanvasModule');
