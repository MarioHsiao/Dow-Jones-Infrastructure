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
    
    selectors: {
        pillContainer: '.pillContainer',
        pill: '.dj-pills > li',
        noDataContainer: '.noData',
        contentContainer: '.content'
    },

    stateCodes: {
        us: { 1871: "al", 1885: "ak", 1853: "az", 1879: "ar", 1854: "ca", 1884: "co", 1880: "ct", 1881: "de", 1858: "dc", 1856: "fl", 1859: "ga", 1882: "hi", 1869: "id", 1855: "il", 1870: "in", 1886: "ia", 1851: "ks", 1900: "ky", 1897: "la", 1802: "me", 1872: "md", 1866: "ma", 1857: "mi", 1868: "mn", 1883: "ms", 1852: "mo", 1899: "mt", 1895: "ne", 1898: "nv", 1893: "nh", 1861: "nj", 1863: "nm", 1865: "ny", 1860: "nc", 1887: "nd", 1877: "oh", 1874: "ok", 1878: "or", 1873: "pa", 1890: "ri", 1896: "sc", 1889: "sd", 1888: "tn", 1862: "tx", 1867: "ut", 1892: "vt", 1876: "va", 1864: "wa", 1891: "wv", 1894: "wi", 1902: "wy" },
        de: {540: "baden-wurttemberg", 548: "bavaria", 547: "berlin", 542: "brandenburg", 549: "bremen", 550: "hamburg", 551: "hesse", 543: "mecklenburg-vorpommern", 552: "niedersachsen", 553: "nordrhein-westfalen", 554: "rheinland-pfalz", 555: "saarland", 544: "sachsen", 545: "sachsen-anhalt", 541: "schleswig-holstein", 546: "thuringia" }
    },
    
    dataLabelOptions: {
        ak: { y: -10 },
        ca: { x: -10, y: 20 },
        fl: { x: 40 },
        id: { y: 40 },
        hi: { color: 'black', y: 15 },
        la: { x: -20 },
        tn: { y: 5 }
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
                if (!this.y || this.y === -1) return false;
                return '<b>' + this.point.name + '</b><br/>Avg: ' + this.y + 's<br/>Min:' + this.point.min + 's<br/>Max:' + this.point.max + 's';
            }
        },

        series: [{ type: 'map', data: [] }]
    },

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: 'StatsMap' }, meta));

        this._initializeChartProps();
        this._initializeChart();
        
        this._showContent();
        if (this.data) {
            this.setData(this.data);
        }
    },


    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData),
            domainChanged: $dj.delegate(this, this._domainChanged)
        });
    },

    _initializeElements: function (ctx) {
        this.$element.html(this.templates.container());
        this.pillContainer = ctx.find(this.selectors.pillContainer);

    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.PageLoadDetailsBySubCountryforCountry', this._delegates.setData);
        $dj.subscribe('comm.domain.changed', this._delegates.domainChanged);
        
        var self = this;
        this.$element.on('click', this.selectors.pill, function () {
            var el = $(this);
            el.siblings('.active').add(el).toggleClass('active');
            self.activePillId = el.data('id');
            if (self.chartGroupsData)
                self.chart.series[0].setData(self._getChartData(self.chartGroupsData[self.activePillId]));
        });
    },
    
    _initializeChartProps: function(map){
        map = map || 'us';
        this.country = Highcharts.Maps[map];
        this.states = this.country.states;
        this.paths = this.country.paths;
        this.stateCodes.current = this.stateCodes[map];

        this.activePillId = null;
    },

    _domainChanged: function (data) {
        if (!data)
            return;

        this.domain = data;
        
        this._initializeChartProps(data.map);
        this._initializeChart();
    },
       
    _initializeChart: function () {
        this._initializingChart = true;
        
        // highcharts will wipe out series object, hence initialize this
        this.mapConfig.series = this.mapConfig.series || [{ type: 'map', data: [] }];
        
        for (var i = 0; i < this.states.length; i++) {
            var key = this.states[i];

            this.mapConfig.series[0].data.push({
                key: key,
                path: this.paths[key],
                dataLabels: this.dataLabelOptions[key] // or undefined
            });
        }

        // blank out previous maps
        this.$element.find('.mapContainer').html('');
        
        this.mapConfig.chart.renderTo = this.$element.find('.mapContainer')[0];
        
        if (this.chart) this.chart.destroy();
        
        this.chart = new Highcharts.Map(this.mapConfig);
        
        this._initializingChart = false;
    },

    setData: function (data) {
        // don't bother until chart is initialized
        if (this._initializingChart) return;
        
        if (!data || !data.length) {
            this._showComingSoon();
            return;
        }

        this._showContent();

        // get some sensible structure from a flat result set
        var mappedData = this._mapData(data);

        // draw the pills
        this.setPills(mappedData.pills);

        this.chartGroupsData = mappedData.chartGroups;

        // now that we know what the active pill is, draw the corresponding chart
        var chartData = this._getChartData(mappedData.chartGroups[this.activePillId]);
        this.chart.series[0].setData(chartData);
    },

    setPills: function (pills) {
        this.activePillId = this.activePillId || pills[0].id;

        // set the active item in the dataset before drawing pills
        for (var i = 0; i < pills.length; i++) {
            pills[i].active = this.activePillId === pills[i].id;
        }
        
        this.pillContainer.html(this.templates.navPills(pills));
    },

    _getChartData: function (workingSet) {
        if (!workingSet)
            return;

        var self = this;
        
        // from a simple array, map it to an associative array
        // with the state abbreviation as the key. this makes subsequent lookups o(1) operation.
        var stateMap = {};
        _.each(workingSet, function (item) {
            var state = self.stateCodes.current[item.subcountry_id];
            stateMap[state] = {
                name: item.subcountry_name,
                avg: parseFloat((item.Avg / 1000).toFixed(2)),
                min: parseFloat((item.Min / 1000).toFixed(2)),
                max: parseFloat((item.Max / 1000).toFixed(2))
            };
        });

        var chartData = [];
        for (var i = 0, len = this.states.length; i < len; i++) {
            var key = this.states[i],
                stateData = stateMap[key] || {};
            chartData.push({
                key: key,
                path: this.paths[key],
                dataLabels: this.dataLabelOptions[key], // or undefined
                name: stateData.name,
                y: stateData.avg || -1,
                min: stateData.min,
                max: stateData.max
            });
        }

        return chartData;
    },

    _getfirstKey: function (data) {
        for (var prop in data)
            if (data.propertyIsEnumerable(prop))
                return prop;
    },

    _mapData: function (data) {
        if (!data || !data.length)
            return;

        var groups = _.groupBy(data, function (item) {
            return item.page_id;
        });

        var pills = _.map(groups, function (item) {
            return { id: item[0].page_id, name: item[0].page_name };
        });

        return {
            pills: pills,
            chartGroups: groups
        };
    },

    _showComingSoon: function () {
        this.$element.find(this.selectors.contentContainer).hide('fast');
        this.$element.find(this.selectors.noDataContainer).show('fast');
    },

    _showContent: function () {
        this.$element.find(this.selectors.contentContainer).show('fast');
        this.$element.find(this.selectors.noDataContainer).hide('fast');
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
