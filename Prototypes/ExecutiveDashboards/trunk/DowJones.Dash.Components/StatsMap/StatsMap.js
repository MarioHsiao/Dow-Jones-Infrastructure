/*!
 * StatsMap
 */

DJ.UI.StatsMap = DJ.UI.Component.extend({

    defaults: {
        mapType: 'country',
        map: 'us'
    },

    selectors: {
        worldViewToggler: '.worldViewToggler'
    },

    mapConfig: {
        chart: {
            type: 'map',
            backgroundColor: 'transparent',
            borderWidth: 0
        },

        plotOptions: {
            series: {
                animation: false
            },
            map: {
                dataLabels: {
                    enabled: false,
                    formatter: function (dataLabelOptions) {
                        return this.point.options.key.toUpperCase();
                    },
                    style: {
                        fontWeight: 'bold'
                    }
                },
                marker: {
                    enabled: false
                },
                states: {
                    hover: {
                        color: Highcharts.getOptions().colors[6]
                    }
                },
                valueRanges: [{
                    color: '#ddd'
                }, {
                    from: 0.01,
                    to: 5,
                    color: Highcharts.getOptions().colors[2]
                }, {
                    from: 5.01,
                    to: 7,
                    color: Highcharts.getOptions().colors[5]
                }, {
                    from: 7.01,
                    color: Highcharts.getOptions().colors[1]
                }]
            }
        },

        credits: { enabled: false },
        title: { text: null },
        legend: { enabled: false },

        tooltip: {
            formatter: function () {
                if (!this.y || this.y === -1) return '<b>' + this.point.key + '</b><br/>No Data';
                return '<b>' + this.point.key + '</b><br/>Avg: ' + this.y + 's<br/>Min:' + this.point.min + 's<br/>Max:' + this.point.max + 's';
            }
        },

        series: [{ type: 'map', data: [] }]
    },

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: 'StatsMap' }, meta));
        this.$element.addClass("dj_statMap");
        this._initializeMapData();
        this._initializeChart();
      
        if (this.data) {
            this.setData(this.data);
        }
    },


    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function (ctx) {
        this._super(ctx);
        this.$element.html(this.templates.container());
        
        this.mapContainer = ctx.find('.mapContainer');

        if (this.options.mapType === 'world')
            ctx.find('.worldViewToggler').hide();
    },

    _initializeEventHandlers: function () {
        
        var self = this;

        this.$element.on('click', this.selectors.worldViewToggler, function () {
            self.publish('worldView.dj.StatsMap');
        });

        $(window).resize(function () {
            var mapContainer = self.mapContainer;
            var width = mapContainer.width(),
                height = mapContainer.height();

            if (!height || !width) return;

            // set height, width to be slightly less than actual box
            var chartWidth = width - width * 0.05,
                chartHeight = height - height * 0.05;

            self.chart.setSize(chartWidth, chartHeight);
            self.chart.series[0].setData(self.chartData, true);
        });
    },

    _initializeMapData: function (map) {
        map = map || this.options.map;
        this.mapSource = map;
        this.map = Highcharts.Maps[map];

        if (!this.map) {
            $dj.warn(map, "Map is not supported yet.");
            return;
        }

        this.territories = this.map.territories;
        this.paths = this.map.paths;
    },


    _initializeChart: function (performanceZones) {
        this._initializingChart = true;

        // highcharts will wipe out series object, hence initialize this
        this.mapConfig.series = this.mapConfig.series || [{ type: 'map', data: [] }];

        if (performanceZones)
            this.mapConfig.plotOptions.map.valueRanges = this._configureValueRanges(performanceZones);

        for (var i = 0; i < this.territories.length; i++) {
            var key = this.territories[i];

            this.mapConfig.series[0].data.push({
                key: key,
                path: this.paths[key]
            });
        }

        // blank out previous maps
        this.mapContainer.html('');

        var width = this.mapContainer.width(),
            height = this.mapContainer.height();

        //console.log('_initializeChart:', this.mapSource, width, height);

        this.mapConfig.chart.width = width - width * 0.05;
        this.mapConfig.chart.height = height - height * 0.05;

        //console.log('_initializeChart:', this.mapSource, this.mapConfig.chart);
        this.mapConfig.chart.renderTo = this.$element.find('.mapContainer')[0];

        if (this.chart) this.chart.destroy();

        this.chart = new Highcharts.Map(this.mapConfig);

        this._initializingChart = false;
    },

    _configureValueRanges: function (zones) {
        var valueRanges = [{ color: '#ddd' }], key, zone;

        for (key in zones) {
            zone = zones[key];
            valueRanges.push({ from: zone.from, to: zone.to, color: this.getZoneColor(zone.zoneType) });
        }

        return valueRanges;
    },

    getZoneColor: function (type) {
        switch (type) {
            case 'Cool':
                return Highcharts.getOptions().colors[2];
            case 'Neutral': return Highcharts.getOptions().colors[5];
            case 'Hot': return Highcharts.getOptions().colors[1];
            default:
                return '#ddd';
        }

    },

    setData: function (data) {
        // don't bother until chart is initialized
        if (this._initializingChart) return;

        this.chartData = data;
        this.chart.series[0].setData(this.chartData);
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_StatsMap', DJ.UI.StatsMap);
