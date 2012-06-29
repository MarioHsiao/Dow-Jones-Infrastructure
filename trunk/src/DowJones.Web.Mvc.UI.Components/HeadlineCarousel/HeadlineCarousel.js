/*!
* HeadlineCarousel
*/

    DJ.UI.HeadlineCarousel = DJ.UI.Component.extend({

        templates: {},

        // Default options
        defaults: {
            debug: false,
            cssClass: 'HeadlineCarousel',
            mode: 'Video',
            orientation: 'Horizontal'
        },

        carouselSettings: {
            display: '1',
            axis: 'x'
        },

        // Localization/Templating tokens
        tokens: {
            noResultsTkn: "<%= Token('noResults')%>",
            articlesLabelTkn: "<%= Token('articlesLabel')%>"
        },

        events: {
            headlineClick: "headlineClick.dj.HeadlineCarousel"
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "HeadlineCarousel" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            this.$element = $(element);

            this.bindOnSuccess();
        },

        bindOnSuccess: function (data) {
            this.$element.html("");
            if (data) {

                //Set Data Template (Video|NewsStandTicker)
                this.setDataTemplate();

                //Bind data depending on the mode (video|ticker)
                switch (this.options.mode.toLowerCase()) {
                    case "video":
                        data = data.resultSet;
                        this.bindVideoData(data);
                        break;
                    case "ticker":
                        this.bindTickerData(data);
                        break;
                }
            }
        },

        setDataTemplate: function () {
            if (this.options.mode.toLowerCase() === "ticker") {
                if (this.options.packageType.toLowerCase() === "headlinehitcountspackage") {
                    this.templates.success = this.templates.headlineHitCountsPackage;
                }
                else {
                    this.templates.success = this.templates.discoveredEntitiesPackage;
                }
            } else {
                if ((this.options.mode.toLowerCase() === "video") && (this.options.orientation.toLowerCase() === "vertical")) {
                    this.carouselSettings.axis = 'y';
                    this.templates.success = this.templates.videoY;
                }
                else {
                    this.carouselSettings.axis = 'x';
                    this.templates.success = this.templates.videoX;
                }
            }
        },

        bindVideoData: function (data) {
            if (data && data.count && data.count.value > 0) {
                // call to bind and append html to ul in one shot
                this.$element.append(this.templates.success({ data: data }));

                // bind events and perform other wiring up
                this._initializeHeadlineCarousel(data.headlines);
            }
            else {
                this.bindOnNoResultsData();
            }
        },

        bindTickerData: function (data) {
            if (data && data.newsstandHeadlineHitCounts && data.newsstandHeadlineHitCounts.length > 0) {
                // call to bind and append html to ul in one shot
                this.$element.append(this.templates.success({
                    headlines: data.newsstandHeadlineHitCounts
                }));

                // bind events and perform other wiring up
                this._initializeHeadlineCarousel(data.newsstandHeadlineHitCounts);
            }
            else
                if (data && data.topNewsVolumeEntities && data.topNewsVolumeEntities.length > 0) {
                    // call to bind and append html to ul in one shot
                    this.$element.append(this.templates.success({
                        headlines: data.topNewsVolumeEntities
                    }));
                    // bind events and perform other wiring up
                    this._initializeHeadlineCarousel(data.topNewsVolumeEntities);
                }
                else {
                    this.bindOnNoResultsData();
                }
        },

        bindOnNoResultsData: function () {
            // bind the template
            this.$element.append(this.templates.noData());
            if (!this.options.displayNoResultsToken) {
                var no_results = $("span.dj_noResults", this.$element).get(0);
                if (no_results) {
                    $(no_results).hide();
                }
            }
        },

        bindOnError: function (data) {
            //this.$element.html("");
            //this.$element.append(this.templates.error(data));
            if(data)
                this.$element.html($dj.formatError(data.returnCode, data.statusMessage));
        },

        getSuccessTemplate: function () {
            return this.templates.success;
        },

        setSuccessTemplate: function (markup) {
            this.templates.success = _.template(markup);
        },

        getNoDataTemplate: function () {
            return this.templates.noData;
        },

        setNoDataTemplate: function (markup) {
            this.templates.noData = _.template(markup);
        },

        getErrorTemplate: function () {
            return this.templates.error;
        },

        setErrorTemplate: function (markup) {
            this.templates.error = _.template(markup);
        },

        setData: function (headlineData) {
            this.bindOnSuccess(headlineData);
        },

        _initializeHeadlineCarousel: function (data) {
            var items;
            var me = this;

            if ((this.options.mode.toLowerCase() === "video") && (this.options.orientation.toLowerCase() === "horizontal") && (this.carouselSettings.axis == 'x')) {
                items = $("div.dj_video_carousel-wrap ul li", this.$element);
                var $videoCarousel = $('.dj_video_carousel', this.$element);
                $videoCarousel.headlineCarousel({
                    height: 225,
                    width: 'auto',
                    carouselWrap: '.dj_video_carousel-wrap'
                });

                if($.iDevices.iPad){
                    $(".dj_video_carousel-wrap", this.$element).touchwipe({
                        wipeLeft: function() {
                            $videoCarousel.headlineCarousel('moveRight');
                        },
                        wipeRight: function() {
                            $videoCarousel.headlineCarousel('moveLeft');
                        },
                        preventDefaultEvents: false
                    });
                }

            } else {
                items = $("div.dj_headline_carousel-wrap ul li h3", this.$element);
                this.carouselSettings.display = this.options.display;
                $('.dj_headline_carousel', this.$element).headlineCarousel();
            }

            $.each(data, function (i, headline) {
                me._initializeHeadline(items, headline, i);
            });
        },

        _initializeHeadline: function (items, headline, i) {
            var item = items.eq(i), title = '';
            
            if(item.hasClass("noArticle")){
                return;
            }

            // Set the data to the li
            item.data("headline", headline);
            if (headline) {
                if (headline.sourceTitle)
                    title = headline.sourceTitle;
                if (headline.sectionTitle)
                    title = title ? title + " - " + headline.sectionTitle : headline.sectionTitle;
                if (headline.descriptor)
                    title = headline.descriptor;

                if (title)
                    item.data("title", title);

                if (headline.hitCount || headline.hitCount == 0)
                    item.data("hitCount", headline.hitCount);
                else if (headline.currentTimeFrameNewsVolume)
                    item.data("hitCount", headline.currentTimeFrameNewsVolume.value);
            }

            
            this._addHeadlineEvents(item);
        },

        _addHeadlineEvents: function (item) {
            var $parentContainer = this.$element;
            var self = this;
            $(item).click(function (e) {
                var $this = $(this);
                $parentContainer.triggerHandler(self.events.headlineClick, { headline: $this.data("headline"), element: this, title: $this.data("title"), hitCount: $this.data("hitCount") });
                e.stopPropagation();
                return false;
            });
        }

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_HeadlineCarousel', DJ.UI.HeadlineCarousel);
