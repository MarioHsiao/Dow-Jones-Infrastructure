/*!
 * AlertsCanvasModule
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
    DJ.UI.AlertsCanvasModule = DJ.UI.AbstractCanvasModule.extend({
        // client side properties here
        selectors: {
            alertTitles: 'h3.module-col-title',
            viewAllBtns: 'ul.view-all-btn a.dashboard-control',
            portalHeadlineLists: 'div.dj_headlineListContainer',
            footer: '.module-footer',
            pager: '.module-pager',
            overlay: '.alertsOverlay'
        },

        events: {
            alertsViewAllClick: 'viewAll.dj.AlertsCanvasModule',
            alertHeadlineClick: 'headlineClick.dj.AlertsCanvasModule'
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            this._super(element, meta);
            // get all control plugins and preserve a reference to avoid subsequent lookups
            // specifying scope + [tag.selector] pattern for ultra fast lookups
            
        },
        
        _initializeElements: function (ctx) {
            this._super();

            this.alertTitles = $(this.selectors.alertTitles, ctx);
            this.portalHeadlineLists = $(this.selectors.portalHeadlineLists, ctx);
            this.viewAllBtns = $(this.selectors.viewAllBtns, ctx);
            if(this._editor)
            this.alertsOverlay = $(this.selectors.overlay, this._editor.$element)[0];
            this._portalHeadlinesCache = [];
            this._initializeHandlers();

            if (this.alertsOverlay) {
                //Assign Dynamic Id to overlay div (alertsOverlay-moduleId)
                this.alertsOverlay.id = "alertsOverlay" + '-' + this.options.moduleId;

                //Bind ovelay buttons
                this._bindOverlayButtons();
            }
        },

        _initializePager: function (numberOfPages, activePage) {
            if (!this.modulePager) {
                this.modulePager = $(this.selectors.footer, this.$element).dj_Pager({ options: { activePage: activePage, totalPages: numberOfPages} });
                this.$pager = $(this.selectors.pager, this.$element);
                this.pager = $(this.selectors.footer, this.$element).findComponent(DJ.UI.Pager);
                $.extend(this._delegates, {
                    OnAlertsPagerClick: $dj.delegate(this, this._onAlertsPagerClick)
                });
                // register pager click handler
                if (this.$pager && this.pager) {
                    $(this.selectors.footer, this.$element).bind(this.pager.events.pagerClick, this._delegates.OnAlertsPagerClick);
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
                OnServiceCallError: $dj.delegate(this, this._onError),
                OnPortalHeadlineClick: $dj.delegate(this, this._onHeadlineClick),
                OnAlertUpdateClick: $dj.delegate(this, this._onAlertsSaveClick)
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

        _onError: function (errorThrown, jqXHR, serverMessage) {
            this._hideAlertAreas();
            this.showErrorMessage({ returnCode: errorThrown.code, statusMessage: errorThrown.message });
        },

        _onHeadlineClick: function (event, data) {
            this._publish(this.events.alertHeadlineClick, data);
        },

        _onAlertsPagerClick: function (event, data) {
            if (data && data.index) {
                this._slideDirection = (data.index < this._pageIndex) ? 'left' : 'right';
                this._pageIndex = data.index;
                this.getData(data.index);
            }
        },

        getData: function (pageIndex) {
            this._super();
            var initPager;
            if (!pageIndex) {
                pageIndex = 1;
                this._portalHeadlinesCache.length = 0;
                initPager = true;
            }

            this.options.firstAlertToReturn = (pageIndex - 1) * this.options.maxAlertsToReturn;
            if (!this._portalHeadlinesCache[pageIndex]) {
                $dj.proxy.invoke({
                    url: this.options.dataServiceUrl,
                    queryParams: {
                        "pageid": this._canvas.get_canvasId(),
                        "moduleid": this.get_moduleId(),
                        "firstPartToReturn": this.options.firstAlertToReturn,
                        "maxPartsToReturn": this.options.maxAlertsToReturn,
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

        _onSuccess: function (parmObj, data) {
            if (!data) {
                this.showErrorMessage({ returnCode: '-1', statusMessage: '<%=Token("errorForMinus1")%>' });
                this.pager._showHidePager(false);
                return;
            };

            if (data.returnCode != 0) {
                this.showErrorMessage(data);
                this.pager._showHidePager(false);
                return;
            };

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
                this.showErrorMessage(data);
                if (this.pager) {
                    this.pager._showHidePager(false);
                }
            }
        },

        setData: function (partResults, pageIndex) {
            // reset visibility if there's a mismatch between requested and received results
            if (partResults.length < this.options.maxAlertsToReturn) {
                this._hideAlertAreas();
            }

            _.each(partResults,
                    function (partResult) {
                        // find the position of the Portal Headline List
                        var controlIndex = this._getPortalHeadlineIndex(partResult.identifier, pageIndex);
                        this._bindAlertTitles(controlIndex, partResult);
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

        _getNumberOfPages: function (totalNumberOfAlerts) {
            return Math.floor(totalNumberOfAlerts % this.options.maxAlertsToReturn == 0 ? (totalNumberOfAlerts / this.options.maxAlertsToReturn) : (totalNumberOfAlerts / this.options.maxAlertsToReturn) + 1);
        },

        fireOnSaveAndCloseEditArea: function (e) {
            var editorProps = this._editor.buildProperties();
            this._onAlertsSaveClick(this, editorProps);
        },

        validateBuildProperties: function (paramsObj) {
            errObj = {};
            if (paramsObj.alerts.length < 1) { errObj.AlertListCount = 0; }
            if ($.trim(paramsObj.title).length < 1) { errObj.TitleLength = 0; }
            if (errObj.AlertListCount < 1 || errObj.TitleLength === 0) { return errObj; }
        },

        _getPortalHeadlineIndex: function (id, pageIndex) {
            var index = parseInt(id, 10) || 0;
            // get a 0-2 based index from the part identifier
            return (index - (this.options.maxAlertsToReturn * (pageIndex - 1)));
        },

        _showAddContentSection: function (idx) {
            // if not authorized, do nothing
            if (!this.options.canEdit) { return; }
            var phl = this.portalHeadlineLists[idx];
            if (phl) {
                $(phl).findComponent(DJ.UI.PortalHeadlineList).showEditSection();
            }
        },

        _bindAlertTitles: function (idx, partResult) {
            if (partResult.returnCode !== 0) { return; }
            var at = this.alertTitles[idx];
            // populate the alert title
            if (at) {
                $(at).html(partResult.package.alertName).show();
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
                this._publish(this.events.alertsViewAllClick, {
                    alertId: partResult.package.alertId,
                    modulePart: "<%= Token("alertLabel") %>: " + partResult.package.alertName,
                    viewAllSearchContext: partResult.package.viewAllSearchContext
                });
                e.stopPropagation();
                $dj.debug('Published ' + this.events.alertsViewAllClick);
                return false;
            })).removeClass('hidden').show();
        },

        _hideAlertAreas: function () {
            var idx, len;
            for (idx = 0, len = this.options.maxAlertsToReturn; idx < len; idx++) {
                $(this.alertTitles[idx]).hide();
                $(this.portalHeadlineLists[idx]).hide();
                $(this.viewAllBtns[idx]).hide();
            }

        },

        _bindOverlayButtons: function () {
            var self = this;

            //Bind close button
            var overlayClose = $('p.modalClose', this.alertsOverlay)[0];
            if (overlayClose) {
                $(overlayClose).click(function () { self._hideAlertsOverlay(self) });
            }

            //Bind proceed button
            var overlayProceed = $('a.dj_modal-proceed', this.alertsOverlay)[0];
            if (overlayProceed) {
                $(overlayProceed).click(function () {
                    //set make public flag to true for the selected private alerts
                    var data = self._editor.buildProperties();
                    _.each(data.alerts, function (alertObj) {
                        if (alertObj.isPrivate === "true") {
                            alertObj.makePublic = "true";
                        }
                    }, this);
                    self.updateAlerts(this, data);
                    self._hideAlertsOverlay(self);
                });
            }

            //Bind cancel button            
            var overlayCancel = $('a.dj_modal-close', this.alertsOverlay)[0];
            if (overlayCancel) {
                $(overlayCancel).click(function () { self._hideAlertsOverlay(self) });
            }
        },

        _hideAlertsOverlay: function (self) {
            var overlayDiv = $("#alertsOverlay-" + self.options.moduleId);
            $().overlay.hide("#" + $(overlayDiv)[0].id);
            return false;
        },

        //Fuction to check for private alerts. If private show the overlay.
        _checkForPrivateAlerts: function (data) {
            var personalAlertsObj = {};
            personalAlertsObj.PrivateAlerts = [];
            _.each(data.alerts, function (alertObj) {
                if (alertObj.isPrivate === "true") {
                    personalAlertsObj.PrivateAlerts[personalAlertsObj.PrivateAlerts.length] = alertObj;
                }
            }, this);

            // personalAlertsObj.callback;
            return personalAlertsObj;
        },

        _onAlertsSaveClick: function (sender, data) {
            if (this._canvas.options.isPublished) {
                var privateAlertsObj = this._checkForPrivateAlerts(data);
                if (privateAlertsObj && privateAlertsObj.PrivateAlerts.length > 0) {
                    this.callAlertsOverlay(privateAlertsObj);
                }
                else { this.updateAlerts(sender, data); }
            }
            else { this.updateAlerts(sender, data); }
        },

        callAlertsOverlay: function (personalAlertsObj) {
            var publishAlertsOverlayTemplate = this._editor.templates.publishAlertsOverlay(personalAlertsObj);
            var overlayId = "alertsOverlay-" + this.options.moduleId;
            var overlayDiv = $("#" + overlayId);
            if (overlayDiv.length == 0)
                overlayDiv = $("#" + overlayId, this._editor.$element);
            //show overlay
            $(".modalContent", overlayDiv).html(publishAlertsOverlayTemplate);
            overlayDiv.show().overlay({ closeOnEsc: true });

        },

        updateAlerts: function (sender, data) {
            var validationObj = this.validateBuildProperties(data);
            if (!validationObj) {
                $dj.proxy.invoke({
                    url: this.options.createUpdateAlertModuleUrl,
                    data: {
                        "title": data.title,
                        "description": data.description,
                        "alerts": data.alerts,
                        "moduleId": this.options.moduleId,
                        "pageId": data.pageId
                    },
                    method: 'PUT',
                    controlData: this._canvas.get_ControlData(),
                    preferences: this._canvas.get_Preferences(),
                    onSuccess: $dj.delegate(this, this._onAlertsEditCallSuccess, { "moduleId": this.options.moduleId, "moduleTitle": data.title, "moduleDescription": data.description }),
                    onError: $dj.delegate(this, this._onAlertsEditCallError, sender)
                });
                
            //Hide Edit and Show Content
            this.showContentArea();

            }
            else {
                this._onAlertsEditValidateError(validationObj);
            }
        },

        _onAlertsEditCallSuccess: function (data) {
            this.set_moduleId(data.moduleId);
            this.set_moduleTitle(data.moduleTitle);
            this._editor.options.moduleName = data.moduleTitle;
            this._editor.options.moduleDescription = data.moduleDescription;
            this.getData();
        },

        _onAlertsEditCallError: function (sender, error) {
            sender.showErrorMessage({ returnCode: error.code, statusMessage: error.message });
        },

        _onAlertsEditValidateError: function (data) {
            var errMsg = "";
            if (data) {
                if (data.AlertListCount === 0 || data.TitleLength === 0) {
                    if (data.AlertListCount === 0) {
                        errMsg = "<%= Token("emptyAlertCountMessage") %>";
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
    $.plugin('dj_AlertsCanvasModule', DJ.UI.AlertsCanvasModule);
    $dj.debug('Registered DJ.UI.AlertsCanvasModule as dj_AlertsCanvasModule');
})(jQuery);






