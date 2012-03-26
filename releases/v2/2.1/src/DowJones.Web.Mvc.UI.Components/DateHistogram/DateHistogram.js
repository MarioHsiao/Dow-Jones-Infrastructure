/*!
 * DateHistogram
 *
 */

(function ($) {

    DJ.UI.DateHistogram = DJ.UI.Component.extend({

        /*
        * Properties
        */

        // Default options
        defaults: {
            debug: false,
            cssClass: 'DateHistogramControl'
            // ,name: value     // add more defaults here separated by comma
        },

        // Localization/Templating tokens
        tokens: {
            // name: value     // add more defaults here separated by comma
        },

        eventNames: {
            barClicked: 'barClicked.dj.DateHistogram'
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "DateHistogram" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            this.renderChart();
        },

        _initializeDelegates: function () {
            $.extend(this._delegates, {
                OnBarClicked: $dj.delegate(this, this._onBarClicked)
            });
        },

        _initializeEventHandlers: function () {
            this.$element.click(this._delegates.OnPillClicked);
        },

        _initializeElements: function () {
        },

        _onBarClicked: function () {
        },

        initializeGraphOptions: function (chartContainer, seriesData) {
            var self = this,
                o = self.options,
                el = $(self.element);

            var hasSVG = Modernizr.svg;
            var histogramChartConfig = {
                chart: {
                    defaultSeriesType: 'column',
                    height: o.height || 60,
                    backgroundColor: 'none',
                    spacingTop: 12,
                    spacingRight: 10,
                    spacingBottom: 0,
                    spacingLeft: 12,
                    marginLeft: 16,
                    marginBottom: 10,
                    plotBorderColor: '#e6e6e6',
                    plotBorderWidth: 0
                },

                colors: [(o.barColor || '#b2e2f3')],

                tooltip: {
                    formatter: function() {
                        if (this.point.packet.currentDate) {
                            return "<b><%= Token("currentDate") %>:</b> " +  this.point.packet.currentDateText  + "<br/>" +
                                   "<b><%= Token("hitCount") %>:</b> "    +  this.point.packet.hitCount.displayText.value;
                        }
                        else {
                            return "<b><%= Token("startDate") %>:</b> " +  this.point.packet.startDateText  + "<br/>" +
                                   "<b><%= Token("endDate") %>:</b> "   +  this.point.packet.endDateText  + "<br/>" + 
                                   "<b><%= Token("hitCount") %>:</b> "  +  this.point.packet.hitCount.displayText.value;

                        }                        
                    },
                    style: {
                            color: '#333333',
                            fontSize: '10px',
                            fontFamily: 'arial, helvetica, sans-serif' // default font
                    },
                    enabled: false
                },

                exporting: {
                    enabled: false
                },

                credits: false,

                title: {
                    text: null
                },

                subtitle: {
                    text: null
                },

                legend: {
                    enabled: false
                },

                xAxis: {
                    type: 'datetime',
                    dateTimeLabelFormats: { // don't display the dummy year
                        month: '%e. %b',
                        year: '%b'
                    },
                    showFirstLabel: true,
                    tickWidth: 0,
                    labels: {
                        enabled: false
                    }
                },

                yAxis: {
                    title: {
                        text: null
                    },
                    tickWidth: 1,
                    tickPixelInterval: (o.height || 60) / 4,
                    endOnTick: true,
                    gridLineColor: '#fff',
                    maxPadding: 0.01,
                    max: self.data.max.value,
                    opposite: true,
                    labels: {
                        style: {
                            color: '#999999',
                            fontSize: '11px',
                            fontFamily: 'arial, helvetica, clean, sans-serif'
                        }
                    }
                },

                plotOptions: {
                    column: {
                        pointPadding: 0.1,
                        groupPadding: 0,
                        borderWidth: 0,
                        minPointLength: 1,
                        shadow: false,
                        cursor: 'pointer',
                        point: {
                            events: {
                                mouseOver: function(event) {
                                    var $chartContainer = $(event.target.series.chart.container).parent(),
                                        hChart = event.target.series,
							            chartOffset = $chartContainer.offset(),
							            calloutX = parseInt( chartOffset.left +  event.target.plotX + (event.target.barW/2 ) ),
							            calloutY = parseInt( chartOffset.top + event.target.series.chart.chartHeight - event.target.barH - 9 );
                                    // todo: update the callout type                
                                    var calloutContent;
                                    if (event.target.packet.currentDate) {
                                        calloutContent =  "<b><%= Token("currentDate") %>:</b> " +  event.target.packet.currentDateText  + "<br/>" +
                                                          "<b><%= Token("hitCount") %>:</b> "    +  event.target.packet.hitCount.displayText.value;
                                    }
                                    else {
                                        calloutContent = "<b><%= Token("startDate") %>:</b> " +  event.target.packet.startDateText  + "<br/>" +
                                                          "<b><%= Token("endDate") %>:</b> "   +  event.target.packet.endDateText  + "<br/>" + 
                                                          "<b><%= Token("hitCount") %>:</b> "  +  event.target.packet.hitCount.displayText.value;

                                    }  

                                    hChart._currentHover = event.target.x;
                                    if( $chartContainer.data("popbox") ) {
							            if( $chartContainer.data('chartTimeout') ) {
								            clearTimeout( $chartContainer.data('chartTimeout') );
								            $chartContainer.data('chartTimeout', '');
							            }

							            $chartContainer.popupBalloon('option', {
								            content: calloutContent,
								            xOffsetInverted: (-1 * event.target.barW)
							            } );			

							            if( !$chartContainer.data("popbox").is(':visible') ){

								            $chartContainer.popupBalloon('option', { 
										                        positionX: calloutX,
										                        positionY: calloutY
									                        })
                                                            .popupBalloon('show');
							            } else {

								            $chartContainer.popupBalloon('updatePosition', calloutX, calloutY);
							            }

							            if( !$chartContainer.data("popbox").is(':visible') ){
								            $chartContainer.popupBalloon('show');
							            }

						            } else {
							            $chartContainer.popupBalloon({
								            popupClass: 'dj_metric-callout pie-chart disable-pointer-events',
								            height: 'auto', 
								            width: 'auto',
								            positionX: calloutX,
								            positionY: calloutY,
								            interactionMode: 'custom',
								            content: calloutContent,
								            animateDisplayEnabled: false,
								            animateHideEnabled: true,
								            animateMoveEnabled: false,
								            jScrollPaneEnabled: false,
								            popupPosition: 'right',
								            popupAlign: 'top',
								            xOffsetInverted: (-1 * event.target.barW)
							            });
						            }
                                },
                                mouseOut: function(event){
                                    var $chartContainer = $(event.target.series.chart.container).parent(); //dj_metric-chart

						            if( !$chartContainer.data('chartTimeout') ) {
							            $chartContainer.data('chartTimeout', window.setTimeout( function(){
								            $chartContainer.popupBalloon('hide');
							            }, 200 ) );
						            }
                                }
                            }
                        },
                        events: {
                            mouseOver: function(event) {
                                var $chartContainer = $(event.target.chart.container).parent(); //dj_metric-chart
                                $chartContainer.data('isOverChart', true);
                            },
                            mouseOut: function(event){
                                var $chartContainer = $(event.target.chart.container).parent(); //dj_metric-chart
                                $chartContainer.data('isOverChart', false);

					            if( !$chartContainer.data('chartTimeout') ) {
                                    $chartContainer.data('chartTimeout', window.setTimeout( function(){
							            $chartContainer.popupBalloon('hide');
						            }, 200 ) );
					            }
                            },
                            click: function (event) {
                                $dj.debug(event.point.packet);
                                self.publish(self.eventNames.barClicked, {
                                    data: event.point.packet
                                })
                            }
                        }
                    }
                }
            }

            return $.extend(true, {}, histogramChartConfig, {
                chart: {
                    renderTo: chartContainer
                },
                series: seriesData
            })
        },

        showChart: function (chartOptions) {
            try {
                this.chart = new Highcharts.Chart(chartOptions);
            }
            catch (e) {
                $dj.debug(e.message);
            }
        },

        setData: function (dataResult) {
            this.data = dataResult;
            this.renderChart();
        },

        convertData: function (histogram) {
            var dataSeries = [];
            if (histogram && histogram.items) {
                $.each(histogram.items, function (i, objvalue) {
                    if (objvalue.currentDate) {
                        dataSeries.push({
                            x: objvalue.currentDate.getTime(),
                            y: objvalue.hitCount.value,
                            packet: $.extend(true, {}, objvalue, {distribution: histogram.distribution})
                        });
                    }
                    else {
                        dataSeries.push({
                            x: objvalue.startDate.getTime(),
                            y: objvalue.hitCount.value,
                            packet: $.extend(true, {}, objvalue, {distribution: histogram.distribution})
                        });
                    }
                });
            }
            return [{
                name: 'time',
                data: dataSeries
            }];
        },

        renderChart: function () {
            var self = this,
                el = $(self.element),
                o = self.options;
            
            if (self.data) {
                self.$element.html(this.templates.datehistogramContents({ data: self.data }));
                var chartContainer = el.find('.dj_datehistogram-chart'),
                    graphDataModel = this.convertData(self.data),
                    chartOptions;

                chartOptions = this.initializeGraphOptions(chartContainer[0], graphDataModel);
                this.showChart(chartOptions);
            }
        },

        dispose: function () {
        }

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_DateHistogram', DJ.UI.DateHistogram);


})(jQuery);