/*!
 * PageLoadByRegionCanvasModule
 */

DJ.UI.PageLoadByRegionCanvasModule = DJ.UI.AbstractCanvasModule.extend({

    defaults: {
        mapType: 'country',
        map: 'us'
    },

    selectors: {
        pillContainer: '.pillContainer',
        pill: '.dj-pills > li',
        statsMap: '.dj_StatsMap'
    },

    stateCodes: {
        us: { 1871: "Alabama", 1885: "Alaska", 1853: "Arizona", 1879: "Arkansas", 1854: "California", 1884: "Colorado", 1880: "Connecticut", 1881: "Delaware", 1858: "District of Columbia", 1856: "Florida", 1859: "Georgia", 1882: "Hawaii", 1869: "Idaho", 1855: "Illinois", 1870: "Indiana", 1886: "Iowa", 1851: "Kansas", 1900: "Kentucky", 1897: "Louisiana", 1802: "Maine", 1872: "Maryland", 1866: "Massachusetts", 1857: "Michigan", 1868: "Minnesota", 1883: "Mississippi", 1852: "Missouri", 1899: "Montana", 1895: "Nebraska", 1898: "Nevada", 1893: "New Hampshire", 1861: "New Jersey", 1863: "New Mexico", 1865: "New York", 1860: "North Carolina", 1887: "North Dakota", 1877: "Ohio", 1874: "Oklahoma", 1878: "Oregon", 1873: "Pennsylvania", 1890: "Rhode Island", 1896: "South Carolina", 1889: "South Dakota", 1888: "Tennessee", 1862: "Texas", 1867: "Utah", 1892: "Vermont", 1876: "Virginia", 1864: "Washington", 1891: "West Virginia", 1894: "Wisconsin", 1902: "Wyoming" },
        de: { 540: "Baden-Wurttemberg", 548: "Bavaria", 547: "Berlin", 542: "Brandenburg", 549: "Bremen", 550: "Hamburg", 551: "Hesse", 543: "Mecklenburg-Vorpommern", 552: "Lower Saxony", 553: "North Rhine-Westphalia", 554: "Rhineland-Palatinate", 555: "Saarland", 544: "Saxony", 545: "Saxony-Anhalt", 541: "Schleswig-Holstein", 546: "Thuringia" },
        world: { 6: "Afghanistan", 9: "Albania", 62: "Algeria", 15: "American Samoa", 4: "Andorra", 12: "Angola", 7: "Antigua and Barbuda", 14: "Argentina", 10: "Armenia", 18: "Aruba", 17: "Australia", 16: "Austria", 19: "Azerbaijan", 33: "Bahamas, The", 26: "Bahrain", 22: "Bangladesh", 21: "Barbados", 37: "Belarus", 23: "Belgium", 38: "Belize", 28: "Benin", 34: "Bhutan", 31: "Bolivia", 20: "Bosnia and Herzegovina", 36: "Botswana", 32: "Brazil", 30: "Brunei Darussalam", 25: "Bulgaria", 24: "Burkina Faso", 27: "Burundi", 114: "Cambodia", 48: "Cameroon", 39: "Canada", 121: "Cayman Islands", 42: "Central African Republic", 206: "Chad", 47: "Chile", 49: "China", 50: "Colombia", 43: "Congo", 51: "Costa Rica", 45: "Cote d'Ivoire", 97: "Croatia", 52: "Cuba", 55: "Cyprus", 56: "Czech Republic", 59: "Denmark", 58: "Djibouti", 60: "Dominica", 61: "Dominican Republic", 63: "Ecuador", 65: "Egypt, Arab Rep.", 202: "El Salvador", 87: "Equatorial Guinea", 67: "Eritrea", 64: "Estonia", 69: "Ethiopia", 71: "Fiji", 70: "Finland", 75: "France", 76: "Gabon", 84: "Gambia, The", 79: "Georgia", 57: "Germany", 81: "Ghana", 88: "Greece", 83: "Greenland", 78: "Grenada", 90: "Guatemala", 85: "Guinea", 93: "Guyana", 98: "Haiti", 96: "Honduras", 99: "Hungary", 107: "Iceland", 103: "India", 100: "Indonesia", 106: "Iran, Islamic Rep.", 105: "Iraq", 101: "Ireland", 102: "Israel", 108: "Italy", 109: "Jamaica", 111: "Japan", 110: "Jordan", 122: "Kazakhstan", 112: "Kenya", 118: "Korea, Dem. Rep.", 119: "Korea, Rep.", 120: "Kuwait", 113: "Kyrgyz Republic", 123: "Lao PDR", 132: "Latvia", 124: "Lebanon", 129: "Lesotho", 128: "Liberia", 133: "Libya", 130: "Lithuania", 131: "Luxembourg", 139: "Macedonia, FYR", 137: "Madagascar", 151: "Malawi", 153: "Malaysia", 140: "Mali", 148: "Malta", 146: "Mauritania", 152: "Mexico", 136: "Moldova", 135: "Monaco", 142: "Mongolia", 147: "Montenegro", 134: "Morocco", 154: "Mozambique", 141: "Myanmar", 155: "Namibia", 163: "Nepal", 161: "Netherlands", 156: "New Caledonia", 166: "New Zealand", 160: "Nicaragua", 157: "Niger", 159: "Nigeria", 162: "Norway", 167: "Oman", 173: "Pakistan", 168: "Panama", 171: "Papua New Guinea", 180: "Paraguay", 169: "Peru", 172: "Philippines", 174: "Poland", 178: "Portugal", 176: "Puerto Rico", 181: "Qatar", 183: "Romania", 184: "Russian Federation", 185: "Rwanda", 186: "Saudi Arabia", 198: "Senegal", 196: "Sierra Leone", 191: "Singapore", 195: "Slovak Republic", 193: "Slovenia", 187: "Solomon Islands", 199: "Somalia", 238: "South Africa", 68: "Spain", 127: "Sri Lanka", 189: "Sudan", 200: "Suriname", 204: "Swaziland", 190: "Sweden", 44: "Switzerland", 203: "Syria Arab Republic", 210: "Tajikistan", 219: "Tanzania", 209: "Thailand", 208: "Togo", 214: "Tonga", 216: "Trinidad and Tobago", 213: "Tunisia", 215: "Turkey", 212: "Turkmenistan", 221: "Uganda", 220: "Ukraine", 5: "United Arab Emirates", 77: "United Kingdom", 223: "United States", 224: "Uruguay", 225: "Uzbekistan", 232: "Vanuatu", 228: "Venezuela, RB", 231: "Vietnam", 235: "Yemen, Rep.", 3001: "Zaire", 239: "Zambia", 240: "Zimbabwe" },
        id: { 1022: "Aceh", 67752: "Bali", 69378: "Bangka-Belitung", 67587: "Banten", 69559: "Bengkulu", 1032: "Central Java", 68774: "Central Kalimantan", 1026: "Central Sulawesi", 1033: "East Java", 67351: "East Kalimantan", 69438: "East Nusa Tenggara", 68730: "Gorontalo", 68313: "Jakarta", 1030: "Jambi", 68583: "Lampung", 67306: "Maluku", 68797: "North Sulawesi", 67591: "North Sumatra", 196719: "Riau Islands", 69374: "South East Sulawesi", 69220: "South Kalimantan", 67452: "South Sumatra", 1029: "West Java", 68342: "West Kalimantan", 1024: "West Nusa Tenggara", 196718: "West Papua", 1027: "West Sumatra", 1023: "Yogyakarta" },
        apac: { 15: "American_Samoa", 17: "Australia", 22: "Bangladesh", 34: "Bhutan", 104: "British_Indian_Ocean_Te", 30: "Brunei_Darussalam", 114: "Cambodia", 49: "China", 46: "Cook_Islands", 71: "Fiji", 170: "French_Polynesia", 91: "Guam", 103: "India", 100: "Indonesia", 111: "Japan", 115: "Kiribati", 118: "North_Korea", 119: "South_Korea", 123: "Laos", 153: "Malaysia", 150: "Maldives", 138: "Marshall_Islands", 73: "Micronesia", 142: "Mongolia", 141: "Myanmar", 164: "Nauru", 163: "Nepal", 156: "New_Caledonia", 166: "New_Zealand", 144: "Northern_Mariana_Island", 179: "Palau", 171: "Papua_New_Guinea", 172: "Philippines", 184: "Russian_Federation", 234: "Samoa", 191: "Singapore", 187: "Solomon_Islands", 127: "Sri_Lanka", 192: "St_Helena", 189: "Sudan", 209: "Thailand", 214: "Tonga", 235: "Yemen" },
        kr: { 67747: "Incheon", 1179: "Gyeonggi", 67167: "South Gyeongsang", 1177: "Busan", 69406: "South Jeolla", 68540: "Jeju", 69349: "North Jeolla", 69200: "Gwangju", 68054: "South Chungcheong", 68848: "Daejeon", 68554: "North Chungcheong", 1181: "Ulsan", 69648: "Daegu", 1180: "North Gyeongsang", 1178: "Seoul", 68353: "Gangwon" }
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "PageLoadByRegionCanvasModule" }, meta);

        // Call the base constructor
        this._super(element, $meta);
        
        this.currentStateCodes = this.stateCodes[this.options.map];
    },


    _initializeElements: function (ctx) {
        this._super(ctx);
        this.pillContainer = ctx.find(this.selectors.pillContainer);
        this.$statsMap = ctx.find(this.selectors.statsMap);
        this.statsMap = this.$statsMap.findComponent(DJ.UI.StatsMap);
    },


    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData),
            domainChanged: $dj.delegate(this, this._domainChanged)
        });
    },


    _initializeEventHandlers: function () {
        var self = this;
        
        this.$element.on('click', this.selectors.pill, function () {
            var el = $(this);
            el.siblings('.active').add(el).toggleClass('active');
            self.activePillId = el.data('id');
            if (self.chartGroupsData && self.statsMap)
                self.statsMap.setData(self._getChartData(self.chartGroupsData[self.activePillId]));
        });

        
        if (this.options.mapType !== 'world') {
            $dj.subscribe('data.PageLoadDetailsByType', this._delegates.setData);
        }
        else {
            $dj.subscribe('data.PageLoadDetailsByTypeForWorld', this._delegates.setData);
        }

        $dj.subscribe('data.BasicHostConfiguration', this._delegates.domainChanged);
    },


    _domainChanged: function (data) {
        if (!data)
            return;

        if (this.options.mapType !== 'world') {
            this.statsMap._initializeMapData(data.map);
        }

        this.currentStateCodes = this.stateCodes[data.map];

        this.statsMap._initializeChart(data.performanceZones);

        this.activePillId = null;
    },


    setPills: function (pills) {
        this.activePillId = this.activePillId || pills[0].id;

        // set the active item in the dataset before drawing pills
        for (var i = 0; i < pills.length; i++) {
            pills[i].active = this.activePillId === pills[i].id;
        }

        this.pillContainer.html(this.templates.navPills(pills));
    },


    setData: function (data) {
        if (!data || !data.length) {
            this.showMessage("Coming Soon");
            return;
        }

        // get some sensible structure from a flat result set
        var mappedData = this._mapData(data);

        // draw the pills
        this.setPills(mappedData.pills);

        this.chartGroupsData = mappedData.chartGroups;

        // now that we know what the active pill is, draw the corresponding chart
        this.chartData = this._getChartData(mappedData.chartGroups[this.activePillId]);

        this.statsMap.setData(this.chartData);

        this.showContentArea();

    },

    _getChartData: function (workingSet) {
        if (!workingSet)
            return;

        var self = this;

        // from a simple array, map it to an associative array
        // with the state abbreviation as the key. this makes subsequent lookups o(1) operation.
        var stateMap = {};

        if (this.currentStateCodes) {
            _.each(workingSet, function (item) {
                var state = self.currentStateCodes[item.Id];
                stateMap[state] = {
                    name: item.Name,
                    avg: parseFloat((item.Avg / 1000).toFixed(2)),
                    min: parseFloat((item.Min / 1000).toFixed(2)),
                    max: parseFloat((item.Max / 1000).toFixed(2))
                };
            });
        }
        else {
            $dj.warn("Mapping for states to codes not found for country code: '", this.mapSource, "'. Blank map will be shown.");
        }

        var chartData = [];
        for (var i = 0, len = this.statsMap.territories.length; i < len; i++) {
            var key = this.statsMap.territories[i],
                stateData = stateMap[key] || {};
            chartData.push({
                key: key,
                path: this.statsMap.paths[key],
                name: stateData.name,
                y: stateData.avg || -1,
                min: stateData.min,
                max: stateData.max
            });
        }

        return chartData;
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
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_PageLoadByRegionCanvasModule', DJ.UI.PageLoadByRegionCanvasModule);

$dj.debug('Registered DJ.UI.PageLoadByRegionCanvasModule as dj_PageLoadByRegionCanvasModule');
