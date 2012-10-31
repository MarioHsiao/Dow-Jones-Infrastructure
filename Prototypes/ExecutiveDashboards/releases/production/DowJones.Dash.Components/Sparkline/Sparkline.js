/*!
 * Sparkline
 *
 */

DJ.UI.Sparkline = DJ.UI.Component.extend({

    // Default options
    defaults: {
        debug: false,
        cssClass: 'Sparkline',
        type: 0,
        baselineSeriesColor: Highcharts.getOptions().colors[0],
        seriesColorForDecrease: Highcharts.getOptions().colors[1],
        seriesColorForIncrease: Highcharts.getOptions().colors[2]
    },
    
    sparklineType: {
        line: 0,
        column: 1
    },

    eventNames: {
        clicked: 'click.dj.Sparkline',
        mouseover: 'mouseover.dj.Sparkline',
        mouseout: 'mouseout.dj.Sparkline'
    },
    
    init: function (element, meta) {
        var $meta = $.extend({ name: "Sparkline" }, meta);

        // Call the base constructor
        this._super(element, $meta);
        this.renderChart();
    },

    _initializeDelegates: function () {
        $.extend(this._delegates, {
            clicked: $dj.delegate(this, this._clicked),
            mouseover: $dj.delegate(this, this._mouseover),
            mouseout: $dj.delegate(this, this._mouseout)
        });
    },

    _initializeEventHandlers: function () {
    },

    _initializeElements: function () {
    },

    _clicked: function (evt) {
        var self = this,
            o = self.options;
        if (o.click && $.isFunction(o.click)) {
            o.click(evt);
            return;
        }
        self.publish(self.eventNames.clicked, evt);
    },
    
    _mouseover: function (evt) {
        var self = this,
            o = self.options;
        if (o.mouseover && $.isFunction(o.mouseover)) {
            o.mouseover(evt);
            return;
        }
        self.publish(self.eventNames.mouseover, evt);
    },
    
    _mouseout: function (evt) {
        var self = this,
            o = self.options;
        if (o.mouseout && $.isFunction(o.mouseout)) {
            o.mouseout(evt);
            return;
        }
        self.publish(self.eventNames.mouseout, evt);
    },

    initializeColumnGraphOptions: function (chartContainer, seriesData) {
        var self = this,
            o = self.options;

        var sparklineChartConfig = {
            chart: {
                renderTo: 'container',
                defaultSeriesType: 'column',
                backgroundColor: 'transparent',
                margin: [0, 0, 0, 0],
                
                borderRadius: 0
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
                max: Math.ceil(o.max + (o.max * .001)),
                min: Math.floor(o.min - (o.min * .001)),
                gridLineWidth: 0,
                startOnTick: false,
                endOnTick: false,
                labels: {
                    enabled: false
                }
            },
            legend: {
                enabled: false
            },
            tooltip: {
                useHTML: false,
                enabled: false
            },
            plotOptions: {
                series: {
                    cursor: 'pointer',
                    pointPadding: .1,
                    groupPadding: .1,
                    borderWidth: 0,
                    point: {
                            events: {
                                click: self._delegates.clicked,
                                mouseOver: self._delegates.mouseover,
                                mouseOut: self._delegates.mouseout
                        }
                    }
                }
            },
            series: [{
                shadow: false,
                type: 'column',
                id: 'sparkline',
                name: 'sparkline'
            }]
        };

        if (o.height) {
            $.extend(true, sparklineChartConfig, {
                chart: {
                    height: o.height
                }
            });
        }

        if (o.width) {
            $.extend(true, sparklineChartConfig, {
                chart: {
                    width: o.width
                }
            });
        }

        return $.extend(true, sparklineChartConfig, {
            chart: {
                renderTo: chartContainer
            },
            series: seriesData
        });
    },

    initializeHistogramGraphOptions: function (chartContainer, seriesData, seriesColor) {

        var self = this,
            o = self.options;
        var sparklineChartConfig = {
            chart: {
                renderTo: 'container',
                defaultSeriesType: 'areaspline',
                backgroundColor: 'transparent',
                margin: [0, 0, 0, 0]
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
        
        if (o.height) {
            $.extend(true, sparklineChartConfig, {
                chart: {
                    height: o.height
                }
            });
        }
        
        if (o.width) {
            $.extend(true, sparklineChartConfig, {
                chart: {
                    width: o.width
                }
            });
        }

        return $.extend(true, sparklineChartConfig, {
            chart: {
                renderTo: chartContainer
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
            data: dataSeries
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
                chartOptions = null,
                graphDataModel;

            if (self.data &&
                self.data.values &&
                self.data.values.length > 0)  {
                switch(o.type) {
                    case self.sparklineType.line:
                        graphDataModel = self.mapData();
                        chartOptions = self.initializeHistogramGraphOptions(chartContainer[0], graphDataModel, o.baselineSeriesColor);
                        break;
                    case self.sparklineType.column:
                        graphDataModel = self.mapData();
                        chartOptions = self.initializeColumnGraphOptions(chartContainer[0], graphDataModel);
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
