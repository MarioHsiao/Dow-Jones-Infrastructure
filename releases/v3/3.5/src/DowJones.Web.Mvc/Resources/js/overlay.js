/********** overlay plugin **************/

$.iDevices = {
    iPad: (navigator.userAgent.indexOf('iPad') !== -1)
};

$.fn.overlay = function (options) {
    if (!this.selector) { return; }

    options = $.extend({}, $.fn.overlay._defaults, (options || {}));

    if ($(this.selector).length > 1) {
        $dj.debug('Selector must result in a unique DOM Element.');
        return;
    }

    var overlayIds = $.fn.overlay._getIds(this.selector);
    options.overlayIds = overlayIds;

    if (options.background === true) {
        if (options.bgcolor == 'transparent') {
            if ($('#' + overlayIds.tBackground).length === 0) {
                $(document.body).append($('<div id="' + overlayIds.tBackground + '"></div>').css({ 'background': '#fff', 'opacity': '0', 'position': 'absolute', 'top': '0px', 'left': '0px', 'display': 'none' }));
            }
        }
        else if ($('#' + overlayIds.background).length === 0) {
            $(document.body).append('<div id="' + overlayIds.background + '"></div>');
            $('#' + overlayIds.background).css({ 'position': 'absolute', 'top': '0px', 'left': '0px', 'display': 'none' });
        }
    }

    //load the overlay div element;
    if ($('#' + overlayIds.overlay).length === 0) {
        $(document.body).append('<div id="' + overlayIds.overlay + '"></div>');
        $('#' + overlayIds.overlay).append($(this.selector));
        $('#' + overlayIds.overlay).css({ 'position': ($.browser.msie && $.browser.version === 6) ? 'absolute' : 'fixed', 'display': 'none' });
    }
    $(this.selector).css({ "display": "block", "visibility": "visible" });

    $(this.selector).data("overlayoptions", options);

    $().overlay.show(this.selector);

    return this;
};

$.fn.overlay._defaults = {
    background: true,
    bgcolor: '#000000',
    closeOnEsc: false,
    fadeInTime: 500,
    fadeOutTime: 200,
    hideSelect: true,
    onShow: null,
    onHide: null,
    autoScroll: true
};
$.fn.overlay._getIds = function (selector) {
    if (selector && $(selector).length > 0) {
        var ids = {};
        var selectorId = $(selector).attr("id");
        if (!selectorId || selectorId === "") {
            selectorId = selector.trim().replace(" ", "_").replace(".", "_");
        }

        ids.background = '__djBackground';
        ids.tBackground = '__djTBackground';
        ids.overlay = selectorId + '__djoverlay';
        return ids;
    }
};
$.fn.overlay._activeOverlays = [];
$.fn.overlay._position = function (selector) {
    var selectorJObj = $(selector);
    var options = selectorJObj.data("overlayoptions");

    $('#' + options.overlayIds.overlay).width("auto");
    selectorJObj.show();
    var intHeight = selectorJObj.outerHeight(true);
    var intWidth = selectorJObj.outerWidth(true);

    var t, l, css;
    var position = (($.browser.msie && $.browser.version === 6) || $.iDevices.iPad) ? 'absolute' : 'fixed';

    if (!intHeight) {
        intHeight = selectorJObj.css("height");
    }

    if (!intWidth) {
        intWidth = selectorJObj.css("width");
    }

    if (typeof (intHeight) === 'string') {
        intHeight = intHeight.replace("px", "");
    }

    if (typeof (intWidth) === 'string') {
        intWidth = intWidth.replace("px", "");
    }

    t = ($(window).height() - intHeight) / 2;
    l = (($(window).width() - intWidth) / 2);

    if (($.browser.msie && $.browser.version === 6) || $.iDevices.iPad) {
        t = t + ($.iDevices.iPad ? window.pageYOffset : $(document).scrollTop());
    }

    css = { 'height': intHeight, 'width': intWidth, 'top': t, 'left': l, 'position': position };

    $('#' + options.overlayIds.overlay).css(css);

    if (options.background) {
        $('#' + options.overlayIds[options.bgcolor == 'transparent' ? "tBackground" : "background"]).css({ 'height': ($(document).height() - 1), 'width': $(window).width() });
    }
};
$.fn.overlay.hide = function (selector, callback) {
    if (!selector) { return; }

    var options = $(selector).data("overlayoptions");
    if (!options || $('#' + options.overlayIds.overlay).length === 0) { return; }

    $('#' + options.overlayIds.overlay).fadeOut(options.fadeOutTime, function () {
        $('#' + options.overlayIds.overlay).css("display", "none");
        callback = callback || options.onHide;
        if (callback && $.isFunction(callback)) {
            callback.apply(this);
        }
    });

    $.fn.overlay._activeOverlays = $.grep($.fn.overlay._activeOverlays, function (val) { return val !== selector; });
    if (options.bgcolor == 'transparent') {
        $('#' + options.overlayIds.tBackground).hide();
    }
    if ($.fn.overlay._activeOverlays.length > 0) {
        var prevOverlay = $.fn.overlay._activeOverlays[$.fn.overlay._activeOverlays.length - 1];
        $().overlay.show(prevOverlay, true);
    }
    else {
        if ($('#' + options.overlayIds.background).length > 0) {
            $('#' + options.overlayIds.background).fadeOut(options.fadeOutTime, function () {
                //Show all the hidden select dropdowns
                if (options.hideSelect && $.browser.msie && $.browser.version === 6) {
                    $("select").css("visibility", "visible");
                }
            });
        }

        $(document).unbind("keyup.overlay");
        $(window).unbind("scroll.overlay");
        $(window).unbind("resize.overlay");
    }
};

