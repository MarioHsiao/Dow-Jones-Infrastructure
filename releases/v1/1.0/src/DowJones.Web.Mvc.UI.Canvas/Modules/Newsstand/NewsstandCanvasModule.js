/*!
* NewsstandCanvasModule
*   e.g. , "this._imageSize" is generated automatically.
*
*   
*  Getters and Setters are generated automatically for every Client Property during init;
*   e.g. if you have a Client Property called "imageSize" on server side code
*        get_imageSize() and set_imageSize() will be generated during init.
*  
*  These can be overriden by defining your own implementation in the script. 
*  You'd normally override the base implementation if you have extra logic in your getter/setter 
*  such as calling another function or validating some params.
*
*/

(function ($) {

    DJ.UI.NewsstandModule = DJ.UI.AbstractCanvasModule.extend({

        /*
        * Initialization (constructor)
        */

        selectors: {
            headlineCarousel: ".dj_headlineListCarousel",
            headlineHitCountContainer: ".dj_newsstand_module-source-carousel",
            discoveredEntitiesContainer: ".dj_newsstand_module-headline-carousel"
        },

        events: {
            newsStandHeadlineSectionViewAllClick: 'newsStandViewAll.dj.NewsStandCanvasModule',
            newsStandHeadlineClick: 'headline.dj.NewsStandCanvasModule',
            newsStandCarouselHeadlineClick: 'carouselHeadline.dj.NewsStandCanvasModule'
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "NewsstandModule" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // get all control plugins and preserve a reference to avoid subsequent lookups
            // specifying scope + [tag.selector] pattern for ultra fast lookups
            this.sourceLogos = $("h3.module-col-title img.module-col-source-img", this.$element);
            this.sourceTitles = $("h3.module-col-title span.module-col-title-source-icon-text", this.$element);
            this.sectionTitles = $("span.module-col-section-title", this.$element)
            this.portalHeadlineLists = $(".dj_headlineListContainer", this.$element);
            this.viewAllBtns = $("ul.view-all-btn a.dashboard-control", this.$element);

            this.newsStandHeadlineSections = $("div.newsStandHeadlineSections", this.$element);
            this.newStandHitCountCarousel = $(".dj_newsstand_module-source-carousel", this.$element);
            this.discoveredEntities = $(".dj_newsstand_module-headline-carousel", this.$element);

            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this.showErrorMessage),
                OnPortalHeadlineClick: $dj.delegate(this, this._onHeadlineClick),
                OnCarouselHeadlineClick: $dj.delegate(this, this._onCarouselHeadlineClick)
            });

            this.headlineCarouselContainer = $(this.selectors.headlineCarouselContainer, this.$element);
            this._initializeHandlers();
        },

        _initializeHandlers: function () {
            // register portal headline click handlers
            _.each(this.portalHeadlineLists, function (phl) {
                if (!this._portalEventMapCache) {
                    $fnPhl = $(phl).findComponent(DJ.UI.PortalHeadlineList);
                    var _map = {};
                    _map[$fnPhl.events.headlineClick] = this._delegates.OnPortalHeadlineClick;
                    this._portalEventMapCache = _map;
                }
                $(phl).bind(this._portalEventMapCache);

            }, this);
        },

        fireOnSaveAndCloseEditArea: function (e) {
            var editorProps = this._editor.buildProperties();
            this._publish('swap', editorProps);
        },

        /*
        * Public methods
        */

        getData: function () {
            this._super();
            $dj.proxy.invoke({
                url: this.options.dataServiceUrl,
                queryParams: {
                    "pageid": this._canvas.get_canvasId(),
                    "moduleid": this.get_moduleId(),
                    "firstResultToReturn": this.options.firstHeadlineToReturn,
                    "maxResultsToReturn": this.options.maxHeadlinesToReturn,
                    "parts": "Headlines|Counts|DiscoveredEntities"
                },
                controlData: this._canvas.get_ControlData(),
                preferences: this._canvas.get_Preferences(),
                onSuccess: this._delegates.OnServiceCallSuccess,
                onError: this._delegates.OnServiceCallError
            });
        },

        SetTickerData: function (tickerType, tickerData) {
            var phl = null;
            //var headlineCarousel = this.headlineCarouselContainer.findComponent(DJ.UI.HeadlineCarousel);            

            switch (tickerType.toLowerCase()) {
                case 'discoveredentities':
                    phl = $(".dj_headlineListCarousel", this.discoveredEntities);
                    break;
                case 'headlinehitcounts':
                    phl = $(".dj_headlineListCarousel", this.newStandHitCountCarousel);
            }
            if (phl && phl.length > 0) {
                var headlineCarousel = $(phl).findComponent(DJ.UI.HeadlineCarousel);
                headlineCarousel.bindOnSuccess(tickerData);
                $(headlineCarousel.element).bind(headlineCarousel.events.headlineClick, this._delegates.OnCarouselHeadlineClick);
            }
        },

        /*
        * Private methods
        */
        _onSuccess: function (data) {

            var me = this;
            var error = $dj.getError(data);
            if (error) {
                this.showErrorMessage(error);
            }
            else {
                if (data && data.partResults && data.partResults.length > 0) {
                    for (var i = 0; i < data.partResults.length; i++) {
                        if (data.partResults[i].packageType) {
                            var type = data.partResults[i].packageType;
                            switch (type.toLowerCase()) {
                                case 'newsstanddiscoveredentitiespackage': //top ticker
                                    var autoScrollTickerData = data.partResults[i].package;
                                    if (data && data.partResults[i] && data.partResults[i].returnCode === 0) {
                                        if (autoScrollTickerData && autoScrollTickerData.topNewsVolumeEntities && autoScrollTickerData.topNewsVolumeEntities.length > 0) {
                                            setTimeout(function () { me.SetTickerData("discoveredEntities", autoScrollTickerData) }, 100);
                                        }
                                    }
                                    else {
                                        error = $dj.getError(data.partResults[i]);
                                        me._showModulePartError(error, this.discoveredEntities);

                                    }
                                    break;

                                case 'newsstandheadlinespackage': //middle headlines
                                    // reset visibility if there's a mismatch between requested and received results
                                    if (data && data.partResults[i] && data.partResults[i].package && data.partResults[i].package.newsstandSections.length < this.options.maxHeadlineSectionsToReturn) {
                                        this._hideNewsStandHeadlineAreas();
                                    }
                                    if (data && data.partResults[i] && data.partResults[i].package && data.partResults[i].returnCode === 0) {
                                        var newsstandSectionsArr = data.partResults[i].package.newsstandSections;
                                        _.each(newsstandSectionsArr,
                                            function (newsstandSection) {
                                                // find the position of the Portal Headline List
                                                var controlIndex = $.inArray(newsstandSection, newsstandSectionsArr);

                                                // get hold of the component by the index
                                                // make sure this collection is populated during init
                                                var partResult = data.partResults[i];
                                                if (partResult.returnCode === 0) {

                                                    this._bindSourceTitles(controlIndex, newsstandSection);
                                                    this._bindSourceLogos(controlIndex, newsstandSection);
                                                    this._bindSectionTitles(controlIndex, newsstandSection);
                                                    this._bindHeadlines(controlIndex, newsstandSection);
                                                    this._bindViewAllBtns(controlIndex, newsstandSection);
                                                }
                                                else {
                                                    if (phl) $(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnError({ 'code': partResult.returnCode, 'message': partResult.statusMessage });
                                                }

                                            }, this);
                                    }
                                    else {
                                        error = $dj.getError(data.partResults[i]);
                                        me._showModulePartError(error, this.newsStandHeadlineSections);
                                    }
                                    break;

                                case 'newsstandheadlinehitcountspackage': //bottom ticker
                                    var manualScrollTickerData = data.partResults[i].package;
                                    if (data && data.partResults[i] && data.partResults[i].returnCode == 0) {
                                        if (manualScrollTickerData && manualScrollTickerData.newsstandHeadlineHitCounts && manualScrollTickerData.newsstandHeadlineHitCounts.length > 0) {
                                            setTimeout(function () { me.SetTickerData("headlineHitCounts", manualScrollTickerData) }, 100);
                                        }
                                    }
                                    else {
                                        error = $dj.getError(data.partResults[i]);
                                        me._showModulePartError(error, this.newStandHitCountCarousel);
                                    }
                                    break;
                                default:
                                    this._hideNewsStandHeadlineAreas();
                                    this.showMessage("<%= Token("noNewsStandSectionsInModule") %>");
                            }
                        }
                    }
                    this.showContentArea();
                }
                else {
                    this._hideNewsStandHeadlineAreas();
                    this.showMessage("<%= Token("noNewsStandSectionsInModule") %>");
                }
            }
        },

        _showModulePartError: function (error, $modulePart) {
            var formattedError = $dj.formatError(error);
            $modulePart.html(formattedError);
        },

        _onHeadlineClick: function (sender, data) {
            this._publish(this.events.newsStandHeadlineClick, data);
            $dj.debug('Published ' + this.events.newsStandHeadlineClick);
        },

        _onCarouselHeadlineClick: function (sender, args) {
//            var context = args.headline.searchContextRef
            //var t = $('h3', $(args.element)).html().trim();
//            var headlineCount, $count = $('span', args.element);
//            if ($count.size() > 0) {
//                headlineCount = parseInt($count.html().slice(0, 1), 10); //first digit of ##{articles}
//            }

            this._publish(this.events.newsStandCarouselHeadlineClick, { searchContext: args.headline.searchContextRef, title: args.title, modulePart: args.title, target: args.element, headlineCount: args.hitCount });
            $dj.debug('Published ' + this.events.newsStandCarouselHeadlineClick);
        },

        _bindSourceLogos: function (idx, newsstandSection) {
            if (!newsstandSection) { return; }
            var self = this;
            var at = this.sourceLogos[idx];
            if (at) {
                $(at).attr("src", newsstandSection.sourceLogoUrl)
                .load(function () {
                    self._onSourceLogoLoad(idx);
                })
                .error(function () {
                    self._onSourceLogoError(idx);
                });
            }
        },

        _onSourceLogoLoad: function (idx) {
            var att = this.sourceTitles[idx];
            if (att) {
                $(att).hide();
                var at = this.sourceLogos[idx];
                if (at) {
                    $(at).show();
                }
            }
        },

        _onSourceLogoError: function (idx) {
            var att = this.sourceTitles[idx];
            if (att) {
                $(att).show();
                var at = this.sourceLogos[idx];
                if (at) {
                    $(at).hide();
                }
            }
        },

        _bindSourceTitles: function (idx, newsstandSection) {
            if (!newsstandSection) { return; }
            var at = this.sourceTitles[idx];
            if (at) {
                $(at).html(newsstandSection.sourceTitle).show();
                var att = this.sourceLogos[idx];
                if (att) {
                    $(att).hide();
                }

            }
        },

        _bindSectionTitles: function (idx, newsstandSection) {
            if (!newsstandSection) { return; }
            var at = this.sectionTitles[idx];
            if (at) {
                $(at).html(newsstandSection.sectionTitle).show();
            }
        },

        _bindHeadlines: function (idx, newsstandSection) {
            var phl = this.portalHeadlineLists[idx];
            if (!phl) { return; }

            if (!newsstandSection) {
                // let the control display no data (based on options)
                $(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnSuccess(null);
                return;
            }

            if (newsstandSection.result != null && newsstandSection.result.resultSet != null) {
                // get hold of the component by the index
                // make sure this collection is populated during init
                var headlines = newsstandSection.result.resultSet;
                $(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnSuccess(headlines);
                $(phl).show();
            }
            else {
                $(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnError({
                    'code': newsstandSection.status,
                    'message': newsstandSection.statusMessage
                });
                $(phl).show();
            }
        },

        _bindViewAllBtns: function (idx, newsstandSection) {
            // wire up the view all button
            var viewAllBtn = this.viewAllBtns[idx];
            //when the status is 0 OR when the partresult was found and the hitCount is 0, then no View All.
            if (newsstandSection.status != 0 ||
                (newsstandSection.result &&
                 !(newsstandSection.result.hitCount) ||
                 (newsstandSection.result.hitCount && newsstandSection.result.hitCount.value === 0) ||
                 (newsstandSection.result.hitCount.value < this.options.maxHeadlinesToReturn)
                 )) {
                $(viewAllBtn).hide();
                return;
            }
            else {
                $(viewAllBtn).show();
            }

            // wire up the view all button
            $(viewAllBtn).click($dj.delegate(this, function (e) {
                this._publish(this.events.newsStandHeadlineSectionViewAllClick, { "searchContext": newsstandSection.viewAllSearchContext, "modulePart": newsstandSection.sourceTitle + ": " + newsstandSection.sectionTitle
                })
                e.stopPropagation();
                $dj.debug('Published ' + this.events.newsStandHeadlineSectionViewAllClick);
                return false;
            })).removeClass('hidden').show();
        },

        _hideNewsStandHeadlineAreas: function () {
            var idx, len;
            for (idx = 0, len = this.options.maxHeadlineSectionsToReturn; idx < len; idx++) {
                $(this.sourceLogos[idx]).hide();
                $(this.sourceTitles[idx]).hide();
                $(this.sectionTitles[idx]).hide();
                $(this.portalHeadlineLists[idx]).hide();
                $(this.viewAllBtns[idx]).hide();
            }
        },

        _onError: function (errorThrown, jqXHR, serverMessage) {
            this._hideNewsStandHeadlineAreas();
            this.showErrorMessage({ returnCode: errorThrown.code, statusMessage: errorThrown.message });
        },

        EOF: null

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_NewsstandModule', DJ.UI.NewsstandModule);

    $dj.debug('Registered DJ.UI.NewsstandCanvasModule as dj_NewsstandModule');

})(jQuery);

