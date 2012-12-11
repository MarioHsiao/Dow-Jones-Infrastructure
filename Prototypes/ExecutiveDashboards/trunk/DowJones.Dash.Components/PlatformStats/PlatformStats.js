/*!
 * PlatformStatsModel
 */

DJ.UI.PlatformStats = DJ.UI.DashboardComponent.extend({

    selectors: {
        mobileContainer: 'div.platformContainer div.mobile span.val',
        desktopContainer: 'div.platformContainer div.desktop span.val',
        device: '.device',
        shareChartContainers: '.shareChartContainer',
        desktopShareChartContainer: '.shareChartContainer.desktop',
        mobileShareChartContainer: '.shareChartContainer.mobile',
        closeButton: '.actions-container .icon-remove',
        detailsWrapper: '.detailsWrapper'
    },

    events: {
        deviceClick: 'deviceClick.dj.PlatformStats'
    },

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "PlatformStats" }, meta));

        this._initBrowserShare();
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            updateStats: $dj.delegate(this, this._updateStats)
        });
    },

    _initializeElements: function (ctx) {
        this.$element.html(this.templates.container());
        this.shareChartContainers = ctx.find(this.selectors.shareChartContainers);
        this.desktopShareChartContainer = ctx.find(this.selectors.desktopShareChartContainer);
        this.mobileShareChartContainer = ctx.find(this.selectors.mobileShareChartContainer);
        this.closeButton = ctx.find(this.selectors.closeButton);
        this.detailsWrapper = ctx.find(this.selectors.detailsWrapper);
    },
    
    _initBrowserShare: function() {
        DJ.add("BrowserShare", {
            container: this.desktopShareChartContainer[0],
            options: {deviceType: 'desktop'}
        });
        DJ.add("BrowserShare", {
            container: this.mobileShareChartContainer[0],
            options: {deviceType: 'mobile'}
        });
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateStats);
        this.$element.find(".tip").tooltip();

        var self = this;

        this.$element.on('click', this.selectors.closeButton, function () {
            self.detailsWrapper.toggleClass('visible');
        });
        
        this.$element.on('click', this.selectors.device, function () {
            var deviceType = $(this).data('device-type');
            self.shareChartContainers.hide();
            self[deviceType + 'ShareChartContainer'].show();
            self.detailsWrapper.addClass('visible');
        });
    },

    _updateStats: function (data) {
        if (!data) {
            data = { platform: { m: 0, d: 0 } };
        }

        var platformStats = data.platform || { m: 0, d: 0 };
        var total = platformStats.d + platformStats.m || 100;
        
        var mPercentage = (platformStats.m * 100 / total).toFixed(2);
        var dPercentage = (platformStats.d * 100 / total).toFixed(2);

        $(this.selectors.desktopContainer, this.element).html(dPercentage + "%");
        $(this.selectors.mobileContainer, this.element).html(mPercentage + "%");
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_PlatformStats', DJ.UI.PlatformStats);