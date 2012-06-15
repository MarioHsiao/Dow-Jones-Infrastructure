DJ.UI.NewsRadar = DJ.UI.Component.extend({
    selectors: {
        scrollable: '.scrollable'
    },

    options: {
        displayTicker: false,
        hitcolor: "666666",
        hitfont: "Verdana",
        hitsize: "8",
        bgcolor: "",
        windowSize: 6,
        scrollSize: 5
    },
    defaults: {

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

        // call databind if we got data from server
        //$dj.debug('this data', this.data);
        if (this.data)
            this.bindOnSuccess(this.data, this.options);
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
            HandleSort: $dj.delegate(this, this._handleSort)
        });
    },
    _renderContainer: function () {
        this.$element.html(this.templates.container());
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
        var scrollMax = Math.min(this.options.scrollSize, scrollCount - scrollIndex - this.options.scrollSize - 1);
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
            var scrollApi = $(this.selectors.scrollable, this.$element).eq(0).data('scrollable');
            scrollApi.move(-1 * this.options.scrollSize);
        }
        return false;

    },
    _onNextClicked: function () {
        if ($('.djNext', this.$element).hasClass('djEnabled')) {

            var scrollApi = $(this.selectors.scrollable, this.$element).eq(0).data('scrollable');
            var scrollIndex = scrollApi.getIndex();
            var scrollCount = scrollApi.getItems().length;
            var scrollMax = Math.min(this.options.scrollSize, scrollCount - scrollIndex - this.options.scrollSize - 1);
            scrollApi.move(scrollMax);
        }
        return false;
    },
    _onTopClicked: function (duration) {
        if ($('.djTop', this.$element).hasClass('djEnabled')) {
            var scrollApi = $(this.selectors.scrollable, this.$element).eq(0).data('scrollable');
            scrollApi.seekTo(0, duration);
        }
        return false;
    },
    clear: function () {
        showErrors = TRUE;
        provider = NULL;
        radar = {};
        maxItemNews = 0;
        maxCompanyNews = 0;

        DJ.Notify(handleSort).on('widgetSort').forWidget(id).detach();

        $el.empty();
    },
    dispose: function () {
        clear();

        $el.remove();
    },
    requestData: function () {
        var params = {};
        DJ.Util.extend(params, settings);
        params.callback = feedCallback;


        // request the data from the model
        if (params.entityid && (params.subjectid || (params.customquery && params.customqueryname))) {
            if (self.provider) {
                provider = self.provider;
            } else if (!provider) {
                provider = DJ.Data.getProvider("RadarEx");
            }

            provider.getData(feedCallback, params);
        } else {
            this.publish(this.events.log, ['Nothing to request %o', params]);
            return;
        }

        this.publish(this.events.dataRequested, params);
    },
    bindOnSuccess: function (response, params) {
        this.publish(this.events.dataReceived, response);

        if (!this.validateResponse(response)) {
            return;
        }

        // After parsing out the necessary data, this is the object we will use to render the widget.
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
        for (var k in items) {
            var code = items[k].radarSearchQuery.searchString && items[k].radarSearchQuery.searchString;
            var categoryName = localSubjects && localSubjects[code] ? localSubjects[code] : items[k].radarSearchQuery.name;
            FinalResults.RadarCategories.push({ fcode: code, name: categoryName });
        }

        // Add the 'All News' category to the list
        var categoryName = localSubjects && localSubjects['ALLNEWS'] ? localSubjects['ALLNEWS'] : 'All News';
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
        //carousel.addItems(0, FinalResults.RadarItems);

        //carousel.setEof(true);

        this.showErrors = false;

        //this.publish(this.events.viewRendered, $el[0]);

        this.renderContent(FinalResults.RadarItems);
        this._setScrollable();

        // When hovering over an individual item
        $(".djWidgetRadar90 .djRadarItemNode").hover(function () {
            var index = $(this).attr("data-index");
            $(this).addClass("highlight highlightCurrent");
            $(this).closest(".djWidgetItem").find(".djRadarItemNode").addClass("highlight");
            $(this).closest(".djWidgetRadar90").find("[data-index=" + index + "]").addClass("highlight");
        }, function () {
            var index = $(this).attr("data-index");
            $(this).removeClass("highlight highlightCurrent");
            $(this).closest(".djWidgetItem").find(".djRadarItemNode").removeClass("highlight");
            $(this).closest(".djWidgetRadar90").find("[data-index=" + index + "]").removeClass("highlight");
        });

        // When hovering over a category
        $(".djWidgetRadar90 li.djRadarCategory").hover(function () {
            var index = $(this).attr("data-index");
            $(this).addClass("highlight highlightCurrent");
            $(this).closest(".djWidgetRadar90").find("[data-index=" + index + "]").addClass("highlight");
        }, function () {
            var index = $(this).attr("data-index");
            $(this).removeClass("highlight highlightCurrent");
            $(this).closest(".djWidgetRadar90").find("[data-index=" + index + "]").removeClass("highlight");
        });


        $('.djRadarCategory', this.$element).on('click', this._delegates.HandleSort);

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
        //carousel.reset();
        //carousel.addItems(0, radar.RadarItems);
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
            return;
        }
        if (response.Error) {
            this.renderError(response.Error[0]);
            return;
        }
        //if (!response.ParentNewsEntities) {
        //    return;
        //}
        return true;
    },
    // render methods
    setupRender: function () {
        // create main view
        //main = tpls.main({}, ctx, publicInterface);
        //$el.html(main.html());
        //renderHeader(canGoPrev, canGoNext);
        //renderFooter(canGoPrev, canGoNext);

    },
    renderError: function (data) {
        this.publish(this.events.error, publicInterface, data);

        if (showErrors == FALSE) {
            return;
        }
        this.$el.render(this.templates.error());
        //main.hooks('content').filter('[djContent=errors]').empty();
        //main.hooks('content').filter('[djContent=content]').empty();

        //var content = tpls.errors({ Error: data }, ctx, publicInterface);
        //main.hooks('content').filter('[djContent=errors]').append(content.html());
    },
    renderContent: function (data, globalIdx, localIdx) {
        //main.hooks('content').filter('[djContent=errors]').empty();

        //var args = { Item: data, GlobalIdx: globalIdx, LocalIdx: localIdx },
        //    content = tpls.content(args, ctx, publicInterface);


        var $items = $('.djWidgetItems', this.$element);

        var html = '';
        var self = this;
        var radarTemplate = self.templates.radar;
        for (var i in data) {
            html += this.templates.success({ settings: this.options, item: data[i], templates: { radar: radarTemplate} });
        }

        $items.html(html);

        //return content.html();
    },
    renderCategories: function (data) {
        //var $categories = main.hooks('content').filter('[djcontent=categories]');
        var $categories = $('.djCategories', this.$element);
        //var args = { Categories: data.RadarCategories },
        //    content = tpls.categories(args, ctx, publicInterface);

        var self = this;
        var cats = [];
        for (var i in data.RadarCategories) {
            cats.push(self.templates.category({ index: i, settings: self.options, category: data.RadarCategories[i] }));
        }
        cats.reverse();
        $categories.html(cats.join(''));
    },
    renderHeader: function (canGoPrev, canGoNext) {
        var content = tpls.header({
            canGoPrev: canGoPrev,
            canGoNext: canGoNext
        }, ctx, publicInterface);

        main.hooks('content').filter('[djContent=header]').html(content.html());
    },
    renderFooter: function (canGoPrev, canGoNext) {
        var content = tpls.footer({
            canGoPrev: canGoPrev,
            canGoNext: canGoNext
        }, ctx, publicInterface);

        main.hooks('content').filter('[djContent=footer]').html(content.html());
    },
    renderHeaderAndFooter: function (canGoPrev, canGoNext) {
        renderHeader(canGoPrev, canGoNext);
        renderFooter(canGoPrev, canGoNext);
    },
    getTicker: function (Item) {
        if (Item.InstrumentReference.Ticker) {
            return Item.instrumentReference.ticker;
        } else {
            return Item.companyName;
        }
    },
    // custom event handlers
    handleSort: function (ev, args) {
        var fcode = args.data.fcode;

        if (!fcode) {
            return;
        }
        var self = this;
        radar.RadarItems.sort(function (a, b) {
            var rateA = self.extractGrowthRate(a.newsEntities, fcode);
            var rateB = self.extractGrowthRate(b.newsEntities, fcode);

            // if growth code is the same, sort by growth rate
            if (rateB.growthCode == rateA.growthCode) {
                return rateB.growthRate - rateA.growthRate;
            } else {
                return rateB.growthCode - rateA.growthCode;
            }
        });

        carousel.reset();
        carousel.addItems(0, radar.RadarItems);
    },
    // misc methods
    extractGrowthRate: function (categories, v) {
        for (var i = 0; i < categories.length; i++) {
            var category = categories[i];
            if (category.radarSearchQuery.searchString == v) {
                return {
                    growthCode: category.growthCode,
                    growthRate: category.growthRate
                }
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
$.plugin('dj_NewsRadar', DJ.UI.NewsRadar);
$dj.debug('Registered DJ.UI.NewsRadar (extends DJ.UI.Component)');