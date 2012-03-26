(function ($) {

	DJ.UI.SyndicationModule = DJ.UI.AbstractCanvasModule.extend({

		selectors: {
			newsFeedTitles: 'h3.module-col-title span.module-col-title-source-icon-text',
			viewAllBtns: 'ul.view-all-btn a.dashboard-control',
			viewAllErrDiv: 'div.dj_viewAllErr',
			newsFeedIcons: 'h3.module-col-title img.module-col-title-source-img',
			portalHeadlineLists: 'div.dj_headlineListContainer',
			footer: '.module-footer',
			feedArea: 'div.module-col'
		},

		events: {
			syndicationViewAllClick: 'viewAllClick.dj.SyndicationModule',
			syndicationHeadlineClick: 'headlineClick.dj.SyndicationModule',
			syndicationSourceClick: 'sourceClick.dj.SyndicationModule'
		},

		init: function (element, meta) {
			this._super(element, meta);

			this._portalHeadlinesCache = new Array(this.options.numPages);
			this._applyPager();
			this._registerChildEventHandlers();
		},

		_initializeElements: function (ctx) {
			this._super();

			this.newsFeedTitles = $(this.selectors.newsFeedTitles, ctx);
			this.portalHeadlineLists = $(this.selectors.portalHeadlineLists, ctx);
			this.viewAllBtns = $(this.selectors.viewAllBtns, ctx);
			this.viewAllErrDivs = $(this.selectors.viewAllErrDiv, ctx);
			this.newsFeedIcons = $(this.selectors.newsFeedIcons, ctx);
			this.feedAreas = $(this.selectors.feedArea, ctx);
			this.$footer = $(this.selectors.footer, ctx);
		},


		_initializeDelegates: function () {
			this._super();

			$.extend(this._delegates, {
				OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
				OnServiceCallError: $dj.delegate(this, this._onError),
				OnPortalHeadlineClick: $dj.delegate(this, this._onHeadlineClick),
				OnSourceClick: $dj.delegate(this, this._onSourceClick)
			});
		},


		_registerChildEventHandlers: function () {
			// register portal headline click handlers
			_.each(this.portalHeadlineLists, function (phl) {
				if (!this._portalEventMapCache) {
					var $fnPhl = $(phl).findComponent(DJ.UI.PortalHeadlineList);
					var _map = {};
					_map[$fnPhl.events.headlineClick] = this._delegates.OnPortalHeadlineClick;
					_map[$fnPhl.events.sourceClick] = this._delegates.OnSourceClick;

					this._portalEventMapCache = _map;
				}
				$(phl).bind(this._portalEventMapCache);

			}, this);
		},


		_applyPager: function (activePage) {
			var numPages = Math.floor((this.options.totalFeeds % this.options.feedsPerPage === 0)
                    ? this.options.totalFeeds / this.options.feedsPerPage
                    : this.options.totalFeeds / this.options.feedsPerPage + 1);

			if (numPages > 1) {
				this.modulePager =
                        this.$footer.dj_AbstractModulePager({
                        	options: {
                        		numPages: numPages,
                        		activePage: activePage || this.options.activePage,
                        		getDataHandler: $dj.delegate(this, this.getData),
                        		showContentAreaHandler: $dj.delegate(this, this.showContentArea)
                        	}
                        }).findComponent(DJ.UI.AbstractModulePager);
			}
		},

		_onHeadlineClick: function (event, data) {
			this._publish(this.events.syndicationHeadlineClick, data);
		},

		_onSourceClick: function (event, data) {
			this._publish(this.events.syndicationSourceClick, data);
		},

		_onSuccess: function (pageIndex, data) {
			var errors = $dj.getError(data);
			if (errors) {
				this.showErrorMessage(errors);
			}
			else {
				if (data && data.partResults && data.partResults.length > 0) {
					// push data into cache
					this._portalHeadlinesCache[pageIndex] = data.partResults;

					// bind data to controls
					this.setData(data.partResults, pageIndex);
				}
				else {
					this.showErrorMessage(errors);
				}
			}
		},


		setData: function (partResults, pageIndex) {

			// reset visibility to hide viewall, icons etc
			this._hideFeedAreas();

			_.each(partResults,
                    function (partResult) {
                    	// find the position of the Portal Headline List
                    	var controlIndex = this._getPortalHeadlineIndex(partResult.identifier, pageIndex);

                    	// eliminate the reserved word 'package' from all but one places
                    	var data = partResult.package,
                            returnCode = partResult.returnCode,
                            statusMessage = partResult.statusMessage;

                    	if (returnCode === 0) {
                    		this._bindNewsFeedTitles(controlIndex, data);
                    		this._bindNewsFeedIcons(controlIndex, data);

                    	}
                    	else {
                    		var errors = $dj.getError(partResult);
                    		this.showErrorMessage(errors);
                    	}

                    	this._bindViewAllBtn(controlIndex, data, returnCode);
                    	this._bindHeadlines(controlIndex, data, returnCode, statusMessage);
                    }, this);


			if (partResults.length % 3 !== 0) {
				// since we've a 0-based system here, partResults.length will point to last + one position
				//this._showAddContentSection(partResults.length);
			}

			if (this.modulePager) {
				this.modulePager.slideContentArea();
			}
			else {
				this.showContentArea();
			}
		},


		fireOnSaveAndCloseEditArea: function (e) {
			var editorProps = this._editor.buildProperties();
			if (this.validationPassed(editorProps)) {
				this._updateModuleDef(editorProps);
			}
		},


		validationPassed: function (props) {
			if (!props) {
				$dj.debug('SyndicationModule.validationPassed: Editor Props are null or undefined');
				return;
			}

			// check for at least one news feed
			if (!props.syndicationIds || props.syndicationIds.length === 0) {
				alert("<%= Token('noNewsFeed') %>");
				return;
			}

			// check for empty title
			if (!props.title) {
				alert("<%=Token('emptyTitleMessage')%>");
				return;
			}

			// check for illegal chars in description
			if (props.description.match(/[<>@#\\%+|]/)) {
				alert("<%=Token('illegalChar-1')%>" + " <>@#\\%+|");
				return;
			}

			// check for char limit in description
			if (props.description.length > 250) {
				alert("<%=Token('descriptionLimitMessage')%>");
				return;
			}

			// aal izzzz well!
			return true;
		},

		_updateModuleDef: function (props) {
			this._invalidatePortalHeadlineCache();

			$dj.proxy.invoke({
				url: this.options.moduleServiceUrl,
				data: $.extend({}, props, { moduleId: this.options.moduleId }),
				method: 'PUT',
				controlData: this._canvas.get_ControlData(),
				preferences: this._canvas.get_Preferences(),
				onSuccess: $dj.delegate(this, this._onSaveSuccess, props),
				onError: this._delegates.OnServiceCallError
			});
		},


		_onSaveSuccess: function (props, result) {
			// update module title
			this.set_moduleTitle(props.title);
			// update description on the editor
			this._editor.setDescription(props.description);

			this.options.totalFeeds = props.syndicationIds.length;

			this._applyPager();

			// get data and reset the page index to 1
			this.getData(1);
		},


		_getPortalHeadlineIndex: function (id, pageIndex) {
			var index = parseInt(id, 10) || 0;

			// get a 0-2 based index from the part identifier
			return (index - (this.options.feedsPerPage * (pageIndex - 1)));
		},


		_getDefaultModuleErrorObject: function () {
			return { returnCode: 'unknown', statusMessage: 'data is undefined or null' };
		},


		///<summary>
		/// Hides All visible components in a feed section
		///</summary>
		_hideFeedAreas: function () {

			var idx, len;
			for (idx = 0, len = this.options.feedsPerPage; idx < len; idx++) {

				$(this.newsFeedTitles[idx]).hide();
				$(this.newsFeedIcons[idx]).hide();
				$(this.portalHeadlineLists[idx]).hide();
				$(this.viewAllErrDivs[idx]).hide();
				$(this.viewAllBtns[idx]).closest('ul').hide();

			}
		},


		_showAddContentSection: function (idx) {
			// if not authorized, do nothing
			if (!this.options.canEdit) { return; }

			var phl = this.portalHeadlineLists[idx];

			if (phl) {
				$(phl).findComponent(DJ.UI.PortalHeadlineList).showEditSection();
			}

		},


		_onError: function (errorThrown, jqXHR, serverMessage) {
			this._hideFeedAreas(false);

			_.each(this.portalHeadlineLists, function (elem) {
				var error = errorThrown;

				if (errorThrown && errorThrown.error) {
					error = errorThrown.error;
				}

				$(elem).findComponent(DJ.UI.PortalHeadlineList).bindOnError(error);
			});

			this.showContentArea();
		},


		_bindNewsFeedTitles: function (idx, data) {
			// populate the news feed title
			var nft = this.newsFeedTitles[idx];
			var feedTitle = data.feedTitle || "(<%= Token('rssEmptyTitle')%>)";
			$(nft).html(feedTitle).show();

		},


		_bindNewsFeedIcons: function (idx, data) {
			if (!data || !data.faviconUri) { return; }

			var nfi = this.newsFeedIcons[idx];
			// restore padding first
			var h3 = $(nfi).parent();
			if (!h3.data('padding-left')) {
				h3.data('padding-left', h3.css('padding-left'));
			}

			// start with 0 to avoid flickr effect on title getting adjusted after image load
			h3.css('padding-left', '0');

			// IE7 won't trigeer onerror properly if src is set first
			$(nfi).attr("alt", data.feedTitle)
				  .one("error", function () {
				  	$(this).hide();
				  })
				  .one("load", function () {
				  	h3.css('padding-left', h3.data('padding-left'));
				  	$(this).show();
				  })
				  .attr("src", data.faviconUri);
		},


		_bindHeadlines: function (idx, data, returnCode, statusMessage) {
			// get hold of the component by the index
			// make sure this collection is populated during init
			var phl = this.portalHeadlineLists[idx];
			if (!phl) { return; }

			if (returnCode !== 0) {
				$(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnError({
					'code': returnCode,
					'message': statusMessage
				});

				$(phl).show();
				return;
			}

			if (!data) {
				// let the control display no data (based on options)
				$(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnSuccess(null);
			}
			else {
				var headlines = data.portalHeadlineListDataResult.resultSet;
				$(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnSuccess(headlines);
			}
			$(phl).show();
		},


		_bindViewAllBtn: function (idx, data, returnCode) {
			// wire up the view all button

			var $viewAllErrDiv = $(this.viewAllErrDivs[idx]);
			var $viewAllBtn = $(this.viewAllBtns[idx]);

			if (returnCode === 0) {
				$viewAllBtn.html("<%= Token('viewAll') %>");
				$viewAllErrDiv.hide();
			}
			else {
				// show the button only if there's a feed URI
				if (data && data.htmlPageForFeedUri) {
					$viewAllErrDiv.show();
					$viewAllBtn.html("<%= Token('viewFeed') %>");
					$viewAllBtn.show();
				}
				else {
					$viewAllErrDiv.hide();
					$viewAllBtn.hide();
				}
			}

			if (data) {
				var me = this;
				$viewAllBtn.unbind('click').click(function (e) {
					me._publish(me.events.syndicationViewAllClick, {
						feedTitle: data.feedTitle,
						feedUri: data.htmlPageForFeedUri
					});

					e.stopPropagation();
					$dj.debug('Published', me.events.syndicationViewAllClick);
					return false;
				});
			}


			$viewAllBtn.closest('ul').show();
		},


		_getQueryParams: function (pageIndex) {

			var firstPartToReturn = 0
                , firstResultToReturn = 0;


			firstPartToReturn = (pageIndex - 1) * this.options.feedsPerPage;


			return {
				"pageid": this._canvas.get_canvasId(),
				"moduleid": this.get_moduleId(),
				"firstPartToReturn": firstPartToReturn,
				"maxPartsToReturn": Math.min(this.options.feedsPerPage, this.options.totalFeeds),
				"firstResultToReturn": firstResultToReturn,
				"maxResultsToReturn": this.options.maxResultsPerFeed
			};
		},



		_invalidatePortalHeadlineCache: function () {
			this._portalHeadlinesCache = [];
		},

		refreshData: function () {
			this._invalidatePortalHeadlineCache();
			if (this.modulePager) {
				this.modulePager.setActivePage(1);
			}
			this.getData();
		},

		getData: function (pageIndex) {
			// show loading area etc.
			this._super();

			pageIndex = pageIndex || 1;

			if (this._portalHeadlinesCache[pageIndex] === undefined) {
				$dj.proxy.invoke({
					url: this.options.dataServiceUrl,
					queryParams: this._getQueryParams(pageIndex),
					controlData: this._canvas.get_ControlData(),
					preferences: this._canvas.get_Preferences(),
					onSuccess: $dj.delegate(this, this._onSuccess, pageIndex),
					onError: this._delegates.OnServiceCallError
				});
			}
			else {
				this.setData(this._portalHeadlinesCache[pageIndex], pageIndex);
			}
		},


		EOF: null
	});

	$.plugin('dj_SyndicationModule', DJ.UI.SyndicationModule);

	$dj.debug('Registered DJ.UI.SyndicationModule as dj_SyndicationModule');


} (jQuery));
