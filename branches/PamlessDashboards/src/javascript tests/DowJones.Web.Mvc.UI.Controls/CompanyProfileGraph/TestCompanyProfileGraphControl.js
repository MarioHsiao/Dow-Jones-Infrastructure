/*!
* Company Profile Graph Control
*/




(function ($) {





    DJ.UI.CompanyProfileGraphControl = DJ.UI.Component.extend({
        defaults: {

            debug: false,
            cssClass: 'CompanyProfileGraph',

            //companyGraphDataResult: {"__type":"companyChartPackage","newsDataResult":{"dataPoints":[{"dataPoint":{"__type":"wholeNumber","displayText":{"value":"316"},"value":316},"date":"\/Date(1281067200000-0400)\/","dateDisplay":"6 August 2010"},{"dataPoint":{"__type":"wholeNumber","displayText":{"value":"85"},"value":85},"date":"\/Date(1281153600000-0400)\/","dateDisplay":"7 August 2010"},{"dataPoint":{"__type":"wholeNumber","displayText":{"value":"9"},"value":9},"date":"\/Date(1281240000000-0400)\/","dateDisplay":"8 August 2010"},{"dataPoint":{"__type":"wholeNumber","displayText":{"value":"464"},"value":464},"date":"\/Date(1281326400000-0400)\/","dateDisplay":"9 August 2010"},{"dataPoint":{"__type":"wholeNumber","displayText":{"value":"791"},"value":791},"date":"\/Date(1281412800000-0400)\/","dateDisplay":"10 August 2010"}],"frequency":0},"stockDataResult":{"currencyCode":"USD","dataPoints":[{"closePrice":{"displayText":{"value":"25.37"},"value":25.37},"dataPoint":{"__type":"DoubleNumberStock:#DowJones.Utilities.Formatters","displayText":{"value":"25.37"},"value":25.37},"date":"\/Date(1280980800000-0400)\/","dateDisplay":"5 August 2010","dayHighPrice":{"displayText":{"value":"25.58"},"value":25.58},"dayLowPrice":{"displayText":{"value":"25.21"},"value":25.21},"openPrice":null,"volume":{"displayText":{"value":"64,922,008"},"value":64922008}},{"closePrice":{"displayText":{"value":"25.55"},"value":25.55},"dataPoint":{"__type":"DoubleNumberStock:#DowJones.Utilities.Formatters","displayText":{"value":"25.55"},"value":25.55},"date":"\/Date(1281067200000-0400)\/","dateDisplay":"6 August 2010","dayHighPrice":{"displayText":{"value":"25.56"},"value":25.56},"dayLowPrice":{"displayText":{"value":"25.02"},"value":25.02},"openPrice":null,"volume":{"displayText":{"value":"55,985,488"},"value":55985488}},{"closePrice":{"displayText":{"value":"25.61"},"value":25.61},"dataPoint":{"__type":"DoubleNumberStock:#DowJones.Utilities.Formatters","displayText":{"value":"25.61"},"value":25.61},"date":"\/Date(1281326400000-0400)\/","dateDisplay":"9 August 2010","dayHighPrice":{"displayText":{"value":"25.73"},"value":25.73},"dayLowPrice":{"displayText":{"value":"25.37"},"value":25.37},"openPrice":null,"volume":{"displayText":{"value":"57,102,540"},"value":57102540}},{"closePrice":{"displayText":{"value":"25.07"},"value":25.07},"dataPoint":{"__type":"DoubleNumberStock:#DowJones.Utilities.Formatters","displayText":{"value":"25.07"},"value":25.07},"date":"\/Date(1281412800000-0400)\/","dateDisplay":"10 August 2010","dayHighPrice":{"displayText":{"value":"25.34"},"value":25.34},"dayLowPrice":{"displayText":{"value":"24.88"},"value":24.88},"openPrice":null,"volume":{"displayText":{"value":"87,257,680"},"value":87257680}},{"closePrice":{"displayText":{"value":"24.86"},"value":24.86},"dataPoint":{"__type":"DoubleNumberStock:#DowJones.Utilities.Formatters","displayText":{"value":"24.86"},"value":24.86},"date":"\/Date(1281499200000-0400)\/","dateDisplay":"11 August 2010","dayHighPrice":{"displayText":{"value":"24.90"},"value":24.9},"dayLowPrice":{"displayText":{"value":"24.56"},"value":24.56},"openPrice":null,"volume":{"displayText":{"value":"76,745,824"},"value":76745824}}],"djTicker":"","exchange":{"code":"","descriptor":null,"isPrimary":false},"frequency":0,"fromDate":"\/Date(1280980800000-0400)\/","instrumentName":"MCROST","requestedSymbol":"MSFT.O","ric":"MSFT.O","source":"Tradeline","toDate":"\/Date(1296709200000-0500)\/"},"type":"CompanyChartPackage"},
            companyGraphDataResult: null, 
                        
            fullChart: true,
            newsSeriesTitle:  'News Volume',
            stockSeriesTitle: 'Stock Price'
            
        },
        chart: null,

        init: function (element, meta) {
            var $meta = $.extend({ name: "CompanyProfileGraphControl" }, meta);
            this._super(element, $meta);
            var self = this;
            
            this.container = element;
            this.$container = $(element);
            var graphControlInput = this.options.companyGraphDataResult;        //handle to JSON data
            
            var NewsSeries = graphControlInput.newsDataResult;               // getting News-Series-Data from JSON
            var StockSeries = graphControlInput.stockDataResult;             // getting Stock-Series-Data from JSON

            var graphDataModel = this.trasformNewsDataResult(NewsSeries,StockSeries);



            var DivElement = $('.company-module-chart')[0];

            FullSizeGraph = this.options.fullChart; 
       
           

            //***************************************************************************************************

           
            var ChartOptions = this.initializeGraphOptions(DivElement,graphDataModel,FullSizeGraph);
            this.showCompanyProfileGraphControl(ChartOptions);
            
            var newsVolumeToggler = $(".news-volume");
            var stockVolumeToggler = $(".stock-price");
            newsVolumeToggler.html(this.options.newsSeriesTitle);
            stockVolumeToggler.html(this.options.stockSeriesTitle);

             newsVolumeToggler.click(function () {
                        $(this).toggleClass('disabled-chart-control');
                        var objSeries = self.chart.series[0];
                        if (objSeries.visible) 
                            objSeries.hide();
                         else 
                            objSeries.show();                    
                     });
             
                     stockVolumeToggler.click(function () {
                        $(this).toggleClass('disabled-chart-control');
                        var objSeries = self.chart.series[1];
                        if (objSeries.visible) 
                            objSeries.hide();
                         else 
                            objSeries.show();                    
                     });
             
         
            if (FullSizeGraph == false)
            {
                newsVolumeToggler.removeClass('news-volume').addClass('small-news-volume');
                 stockVolumeToggler.removeClass('stock-price').addClass('small-stock-price');
                 $(".date-range").hide();
                 $(".company-chart-date-range").hide();
              }          
        },

        //****************************************************************************************
        //****************************************************************************************
        //****************************************************************************************
        //****************************************************************************************
        trasformNewsDataResult: function(NewsSeries,StockSeries )
        {
               var NewsDataSeries= []  ;
            $.each(NewsSeries.dataPoints, function(i,objvalue)
                {
                    NewsDataSeries.push([objvalue.date.getTime(), objvalue.dataPoint.value]);
                    //NewsDataSeries += "['" + objvalue.dateDisplay  + "' , " + objvalue.dataPoint.value + "],";
                }
            )
        
            var StockDataSeries = [] ;
            $.each(StockSeries.dataPoints, function(i,objvalue)
                {
                    StockDataSeries.push([objvalue.date.getTime(),objvalue.dataPoint.value]);
                }
            )
             
            return {
            objNewsSeries: NewsDataSeries, 
            objStockSeries: StockDataSeries
            };

        },



        //****************************************************************************************
        //****************************************************************************************
        //****************************************************************************************
        //****************************************************************************************




        initializeGraphOptions: function(DivToRender,graphDataModel,FullSizeGraph)
        {
           

            var seriesConfig; 
            var options;
       
            seriesConfig = this.fetchSeriesConfig(graphDataModel);
         
         
           if (FullSizeGraph == false)
                options = this.Summary_Chart_News(DivToRender,seriesConfig,FullSizeGraph);
                else
                options = this.Chart_News_Stock(DivToRender,seriesConfig,FullSizeGraph);

            return options;

        },
//        Summary_Chart_News: function (DivElement,GraphLineConfig,FullSizeGraph)
//        {
//           
//        var New_Stock_Options = {
//		chart: {
//            renderTo: DivElement,
//			defaultSeriesType: 'line',
//			height: 220,
//            width: 325,
//			spacingTop: 10,
//			spacingRight: 1,
//			spacingBottom: 5,
//			spacingLeft: 1,
//			plotBorderColor: '#ebebeb',
//			plotBackgroundColor: '#f2fafd',
//	        plotBorderWidth: 1,
//			showAxes: true,
//			style: {
//				fontFamily: 'arial, helvetica, clean, sans-serif', // default font
//				fontSize: '10px'
//			}
//		},

//		loading: {
//	        hideDuration: 250,
//	        showDuration: 250
//	    },

//		 colors: [
//			this.options.NewsSeriesColor,              //'#ff9300','#41a4ce'
//            this.options.StockSeriesColor
//		],

//		credits: false,

//		title: {
//			text: null
//		},

//		subtitle: {
//			text: null
//		},

//		xAxis: {
//			type: 'datetime',
//			tickLength: 0,
//			minorTickLength: 0,			
//			tickInterval: 24 * 60 * 60 * 1000, // one days
//			lineColor: '#ebebeb',
//			minPadding: 0.1,
//			maxPadding: 0.1,
//			gridLineColor: '#cccccc',
//			gridLineDashStyle: 'shortdash',
//			gridLineWidth: 1, 
//            labels: {
//                        formatter: function () {

//                            return Highcharts.dateFormat('%m/%e', this.value);
//                        },
//                        style: {
//                            color: '#999999',
//                            fontFamily: 'arial, helvetica, clean, sans-serif',
//                            fontSize: '8pt'
//                        }, 
//                        
//                    },
//		},

//		yAxis: [
//        {
//			title: {
//				text: null
//			},
//			labels: {
//				enabled: false
//			},
//			alternateGridColor: '#ffffff',
//			gridLineColor: '#e9edee',
//			labels: {
//				enabled: false
//			},
//			maxPadding: 0.2,
//			minPadding: 0.2
//		}
//        ,
//        {
//			title: {
//				text: null
//			},
//			alternateGridColor: '#ffffff',
//			gridLineColor: '#e9edee',
//			gridLineWidth: 0,
//			labels: {
//				enabled: false
//			},
//			maxPadding: 0.2,
//			minPadding: 0.2,
//			opposite: true
//		}
//        ],
//        tooltip: {
//                    backgroundColor: 'rgba(100, 100, 100, .85)',
//                    borderColor: '#333333',
//                    style: {
//                        color: '#ffffff'
//                    },
//                    shared: true,
//                    crosshairs: {
//                        color: '#000000'
//                    },


//                    formatter: function () {
//                        var s = Highcharts.dateFormat('%B %e, %Y', this.x) + '<br/>';
//                        s += 'News Volume: <b>' + this.points[0].y + ' Articles</b>';
//                        if (this.points[1])
//                            s += '<br/>Price: <b>$' + Highcharts.numberFormat(this.points[1].y) + '</b>';
//                        return s;
//                    }
//                },
//		legend: {
//			enabled: false
//		},

//		plotOptions: {
//			series: {
//				lineWidth: 3,
//               
//				marker: {
//					enabled: false,
//	                states: {
//                    	hover: {
//							enabled: true
//	                    }
//	                }
//				}
//			}
//		},
//        series: GraphLineConfig
//	}
//            return New_Stock_Options;
//        },


        Chart_News_Stock: function (DivElement,GraphLineConfig,FullSizeGraph)
        { 
            var self = this,
                el = $(self.element);
            var New_Stock_Options = {
                chart: {
                    defaultSeriesType: 'line',
                    height: this.options.ControlHeight,
                    //width: 1125,
                    zoomType: 'x',
                    spacingRight: 15,
                    spacingLeft: 15,
                    renderTo: DivElement,
                    events: {
                            selection: function(event) {
                                if (event.xAxis) {
                                        var minDate = Highcharts.dateFormat('%B %e, %Y', event.xAxis[0].min)
                                        maxDate = Highcharts.dateFormat('%B %e, %Y', event.xAxis[0].max);
                                        alert('Zoom-Start: '+ minDate +', Zoom-End: '+ maxDate);
                                } else {

                                        var minDate = Highcharts.dateFormat('%B %e, %Y', this.series[0].data[0].x)
                                        maxDate = Highcharts.dateFormat('%B %e, %Y', this.series[0].data[this.series[0].data.length - 1].x);
                                        alert('reset: '+ minDate +', Zoom-End: '+ maxDate);
                                    
//                                    alert('Selection reset');
                                }
                            }
                        } 
                },

//                loading: {
//                    hideDuration: 250,
//                    showDuration: 250
//                },

                colors: [
//			this.options.NewsSeriesColor,              //'#ff9300','#41a4ce'
//            this.options.StockSeriesColor

            '#ff9300','#41a4ce'
		],

                credits: false,

                title: {
                    text: null
                },


                subtitle: {
                    text: null
                },

                xAxis: {
                    type: 'datetime',
                    tickInterval:  24 * 60 * 60 * 1000, // one day
                    labels: {
                        formatter: function () {

                            return Highcharts.dateFormat('%b.%e', this.value);
                        },
                        step:10,
                        style: {
                            color: '#999999',
                            fontSize: '6pt'
                        }, 
                        enabled:FullSizeGraph
                    },
                  
                    gridLineDashStyle: 'shortdash',
                    gridLineWidth: 1
                },

                yAxis: [
                {
                   
                   title: {
                        text: this.options.newsSeriesTitle || null, 
                        style: {
                                color: '#cccccc',
                                fontSize: '12px'
                                },
                        margin: -1
                
                    },
                    labels: {
                        style: {
                            color: '#999999',
                            fontSize: '7pt'
                        }, 
                        enabled:FullSizeGraph

                    },
                    gridLineWidth: 0
                }
                ,
                 {
                    title: {
                        text: '$USD',
                        style: {
                            color: '#999999',
                            fontSize: '7pt'
                           },
                    
                    },
                    alternateGridColor: '#f1f3f8',
                    gridLineWidth: 0,
                    labels: {
                        style: {
                            color: '#999999',
                            fontSize: '7pt'
                        }, 

                        enabled: FullSizeGraph
                    },
                    opposite: true
                    
                }
                ],

                tooltip: {
                    backgroundColor: 'rgba(100, 100, 100, .85)',
                    borderColor: '#333333',
                    style: {
                        color: '#ffffff'
                    },
                    shared: true,
                    crosshairs: {
                        color: '#000000'
                    },


                    formatter: function () {
                        var s = Highcharts.dateFormat('%B %e, %Y', this.x) + '<br/>';
                        s += 'News Volume: <b>' + this.points[0].y + ' Articles</b>';
                        if (this.points[1])
                            s += '<br/>Price: <b>$' + Highcharts.numberFormat(this.points[1].y) + '</b>';
                        return s;
                    }
                },

                legend: {
                    enabled: false
                },

                plotOptions: {

                    series: {
                        shadow: false,
                        lineWidth: 3,

                        point: {
                                events: {
                                    click: function() {
                                        var xVal = Highcharts.dateFormat('%B %e, %Y', this.x);
                                        alert ('clicked point: '+ xVal +', point Value clicked: '+ this.y + "serises name" + this.series.name );
                                        el.triggerHandler("dj_CompanyProfileGraphControl.companyProfileControlLoad", { sender: this, "xVal": xVal, "y": this.y});
                                        
                                    }
                                  }
                                },
                        marker: {
                            enabled: false,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                },

                series: GraphLineConfig
            };
            return New_Stock_Options;
        },



         fetchSeriesConfig: function (graphDataModel) {
         
         
         var tempDate = new Date(graphDataModel.newsSeqStartDate); // stockDateSeq[0]); //newsSeqStartDate
				//var t = new Date(graphDataModel.stockDateSeq[0]);

            
         return [{
                name: "News Volume Series",
                //pointStart: Date.UTC(tempDate.getFullYear(), tempDate.getMonth(), tempDate.getDate()),
                //pointInterval: 24 * 60 * 60 * 1000, // 1 day,
                //data: graphDataModel.newsPointSeq
                data: graphDataModel.objNewsSeries
            }, {
                name: "Stock Price Series",
                type: 'spline',
                yAxis: 1,
                //pointStart: Date.UTC(t.getFullYear(), t.getMonth(), t.getDate()),
                //pointInterval: 24 * 60 * 60 * 1000, // 1 day
                //data: graphDataModel.stockPointSeq
                data: graphDataModel.objStockSeries
            }];

        },

        showCompanyProfileGraphControl: function (ChartOptions) {

           this.chart = new Highcharts.Chart(ChartOptions);        
        } // end function


       

    });

    $.plugin('dj_CompanyProfileGraphControl', DJ.UI.CompanyProfileGraphControl);
})(jQuery);