/*!
 * StockKiosk
 *   e.g. , "this._imageSize" is generated automatically.
 *
 *   
 *  Getters and Setters are generated automatically for every Client Property during init;
 *   e.g. if you have a Client Property called "imageSize" on server side code
 *        get_imageSize() and set_imageSize() will be generated during init.
 *  
 *  These can be overriden by defining your own implementation in the script. 
 *  You'd normally override the base implementation if you have extra logic in your getter/setter 
 *  such as calling another function or validating some params.
 *
 */

DJ.UI.StockKiosk = DJ.UI.Component.extend({

    defaults: {
        debug: false,
        cssClass: 'dj_StockKiosk'
    },

    frequency: {
        OneMinute: 1,
        FiveMinutes: 5,
        FifteenMinutes: 15,
        OneHour: 60
    },

    timePeriod: {
        OneHour: 1,
        OneDay: 24,
        TwoDays: 48,
        FiveDays: 120,
        TenDays: 240
    },

    selectors: {
        quotes_ticker: '.dj_tickerRow',
        quotes_ticker_selected: '.selected',
        quotes_scrollable: '.scrollable',
        quotes_symbol: '.dj_tickerRowSymbol',
        stock_chart: '.dj_chartContainer',
        market_block: '.dj_marketBlock',
        stock_last_updated: '.dj_chartLastUpdated',
        stock_currency: '.dj_currency ',
        stock_label_name: '.dj_chartLabel',
        exchange_name: '.dj_exchange .dj_value',
        timezone_name: '.dj_timezone'
    },

    events: {
            // jQuery events <event>.<namespace>
            tickersymbolClick: "tickersymbolClick.dj.StockKiosk"   
        }, 

    chart: null,

    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "StockKiosk" }, meta);
        this._super(element, $meta);

        this.setData(this.data);      
    },

    // gets called in base.init()
    _initializeEventHandlers: function () {
        var self = this;

        this.$element.on('mouseover', self.selectors.quotes_ticker, function (event) {          
            var symbol = $(self.selectors.quotes_symbol, this).attr("data-symbol");   //using attr to alws use it as a string instead of data 
            self.$element.find(self.selectors.quotes_ticker_selected)
                         .removeClass('selected');
            $(this).addClass("selected"); 
            self.addSeriestoChart(symbol);             
        });
        
        /*
        this.$element.on('click', self.selectors.quotes_symbol , function(event) { 
            var symbol = $(this).attr("data-symbol");
            self.publish(self.events.tickersymbolClick, {tickersymbol: self.getStockticker(symbol) } ); 
            return false;
            //return(self.getStockticker(code));
        });*/
    },

    setHeight: function(){
        var tickerrowheight = $(this.selectors.quotes_ticker,this.$element).height() + 4; //padding of 4        
        var marketblocksize = Math.min(this.options.pagesize, this.data.length);     
        var marketblockheight = tickerrowheight * marketblocksize;
        $(this.selectors.market_block,this.$element).height(marketblockheight);
    },

    setScrollable: function()
    {
       $(this.selectors.quotes_scrollable,this.$element).scrollable({ vertical: true, mousewheel: true }); 
    },
    
    transformDataResult: function (dataPoints, marketDirection) {
        var stockDataSeries = [];
        if (dataPoints && dataPoints != null) {  //change to small case dataPoints
            $.each(dataPoints, function (i, objvalue) {
                //or new Date(objvalue.date).getTime()
                if (objvalue.dataPoint) {
                    if (objvalue.isLast && (marketDirection === 'up' || marketDirection === 'down')) {
                        if (marketDirection === 'up') {
                            stockDataSeries.push({ x: JSON.parseDate(objvalue.date).getTime(), y: objvalue.dataPoint.value, dateDisplay: objvalue.dateDisplay, marker: { enabled:true, radius:2, symbol:'circle', fillColor: '#11a106' } });                 
                        }
                        else {
                            stockDataSeries.push({ x: JSON.parseDate(objvalue.date).getTime(), y: objvalue.dataPoint.value, dateDisplay: objvalue.dateDisplay, marker: { enabled:true, radius:2, symbol:'circle', fillColor: '#cc3e33'  } });
                        }
                    }
                    else {
                        stockDataSeries.push({ x: JSON.parseDate(objvalue.date).getTime(), y: objvalue.dataPoint.value, dateDisplay: objvalue.dateDisplay });
                    }
                }
                else {
                    stockDataSeries.push({x:JSON.parseDate(objvalue.date).getTime(),  y:null, dateDisplay:null});
                }
            })
        }

        return {
            objStockSeries: stockDataSeries
        };
    },

    baseChartOptions: function (chartContainer, graphLineConfig) {
        var _highChartDateFormat = '%B %e, %Y';
        var self = this,
                _optionBag = self.options,
                el = $(self.element);
        var _dateformat = this.options.graphDateFormat || '%l%p';

        var marketIndexChartOptions = {
            chart: {
                renderTo: chartContainer,
                animation:false,
                defaultSeriesType: 'line',
                height: 110,
                width: this.options.graphWidth || 160,
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
                tickInterval: 4 * 60 * 60 * 1000, // 4 hours
                lineColor: '#ebebeb',  
			    minorTickLength: 0,	
			    minorTickInterval: 2 * 60 * 60 * 1000, // 2 hour			    		
			    minorGridLineWidth: 1,
			    minorGridLineColor: '#cccccc',
			    minorGridLineDashStyle: 'shortdot',
                minRange:  2 * 60 * 60 * 1000,

                labels: {
                    formatter: function () {
                        return (new Date(this.value)).format("hT", true);
                    },
                    style: {
                        color: '#999999',                        
                        fontFamily: 'arial, helvetica, clean, sans-serif', // default font
                        fontSize: '10px'
                    }
                },

                maxPadding: 0.05,
                minPadding: 0.05             
            },

            yAxis: {
                
                title: {
                    text: null
                },
                labels: {
                      align: _optionBag.yLabelsAlign || 'right',
                      x: -3,
                      y: 0,
                      step:2,
                      formatter: function () {    
                        if (!this.isLast) {
                            if (this.value < 5) {
                                return Highcharts.numberFormat(this.value, 3, '.'); 
                            }
                            return Highcharts.numberFormat(this.value, 2, '.');
                        }
                      },
                      style: {
                        color: '#999999',
                        fontFamily: 'arial, helvetica, clean, sans-serif', // default font
                        fontSize: '10px'
                    }
                },
                opposite: true,
                alternateGridColor: '#ffffff',
                gridLineColor: '#e9edee',
                showFirstLabel: false,
                endOnTick: false,
                startOnTick: false, 
                tickPixelInterval: 10,         
                maxPadding: 0.05,
			    minPadding: 0.05 
            },

            tooltip: {
                enabled: false,
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
                    //return( this.points[0].y + '<br/>' + Highcharts.dateFormat('%l:%M %p', this.x ) + ' ' );
                    return (this.points[0].y + '<br/>' + (new Date(this.x)).format("hh:MM TT", true));
                }
            },

            legend: {
                enabled: false
            },

            plotOptions: {
                series: {
                    lineWidth: 2,
                    marker: {
                        enabled: true,
                        radius: 0,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    shadow: false,
                    states: {
                        hover: {
                            enabled: false
                        }
                    }
                }
            },

            series: graphLineConfig

        } //end of marketIndexChartOptions
        return marketIndexChartOptions;
    },

    getSeriesOptions: function (graphDataModel) {
        return [{
            color: '#F80',            
            pointInterval: 1 * 60 * 1000, // 15 minutes
            name: "stockSeriesTitle",
            data: graphDataModel.objStockSeries
        }];
    },

    getStockticker:function(symbolcode){
        var self = this;
        $.each(self.data, function (index, stockobject) {
            if (symbolcode.toLowerCase() == stockobject.symbol.toLowerCase()) {   
                tickerobj = stockobject;
                return false;                            
            }
        });
        return tickerobj;
    },

    setData: function (intradarymarketResultSet) {
        this.data = intradarymarketResultSet;
        this.renderStockKiosk();

        //initialize only if data is returned.
        if (this.data){             
            this.setHeight();
            this.setScrollable();         
        } 
    },

    renderStockKiosk: function () {
        this.renderStocktemplate();        
        this.renderChart();
    },

    renderStocktemplate: function () {
        var self = this;

        if (self.data) {
            self.$element.html(this.templates.stockkiosk({ data: self.data, options: this.options }));                  
        }
        else {
            self.$element.html("<span class='no-results'><%= Token("noStockKioskResults") %></span>"); 
            return;           
        } 
    },

    addSeriestoChart: function(symbolcode){
        var self = this; 
        if (self.data && symbolcode) {
            var requestedIntradaryMarketData = self.getStockticker(symbolcode);
            
            // update the last updated timestamp
            var stock_last_updated = $(self.selectors.stock_last_updated,this.$element);
            if (stock_last_updated.length > 0 && requestedIntradaryMarketData.adjustedLastUpdatedDescripter) {
                stock_last_updated.html($.trim(requestedIntradaryMarketData.adjustedLastUpdatedDescripter));
            }
            
            $(self.selectors.stock_label_name, this.$element).html(requestedIntradaryMarketData.name);       
            $(self.selectors.timezone_name, this.$element).html(requestedIntradaryMarketData.exchange.timeZoneDescriptor);      
            $(self.selectors.exchange_name, this.$element).html(requestedIntradaryMarketData.exchange.code);      
            var currency = $(self.selectors.stock_currency, this.$element);
            var currencyVal =  $(self.selectors.stock_currency + " .dj_value", this.$element);
            if (currency.length > 0 )
            {
                if (requestedIntradaryMarketData.currency) {
                    currency.show();
                    currencyVal.html($.trim(requestedIntradaryMarketData.currency));
                }
                else {
                    currency.hide();
                }
            }
            
            var graphDataModel = self.transformDataResult(requestedIntradaryMarketData.dataPoints, self.getMarketDirection(requestedIntradaryMarketData.percentChange.value || 0)),
                chartContainer = self.$element.find(self.selectors.stock_chart);

            self.displayStockMarketChart(graphDataModel, chartContainer); 
        } 
    },

    getMarketDirection: function(percentChange) {
        if (percentChange > 0){
            return 'up';
        }
        else if (percentChange < 0) {
            return 'down';
        }
        else {
            return "-";
        } 
    },

    renderChart: function (symbolcode) {
        var self = this;
        var symbolcode = symbolcode || 0;

        if (self.data) {
            var requestedIntradaryMarketData;
            
            if (symbolcode == 0) {
                requestedIntradaryMarketData = self.data[0];  //render first chart // 0th will change
            }
            else {              
                requestedIntradaryMarketData = self.getStockticker(symbolcode);
            }

            // update the last updated timestamp
            var stock_last_updated = $(self.selectors.stock_last_updated,this.$element);
            if (stock_last_updated.length > 0 && requestedIntradaryMarketData.adjustedLastUpdatedDescripter) {
                stock_last_updated.html($.trim(requestedIntradaryMarketData.adjustedLastUpdatedDescripter));
            }

            $(self.selectors.stock_label_name, this.$element).html(requestedIntradaryMarketData.name);      
            $(self.selectors.timezone_name, this.$element).html(requestedIntradaryMarketData.exchange.timeZoneDescriptor);      
            $(self.selectors.exchange_name, this.$element).html(requestedIntradaryMarketData.exchange.code);      
            var currency = $(self.selectors.stock_currency, this.$element);
            var currencyVal =  $(self.selectors.stock_currency + " .dj_value", this.$element);
            if (currency.length > 0 )
            {
                if (requestedIntradaryMarketData.currency) {
                    currency.show();
                    currencyVal.html($.trim(requestedIntradaryMarketData.currency));
                }
                else {
                    currency.hide();
                }
            }

            //push in series of DataPoints (x and y points) .
            var graphDataModel = self.transformDataResult(requestedIntradaryMarketData.dataPoints, self.getMarketDirection(requestedIntradaryMarketData.percentChange.value || 0)),
                chartContainer = self.$element.find(self.selectors.stock_chart),
                chartOptions,
                seriesConfig;
            
            self.displayStockMarketChart(graphDataModel, chartContainer);        
        } //end of self.data

    },

    displayStockMarketChart: function (graphDataModel, chartContainer) {
        var self = this;
        try {
            if (graphDataModel.objStockSeries.length == 0) {
                chartContainer.html("<span class='no-results'><%= Token("noChartResults") %></span>"); 
                if (this.chart) {
                    this.chart = null;
                }
                return;
            }

            if (this.chart) {
                this.chart.series[0].setData(graphDataModel.objStockSeries);
            }
            else {
                var seriesOptions = self.getSeriesOptions(graphDataModel);
                chartOptions = self.baseChartOptions(chartContainer[0], seriesOptions);
                this.chart = new Highcharts.Chart(chartOptions);
            }
        }
        catch (e) {
            $dj.debug(e.message);
        }
    },

    dispose: function () {
    },

    EOF: {}
});


    // Declare this class as a jQuery plugin
    $.plugin('dj_StockKiosk', DJ.UI.StockKiosk);
