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
        $dj.subscribe('data.BrowserStats', this._delegates.setData);
    },

    _setData: function (data) {
        var activeTab = 421139;

        var mappedData = this._mapData(data, activeTab);

        if (!this._intializedData) {
            this.barContainer.html(this.templates.statBars(mappedData));
        }
        else {
            this._updateData(mappedData);
        }

        if(data && data.length > 0) {
            this._intializedData = true;
        }
    },
    
    _mapData: function (data, groupToReturn) {
        if (!data || !data.length)
            return [];
        
        var self = this;
        
        var browserStatsByPages = _.groupBy(data, function (item) {
            return item.page_id;
        });

        var workingSet = browserStatsByPages[groupToReturn];

        var pageLoadTimings = 0;

        _.each(workingSet, function (item) {
            pageLoadTimings += parseInt(item.Avg, 10);
        });

        var avgPageLoadAcrossBrowsers = pageLoadTimings / workingSet.length;
        var maxPageLoad = 15000;
        var greenZone = 5000; //avgPageLoadAcrossBrowsers * 0.3; // 30%
        var neutralZone = 7000; //avgPageLoadAcrossBrowsers * 0.7; // 30% < x < 70%
        
        
        var browserData = _.map(workingSet, function (item) {
            var browser = self._parseBrowserInfo(item.Browser),
                avg = parseInt(item.Avg, 10),
                zone = avg < greenZone ? 'cool' : (avg <= neutralZone ? 'neutral' : 'hot');
            return {
                browser: browser.name.toLowerCase(),
                visitors: item.Count,
                timing: (avg / 1000).toFixed(2),
                percent: Math.min(avg / maxPageLoad * 100, 100),
                temperature: zone,
                browserVersion: browser.version
            };
        });

        return browserData;

    },
    
    _updateData: function (data) {
        var trafficBars = this.barContainer.find('.dj-trafficBar');
        for (var i = 0, len = data.length; i < len; i++) {
            var stat = data[i],
                trafficBar = $(trafficBars[i]),
                version = (stat.browser === 'msie' || (stat.browser === 'firefox' && stat.browserVersion < 4)) ? stat.browserVersion : '';
            trafficBar.find('.visitors').counter(stat.visitors);
            trafficBar.find('.bar').removeClass().addClass('bar').addClass(stat.temperature);
            trafficBar.find('.timing').text(stat.timing + 's');
            trafficBar.find('.gauge').animate({width: stat.percent + '%'}, 600);
            trafficBar.find('.browser i').removeClass().addClass('dj-logo-' + stat.browser + version);
            trafficBar.find('.browserVersion').text(version);
        }
    },

        
    _parseBrowserInfo: function (info) {
        // browser string looks like msie9, chrome, firefox3.6
        var items = info.split(/(\d+(\.\d)?)$/);
        
        return {
            name: items[0],
            version: items.length > 0 ? items[1] : ''
        };
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_BrowserStats', DJ.UI.BrowserStats);
