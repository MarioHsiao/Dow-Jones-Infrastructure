/*
 * Page-Garden Module functions
 * 
 * Author: jquery ui
 * Copyright 2011 Dow Jones & Company Inc.
 *
 */

$(function () {
    // Plugin Goodness Needed
    var contentWidth = 0;
    if ($('#selectModule').is(':visible')) {
        setModuleGarden();
    }

    // Adjust Content Width
    $("#divAddModule").live('click', function () {
        $("#divAddModule").hide();
        $('#selectModule').show();
        setModuleGarden();
    });

    function setModuleGarden() {
        $(".dj_module-garden").find('.dj_module-garden-item').each(function () {
            contentWidth += $(this).outerWidth(true);
        });
        $(".dj_module-garden").find('.dj_module-garden-content').width(contentWidth);
        $(".dj_module-garden").jScrollPane({ autoReinitialize: true });
    }
});


