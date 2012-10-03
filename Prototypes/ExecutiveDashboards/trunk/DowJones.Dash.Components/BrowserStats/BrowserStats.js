/*!
 * BrowserStats
 */

DJ.UI.BrowserStats = DJ.UI.CompositeComponent.extend({

    selectors: {
        shareChartContainer: '.shareChartContainer',
        barContainer: '.barContainer',
        pillContainer: '.pillContainer',
        noDataContainer: '.noData',
        contentContainer: '.content',
        pill: '.dj-pills > li'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "BrowserStats" }, meta));
        this._showContent();

        if (this.data) {
            this.setData(this.data);
        }
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function (ctx) {
        ctx.html(this.templates.container());
        this.barContainer = ctx.find(this.selectors.barContainer);
        this.pillContainer = ctx.find(this.selectors.pillContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.BrowserStats', this._delegates.setData);

        var self = this;
        this.$element.on('click', this.selectors.pill, function () {
            var el = $(this);
            el.siblings('.active').add(el).toggleClass('active');
            self.activePillId = el.data('id');
            if (self.browserStats)
                self._updateData(self._getBarData(self.browserStats[self.activePillId]));
        });
    },

    _setData: function (data) {
        if (!data || !data.length) {
            if (data != null) {
                this._showComingSoon();
            }
            return;
        }

        this._showContent();
        // get some sensible structure from a flat result set
        var mappedData = this._mapData(data);

        // draw the pills
        this.setPills(mappedData.pills);

        this.browserStats = mappedData.browserStatsByPages;
        var statBarData = this._getBarData(mappedData.browserStatsByPages[this.activePillId]);

        if (!this._intializedData) {
            this.barContainer.html(this.templates.statBars(statBarData));
        }
        else {
            this._updateData(statBarData);
        }

        if (data && data.length > 0) {
            this._intializedData = true;
        }
    },
    
    _getBarData: function (workingSet) {
        if (!workingSet)
            return;

        var self = this;
        
        var pageLoadTimings = 0,
            numVisitors = 0;

        _.each(workingSet, function (item) {
            pageLoadTimings += parseInt(item.Avg, 10);
        });

        _.each(workingSet, function (item) {
            numVisitors += parseInt(item.Count, 10);
        });

        var maxPageLoad = 15000;
        var greenZone = 5000; //avgPageLoadAcrossBrowsers * 0.3; // 30%
        var neutralZone = 7000; //avgPageLoadAcrossBrowsers * 0.7; // 30% < x < 70%


        var browserData = _.map(workingSet, function (item) {
            var browser = self._parseBrowserInfo(item.Browser),
                avg = parseInt(item.Avg, 10),
                zone = avg < greenZone ? 'cool' : (avg <= neutralZone ? 'neutral' : 'hot');
            return {
                browser: browser.name.toLowerCase(),
                visitors: ((item.Count * 100) / numVisitors).toFixed(0),
                timing: (avg / 1000).toFixed(2),
                percent: Math.min(avg / maxPageLoad * 100, 100),
                temperature: zone,
                browserVersion: browser.version
            };
        });

        return browserData;
    },

    _mapData: function (data) {
        if (!data || !data.length)
            return [];

        var browserStatsByPages = _.groupBy(data, function (item) {
            return item.page_id;
        });

        var pills = _.map(browserStatsByPages, function (item) {
            return { id: item[0].page_id, name: item[0].page_name };
        });

        return {
            pills: pills,
            browserStatsByPages: browserStatsByPages
        };

    },

    _updateData: function (data) {
        if (data && data.length) {
            var trafficBars = this.barContainer.find('.dj-trafficBar');
            for (var i = 0, len = data.length; i < len; i++) {
                var stat = data[i],
                    trafficBar = $(trafficBars[i]),
                    version = (stat.browser === 'msie' || (stat.browser === 'firefox' && stat.browserVersion < 4)) ? stat.browserVersion : '';
                trafficBar.find('.visitors').counter(stat.visitors);
                trafficBar.find('.bar').removeClass().addClass('bar').addClass(stat.temperature);
                trafficBar.find('.timing').text(stat.timing + 's');
                trafficBar.find('.gauge').animate({ width: stat.percent + '%' }, 600);
                trafficBar.find('.browser i').removeClass().addClass('dj-logo-' + stat.browser + version);
                trafficBar.find('.browserVersion').text(version);
            }
        }
    },


    _parseBrowserInfo: function (info) {
        // browser string looks like msie9, chrome, firefox3.6
        var items = info.split(/(\d+(\.\d)?)$/);

        return {
            name: items[0],
            version: items.length > 0 ? items[1] : ''
        };
    },
    
    _showComingSoon: function () {
        this.$element.find(this.selectors.contentContainer).hide('fast');
        this.$element.find(this.selectors.noDataContainer).show('fast');
    },
    
    _showContent: function () {
        this.$element.find(this.selectors.contentContainer).show('fast');
        this.$element.find(this.selectors.noDataContainer).hide('fast');
    },

    setPills: function (pills) {
        this.activePillId = this.activePillId || pills[0].id;
        
        if (pills.length <= 1) {
            return;
        }

        // set the active item in the dataset before drawing pills
        for (var i = 0; i < pills.length; i++) {
            pills[i].active = this.activePillId === pills[i].id;
        }

        this.pillContainer.html(this.templates.navPills(pills));
    },
});


// Declare this class as a jQuery plugin
$.plugin('dj_BrowserStats', DJ.UI.BrowserStats);
