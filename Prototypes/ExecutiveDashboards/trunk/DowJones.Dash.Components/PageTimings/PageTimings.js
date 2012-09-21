/*!
 * TopReferrer
 */

DJ.UI.PageTimings = DJ.UI.CompositeComponent.extend({

    selectors: {
        portalHeadlineListContainer: '.portalHeadlineListContainer',
        timings: '.pageTimings .time-stamp'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "PageTimings" }, meta));
        this._initPortalHeadlines();
    },

    _initPortalHeadlines: function () {
        var self = this;
        DJ.add('PortalHeadlineList', {
            container: this._portalHeadlinesContainer[0],
            options: { layout: 2, displayNoResultsToken: false },
            templates: { successTimeline: this.templates.headlineSuccess }
        }).done(function (comp) {
            self.portalHeadlines = comp;
            comp.owner = self;
        });
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            updateHeadlines: $dj.delegate(this, this._updateHeadlines),
            updateSparklines: $dj.delegate(this, this._updateSparklines)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._portalHeadlinesContainer = this.$element.find(this.selectors.portalHeadlineListContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.PageTimings', this._delegates.updateHeadlines);
        $dj.subscribe('data.PageLoadHistoricalDetails', this._delegates.updateSparklines);
    },
    
    _destroySparklines: function () {
        var self = this;
        if (self.sparklineCharts && self.sparklineCharts.length) {
            for (var i = 0, len = self.sparklineCharts.length; i < len; i++) {
                var sLineComp = self.sparklineCharts[i];
                if (sLineComp) {
                    if (sLineComp.chart) {
                        sLineComp.chart.destroy();
                    }
                    sLineComp.owner = null;
                    self.sparklineCharts[i] = null;
                }
            }
        }
        self.sparklineCharts = [];
    },

    _updateSparklines: function (data) {
        var self = this;
        if (!this.portalHeadlines) {
            $dj.error("PortalHeadlinesComponent is not initialized. Refresh the page to try again.");
            return;
        }

        this.tSparklineData = data;
        var tData = data || this.tSparklineData;
        this.tSparklineData = tData;

        if (tData) {
            var subPages = _.filter(tData, function (point) {
                                return point.page_id === 421139;
                            });
            
            var pubPages = _.filter(tData, function (point) {
                                return point.page_id === 421143;
                            });

            var artPages = _.filter(tData, function(point) {
                                return point.page_id === 1940521;
                            });
            
            if (!self.isSparklinesSeeded) {
                var sparklineContainers = self.$element.find(".sparklineContainer");
                self.sparklineCharts = [];

                $.each(sparklineContainers, function (i, val) {
                    var tVals = (i == 0) ? _.pluck(subPages, "Avg") : (i == 1) ? _.pluck(pubPages, "Avg") : _.pluck(artPages, "Avg");
                    var vals = _.map(tVals, function (num) { return num / 1000; });
                    var tMax = _.max(vals);
                    var tMin = _.min(vals);
                    DJ.add('Sparkline', {
                        container: val,
                        options: {
                            max: tMax,
                            min: tMin
                        },
                        data: {
                            values: vals
                        }
                    }).done(function (comp) {
                        comp.owner = self;
                        self.sparklineCharts.push(comp);
                    });
                });
                self.isSparklinesSeeded = true;
                return;
            }

            $.each(self.sparklineCharts, function(i) {
                var tVals = (i == 0) ? _.pluck(subPages, "Avg") : (i == 1) ? _.pluck(pubPages, "Avg") : _.pluck(artPages, "Avg");
                var vals = _.map(tVals, function (num) { return num / 1000; });
                var tMax = _.max(vals);
                var tMin = _.min(vals);
                this.setExtremes(tMin, tMax);
                this.setData({ values: vals });
            });
        }
    },
    
    _updateHeadlines: function (data) {
        var self = this;
        if (!this.portalHeadlines) {
            $dj.error("PortalHeadlinesComponent is not initialized. Refresh the page to try again.");
            return;
        }

        if (!self.isPageListSeeded) {
            var headlines = [];
            for (var i = 0; i < data.length; i++) {
                headlines[headlines.length] = { title: data[i].page_name, modificationTimeDescriptor: window.Highcharts.numberFormat(data[i].Avg/1000, 3) + "s" };
            }

            var result = {
                count: { value: headlines.length },
                headlines: headlines
            };
        
            self.portalHeadlines.setData({ resultSet: result });
            this._updateSparklines();
            self.isPageListSeeded = true;
            return;
        }

        var temp = self.$element.find(self.selectors.timings);
        $.each(temp, function (j) {
            $(this).html(window.Highcharts.numberFormat(data[j].Avg / 1000, 3) + "s");
        });
    },
    
    EOF: null
});


// Declare this class as a jQuery plugin
$.plugin('dj_PageTimings', DJ.UI.PageTimings);
