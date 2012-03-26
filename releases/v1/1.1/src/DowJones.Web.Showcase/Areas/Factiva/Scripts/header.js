$(function () {

    $('#close-message-wrap').click(function () {
        $('.message').hide();
    });

    $('#header-toggle').click(function () {
        $('.fi_expand', this).toggle();
        $('.fi_collapse', this).toggle();

        $('#header-content').toggle('blind');
    });

    $('.search-field').watermark('watermark');

    $('#dashboard-options-panel .fi_close').click(closeDashboardOptionsPanel);

    $('#dashboard-nav .active-page .action')
        .add('#dashboard-nav .global-page .action')
        .append('<span class="config fi fi_gear" data-command="edit"></span>');

    $('#dashboard-nav .config').click(function () {
        if ($('#dashboard-options-panel').data('last-action') == this) {
            closeDashboardOptionsPanel();
            return false;
        }

        var command = $(this).data('command');
        var pageId = $(this).parents('li').data('page-id');
        $('#dashboard-options-container').html(buildDashboardOptionsContent(command, pageId))

        $('#dashboard-options-panel')
            .data('last-action', this)
            .show('blind');
    });

    function buildDashboardOptionsContent(command, pageId) {
        if (command == 'add-page')
            return $('<div />').html('[[ Enter information for new page ]]');
        else
            return $('<div />').html('Configuration for page "' + pageId + '"');
    }

    function closeDashboardOptionsPanel() {
        $('#dashboard-options-panel').data('last-action', null);
        $('#dashboard-options-panel').hide('blind');
    }

});