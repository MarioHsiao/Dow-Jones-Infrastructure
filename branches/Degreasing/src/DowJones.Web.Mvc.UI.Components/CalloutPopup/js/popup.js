$.extend($dj, {

    selector: {
        ele: ''
    },

    callout: function (options) {

        if (this.length < 1) { return; }
        this.selector.ele = options.trig;

        $(this.selector.ele).popupBalloon('destroy').popupBalloon(
            $.extend({}, {
                popupClass: (options.popupClass + ' dj-loading'),
                onHide: options.close,
                onShow: options.show,
                jScrollPaneEnabled: false
            }, options)
        );
    },

    updateCalloutContent: function (option) {
        var $elem = $(this.selector.ele);
        $elem.popupBalloon("option", "popupClass", option.cssClass || "");
        $elem.popupBalloon("getpopbox").find('.content').html(option.body || '');
        if (!option.noAutoResizeHeight) {
            $elem.popupBalloon("option", "height", 'auto');
        }
    },

    hideCallout: function () {
        $(this.selector.ele).popupBalloon("hide");
    },

    repositionCallout: function () {
        $(this.selector.ele).popupBalloon("rePosition");
    },

    getCalloutContainer: function () {
        return $(this.selector.ele).popupBalloon("getpopbox");
    },

    isCalloutVisible: function () {
        return ($(this.selector.ele).popupBalloon("isVisible") == true);
    },

    setCalloutHeight: function (height) {
        $(this.selector.ele).popupBalloon("option", "height", height);
    },

    setCalloutWidth: function (width) {
        $(this.selector.ele).popupBalloon("option", "width", width);
    }
});