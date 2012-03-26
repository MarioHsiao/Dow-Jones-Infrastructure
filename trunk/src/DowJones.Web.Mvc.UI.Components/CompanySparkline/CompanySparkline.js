/*!
 * CompanySparkline
 *
 */

    DJ.UI.CompanySparkline = DJ.UI.Component.extend({

        // Default options
        defaults: {
            debug: false,
            cssClass: 'CompanySparkline'
        },

        eventNames: {
            pillClicked: 'pillClick.dj.CompanySparkline'
        },

        // Localization/Templating tokens
        tokens: {
            // name: value     // add more defaults he0re separated by comma
        },


        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "CompanySparkline" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this.renderChart();

            // TODO: Add custom initialization code like the following:
            // this._testButton = $('.testButton', element).get(0);
        },

        _initializeDelegates: function () {
            $.extend(this._delegates, {
                OnPillClicked: $dj.delegate(this, this._onPillClicked)
            });
        },

        _initializeEventHandlers: function() {
            this.$element.click(this._delegates.OnPillClicked);
        },

        _initializeElements: function() {            
        },

        _onPillClicked: function (evt) {
             var self = this,
                o = self.options,
                el = $(self.element);

             if (self.data && 
                 self.data.closePrices &&
                 self.data.closePrices.length > 0) {
                    $dj.debug(self.eventNames.pillClicked + " Event clicked");
                    self.publish(self.eventNames.pillClicked, { "code": self.data.code });
            }
        },

        initializeGraphOptions: function (chartContainer, seriesData, max, min, seriesColor) {

            var self = this,
                o = self.options,
                el = $(self.element);

            var sparklineChartConfig = {
                chart: {
                    renderTo: 'container',
                    backgroundColor: 'none',
                    defaultSeriesType: 'line',
                    margin: [0, 0, 0, 0],
                    height: o.height || 14
                },
                title: {
                    text: ''
                },
                exporting: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },
                xAxis: {
                    minPadding: 0.05,
                    maxPadding: 0.05,
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    endOnTick: false,
                    startOnTick: false,
                    max: Math.ceil(max + (max * .01)),
                    min: Math.floor(min - (min * .01)),
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
                    series: {
                        lineWidth: 1,
                        shadow: false,
                        states: {
                            hover: {
                                lineWidth: 2
                            }
                        },
                        marker: {
                            enabled:false,
                            radius: 0,
                            states: {
                                hover: {
                                    radius: 0
                                }
                            }
                        }
                    }
                }
            };

            if (seriesData && seriesData.length > 0) {
                seriesData[0]["color"] = seriesColor;
            }

            return $.extend(true, {}, sparklineChartConfig, {
                chart: {
                    renderTo: chartContainer,
                    events: {
                        click: self._delegates.OnPillClicked
                    }
                },
                series: seriesData
            })
        },

        transformDataResult: function () {
            var self = this,
                el = $(self.element),
                o = self.options,
                dataSeries = [];

            if (self.data &&
                self.data.closePrices &&
                self.data.closePrices.length > 0) {
                $.each(self.data.closePrices, function (i, objvalue) {
                    dataSeries.push(objvalue.value);
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

        setData: function (sparklineDataResult) {
            this.data = sparklineDataResult;
            this.renderChart();
        },

        renderChart: function () {
            var self = this,
                el = $(self.element),
                o = self.options;

            if (self.data) {
                self.$element.html(self.templates.pillcontents({ data: self.data }));
                var chartContainer = el.find('.dj_sparkline-container'),
                    graphDataModel = self.transformDataResult(),
                    chartOptions;

                if (graphDataModel.length > 0) {

                    switch (self.data.status) {
                        case 1:
                            chartOptions = self.initializeGraphOptions(chartContainer[0], graphDataModel, self.data.max.value, self.data.min.value, o.seriesColorForIncrease);
                            el.addClass("increase");
                            break;
                        case 2:
                            chartOptions = self.initializeGraphOptions(chartContainer[0], graphDataModel, self.data.max.value, self.data.min.value, o.seriesColorForDecrease);
                            el.addClass("decrease");                            
                            break;
                        default:
                            chartOptions = self.initializeGraphOptions(chartContainer[0], graphDataModel, self.data.max.value, self.data.min.value, o.baselineSeriesColor);
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
    $.plugin('dj_CompanySparkline', DJ.UI.CompanySparkline);
