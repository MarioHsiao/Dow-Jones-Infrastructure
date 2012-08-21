
DJ.UI.NewsMatrix = DJ.UI.Component.extend({
    selectors: {
        scrollable: '.scrollable'
    },

    defaults: {
        displayTicker: false,
        hitColor: "#999999",
        hitFont: "\"Lucida Grande\", \"Lucida Sans Unicode\", Arial, Helvetica, sans-serif",
        hitSize: "11",
        noMovementColor: "#BCBCBC",
        negativeMovementColor: "#F98F8B",
        positiveMovementColor: "#4682b4",
        highlightColor: "#ffffb7",
        backgroundColor: "#f0f0f0",
        windowSize: 5,
        chipHeight: 19
    },

    events: {
        matrixItemClicked: 'matrixItemClicked.dj.NewsMatrix',
        dataTransformed: 'dataTransformed.dj.NewsMatrix',
        error: 'error.dj.NewsMatrix'
    },


    init: function (element, meta) {
        var $meta = $.extend({ name: "NewsMatrix" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        // set up some variables
        this.scrollSize = (this.options.windowSize) - 1;

        // calculate and memoize colors for chips based on options
        this._initChipColorMap();
        
        // call databind if we got data from server
        if (this.data) {
            this.bindOnSuccess(this.data, this.options);
        }
    },


    _initializeElements: function (ctx) {
        this.$element.html(this.templates.container());
        this.scrollableContainer = ctx.find('.djWidgetContentList');
    },


    _initializeEventHandlers: function () {
        this.$element.on('click', '.djMatrixItemNode', this._delegates.OnMatrixItemBoxClicked);
    },


    _initializeDelegates: function () {
        this._super();
        $.extend(this._delegates, {
            OnMatrixItemBoxClicked: $dj.delegate(this, this._onMatrixItemBoxClicked)
        });
    },


    _onMatrixItemBoxClicked: function (ev) {
        var $target = $(ev.currentTarget);
        var propname = $target.data("propname");
        var index = $target.data("index");
        var rItem = this.matrix.MatrixItems[propname];
        var rNewsEntity = rItem.NewsEntities[index];
        var data = {
            CompanyName: rItem.CompanyName,
            OwnershipType: rItem.OwnershipType,
            IsNewsCoded: rItem.IsNewsCoded,
            InstrumentReference: rItem.InstrumentReference,
            NewsEntity: rNewsEntity
        };
        this.publish(this.events.matrixItemClicked, data);
    },


    _setScrollable: function () {
        this.scrollableContainer
            .addClass('scrollable')
            .scrollable({
                vertical: true,
                mousewheel: true,
                easing: 'linear'
            });

        var scrollSize = this.scrollSize;
        var windowSize = this.options.windowSize;

        if (scrollSize > 0 && windowSize < this.matrix.MatrixItems.length) {
            $(".djWidgetContentList", this.$element).height((windowSize * this.options.chipHeight) + (windowSize - 1));
        }
        else {
            $(".djWidgetContentList", this.$element).height(((this.matrix.MatrixItems.length) * this.options.chipHeight) + (this.matrix.MatrixItems.length - 1));
        }
    },
    
    _initChipColorMap: function () {
        var options = this.options,
            neutralColor = options.noMovementColor,
            negativeColor = options.negativeMovementColor,
            positiveColor = options.positiveMovementColor,
            baseColor = options.backgroundColor;

        // could be an array but having as an object leaves room for 
        // adding labels in future (e.g. 'less', 'same' etc.)
        this.chipColorMap = {
            0: baseColor,
            1: negativeColor,
            2: neutralColor,
            3: this.shade(positiveColor, .6),
            4: this.shade(positiveColor, .4),
            5: this.shade(positiveColor, .2),
            6: positiveColor
        };
    },

    bindOnSuccess: function (response, params) {
        if (!this.validateResponse(response)) {
            return;
        }

        this.matrix = this.extractCategoriesAndItems(response);
        this.publish(this.events.dataTransformed, this.matrix);

        this.showErrors = false;

        this.renderContent(this.matrix.MatrixItems);

        //this._setScrollable();
    },

    extractCategoriesAndItems: function (data) {
        var categories = this.extractCategories(data[0].NewsEntities);
        var items = this.extractItems(data);

        return {
            MatrixCategories: categories,
            MatrixItems: items
        };
    },

    extractCategories: function (newsEntities) {
        var categories = [];

        for (var k in newsEntities) {
            var radarSearchQuery = newsEntities[k].RadarSearchQuery,
                code = radarSearchQuery.SearchString,
                name = radarSearchQuery.Name;
            categories.push({ fcode: code, name: name });
        }

        // Add the 'All News' category to the list
        categories.push({ fcode: 'ALLNEWS', name: 'All News' });

        return categories;
    },

    extractItems: function (response) {
        var items = [];

        // Loop through each company and add the needed companies info 
        for (var p = 0; p < response.length; p++) {
            var company = response[p];
            company.NewsEntities = this.setNewsData(response[p]);
            items.push(company);
        }

        return items;
    },

    // Loop through each news category of a given company and pull out needed data for the widget
    setNewsData: function (company) {
        var categories = [],
            allNewsDayCount = 0,
            allNewsThreeMonthCount = 0;
        for (var i in company.NewsEntities) {
            var category = company.NewsEntities[i],
                newsVolumes = category.NewsVolumes;
            category.oneDayCount = this.extractCount("Day", newsVolumes);
            category.threeMonthCount = this.extractCount("ThreeMonth", newsVolumes);
            category.threeMonthAvg = this.getAverage(category.threeMonthCount, 89);
            category.growthRate = this.getGrowthRate(category.oneDayCount, category.threeMonthAvg);
            category.growthCode = this.getGrowthCode(category);
            categories.push(category);

            // add up totals for each category to make caculations for the "All News" category
            allNewsDayCount += category.oneDayCount;
            allNewsThreeMonthCount += category.threeMonthCount;
        }

        // Create an 'All News' category for the company by adding up each category
        var allNews = {};
        allNews.Name = "All News";
        allNews.SubjectCode = "ALLNEWS";
        allNews.RadarSearchQuery = { searchString: "ALLNEWS", name: "All News" };
        allNews.oneDayCount = allNewsDayCount;
        allNews.threeMonthCount = allNewsThreeMonthCount;
        allNews.threeMonthAvg = this.getAverage(allNewsThreeMonthCount, 89);
        allNews.growthRate = this.getGrowthRate(allNewsDayCount, allNews.threeMonthAvg);
        allNews.growthCode = this.getGrowthCode(allNews);
        categories.push(allNews);
        return categories;
    },

    // Getting the count of certain category of a certain timeframe
    extractCount: function (timeFrame, newsVolumes) {
        for (var i = 0; i < newsVolumes.length; i++) {
            if (newsVolumes[i].TimeFrame === timeFrame) {
                return newsVolumes[i].HitCount;
            }
        }
        return null;
    },

    // Get the average news items of a category (used to calculate threeMonthAvg)
    getAverage: function (count, days) {
        if (days === 0) {
            return 0;
        }

        var avg = count / days;
        if (avg === 0) {
            return 0;
        } else if (avg >= 0.5) {
            return Math.round(avg);
        } else {
            return 1;
        }
    },

    // Get the growth rate of a category
    getGrowthRate: function (dayCount, avg) {
        if (avg === 0) {
            return 0;
        } else {
            return ((dayCount - avg) / avg);
        }
    },

    // Get a integer code (0-6) to represent the size of growth of a category (used to display icon in widget).
    getGrowthCode: function (category) {
        if (category.oneDayCount === 0) {
            return 0;
        } else if (category.threeMonthAvg === 0) {
            return 5;
        } else if (category.oneDayCount === category.threeMonthAvg) {
            return 2;
        } else if (category.oneDayCount < category.threeMonthAvg) {
            return 1;
        } else if ((category.oneDayCount > category.threeMonthAvg) && (category.growthRate <= 0.5)) {
            return 3;
        } else if ((category.oneDayCount > category.threeMonthAvg) && ((category.growthRate > 0.5) && (category.growthRate <= 2.0))) {
            return 4;
        } else if ((category.oneDayCount > category.threeMonthAvg) && ((category.growthRate > 2.0) && (category.growthRate <= 4.0))) {
            return 5;
        } else {
            return 6;
        }
    },

    validateResponse: function (response) {
        if (!response) {
            this.renderError({ Message: 'Invalid response.' });
            return false;
        }
        if (response.Error) {
            this.renderError(response.Error[0]);
            return false;
        }
        //if (!response.ParentNewsEntities) {
        //    return;
        //}
        return true;
    },

    renderError: function (data) {
        this.publish(this.events.error, data);

        if (showErrors == FALSE) {
            return;
        }
        this.$el.render(this.templates.error());

    },

    renderContent: function (data) {

        var $items = $('.djWidgetItems', this.$element);
        var html = [];
        for (var i in data) {
            html[html.length] = this.templates.success({
                settings: this.options,
                propName: i,
                item: data[i],
                f: { getTicker: this.getTicker }
            });
        }

        $items.html(html.join(""));

        //table and key
        this.drawChips($('.djMatrixItemBox', this.$element));

        // attach tooltips        
        $items.find('.djMatrixItemNode').tooltip({
            showURL: false,
            showBody: " - ",
            delay: 0,
            fade: 250
        });
    },

    drawChips: function (matrixCells) {
        for (var r = 0, len = matrixCells.length; r < len; r++) {
            var cell = $(matrixCells[r]);
            var chipStyle = cell.data('grc');
            cell.css('backgroundColor', this.chipColorMap[chipStyle] || this.options.backgroundColor);
        }
    },

    colorLuminance: function (hex, lum) {
        // validate hex string  
        hex = String(hex).replace(/[^0-9a-f]/gi, '');
        if (hex.length < 6) {
            hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
        }
        lum = lum || 0;
        // convert to decimal and change luminosity  
        var rgb = "#", c, i;
        for (i = 0; i < 3; i++) {
            c = parseInt(hex.substr(i * 2, 2), 16);
            c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);
            rgb += ("00" + c).substr(c.length);
        }
        return rgb;
    },

    shade: function (/* #rrggbb */ color, /* -1 (darkest) — 1 (lightest) */ ratio) {
        if (ratio === 0) return color;

        var darker = ratio < 0,
            difference = Math.round(Math.abs(ratio) * 255) * (darker ? -1 : 1),
            minmax = darker ? Math.max : Math.min,
            edge = darker ? 0 : 255,
            hex = ('' + color).replace(/[^0-9a-f]/gi, '');

        if (hex.length < 6) {
            hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
        }

        // convert to decimal and change saturation
        var rgb = "#", c, i;
        for (i = 0; i < 3; i++) {
            c = minmax(parseInt(hex.substr(i * 2, 2), 16) + difference, edge).toString(16);
            rgb += ("00" + c).substr(c.length); // padleft
        }
        return rgb;
    },

    getTicker: function (item) {
        if (item.instrumentReference.ticker) {
            return item.instrumentReference.ticker;
        } else {
            return item.companyName;
        }
    },

    // misc methods
    extractGrowthRate: function (categories, v) {
        for (var i = 0; i < categories.length; i++) {
            var category = categories[i];
            if (category.radarSearchQuery.searchString == v) {
                return {
                    growthCode: category.growthCode,
                    growthRate: category.growthRate
                };
            }
        }

        return 0;
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_NewsMatrix', DJ.UI.NewsMatrix);
$dj.debug('Registered DJ.UI.NewsMatrix (extends DJ.UI.Component)');