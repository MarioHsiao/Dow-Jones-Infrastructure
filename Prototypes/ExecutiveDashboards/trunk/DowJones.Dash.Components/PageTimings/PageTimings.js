/*!
 * TopReferrer
 */

DJ.UI.PageTimings = DJ.UI.CompositeComponent.extend({

    defaults: {
        bands: {
            green: {
                max: 5,
                min: 0,
            },
            yellow: {
                max: 7,
                min: 5,
            },
            red: {
                max: 15,
                min: 7,
            }
        }
    },
    
    selectors: {
        timingsContainer: '.dj_pageTimings',
        timings: '.pageTimings .time-stamp',
        timestamp: '.curTime-stamp'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "PageTimings" }, meta));
        this._initPortalHeadlines();
    },

    _initPortalHeadlines: function () {
        var self = this;
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            updateTimings: $dj.delegate(this, this._updateTimings),
            updateSparklines: $dj.delegate(this, this._updateSparklines),
            getColor: $dj.delegate(this, this._getColor),
            getSingleColor: $dj.delegate(this, this._getSingleColor)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._timingsContainer = this.$element.find(this.selectors.timingsContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.PageTimings', this._delegates.updateTimings);
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
                    sLineComp._owner = null;
                    self.sparklineCharts[i] = null;
                }
            }
        }
        self.sparklineCharts = [];
    },

    _updateSparklines: function (data) {
        var self = this;
        
        var tData = data || this.tSparklineData;
        
        if (tData) {
            this.tSparklineData = tData;
            if (self.isPageTimingsListSeeded) {
                var subPages = _.filter(tData, function(point) {
                    return point.page_id === 421139;
                });

                var pubPages = _.filter(tData, function(point) {
                    return point.page_id === 421143;
                });

                var artPages = _.filter(tData, function(point) {
                    return point.page_id === 1940521;
                });

                if (!self.isSparklinesSeeded) {
                    var sparklineContainers = self.$element.find(".sparklineContainer");
                    self.sparklineCharts = [];

                    $.each(sparklineContainers, function(i, val) {
                        var tVals = (i == 0) ? _.pluck(subPages, "Avg") : (i == 1) ? _.pluck(pubPages, "Avg") : _.pluck(artPages, "Avg");
                        var vals = _.map(tVals, function(num) { return num / 1000; });
                        var objs = _.map(vals, function(num) { return { color: self._delegates.getColor(num), y: num, container: val }; });
                        var tMax = _.max(vals);
                        var tMin = 0;
                        DJ.add('Sparkline', {
                            container: val,
                            options: {
                                max: tMax,
                                min: tMin,
                                type: 1,
                                mouseover: function (evt) {
                                    var el = $(evt.target.container).parent('LI').find(self.selectors.timestamp);
                                    el.html(evt.target.y.toFixed(3));
                                },
                                mouseout: function(evt) {
                                    var el = $(evt.target.container).parent('LI').find(self.selectors.timestamp);
                                    el.html('&nbsp;');
                                }
                            },
                            data: {
                                values: objs
                            }
                        }).done(function(comp) {
                            comp.setOwner(self);
                            self.sparklineCharts.push(comp);
                        });
                    });
                    self.isSparklinesSeeded = true;
                    return;
                }

                sparklineContainers = self.$element.find(".sparklineContainer");
                $.each(self.sparklineCharts, function(i) {
                    var tVals = (i == 0) ? _.pluck(subPages, "Avg") : (i == 1) ? _.pluck(pubPages, "Avg") : _.pluck(artPages, "Avg");
                    var vals = _.map(tVals, function(num) { return num / 1000; });
                    var objs = _.map(vals, function(num) {
                        return { color: self._delegates.getColor(num), y: num, container: sparklineContainers[i] };
                    });
                    var tMax = _.max(vals);
                    var tMin = 0;
                    this.setExtremes(tMin, tMax);
                    this.setData({ values: objs });
                });
            }
        }
    },
    
    _getColor: function (num) {
        var self = this,
         o = self.options;
        var color = Highcharts.getOptions().colors[1];
        if (num <= o.bands.green.max) {
            return Highcharts.getOptions().colors[2];
        } else if (num > o.bands.yellow.min && num < o.bands.yellow.max) {
            return Highcharts.getOptions().colors[5];
        }
        return color;
    },
    
    _getSingleColor: function (num) {
        var self = this,
          o = self.options;
        var color = this._parseColor(Highcharts.getOptions().colors[1]);
        if (num <= o.bands.green.max) {
            return this._parseColor(Highcharts.getOptions().colors[2]);
        } else if (num > o.bands.yellow.min && num < o.bands.yellow.max) {
            return this._parseColor(Highcharts.getOptions().colors[5]);
        }
        return color;
    },
    
    _parseColor: function (color) {
        if ($.isPlainObject(color)) {
            return color.stops[0][1];
        }
        return color;
    },
    
    _updateTimings: function (data) {
        var self = this;
      
        if (!self.isPageTimingsListSeeded) {
            var pageTimings = [];
            for (var i = 0; i < data.length; i++) {
                var p = data[i].Avg / 1000;
                pageTimings.push({
                    title: data[i].page_name,
                    avg: Highcharts.numberFormat(p, 3) + "s",
                    color: self._delegates.getSingleColor(p),
                    width: 4
                });
            }

            self._timingsContainer.html(self.templates.success(pageTimings));
            self.isPageTimingsListSeeded = true;
            this._updateSparklines();
            return;
        }

        var temp = self.$element.find(self.selectors.timings);
        $.each(temp, function (j) {
            var $this = $(this);
            var n = data[j].Avg / 1000;
            $this.html(Highcharts.numberFormat(n, 3) + "s");
            $this.css({ borderBottom: "solid 4px " + self._delegates.getSingleColor(n) });
        });
    },
    
    EOF: null
});


// Declare this class as a jQuery plugin
$.plugin('dj_PageTimings', DJ.UI.PageTimings);
