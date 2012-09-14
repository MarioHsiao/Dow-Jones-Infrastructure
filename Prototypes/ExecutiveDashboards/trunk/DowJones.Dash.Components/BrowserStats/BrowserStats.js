/*!
 * BrowserStats
 */

DJ.UI.BrowserStats = DJ.UI.Component.extend({

    selectors: {
        shareChartContainer: '.shareChartContainer',
        barContainer: '.barContainer'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "BrowserStats" }, meta));

        if (this.data)
            this.setData(this.data);
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this.barContainer = this.$element.find(this.selectors.barContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.Browser', this._delegates.setData);
    },

    _setData: function (data) {
        
        var self = this;
        var browserData = _.map(data, function (item) {
            var browser = self._parseBrowserInfo(item.browser);
            var name = browser.name.toLowerCase().replace(' ', '').replace('-','');
            return {
                browser: name,
                visitors: item.count,
                timing: self._getStubTimings(name, browser.version),
                temperature: self._getStubTemperature(name, browser.version),
                browserVersion: browser.version
            };
        });

        if (!this._intializedData) {
            this.barContainer.html(this.templates.statBars(browserData));
        }
        else {
            this._updateData(browserData);
        }

        if(data && data.length > 0) {
            this._intializedData = true;
        }
    },
    
    _updateData: function (data) {
        var trafficBars = this.barContainer.find('.dj-trafficBar');
        for (var i = 0, len = data.length; i < len; i++) {
            var stat = data[i],
                trafficBar = $(trafficBars[i]),
                broswerLogo = stat.browser === 'internetexplorer' ? 'ie' + stat.browserVersion : stat.browser,
                version = (stat.browser === 'internetexplorer' || (stat.browser === 'firefox' && stat.browserVersion < 4)) ? stat.browserVersion : '';
            trafficBar.find('.visitors').counter(stat.visitors);
            trafficBar.find('.bar').removeClass().addClass('bar').addClass(stat.temperature);
            trafficBar.find('.timing').text(stat.timing + 'ms');
            trafficBar.find('.gauge').animate({width: Math.round((stat.timing * 100) / 210) + '%'}, 600);
            trafficBar.find('.browser i').removeClass().addClass('dj-logo-' + broswerLogo);
            trafficBar.find('.browserVersion').text(version);
        }
    },

        /* TODO: dummy data related functions. review their worthiness after requirements are finalized */
    _parseBrowserInfo: function (info) {
        var items = info.split(' ');
        var name = items[0];
        var version = items.length > 1 ? items[1] : '';
        
        if (items.length === 3) {
            name = items[0] + ' ' + items[1];
            version = items[2];
        }

        return {
            name: name,
            version: version
        };
    },

    _getStubTimings: function (browser, version) {
        var min = 10, max = 10;
        switch (browser.toLowerCase()) {
            case 'chrome':
            case 'safari':
                max = 30;
                break;
            case 'firefox':
            case 'opera':
                max = 70;
                break;
            case 'internetexplorer':
                max = version < 9 ? 150 : 80;
                break;
            default:
                max = 100;
        }

        return Math.floor(Math.random() * max) + min;
    },

    _getStubTemperature: function (browser, version) {
        switch (browser.toLowerCase()) {
            case 'chrome':
            case 'safari':
                return 'cool';
            case 'firefox':
            case 'opera':
                return 'neutral';
            case 'internetexplorer':
                return version < 9 ? 'hot' : 'neutral';
            default:
                return 'neutral';
        }
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_BrowserStats', DJ.UI.BrowserStats);
