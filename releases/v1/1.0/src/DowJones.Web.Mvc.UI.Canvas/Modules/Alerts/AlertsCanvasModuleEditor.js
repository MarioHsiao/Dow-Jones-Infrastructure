/// <reference name="jquery.js" assembly="DowJones.Web.Mvc" />
/// <reference name="common.js" assembly="DowJones.Web.Mvc" />
/// <reference name="ServiceProxy.js" assembly="DowJones.Web.Mvc" />

(function ($) {

    DJ.UI.AlertsCanvasModuleEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

        selectors: {
            subscribedFeedsList: 'ul.subscribed-news-feeds',
            name: 'input.dj_Edit_PageName',
            descriptionHead: 'div.dj_Edit_PageDescription',
            descriptionEdit: 'textarea.dj_Edit_PageDescription'
        },

        templates: {
            feedUris: _.template(['<% for (var i = 0, len = obj.length; i < len; i++) { ',
                                  'feedUri = obj[i]; %>',
                                    '<li class="sortable-item editable-item"><span class="reorder-icon"></span>',
                                        '<span class="label"><%= feedUri %></span></li>',
                                  '<% } %>'].join(''))

        },


        init: function (element, meta) {
            this._super(element, meta);

            this.initElements(element);

            this.setData();
        },


        initElements: function (ctx) {
            this.$pageName = $(this.selectors.name, ctx);
            this.$descriptionHead = $(this.selectors.descriptionHead, ctx);
            this.$desciptionEdit = $(this.selectors.descriptionEdit, ctx);
            this.$subscribedFeedsList = $(this.selectors.subscribedFeedsList, ctx);
        },


        setData: function () {
            this.setName();
            this.setDescription();
            this.populateFeedUris();
        },

        setName: function () {
            this.$pageName.val(this.options.moduleName);
        },

        setDescription: function () {
            this.$descriptionHead.html(this.options.moduleDescription);
            this.$desciptionEdit.val(this.options.moduleDescription);
        },

        populateFeedUris: function () {
            var feedUris = this.options.feedUris;

            if (feedUris) {
                this.$subscribedFeedsList.append(
                    this.templates.feedUris(feedUris));
            
            }
        },

        _getFeedUris: function () {
            return _.map(
                        $('li > span.label', this.$subscribedFeedsList),
                            function (item) { return item.innerHTML; }
                        );
        },

        _initializeDelegates: function () {
            this._super();

            $.extend(this._delegates, {
                OnAddNewsFeed: $dj.delegate(this, this._handleAddNewsFeed),
                OnShowAddNewsFeed: $dj.delegate(this, this._handleShowAddNewsFeed),
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this._onError)
            });
        },

        _initializeEventHandlers: function () {
            this._super();

            $('.show-news-feed-entry', this._editArea).click(this._delegates.OnShowAddNewsFeed);
            $('.add-news-feed', this._editArea).click(this._delegates.OnAddNewsFeed);
        },

        _handleAddNewsFeed: function () {
            var urlEntryInput = $('.news-feed-url', this._editArea);

            this._addNewsFeed(urlEntryInput.val());

            urlEntryInput.val('');

            $('.add-news-feed-container', this._editArea).hide();
            $('.show-news-feed-entry', this._editArea).show();
        },

        _handleShowAddNewsFeed: function () {
            $('.add-news-feed-container', this._editArea).show();
            $('.show-news-feed-entry', this._editArea).hide();
        },

        buildProperties: function () {
            return {
                "canvasId": this.getCanvas()._canvasId,
                "moduleId": this.getModule()._moduleId,
                "description": this.$desciptionEdit.val(),
                "feedUriCollection": this._getFeedUris(),
                "title": this.$pageName.val()
            }
        },

        saveProperties: function (props) {
            $dj.proxy.invoke(
                {
                    url: this.options.webServiceUrl + ((this.options.payloadFormat == 0) ? "/json" : "/xml"),
                    data: props,
                    method: 'PUT',
                    controlData: this.getCanvas().get_ControlData(),
                    onSuccess: this._delegates.OnServiceCallSuccess,
                    onError: this._delegates.OnServiceCallError
                }
            );
        },

        _onSuccess: function () {
            $dj.debug('success');
        },

        _onError: function () {
            $dj.debug('error');
        }
    });

    $.plugin('dj_AlertsCanvasModuleEditor', DJ.UI.AlertsCanvasModuleEditor);

    $dj.debug('Registered DJ.UI.AlertsCanvasModuleEditor as dj_AlertsCanvasModuleEditor');


})(jQuery);
