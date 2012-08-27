/*
*  Portal Headline List Control
*/

    DJ.UI.PortalHeadlineList = DJ.UI.Component.extend({

        selectors: {
            source: 'span.source-clickable',
            author: 'span.article-clickable',
            noResultSpan: 'span.dj_noResults',
            headline: 'a.article-view-trigger',
            headlineEntry: 'li.dj_entry',
            headlineList: '.article-list'
        },

        defaults: {
            displayHeadlineTooltip: false,
            displayNoResultsToken: true,
            displaySnippets: 3,
            extension: "",
            layout: 0,
            maxNumHeadlinesToShow: 5,
            truncationType: 0,
            allowPagination: false,
            circularPaging: false,
            pageSize: 5,            
            pageDirection: 'vertical',
            pageSpeed: 500,
            pagePrevSelector: '.prev',
            pageNextSelector: '.next'
        },

        events: {
            headlineClick: "headlineClick.dj.PortalHeadlineList",
            sourceClick: "sourceClick.dj.PortalHeadlineList",
            authorClick: "authorClick.dj.PortalHeadlineList",
            
            // 08/24/2012 NN - these 2 events are currently only thrown when pagination is on right now
            // dataAssociated - returns data object
            dataAssociated: "dataAssociated.dj.PortalHeadlineList",
            // pageIndexChanged throws currentPageIndex, newPageIndex, and pagesCount
            pageIndexChanged: "pageIndexChanged.dj.PortalHeadlineList",
            // componentRendered is raised when the component is done rendering
            componentRendered: "componentRendered.dj.PortalHeadlineList",
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "PortalHeadlineList" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // Initialize component if we got data from server
            this._setData(this.data);
        },

        _initializeHeadlineList: function (data) {
            var items = $(this.selectors.headlineEntry, this.$element);
            var me = this;
            _.each(_.first(data, items.length), function (headline, i) {
                var tLi = items.get(i);
                // Set the data to the li
                $(tLi).data("headline", headline);

                // Set the tooltip (snippets)
                // SnippetDisplayType = Hover
                if ((me.options.displaySnippets === 3) && headline.snippets && headline.snippets.length > 0) {
                    me._renderSnippets(headline, tLi);
                }

                else // SnippetDisplayType = HybridHover
                    if ((me.options.displaySnippets === 4) && headline.snippets && headline.snippets.length > 0) {
                        if (i != 0) {
                            me._renderSnippets(headline, tLi);
                        }
                        else {
                            var snippetStr = "";
                            _.each(headline.snippets, function (snippet, s) {
                                snippetStr += snippet;
                            });
                            var inlineSnippetHtml = '<p class="article-snip">' + snippetStr + '</p>';
                            $('div.article-wrap', tLi).append(inlineSnippetHtml);
                        }
                    }
            }, this);

            if (this.options.allowPagination) {
                this._setupPagination();
            }
        },

        _setupPagination: function () {
            var me = this;

            this.$carousel = this.$element.find(".dj_Carousel");
            this.$carouselInner = this.$carousel.find(".dj_Carousel-inner");
            this.$pages = this.$carouselInner.find(".slidePanel");
            this.currentPageIndex = 0;

            var containerWidth = this.$element.width();

            this.$pages.width(containerWidth);

            this._resizeCarousel(true);
            
            $(this.options.pagePrevSelector).click(function () {
                me._goToPage(me.currentPageIndex - 1);
            });

            $(this.options.pageNextSelector).click(function () {
                me._goToPage(me.currentPageIndex + 1);
            });


            // to capture resizing of the container this.$element
            $(window).resize(function () {
                var containerWidth = me.$element.width();
                me.$pages.width(containerWidth);
                me.$carouselInner.width(me._getCarouselWidth(containerWidth));
                me._goToPage(me.currentPageIndex, true);
                //resize(containerWidth);
            });
        },

        _goToPage: function (pageIndex, disableAnimation) {
            if (pageIndex < 0 || pageIndex >= this.pagesCount)
            {
                if (!this.options.circularPaging)
                    return;
                    
                if (pageIndex < 0)
                    pageIndex = this.pagesCount - 1;
                else if (pageIndex >= this.pagesCount)
                    pageIndex = 0;
            }
            
            if (pageIndex !== this.currentPageIndex) {            
                this.publish(this.events.pageIndexChanged,
                             {
                                 currentPageIndex: this.currentPageIndex,
                                 newPageIndex: pageIndex,
                                 pagesCount: this.pagesCount
                             });            
            }

            var targetPosition = this.$pages.eq(pageIndex).position();
            
            if (disableAnimation) {
                this.$carouselInner.css({ left: -targetPosition.left, top: -targetPosition.top });
            }
            else {
                this.$carouselInner.animate({ left: -targetPosition.left, top: -targetPosition.top }, this.options.speed);
            }

            this.currentPageIndex = pageIndex;
            this._resizeCarousel(true);
        },

        _resizeCarousel: function(disableAnimation) {
            var me = this;
            var $currentPage = this.$pages.eq(this.currentPageIndex);
            var width, height;
  
            // if there are hidden parents, need to recalculate everything 
            var $hiddenParents = this.$element.parents(":hidden");         
            if ($hiddenParents.length > 0) {
                $hiddenParents.addClass("dj_show");
                
                var containerWidth = this.$element.width();
                this.$pages.width(containerWidth);
                this.$carouselInner.width(this._getCarouselWidth(containerWidth));
                
                var $images = this.$element.find("img");                
                if ($images.length > 0) {
                    this.$element.imagesLoaded(function() {
                        me._setCarouselWidthAndHeight($currentPage.width(), $currentPage.height(), disableAnimation);
                        $hiddenParents.removeClass("dj_show");                        
                    });
                    return;
                }
                
                this._setCarouselWidthAndHeight($currentPage.width(), $currentPage.height(), disableAnimation);
                $hiddenParents.removeClass("dj_show");
            }
            else {
                this._setCarouselWidthAndHeight($currentPage.width(), $currentPage.height(), disableAnimation);
            }
        },
        
        _setCarouselWidthAndHeight: function(width, height, disableAnimation) {
            if (disableAnimation) {
                this.$carousel.width(width).height(height);
            }
            else {
                this.$carousel.animate({
                    width: width,
                    height: height
                }, this.options.pageSpeed);
            }
        },
        
        _getCarouselWidth: function(containerWidth) {
            if (this.$carousel.hasClass("vertical")) {
                this.$carouselInner.width(containerWidth);
            }
            else {
                this.$carouselInner.width(this.pagesCount * containerWidth);
            }
        },

        //Render snippets
        _renderSnippets: function (headline, tLi) {
            var snippetStr = "";
            _.each(headline.snippets, function (snippet, s) {
                snippetStr += "<div>" + snippet + "</div>";
            });
            $(this.selectors.headline, tLi).attr("title", snippetStr);
            $(this.selectors.headline, tLi).dj_simpleTooltip("tooltip");
        },
        
        _includePagingCssStyles: function() {
            if ($("style#PortalHeadlineListPagination").length === 0) {
                $("<style id='PortalHeadlineListPagination' type='text/css'>.dj_Carousel{position:relative;width:0;overflow:hidden;margin:0;}.dj_Carousel .dj_Carousel-inner{position:absolute;}.dj_Carousel.vertical .dj_Carousel-inner .slidePanel{float:none;}.dj_Carousel .dj_Carousel-inner .slidePanel{padding-bottom:1px;float:left;}.dj_show{visibility:hidden!important;display:block!important;position:absolute!important;}</style>").appendTo("head");                
            }
        },

        _initializeElements: function () {
        },

        _initializeEventHandlers: function () {
            var $parentContainer = this.$element
                , me = this;

            this.$element.delegate(this.selectors.headline, 'click', function () {
                me.publish(me.events.headlineClick, { headline: $(this).closest('li').data("headline") });
                // prevent browser from handling the click
                return false;
            });

            if (this.options.sourceClickable) {
                this.$element.delegate(this.selectors.source, 'click', function () {
                    me.publish(me.events.sourceClick, { sourceCode: $(this).attr("rel") });
                    return false;
                });
            }

            if (this.options.authorClickable) {
                this.$element.delegate(this.selectors.author, 'click', function () {
                    me.publish(me.events.authorClick, { authorCode: $(this).attr("rel") });
                    return false;
                });
            }
        },

        _setData: function (data) {
            if (data && data.resultSet)
                this.bindOnSuccess(data.resultSet);
            else
                this.bindOnSuccess({});
        },

        bindOnSuccess: function (data) {
            this.publish(this.events.dataAssociated,
                         {
                             data: data
                         });
                         
            var headlineMarkup;

            try {
                this.$element.html("");
                if (data && data.count && data.count.value > 0) {
                    // call to bind and append html to ul in one shot
                    var successTemplate;

                    switch (this.options.layout) {
                        case 0: // normal headline view
                            successTemplate = this.templates.successHeadline;
                            break;
                        case 1: // author headline view
                            successTemplate = this.templates.successAuthor;
                            break;
                        case 2: // timeline headline view
                            successTemplate = this.templates.successTimeline;
                            break;
                    }

                    if (this.options.allowPagination) {
                        this._includePagingCssStyles();
                        
                        var headlinePages = [];
                        // split the headlines into pages
                        for (var i = 0; i < data.headlines.length; i+=this.options.pageSize) {
                            headlinePages.push(data.headlines.slice(i, i + this.options.pageSize));
                        }
                        this.pagesCount = headlinePages.length;                        
                        headlineMarkup = this.templates.pagination({ headlinePages: headlinePages, options: this.options, sucessTemplate: successTemplate });
                    }
                    else {
                        headlineMarkup = successTemplate({ headlines: data.headlines, options: this.options });
                    }

                    this.$element.append(headlineMarkup);

                    // bind events and perform other wiring up
                    this._initializeHeadlineList(data.headlines);
                    
                    this.publish(this.events.componentRendered,
                                 {
                                     currentPageIndex: 0,
                                     pagesCount: this.pagesCount,
                                     data: data
                                 });
                }
                else {
                    // bind the template
                    this.$element.append(this.templates.noData());

                    if (!this.options.displayNoResultsToken) {
                        var no_results = $(this.selectors.noResultSpan, this.$element).get(0);
                        if (no_results) {
                            $(no_results).hide();
                        }
                    }
                }
            } catch (e) {
                $dj.error('Error in PortalHeadlineList.bindOnSuccess:', e);
            }
        },

        bindOnError: function (data) {
            try {
                this.$element.html("");
                this.$element.append(this.templates.error(data));
            } catch (e) {
                $dj.error('Error in PortalHeadlineList.bindOnError:', e);
            }
        },


        getErrorTemplate: function () {
            return this.templates.error;
        },


        getNoDataTemplate: function () {
            return this.templates.noData;
        },


        showEditSection: function (show) {
            show = show || true;

            this.$element.html("");

            if (show) {
                this.$element.append(this.templates.addNewContent());
            }
        },


        setErrorTemplate: function (markup) {
            this.templates.error = _.template(markup);
        },


        setNoDataTemplate: function (markup) {
            this.templates.noData = _.template(markup);
        },
        
        EOF: null
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_PortalHeadlineList', DJ.UI.PortalHeadlineList);
    $dj.debug('Registered DJ.UI.PortalHeadlineList (extends DJ.UI.Component)');