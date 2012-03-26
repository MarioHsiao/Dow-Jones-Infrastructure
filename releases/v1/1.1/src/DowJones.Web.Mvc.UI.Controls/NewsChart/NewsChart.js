/*!
* Company Profile Graph Control
*/

(function ($) {



    DJ.UI.NewsChart = DJ.UI.Component.extend({

        defaults: {

            debug: false,
            cssClass: 'CompanyProfileGraph'
        },

        events: {
            zoom: "chartZoom.dj.NewsChartControl",
            zoomReset: "chartZoomReset.dj.NewsChartControl",
            pointClick: "pointClick.dj.NewsChartControl",
            sourceClick: "sourceClick.dj.NewsChartControl"
        },

        chart: null,

        init: function (element, meta) {
            var $meta = $.extend({
                name: "NewsChartControl"
            }, meta);
            this._super(element, $meta);

            this.options.currencyCode = "";
            this.options.mdProvider = "";
            this.options.mdProviderUrl = "";
            this.options.languageCode = "en";
            this.container = element;

            this.renderChart();

        },

        trasformNewsDataResult: function (newsSeries, stockSeries) {
            var newsDataSeries = [];
            if (newsSeries && newsSeries.dataPoints) {
                $.each(newsSeries.dataPoints, function (i, objvalue) {
                    newsDataSeries.push({x:objvalue.date.getTime(), y:objvalue.dataPoint.value, dateDisplay: objvalue.dateDisplay});
                });
            }

            var stockDataSeries = [];
            if (stockSeries && stockSeries.dataPoints != null) {
                $.each(stockSeries.dataPoints, function (i, objvalue) {
                    stockDataSeries.push({x:objvalue.date.getTime(), y:objvalue.dataPoint.value, dateDisplay: objvalue.dateDisplay});
                })
            }

            return {
                objNewsSeries: newsDataSeries,
                objStockSeries: stockDataSeries
            };

        },
        
        initializeGraphOptions: function (DivToRender, graphDataModel, isfullSizeGraph) {
            var seriesConfig = this.fetchSeriesConfig(graphDataModel, isfullSizeGraph);

            return (!isfullSizeGraph) ? this.summaryChartOptions(DivToRender, seriesConfig, false)
                                    : this.newsStockChartOptions(DivToRender, seriesConfig, true);
        },

        summaryChartOptions: function (chartContainer, graphLineConfig, isfullSizeGraph) {
            var _highChartDateFormat = '%B %e, %Y';
            var self = this,
                _optionBag = self.options,
                el = $(self.element);
            var _dateformat = this.options.summaryGraphDateFormat || '%l%p';
            var marketIndexChartOptions = {
                chart: {
                    renderTo: chartContainer,
                    defaultSeriesType: 'line',
			        height: this.options.graphHeight || 224,
                    width: this.options.graphWidth || 302,
			        spacingTop: 0,
			        spacingRight: 1,
			        spacingBottom: 5,
			        spacingLeft: 1,
			        plotBorderColor: '#ebebeb',
			        plotBackgroundColor: '#f2fafd',
	                plotBorderWidth: 1,
			        showAxes: true,
			        style: {
				        fontFamily: 'arial, helvetica, clean, sans-serif', // default font
				        fontSize: '10px'
			        }

                },

                colors: [(_optionBag.stockSeriesColor || '#5ea9c3')],

                loading: {
                    hideDuration: 250,
                    showDuration: 250
                },

                credits: false,

                title: {
                    text: null
                },

                xAxis: {
			        type: 'datetime',
			        tickLength: 0,
			        minorTickLength: 0,			
			        tickInterval: 60 * 60 * 1000, // 60 minutes
			        lineColor: '#ebebeb',
			        labels: {
				
				        formatter: function() {
					        //return Highcharts.dateFormat(_dateformat, this.value);
                            return (new Date(this.value)).format("hTT", true);
				        },
				        style: {
					        color: '#999999',
					        fontFamily: 'arial, helvetica, clean, sans-serif', // default font
					        fontSize: '10px'
					
				        }

			        },
			        maxPadding: 0.15,
			        minPadding: 0.04
		        },

                yAxis: {
			        title: {
				        text: null
			        },
			        labels: {
                        align: 'right',
                        x: -3,
                        y: 14,
                        style: {
                            color: '#b3b3b3',
                            fontSize: '10px'
                        }
                    },
                    opposite: true,
			        alternateGridColor: '#ffffff',
			        gridLineColor: '#e9edee',
                    showFirstLabel: false
		        },

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
                    formatter: function() {
						//return( this.points[0].y + '<br/>' + Highcharts.dateFormat('%l:%M %p', this.x ) + ' ' );
                        return ( this.points[0].y + '<br/>' + (new Date(this.x)).format("hh:MM TT", true));
					}
		        },

		        legend: {
			        enabled: false
		        },

		        plotOptions: {
			        series: {
				        lineWidth: 3,
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

                series: graphLineConfig
            }
            return marketIndexChartOptions;
        },

        newsStockChartOptions: function (chartContainer, graphLineConfig, isfullSizeGraph) {
            var self = this,
                _optionBag = self.options,
                el = $(self.element);
            var _dateformat = this.options.FullChartDisplayDateFormat || '%b. %d';
            var _highChartDateFormat = '%B %e, %Y';


            var newsStockChartOption = {
                chart: {
                    defaultSeriesType: 'line',
                    height: this.options.graphHeight || 300,
                    width: this.options.graphWidth || 987,
                    zoomType: 'x',
                    spacingTop: 0,
			        spacingRight: 3,
			        spacingBottom: 25,
			        spacingLeft: 3,
			        plotBackgroundColor: '#f2fafd',
			        showAxes: true,
			        style: {
				        fontFamily: 'arial, helvetica, clean, sans-serif', // default font
				        fontSize: '10px'
			        },
                    renderTo: chartContainer,
                    events: {
                        selection: function (event) {
                            if (event.xAxis) {
                                var minDate = Highcharts.dateFormat(_highChartDateFormat, event.xAxis[0].min)
                                maxDate = Highcharts.dateFormat(_highChartDateFormat, event.xAxis[0].max);
                                el.triggerHandler(self.events.zoom, {
                                    sender: self,
                                    "startDate": minDate,
                                    "endDate": maxDate
                                });
                            } else {
                                var minDate = Highcharts.dateFormat(_highChartDateFormat, this.series[0].data[0].x);
                                var maxDate = Highcharts.dateFormat(_highChartDateFormat, this.series[0].data[this.series[0].data.length - 1].x);
                                el.triggerHandler(self.events.zoomReset, {
                                    sender: self,
                                    "startDate": minDate,
                                    "endDate": maxDate
                                });
                            }
                        }
                    }
                },

                loading: {
                    hideDuration: 250,
                    showDuration: 250
                },

                colors: [
                    _optionBag.newsSeriesColor || '#FFAD33',
                    _optionBag.stockSeriesColor || '#5EA9C3'
                ],

                credits: false,

                title: {
                    text: null
                },

                xAxis: {
                    type: 'datetime',
			        tickLength: 0,
			        minorTickLength: 0,	
			        minorTickInterval: 24 * 60 * 60 * 1000, // one days
				
			        tickInterval: 24 * 60 * 60 * 1000 * 7, // one days
			        maxZoom: 14 * 24 * 3600000, // fourteen days
			        lineColor: '#ebebeb',
			
			        minorGridLineWidth: 1,
			        minorGridLineColor: '#cccccc',
			        minorGridLineDashStyle: 'shortdot',

//			        dateTimeLabelFormats: {
//				        day: '%d - %b',
//				        week: '%d - %b'   
//	                },
			        labels: {

                        //not sure we need this anymore....
				        formatter: function() {

						    //return parseInt( Highcharts.dateFormat('%m', this.value) ) + Highcharts.dateFormat('/%e', this.value);
                            return (new Date(this.value)).format("dateMonth", true, _optionBag.languageCode);
						
				        },

                        style: {
					        fontFamily: 'Arial, Helvetica, sans-serif',
					        color: '#999999',
					        fontSize: '11px'
				        }
			        }
                },

                yAxis: [{
                    title: {
                        text: _optionBag.newsSeriesTitle || null,
                        style: {
					        color: '#cccccc',
					        fontSize: '11px'
				        },
				        margin: 3
                    },
                    alternateGridColor: '#ffffff',
			        gridLineColor: '#e8eced',
			        gridLineDashStyle: 'Solid',
			
			        lineColor: '#e0e0e0',
			        lineWidth: 1,
			
			        labels: {
				        align: 'right',
				        x: -3,
				        y: 14,
				        style: {
					        fontFamily: 'Arial, Helvetica, sans-serif',
					        color: '#b3b3b3',
					        fontSize: '11px'
				        },
				        formatter: function() {
					
					        if( this.value > 999999 )
						        return (this.value/1000000) + 'm';
					        else if( this.value > 999 )
						        return (this.value/1000) + 'k';
					        else	
						        return this.value;
						
				        }
                    },
			        showFirstLabel: false
                }, {
                    title: {
                        text: this.options.currencyCode,
                        style: {
					        color: '#cccccc',
					        fontSize: '11px'
				        },
				        margin: 3
                    },
                    alternateGridColor: '#ffffff',
			        gridLineColor: '#e8eced',
			        gridLineDashStyle: 'Solid',
			        lineColor: '#e0e0e0',
			        lineWidth: 1,
			        labels: {
				        align: 'left',
				        x: 3,
				        y: 14,
				        style: {
					        fontFamily: 'Arial, Helvetica, sans-serif',
					        color: '#b3b3b3',
					        fontSize: '11px'
				        }
			        },
                    opposite: true,
                    showFirstLabel: false
                }],

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
                        //var s =  (new Date(this.x)).format("UTC:mmmm dd, yyyy") + '<br/>';//Highcharts.dateFormat('%B %e, %Y', this.x) + '<br/>';
                        var s = this.points[0].point.dateDisplay + '<br/>';

                        if (this.points.length > 1) s += this.points[0].series.name + ': <b>' + this.points[0].y + " <%= Token("articlesLabel") %></b>";
                        else {
                            if (this.points[0].series.name == _optionBag.newsSeriesTitle) s += this.points[0].series.name + ': <b>' + this.points[0].y + " <%= Token("articlesLabel") %></b>";
                            else
                                s += 'Price: <b>' + Highcharts.numberFormat(this.points[0].y) + '</b>';
                        }
                        if (this.points[1]) s += "<br/><%= Token("priceLabel") %>: <b>" + Highcharts.numberFormat(this.points[1].y) + '</b>';
                        return s;
                    }
                },

                legend: {
                    enabled: false
                },

                plotOptions: {
                    series: {
                        lineWidth: 3,

                        point: {
                            events: {
                                click: function () {
                                    el.triggerHandler(self.events.pointClick, {
                                        sender: self,
                                        "xVal": this.x,
                                        "yVal": this.y,
                                        "formattedXVal": Highcharts.dateFormat(_highChartDateFormat, this.x),
                                        "seriesName": this.series.name,
                                        "dateDisplay": this.dateDisplay
                                    });
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

                series: graphLineConfig
            };
            return newsStockChartOption;
        },

        setCurrencyCode: function(currencyCode){
            this.options.currencyCode = currencyCode || "";
        },

        setLanguageCode: function(languageCode){
            this.options.languageCode = languageCode || "en";
        },

        setMDProvider: function(provider, url){
            this.options.mdProvider = provider || "";
            this.options.mdProviderUrl = url || "";
        },

        fetchSeriesConfig: function (graphDataModel, isFullSizeGraph) {
            if (!isFullSizeGraph) {
                
                if(graphDataModel.objStockSeries.length > 0 && graphDataModel.objStockSeries.length < 27 ) {
                    var padNull = 27 - graphDataModel.objStockSeries.length,
                        lastTime = graphDataModel.objStockSeries[graphDataModel.objStockSeries.length-1].x;

                    for( var i = 0; i < padNull; i++){
                        lastTime +=  15 * 60 * 1000;
                        graphDataModel.objStockSeries.push({x:lastTime,  y:null, dateDisplay:null});
                        
                    }
                }

                return [{
                    color: '#2f90b3',
					pointInterval: 15 * 60 * 1000, // 15 minutes
                    name: this.options.stockSeriesTitle || "<%= Token("marketIndex") %>",
                    data: graphDataModel.objStockSeries
                }];
            }

            return [{
                name: this.options.newsSeriesTitle || "<%= Token("newSeries") %>",
                type: 'spline',
                data: graphDataModel.objNewsSeries
            }, {
                name: this.options.stockSeriesTitle || "<%= Token("stockSeries") %>",
                yAxis: 1,
                data: graphDataModel.objStockSeries
            }];

        },

        showNewsChartControl: function (chartOptions) {
            try {
                this.chart = new Highcharts.Chart(chartOptions);
            }
            catch (e) {
                $dj.debug(e.message);
            }
        },

        setData: function (companyGraphDataResult) {
            this.data = companyGraphDataResult;
            this.renderChart();
        },

        renderChart: function () {
            var self = this,
                el = $(self.element),
                o = self.options,
                sourcesLabel = "<%= Token("sourcesLabel") %>";

            if (self.data) {
                var isFullSizeGraph = o.isFullChart;
                
                if(!isFullSizeGraph){
                    self.$element.html( this.templates.marketindex({ data: self.data.stockDataResult }) );

                    var indexSeries = self.data.stockDataResult,
                        graphDataModel = self.trasformNewsDataResult(null, indexSeries),
                        chartContainer = el.find('.dj_market_index-chart'),
                        chartOptions;
                        

                    if(graphDataModel.objStockSeries.length == 0)
                    {
                        chartContainer.html("<span class='no-results'><%= Token("noResults") %></span>");
                        return;
                    }
                    else
                    {
                        chartOptions = self.initializeGraphOptions(chartContainer[0], graphDataModel, isFullSizeGraph);

                        if(o.mdProvider && o.mdProviderUrl){

                            chartOptions.subtitle = {
	                            text: sourcesLabel + ': <a style="color: #3399CC" href="javascript:void(0);">'+o.mdProvider+'</a>',
	                            floating: true,
	                            align: 'left',
	                            x: 4,
	                            y: 14,
			                    style: {
				                    color: '#999999',
				                    fontSize: '10px'
			                    }
	                        };

                            chartOptions.chart.events = chartOptions.chart.events || {};
                            chartOptions.chart.events.load = $dj.delegate(self, self.onChartLoad);
                        }
                    }
                }
                else{

                    self.$element.html( this.templates.newsvolume() );

                    var NewsSeries = self.data.newsDataResult, 
                        StockSeries = self.data.stockDataResult,
                        NewsSeriesTitle = "<%= Token("newsVolume") %>", 
                        StockSeriesTitle = "<%= Token("stockPriceToken") %>",
                        graphDataModel = self.trasformNewsDataResult(NewsSeries, StockSeries),
                        chartContainer = el.find('.dj_module-company-chart'),
                        chartOptions;
                    var newsVolumeToggler = el.find(".news-volume"),
                        stockVolumeToggler = el.find(".stock-price");
                    
                    newsVolumeToggler.html(NewsSeriesTitle);
                    stockVolumeToggler.html(StockSeriesTitle);
                       
                    if(graphDataModel.objNewsSeries.length == 0 && graphDataModel.objStockSeries.length == 0)
                    {
                        newsVolumeToggler.css("cursor", "default").after("<span>,&nbsp;</span>");
                        stockVolumeToggler.css("cursor", "default");
                        el.find('.dj_module-company-chart-disclaimer').css( 'visibility', 'hidden' );
                        chartContainer.html("<span class='no-results'><%= Token("noResults") %></span><br /><br />");
                        return;
                    }
                    else{
                        chartContainer.html("");
                        chartOptions = self.initializeGraphOptions(chartContainer[0], graphDataModel, isFullSizeGraph);    

                        if(o.mdProvider && o.mdProviderUrl){
                            chartOptions.subtitle = {
	                            text: sourcesLabel + ': <a style="color: #3399CC" href="javascript:void(0);">'+o.mdProvider+'</a>',
	                            floating: true,
	                            align: 'left',
	                            x: 55,
	                            y: 14,
			                    style: {
				                    color: '#999999',
				                    fontSize: '10px'
			                    }
	                        };

                            chartOptions.chart.events = chartOptions.chart.events || {};
                            chartOptions.chart.events.load = $dj.delegate(self, self.onChartLoad);
                        }
                    }
                    
                    if(graphDataModel.objNewsSeries.length > 0 && graphDataModel.objStockSeries.length > 0){
                        newsVolumeToggler.html(o.newsSeriesTitle).click(function () {
                            $(this).toggleClass('disabled-chart-control');
                            var objSeries = self.chart.series[0];
                            objSeries.visible ? objSeries.hide() : objSeries.show();
                        }).after("<span>,&nbsp;</span>");

                        stockVolumeToggler.click(function () {
                            $(this).toggleClass('disabled-chart-control');
                            var objSeries = self.chart.series[1] || self.chart.series[0];
                            objSeries.visible ? objSeries.hide() : objSeries.show();
                        });

                        newsVolumeToggler.css("cursor", "pointer");
                        stockVolumeToggler.css("cursor", "pointer");
                        el.find('.dj_module-company-chart-disclaimer').css( 'visibility', 'visible' );
                    }
                    else
                    {

                        chartOptions.chart.spacingRight = 14;

                        if(graphDataModel.objNewsSeries.length == 0){
                            newsVolumeToggler.hide();
                        }
                        else{
                            newsVolumeToggler.css("cursor", "default");
                        }

                        if(graphDataModel.objStockSeries.length == 0){
                            stockVolumeToggler.hide();
                            el.find('.dj_module-company-chart-disclaimer').css( 'visibility', 'hidden' );
                        }
                        else{
                            stockVolumeToggler.css("cursor", "default");
                        }
                    }
                }

                this.showNewsChartControl(chartOptions);
            }
        },

        getChartDateRange: function(){
            try {
                if(this.data && this.options.isFullChart)
                {
                    var newsSeries = this.data.newsDataResult, 
                        stockSeries = this.data.stockDataResult,
                        newsStart, stockStart, newsEnd, stockEnd, start, end;
                
                    if(newsSeries&&newsSeries.dataPoints&&newsSeries.dataPoints.length > 0)
                    {
                         newsStart = newsSeries.dataPoints[0];
                         newsEnd = newsSeries.dataPoints[newsSeries.dataPoints.length -1];
                    }

                    if(stockSeries&&stockSeries.dataPoints&&stockSeries.dataPoints.length > 0)
                    {
                         stockStart = stockSeries.dataPoints[0];
                         stockEnd = stockSeries.dataPoints[stockSeries.dataPoints.length -1];
                    }

                    if(newsStart && stockStart)
                    {
                        start = newsStart;
                        if(newsStart.date > stockStart.date)
                            start = stockStart;

                        end = newsEnd;
                        if(newsEnd.date < stockEnd.date)
                            end = stockEnd;
                    }
                    else{
                        start = newsStart || stockStart;
                        end = newsEnd || stockEnd;
                    }
                    
                    return { start: start.dateDisplay, end: end.dateDisplay }; 
                }
            } 
            catch (e) {}
        },

        onChartLoad: function(){
            var self = this;
            var anchor = self.$element.find(".highcharts-subtitle").find("a");
            if(anchor.length == 0)
            {
                anchor = self.$element.find(".highcharts-subtitle").find("[onclick]");
                anchor.removeAttr("onclick");
            }
            anchor.click(function(){
                self.$element.triggerHandler(self.events.sourceClick, {
                    sender: self,
                    "url": self.options.mdProviderUrl,
                    "source": self.options.mdProvider
                });
            })
        },

        setError: function(error){
            if (error) {
                var self = this,
                el = $(self.element);
                
                if(!self.options.isFullChart){
                    self.$element.html($dj.formatError(error.returnCode, error.statusMessage));
                }
                else{

                    self.$element.html(this.templates.newsvolume());

                    var NewsSeriesTitle = "<%= Token("newsVolume") %>", 
                        StockSeriesTitle = "<%= Token("stockPriceToken") %>",
                        chartContainer = el.find('.dj_module-company-chart');

                    var newsVolumeToggler = el.find(".news-volume"),
                        stockVolumeToggler = el.find(".stock-price");
                    
                    newsVolumeToggler.html(NewsSeriesTitle);
                    stockVolumeToggler.html(StockSeriesTitle);
                    el.find("p.dj_module-company-chart-date-range").hide();
                    newsVolumeToggler.css("cursor", "default").after("<span>,&nbsp;</span>");
                    stockVolumeToggler.css("cursor", "default");
                    el.find('.dj_module-company-chart-disclaimer').css( 'visibility', 'hidden' );
                    chartContainer.html("<span class='no-results'>"+ $dj.formatError(error.returnCode, error.statusMessage) + "</span>");
                }
            }
        }

    });

    $.plugin('dj_NewsChartControl', DJ.UI.NewsChart);

})(jQuery);
