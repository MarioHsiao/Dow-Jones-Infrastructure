/*!
 * CurrentRegionalMap
 */

DJ.UI.CurrentRegionalMap = DJ.UI.Component.extend({

    mapConfig: {
        chart: {
            type: 'map',
            backgroundColor: 'transparent',
            borderWidth: 0
        },

        tooltip: { enabled: true },

        plotOptions: {
            series: {
                animation: false
            },
            map: {
                color: 'rgba(148,198,219, 1)',
                borderColor: 'rgba(148,198,219, 1)'
            },

        },

        credits: { enabled: false },
        title: { text: null },
        legend: { enabled: false },

        series: [
            {
                id: 'map',
                type: 'map',
                data: []
            },
            {
                id: 'bubbles',
                type: 'scatter',
                marker: {
                    symbol: 'circle',
                    fillColor: 'yellow',
                    lineColor: 'rgba(24,90,169,.75)',
                    lineWidth: 1,
                    color: 'rgba(24,90,169,1)',
                    states: {
                        hover: {
                            enabled: false
                        }
                    }
                }
            }
        ]
    },

    regions: {
        NAMZ: {
            id: "NAMZ",
            name: "<%= Token('northAmerica') %>",
            posX: 1663,
            posY: 2702
        },
        /*CAMZ: {
            id: "CAMZ",
            name: "<%= Token('centralAmerica') %>",
            posX: 21.5,
            posY: 52.27
        },
        SAMZ: {
            id: "SAMZ",
            name: "<%= Token('southAmerica') %>",
            posX: 31.5,
            posY: 75.09
        },
        EURZ: {
            id: "EURZ",
            name: "<%= Token('europe') %>",
            posX: 50.25,
            posY: 32.55
        },
        MEASTZ: {
            id: "MEASTZ",
            name: "<%= Token('middleEast') %>",
            posX: 57.75,
            posY: 44.186
        },
        ASIAZ: {
            id: "ASIAZ",
            name: "<%= Token('asia') %>",
            posX: 82,
            posY: 32,
            tooltipAlign: 'left'
        },
        AUSNZ: {
            id: "AUSNZ",
            name: "<%= Token('countryName9Aus') %>",
            posX: 87.125,
            posY: 81,
            tooltipAlign: 'left'
        },
        RUSS: {
            id: "RUSS",
            name: "<%= Token('s2regionRussia') %>",
            posX: 70,
            posY: 16,
            tooltipAlign: 'left'
        },*/
        AFRICAZ: {
            id: "AFRICAZ",
            name: "<%= Token('africa') %>",
            posX: 5127,
            posY: 3494
        },/*
        INDSUBZ: {
            id: "INDSUBZ",
            name: "<%= Token('countryName9Ind') %>",
            posX: 70,
            posY: 46,
            tooltipAlign: 'left'
        }*/
    },

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "CurrentRegionalMap" }, meta));

        this._initializeChart();
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
        });
    },

    _initializeElements: function () {
        this.mapContainer = this.$element.find('.mapContainer');
    },

    _initializeEventHandlers: function () {
    },

    _initializeChart: function () {
        this._initializingChart = true;

        var map = Highcharts.Maps['world'];
        for (var i = 0; i < map.territories.length; i++) {
            var key = map.territories[i];

            this.mapConfig.series[0].data.push({
                key: key,
                path: map.paths[key]
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
        this.mapConfig.chart.renderTo = this.mapContainer[0];

        // 2% of diagonal length
        this.minMarkerRadius = this._getDiagonalLength(this.mapConfig.chart.height, this.mapConfig.chart.width) * 0.02;

        // 9% of diagonal length
        this.maxMarkerRadius = this._getDiagonalLength(this.mapConfig.chart.height, this.mapConfig.chart.width) * 0.07;

        if (this.chart) this.chart.destroy();

        this.chart = new Highcharts.Map(this.mapConfig);

        this._initializingChart = false;
    },

    setData: function (data) {
        if (!data || !data.regionNewsVolume)
            return;

        var chartData = this.mapChartData(data.regionNewsVolume);
        this.chart.series[1].setData(chartData);
    },

    mapChartData: function (regionNewsVolume) {
        if (!regionNewsVolume)
            return;

        var chartData = [], regionConfig, region;

        var maxVolume = _.max(regionNewsVolume, function (r) {
            return r.currentTimeFrameNewsVolume.value;
        }).currentTimeFrameNewsVolume.value;

        for (var i = 0, len = regionNewsVolume.length; i < len; i++) {
            region = regionNewsVolume[i];

            if (region.code &&
                region.currentTimeFrameNewsVolume.value > 0) {

                regionConfig = this.regions[region.code];

                if (regionConfig) {
                    var radius = this._getSafeRadius(region.currentTimeFrameNewsVolume.value, maxVolume);
                    
                    chartData.push({
                        //x: this.chart.xAxis[0].translate(regionConfig.posX, true) - radius,
                        //y: this.chart.yAxis[0].translate(regionConfig.posY, true) - radius,
                        x: regionConfig.posX,
                        y: regionConfig.posY,
                        marker: {
                            radius: radius
                        }
                    });

                } else {
                    $dj.debug("CurrentRegionalMap:: Region code -", region.code, "- is not found in config");
                }
            }
        }
        $dj.warn(chartData);
        return chartData;
    },

    _getDiagonalLength: function (height, width) {
        return Math.round(Math.sqrt((height * height + width * width)) * 10000) / 10000;
    },

    _getSafeRadius: function (curValue, maxValue) {
        // return a value that is not less than minimum radius and not more than maxRadius
        return Math.min(
                Math.max(this.minMarkerRadius, (curValue * this.maxMarkerRadius) / maxValue),
                this.maxMarkerRadius);
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_CurrentRegionalMap', DJ.UI.CurrentRegionalMap);
