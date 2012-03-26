var moduleGarden = moduleGarden || {

    getCanvas: function () {
        return $('.dj_Canvas').findComponent(DJ.UI.AbstractCanvas);
    },

    addModuleDefinition: function (moduleId) {
        this.getCanvas().addModule(moduleId);
    },

    init: function () {
        var me = this;


        this.hideEditors();
        $('#moduleTypes').val(-1);


        $('#moduleTypes').get(0).onchange = function () {
            var val = $(this).val();
            if (val === -1) {
                me.hideEditor();
                return;
            }

            // hide the previous one
            if (me.$editor) { me.$editor.hide(); }

            var data = val.split('|');
            me.showEditor(data[0], data[1]);
        },


        $('#moduleGardenEditorActions').children('div.button-pane').find('a.dc_btn-save').click(function () {
            var props = me.currentEditorComp.buildProperties();

            if (me.currentEditorType !== 'Swap') {
                me.currentEditorComp.saveProperties(null, $dj.delegate(me, me.onEditorSave));
            }
            else {
                if (props && props.moduleIdToAdd) {
                    me.onEditorSave(props.moduleIdToAdd);
                }
                else {
                    $dj.debug('Module not selected');
                }
            }
            return false;
        });

        $('#moduleGardenEditorActions').children('div.button-pane').find('a.dc_btn-cancel').click(function () {
            me.hideEditor();
            return false;
        });

        // jQuery way fires the event twice. seems like the bug reappeared in jQuery 1.5
        //        $('#moduleTypes').change(function (e) {
        //            var data = $(this).attr('value').split('|');
        //            me.showEditor(data[0], data[1]);
        //            e.stopPropagation();
        //        });
    },


    onEditorSave: function (moduleId) {
        $dj.debug('Save Called: ', arguments);
        this.addModuleDefinition(moduleId);
        this.hideEditor();
    },

    hideEditor: function () {
        $('div.module-edit-options', $('#moduleGarden')).removeClass('module-edit-options-open');
    },

    hideEditors: function () {
        // hide it initially; should be rendered as such but until css catches up...
        $('div.dj_SwapModuleEditor', $('#moduleGarden')).hide();
        $('div.dj_SyndicationModuleEditor', $('#moduleGarden')).hide();

    },


    showEditor: function (editorType, moduleType) {
        var $editorEl = $('div.dj_' + editorType + 'ModuleEditor', $('#moduleGarden'));

        if (!$editorEl || $editorEl.length === 0) {
            $dj.debug('Editor not found: ', editorType);
            return;
        }

        var editorComp = $editorEl.findComponent(DJ.UI.AbstractCanvasModuleEditor);

        // dynamically set the module type since it varies from request to request

        //swapEditorComp.set_moduleType(moduleType);
        editorComp.options.moduleType = moduleType;
        // let it get data
        editorComp.onShow();

        this.currentEditorComp = editorComp;
        this.currentEditorType = editorType;
        this.$editor = $editorEl;

        // show edit area
        $editorEl.show();
        $('div.module-edit-options', $('#moduleGarden')).addClass('module-edit-options-open');
    }

};
