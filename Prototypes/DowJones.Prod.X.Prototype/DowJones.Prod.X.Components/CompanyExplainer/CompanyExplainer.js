/*!
 * CompanyExplainerComponentModel
 */

    DJ.UI.CompanyExplainer = DJ.UI.Component.extend({

        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "CompanyExplainerComponentModel" }, meta));
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
            });
        },

        _initializeElements: function () {
        },

        _initializeEventHandlers: function () {
            DJ.subscribe('dj.productx.core.widgetSorted', $dj.delegate(this, this._resize));
            Response.resize($dj.delegate(this, this._resize));
        },
        
        _resize: function () {
            var band = this._getBand();
            var column = this._findColumn();
            var type = band + " " + column;
            this._processTemplate(type, null);
        },

        _findColumn: function () {
            var selector = 'ul.column';
            var column = this.$element.closest(selector);

            if (column.hasClass('largeColumn')) {
                return 'Large';
            }

            if (column.hasClass('mediumColumn')) {
                return 'Medium';
            }

            return 'Small';

        },

        _processTemplate: function (type, data) {
            if (this._currentView === type) {
                return;
            }

            switch (type) {
                case 'Large Large':
                case 'Large Medium':
                    this.$element.html(this.templates.successLarge(data));
                    this._addChartLargeChart('.dj_chartContainer');
                    this._currentView = type;
                    break;
                case 'Medium Large':
                case 'Medium Medium':
                case 'Small Large':
                    this.$element.html(this.templates.successMedium(data));
                    this._addChartMediumChart('.dj_chartContainer', 'Medium');
                    this._currentView = type;
                    break;
                case 'Large Small':
                case 'Medium Small':
                case 'Small Medium':
                case 'Small Small':
                    this.$element.html(this.templates.successSmall(data));
                    this._addChartMediumChart('.dj_chartContainer', 'Small');
                    this._currentView = type;
                    break;
            }
        },
        

        
        newsStockChartOptions: function (chartContainer, graphLineConfig) {
            var self = this,
                optionBag = self.options,
                el = $(self.element);
            var highChartDateFormat = '%B %e, %Y';
            var minDate, maxDate;
            
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
                                minDate = Highcharts.dateFormat(highChartDateFormat, event.xAxis[0].min);
                                maxDate = Highcharts.dateFormat(highChartDateFormat, event.xAxis[0].max);
                                el.triggerHandler(self.events.zoom, {
                                    sender: self,
                                    "startDate": minDate,
                                    "endDate": maxDate
                                });
                            } else {
                                minDate = Highcharts.dateFormat(highChartDateFormat, this.series[0].data[0].x);
                                maxDate = Highcharts.dateFormat(highChartDateFormat, this.series[0].data[this.series[0].data.length - 1].x);
                                el.triggerHandler(self.events.zoomReset, {
                                    sender: self,
                                    "startDate": minDate,
                                    "endDate": maxDate
                                });
                            }
                        }
                    }
                },

                exporting: {
                    enabled: false
                },

                loading: {
                    hideDuration: 250,
                    showDuration: 250
                },

                colors: [
                    optionBag.newsSeriesColor || '#5ea9c3',
                    optionBag.stockSeriesColor || '#ffad33'
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
                        formatter: function () {
                            //return parseInt( Highcharts.dateFormat('%m', this.Data) ) + Highcharts.dateFormat('/%e', this.Data);
                            return (new Date(this.value)).format("d - mmm", true);
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
                        text: optionBag.newsSeriesTitle || null,
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
                        formatter: function () {

                            if (this.value > 999999)
                                return (this.value / 1000000) + 'm';
                            else if (this.value > 999)
                                return (this.value / 1000) + 'k';
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

                        if (this.points.length > 1) s += this.points[0].series.name + ': <b>' + this.points[0].y + " <%= Token('articlesLabel') %></b>";
                        else {
                            if (this.points[0].series.name == optionBag.newsSeriesTitle) s += this.points[0].series.name + ': <b>' + this.points[0].y + " <%= Token('articlesLabel') %></b>";
                            else
                                s += 'Price: <b>' + Highcharts.numberFormat(this.points[0].y) + '</b>';
                        }
                        if (this.points[1]) s += "<br/><%= Token('priceLabel') %>: <b>" + Highcharts.numberFormat(this.points[1].y) + '</b>';
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
                                        "formattedXVal": Highcharts.dateFormat(highChartDateFormat, this.x),
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


        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CompanyExplainer', DJ.UI.CompanyExplainer);
