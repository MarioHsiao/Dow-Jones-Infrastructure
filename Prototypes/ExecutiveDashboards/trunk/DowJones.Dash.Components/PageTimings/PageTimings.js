/*!
 * TopReferrer
 */

DJ.UI.PageTimings = DJ.UI.CompositeComponent.extend({

    defaults: {
        zones: {
            cool: {
                to: 5,
                from: 0,
            },
            neutral: {
                to: 7,
                from: 5,
            },
            hot: {
                to: 100,
                from: 7,
            }
        }
    },
    
    selectors: {
        timingsContainer: '.dj_pageTimings .content',
        noDataContainer: '.noData',
        contentContainer: '.content',
        avg: '.pageTimings .avg-stamp .value',
        min: '.pageTimings .min-stamp .value',
        max: '.pageTimings .max-stamp .value',
        sparklineTooltip: '.sparklineTooltip'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "PageTimings" }, meta));
        this._initPortalHeadlines();
        this._showContent();
    },

    _initPortalHeadlines: function () {
        var self = this;
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            updateTimings: $dj.delegate(this, this._updateTimings),
            updateSparklines: $dj.delegate(this, this._updateSparklines),
            getColor: $dj.delegate(this, this._getColor),
            getSingleColor: $dj.delegate(this, this._getSingleColor),
            domainChanged: $dj.delegate(this, this._domainChanged)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._timingsContainer = this.$element.find(this.selectors.timingsContainer);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.PageTimings', this._delegates.updateTimings);
        $dj.subscribe('data.PageLoadHistoricalDetails', this._delegates.updateSparklines);
        $dj.subscribe('data.BasicHostConfiguration', this._delegates.domainChanged);
    },
    
    _domainChanged: function (data) {
        var self = this;
        self.domain = data.domain;
        self._destroySparklines();
        self._timingsContainer.html("");
        self.isPageTimingsListSeeded = false;
        self.isSparklinesSeeded = false;
        this._mapZones(data.performanceZones);
    },
    
    _mapZones: function (zones) {
        var self = this,
            cZones = self.options.zones;
        
        cZones.cool = _.find(zones, function(item) {
            return item.zoneType.toLowerCase() == 'cool';
        });
        
        cZones.neutral = _.find(zones, function (item) {
            return item.zoneType.toLowerCase() == 'neutral';
        });
        
        cZones.hot = _.find(zones, function (item) {
            return item.zoneType.toLowerCase() == 'hot';
        });
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
        if (data) {
            if (this.tSparklineData && data) {

                if (_.isEqual(this.tSparklineData, data)) {
                    return;
                }
            }
        }

        var tData = data || this.tSparklineData;

        if (tData && tData.length && tData.length > 0) {
            this.tSparklineData = tData;
            
            if (self.isPageTimingsListSeeded) {
                
                var statsByPages = _.groupBy(tData, function (item) {
                    return item.page_id;
                });

                var subPages = [];
                for (var prop in statsByPages) {
                    subPages.push(statsByPages[prop]);
                }

                if (!self.isSparklinesSeeded) {
                    var sparklineContainers = self.$element.find(".sparklineContainer");
                    self.sparklineCharts = [];

                    $.each(sparklineContainers, function (i, val) {
                        var vals = _.chain(subPages[i])
                                    .pluck("Avg")
                                    .map(function (num) { return num / 1000; })
                                    .value();
                        var objs = _.map(vals, function(num) {
                             return { color: self._delegates.getColor(num), y: num, container: val };
                        });
                        var tMax = _.max(vals);
                        var tMin = 0;
                        DJ.add('Sparkline', {
                            container: val,
                            options: {
                                max: tMax,
                                min: tMin,
                                height: 20,
                                width: 71,
                                type: 1,
                                mouseover: function (evt) {
                                    var el = $(evt.target.container).parent('LI').find(self.selectors.sparklineTooltip);
                                    el.html(evt.target.y.toFixed(2) + "s");
                                },
                                mouseout: function(evt) {
                                    var el = $(evt.target.container).parent('LI').find(self.selectors.sparklineTooltip);
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
                    var vals = _.chain(subPages[i])
                                    .pluck("Avg")
                                    .map(function (num) { return num / 1000; })
                                    .value();
                    
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
        
        return num <= o.zones.cool.to ? Highcharts.getOptions().colors[2] :
                                        (num <= o.zones.neutral.to ? Highcharts.getOptions().colors[5] :
                                                                     Highcharts.getOptions().colors[1]);
    },
    
    _getSingleColor: function (num) {
        var self = this,
            o = self.options;
        
        return num <= o.zones.cool.to ? this._parseColor(Highcharts.getOptions().colors[2]) :
                                        (num <= o.zones.neutral.to ? this._parseColor(Highcharts.getOptions().colors[5]) :
                                                                     this._parseColor(Highcharts.getOptions().colors[1]));
    },
    
    _parseColor: function (color) {
        if ($.isPlainObject(color)) {
            return color.stops[1][1];
        }
        return color;
    },
    
    _showComingSoon: function () {
        this.$element.find(this.selectors.contentContainer).hide('fast');
        this.$element.find(this.selectors.noDataContainer).show('fast');
    },

    _showContent: function () {
        this.$element.find(this.selectors.contentContainer).show('fast');
        this.$element.find(this.selectors.noDataContainer).hide('fast');
    },
    
    _updateTimings: function (data) {
        if (!data || !data.length) {
            this._showComingSoon();
            return;
        }
        
        var self = this;
        self._showContent();

        if (!self.isPageTimingsListSeeded) {
            var pageTimings = [];
            for (var i = 0; i < data.length; i++) {
                var d = data[i];
                pageTimings.push({
                    title: d.page_name.replace("Mobile","").replace("Germany", ""),
                    width: 4
                });
            }

            self._timingsContainer.html(self.templates.success(pageTimings));
            self.isPageTimingsListSeeded = true;
            
        }

        var temp = self.$element.find(self.selectors.avg);
        $.each(temp, function (j) {
            var $this = $(this);
            var n = data[j];
            $this.html(Highcharts.numberFormat(n.Avg / 1000, 2) + "s");
            var color = self._delegates.getSingleColor(n.Avg / 1000);
            $this.css({ borderBottom: "solid 1px " + color, color: color });
        });
        
        temp = self.$element.find(self.selectors.max);
        $.each(temp, function (j) {
            var $this = $(this);
            var n = data[j];
            $this.html(Highcharts.numberFormat(n.Max / 1000, 2) + "s");
            var color = self._delegates.getSingleColor(n.Max / 1000);
            $this.css({ borderBottom: "solid 1px " + color, color: color });

        });
        
        temp = self.$element.find(self.selectors.min);
        $.each(temp, function (j) {
            var $this = $(this);
            var n = data[j];
            $this.html(Highcharts.numberFormat(n.Min / 1000, 2) + "s");
            var color = self._delegates.getSingleColor(n.Min / 1000);
            $this.css({ borderBottom: "solid 1px " + color,  color: color });

        });
        if (!self.isSparklinesSeeded)
            this._updateSparklines();
    },
    
    EOF: null
});


// Declare this class as a jQuery plugin
$.plugin('dj_PageTimings', DJ.UI.PageTimings);
