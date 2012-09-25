/*!
 * StatsMap
 */

DJ.UI.StatsMap = DJ.UI.Component.extend({

    defaults: {
        debug: false,
        cssClass: 'statMap'
    },

    mapSize: {
        width: 298,             //width of the component 
        height: 198,
        circleMaxRadius: 32,    //The maximum radius that a circle can have
        circleMinRadius: 7      //The minimum radius that a circle can have
    },

    stateCodes: {
        'alabama': 'al',
        'alaska': 'ak',
        'arizona': 'az',
        'arkansas': 'ar',
        'california': 'ca',
        'colorado': 'co',
        'connecticut': 'ct',
        'delaware': 'de',
        'district of columbia': 'dc',
        'florida': 'fl',
        'georgia': 'ga',
        'hawaii': 'hi',
        'idaho': 'id',
        'illinois': 'il',
        'indiana': 'in',
        'iowa': 'ia',
        'kansas': 'ks',
        'kentucky': 'ky',
        'louisiana': 'la',
        'maine': 'me',
        'maryland': 'md',
        'massachusetts': 'ma',
        'michigan': 'mi',
        'minnesota': 'mn',
        'mississippi': 'ms',
        'missouri': 'mo',
        'montana': 'mt',
        'nebraska': 'ne',
        'nevada': 'nv',
        'new hampshire': 'nh',
        'new jersey': 'nj',
        'new mexico': 'nm',
        'new york': 'ny',
        'north carolina': 'nc',
        'north dakota': 'nd',
        'ohio': 'oh',
        'oklahoma': 'ok',
        'oregon': 'or',
        'pennsylvania': 'pa',
        'rhode island': 'ri',
        'south carolina': 'sc',
        'south dakota': 'sd',
        'tennessee': 'tn',
        'texas': 'tx',
        'utah': 'ut',
        'vermont': 'vt',
        'virginia': 'va',
        'washington': 'wa',
        'west virginia': 'wv',
        'wisconsin': 'wi',
        'wyoming': 'wy'
    },

    dataLabelOptions: {
        ak: {
            y: -10
        },
        ca: {
            x: -10,
            y: 20
        },
        dc: {},
        fl: {
            x: 40
        },
        id: {
            y: 40
        },
        hi: {
            color: 'black',
            y: 15
        },
        la: {
            x: -20
        },
        tn: {
            y: 5
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
                    enabled: true,
                    formatter: function (dataLabelOptions) {
                        return this.point.options.key.toUpperCase();
                    },
                    style: {
                        fontWeight: 'bold'
                    }
                },
                valueRanges: [ {
                    color: '#ccc'
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
                if (!this.y || this.y === -1) return false;
                return '<b>' + this.point.name + '</b><br/>Avg: ' + this.y + 's<br/>Min:' + this.point.min + 's<br/>Max:' + this.point.max + 's';
            }
        },

        series: [{ type: 'map', data: [] }]
    },

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: 'StatsMap' }, meta));

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
        $dj.subscribe('data.PageLoadDetailsBySubCountryforCountry', this._delegates.setData);
    },

    _initializeChart: function () {
        for (var i = 1; i < Highcharts.UsMapStates.length; i++) {
            var key = Highcharts.UsMapStates[i];

            this.mapConfig.series[0].data.push({
                key: key,
                path: Highcharts.pathToArray(Highcharts.UsMapShapes[key]),
                dataLabels: this.dataLabelOptions[key] // or undefined
            });
        }

        this.mapConfig.chart.renderTo = this.$element.find('.mapContainer')[0];
        this.chart = new Highcharts.Map(this.mapConfig);

    },

    setData: function (data) {
        if (!data)
            return;

        this.chart.series[0].setData(this._mapData(data));
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

        // from a simple array, map it to an associative array
        // with the state abbreviation as the key. this makes subsequent lookups o(1) operation.
        var stateMap = {};
        _.each(workingSet, function (item) {
            var state = self.stateCodes[item.subcountry_name.toLowerCase()];
            stateMap[state] = {
                name: item.subcountry_name,
                avg: parseFloat((item.Avg / 1000).toFixed(2)),
                min: parseFloat((item.Min / 1000).toFixed(2)),
                max: parseFloat((item.Max / 1000).toFixed(2))
            };
        });

        var chartData = [];
        for (var i = 1, len = Highcharts.UsMapStates.length; i < len; i++) {
            var key = Highcharts.UsMapStates[i],
                stateData = stateMap[key] || {};
            chartData.push({
                key: key,
                path: Highcharts.pathToArray(Highcharts.UsMapShapes[key]),
                dataLabels: this.dataLabelOptions[key], // or undefined
                name: stateData.name,
                y: stateData.avg || -1,
                min: stateData.min,
                max: stateData.max
            });
        }

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
