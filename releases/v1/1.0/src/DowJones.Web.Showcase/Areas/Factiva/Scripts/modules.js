$(function () {

    var modules = $('.module');
    $('.dj_module-refresh', modules).addClass('fi fi_refresh');
    $('.dj_module-info', modules).addClass('fi fi_info');
    $('.dj_module-edit', modules).addClass('fi fi_gear');
    $('.dj_module-options', modules).addClass('fi fi_gear');
    $('.dj_module-collapse', modules).addClass('fi fi_d-double-arrow');

    // TEMPORARY: Disable all the footer links - they're just for show
    $('#footer a').click(function (e) {
        $(this).effect("pulsate");
        e.stopPropagation();
        return false;
    });
});