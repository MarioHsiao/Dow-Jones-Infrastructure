
DJ.UI.NewsRadar = DJ.UI.Component.extend({
    selectors: {
        scrollable: '.scrollable'
    },

    defaults: {
        displayTicker: false,
        hitColor: "#999999",
        hitFont: "\"Lucida Grande\", \"Lucida Sans Unicode\", Arial, Helvetica, sans-serif",
        hitSize: "11",
        noMovementColor: "#DCDCDC",
        negativeMovementColor: "#FF0000",
        positiveMovementColor: "#666666",
        highlightColor: "#ffffb7",
        backgroundColor: "#FFFFFF",
        windowSize: 5,
        chipHeight: 19,
        colors: { WHITE: "#FFFFFF", BLACK: "#000000" }
    },
    
    events: {
        radarItemClicked: 'radarItemClicked.dj.NewsRadar',
        dataTransformed: 'dataTransformed.dj.NewsRadar',
        error: 'error.dj.NewsRadar',
        log: 'log.dj.NewsRadar'
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "NewsRadar" }, meta);

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
        this._renderContainer();
    },

    _initializeEventHandlers: function () {
        $('.djPrev', this.$element).on('click', this._delegates.OnPrevClicked);
        $('.djNext', this.$element).on('click', this._delegates.OnNextClicked);
        $('.djTop', this.$element).on('click', this._delegates.OnTopClicked);
    },

    _initializeDelegates: function () {
        this._super();
        $.extend(this._delegates, {
            OnPrevClicked: $dj.delegate(this, this._onPrevClicked),
            OnNextClicked: $dj.delegate(this, this._onNextClicked),
            OnTopClicked: $dj.delegate(this, this._onTopClicked),
            OnSeek: $dj.delegate(this, this._onSeek),
            HandleSort: $dj.delegate(this, this._handleSort),
            OnRadarItemBoxClicked: $dj.delegate(this, this._onRadarItemBoxClicked),
            DrawChip: $dj.delegate(this, this.drawChip)
        });
    },

    _renderContainer: function () {
        this.$element.html(this.templates.container());
    },

    _onRadarItemBoxClicked: function (ev) {
        var $target = $(ev.currentTarget);
        var propname = $target.data("propname");
        var index = $target.data("index");
        var rItem = this.radar.RadarItems[propname];
        var rNewsEntity = rItem.newsEntities[index];
        var data = { companyName: rItem.companyName, ownershipType: rItem.ownershipType, isNewsCoded: rItem.isNewsCoded,  instrumentReference: rItem.instrumentReference, newsEntity: rNewsEntity };
        this.publish(this.events.radarItemClicked, data);
    },

    _setScrollable: function () {
        $(this.selectors.scrollable, this.$element).scrollable({
            vertical: true,
            mousewheel: true,
            easing: 'linear',
            onSeek: this._delegates.OnSeek
        });
    },

    _onSeek: function () {
        var scrollApi = $(this.selectors.scrollable, this.$element).eq(0).data('scrollable');
        var scrollIndex = scrollApi.getIndex();
        var scrollCount = scrollApi.getItems().length;
        var scrollSize = this.scrollSize;

        var scrollMax = Math.min(scrollSize, scrollCount - scrollIndex - scrollSize - 1);
        if (scrollIndex > 0) {
            $('.djPrev, .djTop', this.$element).addClass('djEnabled');

        } else {
            $('.djPrev, .djTop', this.$element).removeClass('djEnabled');
        }
        if (scrollMax > 0) {
            $('.djNext', this.$element).addClass('djEnabled');
        } else {
            $('.djNext', this.$element).removeClass('djEnabled');
        }
    },

    _onPrevClicked: function () {
        if ($('.djPrev', this.$element).hasClass('djEnabled')) {
            var scrollSize = this.scrollSize;
            var scrollApi = $(this.selectors.scrollable, this.$element).eq(0).data('scrollable');
            var scrollIndex = scrollApi.getIndex();
            if (scrollIndex < scrollSize) {
                scrollApi.seekTo(0);
            } else {
               scrollApi.move(-1 * scrollSize);      
            }        
        }
        return false;

    },

    _onNextClicked: function () {
        if ($('.djNext', this.$element).hasClass('djEnabled')) {
            var scrollSize = this.scrollSize;
            var scrollApi = $(this.selectors.scrollable, this.$element).eq(0).data('scrollable');
            var scrollIndex = scrollApi.getIndex();
            var scrollCount = scrollApi.getItems().length;
            var scrollMax = Math.min(scrollSize, scrollCount - scrollIndex - scrollSize - 1);
            scrollApi.move(scrollMax);
        }
        return false;
    },

    _onTopClicked: function (duration) {
        if ($('.djTop', this.$element).hasClass('djEnabled')) {
            var scrollApi = $(this.selectors.scrollable, this.$element).eq(0).data('scrollable');
            scrollApi.seekTo(0, duration);
            $('.djPrev, .djTop', this.$element).removeClass('djEnabled');
            $('.djNext', this.$element).addClass('djEnabled');
        }
        return false;
    },

    bindOnSuccess: function (response, params) {
        //this.publish(this.events.dataReceived, response);
        var self = this;
        
        if (!this.validateResponse(response)) {
            return;
        }

        // After parsing out the necessary data, this is the object we will use to render the widget..remove("highlightCurrent")
        var FinalResults = {
            RadarCategories: [],
            RadarItems: []
        };

        // Get a list of news categories to display in the category template
        var items = response[0].newsEntities;
        $dj.debug('what to do about localize()');
        //var local = DJ.Widgets.localize();
        var local = {};
        var localSubjects = local.radar && local.radar.subjects ? local.radar.subjects : false;
        var categoryName;
        for (var k in items) {
            var code = items[k].radarSearchQuery.searchString && items[k].radarSearchQuery.searchString;
            categoryName = localSubjects && localSubjects[code] ? localSubjects[code] : items[k].radarSearchQuery.name;
            FinalResults.RadarCategories.push({ fcode: code, name: categoryName });
        }

        // Add the 'All News' category to the list
        categoryName = localSubjects && localSubjects['ALLNEWS'] ? localSubjects['ALLNEWS'] : 'All News';
        FinalResults.RadarCategories.push({ fcode: 'ALLNEWS', name: categoryName });

        // Loop through each company and add the needed companies info into the FinalResults object 
        for (var p = 0; p < response.length; p++) {
            var company = response[p];
            company.newsEntities = this.setNewsData(response[p]);

            FinalResults.RadarItems.push(company);
        }
        
        this.radar = FinalResults;
        this.publish(this.events.dataTransformed, FinalResults);
        this.renderCategories(FinalResults);
        this.showErrors = false;
        this.renderContent(this.radar.RadarItems);
        
        // update the window viewport (windowsize * 15) + (windowsize -1)
        this._setScrollable();
        var scrollSize = this.scrollSize;
        var windowSize = this.options.windowSize || this.defaults.windowSize;
        
        if (scrollSize > 0 && windowSize < FinalResults.RadarItems.length) {
            $(".djWidgetContentList", this.$element).height((windowSize * this.defaults.chipHeight) + (windowSize - 1));
        }
        else {
            $(".djWidgetContentList", this.$element).height(((FinalResults.RadarItems.length) * this.defaults.chipHeight) + (FinalResults.RadarItems.length - 1));
            $(".djActions", this.$element).hide();
        }
        
        var bgColor = self.options.backgroundColor;
        var hlColor = self.options.highlightColor;
        
        this.$element.on({
            mouseenter: function (e) {
                var hlColor = e.data.hlColor;
                var index = $(this).attr("data-index");
                var $this = $(this);
                $(this).css({ backgroundColor: hlColor });
                $(this).closest(".djWidgetItem").find(".djRadarItemNode").css({ backgroundColor: hlColor });
                $(this).closest(".djWidgetRadar90").find("[data-index=" + index + "]").css({ backgroundColor: hlColor });
            },
            mouseleave: function (e) {
                var index = $(this).attr("data-index");
                $(this).css({ backgroundColor: bgColor });
                $(this).closest(".djWidgetItem").find(".djRadarItemNode").css({ backgroundColor: bgColor });
                $(this).closest(".djWidgetRadar90").find("[data-index=" + index + "]").css({ backgroundColor: bgColor });
            }
        }, ".djWidgetRadar90 .djRadarItemNode", { bgColor: self.options.backgroundColor, hlColor: self.options.highlightColor })
        // When hovering over an individual item
        $(".djWidgetRadar90 .djRadarItemNode").hover(function() {
            
        }, function() {
            
        });
        
        // When hovering over an company item
        $(".djWidgetRadar90 .djWidgetItem").hover(function () {
            $(this).css({ backgroundColor: hlColor});
            $(this).children().css({ backgroundColor: hlColor});
        }, function () {
            $(this).css({ backgroundColor: bgColor });
            $(this).children().css({ backgroundColor: bgColor });
        });

        // When hovering over a category
        $(".djWidgetRadar90 li.djRadarCategory").hover(function () {
            var index = $(this).attr("data-index");
            $(this).css({ backgroundColor: hlColor});
            $(this).closest(".djWidgetRadar90").find("[data-index=" + index + "]").css({ backgroundColor: hlColor});
        }, function () {
            var index = $(this).attr("data-index");
            $(this).css({ backgroundColor: bgColor });
            $(this).closest(".djWidgetRadar90").find("[data-index=" + index + "]").css({ backgroundColor: bgColor });
        });
        
        $('.djRadarCategory', this.$element).on('click', this._delegates.HandleSort);
        $('.djRadarItemNode', this.$element).on('click', this._delegates.OnRadarItemBoxClicked);
    },

    _handleSort: function (ev) {
        var fcode = $(ev.currentTarget).data('fcode');
        if (!fcode) {
            return;
        }
        var self = this;
        this.radar.RadarItems.sort(function (a, b) {
            var rateA = self.extractGrowthRate(a.newsEntities, fcode);
            var rateB = self.extractGrowthRate(b.newsEntities, fcode);

            // if growth code is the same, sort by growth rate
            if (rateB.growthCode == rateA.growthCode) {
                return rateB.growthRate - rateA.growthRate;
            } else {
                return rateB.growthCode - rateA.growthCode;
            }
        });

        this.renderContent(this.radar.RadarItems);
        this._onTopClicked(0);
        $('.djRadarItemNode', this.$element).on('click', this._delegates.OnRadarItemBoxClicked);
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
        allNews.radarSearchQuery = { searchString: "ALLNEWS" };
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
        var radarTemplate = self.templates.radar;
        var html = [];
        for (var i in data) {
            html[html.length] = this.templates.success({ settings: self.options, propName: i, item: data[i], templates: { radar: radarTemplate}, f: { getTicker: self.getTicker} });
        }


        $items.html(html.join(""));

        var radars = $('.djRadarItemBox', this.$element); //table and key

        for (var r = 0; r < radars.length; r++) {
            var radar = $(radars[r]);
            if (radar && radar.length) {
                var chipStyle = radar.data('grc');
                if (chipStyle) {
                    self._delegates.DrawChip(radar[0], chipStyle);
                }
            }
        }
    },

    drawChip: function (target, chipStyle) {
        var self = this,
            options = self.options,
            defaults = self.defaults;
        
        var chipRenderer = new Highcharts.Renderer(target, 19, 19),
            neutralColor = options.noMovementColor || defaults.noMovementColor,
            negativeColor = options.negativeMovementColor ||  defaults.negativeMovementColor,
            positiveColor = options.positiveMovementColor ||  defaults.positiveMovementColor,
            baseColor;
        
        switch (chipStyle) {
            case 'less':
            case 'djRadarItemBox1':
            case 1:
                baseColor = this.colorLuminance(negativeColor, 0);
                chipRenderer.rect(5, 9, 9, 1).attr({
                    fill: baseColor
                }).add();
                break;
            case 'same':
            case 'djRadarItemBox2':
            case 2:
                baseColor = this.colorLuminance(neutralColor, 0);
                chipRenderer.circle(9.5, 9.5, 2.5).attr({
                    fill: baseColor
                }).add();
                break;
            case '50':
            case 'djRadarItemBox3':
            case 3:
                baseColor = this.colorLuminance(positiveColor, .6);
                chipRenderer.circle(9.5, 9.5, 4).attr({
                    fill: 'none',
                    stroke: baseColor,
                    'stroke-width': 1
                }).add();
                break;
            case '200':
            case 'djRadarItemBox4':
            case 4:
                baseColor = this.colorLuminance(options.positiveMovementColor, .4);
                chipRenderer.circle(9.5, 9.5, 6).attr({
                    fill: 'none',
                    stroke: baseColor,
                    'stroke-width': 1
                }).add();
                chipRenderer.circle(9.5, 9.5, 2.5).attr({
                    fill: baseColor
                }).add();
                break;
            case '400':
            case 'djRadarItemBox5':
            case 5:
                baseColor = this.colorLuminance(positiveColor, .2);
                chipRenderer.circle(9.5, 9.5, 6.5).attr({
                    fill: baseColor
                }).add();
                break;
            case '400plus':
            case 'djRadarItemBox6':
            case 6:
                baseColor = this.colorLuminance(positiveColor, 0);
                chipRenderer.circle(9.5, 9.5, 6.5).attr({
                    fill: baseColor
                }).add();
                //horiz
                chipRenderer.rect(6, 9, 7, 1).attr({
                    fill: defaults.colors.WHITE
                }).add();
                //vert
                chipRenderer.rect(9, 6, 1, 7).attr({
                    fill: defaults.colors.WHITE
                }).add();
                break;
            default:
                break;
        }
        return target;

    },
    


    renderCategories: function (data) {
        var $categories = $('.djCategories', this.$element);

        var self = this;
        var cats = [];
        
        // need to rework this for 1e8 and under.
        for (var i in data.RadarCategories) {
            var category = data.RadarCategories[i];
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
            hex = hex[0]+hex[0]+hex[1]+hex[1]+hex[2]+hex[2];  
        }  
        lum = lum || 0;  
        // convert to decimal and change luminosity  
        var rgb = "#", c, i;  
        for (i = 0; i < 3; i++) {  
            c = parseInt(hex.substr(i*2,2), 16);  
            c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);  
            rgb += ("00"+c).substr(c.length);  
        }  
        return rgb;  
    },
    
    drawCategoryLabel: function (target, text) {
        var categoryLabel = '<div title="' + text + '">' + text + '</div>';
        var fontSize = this.options.hitSize || this.defaults.hitSize;
        var fontFamily = this.options.hitFont || this.defaults.hitFont;
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
$.plugin('dj_NewsRadar', DJ.UI.NewsRadar);
$dj.debug('Registered DJ.UI.NewsRadar (extends DJ.UI.Component)');