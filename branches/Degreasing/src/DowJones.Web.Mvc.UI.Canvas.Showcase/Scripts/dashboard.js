$(function () {

    function showModuleSelector() {
        $('#module-selector').show().overlay();
    }
    function hideModuleSelector() {
        $().overlay.hide('#module-selector');
    }

    $('#add-module').click(function() {
        showModuleSelector();
    });

    $('#module-selector .module').click(function () {
        $('#module-selector .module').removeClass('selected');
        $(this).addClass('selected');
        var editor = $(this).data('editor');
        $('#new-module-info .editor').load(DJ.Dashboards.EditorsUrl + editor, function () {
            $('#new-module-info').show();
        });
    });


    $('#new-module-info .actions .cancel').click(hideModuleSelector);

    $('#new-module-info .actions .create').click(function() {
        var editor = $('#new-module-info .editor .dj_Editor').findComponent(DJ.UI.AbstractCanvasModuleEditor);
        editor.saveProperties(editor.buildProperties(), function (data) {
            if (!data.moduleId) return;

            editor.getCanvas().loadModule(data.moduleId, function () {
                hideModuleSelector();
            });
        });
    });
    

})