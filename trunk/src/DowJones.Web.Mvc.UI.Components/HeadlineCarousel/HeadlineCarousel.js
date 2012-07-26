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

    /**
    * jQuery headline carousel
    * 
    * Author: Ron Edgecomb
    * Copyright 2010-2011 Dow Jones & Company Inc.
    *
    * Depends:
    *   jquery.ui.core.js
    *   jquery.ui.widget.js
    *
    */

    $.widget("ui.headlineCarousel", {
        options: {
            height: 64, 							// Carousel Height
            width: 'auto', 						// Fixed Width or Autocalcuated
            scrollLeft: '.scroll-left', 				// Selector for scroll left control
            scrollRight: '.scroll-right', 			// Selector for scroll right control
            scrollDisabledClass: 'disabled', 					// Selector for disabled scroll control
            scrollClickMove: 1, 							// Number of items to move
            //scrollContinuous: 	false,							// Whether or not to scroll as true carousel
            //startPosition: 		0,								// index of LI to start at
            carouselWrap: '.dj_headline_carousel-wrap'	// Selector for element wrapping carousel
        },

        items: [], // Carousel Items

        /**
        * Plugin Initialization. (Executed automatically)
        */
        _init: function () {
            var self = this,
				el = self.element,
				o = self.options;

            $(el).addClass('dj_headline_carousel-widget');

            self.height = o.height;
            self.width = parseInt((o.width == 'auto') ? $(el).find(o.carouselWrap).width() : o.width);

            self._getItems();
            self.render();

        },

        _create: function () {
            var self = this,
				el = self.element,
				o = self.options;

            el.data('originalElement', $(el).html());

            $(el).find(o.scrollLeft).bind('mousedown', function (e) {
                if (!$(this).hasClass(o.scrollDisabledClass)) {
                    self.moveLeft();
                }
            });

            $(el).find(o.scrollRight).bind('mousedown', function (e) {
                if (!$(this).hasClass(o.scrollDisabledClass)) {
                    self.moveRight();
                }
            });

        },

        _getItems: function () {
            var self = this,
				el = self.element,
				o = self.options,
				items = [];

            $(el).find(o.carouselWrap + ' > ul > li').each(function (index) {

                var item = this,
					$item = $(this);

                $item.attr('carouselid', index);

                // Force absolute positioning and prevent white-space wraping while the element width is being calculated.

                $item.css('position', 'absolute');
                $item.css('white-space', 'nowrap');

                items.push({

                    id: index,
                    el: item,
                    $el: $item,
                    width: $item.outerWidth(true),
                    position: index

                });

                // Restore relative positioning.
                $item.css('position', 'relative');

            });

            self.items = items;

        },

        /*
        get the carousel id of the first item in complete view
        */
        _getFirstInView: function () {

            var self = this,
				el = self.element,
				o = self.options,
				firstItem = 0;

            if (self.wrapPosition.left == 0) {

                firstItem = 0;

            } else {

                var leftOffset = Math.abs(self.wrapPosition.left);

                $.each(self.items, function (key, val) {

                    leftOffset -= val.width;
                    if (leftOffset < 5) { //5px tolerance....

                        firstItem = val.position + 1;
                        return false;

                    }

                });

            }

            return firstItem;

        },

        /*
        get the carousel id of the last item in complete view
        */
        _getLastInView: function () {

            var self = this,
				el = self.element,
				o = self.options,
				lastItem = 0;

            var leftOffset = Math.abs(self.wrapPosition.left),
				itemSpace = leftOffset + self.width;

            $.each(self.items, function (key, val) {

                if ((itemSpace - val.width) < -5) { //5px tolerance....

                    lastItem = val.position - 1;
                    return false;

                }
                lastItem = val.position;
                itemSpace -= val.width;

            });

            return lastItem;

        },

        _setCarouselMinWidth: function () {

            var self = this,
				el = self.element,
				o = self.options,
				width = 1; //1px buffer for good measure

            $.each(self.items, function (key, val) {

                var itemWidth = parseInt(val.width);

                if (itemWidth > 0)
                    width += itemWidth;

            });

            $(el).find(o.carouselWrap + ' > ul').css('minWidth', width);

        },

        _getWrapPosition: function () {
            var self = this,
				el = self.element,
				o = self.options;

            return $(el).find(o.carouselWrap + ' > ul').position();

        },

        destroy: function () {
            var self = this,
				el = self.element,
				o = self.options;

            $(el).addClass('dj_headline_carousel-widget').html(el.data('originalElement'));
            $.Widget.prototype.destroy.apply(this, arguments);

        },

        render: function () {
            var self = this,
				el = self.element,
				o = self.options;

            self._setCarouselMinWidth();
            self._updateCarouselData();

        },

        _updateCarouselData: function () {

            var self = this,
				el = self.element,
				o = self.options;

            self.wrapPosition = self._getWrapPosition();

            $(el).find(o.carouselWrap + ' > ul > li').removeClass('first-in-view last-in-view');
            $(o.scrollLeft + ', ' + o.scrollRight, $(el)).removeClass(o.scrollDisabledClass);

            self.firstInView = self._getFirstInView();
            self.$firstInView = $(el).find(o.carouselWrap + ' > ul > li[carouselid="' + self.firstInView + '"]');

            self.lastInView = self._getLastInView();
            self.$lastInView = $(el).find(o.carouselWrap + ' > ul > li[carouselid="' + self.lastInView + '"]');

            self.$firstInView.addClass('first-in-view');
            self.$lastInView.addClass('last-in-view');

            if (self.firstInView == 0) {
                $(el).find(o.scrollLeft).addClass(o.scrollDisabledClass);
            }

            if (self.lastInView == (self.items.length - 1)) {
                $(el).find(o.scrollRight).addClass(o.scrollDisabledClass);
            }

        },

        moveLeft: function () {
            var self = this,
				el = self.element,
				o = self.options;

            if (self.firstInView > 0) {

                var wrapOffset = Math.abs(self.wrapPosition.left),
					firstOffset = self.$firstInView.position().left,
					nextId = ((self.firstInView - o.scrollClickMove) < 0) ? 0 : (self.firstInView - o.scrollClickMove),
					$nextItem = $(el).find(o.carouselWrap + ' > ul > li[carouselid="' + nextId + '"]'),
					moveOffset = parseInt($nextItem.position().left);
                operator = (moveOffset >= 0) ? '-' : '';

                self._moveCarousel(operator + moveOffset);

            }

        },

        moveRight: function () {
            var self = this,
				el = self.element,
				o = self.options;

            if (self.lastInView != (self.items.length - 1)) {
                var wrapOffset = Math.abs(self.wrapPosition.left),
					lastOffset = self.$lastInView.position().left,
					nextId = ((self.lastInView + o.scrollClickMove) > (self.items.length - 1)) ? (self.items.length - 1) : (self.lastInView + o.scrollClickMove),
					$nextItem = $(el).find(o.carouselWrap + ' > ul > li[carouselid="' + nextId + '"]'),
					moveOffset = parseInt(($nextItem.position().left + self.items[nextId].width) - self.width);
                operator = (moveOffset >= 0) ? '-' : '';
                self._moveCarousel(operator + moveOffset);

            }

        },

        _moveCarousel: function (moveOffset) {
            var self = this,
				el = self.element,
				o = self.options;
            $(el).find(o.carouselWrap + ' > ul').animate({
                left: moveOffset
            }, 'slow', function () {
                self._updateCarouselData();
            });

        }

    });
