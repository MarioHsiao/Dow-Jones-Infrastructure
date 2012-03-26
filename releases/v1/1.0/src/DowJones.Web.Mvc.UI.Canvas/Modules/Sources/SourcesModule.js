/*!
 * SourcesCanvasModule
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

    DJ.UI.SourcesModule = DJ.UI.AbstractCanvasModule.extend({

        // client side properties here
        selectors: {
            sourceLogos: 'h3.module-col-title img.module-col-source-img',
            sourceTitles: 'h3.module-col-title span.module-col-title-source-icon-text',
            viewAllBtns: 'ul.view-all-btn a.dashboard-control',
            portalHeadlineLists: 'div.dj_headlineListContainer',
            footer: '.module-footer',
            pager: '.module-pager'
        },

        events: {
            sourcesViewAllClick: 'viewAll.dj.SourcesModule',
            sourceHeadlineClick: 'headlineClick.dj.SourcesModule'
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            this._super(element, meta);

            // get all control plugins and preserve a reference to avoid subsequent lookups
            // specifying scope + [tag.selector] pattern for ultra fast lookups
            this.sourceLogos = $(this.selectors.sourceLogos, this.$element);
            this.sourceTitles = $(this.selectors.sourceTitles, this.$element);
            this.portalHeadlineLists = $(this.selectors.portalHeadlineLists, this.$element);
            this.viewAllBtns = $(this.selectors.viewAllBtns, this.$element);
            this._portalHeadlinesCache = [];
            this._initializeHandlers();
        },

        _initializePager: function (numberOfPages, activePage) {
            if (!this.modulePager) {
                this.modulePager = $(this.selectors.footer, this.$element).dj_Pager({ options: { activePage: activePage, totalPages: numberOfPages} });
                this.$pager = $(this.selectors.pager, this.$element);
                this.pager = $(this.selectors.footer, this.$element).findComponent(DJ.UI.Pager);
                $.extend(this._delegates, {
                    OnSourcePagerClick: $dj.delegate(this, this._OnSourcePagerClick)
                });
                // register pager click handler
                if (this.$pager && this.pager) {
                    $(this.selectors.footer, this.$element).bind(this.pager.events.pagerClick, this._delegates.OnSourcePagerClick);
                }
            }
            else {
                this.pager.options.activePage = activePage;
                this.pager.options.totalPages = numberOfPages;
                this.pager._applyPager();
            }
            if (numberOfPages > 1) {
                this.pager._showHidePager(true);
            }
            else {
                this.pager._showHidePager(false);
            }
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this,this.showErrorMessage),
                OnPortalHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
            });
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

        _onSuccess: function (parmObj, data) {
            var errors = $dj.getError(data);
            if(errors){
                this.showErrorMessage(errors);
                this.pager._showHidePager(false);
            }
            else{
                if (data && data.partResults && data.partResults.length > 0) {
                    // push data into cache
                    this._portalHeadlinesCache[parmObj.pageIndex] = data.partResults;

                    // bind data to controls
                    this.setData(data.partResults, parmObj.pageIndex);

                    if (parmObj.initPager) {
                        //set the pager
                        this.options.numberOfPages = this._getNumberOfPages(data.maxPartsAvailable);
                        this._initializePager(this.options.numberOfPages, this.options.activePage);
                        this._pageIndex = this.options.activePage;
                    }
            }
            else {
                this.showMessage("<%= Token("noSourcesInModule") %>");
                if(this.pager)
                {
                    this.pager._showHidePager(false);
                }
            }
          }
        },

        _onHeadlineClick: function (event, data) {
            this._publish(this.events.sourceHeadlineClick, data);
        },

        _OnSourcePagerClick: function (event, data) {
            if (data && data.index) {
                this._slideDirection = (data.index < this._pageIndex) ? 'left' : 'right';
                this._pageIndex = data.index;
                this.getData(data.index);
            }
        },

        fireOnSaveAndCloseEditArea: function (e) {
            var editorProps = this._editor.buildProperties();
            this._publish('swap', editorProps);
        },

        _invalidatePortalHeadlineCache: function () {
            this._portalHeadlinesCache.length = 0;
        },

        refreshData: function () {
            this._invalidatePortalHeadlineCache();
            this.getData();
        },

        getData: function (pageIndex) {
            this._super();
            var initPager;
            if (!pageIndex) {
                pageIndex = 1;
                this._portalHeadlinesCache.length = 0;
                initPager = true;
            }

            this.options.firstSourceToReturn = (pageIndex - 1) * this.options.maxSourcesToReturn;
            if (!this._portalHeadlinesCache[pageIndex]) {
                $dj.proxy.invoke({
                    url: this.options.dataServiceUrl,
                    queryParams: {
                        "pageid": this._canvas.get_canvasId(),
                        "moduleid": this.get_moduleId(),
                        "firstPartToReturn": this.options.firstSourceToReturn,
                        "maxPartsToReturn": this.options.maxSourcesToReturn,
                        "firstResultToReturn": this.options.firstHeadlineToReturn,
                        "maxResultsToReturn": this.options.maxHeadlinesToReturn
                    },
                    controlData: this._canvas.get_ControlData(),
                    preferences: this._canvas.get_Preferences(),
                    onSuccess: $dj.delegate(this, this._onSuccess, { "pageIndex": pageIndex, "initPager": initPager }),
                    onError: this._delegates.OnServiceCallError
                });
            }
            else {
                this.setData(this._portalHeadlinesCache[pageIndex], pageIndex);
            }
        },

        setData: function (partResults, pageIndex) {
            // reset visibility if there's a mismatch between requested and received results
            if (partResults.length < this.options.maxSourcesToReturn) {
                this._hideSourceAreas();
            }

            _.each(partResults,
                    function (partResult) {

                        // find the position of the Portal Headline List
                        var controlIndex = this._getPortalHeadlineIndex(partResult.identifier, pageIndex);

                        this._bindSourceTitles(controlIndex, partResult);
                        this._bindSourceLogos(controlIndex, partResult);
                        this._bindHeadlines(controlIndex, partResult);
                        this._bindViewAllBtns(controlIndex, partResult);

                    }, this);


            if (partResults.length % 3 !== 0) {
                // since we've a 0-based system here, partResults.length will point to last + one position
                this._showAddContentSection(partResults.length);
            }

            if (this.modulePager) {
                this.showContentArea('slide', { direction: this._slideDirection }, 1000);
            }
            else {
                this.showContentArea();
            }
        },

        _getNumberOfPages: function (totalNumberOfSources) {
            return Math.floor(totalNumberOfSources % this.options.maxSourcesToReturn == 0 ? (totalNumberOfSources / this.options.maxSourcesToReturn) : (totalNumberOfSources / this.options.maxSourcesToReturn) + 1);
        },

        _getPortalHeadlineIndex: function (id, pageIndex) {
            var index = parseInt(id, 10) || 0;

            // get a 0-2 based index from the part identifier
            return (index - (this.options.maxSourcesToReturn * (pageIndex - 1)));
        },

        _showAddContentSection: function (idx) {
            // if not authorized, do nothing
            if (!this.options.canEdit) { return; }
            var phl = this.portalHeadlineLists[idx];
            if (phl) {
                $(phl).findComponent(DJ.UI.PortalHeadlineList).showEditSection();
            }
        },

        _bindSourceLogos: function (idx, partResult) {
            
            var self = this;
            var at = this.sourceLogos[idx];
            // populate the source logos
            if (at) {
                $(at)
                .error(function () {
                    self._onSourceLogoError(idx, partResult.package.sourceName);
                })
                .attr("src", partResult.package.sourceLogoUrl)
                .show();
//                .load(function () {
//                    self._onSourceLogoLoad(idx);
//                })
                
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

        _onSourceLogoError: function (idx, sourceName) {
            var att = this.sourceTitles[idx];
            if (att) {
                $(att).html(sourceName).show();
                var at = this.sourceLogos[idx];
                if (at) {
                    $(at).hide();
                }
            }
        },

        _bindSourceTitles: function (idx, partResult) {
            var at = this.sourceTitles[idx];
            if(!partResult.package.sourceLogoUrl)
            {
                if (at) {
                    $(at).html(partResult.package.sourceName).show();
                }
            }
            else{
                if (at) {
                    $(at).html("").hide();
                }
            }
        },

        _bindHeadlines: function (idx, partResult) {
            var phl = this.portalHeadlineLists[idx];
            if (!phl) { return; }

            if (!partResult) {
                // let the control display no data (based on options)
                $(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnSuccess(null);
                return;
            }

            if (partResult.returnCode === 0) {
                // get hold of the component by the index
                // make sure this collection is populated during init
                var headlines = partResult.package.portalHeadlineListDataResult.resultSet;
                $(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnSuccess(headlines);
                $(phl).show();
            }
            else {
                $(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnError({
                    'code': partResult.returnCode,
                    'message': partResult.statusMessage
                });
                $(phl).show();
            }
        },

        _bindViewAllBtns: function (idx, partResult) {
            // wire up the view all button
            var viewAllBtn = this.viewAllBtns[idx];
            if (partResult.returnCode !== 0 ||
                    (partResult.package && partResult.package.portalHeadlineListDataResult &&
                    partResult.package.portalHeadlineListDataResult.resultSet &&
                    partResult.package.portalHeadlineListDataResult.resultSet.count &&
                    partResult.package.portalHeadlineListDataResult.resultSet.count.value < this.options.maxHeadlinesToReturn)
                ) {
                $(viewAllBtn).hide();
                return;
            }

            // wire up the view all button
            $(viewAllBtn).unbind('click').click($dj.delegate(this, function (e) {
                this._publish(this.events.sourcesViewAllClick, {
                    modulePart: "<%= Token("sourcesLabel") %>: " + partResult.package.sourceName,
                    searchContext: partResult.package.viewAllSearchContext
                })
                e.stopPropagation();
                $dj.debug('Published ' + this.events.sourcesViewAllClick);
                return false;
            })).removeClass('hidden').show();
        },

        _hideSourceAreas: function () {
            var idx, len;
            for (idx = 0, len = this.options.maxSourcesToReturn; idx < len; idx++) {
                $(this.sourceLogos[idx]).unbind('error').hide();
                $(this.sourceTitles[idx]).html("").hide();
                $(this.portalHeadlineLists[idx]).hide();
                $(this.viewAllBtns[idx]).hide();
            }
        },

        EOF: null
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_SourcesModule', DJ.UI.SourcesModule);
    $dj.debug('Registered DJ.UI.SourcesModule as dj_SourcesModule');
})(jQuery);
