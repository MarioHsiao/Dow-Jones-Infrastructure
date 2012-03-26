/// <reference name="jquery.js" assembly="DowJones.Web.Mvc" />
/// <reference name="common.js" assembly="DowJones.Web.Mvc" />
/// <reference name="ServiceProxy.js" assembly="DowJones.Web.Mvc" />

(function ($) {

    DJ.UI.SyndicationModuleEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

        //#region Private Members

        selectors: {
            subscribedFeedsList: 'ul.dj_edit-lists',
            name: 'input.dj_Edit_PageName',
            descriptionEdit: 'textarea.dj_Edit_PageDescription',
            addNewsFeedToggle: 'a.add-feeds-toggle',
            addNewsFeed: 'a.dc_btn-save',
            newItemBox: 'div.new-item-box',
            addControlsArea: 'div.dj_edit-lists-add-controls',
            listMessage: 'div.list-message',
            feedItem: '.editable-item',
            feedEntry: '.editable-item .label',
            feedCancel: '.editable-item .dc_btn-cancel',
            feedRemove: '.editable-item .dc_btn-remove'
        },

        templates: {
            feeds: _.template(['<li class="sortable-item editable-item"><span class="reorder-icon"></span>',
                '<span class="feedId hide"><%= id %></span><span class="label"><%= url %></span></li>'
            ].join(''))

        },

        //#endregion

        //#region Implementing abstract members

        _initializeElements: function (ctx) {
            this.$pageName = $(this.selectors.name, ctx);
            this.$desciptionEdit = $(this.selectors.descriptionEdit, ctx);
            this.$subscribedFeedsList = $(this.selectors.subscribedFeedsList, ctx);
            this.$addNewsFeedToggle = $(this.selectors.addNewsFeedToggle, ctx);
            this.$newItemBox = $(this.selectors.newItemBox, ctx);
            this.$addControlsArea = $(this.selectors.addControlsArea, ctx);
            this.$listMessage = $(this.selectors.listMessage, ctx);

            this.$newItemBoxInput = this.$newItemBox.find('input');
            this.$addNewsFeed = this.$newItemBox.find(this.selectors.addNewsFeed);

            this.$subscribedFeedsList.sortable();
        },

        _initializeDelegates: function () {
            this._super();

            $.extend(this._delegates, {
                OnAddNewsFeed: $dj.delegate(this, this._onAddNewsFeed)
            });
        },

        _initializeEventHandlers: function () {
            this._super();

            var me = this;
            this.$addNewsFeedToggle.click(function (e) {
                if ($(this).hasClass('state-can-add-item')) {
                    if (me._haveMaximumItems()) {
                        me._maxedOutItems();
                    }
                    else if (me._haveNoItems()) {
                        me._noItems();
                    }
                    else {
                        me._makeAvailableToAddItems();
                    }

                }
                else {
                    me._clearError();
                    $(this).addClass('state-can-add-item');
                    $(this).text("<%= Token('doneAddNewsFeeds') %>");

                    me.$newItemBox.removeClass('hide');
                }
                return false;
            });

            this.$addNewsFeed.bind('click', function (e) {
                var feedUrl = $.trim(me.$newItemBoxInput.val());
                if (feedUrl) {
                    me._onAddNewsFeed(feedUrl);
                }
                else {
                    //TODO: Show Error
                }
                e.stopPropagation();
                return false;
            });

            this.$desciptionEdit.bind('keypress', function () {
                if (this.value.length > 250) {
                    this.value = this.value.substring(0, 250);
                    return false;
                }
            });


            // attach handler to show/hide remove/Cancel buttons to feed URIs
            this.$subscribedFeedsList.delegate(this.selectors.feedEntry, 'click', function (e) {
                me._quitEditFeedMode();
                var $editableItem = $($(this).parent());
                $editableItem.addClass('focus');
                if ($editableItem.find('.controls').length < 1) {
                    $editableItem.append('<div class="controls"><ul class="dc_list"><li class="dc_item"><a href="#" class="dashboard-control dc_btn dc_btn-2 dc_btn-remove"><%= Token("moduleMenuRemove") %></a></li><li class="dc_item"><a href="#" class="dashboard-control dc_btn dc_btn-3 dc_btn-cancel"><%= Token("cancel") %></a></li></ul></div>');
                }

                e.stopPropagation();
                return false;
            });

            this.$subscribedFeedsList.delegate(this.selectors.feedRemove, 'click', $dj.delegate(this, this._onFeedRemove));

            this.$subscribedFeedsList.delegate(this.selectors.feedCancel, 'click', function (e) {
                me._quitEditFeedMode();
                e.stopPropagation();
                return false;
            });

            this.$subscribedFeedsList.delegate(this.selectors.feedItem, 'mouseover', function (e) {
                $(this).addClass('hover');
            });
            this.$subscribedFeedsList.delegate(this.selectors.feedItem, 'mouseout', function (e) {
                $(this).removeClass('hover');
            });
        },

        init: function (element, meta) {
            this._super(element, meta);

        },


        buildProperties: function () {
            return {
                "description": this.$desciptionEdit.val(),
                "title": this.$pageName.val(),
                "pageId": this.getCanvas().get_canvasId(),
                "syndicationIds": this._getFeedIds()
            };
        },


        saveProperties: function (props, callback) {
            $dj.proxy.invoke(
            {
                url: this.options.moduleServiceUrl,
                data: this.buildProperties(),
                method: 'POST',
                controlData: this.getCanvas().get_ControlData(),
                preferences: this.getCanvas().get_Preferences(),
                onSuccess: $dj.delegate(this, this._onSaveSuccess, callback),
                onError: $dj.delegate(this, this._onSaveError, callback)
            }
                    );
        },


        onShow: function () {
            // clear out stuff from previous show and cancel
            this._resetState();

            this.setData();
        },


        setData: function () {
            this.setName();
            this.setDescription();
            this.populatefeeds();
        },


        isDupFeed: function (feedId) {
            if (!feedId) {
                $dj.debug('Feed ID not specified');
                return false;
            }

            return _.find(this._getFeedIds(), function (id) {
                return id === feedId;
            });
        },

        //#endregion

        //#region Private Methods

        _getFeedIds: function () {
            return _.map($('li > span.feedId', this.$subscribedFeedsList),
                    function (item) {
                        return item.innerHTML;
                    });
        },


        _onAddNewsFeed: function (feedUrl) {
            // disable add until validation succeeds
            this._toggleAddNew();

            // treat feed:// as http://
            feedUrl = feedUrl.replace(/^\s*feed:/g, 'http:');

            // see if we're already in middle of a validation
            if (this._validating && this._prevUrl === feedUrl) {
                $dj.debug('Validation in progress. Suppressing possible multiple clicks.');
                return;
            }

            // set a flag to guard against multiple clicks
            this._validating = true;
            this._prevUrl = feedUrl;



            $dj.proxy.invoke({
                url: this.options.webServiceUrl,
                data: { url: feedUrl },
                method: 'POST',
                controlData: this.getCanvas().get_ControlData(),
                preferences: this.getCanvas().get_Preferences(),
                onSuccess: $dj.delegate(this, this._onFeedValidationSuccess, feedUrl),
                onError: $dj.delegate(this, this._onFeedValidationError, feedUrl),
                onComplete: $dj.delegate(this, this._onFeedValidationComplete)
            });
        },


        _onFeedRemove: function (e) {
            $(e.target).parents(this.selectors.feedItem).remove();
            if (this.$subscribedFeedsList.children().length < 1) {
                $(".dj_edit-list-empty-msg").removeClass('hide');
            }
            else if (this.$subscribedFeedsList.children().length >= 1
                    && this.$subscribedFeedsList.children().length < this.options.maximumFeeds) {
                $(".list-message").addClass('hide');
            }

            if (this._haveMaximumItems()) {
                this._maxedOutItems();
            }
            else if (this._haveNoItems()) {
                this._noItems();
            }
            else { //we just removed enough to warrant another being added
                this._makeAvailableToAddItems();
            }
            this._quitEditFeedMode();
            e.stopPropagation();
            return false;
        },


        _onFeedValidationSuccess: function (feedUrl, result) {

            if (!result) {
                this._onError("<%= Token('feedUriValidationError') %>");
                return;
            }
            else if (!result.syndicationId) {
                this._onError("<%= Token('invalidFeedUri') %>");
                return;
            }

            if (this.isDupFeed(result.syndicationId)) {
                this._onError("<%= Token('dupFeedUriError') %>");
            }
            else {
                this.$subscribedFeedsList.append(this.templates.feeds({ url: feedUrl, id: result.syndicationId }));
                this._clearError(); //clear any message

                if (this._haveMaximumItems()) {
                    this._maxedOutItems();
                }
            }

        },


        _onFeedValidationError: function (feedUrl, error) {
            if (error) {
                this._onError($dj.formatError(error));
            }
            else {
                $dj.debug('Unknown error', arguments);
            }
        },

        _onFeedValidationComplete: function (jqXHR, textStatus) {
            $dj.debug(jqXHR, textStatus);

            // enable add
            this._toggleAddNew();


            this._validating = false;
        },


        _quitEditFeedMode: function () {
            $(".editable-item", this.$subscribedFeedsList).each(function () {
                var $label = $(this);
                var $li = $label.closest('li');
                $li.height(20);
                $li.css('height', '');
                $li.removeClass('focus').removeClass('hover');

                if ($label.find(".controls").length > 0) {
                    $label.find(".controls").remove();
                }
            });
        },


        _maxedOutItems: function () {
            this._onError("<%= Token('maxNewsFeeds') %>");
            this.$newItemBox.addClass('hide');
            this.$addNewsFeedToggle.text('').removeClass('state-can-add-item').addClass('hide');
        },


        _makeAvailableToAddItems: function () {
            this._clearError();
            this.$addNewsFeedToggle.text("<%= Token('addNewsFeeds') %>").removeClass('hide state-can-add-item');
            this.$newItemBox.addClass('hide');
        },


        _clearError: function () {
            this.$listMessage.addClass('hide').text('');
        },


        _onError: function (msg) {
            if (!msg || !this.$listMessage) { return; }

            this.$listMessage.text(msg).removeClass('hide');
        },


        _noItems: function () {
            this._onError("<%= Token('noNewsFeed') %>");
            this.$addNewsFeedToggle.text("<%= Token('addNewsFeeds') %>").removeClass('hide state-can-add-item');
            this.$newItemBox.addClass('hide');
        },


        _haveMaximumItems: function () {
            return (this._getFeedIds().length > this.options.maximumFeeds);
        },

        _haveNoItems: function () {
            return this.$subscribedFeedsList.find(this.selectors.feedItem).length === 0;
        },


        _onSaveSuccess: function (callback, result) {
            $dj.debug('success');

            this._publish('SyndicationEdit', result);

            if (callback && $.isFunction(callback)) {
                callback(result);
            }
        },


        _onSaveError: function (callback) {
            $dj.debug('error');

            var returnObj = { status: -1, msg: "<%= Token('genericFrameworkError') %>: " };
            if (callback && $.isFunction(callback)) {
                callback(returnObj);
            }

        },

        _inEditMode: function () {
            if (this.options.moduleId) {
                return true;
            }
            else {
                return false;
            }
        },



        _resetState: function () {

            this._clearError();
            this.$addNewsFeed.text("<%= Token('addThisNewsFeeds') %>");
            this.$newItemBoxInput.val('');
            this.$subscribedFeedsList.html('');
            this.setName();
            this.setDescription();
        },


        _toggleAddNew: function () {
            var disable = 'dc_btn-3', enable = 'dc_btn-2';
            if (this.$addNewsFeed.hasClass(disable)) {
                this.$addNewsFeed.removeClass(disable).addClass(enable);
                this.$addNewsFeed.css('cursor', 'hand');
                this.$addNewsFeed.text("<%= Token('addThisNewsFeeds') %>");

                this.$newItemBoxInput.val('');

                this.$addControlsArea.hideLoading();
            }
            else {
                this.$addNewsFeed.removeClass(enable).addClass(disable);
                this.$addNewsFeed.css('cursor', 'pointer');
                this.$addNewsFeed.text("<%= Token('validatingMsg') %>");


                this.$addControlsArea.showLoading();
            }
        },

        _getFeeds: function () {
            // show a loading message till we get the feeds
            this.$element.showLoading();

            $dj.proxy.invoke({
                url: this.options.moduleServiceUrl,
                queryParams: { pageId: this.options.canvasId, moduleId: this.options.moduleId },
                method: 'GET',
                controlData: this.getCanvas().get_ControlData(),
                preferences: this.getCanvas().get_Preferences(),
                onSuccess: $dj.delegate(this, this._onGetFeedSuccess),
                onError: $dj.delegate(this, this._onGetFeedError),
                onComplete: $dj.delegate(this, this._onGetFeedComplete)
            });
        },


        _onGetFeedSuccess: function (feeds) {
            var syndicationItems = feeds.package.syndicationItems;
            this.$subscribedFeedsList.html('');
            _.each(syndicationItems, function (syndicationItem) {
                this.$subscribedFeedsList.append(this.templates.feeds(syndicationItem));
            }, this);



        },


        _onGetFeedError: function (error) {
            // TODO: Add error handling
            $dj.debug('SyndicationModuleEditor._onGetFeedError:', error)
            if (error) { this._onError($dj.formatError(error)); }
        },

        _onGetFeedComplete: function () {
            if (this._haveMaximumItems()) {
                this._maxedOutItems();
            }
            else if (this._haveNoItems()) {
                this._noItems();
            }

            this.$element.hideLoading();

        },

        //#endregion

        //#region Public Methods

        setName: function () {
            var moduleName = this.options.moduleName || '';

            this.$pageName.val(moduleName);

        },


        setDescription: function (desc) {
            if (desc) { this.options.moduleDescription = desc; }
            desc = this.options.moduleDescription || '';
            this.$desciptionEdit.val(desc);
        },


        populatefeeds: function () {
            if (this._inEditMode()) {

                this._getFeeds();
            }

        }

        //#endregion

    });

    $.plugin('dj_SyndicationModuleEditor', DJ.UI.SyndicationModuleEditor);

    $dj.debug('Registered DJ.UI.SyndicationModuleEditor as dj_SyndicationModuleEditor');


})(jQuery);
