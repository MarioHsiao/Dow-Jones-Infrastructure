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
        us: { 1871: "Alabama", 1885: "Alaska", 1853: "Arizona", 1879: "Arkansas", 1854: "California", 1884: "Colorado", 1880: "Connecticut", 1881: "Delaware", 1858: "District of Columbia", 1856: "Florida", 1859: "Georgia", 1882: "Hawaii", 1869: "Idaho", 1855: "Illinois", 1870: "Indiana", 1886: "Iowa", 1851: "Kansas", 1900: "Kentucky", 1897: "Louisiana", 1802: "Maine", 1872: "Maryland", 1866: "Massachusetts", 1857: "Michigan", 1868: "Minnesota", 1883: "Mississippi", 1852: "Missouri", 1899: "Montana", 1895: "Nebraska", 1898: "Nevada", 1893: "New Hampshire", 1861: "New Jersey", 1863: "New Mexico", 1865: "New York", 1860: "North Carolina", 1887: "North Dakota", 1877: "Ohio", 1874: "Oklahoma", 1878: "Oregon", 1873: "Pennsylvania", 1890: "Rhode Island", 1896: "South Carolina", 1889: "South Dakota", 1888: "Tennessee", 1862: "Texas", 1867: "Utah", 1892: "Vermont", 1876: "Virginia", 1864: "Washington", 1891: "West Virginia", 1894: "Wisconsin", 1902: "Wyoming" },
        de: {540: "Baden-Wurttemberg", 548: "Bavaria", 547: "Berlin", 542: "Brandenburg", 549: "Bremen", 550: "Hamburg", 551: "Hesse", 543: "Mecklenburg-Vorpommern", 552: "Lower Saxony", 553: "North Rhine-Westphalia", 554: "Rhineland-Palatinate", 555: "Saarland", 544: "Saxony", 545: "Saxony-Anhalt", 541: "Schleswig-Holstein", 546: "Thuringia" }
    },
    
    dataLabelOptions: {
        Alaska: { y: -10 },
        California: { x: -10, y: 20 },
        Florida: { x: 40 },
        Idaho: { y: 40 },
        Hawaii: { color: 'black', y: 15 },
        Louisiana: { x: -20 },
        Tennessee: { y: 5 }
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
                if (!this.y || this.y === -1) return '<b>' + this.point.key + '</b><br/>No Data';
                return '<b>' + this.point.key + '</b><br/>Avg: ' + this.y + 's<br/>Min:' + this.point.min + 's<br/>Max:' + this.point.max + 's';
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
                dataLabels: this.dataLabelOptions[key] // Oregon undefined
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
                dataLabels: this.dataLabelOptions[key], // Oregon undefined
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
