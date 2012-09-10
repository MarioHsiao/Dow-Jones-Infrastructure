/*!
 * ShareChart
 */

    DJ.UI.ShareChart = DJ.UI.Component.extend({

        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "ShareChart" }, meta));

            // TODO: Add custom initialization code
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
        },

        getPieConfig: function () {
            return {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: ''
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage}%</b>',
                    percentageDecimals: 1
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            color: '#000000',
                            connectorColor: '#000000',
                            formatter: function() {
                                return '<b>' + this.point.name + '</b>: ' + this.percentage + ' %';
                            }
                        }
                    }
                }/*     // This is here to show how to use the data
                        ,  
                        series: [{
                            type: 'pie',
                            name: 'Browser share',
                            data: [
                                ['Firefox', 45.0],
                                ['IE', 26.8],
                                {
                                    name: 'Chrome',
                                    y: 12.8,
                                    sliced: true,
                                    selected: true
                                },
                                ['Safari', 8.5],
                                ['Opera', 6.2],
                                ['Others', 0.7]
                            ]
                        }]*/
            };
        },

        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_ShareChart', DJ.UI.ShareChart);
