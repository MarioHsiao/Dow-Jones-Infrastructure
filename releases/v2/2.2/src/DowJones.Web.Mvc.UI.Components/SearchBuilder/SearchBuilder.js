/*
*  Search Builder Control
*/

(function ($) {

    DJ.UI.SearchBuilder = DJ.UI.Component.extend({

        selectors: {
            sbTopContainer: 'div.dj_advanced-search-wrap',
            searchTextBox: 'textarea.text-field',
            searchCriteria: 'div.dj_select-box-alt',
            modifySearch: 'div.dj_modify-search',
            selectBoxes: 'select.dj_selectbox',
            searchInDD: 'select.searchIn',
            dateDD: 'select.date',
            sortByDD: 'select.sortBy',
            filtersWap: 'div.dj_filters-wrap',
            filtersContainer: 'div.dj_search-builder_filters',
            filtersList: 'ul.dj_search-builder_filters-list',
            filterClose: 'span.remove',
            filterPill: 'li.dj_pill',
            addPill: 'li.add',
            pillListWrap: '.dj_pill-list-wrap',
            notFilter: 'ul.red',
            lookUpsContainer: 'div.lookUpsContainer',
            category: 'div.category',
            dummyLookUp: 'div.dummyLookUp',
            searchCategoriesLookUp: 'div.dj_SearchCategoriesLookUp',
            modalContent: 'div.modal-content',
            modalClose: 'p.dj_modal-close',
            //resetSearchText: 'span',
            expanded: 'li.expanded',
            saveList: 'span.saveList',
            filtersToolbar: '.dj_search-builder_filters-category_toolbar',
            toggleOperatorSwitch: '.dj_toggle-switch',
            datePicker: 'input.datepicker',
            dateWrap: 'div.date-wrap',
            menuItem: 'div.menuitem',
            menu: 'div.menu',
            searchBtn: 'span.dj_btn-blue',
            excludeBtn: 'span.exclude',
            footer: 'div.footer',
            doneBtn: 'span.dj_btn-blue',
            cancelBtn: 'span.dj_btn-drk-gray',
            duplicate: 'input.duplicates',
            grayBtn: 'span.dj_btn-drk-grey',
            newsFilter: '.news-filter',
            filterList: '.filter-list',
            filterGroup: '.filter-group',
            pillList: '.dj_pill-list'
        },

        events: {
            onSearchTextBoxEnter: 'onSearchTextBoxEnter.dj.SearchBuilder'
        },

        filterType: {
            Company: 0,
            Author: 1,
            Executive: 2,
            Subject: 3,
            Industry: 4,
            Region: 5,
            Source: 6,
            Language: 7
        },

        filterDetails: [
            {
                name: 'Company',
                text: "<%= Token('companyLabel') %>",
                pluralText: "<%= Token('companies') %>"
            },

            {
                name: 'Author',
                text: "<%= Token('author') %>",
                pluralText: "<%= Token('authors') %>"
            },

            {
                name: 'Executive',
                text: "<%= Token('executive') %>",
                pluralText: "<%= Token('executives') %>"
            },

            {
                name: 'Subject',
                text: "<%= Token('subject') %>",
                pluralText: "<%= Token('subjects') %>"
            },

            {
                name: 'Industry',
                text: "<%= Token('industry') %>",
                pluralText: "<%= Token('industries') %>"
            },

            {
                name: 'Region',
                text: "<%= Token('regionLabel') %>",
                pluralText: "<%= Token('regions') %>"
            },

            {
                name: 'Source',
                text: "<%= Token('sourcesLabel') %>",
                pluralText: "<%= Token('sources') %>"
            },

            {
                name: 'Language',
                text: "<%= Token('language') %>",
                pluralText: "<%= Token('language') %>"
            },

            {
                name: 'Keyword',
                text: "<%= Token('keywords') %>"
            },

            {
                name: 'DateRange',
                text: "<%= Token('date') %>"
            }
        ],

        searchOperator: {
            And: "0",
            Not: "1",
            Or: "2"
        },

        //Please do not change the order of filters, it is required to be in this order as per biz
        sortedChannelFilters: ['source', 'author', 'executive', 'company', 'subject', 'industry', 'region', 'language'],

        channelFilters: ['company', 'author', 'executive', 'subject', 'industry', 'region', 'source', 'language'],

        init: function (element, meta) {
            var $meta = $.extend({ name: "SearchBuilder" }, meta);

            this._setDefaultProperties();

            // Call the base constructor
            this._super(element, $meta);

            this.setData(this.data, true);
        },

        _setDefaultProperties: function () {
            this._categoryOptions = this.templates.categoryOptions();
            this.$filterOptions = $(this.templates.filterOptions());

            this._addPill = '<li class="dj_pill add"><span>&nbsp;</span></li>';
            this._notPill = '<li class="dj_pill not"><span><%= Token("notLabel") %></span></li>';
            this._sbSearchBoxWMText = "<%= Token('searchBuilderAutoCompleteText') %>";

            this._dummyLookUpC = $(this.selectors.dummyLookUp, this.$element).children(this.selectors.searchCategoriesLookUp).findComponent(DJ.UI.SearchCategoriesLookUp);
        },

        _initializeDelegates: function () {
            this._super();
        },

        _initializeControls: function () {
            var $filtersWrap = this.$element.children(this.selectors.filtersWap),
                $sbTopContainer = this.$element.children(this.selectors.sbTopContainer);

            this.$searchTexBox = $sbTopContainer.children(":first").children(this.selectors.searchTextBox);

            var $searchOptions = $sbTopContainer.children(this.selectors.searchCriteria).children().eq(0).children(this.selectors.modifySearch),
                $displayOptions = $searchOptions.parent().next().children(this.selectors.modifySearch);

            this.$dateWrap = $searchOptions.children(this.selectors.dateWrap);
            this.$datePickers = this.$dateWrap.children(this.selectors.datePicker);
            this.$startDate = this.$datePickers.eq(0);
            this.$endDate = this.$datePickers.eq(1);
            this.$searchInDD = $searchOptions.children(this.selectors.searchInDD);
            this.$dateDD = $searchOptions.children(this.selectors.dateDD);
            this.$excludeBtn = $searchOptions.children(this.selectors.excludeBtn);
            if (this.options.showDisplayOptions) {
                this.$sortByDD = $displayOptions.children(this.selectors.sortByDD);
                this.$duplicate = $displayOptions.children(this.selectors.duplicate);
            }

            this.$filtersList = $filtersWrap.children(this.selectors.filtersContainer).children(this.selectors.filtersList);
            this.$resetBtn = $filtersWrap.children(this.selectors.filtersContainer).prev().children(this.selectors.grayBtn);
            this.$newsFilter = $filtersWrap.children(this.selectors.newsFilter);

            //Append the category options and not pill
            var $this, type, me = this;
            this.$filtersList.children().each(function () {
                $this = $(this); type = $this.data("type");
                if (type != me.filterDetails[me.filterType.Language].name) {
                    $this.append(me._categoryOptions);
                    $this.children(me.selectors.pillListWrap).children(me.selectors.notFilter).append(me._notPill).hide();
                    if (type != me.filterDetails[me.filterType.Source].name) {
                        $this.find(me.selectors.saveList).remove();
                        if (type == me.filterDetails[me.filterType.Author].name) {//If Author then remove And/Any toggle switch
                            $this.find(me.selectors.toggleOperatorSwitch).remove();
                        }
                    }
                    else {//If source then remove And/Any toggle swtich
                        $this.find(me.selectors.toggleOperatorSwitch).remove();
                    }
                }
            });
        },

        _initializeEventHandlers: function () {
            var me = this, elementId = this.$element.attr('id');
            this._initializeControls();

            this.$searchTexBox.bind('keydown keyup', function (e) {
                if ((e.which == 13) && !me._enterPressed) {//Enter pressed
                    me._enterPressed = true;
                    //Publish onSearchTextBoxEnter event
                    me.publish(me.events.onSearchTextBoxEnter, me);
                    e.stopPropagation();
                    return false;
                }
            }).focus(function (e) {
                me._enterPressed = false;
                e.stopPropagation();
            }).blur(function (e) {
                if (me._enterPressed) {
                    e.stopPropagation();
                    return false;
                }
            }).waterMark({
                waterMarkClass: "watermark",
                waterMarkText: this._sbSearchBoxWMText
            }).autoGrow();

            //            this.$searchTexBox.next(this.selectors.resetSearchText).click($dj.delegate(this, function () {
            //                this.$searchTexBox.val('').focus();
            //            }));

            // Select Box
            $().add(this.$searchInDD).add(this.$dateDD).selectbox().change();
            
            //Custom date range
            this.$dateDD.change(function () {
                me.$dateWrap.toggle(this.value == me.$dateDD.find("option:last").val());
            });

            //Datepicker
            this.$datePickers.waterMark({
                waterMarkClass: "watermark",
                waterMarkText: this.options.dateFormatDisplay
            }).datepicker({
                monthNames: ["<%= Token('january') %>", "<%= Token('february') %>", "<%= Token('march') %>", "<%= Token('april') %>", "<%= Token('may') %>", "<%= Token('june') %>", "<%= Token('july') %>", "<%= Token('august') %>", "<%= Token('september') %>", "<%= Token('october') %>", "<%= Token('november') %>", "<%= Token('december') %>"],
                onSelect: function (date, datePickerInstance) {
                    me._setCustomDate(date, this);
                },
                dateFormat: this._getDateFormat(true)
            }).blur();

            //Exclusions
            this.$excludeBtn.click($dj.delegate(this, this._showExclusions));

            //Sort By and Duplicates
            if (this.options.showDisplayOptions) {
                this.$sortByDD.selectbox().change();
                this.$duplicate.attr('id', elementId + '_duplicate').next().attr('for', elementId + '_duplicate');
            }

            //Filters Reset button
            this.$resetBtn.click($dj.delegate(this, this._resetFilters));

            this.$filtersList.delegate(this.selectors.toggleOperatorSwitch, 'click', function () {//Toggle Switch
                var $this = $(this);
                me._setQueryOperator($this, !$this.children("span.switch").hasClass("on"));
            }).delegate(this.selectors.addPill, "click", function (e) {//Add Pill
                var $category = $(this).parent().closest("li");
                if (!$category.hasClass("disabled")) {
                    me._lookUpSearchCategory($category.data("type"), $category.children("h4").html());
                }
                me._stopPropagation(e, true);
            }).delegate(this.selectors.filterClose, "click", function (e) {//Filter Close
                me._onFilterClose(this);
                me._stopPropagation(e, true);
            }).delegate(this.selectors.filterPill, "click", function (e) {//Filter Click
                var notFilterEnabled = !$(this).closest("ul").closest("li").data("notdisabled");
                if (notFilterEnabled) {
                    me._onFilterClick(this);
                    me._stopPropagation(e);
                }
            }).delegate(this.selectors.menuItem, 'click', function (e) {//Filter Options Click
                var $this = $(this), action = $this.data("action");
                me.$filterOptions.hide();
                if (action == "remove") {//Remove
                    $this.closest(me.selectors.menu).prev().trigger('click');
                }
                else {
                    var filterType = $(this).closest("ul").closest("li").data("type");
                    var filterItems = me.$filtersList.children("li[data-type='" + filterType + "']").children(me.selectors.pillListWrap).children();
                    var pill = $(this).closest("li");
                    if (action == "not") {
                        filterItems.eq(1).append(pill);
                    }
                    else {
                        filterItems.eq(0).append(pill);
                    }
                    me._showHidePillList(filterItems);
                }
                me._stopPropagation(e);
            }).delegate(this.selectors.saveList, 'click', function () {//Save List
                //Get the source lookup component if its available else use the dummy lookUp
                var scLC = $("#" + me.$element.attr("id") + "_Source").findComponent(DJ.UI.SearchCategoriesLookUp), filters = me._getFilters("Source");
                if (scLC) {
                    scLC.saveSourceList(filters);
                }
                else {
                    me._dummyLookUpC.saveSourceList(filters);
                }
            });

            //News Filters
            if (this.$newsFilter.length > 0) {
                this._nFC = this.$element.find(".dj_SearchNewsFilter:first").findComponent(DJ.UI.SearchNewsFilter);
                $dj.subscribe(this._nFC.events.onAllFiltersRemoved, function () {
                    me.$newsFilter.slideUp(200);
                });
            }

            //Subscribe to source list delete event
            $dj.subscribe(this._dummyLookUpC.events.onSrcLstDelete, $dj.delegate(this, this._onSourceListDelete));
        },

        _setCustomDate: function (date, input) {
            $(input).val(date);
        },

        _getDateFormat: function (datePicker) {
            var seperator = (this.options.dateFormatDisplay.indexOf("-") > -1 ? "-" : "/"), yyyy = (datePicker ? "yy" : "yyyy"),
            format = "mm" + seperator + "dd" + seperator + yyyy;
            if (this.options.dateFormat == $dj.dateFormat.MMDDCCYY) {
                format = "mm" + seperator + "dd" + seperator + yyyy;
            }
            else if (this.options.dateFormat == $dj.dateFormat.DDMMCCYY) {
                format = "dd" + seperator + "mm" + seperator + yyyy;
            }
            else if (this.options.dateFormat == $dj.dateFormat.CCYYMMDD) {//No seperator for ISO format
                format = yyyy + "mmdd";
            }
            return format;
        },

        _resetFilters: function () {
            var hasAllLang = (this.$filtersList.children("[data-type='Language']").children(this.selectors.pillListWrap)
                            .children().eq(0).children("[code=alllang]").length > 0);
            if ((hasAllLang && this.$filtersList.children(this.selectors.expanded).length > 1) || (!hasAllLang && this.$filtersList.children(this.selectors.expanded).length > 0)) {
                $dj.confirmDialog({
                    yesClickHandler: $dj.delegate(this, function () {
                        var filterItemPillList, pillList, notPillList, me = this;
                        this.$filtersList.children(this.selectors.expanded).each(function () {
                            filterItemPillList = $(this).children(me.selectors.pillListWrap).children();
                            pillList = filterItemPillList.eq(0);
                            notPillList = filterItemPillList.eq(1);
                            pillList.children().remove().end().append(me._addPill).show().closest("li").removeClass("expanded");
                            notPillList.children(":gt(0)").remove().end().hide();
                        });
                        this._addAllLanguages();
                    }),
                    title: "<%= Token('filterResetTitle') %>",
                    msg: "<%= Token('filterResetMsg') %>"
                });
            }
        },

        _addAllLanguages: function () {
            this._bindFilters({ include: [{ code: "alllang", desc: "<%= Token('allLanguages') %>"}] }, "Language");
        },

        _showExclusions: function () {

            if (this.$excludeBtn.hasClass("disabled-btn")) {
                return;
            }

            if (!this.$exclusionsContainer) {
                this.$exclusionsContainer = this._getModal({
                    idSuffix: "exclusionsContainer",
                    doneHandler: $dj.delegate(this, this._onExclusionsDoneClick),
                    cancelHandler: $dj.delegate(this, this._closeModal, '$exclusionsContainer'),
                    title: "<%= Token('exclusions') %>",
                    cancelText: "<%= Token('cancel') %>",
                    showFooter: true
                });

                //Create exclusion content
                this.$exclusionsContainer.addClass('dj_exclude').children(":last").children().children(this.selectors.modalContent)
                .prepend(this.templates.exclusions({ exclusions: this.data.exclusionsList, idPrefix: this.$element.attr('id') }));
            }
            var excludedItems = this.$excludeBtn.data("excludedItems");
            if (excludedItems) {
                this.$exclusionsContainer.children(":last").children().children(this.selectors.modalContent)
                .find("input").attr("checked", function () {
                    return ($.inArray(($(this).val()), excludedItems) != -1);
                });
            }
            this.$exclusionsContainer.overlay({ closeOnEsc: true });
        },

        _onExclusionsDoneClick: function () {
            this._closeModal('$exclusionsContainer');
            var excludedItems = [];
            this.$exclusionsContainer.children(":last").children().children(this.selectors.modalContent).find("input:checked")
            .each(function () {
                excludedItems.push($(this).val());
            });
            this.$excludeBtn.data("excludedItems", excludedItems);
            this.$excludeBtn.find("span").text(excludedItems.length);
        },

        _lookUpSearchCategory: function (filterType, title) {
            var lookUpId = this.$element.attr("id") + "_" + filterType;
            var lookUp = $("#" + lookUpId), isSource = (filterType == 'Source');
            var scLC;
            if (!lookUp.length) {
                lookUp = this._getLookUpView(lookUpId);
                var data = {};

                if ((filterType == 'Language') || isSource) {//We need language list for souce entity info
                    data.languageList = this._dummyLookUpC.data.languageList;
                }

                if (isSource) {
                    data.sourceGroup = this.data.sourceGroup;
                    data.sourceFilters = this.data.additionalSourceFilters || [];
                    data.additionalSourceFilters = this.data.additionalSourceFilters;
                }

                data.filters = this._getFilters(filterType);

                if (this.data.lookUpAdditionalFooterNotes) {
                    data.additionalFooterNote = this.data.lookUpAdditionalFooterNotes[filterType];
                }

                //Initialize the Search Categories Look Up component
                scLC = $("#" + lookUpId).dj_SearchCategoriesLookUp({
                    options: { filterType: filterType,
                        suggestServiceUrl: this.options.suggestServiceUrl,
                        interfaceLanguage: this.options.preferences.interfaceLanguage,
                        taxonomyServiceUrl: this.options.taxonomyServiceUrl,
                        queriesServiceUrl: this.options.queriesServiceUrl,
                        productId: this.options.productId,
                        enableSaveList: true,
                        enableBrowse: true,
                        enableSourceList: true,
                        showFooter: true
                    }, data: data
                });

                if (!this.subscribedToLookUpEvents) {
                    //On Resize
                    $dj.subscribe(scLC.events.onResize, function () { $().overlay.rePosition(); });
                    //On AddToSearch Click
                    $dj.subscribe(scLC.events.onAddToSearchClick, $dj.delegate(this, this._onLookUpAddToSearchClick));
                    //On Cancel Click
                    $dj.subscribe(scLC.events.onCancelClick, $dj.delegate(this, this._onLookUpCancelClick));

                    this.subscribedToLookUpEvents = true;
                }
            }
            else {
                //Find the component and set the data
                scLC = $("#" + lookUpId).findComponent(DJ.UI.SearchCategoriesLookUp);
                //Bind the filters
                scLC.bindFilters(this._getFilters(filterType));
            }

            //Set the LookUp active tab based on filter type
            this._setLookUpActiveTab(filterType, scLC);

            //Set the title
            this.$lookUpsContainer.children(':first').children("h3").html(title);

            //Show the current search lookup and hide all other
            $("#" + lookUpId).removeClass('hidden').siblings().addClass('hidden');

            this.$lookUpsContainer.overlay({
                closeOnEsc: true,
                onShow: $dj.delegate(this, function () { this._onLookUpShow(filterType, scLC, lookUpId); })
            });
        },

        _setLookUpActiveTab: function (filterType, scLC) {
            switch (filterType) {
                case this.filterDetails[this.filterType.Language].name: break;
                case this.filterDetails[this.filterType.Subject].name:
                case this.filterDetails[this.filterType.Region].name:
                    scLC.setActiveTab(1); //Browse tab
                    break;
                default:
                    scLC.setActiveTab(0); //Lookup tab
                    break;
            }
        },

        _onLookUpShow: function (filterType, scLC, lookUpId) {

            //Hack - Only for IE7 to fix other lookup containers being visible even though they are hidden
            if ($.browser.msie && ($.browser.version == 7)) {
                $("#" + lookUpId).removeClass('hidden').siblings().addClass('hidden');
            }

            switch (filterType) {
                case this.filterDetails[this.filterType.Language].name:
                case this.filterDetails[this.filterType.Subject].name:
                case this.filterDetails[this.filterType.Region].name:
                    break;
                default:
                    scLC.focusOnTextBox(); //Set focus on lookup textbox
                    break;
            }

            scLC.updateFilterScroll();
        },

        _onLookUpAddToSearchClick: function (args) {
            this._closeModal('$lookUpsContainer');
            this._bindFilters(args.filters, this.filterDetails[args.filterType].name);
        },

        _onLookUpCancelClick: function () {
            this._closeModal('$lookUpsContainer');
        },

        _getLookUpView: function (lookUpId) {
            if (!this.$lookUpsContainer) {
                this.$lookUpsContainer = this._getModal({ idSuffix: "lookUpsContainer" });
                this.$lookUpsContainer.addClass("dj_lookup");
            }

            return $("<div />")
                   .attr({ "id": lookUpId, "class": "dj_SearchCategoriesLookUp ui-component" })
                   .prependTo(this.$lookUpsContainer.children(":last").children().children(this.selectors.modalContent));
        },

        _getModal: function (options) {

            var id = this.$element.attr("id") + "_" + options.idSuffix;

            $(this.templates.modalDialog()).attr("id", id)
            .appendTo(this.$element);

            var $modal = $("#" + id);

            if (options.title) {
                $modal.children(':first').children("h3").html(options.title);
            }

            var $footer = $modal.children(":last").children().children(this.selectors.modalContent).children(this.selectors.footer).show();
            if (options.showFooter) {
                var $doneBtn = $footer.children(this.selectors.doneBtn);
                var $cancelBtn = $footer.children(this.selectors.cancelBtn);
                //Done click
                $doneBtn.click(options.doneHandler);
                //Cancel click
                $cancelBtn.click(options.cancelHandler);



                if (options.doneText) {
                    $doneBtn.html(options.doneText);
                }

                if (options.cancelText) {
                    $cancelBtn.html(options.cancelText);
                }
            }
            else {
                $footer.remove();
            }

            return $modal;
        },

        _closeModal: function ($modal) {
            $().overlay.hide("#" + this[$modal].attr("id"));
        },

        _onSourceListDelete: function (listId) {
            var sourceLstitem = this.$filtersList.children("li[data-type='Source']").find("li[code='" + listId + "']");
            if (sourceLstitem.length > 0) {
                this._onFilterClose(sourceLstitem.find(this.selectors.filterClose));
            }
        },

        _expandAndAppendFilterItem: function (filterItem) {
            var expandedFilterItems = this.$filtersList.children(this.selectors.expanded),
                filterSortPos = $.inArray(filterItem.data('type').toLowerCase(), this.sortedChannelFilters);

            if (expandedFilterItems.length == 0 || filterSortPos == 0) {//First item
                this.$filtersList.prepend(filterItem);
            }
            else if (filterSortPos == (this.sortedChannelFilters.length - 1)) {//Last item
                expandedFilterItems.last().after(filterItem);
            }
            else {
                //Append the filter in the sequence defined in sortedChannelFilters object
                //Sequence is ['source', 'author', 'executive', 'company', 'subject', 'industry', 'region', 'language']
                this._sortAndAppendFilterItem(filterItem, true);
            }
            filterItem.addClass('expanded');
        },

        _processSourceFilterFromDom: function (elem) {
            var f = [], $item = $(elem), type, code, desc;
            if ($item.data("code1")) {
                type = $item.data("type");
                code = (type == "SN") ? unescape($item.data("code")) : $item.data("code");
                desc = (type == "SN") ? code : unescape($item.data("desc"));

                f.push({ code: code, desc: desc, type: type });

                type = $item.data("type1");
                code = (type == "BY") ? unescape($item.data("code1")) : $item.data("code1");
                desc = (type == "BY") ? code : unescape($item.data("desc1"));

                f.push({ code: code, desc: desc, type: type });
            }
            else {
                type = $item.data("type");
                code = (type == "SN") ? unescape($item.attr("code")) : $item.attr("code");
                desc = (type == "SN") ? code : $item.find('span:first').text();

                f.push({ code: code, desc: desc, type: type });
            }
            return f;
        },

        _processSourceFilterFromObject: function (filter) {
            var f, item, type, code, eCode;
            item = filter[0];
            type = item.type;
            code = item.code;
            eCode = (type == "SN") ? escape(code) : '';
            f = [{
                code: eCode || code.toLowerCase(),
                desc: eCode || escape(item.desc),
                type: type,
                cdesc: item.desc
            }];

            if (filter.length > 1) {//If multiple filters
                item = filter[1];
                type = item.type;
                code = item.code;
                eCode = (type == "BY") ? escape(code) : '';
                f.push({
                    code: eCode || code.toLowerCase(),
                    desc: eCode || escape(item.desc),
                    type: type
                });

                if (type == "BY") {
                    f[0].cdesc += " (" + item.desc + ")";
                }
                else {
                    f[0].cdesc += ": " + item.desc;
                }
            }
            return f;
        },

        _bindFilters: function (filters, filterType, setQueryOperator) {
            var filterItem = this.$filtersList.children("li[data-type='" + filterType + "']");

            var filterItemPillList = filterItem.children(this.selectors.pillListWrap).children();
            var pillList = filterItemPillList.eq(0);
            var notPillList = filterItemPillList.eq(1);

            //Clean existing filters
            pillList.children().remove();
            notPillList.children(":gt(0)").remove();

            if (filters && ((filters.include && filters.include.length > 0) || (filters.exclude && filters.exclude.length > 0) || filters.list)) {
                var hasIncludes = (filters.include && filters.include.length > 0),
                    hasExcludes = (filters.exclude && filters.exclude.length > 0);

                if (!filterItem.hasClass("expanded")) {
                    this._expandAndAppendFilterItem(filterItem);
                }

                if (filterType == this.filterDetails[this.filterType.Source].name) {
                    if (filters.list) {
                        filters.list.type = "LIST";
                        pillList.append(this.templates.sourceFilterPill({ filter: filters.list }));
                        //Hide Save List
                        filterItem.children(this.selectors.filtersToolbar).children(this.selectors.saveList).addClass("hidden");
                    }
                    else {
                        if (hasIncludes) {
                            for (var i = 0, l = filters.include.length; i < l; i++) {
                                pillList.append(this.templates.sourceFilterPill({ filter: this._processSourceFilterFromObject(filters.include[i]) }));
                            };
                        }

                        if (hasExcludes) {
                            for (var i = 0, l = filters.exclude.length; i < l; i++) {
                                notPillList.append(this.templates.sourceFilterPill({ filter: this._processSourceFilterFromObject(filters.exclude[i]) }));
                            };
                        }

                        //Show Save List
                        filterItem.children(this.selectors.filtersToolbar).children(this.selectors.saveList).removeClass("hidden");
                    }
                }
                else {
                    if (hasIncludes) {
                        for (var i = 0, l = filters.include.length; i < l; i++) {
                            pillList.append(this.templates.filterPill({ filter: filters.include[i] }));
                        };
                    }

                    if (hasExcludes) {
                        for (var i = 0, l = filters.exclude.length; i < l; i++) {
                            notPillList.append(this.templates.filterPill({ filter: filters.exclude[i] }));
                        };
                    }

                    if (setQueryOperator) {
                        this._setQueryOperator(filterItem.children(this.selectors.filtersToolbar).children(this.selectors.toggleOperatorSwitch),
                                        filters.operator == this.searchOperator.And);
                    }

                    if (filterType == this.filterDetails[this.filterType.Language].name) {
                        if ((filters.include.length == 1) && (filters.include[0].code == "alllang")) {//All languages
                            //Remove the filterClose span
                            pillList.children(":first").find(this.selectors.filterClose).remove();
                        }
                    }
                }
            }
            else {
                filterItem.removeClass("expanded");
            }
            this._showHidePillList(filterItem.children(this.selectors.pillListWrap).children());
        },

        _setQueryOperator: function (toggleSwitch, AND) {
            var $switch = toggleSwitch.children("span.switch");
            AND ? $switch.addClass("on") : $switch.removeClass("on");
            $switch.html(toggleSwitch.find('> .text-behind > span:' + ($switch.hasClass("on") ? 'last' : 'first')).html());
        },

        _getFilters: function (filterType) {//filterType is string
            var f = {}, $this, type = this.filterType, filterItem, item, me = this, $this, operator;
            if (filterType) {
                type = {};
                type[filterType] = this.filterType[filterType];
            }

            var noFilters = true, filterItem, $filterItems;
            $.each(type, function (key, val) {
                item = {};

                filterItem = me.$filtersList.children("li[data-type='" + key + "']");

                if (filterItem.hasClass("expanded")) {//Check if the item has filters
                    filterItem = filterItem.children(me.selectors.pillListWrap).children();
                    item = { include: [], exclude: [] };

                    if (key == me.filterDetails[me.filterType.Source].name) {

                        $filterItems = filterItem.eq(0).children("[code]");
                        if ($filterItems.length == 1 && $filterItems.eq(0).data("type") == "LIST") {
                            item.list = { code: $filterItems.eq(0).attr("code"), desc: $.trim($filterItems.eq(0).find('span:first').text()) };
                        }
                        else {
                            item.operator = me.searchOperator.Or;

                            $.each($filterItems, function () {
                                item.include.push(me._processSourceFilterFromDom(this));
                            });

                            $.each(filterItem.eq(1).children("[code]"), function () {
                                item.exclude.push(me._processSourceFilterFromDom(this));
                            });
                        }

                    }
                    else {
                        item.operator = (filterItem.children(me.selectors.filtersToolbar)
                                .children(me.selectors.toggleOperatorSwitch).children("span.switch").hasClass("on")) ? me.searchOperator.And : me.searchOperator.Or;

                        //Include filters
                        $.each(filterItem.eq(0).children("[code]"), function () {
                            $this = $(this);
                            item.include.push({ code: $this.attr('code'), desc: $this.find('span:first').text() });
                        });

                        //Exclude filters
                        $.each(filterItem.eq(1).children("[code]"), function () {
                            $this = $(this);
                            item.exclude.push({ code: $this.attr('code'), desc: $this.find('span:first').text() });
                        });
                    }

                    //While checking for NO filters ignore Language and Source filter
                    if ((key != me.filterDetails[me.filterType.Language].name) && (key != me.filterDetails[me.filterType.Source].name)) {

                        item.noFilters = (item.include.length == 0 && item.exclude.length == 0);

                        if (noFilters && (item.noFilters == false)) {
                            noFilters = false;
                        }

                        //Remove unwanted property after usage
                        item.noFilters = undefined;
                    }
                }
                else {
                    item = null;
                }

                f[key] = item;
            });

            f.noFilters = noFilters;

            //Need to check the filters
            f.noNewsFilters = !this.$newsFilter.is(":visible");

            return filterType ? (f[filterType] || null) : f;
        },

        _onFilterClose: function (elem) {
            var $elem = $(elem), filterType = $elem.closest("ul").closest("li").data("type");
            var filterItems = this.$filtersList.children("li[data-type='" + filterType + "']").children(this.selectors.pillListWrap).children();
            $elem.closest(this.selectors.filterPill).remove();
            if (filterType == this.filterDetails[this.filterType.Language].name) {
                //If all pills are removed then add All languages filter
                if (filterItems.eq(0).children().filter("[code]").length == 0) {
                    this._addAllLanguages();
                }
            }
            else {
                this._showHidePillList(filterItems);
            }
        },

        _onFilterClick: function (elem) {
            var $li = $(elem).closest(this.selectors.filterPill);
            if ($li.hasClass("add") || ($li.data('type') == "LIST")) {
                return; //Do not show filter options for a list
            }
            this.$filterOptions.children("div").children().show().filter(":eq(" + ($(elem).closest("ul").hasClass("not-filter") ? "1" : "2") + ")").hide();
            $li.append(this.$filterOptions);
            $(document).unbind('click.sbc').one('click.sbc', $dj.delegate(this, function () {
                this.$filterOptions.hide();
            }));
            this.$filterOptions.show();
        },

        _showHidePillList: function (filterItems) {
            var pillList = filterItems.eq(0).hide();
            var notPillList = filterItems.eq(1).hide();
            var hasPills = (pillList.children("[code]").length > 0), hasNotPills = (notPillList.children("[code]").length > 0);
            if (hasNotPills) {
                notPillList.children(this.selectors.addPill).remove();
                notPillList.append(this._addPill).show();
            }
            if (hasPills) {
                pillList.show().children(this.selectors.addPill).remove();
                if (!hasNotPills) {
                    pillList.append(this._addPill);
                }
            }
            if (!hasPills && !hasNotPills) {
                pillList.children(this.selectors.addPill).remove();
                var $li = pillList.append(this._addPill).show().closest("li").removeClass("expanded");
                //                //Move the empty channel filter after the last expanded channel filter
                //                $li.parent().children(".expanded").last().after($li);
                //Move all the empty channel in the order after all expanded channel filter
                this._sortAndAppendFilterItem($li, false);
            }
        },

        _sortAndAppendFilterItem: function (filterItem, expanded) {
            var me = this, itemSortPos, $refElement, $this,
                filterItems = filterItem.siblings(expanded ? '.expanded' : ':not(.expanded)'),
                filterSortPos = $.inArray(filterItem.data('type').toLowerCase(), this.sortedChannelFilters);

            filterItems.each(function () {
                $this = $(this);
                itemSortPos = $.inArray($this.data('type').toLowerCase(), me.sortedChannelFilters);
                if (itemSortPos > filterSortPos) {
                    if (!$refElement) {
                        $this.before(filterItem);
                    }
                    return false;
                }
                $refElement = $this;
            });

            if ($refElement) {
                $refElement.after(filterItem);
            }
        },

        _stopPropagation: function (e, hideMenu) {
            e.stopPropagation();
            if (hideMenu) {
                this.$filterOptions.hide();
            }
        },

        _validate: function () {

            var result = { isValid: false };
            var filters = this._getFilters();
            var searchQuery = $.trim(this.$searchTexBox.val());
            var errorMsgs = [];
            if (searchQuery.length > this.options.searchQueryMaxLength) {//Check for search query max(default 2048) length
                errorMsgs.push("<%= Token('errorForUser210101') %>");
            }
            else if ((!searchQuery || (searchQuery == this._sbSearchBoxWMText)) && filters.noFilters && filters.noNewsFilters) {//No search query, no filters and no Newsfilters
                errorMsgs.push("<%= Token('useIntIdxOrEnterWords') %>");
            }

            if (this.$dateDD.val() == this.$dateDD.find("option:last").val()) {
                if (!this.$startDate.val() && !this.$endDate.val()) {
                    errorMsgs.push("<%= Token('customDateSelected') %>");
                }
                else {
                    var startDate = $dj.validDate(this.$startDate.val(), this.options.dateFormat, $dj.dateFormat.MMDDCCYY);
                    var endDate = $dj.validDate(this.$endDate.val(), this.options.dateFormat, $dj.dateFormat.MMDDCCYY);
                    if (!startDate || !endDate) {
                        errorMsgs.push("<%= Token('errorForUser110141') %>");
                    }
                    else {
                        result.startDate = startDate;
                        result.endDate = endDate;
                    }
                }
            }

            result.filters = filters;
            result.error = errorMsgs.join("\n- ");

            result.isValid = !result.error;

            return result;
        },

        getRequestObject: function () {

            var validationResult = this._validate();
            if (validationResult && validationResult.isValid) {
                var reqObj = {}, f;

                reqObj.freeText = (this.$searchTexBox.val() != this._sbSearchBoxWMText) ? $.trim(this.$searchTexBox.val()) : "";
                reqObj.searchIn = this.$searchInDD.val();

                reqObj.dateRange = this.$dateDD.val();
                if (reqObj.dateRange == this.$dateDD.find("option:last").val()) {
                    reqObj.startDate = (new Date(validationResult.startDate)).format("yyyymmdd");
                    reqObj.endDate = (new Date(validationResult.endDate)).format("yyyymmdd");
                }

                reqObj.exclusionFilter = this.$excludeBtn.data("excludedItems");

                if (this.options.showDisplayOptions) {
                    reqObj.sortBy = this.$sortByDD.val();
                    reqObj.duplicates = this.$duplicate.is(":checked");
                }

                reqObj.contentLanguages = [];

                //Channel Filters
                if (validationResult.filters) {
                    reqObj.filters = {
                        company: validationResult.filters.Company,
                        author: validationResult.filters.Author,
                        executive: validationResult.filters.Executive,
                        subject: validationResult.filters.Subject,
                        industry: validationResult.filters.Industry,
                        region: validationResult.filters.Region,
                        source: validationResult.filters.Source
                    }

                    //Only for language we need to send an array of string(lang code)
                    if (validationResult.filters && validationResult.filters.Language && validationResult.filters.Language.include
                    && validationResult.filters.Language.include.length > 0 && validationResult.filters.Language.include[0].code != "alllang") {
                        $.each(validationResult.filters.Language.include, function () {
                            reqObj.contentLanguages.push(this.code);
                        });
                        validationResult.filters.Language = null;
                    }
                }

                //News fitler[Search Query Filter]
                if (!validationResult.filters.noNewsFilters) {
                    reqObj.newsFilters = this._nFC.getFilters();
                }

                validationResult.request = reqObj;
            }

            //Remove all the unwanted object from validationResult object
            validationResult.filters = undefined;
            validationResult.noNewsFilters = undefined;

            return validationResult;
        },


        setData: function (data, onInit) {
            this.data = data;

            if (this.data) {
                var me = this, d = this.data;

                //Bind the filters
                if (d.filters) {
                    var fDetails = this.filterDetails;
                    $.each(this.channelFilters, function (i, val) {
                        if (d.filters[val]) {
                            me._bindFilters(d.filters[val], fDetails[i].name, true);
                        }
                    });
                }

                //Languages
                if (d.contentLanguages && (d.contentLanguages.length > 0)) {
                    var languagesList = {};
                    $.each(this._dummyLookUpC.data.languageList, function () {
                        languagesList[this.code] = this.desc;
                    });
                    var lang = { include: [] }, langCode;
                    $.each(d.contentLanguages, function () {
                        langCode = this.toLowerCase();
                        lang.include.push({ code: langCode, desc: languagesList[langCode] });
                    });
                    this._bindFilters(lang, "Language");
                }
                else {
                    //Add all languages
                    this._addAllLanguages();
                }

                if (!onInit) {

                    //Search In
                    this.$searchInDD.val(d.searchIn).change();

                    //Date Range
                    if (!d.dateRange) {
                        this.$dateDD.val(d.dateRange).change();
                        if (d.dateRange == this.$dateDD.find("option:last").val()) {//Custom date range
                            try {
                                var s = d.startDate;
                                if (s && s.length == 8 && !isNaN(s)) {
                                    s = s.substring(4, 6) + "/" + s.substring(6) + "/" + s.substring(0, 4);
                                    this.$startDate.val((new Date(s)).format(this._getDateFormat()));
                                }

                                var e = d.endDate;
                                if (e && e.length == 8 && !isNaN(e)) {
                                    e = e.substring(4, 6) + "/" + e.substring(6) + "/" + e.substring(0, 4);
                                    this.$endDate.val((new Date(e)).format(this._getDateFormat()));
                                }
                            }
                            catch (e) {
                            }
                        }
                    }

                    //Sort By && Duplicates
                    if (this.options.showDisplayOptions) {
                        this.$sortByDD.val(d.sortBy).change();
                        this.$duplicate.attr("checked", d.duplicates);
                    }
                }

                //Exclusions
                if (d.exclusionFilter) {
                    this.$excludeBtn.data("excludedItems", d.exclusionFilter).find("span").text(d.exclusionFilter.length);
                }

                //Set the search query and focus on it
                this.$searchTexBox.val(d.freeText || '').focus();
            }
        }

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_SearchBuilder', DJ.UI.SearchBuilder);

    $dj.debug('Registered DJ.UI.SearchBuilder (extends DJ.UI.Component)');

})(jQuery);
