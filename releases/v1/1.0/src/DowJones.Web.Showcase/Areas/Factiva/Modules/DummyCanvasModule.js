(function ($) {

    DJ.UI.DummyCanvasModule = DJ.UI.AbstractCanvasModule.extend({

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_DummyCanvasModule', DJ.UI.DummyCanvasModule);

})(jQuery);