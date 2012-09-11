/*!
 * ShareChart
 */

DJ.UI.ShareChart = DJ.UI.Component.extend({

    defaults: {
        colors: Highcharts.getOptions().colors || []
    },

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "ShareChart" }, meta));

        // Initialize component if we got data from server
        this.setData(this.data);
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            // TODO: Add delegates
            // e.g.  OnHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
        });
    },

    _initializeElements: function () {
    },

    _initializeEventHandlers: function () {
        // TODO:  Wire up events to delegates
        // e.g.  this._headlines.click(this._delegates.OnHeadlineClick);
    },

    _mapToHighChartsData: function (result) {
        if (!result) {
            $dj.warn('_mapToHighChartsData: result is or undefined');
            return;
        }

        // Build the data arrays
        var browserData = [];
        var versionsData = [];
        for (var i = 0; i < result.data.length; i++) {

            // add browser data
            browserData.push({
                name: result.categories[i],
                y: result.data[i].share,
                color: this._getSafeColorByIndex(i)
            });

            // add version data
            for (var j = 0; j < result.data[i].drilldown.data.length; j++) {
                var brightness = 0.2 - (j / result.data[i].drilldown.data.length) / 5;
                versionsData.push({
                    name: result.data[i].drilldown.categories[j],
                    y: result.data[i].drilldown.data[j],
                    color: Highcharts.Color(this._getSafeColorByIndex(i)).brighten(brightness).get()
                });
            }
        };

        return {
            chartTitle: result.chartTitle,
            browserData: browserData,
            versionsData: versionsData
        };
    },
    
    _getSafeColorByIndex: function (index) {
        /// <summary>
        /// Always return a valid color. 
        /// If the index is above bounds, it warps to a valid index based on number of colors specified.
        /// </summary>
        
        var upper = this.options.colors.length;
        
        if(upper === 0) {
            $dj.error('_getSafeColorByIndex: Chart colors not specified.');
            return '#fff';
        }
        
        if(index < 0) {
            index = Math.abs(index);
        }

        if (index >= upper)
            return this.options.colors[index - upper];

        return this.options.colors[index];
    },

    setData: function (data) {
        if (!data) {
            this.bindOnNoData();
            return;
        }

        var highChartsData = this._mapToHighChartsData(data);
        this.bindOnSuccess(highChartsData);
    },

    bindOnNoData: function () {

    },

    bindOnSuccess: function (data) {
        this.$element.append(this.templates.success());

        this.chart = new Highcharts.Chart({
            chart: {
                renderTo: this.$element.find('.chartContainer')[0],
                type: 'pie'
            },
            title: {
                text: data.chartTitle || ''
            },
            plotOptions: {
                pie: {
                    shadow: false
                }
            },
            tooltip: {
                valueSuffix: '%'
            },
            credits: false,
            series: [{
                name: 'Browsers',
                data: data.browserData,
                size: '60%',
                dataLabels: {
                    formatter: function () {
                        return this.y > 5 ? this.point.name : null;
                    },
                    color: 'white',
                    distance: -30
                }
            }, {
                name: 'Versions',
                data: data.versionsData,
                innerSize: '60%',
                dataLabels: {
                    formatter: function () {
                        // display only if larger than 1
                        return this.y > 1 ? '<b>' + this.point.name + ':</b> ' + this.y + '%' : null;
                    }
                }
            }]
        });
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_ShareChart', DJ.UI.ShareChart);
