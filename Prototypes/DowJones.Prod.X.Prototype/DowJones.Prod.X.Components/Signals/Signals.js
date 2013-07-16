DJ.UI.Signals = DJ.UI.Component.extend({

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "MarketTiles" }, meta));
        this._resize();
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
        });
    },

    _initializeElements: function () {
    },

    _initializeEventHandlers: function () {
        DJ.subscribe('dj.productx.core.widgetSorted', $dj.delegate(this, this._resize));
        DJ.subscribe('dj.productx.core.collapseFired', $dj.delegate(this, this._collapsedFired));
        Response.resize($dj.delegate(this, this._resize));
    },
    
    _collapsedFired: function () {
        var self = this;
        self._currentView = '';
        setTimeout(self._resize());
    },
        
    _resize: function () {
        var band = this._getDevice();
        var column = this._findColumn();
        var type = band + " " + column;
        console.log(type);
        this._processTemplate(type, this._getData());
    },
        
    _getDevice: function () {
        if (Response.band(1200)) {
            return "Large";
        }

        if (Response.band(481)) {
            return "Medium";
        }

        return "Small";
    },

    _findColumn: function () {
        var activeColumnSelector = 'ul.column';
        var inactiveColumnSelector = 'ul.xColumn';
        var column = this.$element.closest(activeColumnSelector); 

        if (column.length == 0) {
            column = this.$element.closest(inactiveColumnSelector);
        }
        
        if (column) {
            if (column.hasClass('largeColumn')) {
                return 'Large';
            }

            if (column.hasClass('mediumColumn')) {
                return 'Medium';
            }

            return 'Small';
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
                this._insertChart('.dj_chartContainer', data);
                this._currentView = type;
                break;
            case 'Medium Large':
            case 'Medium Medium':
            case 'Small Large':
                this.$element.html(this.templates.successMedium(data));
                this._insertChart('.dj_chartContainer', data);
                this._currentView = type;
                break;
            case 'Large Small':
            case 'Medium Small':
            case 'Small Medium':
            case 'Small Small':
                this.$element.html(this.templates.successSmall(data));
                this._currentView = type;
                break;
        }
    },

    _getData: function() {
        var colors = Highcharts.getOptions().colors,
            categories = ['Lawsuit', 'Financial Ratings', 'Financial Announcements', 'Product Agreement', 'Merger / Acquisitions', 'Product Announcement',
                        'NewFunding', 'Management Change', 'Partnerships'],
            name = 'Signals Types',
            data = [{
                y: 32,
                color: colors[0],
                drilldown: {
                    name: 'Lawsuit Types', //32
                    categories: ['Settled', 'Infringement', 'Filed', 'Unspecified'],
                    data: [10, 14, 6, 2],
                    color: colors[0]
                }
            }, {
                y: 15,
                color: colors[1],
                drilldown: {
                    name: 'Financial Ratings', //15
                    categories: ['Stock', 'Revised', 'Downgrade', 'Upgrade', 'Ratings'],
                    data: [4, 3, 3, 3, 2],
                    color: colors[1]
                }
            }, {
                y: 10,
                color: colors[2],
                drilldown: {
                    name: 'Financial Announcements', //10
                    categories: ['Revenue', 'New Income', 'Dividend'],
                    data: [4, 4, 2],
                    color: colors[2]
                }
            }, {
                y: 3,
                color: colors[3],
                drilldown: {
                    name: 'Product Agreement', //3
                    categories: ['Sony', 'Nokia', 'Seven Networks'],
                    data: [1, 1, 1],
                    color: colors[3]
                }
            }, {
                y: 26,
                color: colors[4],
                drilldown: {
                    name: 'Merger / Acquisitions', //26
                    categories: ['Nook Media PLC', 'Intel', 'Barnes & Noble', 'Millennial Media', 'Yammer', 'Skype', 'Google', 'Ericsson'],
                    data: [ 10, 5, 3, 2, 2, 2, 1, 1],
                    color: colors[4]
                }
            }, {
                y: 12,
                color: colors[5],
                drilldown: {
                    name: 'Product Announcement', //12
                    categories: ['XBox One', 'Windows 8', 'OS', 'Patch'],
                    data: [8, 2, 1, 1],
                    color: colors[5]
                }
            }, {
                y: 7,
                color: colors[6],
                drilldown: {
                    name: 'New Funding', //7
                    categories: ['Venture Funding', 'Investment'],
                    data: [4, 3 ],
                    color: colors[6]
                }
            }, {
                y: 22,
                color: colors[7],
                drilldown: {
                    name: 'Management Change', //22
                    categories: ['Entering', 'Leaving', 'Retirement'],
                    data: [14, 7, 1],
                    color: colors[7]
                }
            }, {
                y: 11,
                color: colors[8],
                drilldown: {
                    name: 'Partnerships', //11
                    categories: ['Agreement','Product/Services', 'Project Development'],
                    data: [7, 2, 2],
                    color: colors[8]
                }
            }];
    
    
        // Build the data arrays
        var signalsData = [];
        var typesData = [];
        for (var i = 0; i < data.length; i++) {
    
            // add browser data
            signalsData.push({
                name: categories[i],
                y: data[i].y,
                color: data[i].color
            });
    
            // add version data
            for (var j = 0; j < data[i].drilldown.data.length; j++) {
                var brightness = 0.2 - (j / data[i].drilldown.data.length) / 5 ;
                typesData.push({
                    name: data[i].drilldown.categories[j],
                    y: data[i].drilldown.data[j],
                    color: Highcharts.Color(data[i].color).brighten(brightness).get(),
                    signal: categories[i]
                });
            }
        }

        return {'signalsData': signalsData, 'typesData': typesData };
    },
        
    
    _insertChart: function (selector, data) {
        // Create the chart
        $(selector, this.$element).highcharts({
            chart: {
                type: 'pie'
            },
            title: {
                text: null
            },
            yAxis: {
                title: {
                    text: null
                }
            },
            legend: {
                layout: 'vertical',
                borderWidth:0,
                symbolPadding: 3,
                symbolWidth: 10
            },
            plotOptions: {
                pie: {
                    shadow: false,
                    center: ['50%', '50%']
                }
            },
            credits: {
                enabled: false
            },

            tooltip: {
                //valueSuffix: '%'
                formatter: function () {

                    if (this.point.options.signal) {
                        return '<b>Signal: </b>' + this.point.options.signal + '<br/><b>Type: </b>' + this.point.name + ' (' + this.point.y + ')';
                    }
                    return this.point.name + ': ' + '(' + this.point.y + ')';
                },
            },
            
            series: [{
                name: 'Signals',
                data: data.signalsData,
                size: '100%',
                dataLabels: {
                    formatter: function () {
                        return this.y > 5 ? this.point.name : null;
                    },
                    enabled: false,
                    color: '#f0f0f0',
                    distance: -50
                },
                showInLegend: true,
               
            }, {
                name: 'Signal-Types',
                data: data.typesData,
                size: '100%',
                innerSize: '100%',
                dataLabels: {
                    formatter: function() {
                        // display only if larger than 1
                        return this.y > 1 ? '<b>'+ this.point.name +':</b> '+ this.y + ''  : null;
                    },
                    enabled: false,
                }
            }]
        }); 
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plug-in
$.plugin('dj_Signals', DJ.UI.Signals);
