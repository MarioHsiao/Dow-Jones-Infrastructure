
    DJ.UI.SimpleSearchBuilder = DJ.UI.QueryBuilder.extend({

        init: function (element, meta) {
            var $meta = $.extend({ name: "SimpleSearchBuilder" }, meta);
            this._super(element, $meta);
        },

        _initializeElements: function () {
            this._advancedSearchButton = this.$element.find('.advanced-search');
            this._keywordsInput = this.$element.find('input[name=freeText]');
            this._spellChecker = this.$element.find('.dj_keyword-spellcheck');
            this._sourceSelect = this.$element.find('select#source');
            this._dateRangeSelect = this.$element.find('select#dateRange');
            this._clearButton = this.$element.find('.search-clear');
            this._saveAsBtn = this.$element.find('.save-options > span');
            this._saveAsMenu = this.$element.find('.saveAsMenu');
            this._suggestions = this.$element.find('.suggestions');
        },

        _initializeEventHandlers: function () {
            var me = this;
            this._advancedSearchButton.click(function () { $dj.publish('advancedSearch.dj.SearchBuilder'); });
            this._spellChecker.find('.replacement-text').click(this._delegates.OnSpellingCorrection);

            $dj.subscribe('entitiesRecognized.dj.DidYouMean', this._delegates.OnEntitiesRecognized);

            $dj.subscribe('entitiesToggle.dj.DidYouMean', this._delegates.OnEntitiesToggle);

            // initialize the searchbox
            this._keywordsInput.attr('id', this.$element.attr('id') + "_freeText");

            if (np && np.web && np.web.widgets && np.web.widgets.autocomplete) {
                np.web.widgets.autocomplete({
                    url: this.options.suggestServiceUrl,
                    controlId: this.$element.attr('id') + "_freeText",
                    controlClassName: "djAutoComplete",
                    autocompletionType: "KeyWord",
                    maxResults: "10",
                    resultsClass: "dj_emg_autosuggest_results",
                    resultsOddClass: "dj_emg_autosuggest_odd",
                    resultsEvenClass: "dj_emg_autosuggest_even",
                    resultsOverClass: "dj_emg_autosuggest_over",
                    selectFirst: false,
                    fillInputOnKeyUpDown: true,
                    useSessionId: DJ.config.credentials.sessionId,
                    onItemSelect: function (item) {
                        $dj.publish('autoCompleteItemSelect.dj.SearchBuilder', { keyword: item.word });
                    }
                });
            }

            // initialize the source-range select boxes
            this._sourceSelect.selectbox();
            this._dateRangeSelect.selectbox();

            // initialize the following events.
            this._clearButton.click(this._delegates.OnClear);

            // save as menu
            this._saveAsBtn.click(function (e) {
                e.stopPropagation(); //Need to stop the event propagation
                me._onSaveAsClick(this);
            });

            // save as menu item select
            this._saveAsMenu.delegate('.label', 'click', function () {
                $dj.publish('saveAs.dj.SearchBuilder', { saveAs: $(this).data('saveas') });
            }).appendTo(document.body);
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                OnClear: $dj.delegate(this, this._onClear),
                OnEntitiesRecognized: $dj.delegate(this, this._onEntitiesRecognized),
                OnSpellingCorrection: $dj.delegate(this, this._onSpellingCorrection),
                OnEntitiesToggle: $dj.delegate(this, this._onEntitiesToggle)
            });
        },

        _onClear: function (e) {
            this._suggestions.remove();
            $dj.publish('clear.dj.SearchBuilder');
            this._keywordsInput.val('');
            return false;
        },

        _onEntitiesRecognized: function (entityData) {

            var entities = entityData.entities;
            var context = entityData.context;
            var selectedCodes = _.pluck(entities, 'code');
            var searchTerms = [];

            var filters = [];

            _.each(context, function (item) {
                if (!(item.Code && _.indexOf(selectedCodes, item.Code) != -1)) {
                    searchTerms.push(item.SearchTerm);
                }
            });

            _.each(entities, function (entity) {
                filters.push({
                    category: entity.category,
                    code: entity.code,
                    name: entity.name
                });

            }, this);

            this._keywordsInput.val(searchTerms.join(' '));

            var args = {
                filters: filters,
                updatedFreeText: this._keywordsInput.val()
            };

            $dj.publish('entitiesRecognized.dj.SearchBuilder', args);
        },

        _onEntitiesToggle: function (args) {
            //Update hideDYM hidden field
            $("input[name=hideDYM]").val(args.visible.toString());
        },

        _onSpellingCorrection: function (e) {
            var replacementText = this._spellChecker.find('.replacement-text').text();
            this._keywordsInput.val(replacementText);
            this._spellChecker.hide();

            $dj.publish('spellingCorrection.dj.SearchBuilder', replacementText);
        },

        _onSaveAsClick: function (elem) {
            var $elem = $(elem), elemOffset = $elem.offset();
            this._saveAsBtn.addClass('active');
            this._saveAsMenu.css({ top: elemOffset.top + $elem.outerHeight(), left: elemOffset.left, position: "absolute" }).show();
            $(document).unbind('mousedown.SaveAs').bind('mousedown.SaveAs').click($dj.delegate(this, function () {
                this._saveAsBtn.removeClass('active');
                this._saveAsMenu.hide();
                $(document).unbind('mousedown.SaveAs');
            }));
        },

        EOF: null

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_SimpleSearchBuilder', DJ.UI.SimpleSearchBuilder);

