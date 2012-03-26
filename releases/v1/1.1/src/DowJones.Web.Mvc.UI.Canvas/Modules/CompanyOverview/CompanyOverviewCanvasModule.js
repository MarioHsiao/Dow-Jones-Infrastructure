/*!
 * CompanyOverviewCanvasModule
 */

(function ($) {

    DJ.UI.CompanyOverviewCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        selectors: {
            chartContainer: 'div.company-chart-row',
            csContainer: 'div.company-snapshot-col > div.module-col-wrap',
            quoteControl: 'div.dj_Quote',
            aReportContainer: 'div.analysis-report',
            keywordsContainer: 'div.trending-chart-col > div.module-col-wrap',
            recentArticlesContainer: 'div.article-group-col > div.module-col-wrap',
            viewAllBtn: 'ul.view-all-btn a.dashboard-control',
            portalHeadlineList: 'div.dj_headlineListContainer',
            newsChart: 'div.dj_NewsChartControl',
            snapShotTabsWrap: 'div.dj_module-company-snapshot',
            tabContent: 'div.tab-content-wrap > div',
            tagCloud: 'div.dj_TagCloud',
            bottomRow: 'div.company-snapshot-row',
            noResultsContainer: 'div.no-results',
            executivesContainer: 'div.dj_company-executives',
            csTabs: 'ul.dj_module-company-snapshot-tabs > li',
            executives: 'div.dj_executives',
            marketData: 'div.dj_market-data',
            snapShotDateTime: 'p.snapshot-datetime',
            chartDateRange: 'p.dj_module-company-chart-date-range',
            startDate: 'span.start-date',
            endDate: 'span.end-date',
            companySnapShotError: 'div.company-snapshot-error'
        },

        events: {
            companyOverviewViewAllRecentArticlesClick: 'viewAllRecentArticles.dj.CompanyOverviewCanvasModule',
            companyOverviewKeywordsClick: 'keywordsClick.dj.CompanyOverviewCanvasModule',
            companyOverviewHeadlineClick: 'headlineClick.dj.CompanyOverviewCanvasModule',
            companyOverviewExecutivesClick: 'executivesClick.dj.CompanyOverviewCanvasModule',
            companyOverviewMDProviderClick: 'mdProvider.dj.CompanyOverviewCanvasModule'
        },

        _defaultParts: "SnapShot|Chart|RecentArticles|Trending",

        _zoomParts: "RecentArticles|Trending",

        init: function (element, meta) {
            var $meta = $.extend({ name: "CompanyOverviewCanvasModule" }, meta);

            this._super(element, $meta);
            this._showLoading = true;
            this._companyObj = {};
            this._languageCode = "en";
            this._setDefaultProperties();
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this._onError),
                OnPortalHeadlineClick: $dj.delegate(this, this._onHeadlineClick),
                OnChartZoom: $dj.delegate(this, this._onChartZoom),
                OnChartClick: $dj.delegate(this, this._onChartClick),
                OnChartReset: $dj.delegate(this, this._onChartReset),
                OnKeywordsClick: $dj.delegate(this, this._onKeywordsClick),
                OnMDProviderClick: $dj.delegate(this, this._onMDProviderClick)
            });
        },

        getData: function (noLoading) {
            if (!this._zoom)
                this._super();

            var queryParams = {
                "pageid": this._canvas.get_canvasId(),
                "moduleid": this.get_moduleId(),
                "timeframe": this._timeFrame,
                "parts": this._parts,
                "firstResultToReturn": 0,
                "maxResultsToReturn": this.options.maxResultsToReturn,
                "useCustomDateRange": this._customDateRange
            };

            if (this._customDateRange) {
                queryParams["startDate"] = this._startDate;
                queryParams["endDate"] = this._endDate;
            }

            $dj.proxy.invoke({
                url: this.options.dataServiceUrl,
                queryParams: queryParams,
                controlData: this._canvas.get_ControlData(),
                preferences: this._canvas.get_Preferences(),
                onSuccess: this._delegates.OnServiceCallSuccess,
                onError: this._delegates.OnServiceCallError
            });
        },

        fireOnSaveAndCloseEditArea: function (e) {
            var editorProps = this._editor.buildProperties();
            this._companyOverviewUpdateClick(this, editorProps);
        },

        _setDefaultProperties: function () {
            this._timeFrame = "lastweek",
            this._parts = this._defaultParts;
            this._zoom = false;
            this._partsLoaded = 0;
            this._customDateRange = false;
            this._startDate = null;
            this._endDate = null;
        },

        _setContainers: function () {
            this.chartContainer = $(this.selectors.chartContainer, this.$element);
            this.bottomRow = $(this.selectors.bottomRow, this.$element);
            this.csContainer = $(this.selectors.csContainer, this.bottomRow);
            this.aReportContainer = $(this.selectors.aReportContainer, this.csContainer);
            this.csTabs = $(this.selectors.csTabs, this.csContainer)
            this.keywordsContainer = $(this.selectors.keywordsContainer, this.bottomRow);
            this.recentArticlesContainer = $(this.selectors.recentArticlesContainer, this.bottomRow);
        },

        _bindChart: function (chartData) {
            if(chartData)
            {
                var chartCtrl = this.chartContainer.find(this.selectors.newsChart).findComponent(DJ.UI.NewsChart);
                if (chartData && chartData.package && chartData.returnCode == 0 && chartData.statusMessage == null && (chartData.package.newsDataResult != null || chartData.package.stockDataResult != null)) {
                    JSON.parseDatesInObj(chartData.package);
                
                    $(chartCtrl.element)
                        .unbind(chartCtrl.events.zoom)
                        .unbind(chartCtrl.events.zoomReset)
                        .unbind(chartCtrl.events.pointClick)
                        .bind(chartCtrl.events.zoom, this._delegates.OnChartZoom)
                        .bind(chartCtrl.events.zoomReset, this._delegates.OnChartReset)
                        .bind(chartCtrl.events.pointClick, this._delegates.OnChartClick);

                    //Language code
                    chartCtrl.setLanguageCode(this._languageCode);

                    if (chartData.package.stockDataResult) {
                        //Currency code
                        chartCtrl.setCurrencyCode(chartData.package.stockDataResult.currencyCode);
                        
                        //Provider
                        if (chartData.package.stockDataResult.provider) {
                            chartCtrl.setMDProvider(chartData.package.stockDataResult.provider.name, chartData.package.stockDataResult.provider.externalUrl);
                            $(chartCtrl.element).unbind(chartCtrl.events.sourceClick).bind(chartCtrl.events.sourceClick, this._delegates.OnMDProviderClick);
                        }
                    }
                    chartCtrl.setData(chartData.package);
                    this._updateChartDateRange(chartCtrl.getChartDateRange());
                    this.bottomRow.addClass("no-top-padding");
                    this.chartContainer.show();
                }
                else{
                    //chartCtrl.setError(chartData);
                    this.chartContainer.hide();
                    this.bottomRow.removeClass("no-top-padding");
                }
                this._partsLoaded++;
            }
            else
            {
                this.chartContainer.hide();
            }
        },

        _bindRecentArticles: function (recentArticles) {
            var phL = this.recentArticlesContainer.find(this.selectors.portalHeadlineList).findComponent(DJ.UI.PortalHeadlineList);
            if (recentArticles && recentArticles.package && recentArticles.returnCode == 0 && recentArticles.statusMessage == null && recentArticles.package.portalHeadlineListDataResult && recentArticles.package.portalHeadlineListDataResult.resultSet) {
                phL.bindOnSuccess(recentArticles.package.portalHeadlineListDataResult.resultSet);

                $(phL.element).unbind(phL.events.headlineClick).bind(phL.events.headlineClick, this._delegates.OnPortalHeadlineClick);

                this.recentArticlesContainer.show();

                if (recentArticles.package.portalHeadlineListDataResult.resultSet.count.value >= this.options.maxHeadlinesToShow) {
                    this.recentArticlesContainer.find(this.selectors.viewAllBtn).unbind('click').bind('click', $dj.delegate(this, function (e) {
                        this._publish(this.events.companyOverviewViewAllRecentArticlesClick, { "searchContext": recentArticles.package.viewAllSearchContext, modulePart: "<%= Token("recentArticles") %>" });
                        e.stopPropagation();
                        $dj.debug('Published ' + this.events.companyOverviewViewAllRecentArticlesClick);
                        return false;
                    })).show();
                }
                else {
                    this.recentArticlesContainer.find(this.selectors.viewAllBtn).hide();
                }
            }
            else {
                if (recentArticles) {
                    phL.bindOnError({ code: recentArticles.returnCode, message: recentArticles.statusMessage });
                    this.recentArticlesContainer.show().find(this.selectors.viewAllBtn).hide();
                }
            }
            this._partsLoaded++;
        },

        _bindCompanySnapshot: function (snapShot) {
            if (snapShot && snapShot.package && snapShot.returnCode == 0 && snapShot.statusMessage == null) {
                var me = this, phls = this.csContainer.find(this.selectors.portalHeadlineList), selectedTab = 0;
                
                this.csContainer.find("div:first").show();

                if (this.hasPublicCompanyInformation) {
                    //Quote Info
                    var quoteControl = $(this.selectors.quoteControl, this.csContainer).findComponent(DJ.UI.Quote); ;
                    quoteControl.setData(snapShot.package.quote);
                    $(quoteControl.element).unbind(quoteControl.events.sourceClick).bind(quoteControl.events.sourceClick, this._delegates.OnMDProviderClick);

                    this.csTabs.eq(0).show();
                    this.csContainer.find(this.selectors.marketData).removeClass('ui-tabs-hide');
                    this.csContainer.find(this.selectors.snapShotDateTime).html(snapShot.package.quote.lastTradeDateTimeDisplay).show();

                    //Hide Executives
                    this.csTabs.eq(2).hide();
                    this.csContainer.find(this.selectors.executives).addClass('ui-tabs-hide');
                    selectedTab = 0;
                }
                else {
                    this.executivesContainer = $(this.selectors.executivesContainer, this.csContainer);
                    //Show Executives information
                    if (snapShot.package.executives && snapShot.package.executives.length > 0) {
                        var template = '<% for(var i = 0, l=exec.length;i<l;i++) {%><h5 class="company-executive-name"  index="<%= i %>" href="javascript:void(0);"><%= exec[i].completeName %></h5><p class="company-executive-title"><%= exec[i].jobTitle %></p><% } %>';
                        this.executivesContainer.find("div:first").html((_.template(template))({ exec: snapShot.package.executives }));
                    }
                    else {
                        this.executivesContainer.find("div:first").html("<span class='no-results'><%= Token("noResults") %></span>");
                    }

                    this.csTabs.eq(2).show();
                    this.csContainer.find(this.selectors.executives).removeClass('ui-tabs-hide');

                    //Hide Quote
                    this.csTabs.eq(0).hide();
                    this.csContainer.find(this.selectors.marketData).addClass('ui-tabs-hide');
                    this.csContainer.find(this.selectors.snapShotDateTime).hide();

                    selectedTab = 1;
                }

                //Initialize Tabs - jQuery UI Tabs
                this.csContainer.find(this.selectors.snapShotTabsWrap).tabs({ selected: selectedTab });

                //Zack Reports
                var phIR = phls.eq(0).findComponent(DJ.UI.PortalHeadlineList);
                var phZR = phls.eq(1).findComponent(DJ.UI.PortalHeadlineList);
                var phPR = phls.eq(2).findComponent(DJ.UI.PortalHeadlineList);

                if (snapShot.package.investextReports && snapShot.package.investextReports.resultSet) {
                    phIR.bindOnSuccess(snapShot.package.investextReports.resultSet);
                    $(phIR.element).unbind(phIR.events.headlineClick).bind(phIR.events.headlineClick, this._delegates.OnPortalHeadlineClick);
                }
                else {
                    phIR.bindOnError({ code: snapShot.returnCode, message: snapShot.statusMessage });
                }

                if (snapShot.package.zacksReports && snapShot.package.zacksReports.resultSet) {
                    phZR.bindOnSuccess(snapShot.package.zacksReports.resultSet);
                    $(phZR.element).unbind(phZR.events.headlineClick).bind(phZR.events.headlineClick, this._delegates.OnPortalHeadlineClick);
                }
                else {
                    phZR.bindOnError({ code: snapShot.returnCode, message: snapShot.statusMessage });
                }

                if (snapShot.package.dataMonitorReports && snapShot.package.dataMonitorReports.resultSet) {
                    phPR.bindOnSuccess(snapShot.package.dataMonitorReports.resultSet);
                    $(phPR.element).unbind(phPR.events.headlineClick).bind(phPR.events.headlineClick, this._delegates.OnPortalHeadlineClick);
                }
                else {
                    phPR.bindOnError({ code: snapShot.returnCode, message: snapShot.statusMessage });
                }

                this.csContainer.find(this.selectors.companySnapShotError).hide();
                this._partsLoaded++;
            }
            else {
                this.csContainer.find("div:first").hide();
                this.csContainer.find(this.selectors.snapShotDateTime).hide();
                if (snapShot)
                    this.csContainer.find(this.selectors.companySnapShotError).html($dj.formatError(snapShot.returnCode, snapShot.statusMessage)).show();
            }
        },

        _bindKeywords: function (keyWords) {
            if(keyWords){
                var tgCtrl = this.keywordsContainer.find(this.selectors.tagCloud).findComponent(DJ.UI.TagCloud);
                if (keyWords && keyWords.package && keyWords.returnCode == 0 && keyWords.statusMessage == null) {

                    tgCtrl.setData(keyWords.package);
                    $(tgCtrl.element).unbind(tgCtrl.events.tagItemClick).bind(tgCtrl.events.tagItemClick, this._delegates.OnKeywordsClick);

                    this.keywordsContainer.show();
                }
                else {
                    tgCtrl.bindOnError({ code: keyWords.returnCode, message: keyWords.statusMessage });
                }
                this._partsLoaded++;
            }
            else{
                this.keywordsContainer.show();
            }
        },

        _getErrorMessage: function (code, msg) {
            return '<span class="dj_error">' + code + ': ' + msg + '</span>';
        },

        _onChartZoom: function (sender, data) {
            this._removeChartPlotLine();
            this._zoom = true;
            this._parts = this._zoomParts;
            this._customDateRange = true;
            this._startDate = (new Date(data.startDate)).format("UTC:mmddyyyy");
            this._endDate = (new Date(data.endDate)).format("UTC:mmddyyyy");
            this._updateChartDateRange({ start:(new Date(data.startDate)).format("standardDate", true, this._languageCode), end: (new Date(data.endDate)).format("standardDate", true, this._languageCode) });
            this.recentArticlesContainer.showLoading();
            this.keywordsContainer.showLoading();
            this.getData();
        },

        _updateChartDateRange: function (dataRange) {
            var datesContainer = this.chartContainer.find(this.selectors.chartDateRange);
            if (dataRange && dataRange.start && dataRange.end) {
                datesContainer.find(this.selectors.startDate).html(dataRange.start);
                datesContainer.find(this.selectors.endDate).html(dataRange.end);
                datesContainer.show();
            }
            else {
                datesContainer.hide();
            }
        },

        _onChartReset: function (sender, data) {
            var chartCtrl = this._removeChartPlotLine();
            var chartDateRange = chartCtrl.getChartDateRange();
            this._zoom = true;
            this._parts = this._zoomParts;
            this._customDateRange = false;
            this._startDate = null;
            this._endDate = null;
            this._updateChartDateRange(chartDateRange);
            this.recentArticlesContainer.showLoading();
            this.keywordsContainer.showLoading();
            this.getData();
        },

        _onChartClick: function (sender, data) {
            this._removeChartPlotLine();
            this._addChartPlotLine(data.xVal);
            this._zoom = true;
            this._parts = this._zoomParts;
            this._customDateRange = true;
            this._startDate = (new Date(data.formattedXVal)).format("UTC:mmddyyyy");
            this._endDate = (new Date(data.formattedXVal)).format("UTC:mmddyyyy");
            this._updateChartDateRange({start: data.dateDisplay, end: data.dateDisplay });
            this.recentArticlesContainer.showLoading();
            this.keywordsContainer.showLoading();
            this.getData();
        },

        _onHeadlineClick: function (sender, data) {
            this._publish(this.events.companyOverviewHeadlineClick, data);
            $dj.debug('Published ' + this.events.companyOverviewHeadlineClick);
        },

        _onKeywordsClick: function (sender, args) {
            this._publish(this.events.companyOverviewKeywordsClick, { searchContext: args.data.searchContextRef, title: args.data.text, target: args.element, modulePart: "<%= Token("keywords") %>" });
            $dj.debug('Published ' + this.events.companyOverviewKeywordsClick);
        },

        _onMDProviderClick: function (sender, args) {
            this._publish(this.events.companyOverviewMDProviderClick, { url: args.url, source: args.source });
            $dj.debug('Published ' + this.events.companyOverviewMDProviderClick);
        },

        _removeChartPlotLine: function () {
            var chartCtrl = this.chartContainer.find(this.selectors.newsChart).findComponent(DJ.UI.NewsChart);
            try {
                chartCtrl.chart.xAxis[0].removePlotLine('chartPlotline');
                return chartCtrl;
            } catch (e) { }
        },

        _addChartPlotLine: function (xVal) {
            var chartCtrl = this.chartContainer.find(this.selectors.newsChart).findComponent(DJ.UI.NewsChart);
            chartCtrl.chart.xAxis[0].addPlotLine({
                value: xVal,
                color: 'rgb(0, 0, 0)',
                width: 1,
                id: 'chartPlotline'
            });
        },

        _setCompanyDetails: function (data) {
            if (data.fcode) {
                this._companyObj.fcode = data.fcode;
            }
            if (data.companyName) {
                this._companyObj.companyName = data.companyName;
            }
        },

        _onSuccess: function (data) {
            if (!data) {
                this.showErrorMessage({ returnCode: '-1', statusMessage: '<%=Token("errorForMinus1")%>' });
                return;
            };

            if (data.returnCode != 0) {
                this.showErrorMessage(data);
                return;
            };

            if (data && data.partResults && data.partResults.length > 0) {
                var chartPart, snapShotPart, keywordsPart, recentArticlesPart;
                _.each(data.partResults, function (partResult) {
                    switch (partResult.packageType) {
                        case "CompanySnapshotPackage": snapShotPart = partResult; break;
                        case "CompanyChartPackage": chartPart = partResult; break;
                        case "CompanyRecentArticlesPackage": recentArticlesPart = partResult; break;
                        case "CompanyTrendingPackage": keywordsPart = partResult; break;
                    }
                }, this);

                this._languageCode = this._canvas.get_Preferences().interfaceLanguage;

                this._setContainers();
                this._setCompanyDetails(data);
                this.hasPublicCompanyInformation = data.hasPublicCompanyInformation;

                this._bindKeywords(keywordsPart);
                this._bindRecentArticles(recentArticlesPart);

                if (!this._zoom) {
                    this._bindChart(chartPart);
                    this._bindCompanySnapshot(snapShotPart);
                    this.showContentArea();
                }
                else {
                    this.recentArticlesContainer.hideLoading();
                    this.keywordsContainer.hideLoading();
                }
            }
            else {
                if (!this._zoom) {
                    this.showErrorMessage(data);
                }
                else {
                    this.recentArticlesContainer.hideLoading();
                    this.keywordsContainer.hideLoading();
                }
            }

            if (!this._zoom && this._partsLoaded == 0) {
                $(this.selector.noResultsContainer, this.$element).show();
            }
        },

        _onError: function (response) {
            if (!this._zoom) {
                if (response)
                    this.showErrorMessage({ returnCode: response.code, statusMessage: response.message });
                else
                    this.showErrorMessage({ returnCode: 'unknown', statusMessage: 'data is undefined or null' });
            }
            else {
                this.recentArticlesContainer.hideLoading();
                this.keywordsContainer.hideLoading();
                var errMsg = "";
                if (response)
                    errMsg = "<%= Token("error") %> " + response.code + ': ' + response.message;
                else
                    errMsg = "<%= Token("error") %> unknown: data is undefined or null";

                alert(errMsg);
            }
        },

        validateBuildProperties: function (paramsObj) {
            errObj = {};
            if (paramsObj.fcodes.length < 1) { errObj.CompanyCount = 0; }
            if ($.trim(paramsObj.title).length < 1) { errObj.TitleLength = 0; }
            if (errObj.CompanyCount < 1 || errObj.TitleLength === 0) { return errObj; }
        },

        _companyOverviewUpdateClick: function (sender, data) {
            var validationObj = this.validateBuildProperties(data);
            if (!validationObj) {
                $dj.proxy.invoke({
                    url: this.options.createUpdateCompanyOverviewModuleUrl,
                    data: {
                        "title": data.title,
                        "description": data.description,
                        "fcodes": data.fcodes,
                        "moduleId": this.options.moduleId,
                        "pageId": data.pageId
                    },
                    method: 'PUT',
                    controlData: this._canvas.get_ControlData(),
                    preferences: this._canvas.get_Preferences(),
                    onSuccess: $dj.delegate(this, this._onCompanyOverviewUpdateCallSuccess, { "sender": sender, "moduleId": this.options.moduleId, "moduleTitle": data.title, "moduleDescription": data.description
                    }),
                    onError: $dj.delegate(this, this._onCompanyOverviewUpdateCallError, sender)
                });

                //Hide Edit and Show Content
                this.showContentArea();
            }
            else {
                this._onCompanyOverviewUpdateValidateError(validationObj);
            }

        },

        _onCompanyOverviewUpdateCallSuccess: function (data) {
            this.set_moduleId(data.moduleId);
            this.set_moduleTitle(data.moduleTitle);
            this._editor.options.moduleName = data.moduleTitle;
            this._editor.options.moduleDescription = data.moduleDescription;
            this.options.moduleDescription = data.moduleDescription;
            this._setDefaultProperties();
            this.getData();
        },

        _onCompanyOverviewUpdateCallError: function (sender, error) {
            sender.showErrorMessage({ returnCode: error.code, statusMessage: error.message });
        },

        _onCompanyOverviewUpdateValidateError: function (data) {
            var errMsg = "";
            if (data) {
                if (data.CompanyCount === 0 || data.TitleLength === 0) {
                    if (data.CompanyCount === 0) {
                        errMsg = "<%= Token("emptyCompanyCountMessage") %>";
                    }
                    if (data.TitleLength === 0) {
                        errMsg = errMsg + "<%= Token("emptyTitleMessage") %>";
                    }
                    alert(errMsg);
                    return false;
                }
            }
        },

        EOF: null

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CompanyOverviewCanvasModule', DJ.UI.CompanyOverviewCanvasModule);

    $dj.debug('Registered DJ.UI.CompanyOverviewCanvasModule as dj_CompanyOverviewCanvasModule');

})(jQuery);
