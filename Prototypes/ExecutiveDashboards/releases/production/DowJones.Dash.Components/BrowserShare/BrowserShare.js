/*!
 * BrowserShare
 */

DJ.UI.BrowserShare = DJ.UI.Component.extend({

    defaults: {
        deviceType: 'desktop'
    },
    
    selectors: {
        shareChartContainer: '.shareChartContainer'
    },

    init: function (element, meta) {
        this._super(element, $.extend({ name: "BrowserShare" }, meta));
    },


    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function () {
        this.$element.html(this.templates.container());
        this._shareChartContainer = this.$element.find(this.selectors.shareChartContainer);
    },

    _initializeEventHandlers: function () {
        if(this.options.deviceType === 'mobile')
            $dj.subscribe('data.DeviceTrafficMobile', this._delegates.setData);
        else
            $dj.subscribe('data.DeviceTrafficDesktop', this._delegates.setData);
    },
    
    _setData: function (data) {
        data = data[0];
        var total = parseInt(data['All'], 10);
        var browserData = [];
        for(var key in data) {
            if (key !== 'page_name' && key !== 'All')
                browserData.push([key, parseFloat((parseInt(data[key], 10)/total * 100).toFixed(2))]);
        }
        
        this._renderChart(browserData);
    },
    
      
    _renderChart: function (data) {

        var filteredData = _.filter(data, function (item) {
            return item[1] !== 0;
        });
        if (!this.chart) {
            this.chart = new Highcharts.Chart({
                chart: {
                    renderTo: this.$element.find(this.selectors.shareChartContainer)[0],
                    type: 'pie',
                    backgroundColor: 'transparent',
                    plotBorderWidth: null,
                    plotShadow: false,
                    spacingLeft: 20,
                    spacingRight: 20
                },
                title: {
                    text: data.chartTitle || ''
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            color: '#777', 
                            formatter: function () {
                                return '<b>' + this.point.name + '</b><br/>' + parseFloat(this.percentage).toFixed(2) + '%';
                            }
                        },
                        softConnector: false
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage}%</b>',
                    percentageDecimals: 1
                },
                credits: false,
                series: [{
                    type: 'pie',
                    name: 'Browser Share',
                    data: filteredData
                }]
            });
        }
        else {
            this.chart.series[0].setData(filteredData);
        }
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_BrowserShare', DJ.UI.BrowserShare);
