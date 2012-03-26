(function ($) {

    DJ.UI.SearchResultsPage = DJ.UI.Component.extend({

        _searchBuilderForm: null,

        //_fieldsToResetOnNewSearch: ['server', 'contextId', 'start'],
        _fieldsToResetOnNewSearch: ['server', 'contextId'],

        options: {
            searchResultsUrl: '/search/results',
            searchBuilderFormId: 'searchBuilderForm',
            searchResultsFormId: 'searchResultsForm'
        },

        init: function (el, meta) {
            this._super(el, $.extend(meta, { name: "SearchResultsPage" }));
            this.currentLayout = "full";

            this._searchRequestJSON = $.parseJSON(this._searchResultsForm.find('input[name=request]').val());
        },

        getLayout: function (layout) {
            return this.options.layout;
        },

        viewSwitched: function (layout) {
            if (layout === "full") {
                this._searchResults.addClass('full-view-article');
            }
            else {
                this._searchResults.removeClass('full-view-article');
            }
            this.currentLayout = layout;
        },

        setLayout: function (layout) {

            if (!layout) {
                layout = "full";
            }

            this._searchResults.removeClass("full-view split-view").addClass(layout + '-view');

            if (this._dateHistogram) {
                try {
                    var h = this._dateHistogram.$element.find(".dj_datehistogram-chart").height();
                    var w = this._dateHistogram.$element.outerWidth();
                    this._dateHistogram.chart.setSize(w, h);
                } catch (e) { $dj.debug(e); }
            }

            //Show the headlines if in split view
            if (layout == 'split') {
                this._searchResults.removeClass('full-view-article');
            }
            else {
                this._searchNavigator.updateFiltersContainerHeight();
                //                if (this._articles.is(':visible')) {//if Article is visible
                //                    this._searchResults.addClass('full-view-article');
                //                }
                //                else {
                this._searchResults.removeClass('full-view-article');
                //}
            }

            this.currentLayout = layout;
        },

        executeNewFilteredSearch: function () {
            // Remove form fields that we don't want to send on a "new" search
            // e.g. start index, search server name, etc.

            //            var regex = new RegExp('(\\b' + this._fieldsToResetOnNewSearch.join('\\b)|(\\b') + '\\b)', 'ig');

            //            this._searchResultsForm
            //                .find('input')
            //                .filter(function () { return this.name.match(regex); })
            //            //.remove();
            //                .val('');

            //Reset the search request fields for a "new" search
            this.updateSearchRequestField("Start", 0);
            this.updateSearchRequestField("Server", 0);
            this.updateSearchRequestField("ContextId", null);

            this.refreshSearchResults();
        },

        refreshSearchResults: function () {
            if (this._submitting)
                return;

            this._submitting = true;

            var action = (this._searchResultsForm.attr('action') || location.href).split('?')[0];
            //Only for Chrome browser it is required to update the form action with some random value to avoid caching 
            if ($.browser.webkit && !!window.chrome) {
//                var action = this._searchResultsForm.attr('action') || location.href;
//                if (action.indexOf("?") != -1) {
//                    var randomParamIndex = action.lastIndexOf('rand=');
//                    if (randomParamIndex > 0) {
//                        action = action.substring(0, randomParamIndex - 1);
//                        action = action + (action.match(/\?$/) ? "rand=" : "&rand=") + (Math.random() * 1000);
//                    }
//                    else {
//                        action += "&rand=" + (Math.random() * 1000);
//                    }
//                }
//                else {
//                    action += "?rand=" + (Math.random() * 1000);
                //                }
                action += "?rand=" + (Math.random() * 1000);
            }
            this._searchResultsForm.attr('action', action);

            this._searchResultsForm.find('input[name=request]').val(JSON.stringify(this._searchRequestJSON));

            this._searchResultsForm.submit();
        },

        submitBuilderForm: function () {
            if (this._submittingBuilderForm)
                return;

            this._submittingBuilderForm = true;
            //this._searchBuilderForm.find('input[name=request]').val(JSON.stringify(this._searchRequestJSON));

            this._searchBuilderForm.submit();
        },

        _initializeElements: function () {
            this._searchBuilderForm = this.$element.find('#' + this.options.searchBuilderFormId);
            this._searchResultsForm = this.$element.find('#' + this.options.searchResultsFormId);
            this._searchResults = $('.search-results-container', this.$element);
            //this._startDate = this._getOrCreateHiddenField('startDate');
            //this._endDate = this._getOrCreateHiddenField('endDate');
            this._dateHistogram = this.$element.find(".dj_DateHistogram").findComponent(DJ.UI.DateHistogram);
            this._searchNavigator = this.$element.find(".dj_SearchNavigator").findComponent(DJ.UI.SearchNavigator);
            this._headlines = $('.headlines', this.$element);
            this._articles = $('.articles', this.$element);
            this._returnToHeadlines = $('.returnToHeadlines', this.$element);

            this.$compositeHeadline = $('.dj_CompositeHeadline', this.$element);
            this.$headlineList = $('.dj_HeadlineList', this.$element);
            this.$headlineListControl = $('.dj_HeadlineListControl', this.$element);
            this.$main = $('.main', this._searchResults);
            this.$articleOptions = $('.dj_HeadlineOptions', this._headlines);
            this.$postProcessing = $('.dj_PostProcessing.ui-component', this._headlines);

            var inputs = this._searchBuilderForm.find(':input').get();
            _.each(inputs, function (input) {
                var name = $(input).attr('name');
                this._copyFormValue(name);
            }, this);

            //After copying the search builder form input hidden fields remove the request hidden input 
            //as it is not required for new search when we submit the search builder form. It is very important to remove this field
            this._searchBuilderForm.find('#request').remove();
        },

        _initializeDelegates: function () {
            this._delegates = $.extend({}, this._delegates, {
                ExecuteNewFilteredSearch: $dj.delegate(this, this.executeNewFilteredSearch),
                OnClear: $dj.delegate(this, this._onClear),
                OnCloseSection: $dj.delegate(this, this._onCloseSection),
                OnDateHistogramClick: $dj.delegate(this, this._onDateHistogramClick),
                OnEntitiesRecognized: $dj.delegate(this, this._onEntitiesRecognized),
                OnLayoutChanged: $dj.delegate(this, this.setLayout),
                OnViewSwitched: $dj.delegate(this, this.viewSwitched),
                OnSearchNavigatorSelection: $dj.delegate(this, this._onSearchNavigatorSelection),
                OnSpellingCorrection: $dj.delegate(this, this._correctFreeTextSpelling),
                OnRelatedConceptsClick: $dj.delegate(this, this._onRelatedConceptsClick),
                RefreshSearchResults: $dj.delegate(this, this.refreshSearchResults),
                OnAdvancedSearchClick: $dj.delegate(this, this._OnAdvancedSearchClick),
                OnHeadlinesScroll: $dj.delegate(this, this._onHeadlinesScroll)
            });
        },

        _initializeEventHandlers: function () {
            this.$element.find('.dj_close-section').click(this._delegates.OnCloseSection);
            $dj.subscribe('clear.dj.SearchBuilder', this._delegates.OnClear);
            $dj.subscribe('entitiesRecognized.dj.SearchBuilder', this._delegates.OnEntitiesRecognized);
            $dj.subscribe('filtersChanged.dj.SearchFilters', this._delegates.ExecuteNewFilteredSearch);
            $dj.subscribe('searchChanged.dj.SearchBuilder', this._delegates.ExecuteNewFilteredSearch);
            $dj.subscribe('spellingCorrection.dj.SearchBuilder', this._delegates.OnSpellingCorrection);
            $dj.subscribe('sourceFilterChanged.dj.SearchNavigator', this._delegates.OnSearchNavigatorSelection);
            $dj.subscribe('toggleView.dj.SearchNavigator', this._delegates.OnLayoutChanged);
            $dj.subscribe('pagerClick.dj.SearchResults', this._delegates.RefreshSearchResults);
            $dj.subscribe('viewSwitch.dj.SearchResults', this._delegates.OnViewSwitched);
            $dj.subscribe('optionsChanged.dj.SearchResults', this._delegates.RefreshSearchResults);
            $dj.subscribe('termClick.dj.relatedConcepts', this._delegates.OnRelatedConceptsClick);
            $dj.subscribe('barClicked.dj.DateHistogram', this._delegates.OnDateHistogramClick);
            $dj.subscribe('loadHeadlines.dj.SearchResults', this._delegates.ExecuteNewFilteredSearch);
            $dj.subscribe('advancedSearch.dj.SearchBuilder', this._delegates.OnAdvancedSearchClick);
            $dj.subscribe('headlinesScroll.dj.CompositeHeadline', this._delegates.OnHeadlinesScroll);

        },

        _copyFormValue: function (name) {
            if (!(name || '').length) {
                return;
            }

            var value = $('[name="' + name + '"]', this._searchBuilderForm).val();
            this._getOrCreateHiddenField(name).val(value);
            $dj.debug('Copied form field ' + name + ' (value: ' + value + ') from Builder to Results');
        },

        _onRelatedConceptsClick: function (data) {

            this.freeText = this._searchBuilderForm.find('input[name=freeText]');
            this.freeText.val(this.freeText.val() + ' "' + data.text + '"');

            this._searchRequestJSON.FreeText = (this.freeText.val() + ' "' + data.text + '"');
            this.submitBuilderForm();
        },

        _correctFreeTextSpelling: function (correction) {
            //this._searchResultsForm.find('[name=freeText]').val(correction);
            this._searchRequestJSON.FreeText = correction;
            this.submitBuilderForm();
        },

        _getOrCreateHiddenField: function (name) {
            var field = this._searchResultsForm.find('input[name="' + name + '"]');

            if (!field.get(0)) {
                field = $('<input type="hidden" />')
                            .attr('name', name)
                            .appendTo(this._searchResultsForm);
            }

            return field;
        },

        _onClear: function () {
            this._searchResultsForm.remove();
        },

        _onCloseSection: function (e) {
            var $target = $(e.target);
            var $div = $target.closest('div,section').toggle();

            //Update the hidden input field
            if ($div.hasClass("dj_related-concepts")) {
                $("input[name=hideRelatedConcepts]").val((!$div.is(":visible")).toString())
            }
            else if ($div.hasClass("dj_DateNavigator")) {
                $("input[name=hideNewsVolume]").val((!$div.is(":visible")).toString())
            }
        },

        _onEntitiesRecognized: function (args) {
            //this._correctFreeTextSpelling(args.updatedFreeText);//Commented this because it was submitting the builder form
            this._searchResultsForm.find('[name=freeText]').val(args.updatedFreeText);
            $dj.publish('addFilters.dj.SearchFilters', args.filters);
        },

        _onDateHistogramClick: function (params) {
            if (!params || !params.data) {
                return;
            }

            var dateRange = [params.data.isoStartDate, '-', params.data.isoEndDate].join('');
            var name = params.data.startDateText;
            if (params.data.endDateText && (params.data.endDateText != params.data.startDateText)) {
                name += " - " + params.data.endDateText;
            }
            var filter = { category: 'DateRange', code: dateRange, name: name };

            $dj.publish('addFilters.dj.SearchFilters', [filter]);

            this.refreshSearchResults();
        },

        _onLayoutChanged: function (layout) {
            this.setLayout(layout);
        },

        _onSearchNavigatorSelection: function (navigator) {
            if (navigator && navigator.filter) {
                $dj.publish('addFilters.dj.SearchFilters', [navigator.filter]);
            }

            this.executeNewFilteredSearch();
        },

        _OnAdvancedSearchClick: function () {
            location.href = '<%= AppSetting("AdvancedSearchBuilderPath", "../Searchbuilder/Index") %>';
        },

        _onHeadlinesScroll: function (data) {
            var $headlines = this._headlines,
                scrollTop = $headlines.scrollTop(),
                scrollTolerance = $headlines.data('scrollTolerance'),
                headlineTop = this.$compositeHeadline.position().top;

            if (this.$main.hasClass('fixed-controls') && scrollTop < scrollTolerance) {
                this.$main.removeClass('fixed-controls');
                $headlines.data('scrollTolerance', 0);

                this.$headlineListControl.before(this.$articleOptions.removeClass('fixed'));
                this.$headlineListControl.before(this.$postProcessing.removeClass('fixed'));
                //                this.$articleOptions.removeClass('fixed').insertBefore('.dj_HeadlineList', $headlines);
                //                this.$postProcessing.removeClass('fixed').insertBefore('.dj_HeadlineList', $headlines);

            } else if (!this.$main.hasClass('fixed-controls') && headlineTop < 0) { //less than zero because of element border
                $headlines.data('scrollTolerance', scrollTop + headlineTop);
                this.$main.addClass('fixed-controls');

                this.$postProcessing.addClass('fixed').prependTo(this.$main);
                this.$articleOptions.addClass('fixed').prependTo(this.$main);
            }

            //attempts to remedy IMS issue 41325
            var $menu = $('.menu');
            var $currentMenu;
            $menu.each(function () { // there are more than one menu; get the menu with data(targetElem);
                $this = $(this);
                if ($this.data("targetElem")) {
                    $currentMenu = $this;
                }
            });

            if ($currentMenu) {
                $menu = $currentMenu;
                var $menuTarget = $menu.data('targetElem');
                if ($menuTarget && $menuTarget.length == undefined) {
                    $menuTarget = $($menuTarget);
                }

                if ($menuTarget && $menuTarget.length > 0) {
                    //when the menu's daddy is within the scrolling area
                    if ($menuTarget.closest('.column.headlines').length > 0) {

                        // now reposition the menu
                        // positioning code copied from dashboard/js/jquery.menu.js
                        // seeking more elegant solution: call the positionElement() method 
                        $menu.css('left', $menuTarget.offset().left + 'px');
                        $menu.css('top', $menuTarget.offset().top + $menuTarget.height() + 'px');
                        if (($menu.offset().left + $menu.width()) > $(window).width()) {
                            $menu.css('left', (($menuTarget.offset().left + $menuTarget.width()) - $menu.width()) + 'px');
                        }

                        var targetOffset = $menuTarget.offset();
                        var targetBottomLimit = targetOffset.top + $menuTarget.innerHeight();

                        // now that it has been repositioned, check that my daddy is still visible, if so then I'll go away
                        $headlines = $('.column.headlines');
                        $main = $headlines.parents('.main');
                        $post_processing = $('.dj_PostProcessing.ui-component', $main);
                        var headlinesBottomLimit = $headlines.offset().top + $headlines.innerHeight();
                        if ($main.hasClass('fixed-controls')) {
                            var controlsOffset = $post_processing.offset();
                            if (targetOffset.top <= controlsOffset.top) {
                                $menu.hide();
                            }
                        }

                        if (targetBottomLimit >= headlinesBottomLimit) {
                            $menu.hide();
                        }
                    }
                }
            }

        },

        getSearchRequest: function () {
            return this._searchRequestJSON;
        },

        updateSearchRequestField: function (field, value) {
            this._searchRequestJSON[field] = value;
        },

        EOF: null
    });

    $.plugin('dj_SearchResultsPage', DJ.UI.SearchResultsPage);

})(jQuery);
