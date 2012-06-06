/*!
* Search Results
*/

    DJ.UI.HeadlineSearchResults = DJ.UI.Component.extend({

        name: 'SearchResults',

        selectors: {
            //entityLinks: '.dj_article_entity',
            entityModal: '.dj_entity-info',
            articleOptions: '.dj_ArticleOptionsContainer .controls .drop-down-button .selected-option',
            articleOptionsModal: '.dj_ArticleOptionsContainer .controls .drop-down-button .selected-option .options',
            optionItem: '.dj_options .item',
            optionsMenu: '.dj_options',
            optionsOk: '.dj_optionsOk',
            articleDisplayArea: '.dj_ArticleContainer',
            returnToHeadlines: '.returnToHeadlines',
            headlines: '.headlines'
        },

        events: {
            //entityClick: 'entityClick.dj.ArticleComponent',
            headlinesScroll: 'headlinesScroll.dj.CompositeHeadline', 
            headlineEntityClick: 'entityClick.dj.CompositeHeadline'
        },

        defaults: {
            ArticleUrl: '/search/articles',
            HeadlinesUrl: '/search/results',
            Articles: [],
            ShowDuplicates: null,
            View: null,
            Sort: null,
            chunkSize: 7
        },

        _toggleOptionsMenu: function (show) {
            if (show) {
                this.$articleOptionsModal.addClass('active');
                this.$articleOptions.addClass('active');
            }
            else {
                this.$articleOptionsModal.removeClass('active');
                this.$articleOptions.removeClass('active');
            }
        },

        showLoading: function (container) {
            if (this.chunkedRequests) {
                container.append('<div class="dj_articleLoading"><%= Token("loadingArticle") %></div>');
            }
            else {
                container.html('<div class="dj_articleLoading"><%= Token("loadingArticle") %></div>');
            }
        },

        hideLoading: function (container) {
            container.find('.dj_articleLoading').remove();
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "SearchResults" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // initialize article scroll
            /*$('.articles', this.$element).localScroll({
                target: $('.articles', this.$element),
                lazy: true,
                stop: true,
                hash: false
            });*/
            
        },

        _initializeDelegates: function () {
            this._super();
            this._delegates = $.extend(this._delegates, {
                OnHeadlineClick: $dj.delegate(this, this._onHeadlineClick),
                OnPagerClick: $dj.delegate(this, this._onPagerClick),
                OnRetrieveArticleSuccess: $dj.delegate(this, this._onRetrieveArticleSuccess),
                OnRetrieveArticleError: $dj.delegate(this, this._onRetrieveArticleError),
                OnRetrieveHeadlinesSuccess: $dj.delegate(this, this._onRetrieveHeadlinesSuccess),
                
                OnHeadlineOptionsChanged: $dj.delegate(this, this._onHeadlineOptionsChanged),
                OnPostProcessingClick: $dj.delegate(this, this._postProcessingClick),
                OnLoadArticles: $dj.delegate(this, function () { this.chunkedRequests = null; this._onLoadArticles(); }),
                //OnEntityClick: $dj.delegate(this, this._onEntityClick),
                OnOptionsOkClick: $dj.delegate(this, this._onOptionsOkClick),
                OnHeadlineEntityClick: $dj.delegate(this, this._onHeadlineEntityClick)

            });
        },

        _initializeElements: function (ctx) {
            this.$searchResults = this.$element.parents('.search-results-container');
            this.articleContainer = $(this.selectors.articleDisplayArea, this.$element);
            this.headlinesContainer = $('.dj_HeadlinesContainer', this.$element);
            this.$headlines = $('.headlines', this.$element);
            this.pager = $('.dj_Pager', this.$element);
            //this.$entityLinks = ctx.find(this.selectors.entityLinks);
            this.$entityModal = ctx.find(this.selectors.entityModal);
            
            this.$articleOptionsModal = ctx.find(this.selectors.articleOptionsModal);
            this.$articleOptions = ctx.find(this.selectors.articleOptions);
            this.$returnToHeadlines = ctx.find(this.selectors.returnToHeadlines);
            this.$articles = $('.articles', this.$element);

            // hidden input for search manipulations            
            //this.startFormInput = $('input[name=start]', this.$element);
            //this.pageSizeFormInput = $('input[name=pageSize]', this.$element);
            this.pageNextFormInput = $('input[name=nextIndex]', this.$element);
            this.pagePreviousFormInput = $('input[name=previousIndex]', this.$element);
            this.$articleDisplayOption = $('input[name=articleDisplayOption]', this.$element);
            //this.$showDuplicates = $('input[name=showDuplicates]', this.$element);
            //this.$headlineSort = $('input[name=Sort]', this.$element);
            this.$highlightString = $('input[name=HighlightString]', this.$element);
            this.$usage = $('input[name=usageAggregator]'); 
            this.$pictureSize = $('input[name=pictureSize]', this.$element);   

            // layout
            this.$layout = $('#layout');
        },

        _initializeEventHandlers: function () {
            var self = this;

            this.pager.click(this._delegates.OnPagerClick);
            
            // events from pub/sub to subscribe to
            $dj.subscribe('pagerClick.dj.CompositeHeadline', this._delegates.OnPagerClick);
            $dj.subscribe('headlineClick.dj.CompositeHeadline', this._delegates.OnHeadlineClick);
            $dj.subscribe('postProcessingClick.dj.CompositeHeadline', this._delegates.OnPostProcessingClick);
            $dj.subscribe('loadArticles.dj.SearchResults', this._delegates.OnLoadArticles);
            //$dj.subscribe('entityClick.dj.ArticleComponent', this._delegates.OnEntityClick);
            $dj.subscribe('optionChange.dj.CompositeHeadline', this._delegates.OnHeadlineOptionsChanged);
            $dj.subscribe('entityClick.dj.CompositeHeadline', this._delegates.OnHeadlineEntityClick);

            // events to 
//            this.$element.delegate(this.selectors.entityLinks, 'click', function () {
//                $dj.debug("entity clicked");

//                var data = $(this).data('reference');

//                $dj.debug("entity clicked", data);
//                self.publish(self.events.entityClick, data);
//                return false;
//            });

            this.$element.delegate(this.selectors.articleOptions, 'click', function (e) {
                self._toggleOptionsMenu(true);
                e.stopPropagation();
            });

            this.$element.delegate(this.selectors.optionItem, 'click', function (e) {
                e.stopPropagation();
            });

            this.$element.delegate(this.selectors.optionsMenu, {
                'mouseleave': function () {
                    $('body').unbind('mouseup.dj_CompositeArticleOptionsDropDown')
                             .bind('mouseup.dj_CompositeArticleOptionsDropDown', function (e) {
                                 self._toggleOptionsMenu(false);
                             });
                },
                'mouseenter': function () {
                    $('body').unbind('mouseup.dj_CompositeArticleOptionsDropDown');
                }
            });

            this.$element.delegate(this.selectors.optionsOk, 'click', this._delegates.OnOptionsOkClick);

            this.$returnToHeadlines.click($dj.delegate(this, function(){
                this.$searchResults.removeClass('full-view-article');
            }));

            this.$element.find(this.selectors.headlines).bind('scroll', function(e){
                self.publish(self.events.headlinesScroll, { event: e });
            });
        },

        _updateArticlePager: function() {
            $dj.debug("_updateArticlePager", this.articleContainer);
            var self = this;
            var linkers = $('a.article-pager-hiddenLink', this.$element);
            var pagers = $('div.dj_articlePager', this.$element);
            
            if (pagers && pagers.length && pagers.length > 1) {                
                var len = pagers.length-1;
                $.each(pagers, function(i, val) {

                    var title = $("span.article-pager-index", val);
                    var prev = $("a.article-pager-prev", val);
                    var pipe = $("span.article-pager-pipe", val);
                    var next = $("a.article-pager-next", val);
                    
                    title.text(i+1);

                    if (i==0) {
                        var nId = linkers[i+1].id;
                        prev.hide();
                        pipe.hide();
                        next[0].href = "#" + nId;
                    }
                    else if (i==len) {
                        var pId = linkers[i-1].id;
                        pipe.hide();
                        next.hide();
                        prev[0].href = "#" + pId;
                    }
                    else {
                        var nId = linkers[i+1].id;
                        var pId = linkers[i-1].id;
                        next.show();
                        pipe.show();
                        prev.show();
                        next[0].href = "#" + nId;
                        prev[0].href = "#" + pId;
                    }

                });

                pagers.show();
                pagers.css('visibility','visible')
            }
            else {
                pagers.hide(); 
            }
        },
        
        _onOptionsOkClick: function () {

            // hide menu
            this._toggleOptionsMenu(false);
            var v = $('input[type=radio][name=articleDisplayOptions]:checked').val();
            if (this.$articleDisplayOption.val() != v) {

                // set the value
                this.$articleDisplayOption.val(v);
                // some cleanup
                $('body').unbind('mouseup.dj_CompositeArticleOptionsDropDown');
                
                this.chunkedRequests = null;
                this.options.Articles =  $.extend([], this.options.CurArticles);               
                
                               
                // Reload Articles via ajax
                this._onLoadArticles();
            }
            return false;
        },


//        _onEntityClick: function (data) {
//            $dj.debug("entity event subscried");
//            this.$entityModal.overlay({ closeOnEsc: true });
//        },
         _onHeadlineEntityClick: function (data) {
            $dj.publish('entityClick.dj.SearchResults', { data: data });
        },
        _onHeadlineClick: function (data) {
            $dj.debug('Headline clicked: ', data);
            var hld = data.headline;
            if (hld) {
                switch(hld.contentCategoryDescriptor) {
                    case "publication":
                    case "picture":
                        this.options.CurArticles = [{guid:hld.guid, type:hld.contentCategoryDescriptor, subType: hld.contentSubCategoryDescriptor}];
                        this.options.Articles = [{guid:hld.guid, type:hld.contentCategoryDescriptor, subType: hld.contentSubCategoryDescriptor}];
                        $dj.publish('loadArticles.dj.SearchResults');
                        break;
                }               
            }
        },

        _onPagerClick: function (e) {
            if(!this.searchResultsPageC){//SearchResultsComponent object
                this.searchResultsPageC = $('body').findComponent(DJ.UI.SearchResultsPage);
            }

            //var count = parseInt(this.pageSizeFormInput.val(), 10);
            var count = parseInt(this.searchResultsPageC.PageSize, 10);

            var page = e.page || 0;

//            if (page === 'next')
//                startIndex  = this.pageNextFormInput.val();
//            else if (page === 'prev')
//                startIndex  = this.pagePreviousFormInput.val();
//            else
//                startIndex = parseInt(page, 10) * count;

            if (page === 'next')
                startIndex  = this.pageNextFormInput.val();
            else if (page === 'prev')
                startIndex  = this.pagePreviousFormInput.val();
            else
                startIndex = parseInt(page, 10) * count;
            

            //this.startFormInput.val(Math.max(0, startIndex));

            
            this.searchResultsPageC.updateSearchRequestField("Start", Math.max(0, startIndex));

            $dj.publish('pagerClick.dj.SearchResults', { startIndex: startIndex });
        },

        _onRetrieveArticleSuccess: function (article) {
            if ($.trim(article).substring(2, 7) == "ERROR") {
				var eObj = $.parseJSON(article).ERROR;
                if(this.chunkedRequests){
                    this.articleContainer.html(this._getArticleErrorMessage(eObj)).scrollTop(0).scrollLeft(0);
                }
                else{
                    this.articleContainer.append(this._getArticleErrorMessage(eObj)).scrollTop(0).scrollLeft(0);
                }
			}
            else{
                if (this.chunkedRequests) {
                    this.hideLoading(this.articleContainer);
                    this.articleContainer.append(article).scrollTop(0).scrollLeft(0);
                    if (this.options.Articles.length) {
                        this._onLoadArticles();
                        return;
                    }
                }
                else {
                    this.articleContainer.html(article).scrollTop(0).scrollLeft(0);
                }
            }
            this._updateArticlePager();
        },

        _onRetrieveArticleError: function (response) {
            var errMsg = "<%= Token("failedToLoadArticle") %>";
            html = '<div class="dj_articleLoadError">' + errMsg + '</div>';
            if (response && response.Error) {
                html = this._getArticleErrorMessage(response.Error);
            }

            if (this.chunkedRequests) {
                this.hideLoading(this.articleContainer);
                this.articleContainer.append(html).scrollTop(0).scrollLeft(0);
            }
            else {
                this.articleContainer.html(html).scrollTop(0).scrollLeft(0);
            }
        },

        _getArticleErrorMessage: function(errorObj){
            var msg = '<div class="dj_articleLoading"><%= Token("error") %> ';
	        if (errorObj) {
		        msg += errorObj.Number + ': ' + errorObj.Description;
	        }
	        return msg + '</div>';
        },

        _onRetrieveHeadlinesSuccess: function (headlines) {
            this.headlinesContainer.html(headlines);
        },

        _onHeadlineOptionsChanged: function (data) {
            $dj.debug('setting options in serch results component', data);

//            if (this.$headlineSort.val() != data.sort || this.$showDuplicates.val() != data.showDuplicates) {
//                this.$headlineSort.val(data.sort);
//                this.$showDuplicates.val(data.showDuplicates);
//                $dj.publish('loadHeadlines.dj.SearchResults');
//            }

            if(!this.searchResultsPageC){//SearchResultsComponent object
                this.searchResultsPageC = $('body').findComponent(DJ.UI.SearchResultsPage);
            }
            var searchRequest = this.searchResultsPageC.getSearchRequest();

            if (searchRequest.Sort != data.sort || searchRequest.ShowDuplicates != data.showDuplicates) {
                this.searchResultsPageC.updateSearchRequestField("Sort", data.sort);
                this.searchResultsPageC.updateSearchRequestField("ShowDuplicates", data.showDuplicates);
                $dj.publish('loadHeadlines.dj.SearchResults');
            }
        },

        _postProcessingClick: function (data) {
            $dj.debug('Postprocessing clicked: ' + JSON.stringify(data));

            if (data) {
                switch (data.command) {
                    case 'read':
                        var articles = [];
                        for (var i = 0; i < data.headlines.length; i++) {
                            articles.push({guid:data.headlines[i].guid, type:data.headlines[i].contentCategoryDescriptor});
                        }
                        this.options.Articles = $.extend([], articles);
                        this.options.CurArticles = $.extend([], articles);
                        this._onLoadArticles();
                        $dj.publish('loadArticles.dj.SearchResults');
                        break;
                }
            }
        },

        _onLoadArticles: function () {

            if (this.options.Articles.length > 0
                && this.options.Articles[0].subType != "html"
                && this.options.Articles[0].subType != "pdf"
                && this.options.Articles[0].subType != "analyst") {

                this.showLoading(this.articleContainer);

                var ids;
                if (!this.chunkedRequests) {
                    this.chunkedRequests = true;
                    ids = this.options.Articles.splice(0, 1);
                }
                else {
                    ids = this.options.Articles.splice(0, this.options.chunkSize);
                }

                var idArr = _.pluck(ids, "guid");
                var idType = _.pluck(ids, "type");
                var data = { ids: idArr, type:idType, highlight: this.$highlightString.val(), usage: this.$usage.val(), options: this.$articleDisplayOption.val(), pictureSize: this.$pictureSize.val() };

                $.ajax({
                    url: this.options.ArticleUrl,
                    type: 'POST',
                    data: data,
                    traditional: true,
                    success: this._delegates.OnRetrieveArticleSuccess,
                    error: this._delegates.OnRetrieveArticleError
                });

                $dj.publish('viewSwitch.dj.SearchResults',  this.$layout.val());
            }
        },


        EOF: true

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_HeadlineSearchResults', DJ.UI.HeadlineSearchResults);

    $dj.debug('Registered DJ.UI.HeadlineSearchResults (extends DJ.UI.Component)');
