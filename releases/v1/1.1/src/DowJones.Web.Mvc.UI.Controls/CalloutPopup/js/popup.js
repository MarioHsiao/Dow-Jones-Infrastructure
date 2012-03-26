$.extend($dj, {

    selector: {
        ele: ''
    },

    callout: function (options) {
        if (this.length < 1) { return; }
        this.selector.ele = options.trig;
        if ($.browser.msie) {
            $(options.trig).popupBalloon('destroy');
        }
        $(options.trig).popupBalloon(
                {
                    height: (options.height) && options.height < 250 ? options.height : 250,
                    width: 380,
                    insetMargin: options.insetMargin,
                    popupClass: 'dj-loading',
                    content: '',
                    title: options.title,
                    jScrollPaneEnabled: false,
                    onHide: options.close,
                    onShow: options.show,
                    positionX: options.positionX,
                    positionY: options.positionY
                });

    },

    updateCalloutContent: function (option) {
        $(this.selector.ele).popupBalloon("option", "popupClass", "");
        $(this.selector.ele).popupBalloon("option", "content", option.body);
    },

    hideCallout: function () {
        $(this.selector.ele).popupBalloon("hide");
    },

    repositionCallout: function () {
        $(this.selector.ele).popupBalloon("rePosition");
    },

    getCalloutContainer: function () {
        return $(this.selector.ele).popupBalloon("getPopupBox");
    },

    isCalloutVisible: function () {
        return ($(this.selector.ele).popupBalloon("isVisible") == true);
    }
});
