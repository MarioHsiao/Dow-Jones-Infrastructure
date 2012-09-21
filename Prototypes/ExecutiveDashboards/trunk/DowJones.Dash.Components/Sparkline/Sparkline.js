/*!
 * Sparkline
 *
 */

DJ.UI.Sparkline = DJ.UI.Component.extend({

    // Default options
    defaults: {
        debug: false,
        cssClass: 'Sparkline',
        baselineSeriesColor: Highcharts.getOptions().colors[0],
        seriesColorForDecrease: Highcharts.getOptions().colors[1],
        seriesColorForIncrease: Highcharts.getOptions().colors[2]
    },

    eventNames: {
        pillClicked: 'pillClick.dj.Sparkline'
    },
    
    init: function (element, meta) {
        var $meta = $.extend({ name: "Sparkline" }, meta);

        // Call the base constructor
        this._super(element, $meta);
        this.renderChart();
    },

    _initializeDelegates: function () {
        $.extend(this._delegates, {
            OnPillClicked: $dj.delegate(this, this._onPillClicked)
        });
    },

    _initializeEventHandlers: function () {
        this.$element.click(this._delegates.OnPillClicked);
    },

    _initializeElements: function () {
    },

    _onPillClicked: function (evt) {
        var self = this;
    },

    initializeGraphOptions: function (chartContainer, seriesData, seriesColor) {

        var self = this,
            o = self.options;
        var sparklineChartConfig = {
            chart: {
                renderTo: 'container',
                defaultSeriesType: 'area',
                backgroundColor: 'transparent',
                margin: [0, 0, 0, 0]
                //borderWidth:1
            },
            title: {
                text: ''
            },
            credits: {
                enabled: false
            },
            xAxis: {
                labels: {
                    enabled: false
                }
            },
            yAxis: {
                endOnTick: false,
                startOnTick: false,
                max: Math.ceil(o.max + (o.max * .001)),
                min: Math.floor(o.min - (o.min * .001)),
                gridLineWidth: 0,
                labels: {
                    enabled: false
                }
            },
            legend: {
                enabled: false
            },
            tooltip: {
                enabled: false
            },
            plotOptions: {
                areaspline: {
                    color: Highcharts.getOptions().colors[0],
                    fillColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0, Highcharts.getOptions().colors[0]],
                            [1, 'rgba(255,255,255,0)']
                        ]
                    },
                    lineWidth: 1,
                    shadow: false,
                    states: {
                        hover: {
                            lineWidth: 1
                        }
                    },
                    marker: {
                        //enabled:false,
                        radius: 0,
                        states: {
                            hover: {
                                radius: 0,
                                enabled: false
                            }
                        }
                    }
                }
            },
            series: [{
                type: 'areaspline',
                id: 'sparkline',
                name: 'sparkline'
                
            }]
        };

        if (seriesData && seriesData.length > 0) {
            seriesData[0]["color"] = seriesColor;
        }

        return $.extend(true, { }, sparklineChartConfig, {
            chart: {
                renderTo: chartContainer,
                events: {
                    click: self._delegates.OnPillClicked
                }
            },
            series: seriesData
        });
    },

    mapData: function () {
        var self = this,
            dataSeries = [];

        if (self.data &&
            self.data.values &&
            self.data.values.length > 0) {
            $.each(self.data.values, function (i, objvalue) {
                dataSeries.push(objvalue);
            });
        }

        return [{
            // fillColor:( o.seriesFillColor || 'rgba(204,204,204,.25)'),
            data: dataSeries,
            // cursor: 'pointer', // removed by RE, fixes DJIMS Issue 40587
            point: {
                events: {
                    click: this._delegates.OnPillClicked
                }
            }
        }];
    },

    showSparklineControl: function (chartOptions) {
        try {
            this.chart = new Highcharts.Chart(chartOptions);
        }
        catch (e) {
            $dj.debug(e.message);
        }
    },

    setExtremes: function (min, max) {
        if (this.chart) {
            this.options.min = min;
            this.options.max = max;
            var axis = this.chart.yAxis[0];
            axis.setExtremes(this.options.min, this.options.max, false);
        }
    },
    
    setData: function (sparklineDataResult) {
        this.data = sparklineDataResult;
        this.renderChart();
    },

    renderChart: function () {
        var self = this,
            el = $(self.element),
            o = self.options;

        if (self.data) {
            self.$element.html(self.templates.layout({ data: self.data }));
            var chartContainer = el.find('.dj_sparkline-container'),
                graphDataModel = self.mapData(),
                chartOptions;

            if (graphDataModel.length > 0) {

                switch (self.data.status) {
                    case 1:
                        chartOptions = self.initializeGraphOptions(chartContainer[0], graphDataModel, o.seriesColorForIncrease);
                        el.addClass("increase");
                        break;
                    case 2:
                        chartOptions = self.initializeGraphOptions(chartContainer[0], graphDataModel, o.seriesColorForDecrease);
                        el.addClass("decrease");
                        break;
                    default:
                        chartOptions = self.initializeGraphOptions(chartContainer[0], graphDataModel, o.baselineSeriesColor);
                        break;
                }
                // bind events if necessary
                this.showSparklineControl(chartOptions);
            }
        }
    },

    dispose: function () {
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_Sparkline', DJ.UI.Sparkline);
