/*!
 * PlatformStatsModel
 */

DJ.UI.PlatformStats = DJ.UI.CompositeComponent.extend({

    selectors: {
        mobileContainer: 'div.platformContainer div.mobile span.val',
        desktopContainer: 'div.platformContainer div.desktop span.val'
    },
    
    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "PlatformStats" }, meta));
    },
    
    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
                updateStats: $dj.delegate(this, this._updateStats)
            });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateStats);
    },

    _updateStats: function (data) {
        var total = data.platform.d + data.platform.m;
        var mPercentage = (data.platform.m * 100 / total).toFixed(2);
        var dPercentage = (data.platform.d * 100 / total).toFixed(2);

        $(this.selectors.desktopContainer, this.element).html(dPercentage + "%");
        $(this.selectors.mobileContainer, this.element).html(mPercentage + "%");
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_PlatformStats', DJ.UI.PlatformStats);