/********** overlay plugin **************/

$.iDevices = {
    iPad: (navigator.userAgent.indexOf('iPad') !== -1)
};

$.fn.djOverlay = function (options) {
    if (!this.selector) { return; }

    options = $.extend({}, $.fn.djOverlay._defaults, (options || {}));

    if ($(this.selector).length > 1) {
        $dj.debug('Selector must result in a unique DOM Element.');
        return;
    }

    var overlayIds = $.fn.djOverlay._getIds(this.selector);
    options.overlayIds = overlayIds;

    if (options.background === true) {
        if ($('#' + overlayIds.background).length === 0) {
            $(document.body).append('<div id="' + overlayIds.background + '"></div>');
            $('#' + overlayIds.background).css({ 'position': 'absolute', 'top': '0px', 'left': '0px', 'display': 'none' });
        }
    }

    //load the overlay div element;
    if ($('#' + overlayIds.djOverlay).length === 0) {
        $(document.body).append('<div id="' + overlayIds.djOverlay + '"></div>');
        $('#' + overlayIds.djOverlay).append($(this.selector));
        $('#' + overlayIds.djOverlay).css({ 'position': ($.browser.msie && $.browser.version === 6) ? 'absolute' : 'fixed', 'display': 'none' });
    }
    $(this.selector).css({ "display": "block", "visibility": "visible" });

    $(this.selector).data("overlayoptions", options);

    $().djOverlay.show(this.selector);

    return this;
};

$.fn.djOverlay._defaults = {
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
$.fn.djOverlay._getIds = function (selector) {
    if (selector && $(selector).length > 0) {
        var ids = {};
        var selectorId = $(selector).attr("id");
        if (!selectorId || selectorId === "") {
            selectorId = selector.trim().replace(" ", "_").replace(".", "_");
        }

        ids.background = '__djBackground';
        ids.djOverlay = selectorId + '__djoverlay';
        return ids;
    }
};
$.fn.djOverlay._activeOverlays = [];
$.fn.djOverlay._position = function (selector) {
    var selectorJObj = $(selector);
    var options = selectorJObj.data("overlayoptions");

    $('#' + options.overlayIds.djOverlay).width("auto");
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

    $('#' + options.overlayIds.djOverlay).css(css);

    if (options.background && $('#' + options.overlayIds.background).length > 0) {
        $('#' + options.overlayIds.background).css({ 'height': ($(document).height() - 1), 'width': $(window).width() });
    }
};
$.fn.djOverlay.hide = function (selector, callback) {
    if (!selector) { return; }

    var options = $(selector).data("overlayoptions");
    if (!options || $('#' + options.overlayIds.djOverlay).length === 0) { return; }

    $('#' + options.overlayIds.djOverlay).fadeOut(options.fadeOutTime, function () {
        $('#' + options.overlayIds.djOverlay).css("display", "none");
        callback = callback || options.onHide;
        if (callback && $.isFunction(callback)) {
            callback.apply(this);
        }
    });

    $.fn.djOverlay._activeOverlays = $.grep($.fn.djOverlay._activeOverlays, function (val) { return val !== selector; });
    if ($.fn.djOverlay._activeOverlays.length > 0) {
        var prevOverlay = $.fn.djOverlay._activeOverlays[$.fn.djOverlay._activeOverlays.length - 1];
        $().djOverlay.show(prevOverlay, true);
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

        $(document).unbind("keyup.djOverlay");
        $(window).unbind("scroll.djOverlay");
        $(window).unbind("resize.djOverlay");
    }
};

$.fn.djOverlay.rePosition = function () {
    $(window).trigger("scroll.djOverlay");
};

$.fn.djOverlay.show = function (selector, retainBackground) {
    if (!selector) { return; }

    var options = $(selector).data("overlayoptions");

    if (options.background) {
        if ($.fn.djOverlay._activeOverlays.length > 0) {
            if (options.bgcolor !== 'transparent') {
                $('#' + options.overlayIds.background).css('z-index', ++$dj.maxZIndex);
            }
        }
        else {
            $('#' + options.overlayIds.background).css('z-index', ++$dj.maxZIndex);
        }
    }

    $('#' + options.overlayIds.djOverlay).css('z-index', ++$dj.maxZIndex);

    //Hide all the select dropdowns on the page except the current overlay container
    if (options.hideSelect && $.browser.msie && $.browser.version === 6) {
        $("select").css("visibility", "hidden");
        $('#' + options.overlayIds.djOverlay).find("select").css("visibility", "visible");
    }

    if (!retainBackground) {
        if (options.background) {
            if ($.fn.djOverlay._activeOverlays.length > 0) {
                if (options.bgcolor !== 'transparent') {
                    $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
                }
            }
            else {
                if (options.bgcolor !== 'transparent') {
                    $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
                }
                else {
                    $('#' + options.overlayIds.background).css({ 'background': '#fff', 'opacity': '0', 'display': 'block' });
                }
            }
        }

        $('#' + options.overlayIds.djOverlay).fadeIn(options.fadeInTime, options.onShow);
    }
    else {
        if (options.background) {
            if (options.bgcolor !== 'transparent') {
                $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
            }
            else {
                $('#' + options.overlayIds.background).css({ 'background': '#fff', 'opacity': '0', 'display': 'block' });
            }
        }
    }

    $.fn.djOverlay._position(selector);

    $(document).unbind("keyup.djOverlay");
    $(window).unbind("scroll.djOverlay").unbind("resize.djOverlay").bind("resize.djOverlay", function () {
        $.fn.djOverlay._position(selector);
    });
    if (options.autoScroll) {
        $(window).bind("scroll.djOverlay", function () {
            $.fn.djOverlay._position(selector);
        });
    }

    if (options.closeOnEsc) {
        $(document).bind("keyup.djOverlay", function (e) {
            if (e.keyCode === 27) {
                //To fix FF issue which fires this event when we close the fullscreen mode
                if (typeof $dj !== 'undefined' && $dj.videoPlayerInFullScreen) { return; }
                $().djOverlay.hide(selector);
            }
        });
    }

    if (!retainBackground) {
        $.fn.djOverlay._activeOverlays = $.grep($.fn.djOverlay._activeOverlays, function (val) { return val !== selector; });
        $.fn.djOverlay._activeOverlays.push(selector);
    }    

    if ($.fn.djOverlay._activeOverlays.length === 0 && !options.background && $('#' + options.overlayIds.background).length > 0) {
        $('#' + options.overlayIds.background).hide();
    }

    $('#' + options.overlayIds.djOverlay).focus();
};
