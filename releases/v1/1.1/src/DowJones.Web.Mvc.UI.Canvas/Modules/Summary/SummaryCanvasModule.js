/*!
 * SummaryCanvasModule
 */

(function ($) {

    DJ.UI.SummaryCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        selectors: {
            topRow: 'div.top-row',
            bottomRow: 'div.bottom-row',
            lhContainer: 'div.latest-headlines',
            miChartContainer: 'div.dj_market_index',
            vcContainer: 'div.video-carousel',
            tpContainer: 'div.trending-people',
            tkContainer: 'div.trending-keywords',
            tcContainer: 'div.trending-companies',
            tsContainer: 'div.trending-subjects',
            rmContainer: 'div.regional-map-content',
            portalHeadlineList: 'div.dj_headlineListContainer',
            newsChart: 'div.dj_NewsChartControl',
            noResultsContainer: 'div.no-results',
            regionalMap: "div.dj_RegionalMap",
            tagCloud: 'div.dj_TagCloud',
            portalHeadlineList: 'div.dj_headlineListContainer',
            headlineCarousel: 'div.dj_headlineListCarousel',
            noResults: 'div.no-results',
            mediaThumbNail: 'div.media-thumb'
        },

        events: {
            summaryTrendingPeopleClick: 'tPeopleClick.dj.SummaryCanvasModule',
            summaryTrendingCompaniesClick: 'tCompaniesClick.dj.SummaryCanvasModule',
            summaryTrendingSubjectsClick: 'tSubjectsClick.dj.SummaryCanvasModule',
            summaryTrendingKeywordsClick: 'tKeywordsClick.dj.SummaryCanvasModule',
            summaryRegionClick: "regionClick.dj.SummaryCanvasModule",
            summaryAudioVideoHeadlineClick: "avClick.dj.SummaryCanvasModule",
            summaryLatestHeadlineClick: "lhClick.dj.SummaryCanvasModule",
            summaryMDProviderClick: 'mdProvider.dj.SummaryCanvasModule'
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "SummaryCanvasModule" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            this._partsLoaded = 0;
            this._parts = "Chart|RecentArticles|RecentVideos|RegionalMap|Trending";
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this.showErrorMessage),
                OnKeywordsClick: $dj.delegate(this, this._onKeywordsClick),
                OnLatestHeadlineClick: $dj.delegate(this, this._onLatestHeadlineClick),
                OnVideoHeadlineClick: $dj.delegate(this, this._onVideoHeadlineClick),
                OnRegionClick: $dj.delegate(this, this._onRegionClick),
                OnMDProviderClick: $dj.delegate(this, this._onMDProviderClick)
            });
        },

        /*
        * Public methods
        */

        // TODO: Public Methods here

        getData: function () {
            this._super();

            $dj.proxy.invoke({
                url: this.options.dataServiceUrl,
                queryParams: {
                    "pageid": this._canvas.get_canvasId(),
                    "moduleid": this.get_moduleId(),
                    "parts": this._parts,
                    "firstResultToReturn": 0,
                    "maxResultsToReturn": this.options.maxResultsToReturn,
                    "maxEntitiesToReturn": this.options.maxEntitiesToReturn
                },
                controlData: this._canvas.get_ControlData(),
                preferences: this._canvas.get_Preferences(),
                onSuccess: this._delegates.OnServiceCallSuccess,
                onError: this._delegates.OnServiceCallError
            });
        },

        fireOnSaveAndCloseEditArea: function (e) {
            var editorProps = this._editor.buildProperties();
            this._publish('swap', editorProps);
        },

        _setContainers: function () {
            this.topRow = $(this.selectors.topRow, this.$element);
            this.bottomRow = $(this.selectors.bottomRow, this.$element);
            this.lhContainer = $(this.selectors.lhContainer, this.topRow);
            this.tpContainer = $(this.selectors.tpContainer, this.topRow);
            this.miChartContainer = $(this.selectors.miChartContainer, this.topRow);
            this.tkContainer = $(this.selectors.tkContainer, this.topRow);
            this.vcContainer = $(this.selectors.vcContainer, this.bottomRow);
            this.tcContainer = $(this.selectors.tcContainer, this.bottomRow);
            this.tsContainer = $(this.selectors.tsContainer, this.bottomRow);
            this.rmContainer = $(this.selectors.rmContainer, this.bottomRow);
        },

        _bindLatestHeadlines: function (latestHeadlines) {
            if(latestHeadlines){
                var phL = this.lhContainer.find(this.selectors.portalHeadlineList).findComponent(DJ.UI.PortalHeadlineList);
                if (latestHeadlines && latestHeadlines.package && latestHeadlines.returnCode == 0 && latestHeadlines.statusMessage == null && latestHeadlines.package.portalHeadlineListDataResult && latestHeadlines.package.portalHeadlineListDataResult.resultSet) {
                    phL.bindOnSuccess(latestHeadlines.package.portalHeadlineListDataResult.resultSet);
                    $(phL.element).unbind(phL.events.headlineClick).bind(phL.events.headlineClick, this._delegates.OnLatestHeadlineClick);
                }
                else {
                    if(latestHeadlines){
                        phL.bindOnError({ code: latestHeadlines.returnCode, message: latestHeadlines.statusMessage });
                    }
                }
                this._partsLoaded++;
                this.lhContainer.show();
            }
            else{
                this.lhContainer.hide();
            }
        },

        _bindMarketIndexChart: function (marketData) {
            if(marketData){
                var chartCtrl = this.miChartContainer.find(this.selectors.newsChart).findComponent('dj_NewsChartControl');
                if (marketData.package && marketData.returnCode == 0 && marketData.statusMessage == null  && marketData.package.marketIndexIntradayResult) {
                    JSON.parseDatesInObj(marketData.package);
                    var data = { stockDataResult: marketData.package.marketIndexIntradayResult };
                    if (marketData.package.marketIndexIntradayResult.provider) {
                        chartCtrl.setMDProvider(marketData.package.marketIndexIntradayResult.provider.name, marketData.package.marketIndexIntradayResult.provider.externalUrl);
                        $(chartCtrl.element).unbind(chartCtrl.events.sourceClick).bind(chartCtrl.events.sourceClick, this._delegates.OnMDProviderClick);
                    }
                    chartCtrl.setData(data);
                }
                else{
                    chartCtrl.setError(marketData);
                }
                this.miChartContainer.show();
                this._partsLoaded++;
            }
            else{
                this.miChartContainer.hide();
            }
        },

        _bindVideoCarousel: function (videoHeadlines) {
            if(videoHeadlines){
                var hcCtrl = this.vcContainer.find(this.selectors.headlineCarousel).findComponent(DJ.UI.HeadlineCarousel);
                if(videoHeadlines.package && videoHeadlines.returnCode == 0 && videoHeadlines.statusMessage == null) {
                    hcCtrl.setData(videoHeadlines.package.portalHeadlineListDataResult);
                    $(hcCtrl.element).unbind(hcCtrl.events.headlineClick).bind(hcCtrl.events.headlineClick, this._delegates.OnVideoHeadlineClick);
                    
                }
                else{
                    hcCtrl.bindOnError(videoHeadlines);
                }
                this.vcContainer.show();
                this._partsLoaded++;
            }
            else{
                this.vcContainer.hide();
            }
        },

        _bindTrending: function (trendingData, container, eventName, trendingType) {
            if(trendingData)
            {
                if (trendingData.returnCode == 0 && trendingData.statusMessage == null && trendingData.package[trendingType] && trendingData.package[trendingType].length > 0) {
                    trendingData = trendingData.package[trendingType];
                    var me = this, template = '<% for(var i = 0, l = tI.length;i<l;i++) {%><li><h4 class="trending-item"><a href="javascript:void(0);" index="<%= i %>" class="popup-trigger ellipsis" title="<%= tI[i].descriptor %>"><%= tI[i].descriptor %></a></h4></li><% } %>';
                    container.find("ul:first").html((_.template(template))({ tI: trendingData }))
                        .delegate('a.popup-trigger', 'click', function () {
                            var data = trendingData[$(this).attr("index")];
                            me._publish(eventName, { searchContext: data.searchContextRef, target: this, title: data.descriptor, modulePart: $(this).closest("div").find("h3").text() + " - " + data.descriptor });
                            $dj.debug('Published ' + eventName);
                        });
                }
                else {
                    if(trendingData.returnCode == 0)
                        container.find("ul:first").html("<li><span class='no-results'><%= Token("noResults") %></span></li>");
                    else
                        container.find("ul:first").html("<li>"+$dj.formatError(trendingData.returnCode, trendingData.statusMessage)+"</li>");
                }
                container.show();
                this._partsLoaded++;
            }
            else{
                container.hide();
            }
        },

        _bindTrendingKeywords: function (trendingData) {
            if(trendingData){
                var tgCtrl = this.tkContainer.find(this.selectors.tagCloud).findComponent(DJ.UI.TagCloud);
                if(trendingData.package && trendingData.returnCode == 0 && trendingData.statusMessage == null) {
                    tgCtrl.setData({ result: trendingData.package.keywordsTagCollection });
                    $(tgCtrl.element).unbind(tgCtrl.events.tagItemClick).bind(tgCtrl.events.tagItemClick, this._delegates.OnKeywordsClick);
                }
                else{
                    tgCtrl.bindOnError({ code: trendingData.returnCode, message: trendingData.statusMessage });
                }
                this.tkContainer.show();
                this._partsLoaded++;
            }
            else{
                this.tkContainer.hide();
            }
        },

        _bindRegionalMap: function (regions) {
            if(regions){
                if (regions.package && regions.package.regionNewsVolume && regions.package.regionNewsVolume.length > 0) {
                    this.rmContainer.find(this.selectors.noResults).hide();
                    this.rmContainer.find('div.mini-regional-map').show();
                    var rm = this.rmContainer.find(this.selectors.regionalMap).findComponent(DJ.UI.RegionalMap);
                    rm.setData(regions.package);
                    $(rm.element).unbind(rm.events.regionClick).bind(rm.events.regionClick, this._delegates.OnRegionClick);
                
                }
                else {
                
                    if(regions && regions.returnCode != 0 && regions.statusMessage != null)
                        this.rmContainer.find(this.selectors.noResults).html($dj.formatError(regions.returnCode, regions.statusMessage)).show();
                    else
                        this.rmContainer.find(this.selectors.noResults).html("<%= Token("noResults") %>").show();

                    this.rmContainer.find('div.mini-regional-map').hide();
                }
                this.rmContainer.show();
            }
            else{
                this.rmContainer.hide();
            }
        },

        _onRegionClick: function (sender, data) {
            this._publish(this.events.summaryRegionClick, { "searchContext": data.searchContext, "title": data.title, 
                "target": data.element, 
                "regionCode": data.regionCode,
                "modulePart":  "<%= Token('telecomNewsByRegion') %> - " + data.title,
                "positionX": data.positionX,
                "positionY": data.positionY,
                "offset": data.offset
            });
            $dj.debug('Published ' + this.events.summaryRegionClick);
        },

        _onKeywordsClick: function (sender, args) {
            this._publish(this.events.summaryTrendingKeywordsClick, { searchContext: args.data.searchContextRef, title: args.data.text, target: args.element, modulePart: "<%= Token("trendingKeywords") %> - " + args.data.text });
            $dj.debug('Published ' + this.events.summaryTrendingKeywordsClick);
        },

        _onLatestHeadlineClick: function (sender, args) {
            this._publish(this.events.summaryLatestHeadlineClick, args);
            $dj.debug('Published ' + this.events.summaryLatestHeadlineClick);
        },

        _onVideoHeadlineClick: function (sender, args) {
            args.mediaContainer = $(args.element).find(this.selectors.mediaThumbNail)[0];
            this._publish(this.events.summaryAudioVideoHeadlineClick, args);
            $dj.debug('Published ' + this.events.summaryAudioVideoHeadlineClick);
        },

        _onMDProviderClick: function (sender, args) {
            this._publish(this.events.summaryMDProviderClick, { url: args.url, source: args.source });
            $dj.debug('Published ' + this.events.summaryMDProviderClick);
        },

        _onSuccess: function (data) {
            try {
                if (!data) {
                    this.showErrorMessage({ returnCode: '-1', statusMessage: '<%= Token("errorForMinus1") %>' });
                    return;
                };

                if (data.returnCode != 0) {
                    this.showErrorMessage(data);
                    return;
                };
                this._partsLoaded = 0;
                if (data && data.partResults && data.partResults.length > 0) {
                    var latestHeadlines, trending, marketIndexPart, videos, regionalMap, hasCharts = true;
                    _.each(data.partResults, function (partResult) {
                        if (partResult && partResult.package) {
                            switch (partResult.package.__type) {
                                case "summaryChartPackage": marketIndexPart = partResult; break;
                                case "summaryRecentArticlesPackage": latestHeadlines = partResult; break;
                                case "summaryVideosPackage": videos = partResult; break;
                                case "summaryRegionalMapPackage": regionalMap = partResult; break;
                                case "summaryTrendingPackage": trending = partResult; break;
                            }
                        }
                    }, this);

                    this.hasMarketDataIndex = data.hasMarketDataIndex;

                    this._setContainers();

                    this._bindLatestHeadlines(latestHeadlines);

                    if (marketIndexPart && this.hasMarketDataIndex)
                        this._bindMarketIndexChart(marketIndexPart);
                    else
                        this.miChartContainer.hide();

                    if (trending) {
                        //People
                        this._bindTrending(trending, this.tpContainer, this.events.summaryTrendingPeopleClick, "executivesNewsEntities");
                        //Companies
                        this._bindTrending(trending, this.tcContainer, this.events.summaryTrendingCompaniesClick, "companyNewsEntities");
                        //Industries
                        if (!this.hasMarketDataIndex)
                            this._bindTrending(trending, this.tsContainer, this.events.summaryTrendingSubjectsClick, "newsSubjectsNewsEntities");
                        else
                            this.tsContainer.hide();
                    }
                    else
                    {
                        this.tpContainer.hide();
                        this.tcContainer.hide();
                        this.tsContainer.hide();
                    }

                    if (!this.hasMarketDataIndex)
                        this._bindTrendingKeywords(trending);
                    else
                        this.tkContainer.hide();

                    this.showContentArea();

                    this._bindVideoCarousel(videos);

                    if (regionalMap && this.hasMarketDataIndex)
                        this._bindRegionalMap(regionalMap);
                    else
                        this.rmContainer.hide();
                }
                else {
                    this.showErrorMessage(data);
                }

                if (this._partsLoaded == 0) {
                    $(this.selector.noResultsContainer, this.$element).show();
                }
            }
            catch (e) {
                $dj.debug(e.message);
            }
        }

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_SummaryCanvasModule', DJ.UI.SummaryCanvasModule);

    $dj.debug('Registered DJ.UI.SummaryCanvasModule as dj_SummaryCanvasModule');

})(jQuery);
