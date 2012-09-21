/*!
 * StatsMap
 */

DJ.UI.StatsMap = DJ.UI.Component.extend({

    defaults: {
        debug: false,
        cssClass: 'statMap',
    },

    mapSize: {
        width: 298,             //width of the component 
        height: 198,
        circleMaxRadius: 32,    //The maximum radius that a circle can have
        circleMinRadius: 7      //The minimum radius that a circle can have
    },

    knownCoordinates: {
        'new york': {
            name: 'New York',
            x: 860,
            y: 190,
            dataLabels: { align: 'right', y: -14 }
        },
        'los angeles': {
            name: 'Los Angeles',
            x: 100,
            y: 345
        },
        'chicago': {
            name: 'Chicago',
            x: 630,
            y: 210
        }
    },

    mapConfig: {
        chart: {
            type: 'map',
            backgroundColor: 'transparent',
            borderWidth: 0,
            spacingBottom: 0,
            spacingTop: 0
        },

        plotOptions: {
            series: {
                animation: false
            },
            map: {
                dataLabels: {
                    enabled: false,
                }
            },
            scatter: {
                dataLabels: {
                    enabled: true,
                    align: 'left',
                    formatter: function () {
                        return this.point.name;
                    },
                    style: {
                        color: '#666',
                        fontWeight: 'bold',
                    }
                },
                cursor: 'pointer'
            }
        },

        credits: { enabled: false },
        title: { text: null },
        legend: { enabled: false },

        tooltip: {
            formatter: function () {
                if (!this.x) return false;
                return '<b>' + this.key + '</b><br/>Avg: ' + this.point.avg + 's<br/>Min:' + this.point.min + 's<br/>Max:' + this.point.max + 's';
            }
        },

        series: [{ type: 'map', data:[]}, { type: 'scatter'}]
    },

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "StatsMap" }, meta));

        this.setData(this.data);
    },


    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function (ctx) {
        this.$element.html(this.templates.container());

        this._initializeChart();
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.PageLoadDetailsByCityForUS', this._delegates.setData);
    },

    _initializeChart: function () {
        for (var i = 1; i < Highcharts.UsMapStates.length; i++) {
            var state = Highcharts.UsMapStates[i];

            this.mapConfig.series[0].data.push({
                key: state,
                path: Highcharts.pathToArray(Highcharts.UsMapShapes[state])
            });
        }

        this.mapConfig.chart.renderTo = this.$element.find('.mapContainer')[0];
        this.chart = new Highcharts.Map(this.mapConfig);

    },

    setData: function (data) {
        if (!data)
            return;

        this.chart.series[1].setData(this._mapData(data));
    },

    _mapData: function (data) {
        if (!data)
            return;

        var self = this;
        var statsByPages = _.groupBy(data, function (item) {
            return item.page_id;
        });

        // Homepage: Sub
        var workingSet = statsByPages[421139];

        var city, point, chartData = [],
            dataLabels = this.chart.series[1].dataLabels,
            pointData;
        
        _.each(workingSet, function (item) {
            city = item.city_name;
            if (city) {
                point = self.knownCoordinates[city.toLowerCase()];
            }

            if (point) {

                pointData = {
                    name: point.name,
                    x: point.x,
                    y: point.y,
                    avg: (item.Avg / 1000).toFixed(2),
                    min: (item.Min / 1000).toFixed(2),
                    max: (item.Max / 1000).toFixed(2),
                    marker: {
                        symbol: 'url(' + self._getMarker(item.Avg) + ')'
                    }
                };

                // although not as fluent, this check it improves performance by skipping an expensive array copy
                if (point.dataLabels) {
                    pointData.dataLabels = $.extend(true, { }, dataLabels, point.dataLabels);
                }

                chartData.push(pointData);
            }
        });

        return chartData;
    },

    _getMarker: function (num) {
        if (num <= 5000)
            return '<%= WebResource("DowJones.Dash.Components.StatsMap.marker_rounded_yellow_green.png") %>';

        if (num <= 7000)
            return '<%= WebResource("DowJones.Dash.Components.StatsMap.marker_rounded_yellow_orange.png") %>';

        return '<%= WebResource("DowJones.Dash.Components.StatsMap.marker_rounded_red.png") %>';
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_StatsMap', DJ.UI.StatsMap);
