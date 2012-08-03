/*!
* EmbeddedContentEditor
*/

DJ.UI.EmbeddedContentEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

    init: function (element, meta) {
        this._super(element, $.extend({ name: "EmbeddedContentEditor" }, meta));
    },

    buildProperties: function () {
        return {
          width: $('.width', this.element).val(),
          height: $('.height', this.element).val(),
          url: $('.url', this.element).val()
        };
    },
    
    saveProperties: function (props, callback) {
        $dj.debug('Module properties updated: ', props);
        if(callback) callback(props);
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_EmbeddedContentEditor', DJ.UI.EmbeddedContentEditor);
