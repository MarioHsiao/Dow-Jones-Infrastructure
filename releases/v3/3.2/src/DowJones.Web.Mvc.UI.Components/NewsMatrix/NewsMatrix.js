
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
        error: 'error.dj.NewsMatrix',
        log: 'log.dj.NewsMatrix'
    },


    init: function (element, meta) {
        var $meta = $.extend({ name: "NewsMatrix" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        // set up some variables
        this.scrollSize = (this.options.windowSize || this.defaults.windowSize) - 1;

        // call databind if we got data from server
        if (this.data) {
            this.bindOnSuccess(this.data, this.options);
        }
    },
    

    _initializeElements: function () {
        this.$element.html(this.templates.container());
    },


    _initializeEventHandlers: function () {
        $('.djMatrixItemNode', this.$element).on('click', this._delegates.OnMatrixItemBoxClicked);
    },
    

    _initializeDelegates: function () {
        this._super();
        $.extend(this._delegates, {
            OnMatrixItemBoxClicked: $dj.delegate(this, this._onMatrixItemBoxClicked),
            DrawChip: $dj.delegate(this, this.drawChip)
        });
    },
    

    _onMatrixItemBoxClicked: function (ev) {
        var $target = $(ev.currentTarget);
        var propname = $target.data("propname");
        var index = $target.data("index");
        var rItem = this.matrix.MatrixItems[propname];
        var rNewsEntity = rItem.newsEntities[index];
        var data = { companyName: rItem.companyName, ownershipType: rItem.ownershipType, isNewsCoded: rItem.isNewsCoded, instrumentReference: rItem.instrumentReference, newsEntity: rNewsEntity };
        this.publish(this.events.matrixItemClicked, data);
    },
    

    _setScrollable: function () {
        $(this.selectors.scrollable, this.$element).scrollable({
            vertical: true,
            mousewheel: true,
            easing: 'linear'
        });
    },


    bindOnSuccess: function (response, params) {
        //this.publish(this.events.dataReceived, response);
        var self = this;

        if (!this.validateResponse(response)) {
            return;
        }

        // After parsing out the necessary data, this is the object we will use to render the widget..remove("highlightCurrent")
        var FinalResults = {
            MatrixCategories: [],
            MatrixItems: []
        };

        // Get a list of news categories to display in the category template
        var items = response[0].newsEntities;
        $dj.debug('what to do about localize()');
        //var local = DJ.Widgets.localize();
        var local = {};
        var localSubjects = local.matrix && local.matrix.subjects ? local.matrix.subjects : false;
        var categoryName;
        for (var k in items) {
            var radarSearchQuery = items[k].radarSearchQuery,
                code = radarSearchQuery.searchString,
                name = radarSearchQuery.name;
            categoryName = localSubjects && localSubjects[code] ? localSubjects[code] : name;
            FinalResults.MatrixCategories.push({ fcode: code, name: categoryName });
        }

        // Add the 'All News' category to the list
        categoryName = localSubjects && localSubjects['ALLNEWS'] ? localSubjects['ALLNEWS'] : 'All News';
        FinalResults.MatrixCategories.push({ fcode: 'ALLNEWS', name: categoryName });

        // Loop through each company and add the needed companies info into the FinalResults object 
        for (var p = 0; p < response.length; p++) {
            var company = response[p];
            company.newsEntities = this.setNewsData(response[p]);

            FinalResults.MatrixItems.push(company);
        }


        this.matrix = FinalResults;
        this.publish(this.events.dataTransformed, FinalResults);
        //this.renderCategories(FinalResults);
        this.showErrors = false;
        this.renderContent(this.matrix.MatrixItems);

        // update the window viewport (windowsize * 15) + (windowsize -1)
        this._setScrollable();
        var scrollSize = this.scrollSize;
        var windowSize = this.options.windowSize;

        if (scrollSize > 0 && windowSize < FinalResults.MatrixItems.length) {
            $(".djWidgetContentList", this.$element).height((windowSize * this.defaults.chipHeight) + (windowSize - 1));
        }
        else {
            $(".djWidgetContentList", this.$element).height(((FinalResults.MatrixItems.length) * this.defaults.chipHeight) + (FinalResults.MatrixItems.length - 1));
        }

        var bgColor = self.options.backgroundColor;
        var hlColor = self.options.highlightColor;

        // When hovering over an individual item
        $(".djWidgetMatrix90 .djMatrixItemNode").hover(function () {
            var index = $(this).attr("data-index");
            $(this).css({ backgroundColor: hlColor });
            $(this).closest(".djWidgetItem").find(".djMatrixItemNode").css({ backgroundColor: hlColor });
            $(this).closest(".djWidgetMatrix90").find("[data-index=" + index + "]").css({ backgroundColor: hlColor });
        }, function () {
            var index = $(this).attr("data-index");
            $(this).css({ backgroundColor: bgColor });
            $(this).closest(".djWidgetItem").find(".djMatrixItemNode").css({ backgroundColor: bgColor });
            $(this).closest(".djWidgetMatrix90").find("[data-index=" + index + "]").css({ backgroundColor: bgColor });
        });        
    },

    // Loop through each news category of a given company and pull out needed data for the widget
    setNewsData: function (company) {
        var categories = [],
            allNewsDayCount = 0,
            allNewsThreeMonthCount = 0;
        for (var i in company.newsEntities) {
            var category = company.newsEntities[i];
            category.oneDayCount = this.extractCount("Day", company.newsEntities[i].newsVolumes);
            category.threeMonthCount = this.extractCount("ThreeMonth", company.newsEntities[i].newsVolumes);
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
        allNews.name = "All News";
        allNews.subjectCode = "ALLNEWS";
        allNews.matrixSearchQuery = { searchString: "ALLNEWS" };
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
            if (newsVolumes[i].timeFrame == timeFrame) {
                return newsVolumes[i].hitCount;
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
        var self = this;
        var matrixTemplate = self.templates.matrix;
        var html = [];
        for (var i in data) {
            html[html.length] = this.templates.success({ settings: self.options, propName: i, item: data[i], templates: { matrix: matrixTemplate }, f: { getTicker: self.getTicker } });
        }


        $items.html(html.join(""));

        var matrixs = $('.djMatrixItemBox', this.$element); //table and key

        for (var r = 0; r < matrixs.length; r++) {
            var matrix = $(matrixs[r]);
            if (matrix && matrix.length) {
                var chipStyle = matrix.data('grc');
                if (chipStyle) {
                    self._delegates.DrawChip(matrix[0], chipStyle);
                }
            }
        }
    },

    drawChip: function (target, chipStyle) {
        var options = this.options;

        var neutralColor = options.noMovementColor,
            negativeColor = options.negativeMovementColor,
            positiveColor = options.positiveMovementColor,
            baseColor = '#fff';

        switch (chipStyle) {
            case 'less':
            case 'djMatrixItemBox1':
            case 1:
                baseColor = negativeColor;
                break;
            case 'same':
            case 'djMatrixItemBox2':
            case 2:
                baseColor = neutralColor;
                break;
            case '50':
            case 'djMatrixItemBox3':
            case 3:
                baseColor = this.shade(positiveColor, .6);
                break;
            case '200':
            case 'djMatrixItemBox4':
            case 4:
                baseColor = this.shade(positiveColor, .4);
                break;
            case '400':
            case 'djMatrixItemBox5':
            case 5:
                baseColor = this.shade(positiveColor, .2);
                break;
            case '400plus':
            case 'djMatrixItemBox6':
            case 6:
                baseColor = positiveColor;
                break;
            default:
                return target;
        }

        target.style.backgroundColor = baseColor;
        return target;

    },


    renderCategories: function (data) {
        var $categories = $('.djCategories', this.$element);

        var self = this;
        var cats = [];

        // need to rework this for ie8 and under.
        for (var i in data.MatrixCategories) {
            var category = data.MatrixCategories[i];
            var catMarkup = self.templates.category({ index: i, settings: self.options, category: category });
            var label = this.drawCategoryLabel($(catMarkup)[0], category.name || category.fcode);
            var labelMarkup = $('<div/>').append(label).html();
            cats.push(labelMarkup);
        }
        cats.reverse();
        $categories.html(cats.join(''));
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

    drawCategoryLabel: function (target, text) {
        var categoryLabel = '<div title="' + text + '">' + text + '</div>';
        var fontSize = this.options.hitSize;
        var fontFamily = this.options.hitFont;
        var tColor = this.colorLuminance(this.options.hitColor || this.defaults.hitColor, 0);

        if (Highcharts.VMLRenderer) {
            $(target).css({
                position: "relative",
                top: '0',
                left: '0',
                zoom: '1',
                filter: 'progid:DXImageTransform.Microsoft.BasicImage(rotation=2)'
            });
            var $categoryLabel = $(categoryLabel);
            $categoryLabel.css({
                position: 'absolute',
                writingMode: 'tb-rl',
                top: '3px',
                height: '143px',
                width: '19px',
                textAlign: 'left',
                lineHeight: '19px',
                clip: 'rect(0px,19px,143px,0px)'
            }).addClass("verticalLabel");

            $(target).css({ backgroundColor: '#FFF' });
            $(target).html($categoryLabel);
            return target;
        }

        else {
            var renderer = new Highcharts.Renderer(target, 19, 143);
            var xPos = 12.5;
            var yPos = 140;

            renderer.text(text, xPos, yPos).attr({
                rotation: 270
            }).css({
                fontSize: fontSize + 'px',
                lineHeight: '19px',
                fontFamily: fontFamily,
                fontWeight: 'normal',
                color: tColor
            }).add();
        }
        return target;
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
            if (category.matrixSearchQuery.searchString == v) {
                return {
                    growthCode: category.growthCode,
                    growthRate: category.growthRate
                };
            }
        }

        return 0;
    },
    validateSettings: function () {
        settings = self.settings; // pull from instance
        if (settings === undefined || !settings) settings = {};
        if (typeof settings.symbology === 'undefined' || !settings.symbology) {
            settings.symbology = 'fii';
        }
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_NewsMatrix', DJ.UI.NewsMatrix);
$dj.debug('Registered DJ.UI.NewsMatrix (extends DJ.UI.Component)');