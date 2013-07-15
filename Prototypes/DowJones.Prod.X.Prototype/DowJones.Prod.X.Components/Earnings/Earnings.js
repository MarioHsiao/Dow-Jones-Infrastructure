/*!
 * EarningsComponentModel
 */

    DJ.UI.Earnings = DJ.UI.Component.extend({
        init: function(element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "Earnings" }, meta));
            this._resize();
        },

        _initializeDelegates: function() {
            this._delegates = $.extend(this._delegates, {
                
                // TODO: Add delegates
                // e.g.  OnHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
            });
        },

        _initializeElements: function() {
            // TODO: Get references to child elements
            // e.g.  this._headlines = this.$element.find('.clear-filters');,
        },

        _initializeEventHandlers: function() {
            DJ.subscribe('dj.productx.core.widgetSorted', $dj.delegate(this, this._resize));
            Response.resize($dj.delegate(this, this._resize));
        },

        _getDevice: function() {
            if (Response.band(1200)) {
                return "Desktop";
            }

            if (Response.band(481)) {
                return "Tablet";
            }

            return "Mobile";
        },

        _resize: function() {
            var device = this._getDevice();
            var column = this._findColumn();
            var type = device + " " + column;
            this._processTemplate(type, null);
        },

        _findColumn: function() {
            var largeColumnSelector = 'ul.largeColumn';
            var mediumColumnSelector = 'ul.mediumColumn';

            var largeColumn = this.$element.closest(largeColumnSelector);
            if (largeColumn) {
                return "Large";
            }

            var mediumColumn = this.$element.closest(mediumColumnSelector);
            if (mediumColumn) {
                return 'Medium';
            }

            return 'Small';
        },

        _processTemplate: function(type, data) {
            if (this._currentView === type) {
                return;
            }

            switch (type) {
            case 'Desktop Large':
            case 'Desktop Medium':
                this.$element.html(this.templates.successLarge(data));
                this._insertChart(".dj_chartContainer");
                this._currentView = type;
                break;
            case 'Tablet Large':
            case 'Tablet Medium':
            case 'Mobile Large':
                this.$element.html(this.templates.successMedium(data));
                this._insertChart(".dj_chartContainer");
                this._currentView = type;
                break;
            case 'Desktop Small':
            case 'Tablet Small':
            case 'Mobile Medium':
            case 'Mobile Small':
                this.$element.html(this.templates.successSmall(data));
                this._insertChart(".dj_chartContainer");
                this._currentView = type;
                break;
            }
        },

        _insertChart: function(selector) {
            $(selector, this.$element).highcharts({
                chart: {
                    "renderTo": "StockPriceActivityContainer_MSFT",
                    "defaultSeriesType": "line",
                    "events": {
                        "click": function (event) {
                            return false;
                        },
                        "load": function(event) {
                            $(event.target.container).mouseover(function(e) { $(this).css('cursor', 'pointer'); }).mouseout(function(e) { $(this).css('cursor', 'default'); });
                        }
                    },
                    "height": 150,
                    "width": 233
                },
                credits: {
                    "enabled": true,
                    "href": "javascript:void(0)",
                    "text": "SIX Financial Information",
                    "style": { "cursor": "default" },
                    "position": { "align": "left", "verticalAlign": "bottom", "x": 10, "y": -1 }
                },
                plotOptions: {
                    series: {
                        "pointStart": 1366344000000,
                        "allowPointSelect": false,
                        "cursor": "pointer",
                        "events": {
                            "click": function (event) {
                                return false;
                            }
                        },
                        "lineWidth": 3,
                        "marker": { "enabled": false }, "pointInterval": 604800000, "showInLegend": false, "states": { "hover": { "enabled": false } }
                } },
                title: { "text": "" },
                legend: { "enabled": false },
                exporting: { "enabled": false },
                xAxis: [{
                    "dateTimeLabelFormats": {
                         "month": "%b", "year": "%b"
                    },
                    "labels": {
                        "enabled": true,
                        "style": {
                            "fontWeight": "bold",
                            "fontSize": "9",
                            "fontFamily": "Arial"
                        }
                    },
                    "minorGridLineColor": "#CCC",
                    "minorGridLineWidth": 1,
                    "minorTickInterval": "auto",
                    "minorTickLength": 0,
                    "tickColor": "#000",
                    "tickPixelInterval": 75,
                    "title": {
                         "text": ""
                    },
                    "type": "datetime"
                }],
                yAxis: [{
                    "tickWidth": 1,
                    "gridLineWidth": 1,
                    "labels": {
                        "enabled": true,
                        "style": {
                            "fontWeight": "bold",
                            "fontSize": "9",
                            "fontFamily": "Arial"
                        }
                    },
                    "lineWidth": 1,
                    "opposite": true,
                    "tickColor": "#000",
                    "tickPixelInterval": 20,
                    "title": {
                         "text": ""
                    }
                }],
                tooltip: {
                     "enabled": false
                },
                series: [{
                    "data": [29.765, 31.79, 33.49, 32.69, 34.87, 34.269, 34.9, 35.67, 34.4, 33.265, 34.545, 34.21, 35.67],
                    "type": "line",
                    "color": "#54559B"
                }]
            });
        },   
        
        EOF: null
// Final property placeholder (without a comma) to allow easier moving of functions
    });


        // Declare this class as a jQuery plugin
        $.plugin('dj_Earnings', DJ.UI.Earnings);