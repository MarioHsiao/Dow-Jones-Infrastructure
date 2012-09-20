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
            options: { layout: 2 },
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
        data = data || this.tSparklineData;
        
        if (!self.isSparklinesSeeded) {
            var sparklineContainers = self.$element.find(".sparklineContainer");
            self.sparklineCharts = [];

            $.each(sparklineContainers, function () {
                DJ.add('Sparkline', {
                    container: this,
                    data: {
                        values: [100, 102, 405, 602, 53]
                    }
                }).done(function (comp) {
                    var myArray = [];
                    var arrayMax = 40;
                    var limit = arrayMax + 1;
                    for (var i = 0; i < arrayMax; i++) {
                        myArray.push(Math.floor(Math.random() * limit));
                    }
                    comp.owner = self;
                    comp.setData({ values: myArray });
                    self.sparklineCharts.push(comp);
                });
            });
            self.isSparklinesSeeded = true;
            return;
        }

        $.each(self.sparklineCharts, function (j) {
            var myArray = [];
            var arrayMax = 40;
            var limit = arrayMax + 1;
            for (var i = 0; i < arrayMax; i++) {
                myArray.push(Math.floor(Math.random() * limit));
                this.setData({ values: myArray });
            }
        });
        
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
