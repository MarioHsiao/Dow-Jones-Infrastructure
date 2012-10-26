/*!
 * ConcurrentVisitsModel
 */

DJ.UI.ConcurrentVisits = DJ.UI.CompositeComponent.extend({

    selectors: {
        visitorsGauge: '.visitorsGauge',
        currentVisitorsChart: '.currentVisitorsChart',
        timeCounter: '.dj_DashGaugeChartFooter .time'
    },
    
    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "ConcurrentVisits" }, meta));
        this._initComponent();
    },
    
    _initComponent: function () {
        var self = this;
        DJ.add('DashGauge', this._visitorGaugeConfig()).done(function (comp) {
            self.visitorsGauge = comp;
            comp.setOwner(self);
            comp.updateMax(91995);
        });

        // initialize the histogram
        self._renderHistogram(self._getChartObject());
    },
    
    _visitorGaugeConfig: function() {
        return {
            container: this._visitorsGauge[0],
            options: {
                max: 100,
                min: 0,
                angle: 70,
                footer: "0:0m",
                gaugeType: 0,
                height: 200,
                width: 200
            },
            templates: {
                max: this._maxTemplate.bind(this),
                min: this._minTemplate.bind(this),
                footer: this._footerTemplate.bind(this)
            },
            data: 0
        };
    },
    
    _areaSplineConfig: function (startDate) {
        var themeManager = DJ.UI.ThemeManager.instance;
        return {
            color: themeManager.colors.siteBackground,
            fillColor: {
                linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                stops: [
                    [0, themeManager.colors.ltBlue],
                    [1, Highcharts.Color(themeManager.colors.ltBlue).brighten(-0.3).get('rgb')] // darken
                ]
            },
            lineWidth: 1,
            marker: {
                enabled: false
            },
            shadow: false,
            states: {
                hover: {
                    enabled: false
                }
            },
            pointStart: startDate
        };
    },
    
    _splineConfig:function (startDate) {
        return {
            color: Highcharts.getOptions().colors[1],
            marker: {
                enabled: false
            },
            states: {
                hover: {
                    enabled: false
                }
            },
            pointStart: startDate
        };
    },
        
    _histogramConfig: function () {
        var now = new Date();
        var startDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate());
        var endDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), 24);
        return {
            chart: {
                height: 150,
                backgroundColor: 'transparent'
                
            },
            title: {
                text: null
            },
            subtitle: {
                text: null
            },
            xAxis: {
                id: 'day',
                type: 'datetime',
                title: {
                    text: null
                },
                labels: {
                    formatter: function () {
                        if (!this.isLast) {
                            return Highcharts.dateFormat('%H:%M', this.value);
                        }
                        return '24:00';
                    }
                },
                endOnTick: false,
                min: startDate,
                max: endDate
                
            },
            yAxis: {
                id: 'visitors',
                title: {
                    text: null
                },
                startOnTick: false,
                gridLineDashStyle: 'dot',
                gridLineWidth: 0,
                labels: { enabled: false}
            },
            tooltip: {
                shared: true
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                areaspline: this._areaSplineConfig(startDate),
                spline: this._splineConfig(startDate)
            },
    
            series: [{
                type: 'areaspline',
                name: '7-Days Ago',
                id: 'historical'
            },
            {
                type: 'spline',
                name: 'Today',
                id:'realtime'
            }],
            credits: false
        };
    },
    
    //Get historical chart Object
    _getChartObject: function (value) {
        return $.extend(true, {
            chart: { renderTo: $(this.selectors.currentVisitorsChart, this.$element)[0] }
        }, this._histogramConfig());
    },

    //Render Gauge
    _renderHistogram: function (chartObj) {
        this.histogram = new Highcharts.Chart(chartObj);
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
                updateQuickStats: $dj.delegate(this, this._updateQuickStats),
                updateDashboardStats: $dj.delegate(this, this._updateDashboardStats),
                updateHistoricalSeries: $dj.delegate(this, this._updateHistoricalSeries),
                updateRealtimeSeries: $dj.delegate(this, this._updateRealtimeSeries),
                domainChanged: $dj.delegate(this, this._domainChanged)
            });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._visitorsGauge = this.$element.find(this.selectors.visitorsGauge);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateQuickStats);
        $dj.subscribe('data.DashboardStats', this._delegates.updateDashboardStats);
        $dj.subscribe('data.HistorialTrafficSeries', this._delegates.updateRealtimeSeries);
        $dj.subscribe('data.HistorialTrafficSeriesWeekAgo', this._delegates.updateHistoricalSeries);
        $dj.subscribe('data.BasicHostConfiguration', this._delegates.domainChanged);
    },

    _domainChanged: function(data) {
        this.domain = data.domain;
        this.histogram.get('realtime').hide();
        this.histogram.get('historical').hide();
        delete this.lastVisits;
    },
    
    _updateRealtimeSeries: function (data) {
        var now = new Date();
        var startDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate());
        var endDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), 24);
        if (this.histogram && data) {
            for(var prop in data.data) {
                var obj = data.data[prop];
                if ($.isPlainObject(obj)) {
                    var realtimeSeries = this.histogram.get('realtime');
                    var frequency = data.data.frequency;
                    var tData = obj.series.people;
                    var axis = this.histogram.get('day');
                    axis.setExtremes(startDate, endDate, false);
                    if (realtimeSeries) {
                        realtimeSeries.pointInterval = frequency * 60 * 1000;
                        realtimeSeries.pointStart = startDate;
                        realtimeSeries.setData(tData, true, false);
                    }
                }
            }
        }
    },
    
    _updateHistoricalSeries: function (data) {
        var now = new Date();
        var startDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate());
        var endDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), 24);
        if (this.histogram && data) {
            for (var prop in data.data) {
                var obj = data.data[prop];
                if ($.isPlainObject(obj)) {
                    var historicalSeries = this.histogram.get('historical');
                    var frequency = data.data.frequency;
                    var tData = obj.series.people;
                    var axis = this.histogram.get('day');
                    axis.setExtremes(startDate, endDate, false);
                    if (historicalSeries) {
                        historicalSeries.pointInterval = frequency * 60 * 1000;
                        historicalSeries.pointStart = startDate;
                        historicalSeries.setData(tData, true, false);
                    }
                }
            }
        }
    },
    
    _updateQuickStats: function (data) {
        if (data) {
            if (this.visitorsGauge) {
                var visits = data.visits || 0;
                this.visitorsGauge.setData(visits);
                this.lastVisits = visits;
            }

            var engagedTime = data.engaged_time || {avg:0};
            var minutes = (engagedTime.avg / 60).toFixed(0);
            var seconds = (engagedTime.avg % 60).toFixed(0);
            this.$element.find(this.selectors.timeCounter).html(minutes + ":" + (seconds < 10 ? "0" + seconds : seconds) + "m");
        }
    },
    
    _updateDashboardStats: function (data) {
        if (data) {
            if (this.visitorsGauge) {
                if (this.lastVisits && this.lastVisits < data.people_min) {
                    this.visitorsGauge.updateMax(this.lastVisits < 100? 100 : this.lastVisits < 1000 ? 2000:  this.lastVisits < 5000 ? 5000 : 10000);
                    this.visitorsGauge.updateMin(0);
                }
                else {
                    this.visitorsGauge.updateMax(data.people_max);
                    this.visitorsGauge.updateMin(data.people_min);
                }
            }

            if (this.histogram) {
                var yAxis = this.histogram.get('visitors');
                if (yAxis) {
                    if (this.lastVisits && this.lastVisits < data.people_min) {
                        //yAxis.setExtremes(0, this.lastVisits < 100 ? 200 : this.lastVisits < 1000 ? 2000 : this.lastVisits < 5000 ? 5000 : 10000, false);
                        this.histogram.get('realtime').show();
                        this.histogram.get('historical').show();
                    }
                    else {
                        //yAxis.setExtremes(0, data.people_max, false);
                        this.histogram.get('realtime').show();
                        this.histogram.get('historical').show();
                    }
                }
            }
        }
    },
    
    _maxTemplate: function (val) {
        return "30-Day Max <span class=\"chartMax\">" + val + "</span>"; 
    },
    
    _minTemplate: function (val) {
        return "30-Day Min <span class=\"chartMin\">" + val + "</span>";
    },
    
    _footerTemplate: function (val) {
        return "<span class=\"label\">Engaged for <span class=\"time\">" + val + "</span> on average";
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_ConcurrentVisits', DJ.UI.ConcurrentVisits);