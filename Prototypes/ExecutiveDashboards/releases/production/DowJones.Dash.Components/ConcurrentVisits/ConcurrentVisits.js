/*!
 * ConcurrentVisitsModel
 */

DJ.UI.ConcurrentVisits = DJ.UI.CompositeComponent.extend({

    selectors: {
        concurrentVisitsCounter: '.concurrentVisitsCounter',
        currentVisitorsChart: '.currentVisitorsChart',
        timeCounter: '.engagementArea .time'
    },
    
    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "ConcurrentVisits" }, meta));
        this._initComponent();
    },
    
    _initComponent: function () {
        var self = this;
        // initialize the histogram
        self._renderHistogram(self._getChartObject());
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
    
    _lineConfig: function (startDate) {

        return {
            marker: {
                enabled: false
            },
            lineWidth: 1,
            shadow: false,
            dashStyle: 'solid',
            states: {
                hover: {
                    enabled: false
                }
            },
            pointStart: startDate
        };
        
    },
        
    _histogramConfig: function () {
        var themeManager = DJ.UI.ThemeManager.instance;
        var now = new Date();
        var startDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate());
        var endDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), 24);
        return {
            chart: {
                height: 100,
                margin: [0, 20, 15, 20],
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
                minPadding: 0.01,
                maxPadding: 0.01,
                labels: { enabled: false}
            },
            tooltip: {
                crosshairs: {
                    width: 1,
                    color: '#000000',
                    dashStyle: 'shortdot',
                    zIndex: 15,
                },
                shared: true
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                areaspline: this._areaSplineConfig(startDate),
                line: this._lineConfig(startDate),
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
            },
            {
                type: 'line',
                name: '30-Day Max',
                color: Highcharts.Color(themeManager.colors.green).brighten(-0.3).get('rgb'),
                id: 'max'
            },
            {
                type: 'line',
                name: '30-Day Min',
                color: Highcharts.Color(themeManager.colors.yellow).brighten(-0.3).get('rgb'),
                id: 'min'
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
        this.concurrentVisitsCounter = this.$element.find(this.selectors.concurrentVisitsCounter);
        this.timeCounter = this.$element.find(this.selectors.timeCounter);
        this.concurrentVisitsCounter.counter(0);
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
        this.histogram.get('max').hide();
        this.histogram.get('min').hide();
        delete this.lastVisits;
    },
    
    _updateRealtimeSeries: function (data) {
        var self = this;
        var now = new Date();
        var startDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate());
        var endDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), 24);
        if (this.histogram && data) {
            for(var prop in data.data) {
                var obj = data.data[prop];
                if ($.isPlainObject(obj)) {
                    var realtimeSeries = this.histogram.get('realtime');
                    var frequency = data.data.frequency;
                    self.frequency = frequency;
                    var tData = obj.series.people;
                    var axis = this.histogram.get('day');
                    axis.setExtremes(startDate, endDate, false);
                    if (realtimeSeries) {
                        realtimeSeries.pointInterval = self.frequency * 60 * 1000;
                        realtimeSeries.pointStart = startDate;
                        realtimeSeries.setData(tData, true, false);
                    }
                }
            }
        }
    },
    
    _updateMaxSeries: function (value) {
        var self = this;
        var now = new Date();
        var startDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate());
        var endDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), 24);
        if (this.histogram && value) {
            var maxSeries = this.histogram.get('max');
            if (maxSeries && self.frequency) {
                maxSeries.pointInterval = self.frequency * 60 * 1000;
                maxSeries.pointStart = startDate;
                maxSeries.setData(self._fill((24 * 60)/self.frequency +1, value), true, false);
            }
        }
    },
    
    _updateMinSeries: function (value) {
        var self = this;
        var now = new Date();
        var startDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate());
        var endDate = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), 24);
        if (this.histogram && value) {
            var minSeries = this.histogram.get('min');
            if (minSeries && self.frequency) {
                minSeries.pointInterval = self.frequency * 60 * 1000;
                minSeries.pointStart = startDate;
                minSeries.setData(self._fill((24 * 60) / self.frequency+1, value), true, false);
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
        var self = this;
        
        if (data) {
            /*if (this.visitorsGauge) {
                var visits = data.visits || 0;
                this.visitorsGauge.setData(visits);
                
            }*/
            var visits = data.visits || 0;
            this.lastVisits = visits;
            //self.concurrentVisitsCounter.html(visits);
            self.concurrentVisitsCounter.counter(visits);
            var engagedTime = data.engaged_time || {avg:0};
            var minutes = (engagedTime.avg / 60).toFixed(0);
            var seconds = (engagedTime.avg % 60).toFixed(0);
            self.timeCounter.html(minutes + ":" + (seconds < 10 ? "0" + seconds : seconds) + "m");
        }
    },
    
    _updateDashboardStats: function (data) {
        var self = this;
        if (data) {
          
            if (self.histogram) {
                var yAxis = self.histogram.get('visitors');
                if (yAxis) {
                    if (self.lastVisits && self.lastVisits < data.people_min) {
                        yAxis.setExtremes(0, self.lastVisits < 100 ? 200 : self.lastVisits < 1000 ? 2000 : self.lastVisits < 5000 ? 5000 : 10000, false);
                        self.histogram.get('realtime').show();
                        self.histogram.get('historical').show();
                        self.histogram.get('min').hide();
                        self.histogram.get('max').hide();
                    }
                    else {
                        yAxis.setExtremes(0, data.people_max, false);
                        // update the max and min series
                        self._updateMaxSeries(data.people_max);
                        self._updateMinSeries(data.people_min);
                        self.histogram.get('max').hide();
                        // show chart series
                        self.histogram.get('realtime').show();
                        self.histogram.get('historical').show();
                        self.histogram.get('min').show();
                        self.histogram.get('max').show();
                    }
                }
            }
        }
    },
    
    _fill: function (arraySize, value) {
        var array = [];
        for (var i = 0; i < arraySize; i++) {

            array[i] = value;
        }
        return array;
    },
    
    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_ConcurrentVisits', DJ.UI.ConcurrentVisits);