$(function () {
    

    $('#add-module').click(function() {
        $('#module-selector').toggle();
    });

    $('#module-selector .module').click(function () {
        var editor = $(this).data('editor');
        $('#new-module-info .editor').load(DJ.Dashboards.EditorsUrl + editor, function () {
            $('#new-module-info').show();
        });
    });


    $('#new-module-info .actions .cancel').click(function () {
        $('#new-module-info').hide();
    });

    $('#new-module-info .actions .create').click(function() {
        var editor = $('#new-module-info .editor .dj_Editor').findComponent(DJ.UI.AbstractCanvasModuleEditor);
        editor.saveProperties(editor.buildProperties(), function (data) {
            if (!data.moduleId) return;

            editor.getCanvas().loadModule(data.moduleId, function () {
                $('#new-module-info').hide();
            });
        });
    });
    

})