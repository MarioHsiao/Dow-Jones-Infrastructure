/*!
* HtmlModuleEditor
*/

DJ.UI.HtmlModuleEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

    init: function (element, meta) {
        this._super(element, $.extend({ name: "HtmlModuleEditor" }, meta));
    },

    _initializeElements: function (el) {},

    buildProperties: function () {
        var props = $('textarea', this.element).serialize();
        props.id = this.get_moduleId();
        return {
            id: this.get_moduleId(),
            html: JSON.stringify($('.html', this.element).val()),
            script: JSON.stringify($('.script', this.element).val())
        };
    },

    saveProperties: function (props, callback) {
        $dj.debug('Updating module properties: ', props);

        var canvas = this.getCanvas();

        if (!canvas) return;

        var url = canvas.get_webServiceBaseUrl() + this.get_dataServiceUrl();
        
        $.ajax({
            url: url + '?pageId=' + canvas.get_canvasId(),
            type: props.id ? 'POST' : 'PUT',
            data: JSON.stringify(props),
            success: callback
        });
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_HtmlModuleEditor', DJ.UI.HtmlModuleEditor);
