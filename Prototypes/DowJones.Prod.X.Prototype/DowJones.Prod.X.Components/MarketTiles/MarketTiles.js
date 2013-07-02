/*!
 * MarketTilesModelViewComponent
 */

    DJ.UI.MarketTiles = DJ.UI.Component.extend({

        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "MarketTiles" }, meta));
            
            console.log(this.$element);
            this._resize();
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                // TODO: Add delegates
                // e.g.  OnHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
            });
            
        },

        _initializeElements: function () {
            // TODO: Get references to child elements
            // e.g.  this._headlines = this.$element.find('.clear-filters');
        },

        _initializeEventHandlers: function () {
            // TODO:  Wire up events to delegates
            // e.g.  this._headlines.click(this._delegates.OnHeadlineClick);
            DJ.subscribe('dj.productx.core.widgetSorted', $dj.delegate(this, this._resize));

            Response.resize($dj.delegate(this, this._resize));
        },
        
        _resize: function() {
            var band = this._getBand();
            var column = this._findColumn();
            var type = band + " " + column;
            this._processTemplate(type, null);
        },
        
        _findColumn: function() {
            var selector = 'ul.column';
            var column = this.$element.closest(selector);
            
            if (column.hasClass('largeColumn'))
            {
                return 'Large';
            }
            
            if (column.hasClass('mediumColumn')) {
                return 'Medium';
            }
            
            return 'Small';
                
        },
        
        _processTemplate: function (type, data) {
            if (this.currentView === type) {
                return;
            }
            
            switch(type) {
                case 'Large Large':
                    this.$element.html(this.templates.successLarge(data));
                    this._addChartLargeChart('.chartContainer');
                    this.currentView = type;
                    break;
                case 'Medium Large':
                case 'Medium Medium':
                    this.$element.html(this.templates.successMedium(data));
                    this._addChartMediumChart('.chartContainer', 'Medium');
                    this.currentView = type;
                    break;
                case 'Large Small':
                case 'Medium Small':
                    this.$element.html(this.templates.successSmall(data));
                    this._addChartMediumChart('.chartContainer', 'Small');
                    this.currentView = type;
                    break;
            }
        },
        

        _addChartLargeChart: function(selector) {
            $(selector, this.$element).highcharts({
                chart: {
                },
                title: {
                    text: 'Large chart'
                },
                xAxis: {
                    categories: ['Apples', 'Oranges', 'Pears', 'Bananas', 'Plums']
                },
                tooltip: {
                    formatter: function () {
                        var s;
                        if (this.point.name) { // the pie chart
                            s = '' +
                                this.point.name + ': ' + this.y + ' fruits';
                        } else {
                            s = '' +
                                this.x + ': ' + this.y;
                        }
                        return s;
                    }
                },
                labels: {
                    items: [{
                        html: 'Total fruit consumption',
                        style: {
                            left: '40px',
                            top: '8px',
                            color: 'black'
                        }
                    }]
                },
                series: [{
                    type: 'column',
                    name: 'Jane',
                    data: [3, 2, 1, 3, 4]
                }, {
                    type: 'column',
                    name: 'John',
                    data: [2, 3, 5, 7, 6]
                }, {
                    type: 'column',
                    name: 'Joe',
                    data: [4, 3, 3, 9, 0]
                }, {
                    type: 'spline',
                    name: 'Average',
                    data: [3, 2.67, 3, 6.33, 3.33],
                    marker: {
                        lineWidth: 2,
                        lineColor: Highcharts.getOptions().colors[3],
                        fillColor: 'white'
                    }
                }, {
                    type: 'pie',
                    name: 'Total consumption',
                    data: [{
                        name: 'Jane',
                        y: 13,
                        color: Highcharts.getOptions().colors[0] // Jane's color
                    }, {
                        name: 'John',
                        y: 23,
                        color: Highcharts.getOptions().colors[1] // John's color
                    }, {
                        name: 'Joe',
                        y: 19,
                        color: Highcharts.getOptions().colors[2] // Joe's color
                    }],
                    center: [100, 80],
                    size: 100,
                    showInLegend: false,
                    dataLabels: {
                        enabled: false
                    }
                }]
            });
        },

        _addChartMediumChart: function (selector, type) {
            $(selector, this.$element).highcharts({
                chart: {
                },
                title: {
                    text: type + ' chart'
                },
                xAxis: {
                    categories: ['Apples', 'Oranges', 'Pears', 'Bananas', 'Plums']
                },
                tooltip: {
                    formatter: function () {
                        var s;
                        if (this.point.name) { // the pie chart
                            s = '' +
                                this.point.name + ': ' + this.y + ' fruits';
                        } else {
                            s = '' +
                                this.x + ': ' + this.y;
                        }
                        return s;
                    }
                },
                series: [{
                    type: 'column',
                    name: 'Jane',
                    data: [3, 2, 1, 3, 4]
                }, {
                    type: 'column',
                    name: 'John',
                    data: [2, 3, 5, 7, 6]
                }, {
                    type: 'column',
                    name: 'Joe',
                    data: [4, 3, 3, 9, 0]
                }, {
                    type: 'spline',
                    name: 'Average',
                    data: [3, 2.67, 3, 6.33, 3.33],
                    marker: {
                        lineWidth: 2,
                        lineColor: Highcharts.getOptions().colors[3],
                        fillColor: 'white'
                    }
                }]
            });
        },

        _getBand: function() {
            if (Response.band(1200)) {
                return "Large";
            }

            if (Response.band(481)) {
                return "Medium";
            }  
            
            return "Small";
        },
        
        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_MarketTiles', DJ.UI.MarketTiles);
