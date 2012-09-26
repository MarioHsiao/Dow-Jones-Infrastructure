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
            comp.owner = self;
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
                angle: 65,
                footer: "0:0m",
                gaugeType: 0,
                height: 200,
                width: 200
            },
            templates: {
                max: this._maxTemplate,
                min: this._minTemplate,
                footer: this._footerTemplate
            },
            data: 0
        };
    },
    
    _histogramConfig: function () {
        var now = new Date();
        var startDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate());
        var endDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), 24);
        return {
            chart: {
                spacingRight: 20,
                spacingLeft: 20,
                height: 150,
                backgroundColor: 'transparent',
                margin:[0,20,20,20]
            },
            title: {
                text: null
            },
            subtitle: {
                text: null
            },
            xAxis: {
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
                max: endDate,
                
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
                areaspline: {
                    color: Highcharts.getOptions().colors[0],
                    fillColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1},
                        stops: [
                            [0, "#4183C4"],
                            [1, Highcharts.Color("#CCC").brighten(-0.3).get('rgb')] // darken
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
                    pointInterval: 30 * 60 * 1000,
                    pointStart: startDate
                },
                spline: {
                    color: Highcharts.getOptions().colors[1],
                    marker: {
                        enabled: false
                    },
                    states: {
                        hover: {
                            enabled: false
                        }
                    },
                    pointInterval: 30 * 60 * 1000,
                    pointStart: startDate
                }
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
                updateStats: $dj.delegate(this, this._updateStats),
                updateDashboard: $dj.delegate(this, this._updateDashboard),
                updateHistoricalSeries: $dj.delegate(this, this._updateHistoricalSeries),
                updateRealtimeSeries: $dj.delegate(this, this._updateRealtimeSeries)
            });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._visitorsGauge = this.$element.find(this.selectors.visitorsGauge);
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.QuickStats', this._delegates.updateStats);
        $dj.subscribe('data.DashboardStats', this._delegates.updateDashboard);
        $dj.subscribe('data.HistorialTrafficSeries', this._delegates.updateRealtimeSeries);
        $dj.subscribe('data.HistorialTrafficSeriesWeekAgo', this._delegates.updateHistoricalSeries);
    },

    _updateRealtimeSeries: function (data) {
        if (this.histogram) {
            for(var prop in data.data) {
                var obj = data.data[prop];
                if ($.isPlainObject(obj)) {
                    var realtimeSeries = this.histogram.get('realtime');
                    var frequency = data.data.frequency;
                    this.histogram.options.plotOptions.spline.pointInterval = frequency * 60 * 1000;
                    var tData = obj.series.people;
                    if (realtimeSeries) {
                        realtimeSeries.setData(tData);
                    }
                }
            }
        }
    },
    
    _updateHistoricalSeries: function (data) {
        if (this.histogram) {
            for (var prop in data.data) {
                var obj = data.data[prop];
                if ($.isPlainObject(obj)) {
                    var historicalSeries = this.histogram.get('historical');
                    var frequency = data.data.frequency;
                    this.histogram.options.plotOptions.areaspline.pointInterval = frequency * 60 * 1000;
                    var tData = obj.series.people;
                    if (historicalSeries) {
                        historicalSeries.setData(tData);
                    }
                }
            }
        }
    },
    
    _updateStats: function (data) {
        if (this.visitorsGauge) {
            this.visitorsGauge.setData(data.visits);
        }

        var minutes = (data.engaged_time.avg / 60).toFixed(0);
        var seconds = (data.engaged_time.avg % 60).toFixed(0);
        this.$element.find(this.selectors.timeCounter).html( minutes + ":" + (seconds <10 ? "0" + seconds: seconds) + "m");
    },
    
    _updateDashboard: function (data) {
        if (this.visitorsGauge) {
            this.visitorsGauge.updateMax(data.people_max);
            this.visitorsGauge.updateMin(data.people_min);

            var yAxis = this.histogram.get('visitors');
            yAxis.setExtremes(data.people_min, data.people_max);
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