/*!
* HtmlModuleEditor
*/

DJ.UI.HtmlModuleEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

    init: function (element, meta) {
        this._super(element, $.extend({ name: "HtmlModuleEditor" }, meta));
    },

    _initializeElements: function (el) {},

    buildProperties: function () {
        return {
            html: $('.html', this.element).val(),
            script: $('.script', this.element).val()
        };
    },


    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_HtmlModuleEditor', DJ.UI.HtmlModuleEditor);
