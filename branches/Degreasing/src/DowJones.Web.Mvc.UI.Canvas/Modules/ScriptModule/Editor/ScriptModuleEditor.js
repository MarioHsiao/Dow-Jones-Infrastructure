/*!
* ScriptModuleEditor
*/

DJ.UI.ScriptModuleEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

    init: function (element, meta) {
        this._super(element, $.extend({ name: "ScriptModuleEditor" }, meta));
    },

    _initializeElements: function (el) {},

    buildProperties: function () {
        var props = {};

        var a = $(this.element).closest('form').serializeArray();
        
        $.each(a, function () {
            if (props[this.name]) {
                if (!props[this.name].push) {
                    props[this.name] = [props[this.name]];
                }
                props[this.name].push(this.value || '');
            } else {
                props[this.name] = this.value || '';
            }
        });
        
        return props;
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_ScriptModuleEditor', DJ.UI.ScriptModuleEditor);