$.fn.overlay.rePosition = function () {
    $(window).trigger("scroll.overlay");
};

$.fn.overlay.show = function (selector, retainBackground) {
    if (!selector) { return; }

    var options = $(selector).data("overlayoptions");

    if (options.background) {
        $('#' + options.overlayIds[options.bgcolor == 'transparent' ? "tBackground" : "background"]).css('z-index', ++$dj.maxZIndex);
    }

    $('#' + options.overlayIds.overlay).css('z-index', ++$dj.maxZIndex);

    //Hide all the select dropdowns on the page except the current overlay container
    if (options.hideSelect && $.browser.msie && $.browser.version === 6) {
        $("select").css("visibility", "hidden");
        $('#' + options.overlayIds.overlay).find("select").css("visibility", "visible");
    }

    if (options.background) {
        if (options.bgcolor == 'transparent') {
            $('#' + options.overlayIds.tBackground).show();
        }
        else {
            $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
        }
    }
    if (!retainBackground) {
        $('#' + options.overlayIds.overlay).fadeIn(options.fadeInTime, options.onShow);
    }

    $.fn.overlay._position(selector);

    $(document).unbind("keyup.overlay");
    $(window).unbind("scroll.overlay").unbind("resize.overlay").bind("resize.overlay", function () {
        $.fn.overlay._position(selector);
    });
    if (options.autoScroll) {
        $(window).bind("scroll.overlay", function () {
            $.fn.overlay._position(selector);
        });
    }

    if (options.closeOnEsc) {
        $(document).bind("keyup.overlay", function (e) {
            if (e.keyCode === 27) {
                //To fix FF issue which fires this event when we close the fullscreen mode
                if (typeof $dj !== 'undefined' && $dj.videoPlayerInFullScreen) { return; }
                if (selector == "#navbar_modalNoHeaderDialog" && $("#flipboardInstructionsContainer").length > 0 && $("#flipboardInstructionsContainer").is(":visible")) {
                    if ($("#closeFlipboardSplashLink").length > 0) {
                        $("#closeFlipboardSplashLink").click();
                    }
                } else {
                    $().overlay.hide(selector);
                }
            }
        });
    }

    if (!retainBackground) {
        $.fn.overlay._activeOverlays = $.grep($.fn.overlay._activeOverlays, function (val) { return val !== selector; });
        $.fn.overlay._activeOverlays.push(selector);
    }

    if ($.fn.overlay._activeOverlays.length === 0 && !options.background && $('#' + options.overlayIds.background).length > 0) {
        $('#' + options.overlayIds[options.bgcolor == 'transparent' ? "tBackground" : "background"]).hide();
    }

    $('#' + options.overlayIds.overlay).focus();
};
