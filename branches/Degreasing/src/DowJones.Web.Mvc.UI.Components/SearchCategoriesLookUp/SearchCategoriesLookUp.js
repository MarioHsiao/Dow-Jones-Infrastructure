/*
*  Search Categories Lookup Control
*/

    DJ.UI.SearchCategoriesLookUp = DJ.UI.Component.extend({

        selectors: {
            filtersContainer: 'div.filters',
            filterItems: 'div.filter-items',
            filters: 'ul.yellow',
            notfilters: 'ul.red',
            filterClose: 'span.remove',
            filterPill: 'li.dj_pill',
            notFilterPill: 'li.not',
            searchBox: 'div.dj_lookup-search',
            textBox: 'input.dj_lookup-search-field',
            listItem: '.item',
            browseItem: 'div.browse-item',
            searchBtn: 'input.dj_lookup-search-submit',
            lookUpList: '.lookup-results',
            lookUpListContainer: '.lookup-results-container',
            listItemOptions: '.icon-group span',
            resultsTitle: '.results-title',
            doneBtn: 'span.dj_btn-blue',
            cancelBtn: 'span.dj_btn-drk-gray',
            menuItem: 'div.menuitem',
            menu: 'div.menu',
            modalNav: 'ul.modal-nav',
            tabContent: '.tab-content',
            footer: '.footer',
            filterScroll: '.filter-scroll',
            fScrollUp: '.js_scroll-up',
            fScrollDown: '.js_scroll-down',
            browseTree: '.dj_browse-tree',
            browseToggle: 'span.dj_browse-tree-toggle',
            selectMenuContainer: '.dj_select-box-alt',
            selectMenu: '.dj_btn-select-menu',
            multiLevelMenu: '.dj_menu-multi-level',
            pagging: '.dj_paging',
            prevPage: '.dj_icon-arrow-green-left',
            nextPage: '.dj_icon-arrow-green-right',
            deleteLnk: 'span.delete-link',
            modalClose: 'p.dj_modal-close',
            formFields: '.dj_form-field',
            filtersList: '.filterList',
            editList: '.edit-list',
            alphabetList: '.alphabet-list',
            modalTitle: '.dj_modal-title',
            modalContent: 'div.dj_modal-content'
        },

        events: {
            onSearchClick: 'onSearchClick.dj.SearchCategoriesLookUp',
            onResize: 'onResize.dj.SearchCategoriesLookUp',
            onSrcLstDelete: 'onSrcLstDelete.dj.SearchCategoriesLookUp'
        },

        lookUpDetails: [
            {
                name: 'Company',
                autoCompleteText: "<%= Token('companyAutoCompleteText') %>",
                lookUpTitle: "<%= Token('companyResults') %>",
                notFilter: true,
                restField: 'Companies',
                //'CompanyStatus|Instrument|SecondaryRegionCodes|Address|ICBCodes|IndexCodes|Notes|SecondaryIndustryCodes|ThirdPartyIndustryCodes|UltimateParentCodes|NewsSearch|ChildCodes|Aliases|DJCodes|DJNewsCodes|Status|OrganizationTypes|LocalNames|ParentCodes'
                infoParts: 'SecondaryRegionCodes',
                label: "<%= Token('companyLabel') %>"
            },

            {
                name: 'Author',
                autoCompleteText: "<%= Token('authorAutoCompleteText') %>",
                lookUpTitle: "<%= Token('authorLookupResults') %>",
                notFilter: false,
                restField: 'Authors',
                infoParts: 'LocalNames',
                label: "<%= Token('author') %>"
            },

            {
                name: 'Executive',
                autoCompleteText: "<%= Token('executiveAutoCompleteText') %>",
                lookUpTitle: "<%= Token('executiveLookupResults') %>",
                notFilter: true,
                restField: 'Executives',
                infoParts: 'LocalNames',
                label: "<%= Token('executive') %>"
            },

            {
                name: 'Subject',
                autoCompleteText: "<%= Token('subjectAutoCompleteText') %>",
                lookUpTitle: "<%= Token('subjectLookupResults') %>",
                browse: true,
                notFilter: true,
                restField: 'Subjects',
                autoCompleteField: 'NewsSubject',
                //'ChildCodes|Aliases|Description|ThirdPartyCodes|DJCodes|ParentCodes|LocalNames|Status'
                infoParts: 'Description|ParentCodes',
                label: "<%= Token('subject') %>"
            },

            {
                name: 'Industry',
                autoCompleteText: "<%= Token('industryAutoCompleteText') %>",
                lookUpTitle: "<%= Token('industryLookupResults') %>",
                browse: true,
                notFilter: true,
                restField: 'Industries',
                infoParts: 'Description|ParentCodes',
                label: "<%= Token('industry') %>"
            },

            {
                name: 'Region',
                autoCompleteText: "<%= Token('regionAutoCompleteText') %>",
                lookUpTitle: "<%= Token('regionLookupResults') %>",
                browse: true,
                notFilter: true,
                restField: 'Regions',
                infoParts: 'Description|ParentCodes',
                label: "<%= Token('regionLabel') %>"
            },

            {
                name: 'Source',
                autoCompleteText: "<%= Token('sourceAutoCompleteText') %>",
                lookUpTitle: "<%= Token('sourceLookupResults') %>",
                browse: true,
                savedList: true,
                restField: 'Sources',
                infoParts: 'Verbose',
                label: "<%= Token('sourcesLabel') %>"
            },

            {
                name: 'Language'
            }
        ],

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

        init: function (element, meta) {
            var $meta = $.extend({ name: "SearchCategoriesLookUp" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this.setData();
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnSearchClick: $dj.delegate(this, this._onSearchClick),
                OnAutoSuggestItemSelect: $dj.delegate(this, this._onAutoSuggestItemSelect),
                OnResize: $dj.delegate(this, this._triggerResize),
                OnAutoSuggestInfoClick: $dj.delegate(this, this._onAutoSuggestInfoClick),
                OnAutoSuggestNotClick: $dj.delegate(this, this._onAutoSuggestNotClick)
            });
        },

        _setDefaultProperties: function () {

            this._loadingText = "<%= Token('loading') %>...";

            if (isNaN(this.options.filterType))
                this.options.filterType = this.filterType[this.options.filterType];

            this._lookUpEnabled = (this.options.filterType != this.filterType.Language);

            this._lookUpDetails = this.lookUpDetails[this.options.filterType];

            this._setListItemOptions();

            this._listItemHoverEnabled = (this.options.filterType != this.filterType.Language);

            this._maxLookUpRecords = 100;
            this._lookUpOffset = 0;
        },

        _setListItemOptions: function () {
            this.$listItemOptions = $(this.templates.listItemOptions());
            if (!this._lookUpDetails.notFilter) {//Not filter not supported
                this.$listItemOptions.children().eq(1).remove();
            }
        },

        _initializeControls: function () {

            this._setDefaultProperties();

            //Set the main template
            this.$element.append(this.templates.main({ lookUpDetails: this._lookUpDetails }));

            //Append the filter options if NOT is supported
            if (this._lookUpDetails.notFilter) {
                this.$filterOptions = $(this.templates.filterOptions());
                this.$element.append(this.$filterOptions);
            }

            this.$elementChildren = this.$element.children();
            this.$tabContent = this.$elementChildren.filter(this.selectors.tabContent);

            this.$searchBox = this.$tabContent.eq(0).children(this.selectors.searchBox);
            if (this.options.filterType != this.filterType.Language) {
                this.$textBox = $(this.selectors.textBox, this.$searchBox);
                this.$searchBtn = $(this.selectors.searchBtn, this.$searchBox);
            }
            this.$filtersContainer = this.$elementChildren.filter(this.selectors.filtersContainer);
            var $filterItems = $(this.selectors.filterItems, this.$filtersContainer);
            this.$filters = $(this.selectors.filters, $filterItems);
            this.$notfilters = $(this.selectors.notfilters, $filterItems);
            this.$filterScroll = this.$filtersContainer.children(this.selectors.filterScroll);

            this.$lookUpListContainer = this.$tabContent.eq(0).children(this.selectors.lookUpListContainer);
            this.$lookUpList = this.$lookUpListContainer.children(this.selectors.lookUpList);
            if (this._lookUpDetails.browse && this.options.enableBrowse) {
                this.$browseList = this.$tabContent.eq(1).children(this.selectors.browseTree);
            }
            this.$modalNav = this.$elementChildren.filter(this.selectors.modalNav);
            this.$footer = this.$elementChildren.filter(this.selectors.footer);

            //Source is a special case, need to load other templates as well
            if (this.options.filterType == this.filterType.Source) {

                //Add bottom margin to first lookup results and source class to set the height
                this.$lookUpList.eq(0).addClass('add-margin source');

                //Add Save as List button besides filter section
                if (this.options.enableSaveList) {
                    this.$saveListBtn = $('<span class="dj_btn dj_btn-drk-gray"><%= Token("saveList") %></span>').addClass("hidden");
                    $filterItems.after(this.$saveListBtn);
                }

                if (this.options.enableBrowse) {
                    //Add the browse template inside the tabContent
                    this.$tabContent.eq(1).prepend(this.templates.sourceBrowse({ sourceGroup: this.data.sourceGroup }));

                    var $selectBoxAlt = this.$tabContent.eq(1).children(this.selectors.selectMenuContainer);
                    var $selectMenus = $selectBoxAlt.children(this.selectors.selectMenu);

                    this.$sourceGroupDD = $selectMenus.eq(0);
                    this.$sourceSortByDD = $selectMenus.eq(1);

                    //Create the source group dhtml dropdown
                    if (this.data.sourceGroup) {
                        //Add all group in sourceGroup collection
                        var sourceGroups = [{ code: "", desc: "<%= Token('all') %>", collection: this.data.sourceGroup}];

                        var me = this, $dd = this.$sourceGroupDD.children(this.selectors.multiLevelMenu), markup = [], sGItems = {};
                        me.getSourceGroupItem = function (group) {
                            var code = group.code.toLowerCase();
                            sGItems[code] = group.desc;
                            markup.push('<div data-value="' + code + '">' + group.desc + '</div>');
                            if (group.collection && group.collection.length) {
                                markup.push('<ul>');
                                $.each(group.collection, function () {
                                    markup.push('<li>');
                                    markup.push(me.getSourceGroupItem(this));
                                    markup.push('</li>');
                                });
                                markup.push('</ul>');
                            }
                        };
                        $.each(sourceGroups, function () {
                            markup.push('<li>');
                            markup.push(me.getSourceGroupItem(this));
                            markup.push('</li>');
                        });
                        this._sourceGroupItems = sGItems;
                        $dd.html(markup.join(''));

                        //Set the initial value in the dropdown menu
                        var initialValue = sourceGroups[0];
                        this.$sourceGroupDD.data("value", initialValue.code).children("span:first").html(initialValue.desc);
                    }

                    this.$alphabetList = $selectBoxAlt.children(this.selectors.alphabetList);
                }

                if (this.options.enableSourceList) {
                    //Add source Lists template inside the tabContent
                    this.$savedList = this.$tabContent.eq(2).children(this.selectors.browseTree);
                }
            }
            else {
                this.$lookUpListContainer.children(":gt(2)").remove();
            }

            //Pagging for lookup list
            this.$lookUpPagging = this.$lookUpListContainer.children(this.selectors.pagging);
            this.$lookUpPrevPage = this.$lookUpPagging.children(this.selectors.prevPage);
            this.$lookUpNextPage = this.$lookUpPagging.children(this.selectors.nextPage);

            //Set the lookup results title
            if (this._lookUpEnabled) {
                this.$lookUpListContainer.children("h6:first").html(this._lookUpDetails.lookUpTitle);
            }
            else {
                this.$lookUpListContainer.children("h6").hide();
            }

            //Only for Language look up, change the first tab name to Languages
            if (this.options.filterType == this.filterType.Language) {
                this.$modalNav.find("span:first").html("<%= Token('languages') %>");
            }
        },

        _initializeEventHandlers: function () {

            this._initializeControls();

            var me = this;

            //Filters Operations
            this.$filtersContainer.delegate(this.selectors.filterClose, 'click', function (e) {//Filter Close
                me._onFilterClose(this);
                me._stopPropagation(e, true);
            });

            //If filter type is Source
            if (this.options.filterType == this.filterType.Source) {

                this.$filtersContainer.children(this.selectors.filterItems).addClass("source");

                if (this.options.enableBrowse) {
                    this.$sourceGroupDD.add(this.$sourceSortByDD).bind('click', function (e) {

                        var $this = $(this), $target = $(e.target);
                        $this.children(me.selectors.multiLevelMenu).toggleClass('hidden');
                        if ($target.is("div") && !$target.attr('class')) {
                            var oldVal = $this.data("value"), newVal = $target.data("value");
                            if (oldVal != newVal) {
                                $this.data("value", newVal).children("span:first").html($target.html());
                                $this.trigger('change'); //Trigger the change event
                            }
                        }
                        me._stopPropagation(e);
                        $(document).unbind('mousedown.scl').bind('mousedown.scl').click(function () {
                            me._onMouseDown();
                            $(document).unbind('mousedown.scl');
                        });
                    }).change(function () {//Source group and filter change handler
                        me._onSourceGroupFilterChange();
                    });

                    //Alphabets click event handler
                    this.$alphabetList.delegate("span", "click", function () {
                        var $this = $(this);
                        if (!$this.hasClass('active')) {
                            $this.closest('ul').find('.active').removeClass('active');
                            $this.addClass('active');
                            me._onAlphaClick($this.data("value"));
                        }
                    });
                }

                //Save List
                if (this.options.enableSaveList) {
                    this.$saveListBtn.click(function () {
                        me.saveSourceList();
                    });
                }
            }

            //Filters Click handler when NOT pills are enabled
            if (this._lookUpDetails.notFilter) {
                this.$filtersContainer.delegate(this.selectors.filterPill, 'click', function (e) {//Filter Click
                    me._onFilterClick(this);
                    me._stopPropagation(e);
                });
                this.$element.delegate(this.selectors.menuItem, 'click', function (e) {//Filter Options Click
                    var $this = $(this), action = $this.data("action"), $li = me.$filterOptions.data("li");
                    me.$filterOptions.hide();
                    if (action == "remove") {//Remove
                        $li.children(":last").trigger('click');
                    }
                    else {
                        me._addFilter($li, action == "not");
                    }

                    me._stopPropagation(e);
                });
            }

            //Filter Scroll
            this.$filterScroll.click(function (e) {
                var $target = $(e.target), up = $target.hasClass('scroll-up'), down = $target.hasClass('scroll-down');
                if (up || down) {
                    me._onFilterScroll(up);
                }
            });

            //List Item Click
            if (this.options.filterType == this.filterType.Source) {
                //Extended Sources List Item Click
                this.$lookUpList.delegate(this.selectors.listItem, 'click', function (e) {//List Item Click
                    me._onSourceItemClick(this, e);
                });
            }
            else if (me.options.filterType == me.filterType.Language) {
                this.$lookUpList.eq(0).delegate(this.selectors.listItem, 'click', function (e) {//List Item Click
                    me._onListItemClick(this, e);
                    //Remove all allang if its added
                    me.$filters.children("[code=alllang]").remove();
                });
            }
            else {
                this.$lookUpList.eq(0).delegate(this.selectors.listItem, 'click', function (e) {//List Item Click
                    me._onListItemClick(this, e);
                });
            }

            //List Item Hover
            if (this._listItemHoverEnabled) {
                this.$lookUpList.eq(0).delegate(this.selectors.listItem, 'mouseenter', function (e) {//List Item Hover
                    me._onListItemHover(this, e);
                });
                this.$lookUpList.eq(0).delegate(this.selectors.listItemOptions, 'click', function (e) {
                    me._onListItemOptionsClick(this, e);
                });
            }

            //If LookUp enabled initialize textbox else hide it
            this._lookUpEnabled ? this._bindTextBoxHandlers() : this.$searchBox.hide();

            //Tabs
            if (this._lookUpDetails.browse || this._lookUpDetails.savedList) {
                //Browse
                if (this._lookUpDetails.browse && this.options.enableBrowse) {
                    this.$modalNav.children(":eq(1)").show();

                    this.$browseList.delegate(this.selectors.listItemOptions, 'click', function (e) {
                        me._onListItemOptionsClick(this, e);
                    });

                    if (this.options.filterType == this.filterType.Source) {
                        this.$browseList.delegate(this.selectors.browseItem, 'mouseenter', function (e) {//List Item Hover
                            if (me.$sourceSortByDD.data("value")) {//If source category is selected
                                me._onListItemHover(this, e);
                            }
                        }).delegate(this.selectors.browseItem, 'click', function (e) {
                            me._onSourceGroupItemClick(this, e);
                        }).delegate(this.selectors.browseToggle, 'click', function (e) {//Browse Icon click
                            me._onBrowseMoreClick(this, e, { sourceGroupBrowse: true });
                        });
                    }
                    else {
                        this.$browseList.delegate(this.selectors.browseItem, 'mouseenter', function (e) {//List Item Hover
                            me._onListItemHover(this, e);
                        }).delegate(this.selectors.browseItem, 'click', function (e) {
                            me._onListItemClick(this, e);
                        }).delegate(this.selectors.browseToggle, 'click', function (e) {//Browse Icon click
                            me._onBrowseMoreClick(this, e);
                        });
                    }
                }
                else {
                    this.$tabContent.eq(1).remove();
                }

                //SavedList
                if (this._lookUpDetails.savedList && this.options.enableSourceList) {
                    this.$modalNav.children(":eq(2)").show();
                    this.$savedList.delegate(this.selectors.browseItem, 'click', function (e) {
                        me._onSourceItemClick(this, e);
                    }).delegate(this.selectors.browseItem, 'mouseenter', function (e) {//List Item Hover
                        me._onSourceListItemHover(this, e);
                    }).delegate(this.selectors.editList, 'click', function (e) {
                        me._editSourceList($(this).parent().data("code"));
                        me._stopPropagation(e);
                    }).delegate(this.selectors.browseToggle, 'click', function (e) {//Browse Icon click
                        var sourceListBrowse = $(this).hasClass('sourceList');
                        me._onBrowseMoreClick(this, e, { sourceListBrowse: sourceListBrowse, sourceGroupBrowse: !sourceListBrowse });
                    }).delegate(this.selectors.listItemOptions, 'click', function (e) {
                        me._onListItemOptionsClick(this, e);
                    });
                }
                else {
                    this.$tabContent.eq(2).remove();
                }

                this.$modalNav.delegate('li', 'click', function () {
                    me.setActiveTab($(this).index());
                });
            }

            //LookUp Pagging
            this.$lookUpPrevPage.click(function () {
                if (!$(this).hasClass("dj_icon-arrow-disabled-left")) {
                    me._getLookUpPrevPage();
                }
            });
            this.$lookUpNextPage.click(function () {
                if (!$(this).hasClass("dj_icon-arrow-disabled-right")) {
                    me._getLookUpNextPage();
                }
            });
        },

        _onSourceGroupFilterChange: function () {

            var sourceCategory = this.$sourceSortByDD.data("value")

            if (sourceCategory == "title") {
                //Show the albhabet list
                this.$alphabetList.removeClass('hidden').find('span.active').removeClass('active').end().find('span:first').addClass('active');

                //Hide the expand description
                this.$browseList.prev().hide();

                this._onAlphaClick('A');
            }
            else {
                //Hide the alphabetList
                this.$alphabetList.addClass('hidden');

                var groupCode = this.$sourceGroupDD.data("value");

                //Set the visibility of the expand description conditionally
                if (!sourceCategory) {//Filter not selected

                    if (!groupCode) {//All Source Groups
                        this.$browseList.html("<li><%= Token('allSourceGroupsSelected') %></li>");
                    }
                    else {
                        var data = [{ Code: groupCode, Name: this._sourceGroupItems[groupCode]}];
                        this.bindList(this.$browseList, data, { sourceGroupBrowse: true });
                    }
                    //Hide the expand description
                    this.$browseList.prev().show().css("visibility", "hidden");
                }
                else {
                    this.$browseList.prev().show().css("visibility", "visible");

                    this._getSourceByGroupCode({
                        groupCode: groupCode,
                        sourceCategory: sourceCategory,
                        childrenType: "group",
                        container: this.$browseList
                    });
                }
            }
        },

        _onAlphaClick: function (alpha) {
            this._getSourceByFirstCharacter({
                groupCode: this.$sourceGroupDD.data("value"),
                container: this.$browseList,
                searchString: alpha
            });
        },

        _onMouseDown: function () {
            if (this.$filterOptions) {
                this.$filterOptions.hide();
            }
            if (this.$sourceGroupDD) {
                this.$sourceGroupDD.children(this.selectors.multiLevelMenu).addClass('hidden');
                this.$sourceSortByDD.children(this.selectors.multiLevelMenu).addClass('hidden');
            }
        },

        _bindTextBoxHandlers: function () {
            var me = this;
            this.$textBox.unbind('keypress').keypress(function (e) {
                if (e.which == 13) {//Enter pressed
                    me.$searchBtn.trigger('click');
                }
            })
            .attr("id", this.$element.attr("id") + "_textBox")
            .waterMark({
                waterMarkClass: 'watermark',
                waterMarkText: this._lookUpDetails.autoCompleteText
            });

            this.$searchBtn.click(this._delegates.OnSearchClick);

            this._initializeAutoSuggest();
        },

        _editSourceList: function (sourceListId) {

            $dj.progressIndicator.display(this._loadingText);

            this._getSourceListById(sourceListId, {
                success: $dj.delegate(this, function (data) {
                    $dj.progressIndicator.hide();
                    if (data && data.SourceListQuery) {
                        var slQ = data.SourceListQuery;

                        this._initializeSourceListModal(slQ.Id);

                        //Bind the filters
                        if (data.SourceListQuery.SourceEntityFilters) {
                            this.$sLstFilters.html('');
                            var filters = data.SourceListQuery.SourceEntityFilters, f, d, e, code, type, desc, cdesc;
                            for (var i = 0; i < filters.length; i++) {
                                d = filters[i];
                                if (d.SourceEntites && d.SourceEntites[0]) {
                                    e = d.SourceEntites[0];
                                    code = e.Value;
                                    type = e.SourceEntityType;
                                    if (type == "PDF") {
                                        cdesc = this._sourceGroupItems[code]; //If PDF then get the description from source group items list
                                    }
                                    else {
                                        cdesc = e.Source ? (e.Source.Name || code) : code;
                                    }

                                    if (d.SourceEntites.length > 1) {
                                        f = (type == "SN") ? [{ code: escape(code), desc: null, type: type}]
                                                         : [{ code: code.toLowerCase(), desc: escape(cdesc), type: type}];
                                        e = d.SourceEntites[1];
                                        code = e.Value;
                                        type = e.SourceEntityType;
                                        if (type === "BY") {
                                            cdesc = cdesc + " (" + code + ")";
                                            f.push({ code: escape(code), desc: null, type: type });
                                        }
                                        else {
                                            desc = (e.Source) ? (e.Source.Name || code) : code;
                                            cdesc = cdesc + ": " + desc;
                                            f.push({ code: code.toLowerCase(), desc: escape(desc), type: type });
                                        }
                                        f[0].cdesc = cdesc;
                                    }
                                    else {
                                        code = (type == "SN") ? escape(code) : code.toLowerCase();
                                        f = [{ code: code, desc: cdesc, type: type}];
                                    }

                                    this.$sLstFilters.append(this.templates.sourceFilterPill({ filter: f }));
                                }
                            }
                            this.$sLstFiltersContainer.show();
                        }
                        else {
                            this.$sLstFiltersContainer.hide();
                        }


                        //Set the list name
                        this.$existingSLstName.val(slQ.Name);

                        this.saveSourceList(null, slQ.Id);
                    }

                    else {
                        $dj.progressIndicator.hide();
                        //TODO: Add the appropriate message
                        alert($dj.formatError({ code: -1, message: "<%= Token('failedToGetSourceDetails') %>" }));

                    }
                }),
                error: function (error) {
                    $dj.progressIndicator.hide();
                    alert($dj.formatError(error.Error || error));
                }
            });
        },

        saveSourceList: function (filtersToSave, sourceListId) {

            this._initializeSourceListModal(sourceListId);

            this._sLstfilters = !sourceListId ? (filtersToSave || this.getFilters()) : null;

            if (this._sourceList) {
                this._showSaveSourceListModal();
            }
            else {
                $dj.progressIndicator.display(this._loadingText);
                this._getSourceList({
                    success: $dj.delegate(this, function (data) {
                        this._sourceList = data;
                        if (this.$savedList) {
                            this.$savedList.data("loaded", false);
                        }
                        $dj.progressIndicator.hide();
                        this._showSaveSourceListModal();
                    }),
                    error: function (error) {
                        $dj.progressIndicator.hide();
                        alert($dj.formatError(error.Error || error));
                    }
                });
            }
        },

        _showSaveSourceListModal: function () {
            if (this.$existingSLstDD.children().length == 1) {//No source list
                if (this._sourceList && this._sourceList.SourceListQueries) {
                    var $dd = this.$existingSLstDD;
                    $.each(this._sourceList.SourceListQueries, function () {
                        $dd.append("<option value='" + this.Id + "'>" + this.Name + "</option>");
                    });
                }
            }

            this.$saveSLstModal.overlay({ closeOnEsc: true, onShow: $dj.delegate(this, function () {
                if (this._sourceListId) {
                    this.$existingSLstName.focus();
                }
                else {
                    this.$newSLstName.focus();
                }
            })
            });
        },

        _saveSourceList: function (filtersToSave) {

            //ToDO: Validation
            var listId, isEditing = this._sourceListId, addToExistingList = false;
            if (isEditing) {//Editing the source list
                if (!this.$existingSLstName.val()) {
                    alert("<%= Token('enterTheSourceListName') %>");
                    return;
                }
                listId = this._sourceListId;
            }
            else {//Save to already existing list or create new list
                if (this.$sLstRadioBtns.eq(0).is(":checked")) {
                    addToExistingList = true;
                    listId = this.$existingSLstDD.val();
                    if (!listId) {//
                        alert("<%= Token('selectTheSourceList') %>");
                        return;
                    }
                }
                else {
                    if (!this.$newSLstName.val()) {
                        alert("<%= Token('enterTheSourceListName') %>");
                        return;
                    }
                }
            }

            var me = this, filters = [], modalId = this.$saveSLstModal.attr('id');
            if (isEditing) {//Editing the filters
                var f, $item;
                //Get the filters from filtersList
                this.$sLstFilters.children().each(function () {
                    f = [];
                    $item = $(this);
                    if ($item.data("code1")) {

                        type = $item.data("type");
                        code = (type == "SN") ? unescape($item.data("code")) : $item.data("code");
                        //desc = (type == "SN") ? code : unescape($item.data("desc"));

                        f.push({ Value: code, SourceEntityType: type });

                        type = $item.data("type1");
                        code = (type == "BY") ? unescape($item.data("code1")) : $item.data("code1");
                        //desc = (type == "BY") ?  code: unescape($item.data("desc1"));

                        f.push({ Value: code, SourceEntityType: type });
                    }
                    else {
                        type = $item.data("type");
                        code = (type == "SN") ? unescape($item.attr("code")) : $item.attr("code");
                        //desc = (type == "SN") ? code : $item.text();

                        f.push({ Value: code, SourceEntityType: type });
                    }
                    filters.push({ SourceEntites: f });
                });
            }
            else {
                if (this._sLstfilters && this._sLstfilters.include) {
                    var filtersArr, item;
                    $.each(this._sLstfilters.include, function () {
                        item = []; filtersArr = this; //This is an array
                        $.each(filtersArr, function () {
                            item.push({ SourceEntityType: (this.type || "SC"), Value: this.code });
                        });
                        filters.push({ SourceEntites: item });
                    });
                }
            }

            var queryParams;

            if (addToExistingList) {
                queryParams = {
                    Id: listId,
                    SourceEntityFilters: filters
                }
            }
            else {
                queryParams = {
                    SourceListQuery: {
                        AccessControlScope: "Account",
                        Name: (isEditing ? this.$existingSLstName.val() : this.$newSLstName.val()),
                        SourceEntityFilters: filters
                    }
                }
                if (isEditing) {
                    queryParams.SourceListQuery.Id = listId;
                }
            }

            $dj.progressIndicator.display(this._loadingText);

            $.ajax({
                url: this.options.queriesServiceUrl + (addToExistingList ? "/SourceEntity/Filter/Json?" : "/SourceEntity/Json?"),
                type: listId ? 'PUT' : 'POST',
                data: queryParams,
                success: function (data) {
                    $dj.progressIndicator.hide();
                    $().overlay.hide("#" + modalId);
                    alert((isEditing || addToExistingList) ? "<%= Token('sourceListSavedSuccessfully') %>" : "<%= Token('sourceListCreatedSuccessfully') %>");
                    me.$existingSLstDD.children(":gt(0)").remove();
                    me._sourceList = null;
                    me.$savedList.data("loaded", false);
                    var $savedListTab = me.$modalNav.children('li.active');
                    if ($savedListTab.index() == 2) {//If active tab is saved list then get the fresh list
                        $savedListTab.click();
                    }
                },
                error: function (error) {
                    $dj.progressIndicator.hide();
                    alert($dj.formatError(error.Error || error));
                }
            });
        },

        _deleteSourceList: function () {
            var listId = this._sourceListId || this.$existingSLstDD.val();

            if (!listId) {//
                alert("<%= Token('selectTheSourceListToDelete') %>");
                return;
            }

            $dj.progressIndicator.display(this._loadingText);

            $.ajax({
                url: this.options.queriesServiceUrl + "/SourceEntity/id/json?id=" + listId,
                type: 'DELETE',
                success: $dj.delegate(this, function (data) {
                    $dj.progressIndicator.hide();

                    //Remove the source list from the drop down
                    this.$existingSLstDD.find("option[value=" + listId + "]").remove();
                    this.$existingSLstDD.change();
                    if (this.$existingSLstDD.children().length == 1) {//No source list
                        this._sourceList = {}; //empty object
                    }
                    //Remove the source list from the lsit
                    if (this.$savedList) {
                        if (this.$savedList.children('li').length > 0) {
                            this.$savedList.find("> li > div > div[data-code=" + listId + "]:first").closest("li").remove();
                        }
                        if (this.$savedList.children('li').length == 0) {
                            this.$savedList.html("<%= Token('noResults') %>");
                        }
                    }

                    if (this._sourceListId) {//Deleting source list while editing
                        $().overlay.hide("#" + this.$saveSLstModal.attr('id'));
                    }

                    alert("<%= Token('sourceListDeletedSuccessfully') %>");

                    this._onSourceListDelete(listId);
                    this.publish(this.events.onSrcLstDelete, listId);
                }),
                error: function (error) {
                    $dj.progressIndicator.hide();
                    alert($dj.formatError(error.Error || error));
                }
            });

        },

        _onSourceListDelete: function (listId) {
            if (this._sourceListAdded) {//Remove the source list if it is added in the list
                this._onFilterClose(this.$filters.find("li[code='" + listId + "']").find(this.selectors.filterClose).get(0));
            }
        },

        _initializeSourceListModal: function (sLstId) {

            if (!this.$saveSLstModal) {
                var id = this.$element.attr("id") + "_SaveSourceList", me = this;
                $("<div id='" + id + "' />").hide().appendTo(this.$element);
                this.$saveSLstModal = $("#" + id);
                this.$saveSLstModal.html(this.templates.saveSourceList());

                //Modal close
                this.$saveSLstModal.find(this.selectors.modalClose).click(function () {
                    $().overlay.hide("#" + id);
                });

                var $fieldSet = this.$saveSLstModal.find("fieldset:first");

                //Form Fields
                this.$sLstFormFields = $fieldSet.find(this.selectors.formFields);

                this.$sLstRadioBtns = this.$sLstFormFields.find("input:radio");

                //Set radio button checked/unchecked
                $fieldSet.delegate("ul", "click", function () {
                    $(this).find("input:radio").attr("checked", true);
                });

                this.$existingSLstDD = $fieldSet.find("select:first");
                this.$existingSLstName = this.$sLstFormFields.eq(0).find("input:text:first");
                this.$newSLstName = this.$sLstFormFields.eq(1).find("input:text:first");

                this.$existingSLstDD.selectbox();

                //Delete Source List
                $fieldSet.find(this.selectors.deleteLnk).click(function () {
                    $dj.confirmDialog({
                        yesClickHandler: function () {
                            me._deleteSourceList();
                        },
                        title: "<%= Token('deleteSourceList') %>",
                        msg: "<%= Token('deleteSourceListConfirmMsg') %>"
                    });
                });

                //Filters
                this.$sLstFiltersContainer = this.$saveSLstModal.find(this.selectors.filtersList).hide();
                this.$sLstFilters = this.$sLstFiltersContainer.find(this.selectors.filters);

                this.$sLstFilters.delegate(this.selectors.filterClose, 'click', function () {
                    $(this).parent().remove();
                    me._triggerResize();
                });

                var $footer = this.$sLstFiltersContainer.next();
                $footer.find(this.selectors.cancelBtn).click(function () { $().overlay.hide("#" + id); });
                $footer.find(this.selectors.doneBtn).click(function () { me._saveSourceList(); });
            }

            this._sourceListId = sLstId;

            if (sLstId) {//Edit
                this.$sLstRadioBtns.addClass("hidden");
                this.$existingSLstDD.parent().addClass("hidden");
                this.$existingSLstName.removeClass("hidden");
                this.$sLstFormFields.eq(1).addClass("hidden");
                this.$sLstFormFields.eq(0).find("label").html("<%= Token('listName') %>");
            }
            else {
                this.$sLstRadioBtns.removeClass("hidden");
                this.$existingSLstDD.parent().removeClass("hidden");
                this.$existingSLstName.addClass("hidden");
                this.$sLstFormFields.eq(1).removeClass("hidden");
                this.$sLstFormFields.eq(0).find("label").html("<%= Token('addToList') %>");
                this.$sLstRadioBtns.eq(1).attr("checked", true);
                this.$newSLstName.val('');
                this.$sLstFiltersContainer.hide();
            };

        },

        _onListItemClick: function (elem, e, sourceList) {
            var $item = $(elem), code = $item.data("code");
            if (!this._isFilterPresent(code)) {
                this._addFilter({ code: code, desc: $.trim($item.text()) });
            }
            this._stopPropagation(e, true);
        },

        _onSourceItemClick: function (elem, e) {
            var $item = $(elem), code = $item.data("code"), code1;
            code1 = $item.data("code1") || '';
            if (!this._isFilterPresent(code + code1)) {
                var type = $item.data("type"), desc = $.trim((type == "LIST") ? $item.find("span").text() : $item.text());
                var filter = [{ code: code, desc: ((type != "SN" && code1) ? ($item.data("desc") || desc) : null), type: type, cdesc: desc}];
                if (code1) {//If multiple codes
                    filter.push({ code: code1, desc: ((type != "BY") ? $item.data("desc1") : null), type: $item.data("type1") });
                }

                if (type == "LIST") {//If Source list is added then clear all other filters
                    this.clearFilters();
                    this._sourceListAdded = true;
                    this._addSourceFilter(filter);
                }
                else {
                    if (this._sourceListAdded) {
                        this.clearFilters();
                    }
                    this._addSourceFilter(filter);
                }
            }
            this._stopPropagation(e, true);
        },

        _onSourceGroupItemClick: function (elem, e) {
            var $item = $(elem), code = $item.data("code"), code1 = this.$sourceGroupDD.data("value"), sourceCat = this.$sourceSortByDD.data("value");
            code1 = (sourceCat && code1) ? code1 : ''; //If source category is selected then consider it
            if (!this._isFilterPresent(code1 + code)) {
                var filter = [];
                if (code1 && sourceCat) {//PDF and filter selected
                    var desc = this.$sourceGroupDD.children("span:first").html(), desc1 = $.trim($item.text());
                    filter.push({ code: code1, desc: escape(desc), type: "PDF", cdesc: desc + ": " + desc1 });
                    filter.push({ code: code, desc: escape(desc1), type: "RST" });
                }
                else {
                    filter.push({ code: code, desc: $.trim($item.text()), type: sourceCat ? "RST" : "PDF" });
                }
                if (this._sourceListAdded) {
                    this.clearFilters();
                }
                this._addSourceFilter(filter);
            }
            this._stopPropagation(e, true);
        },

        _onListItemHover: function (elem, e) {
            if (!this.$listItemOptions || !this.$listItemOptions.html() || (this.$listItemOptions.children().length == 0)) {
                this._setListItemOptions();
            }
            $(elem).append(this.$listItemOptions.show());
            this._stopPropagation(e, true);
        },

        _onSourceListItemHover: function (elem, e) {
            var $elem = $(elem), type = $elem.data("type");
            if (type != "LIST" && type != "SN" && type != "PDF") {
                $elem.append(this.$listItemOptions.show());
            }
            this._stopPropagation(e, true);
        },

        _onListItemOptionsClick: function (elem, e) {
            var $option = $(elem), $item = $option.parent().parent(), code = $item.data("code"), gType = ($item.data("gtype") || null);
            ($option.index() == 0) ? this._showEntityInfo(code, gType) :
                                    ((!this._isFilterPresent(code)) ? this._addFilter({ code: code, desc: $.trim($item.text()) }, true) : null);
            this._stopPropagation(e, true);
        },

        _showEntityInfo: function (code, isGroup) {
            //Make an ajax call and get the option
            $dj.progressIndicator.display(this._loadingText);
            var queryParams = {
                language: this.options.interfaceLanguage,
                codes: code,
                parts: this._lookUpDetails.infoParts
            };
            var url = this.options.taxonomyServiceUrl + "/" + this._lookUpDetails.name;
            if (isGroup) {
                url += "/group";
            }

            $.ajax({
                url: url + "/fcode/json?",
                type: 'GET',
                data: queryParams,
                success: $dj.delegate(this, function (data) {
                    $dj.progressIndicator.hide();
                    if (data && data[this._lookUpDetails.restField] && data[this._lookUpDetails.restField][0]) {
                        $dj.progressIndicator.hide();
                        //Set the modal content
                        this.$djiiCodeDetailsModal
                        .html(this.templates.djIICode({
                            data: this._transformEntityInfoResponse(data[this._lookUpDetails.restField][0]),
                            title: this._lookUpDetails.label,
                            isSource: (this.options.filterType == this.filterType.Source)
                        }))
                        .overlay({ closeOnEsc: true })
                        .find(this.selectors.modalContent).scrollTop(0);//Reset the scroll position

                        this._hideAutoCompleteResults();
                    }
                    else {
                        //ToDO: proper message here
                        alert("<%= Token('pleaseTryAgainLater') %>");
                    }
                }),
                error: $dj.delegate(this, function (error) {
                    $dj.progressIndicator.hide();
                    alert($dj.formatError(error.Error || error));
                    this._hideAutoCompleteResults();
                })
            });

            if (!this.$djiiCodeDetailsModal) {
                var id = this.$element.attr("id") + "_djiicode", me = this;
                $("<div id='" + id + "' />").hide().appendTo(this.$element);
                this.$djiiCodeDetailsModal = $("#" + id);
                this.$djiiCodeDetailsModal.delegate(this.selectors.modalClose + "," + this.selectors.doneBtn, 'click', function () {
                    $().overlay.hide("#" + id);
                });
            }
        },

        _transformEntityInfoResponse: function (entityInfo) {
            var info = {
                code: entityInfo.Code,
                name: entityInfo.Name
            };

            //Location
            if (entityInfo.PrimaryRegionCode) {
                info.loacation = entityInfo.PrimaryRegionCode.Descriptor;
            }
            //Primary Industry
            if (entityInfo.PrimaryIndustryCode) {
                info.primaryIndustry = entityInfo.PrimaryIndustryCode.Descriptor;
            }
            //Description
            if (entityInfo.Description && entityInfo.Description.length > 0 &&
                entityInfo.Description[0].Items && entityInfo.Description[0].Items.length > 0) {
                info.description = entityInfo.Description[0].Items[0].Value;
            }
            //Parent Codes
            if (entityInfo.ParentCodes && entityInfo.ParentCodes.length > 0) {
                info.parentCodes = [];
                var desc;
                $.each(entityInfo.ParentCodes, function () {
                    desc = this.Descriptor || this.Description;
                    if (desc) {
                        info.parentCodes.push(desc);
                    }
                });
            }
            //Primary Source/Outlet
            if (entityInfo.PrimarySource) {
                info.primarySource = entityInfo.PrimarySource.Description;
            }

            if (this.options.filterType == this.filterType.Source) {
                var dt;
                //Create language dict
                if (!this._langDict) {
                    var langDict = {};
                    $.each(this.data.languageList, function () {
                        langDict[this.code] = this.desc;
                    });
                    this._langDict = langDict;
                }

                //Language
                if (entityInfo.Language) {
                    info.language = this._langDict[entityInfo.Language.toLowerCase()];
                }
                //Most Recent Issue
                if (entityInfo.MostRecentIssue) {
                    dt = JSON.parseDate(entityInfo.MostRecentIssue);
                    if (dt) {
                        info.mostRecentIssue = dt.format("standardDate", true, this.options.interfaceLanguage);
                    }
                }
                //First Issue
                if (entityInfo.FirstIssue) {
                    dt = JSON.parseDate(entityInfo.FirstIssue);
                    if (dt) {
                        info.firstIssue = dt.format("standardDate", true, this.options.interfaceLanguage);
                    }
                }
                //Frequency
                if (entityInfo.Frequency) {
                    info.frequency = entityInfo.Frequency.Description;
                }
                //Update Schedule
                info.updateSchedule = entityInfo.UpdateSchedule;
                //Online Availability
                if (entityInfo.OnlineAvailability) {
                    dt = JSON.parseDate(entityInfo.OnlineAvailability);
                    if (dt) {
                        info.onlineAvailability = dt.format("standardDate", true, this.options.interfaceLanguage);
                    }
                }
                //Source Coverage
                info.sourceCoverage = entityInfo.SourceCoverage;
                //Article Coverage
                info.articleCoverage = entityInfo.ArticleCoverage;
                //Format
                if (entityInfo.PublicationFormats && entityInfo.PublicationFormats.length > 0) {
                    //ToDo- Get the string value fron token
                    info.format = entityInfo.PublicationFormats[0];
                }
                //Publisher
                info.publisher = (entityInfo.PublisherName || entityInfo.PublisherCode);
                //Circulation
                info.circulation = entityInfo.Circulation;
                //Publisher Url
                info.publisherWebAddress = entityInfo.PublisherWebAddress;
                //Web Address
                info.webAddress = entityInfo.PublicationWebAddress;
                //Editors Notes
                if (entityInfo.Notes && entityInfo.Notes.length > 0 &&
                    entityInfo.Notes[0].Items && entityInfo.Notes[0].Items.length > 0) {
                    info.notes = entityInfo.Notes[0].Items[0].Value;
                }
                //Source Logo
                info.sourceLogo = entityInfo.SourceLogo;
            }

            return info;
        },

        _onBrowseMoreClick: function (elem, e, options) {
            options = options || {};
            var $item = $(elem);
            var $parent = $item.parent();
            var $div = $parent.next();
            if ($div.length == 0) {
                $div = $("<ul />").hide();
                $parent.after($div);
            }

            if ($div.is(":visible")) {
                $div.hide();
                $parent.parent().removeClass("expanded");
            }
            else {
                $div.show();
                $parent.parent().addClass("expanded");
                if (!$div.data("loaded")) {
                    var me = this, $itemNext = $item.next(), code = $itemNext.data("code");
                    if (options.sourceListBrowse) {
                        $div.html(this._loadingText).slideDown(100, this._delegates.OnResize);
                        this._getSourceListById(code, {
                            success: function (data) {
                                me.bindList($div, data.SourceListQuery.SourceEntityFilters, { sourceListBrowse: true });
                            },
                            error: function (error) {
                                $div.html($dj.formatError(error.Error || error)).scrollTop(0);
                            }
                        });
                    }
                    else if (options.sourceGroupBrowse) {
                        this._getSourceByGroupCode({
                            childrenType: $itemNext.data("ctype"),
                            container: $div,
                            code: code,
                            sourceCategory: this.$sourceSortByDD.data("value"),
                            groupCode: this.$sourceGroupDD.data("value")
                        });
                    }
                    else {
                        this._getBrowseList(code, $div, true);
                    }
                }
            }
            this._triggerResize();
            this._stopPropagation(e, true);
        },

        _onFilterScroll: function (scrollUp) {
            if (this.$filters.children().length > 0 || this.$notfilters.children(":gt(0)").length > 0) {
                var $wrap = this.$filters.parent(), wrapTop = parseInt($wrap.css('top'), 10);
                if (!this._pillHeight) {
                    this._pillHeight = $wrap.find("li:first").outerHeight(true);
                }
                var up = (scrollUp && (wrapTop < 0)),
                    down = (!scrollUp && (($wrap.height() + wrapTop - parseInt($wrap.parent().css("max-height"), 10)) >= this._pillHeight));
                if (!$wrap.is(':animated') && (up || down)) {
                    $wrap.animate({ top: ((up ? '+' : '-') + '=' + this._pillHeight) }, 'fast', $dj.delegate(this, this.updateFilterScroll));
                }
            }
        },

        updateFilterScroll: function () {
            var $wrap = this.$filters.parent(), wrapHeight = $wrap.height(), wrapTop = parseInt($wrap.css('top'), 10);

            if (!this._filterItemsMaxHeight) {
                this._filterItemsMaxHeight = parseInt($wrap.parent().css("max-height"), 10);
            }
            if (!this._pillHeight && $wrap.find("li").length > 1) {
                this._pillHeight = $wrap.find("li:not(" + this.selectors.notFilterPill + ")").outerHeight(true);
            }
            this.$filterScroll.removeClass('at-scroll-top at-scroll-bottom hidden');
            if (wrapHeight >= this._filterItemsMaxHeight) {
                if (wrapHeight <= this.$filtersContainer.height()) {
                    this.$filterScroll.addClass('at-scroll-top at-scroll-bottom');
                } else if ((wrapHeight + wrapTop - this._filterItemsMaxHeight) < this._pillHeight) {
                    this.$filterScroll.addClass('at-scroll-bottom');
                } else if (wrapTop >= 0) {
                    this.$filterScroll.addClass('at-scroll-top');
                }
            } else {
                this.$filterScroll.addClass('hidden');
            }
        },

        _showHideSaveListBtn: function (show) {
            //If no filters then hide the saveListBtn
            if (this.$saveListBtn) {
                if (show && !this._sourceListAdded) {
                    this.$saveListBtn.removeClass("hidden");
                }
                else if ((show == false) || this._sourceListAdded) {
                    this.$saveListBtn.addClass("hidden");
                }
                else {
                    if (this.$filters.children().length > 0 || (this._lookUpDetails.notFilter && (this.$notFilters.children(":gt(0)").length > 0))) {
                        this.$saveListBtn.removeClass("hidden");
                    }
                    else {
                        this.$saveListBtn.addClass("hidden");
                    }
                }
            }
        },

        bindFilters: function (filters) {
            //Clean the existing list
            this.$filters.children().remove();
            this.$notfilters.hide().children(":gt(0)").remove();
            this._sourceListAdded = false;
            if (filters && ((filters.include && filters.include.length > 0) || (filters.exclude && filters.exclude.length > 0) || filters.list)) {
                var me = this;

                if (this.options.filterType == this.filterType.Source) {
                    if (filters.list) {
                        filters.list.type = "LIST";
                        this._sourceListAdded = true;
                        this.$filters.append(this.templates.sourceFilterPill({ filter: filters.list }));
                    }
                    else {
                        var f, item, type, code, eCode;
                        $.each(filters.include, function () {
                            item = this[0];
                            type = item.type;
                            code = item.code;
                            eCode = (type == "SN") ? escape(code) : '';
                            f = [{
                                code: eCode || code.toLowerCase(),
                                desc: eCode || escape(item.desc),
                                type: type,
                                cdesc: item.desc
                            }];

                            if (this.length > 1) {//If multiple filters
                                item = this[1];
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

                            me.$filters.append(me.templates.sourceFilterPill({ filter: f }));
                        });
                    }
                }
                else {

                    if (filters.include && filters.include.length > 0) {
                        $.each(filters.include, function () {
                            this.code = this.code.toLowerCase();
                            me.$filters.append(me.templates.filterPill({ filter: this }));
                        });
                    }

                    if (filters.exclude && filters.exclude.length > 0) {
                        $.each(filters.exclude, function () {
                            this.code = this.code.toLowerCase();
                            me.$notfilters.append(me.templates.filterPill({ filter: this }));
                        });
                    }
                }

                this._showHideNotPillList();
                this.updateFilterScroll();
                this._triggerResize();
                this._showHideSaveListBtn(true);
                if (this.options.filterType == this.filterType.Language) {
                    if ((filters.include.length == 1) && (filters.include[0].code == "alllang")) {//All languages
                        //Remove filter close span from all languages
                        this.$filters.children().find(this.selectors.filterClose).remove();
                    }
                }
            }

            if (this.options.filterType == this.filterType.Source) {
                this._showHideSaveListBtn();
            }

            //Set the filters wrapper top to 0px. If it was scrolled the top might have negative value
            this.$filters.parent().css("top", "0px");
        },

        _initializeAutoSuggest: function () {
            try {
                if (this.options.suggestServiceUrl) {
                    var autoSuggestObj = {
                        url: this.options.suggestServiceUrl,
                        controlId: this.$element.attr("id") + "_textBox",
                        controlClassName: "djAutoCompleteSource",
                        resultsClass: "dj_emg_autosuggest_results",
                        resultsOddClass: "dj_emg_autosuggest_odd",
                        resultsEvenClass: "dj_emg_autosuggest_even",
                        resultsOverClass: "dj_emg_autosuggest_over",
                        autocompletionType: this.getFilterType(),
                        selectFirst: false,
                        options: this._getAutoCompleteSearchOptions(),
                        useSessionId: DJ.config.credentials.sessionId,
                        onItemSelect: this._delegates.OnAutoSuggestItemSelect,
                        tokens: {
                            infoTitleTkn: "<%= Token('infoTitleTknPre') %>|true|<%= Token('infoTitleTknPost') %>",
                            promoteTitleTkn: "<%= Token('promoteTitleTkn') %>",
                            notTitleTkn: "<%= Token('notTitleTkn') %>",
                            publicationTkn: "<%= Token('publication') %>",
                            webpageTkn: "<%= Token('webSite') %>",
                            multimediaTkn: "<%= Token('multimedia') %>",
                            pictureTkn: "<%= Token('picture') %>",
                            disContTkn: "<%= Token('discontSrcTitleTkn') %>"
                        }
                    }

                    var me = this;
                    autoSuggestObj.onInfoClick = this._delegates.OnAutoSuggestInfoClick;
                    if (this._lookUpDetails.notFilter) {
                        autoSuggestObj.onNotClick = this._delegates.OnAutoSuggestNotClick;
                    }

                    if (np && np.web && np.web.widgets && np.web.widgets.autocomplete) {
                        np.web.widgets.autocomplete(autoSuggestObj);
                    }
                }
            }
            catch (e) {
                $dj.debug(e);
            }
        },

        _getAutoCompleteSearchOptions: function () {
            switch (this.options.filterType) {
                case this.filterType.Company: return { maxResults: '10', dataSet: 'newsCodedAbt', filterADR: false };
                case this.filterType.Source: return { maxResults: '10', types: '1|2|3|4', statuses: 'active|discont' };
                case this.filterType.Author: return { maxResults: '10', includeCommunicatorRecords: false };
                case this.filterType.Executive: return { maxResults: '10', filterNewsCoded: true };
                case this.filterType.Subject:
                case this.filterType.Industry:
                case this.filterType.Region:
                    return { maxResults: '10', status: 'active', interfaceLanguage: (this.options.interfaceLanguage || 'en') };
            }
            return { maxResults: '10' };
        },

        _onSearchClick: function () {

            if (this.itemSelect) {
                this.itemSelect = false;
                return;
            }

            if ($.trim(this.$textBox.val()) == "" || this.$textBox.val() == this._lookUpDetails.autoCompleteText) {
                if (this.$lookUpListContainer.is(":visible")) {
                    this.$lookUpListContainer.slideUp(100, this._delegates.OnResize);
                }
                alert("<%= Token('enterSearchCriteria') %>");
            }
            else {

                if ($.trim(this.$textBox.val().length) > 3) {
                    this._lookUpOffset = 0;
                    this.$lookUpList.data("loaded", false);
                    this.$lookUpListContainer.slideDown(100, this._delegates.OnResize);
                    this._getLookUpList(this.$textBox.val(), this.$lookUpList.eq(0));
                    //For Extended Search
                    if (this.options.filterType == this.filterType.Source) {
                        this._getLookUpList(this.$textBox.val(), this.$lookUpList.eq(1), 0, true);
                    }
                }
                else {
                    //ToDo: Should we alert the user to enter more than 3 characters atleast to run the search?
                }
            }
        },

        _onFilterClick: function (elem) {
            var $elem = $(elem), $li = $elem.closest(this.selectors.filterPill), elemOffset = $elem.offset();
            this.$filterOptions.children("div").children().show().filter(":eq(" + ($elem.closest("ul").hasClass("not-filter") ? "1" : "2") + ")").hide();
            //$li.append(this.$filterOptions.show());
            this.$filterOptions.css({
                top: (elemOffset.top + $elem.outerHeight() - 4 - $(document).scrollTop()),
                left: (elemOffset.left + 8),
                position: "fixed"
            }).data("li", $elem).show();
            $(document).unbind('mousedown.scl').bind('mousedown.scl').click($dj.delegate(this, function () {
                this._onMouseDown();
                $(document).unbind('mousedown.scl');
            }));
        },

        _onAutoSuggestItemSelect: function (item) {
            this.itemSelect = true;
            if (item) {
                item = this._createFilterObject(item);
                if (this.options.filterType == this.filterType.Source) {
                    item.type = "RST";
                    if (this._sourceListAdded) {
                        this.clearFilters();
                        this._addSourceFilter(item);
                    }
                    else {
                        if (!this._isFilterPresent(item.code)) {
                            this._addSourceFilter(item);
                        }
                    }
                }
                else {
                    if (!this._isFilterPresent(item.code)) {
                        this._addFilter(item);
                    }
                }
            }
            setTimeout($dj.delegate(this, function () { this.itemSelect = false; }), 200); //Reset itemSelect to false
            this.$textBox.val("");
        },

        _onAutoSuggestInfoClick: function (item) {
            if (item) {
                this._showEntityInfo((this.options.filterType == this.filterType.Author) ? item.nnId : item.code);
            }
        },

        _onAutoSuggestNotClick: function (item) {
            if (item) {
                item = this._createFilterObject(item);
                if (!this._isFilterPresent(item.code)) {
                    this._addFilter(item, true);
                }
            }
        },

        _hideAutoCompleteResults: function () {
            if (!this.$autoCompleteContainer) {
                this.$autoCompleteContainer = $(".dj_emg_autosuggest_results:last");
            }
            this.$autoCompleteContainer.hide();
        },

        _isFilterPresent: function (itemCode) {
            return ((this.$notfilters.children("li[code='" + itemCode + "']").length > 0)
                    || (this.$filters.children("li[code='" + itemCode + "']").length > 0));
        },

        _addFilter: function (filter, not) {
            if (filter) {
                if (not) {
                    this.$notfilters.append(filter.jquery ? filter : this.templates.filterPill({ filter: filter }));
                }
                else {
                    if (this.options.filterType == this.filterType.Source) {
                        this.$filters.append(filter.jquery ? filter : this.templates.sourceFilterPill({ filter: filter }));
                    }
                    else {
                        this.$filters.append(filter.jquery ? filter : this.templates.filterPill({ filter: filter }));
                    }
                }
                this._showHideNotPillList();
                this.updateFilterScroll();
                this._showHideSaveListBtn(true);
                this._triggerResize();
            }
        },

        _addSourceFilter: function (filter) {
            if (filter) {
                this.$filters.append(this.templates.sourceFilterPill({ filter: filter }));
                this.updateFilterScroll();
                this._showHideSaveListBtn(true);
                this._triggerResize();
            }
        },

        _createFilterObject: function (item) {
            if (this.options.filterType == this.filterType.Author) {//For Author we have to pick nnId field from autocomplete response
                return { code: item.nnId, desc: item.formalName };
            }
            else {
                return { code: item.code.toLowerCase(), desc: (item.value || item.descriptor || item.completeName || item.formalName || item.directoryName) };
            }
        },

        getFilters: function () {
            var f = {}, $item, filter;

            if (this.options.filterType == this.filterType.Source) {
                if (this._sourceListAdded) {
                    $item = this.$filters.children().eq(0);
                    f.list = { code: $item.attr("code"), desc: $.trim($item.text()) };
                }
                else {
                    f.include = [];
                    var type, code, desc;
                    //Include filters
                    $.each(this.$filters.children(), function () {
                        filter = [];
                        $item = $(this);
                        if ($item.data("code1")) {

                            type = $item.data("type");
                            code = (type == "SN") ? unescape($item.data("code")) : $item.data("code");
                            desc = (type == "SN") ? code : unescape($item.data("desc"));

                            filter.push({ code: code, desc: desc, type: type });

                            type = $item.data("type1");
                            code = (type == "BY") ? unescape($item.data("code1")) : $item.data("code1");
                            desc = (type == "BY") ? code : unescape($item.data("desc1"));

                            filter.push({ code: code, desc: desc, type: type });
                        }
                        else {
                            type = $item.data("type");
                            code = (type == "SN") ? unescape($item.attr("code")) : $item.attr("code");
                            desc = (type == "SN") ? code : $item.text();

                            filter.push({ code: code, desc: desc, type: type });
                        }
                        f.include.push(filter);
                    });
                }
            }
            else {
                f = { include: [], exclude: [] };
                //Include filters
                $.each(this.$filters.children(), function () {
                    $this = $(this);
                    f.include.push({ code: $this.attr('code'), desc: $this.find('span:first').text() });
                });

                //Exclude filters
                if (this._lookUpDetails.notFilter) {
                    $.each(this.$notfilters.children(":gt(0)"), function () {
                        $this = $(this);
                        f.exclude.push({ code: $this.attr('code'), desc: $this.find('span:first').text(), operator: 1 });
                    });
                }
            }

            return f;
        },

        clearFilters: function () {
            this.$filters.children().remove();
            this.$notfilters.hide().children(":gt(0)").remove();
            this._showHideSaveListBtn(false);
            if (this.options.filterType == this.filterType.Language) {
                this._addAllLanguages();
            }
            this._sourceListAdded = false;
            //Set the filters wrapper top to 0px. If it was scrolled the top might have negative value
            this.$filters.parent().css("top", "0px");
            this._triggerResize();
        },

        _onFilterClose: function (elem) {
            if (elem) {
                var isNotFilter = $(elem).closest("ul").hasClass("not-filter");
                $(elem).closest(this.selectors.filterPill).remove();
                if (isNotFilter) {
                    this._showHideNotPillList();
                }
                if (this.options.filterType == this.filterType.Language) {
                    if (this.$filters.children().length == 0) {
                        //Add all languages
                        this._addAllLanguages();
                    }
                }
                this._sourceListAdded = false;
                this.updateFilterScroll();
                this._showHideSaveListBtn();
                this._triggerResize();
            }
        },

        _addAllLanguages: function () {
            this._addFilter($('<li code="alllang" class="dj_pill"><span class="filter"><%= Token("allLanguages") %></span></li>'));
        },

        _showHideNotPillList: function () {
            if (this.$notfilters.children(":gt(0)").length > 0) {
                this.$notfilters.show();
            }
            else {
                this.$notfilters.hide();
            }
        },

        _triggerResize: function () {
            this.publish(this.events.onResize);
        },

        _stopPropagation: function (e, hideMenu) {
            e.stopPropagation();
            if (hideMenu) {
                if (this.$filterOptions)
                    this.$filterOptions.hide();
            }
        },

        _getSourceByGroupCode: function (params) {

            params.container.removeClass("no-browse-margin").html(this._loadingText);

            var data = {
                language: this.options.interfaceLanguage,
                assetCode: this.options.productId,
                offset: 0,
                records: 1000,
                code: (params.code || "root"),
                childrenType: (params.childrenType || "source"),
                sourceCategory: params.sourceCategory
            };
            if (params.groupCode) {
                data.customSourceGroupCode = params.groupCode;
            }

            $.ajax({
                url: this.options.taxonomyServiceUrl + "/Source/pst/code/json?",
                type: 'GET',
                data: data,
                success: $dj.delegate(this, function (data) {
                    this.bindList(params.container, data.Sources, { sourceGroupBrowse: true });
                }),
                error: $dj.delegate(this, function (error) {
                    params.container.html($dj.formatError(error.Error || error)).scrollTop(0);
                })
            });
        },

        _getSourceByFirstCharacter: function (params) {

            params.container.addClass("no-browse-margin").html(this._loadingText);

            var data = {
                language: this.options.interfaceLanguage,
                assetCode: this.options.productId,
                offset: 0,
                records: 1000
            };
            if (params.groupCode) {
                data.customSourceGroupCode = params.groupCode;
            }
            if (params.searchString == "other") {
                data.searchOperator = "NonAlphaFirstCharacters";
            }
            else {
                data.searchString = params.searchString,
                data.searchOperator = "FirstCharacterOnly";
            }
            $.ajax({
                url: this.options.taxonomyServiceUrl + "/Source/pst/json?",
                type: 'GET',
                data: data,
                success: $dj.delegate(this, function (data) {
                    this.bindList(params.container, data.Sources, { sourceGroupBrowse: true });
                }),
                error: $dj.delegate(this, function (error) {
                    params.container.html($dj.formatError(error.Error || error)).scrollTop(0);
                })
            });
        },

        _getSourceList: function (callback) {
            if (this._sourceList) {
                callback.success(this._sourceList);
            }
            else {
                $.ajax({
                    url: this.options.queriesServiceUrl + "/SourceEntity/List/json",
                    data: { AccessControlScope: "Personal" },
                    type: 'GET',
                    success: callback.success,
                    error: callback.error
                });
            }
        },

        _getSourceListById: function (id, callback) {
            $.ajax({
                url: this.options.queriesServiceUrl + "/SourceEntity/id/json?",
                type: 'GET',
                data: {
                    id: id,
                    parts: "SourceInfo"
                },
                success: callback.success,
                error: callback.error
            });
        },

        _getLookUpList: function (keyword, listContainer, offset, extendedSearch) {

            if (!listContainer.data("loaded")) {
                listContainer.html(this._loadingText);
                this.$lookUpPagging.addClass('hidden');
            }
            else {
                $dj.progressIndicator.display(this._loadingText);
            }

            var queryParams = {
                language: this.options.interfaceLanguage,
                searchString: keyword,
                Offset: (offset || 0),
                Records: this._maxLookUpRecords
            };

            $.ajax({
                url: this.options.taxonomyServiceUrl + "/" + this._lookUpDetails.name + (extendedSearch ? "/Extendedsearch/json?" : "/json?"),
                type: 'GET',
                data: queryParams,
                success: $dj.delegate(this, function (data) {
                    listContainer.data("loaded", true);
                    this.bindList(listContainer, data[this._lookUpDetails.restField], { extSearch: extendedSearch, sourceLookUp: (!extendedSearch && this.options.filterType == this.filterType.Source) });
                    //Update pagging
                    if (listContainer.prev(this.selectors.pagging).length > 0) {//If it has pagging
                        this._updateLookUpPagging(data.TotalRecords);
                    }
                    $dj.progressIndicator.hide();
                    //This is just to hide the autocomplete dropdown
                    this._hideAutoCompleteResults();
                }),
                error: $dj.delegate(this, function (error) {
                    $dj.progressIndicator.hide();
                    listContainer.html($dj.formatError(error.Error || error)).scrollTop(0);
                    this.$lookUpPagging.addClass('hidden');
                    //This is just to hide the autocomplete dropdown
                    this._hideAutoCompleteResults();
                })
            });
        },

        _updateLookUpPagging: function (totalRecords) {
            if ((this._lookUpOffset > 0) || ((this._lookUpOffset + this._maxLookUpRecords) < totalRecords)) {

                if (this._lookUpOffset == 0) {
                    //Disable Prev
                    this.$lookUpPrevPage.addClass("dj_icon-arrow-disabled-left");
                    //Enable Next
                    this.$lookUpNextPage.removeClass("dj_icon-arrow-disabled-right");
                }
                else {
                    this.$lookUpPrevPage.removeClass("dj_icon-arrow-disabled-left");
                    if ((this._lookUpOffset + this._maxLookUpRecords) < totalRecords) {
                        this.$lookUpNextPage.removeClass("dj_icon-arrow-disabled-right");
                    }
                    else {
                        this.$lookUpNextPage.addClass("dj_icon-arrow-disabled-right");
                    }
                }

                var start = this._lookUpOffset + 1, end = (this._lookUpOffset + this._maxLookUpRecords);

                if (end > totalRecords)
                    end = totalRecords;

                this.$lookUpPrevPage.next().html([start, " - ", end, " <%= Token('of') %> ", totalRecords].join(''));

                //Show the pagging
                this.$lookUpPagging.removeClass('hidden');
            }
            else {
                this.$lookUpPagging.addClass('hidden');
            }
        },

        _getLookUpNextPage: function () {
            this._lookUpOffset = this._lookUpOffset + this._maxLookUpRecords;
            this._getLookUpList(this.$textBox.val(), this.$lookUpList.eq(0), this._lookUpOffset);
        },

        _getLookUpPrevPage: function () {
            this._lookUpOffset = this._lookUpOffset - this._maxLookUpRecords;
            this._getLookUpList(this.$textBox.val(), this.$lookUpList.eq(0), this._lookUpOffset);
        },

        _getBrowseList: function (code, listContainer, isChild) {

            listContainer.html(this._loadingText).slideDown(100, this._delegates.OnResize);

            var queryParams = {
                language: this.options.interfaceLanguage,
                codes: (code || "root"),
                parts: isChild ? "childcodes" : ""
            };

            $.ajax({
                url: this.options.taxonomyServiceUrl + "/" + this._lookUpDetails.name + "/fcode/json?",
                type: 'GET',
                data: queryParams,
                success: $dj.delegate(this, function (data) {
                    var data = data[this._lookUpDetails.restField];
                    this.bindList(listContainer, (isChild ? data[0].ChildCodes : data), { browse: true });
                }),
                error: $dj.delegate(this, function (error) {
                    listContainer.html($dj.formatError(error.Error || error)).scrollTop(0);
                })
            });
        },

        setActiveTab: function (tabIndex) {
            this.$modalNav.children().removeClass('active').eq(tabIndex).addClass("active");
            this.$tabContent.addClass('hidden').eq(tabIndex).removeClass('hidden');
            if (tabIndex == 0) {//LookUp
                if (this.options.filterType != this.filterType.Language) {
                    if (!this.$lookUpList.data("loaded")) {
                        this.$lookUpListContainer.hide();
                    }
                }
            }
            else if (tabIndex == 1) {//Browse
                if (!this.$browseList.data("loaded")) {//If Browse list is already loaded don't get it again
                    if (this.options.filterType == this.filterType.Source) {
                        this.$sourceGroupDD.change();
                    }
                    else {
                        this._getBrowseList("root", this.$browseList);
                    }
                }
            }
            else if (tabIndex == 2) {//Saved List
                if (this.options.filterType == this.filterType.Source) {
                    if (!this.$savedList.data("loaded")) {//If no source list get it
                        this.$savedList.html(this._loadingText).slideDown(100, this._delegates.OnResize);
                        this._getSourceList({
                            success: $dj.delegate(this, function (data) {
                                this._sourceList = data;
                                this.bindList(this.$savedList, data.SourceListQueries, { sourceList: true });
                            }),
                            error: $dj.delegate(this, function (error) {
                                this.$savedList.html($dj.formatError(error.Error || error)).scrollTop(0);
                            })
                        });
                    }
                }
            }
            this._triggerResize();
        },

        setData: function (data) {
            if (data)
                this.data = data;

            this.data = this.data || {};
            this.data.filters = this.data.filters || { include: [], exclude: [] };

            if (this.data) {
                this.bindFilters(this.data.filters);

                if ((this.options.filterType == this.filterType.Language) && this.data.languageList) {
                    this.bindList(this.$lookUpList, this.data.languageList);
                    this.$lookUpListContainer.show();
                }
            }

        },

        focusOnTextBox: function () {
            if (this.options.filterType != this.filterType.Language) {
                if (!this.$lookUpList.data("loaded")) {
                    this.$textBox.val('');
                }
                this.$textBox.focus();
            }
        },

        getFilterType: function () {
            return this._lookUpDetails.autoCompleteField || this._lookUpDetails.name;
        },

        bindList: function (listContainer, data, options) {
            options = options || {};
            if (data && data.length) {
                if (data.length == 1 && data[0].Error) {
                    listContainer.html($dj.formatError({ code: data[0].Code, message: data[0].Message }));
                }
                else {
                    if (options.browse) {
                        listContainer.html(this.templates.browseList({ data: data }));
                    }
                    else if (options.extSearch) {
                        listContainer.html(this.templates.extSourceList({ data: data }));
                    }
                    else if (options.sourceLookUp) {
                        listContainer.html(this.templates.sourceLookUpList({ data: data }));
                    }
                    else if (options.sourceList) {
                        listContainer.html(this.templates.sourceList({ data: data }));
                    }
                    else if (options.sourceListBrowse) {
                        listContainer.html(this.templates.sourceListBrowse({ data: data, sourceGroupItems: this._sourceGroupItems }));
                    }
                    else if (options.sourceGroupBrowse) {
                        listContainer.html(this.templates.sourceGroupBrowse({ data: data }));
                    }
                    else {
                        listContainer.html(this.templates.searchList({ data: data, isAuthor: (this.options.filterType == this.filterType.Author) }));
                    }
                    listContainer.data("loaded", true);
                }
            }
            else {
                listContainer.html("<%= Token('noResults') %>");
            }
            listContainer.scrollTop(0);
            this._triggerResize();
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_SearchCategoriesLookUp', DJ.UI.SearchCategoriesLookUp);

    $dj.debug('Registered DJ.UI.SearchCategoriesLookUp (extends DJ.UI.Component)');
