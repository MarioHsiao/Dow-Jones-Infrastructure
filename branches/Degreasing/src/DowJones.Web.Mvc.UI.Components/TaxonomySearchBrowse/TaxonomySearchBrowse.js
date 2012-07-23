/*
*  Search Categories Lookup Control
*/

    DJ.UI.TaxonomySearchBrowse = DJ.UI.Component.extend({

        selectors: {
            modalNav: 'ul.modal-nav',
            tabContent: '.tab-content',
            searchBox: 'div.dj_lookup-search',
            textBox: 'input.dj_lookup-search-field',
            searchBtn: 'input.dj_lookup-search-submit',
            lookUpList: '.lookup-results',
            lookUpListContainer: '.lookup-results-container',
            doneBtn: 'span.dj_btn-primary',
            cancelBtn: 'span.dj_btn-secondary',
            footer: '.footer',
            browseTree: '.dj_browse-tree',
            browseToggle: 'span.dj_browse-tree-toggle',
            listItem: 'li.item',
            browseItem: 'div.browse-item',
            pagging: '.dj_paging',
            prevPage: '.dj_icon-arrow-green-left',
            nextPage: '.dj_icon-arrow-green-right',
            listItemOptions: '.icon-group span',
            modalClose: 'p.dj_modal-close'
        },

        events: {
            onSearchClick: 'onSearchClick.dj.TaxonomySearchBrowse',
            onResize: 'onResize.dj.TaxonomySearchBrowse'
        },

        lookUpDetails: [
            {
                name: 'Industry',
                restField: 'Industries',
                lookUpTitle: "<%= Token('searchResults') %>",
                browse: true,
                browseText: "<%= Token('browseIndustry') %>",
                infoParts: 'Description|ParentCodes',
                detailTitle: "<%= Token('industryDetail') %>"
            },

            {
                name: 'Region',
                restField: 'Regions',
                lookUpTitle: "<%= Token('searchResults') %>",
                browse: true,
                browseText: "<%= Token('browseRegion') %>",
                infoParts: 'Description|ParentCodes',
                detailTitle: "<%= Token('regionDetail') %>"
            }
        ],

        taxonomyType: {
            Industry: 0,
            Region: 1
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "TaxonomySearchBrowse" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this.setData();
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnSearchClick: $dj.delegate(this, this._onSearchClick),
                OnResize: $dj.delegate(this, this._triggerResize)
            });
        },

        _setDefaultProperties: function () {

            this._loadingText = "<%= Token('loading') %>...";
            this._searchText = "<%= Token('searchText') %>";

            if (typeof (this.options.taxonomyType) == 'string')
                this.options.taxonomyType = this.taxonomyType[this.options.taxonomyType];

            this._lookUpEnabled = true;

            this._lookUpDetails = this.lookUpDetails[this.options.taxonomyType];
            this._setParentCodes();
            this._setListItemOptions();
            this._listItemHoverEnabled = true;

            this._maxLookUpRecords = 100;
            this._lookUpOffset = 0;
            this._selectedItem = {};
        },

        _setParentCodes: function () {
            if (this.options.parentCodes && $.isArray(this.options.parentCodes) && this.options.parentCodes.length > 0) {
                this._parentCodes = this.options.parentCodes.join("|");
            }
        },

        _setListItemOptions: function () {
            this.$listItemOptions = $(this.templates.listItemOptions());
        },

        _initializeControls: function () {

            this._setDefaultProperties();

            //Set the main template
            this._lookUpDetails.lookupText = this.options.lookupText;
            this.$element.append(this.templates.main({ lookUpDetails: this._lookUpDetails }));

            this.$elementChildren = this.$element.children();
            this.$tabContent = this.$elementChildren.filter(this.selectors.tabContent);

            this.$searchBox = this.$tabContent.eq(0).children(this.selectors.searchBox);
            this.$textBox = $(this.selectors.textBox, this.$searchBox);
            this.$searchBtn = $(this.selectors.searchBtn, this.$searchBox);

            this.$lookUpListContainer = this.$tabContent.eq(0).children(this.selectors.lookUpListContainer);
            this.$lookUpList = this.$lookUpListContainer.children(this.selectors.lookUpList);
            this.$browseList = this.$tabContent.eq(1).children(this.selectors.browseTree);
            this.$modalNav = this.$elementChildren.filter(this.selectors.modalNav);
            this.$footer = this.$elementChildren.filter(this.selectors.footer);

            //Pagging for lookup list
            this.$lookUpPagging = this.$lookUpListContainer.children(this.selectors.pagging).hide();
            this.$lookUpPrevPage = this.$lookUpPagging.children(this.selectors.prevPage);
            this.$lookUpNextPage = this.$lookUpPagging.children(this.selectors.nextPage);

            //Set the lookup results title
            this.$lookUpListContainer.children("h6:first").html(this._lookUpDetails.lookUpTitle);
        },

        _initializeEventHandlers: function () {

            this._initializeControls();

            var me = this;

            //List Item Click
            this.$lookUpList.delegate(this.selectors.listItem, 'click', function (e) {//List Item Click
                me._onListItemClick(this, e);
            });

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

            //Browse
            if (this._lookUpDetails.browse) {
                this.$modalNav.children(":eq(1)").show();
                this.$browseList.eq(0).delegate(this.selectors.listItemOptions, 'click', function (e) {
                    me._onListItemOptionsClick(this, e);
                });
                this.$browseList.eq(0).delegate(this.selectors.browseItem, 'mouseenter', function (e) {//List Item Hover
                    me._onListItemHover(this, e);
                }).delegate(this.selectors.browseItem, 'click', function (e) {
                    me._onListItemClick(this, e);
                }).delegate(this.selectors.browseToggle, 'click', function (e) {//Browse Icon click
                    me._onBrowseMoreClick(this, e);
                });
                this.$modalNav.delegate('li', 'click', function () {
                    me.setActiveTab($(this).index());
                });
            }
        },

        _onListItemClick: function (elem, e) {
            var $item = $(elem), code = $item.data("code");
            this._selectedItem = { code: code, desc: $.trim($item.text()), type: this.options.taxonomyType };
            //remove selection from previously selected element and select this
            this.$browseList.eq(0).find(this.selectors.browseItem).removeClass("selected");
            this.$lookUpList.eq(0).find(this.selectors.listItem).removeClass("selected");
            $item.addClass("selected");
            this._stopPropagation(e, true);
        },

        _onListItemHover: function (elem, e) {
            if (!this.$listItemOptions || !this.$listItemOptions.html() || (this.$listItemOptions.children().length == 0)) {
                this._setListItemOptions();
            }
            $(elem).append(this.$listItemOptions.show());
            this._stopPropagation(e, true);
        },

        _onListItemOptionsClick: function (elem, e) {
            var $option = $(elem), $item = $option.parent().parent(), code = $item.data("code"), gType = ($item.data("gtype") || null);
            this._showEntityInfo(code, gType);
            this._stopPropagation(e, true);
        },

        _showEntityInfo: function (code, isGroup) {
            //Make an ajax call and get the option
            $dj.progressIndicator.display(this._loadingText);
            var queryParams = {
                language: (DJ.config.preferences.interfaceLanguage || 'en'),
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
                        this.$djiiCodeDetailsModal
                        .html(this.templates.djIICode({
                            data: this._transformEntityInfoResponse(data[this._lookUpDetails.restField][0]),
                            title: this._lookUpDetails.detailTitle
                        }))
                        .overlay({ closeOnEsc: true });
                    }
                    else {
                        //ToDO: proper message here
                        alert("<%= Token('pleaseTryAgainLater') %>");
                    }
                }),
                error: $dj.delegate(this, function (error) {
                    $dj.progressIndicator.hide();
                    alert($dj.formatError(error.Error || error));
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
                info.location = entityInfo.PrimaryRegionCode.Descriptor;
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
            return info;
        },


        _bindTextBoxHandlers: function () {
            var me = this;
            this.$textBox.unbind('keypress').keypress(function (e) {
                if (e.which == 13) {//Enter pressed
                    me.$searchBtn.trigger('click');
                    //This is just to hide the autocomplete dropdown
                    //$(document).trigger("click");
                }
            })
            .attr("id", this.$element.attr("id") + "_textBox")
                                    .waterMark({
                                        waterMarkClass: 'watermark',
                                        waterMarkText: this._searchText
                                    });
            this.$searchBtn.unbind('click').click(this._delegates.OnSearchClick);

        },

        _onBrowseMoreClick: function (elem, e) {
            var $item = $(elem);
            var $parent = $item.parent();
            var $div = $parent.next();
            if ($div.length == 0) {
                $div = $("<div />").hide();
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
                    this._getBrowseList($item.next().data("code"), $div, true);
                }
            }

            this._stopPropagation(e, true);
        },

        _triggerResize: function () {
            this.publish(this.events.onResize);
        },

        _stopPropagation: function (e) {
            e.stopPropagation();
        },

        _onSearchClick: function () {

            if (this.itemSelect) {
                this.itemSelect = false;
                return;
            }

            if ($.trim(this.$textBox.val()) == "" || this.$textBox.val() == this._searchText) {
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
                }
                else {
                    //ToDo: Should we alert the user to enter more than 3 characters atleast to run the search?
                    alert(this._searchText);
                }
            }
        },

        _getLookUpList: function (keyword, listContainer, offset) {

            if (!listContainer.data("loaded")) {
                listContainer.html(this._loadingText);
                this.$lookUpPagging.addClass('hidden');
            }
            else {
                $dj.progressIndicator.display(this._loadingText);
            }

            var queryParams = {
                sessionId: DJ.config.credentials.sessionId,
                searchString: keyword,
                Offset: (offset || 0),
                Records: this._maxLookUpRecords,
                language: (DJ.config.preferences.interfaceLanguage || 'en')
            };

            if (this._parentCodes) {
                queryParams.restrictingparentcodes = this._parentCodes;
            }

            $.ajax({
                url: this.options.taxonomyServiceUrl + "/" + this._lookUpDetails.name + "/json?",
                type: 'GET',
                data: queryParams,
                success: $dj.delegate(this, function (data) {
                    listContainer.data("loaded", true);
                    this.bindList(listContainer, data[this._lookUpDetails.restField]);
                    //Update pagging
                    if (listContainer.prev(this.selectors.pagging).length > 0) {//If it has pagging
                        this._updateLookUpPagging(data.TotalRecords);
                    }
                    $dj.progressIndicator.hide();
                }),
                error: $dj.delegate(this, function (error) {
                    $dj.progressIndicator.hide();
                    listContainer.html($dj.formatError(error.Error || error)).scrollTop(0);
                    this.$lookUpPagging.addClass('hidden');
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
                sessionId: DJ.config.credentials.sessionId,
                codes: (code || "root"),
                parts: isChild ? "childcodes" : "",
                language: (DJ.config.preferences.interfaceLanguage || 'en')
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
                    listContainer.html($dj.formatError(error)).scrollTop(0);
                })
            });
        },

        getSelectedItem: function () {
            return (this._selectedItem || {});
        },

        focusOnTextBox: function () {
            this.$textBox.focus();
        },

        setActiveTab: function (tabIndex) {
            this.$modalNav.children().removeClass('active').eq(tabIndex).addClass("active");
            this.$tabContent.addClass('hidden').eq(tabIndex).removeClass('hidden');
            if (tabIndex == 0) {//LookUp
                if (!this.$lookUpList.data("loaded")) {
                    this.$lookUpListContainer.hide();
                }
            }
            else if (tabIndex == 1) {//Browse
                if (!this.$browseList.data("loaded")) {//If Browse list is already loaded don't get it again
                    this._getBrowseList(this._parentCodes, this.$browseList);
                }
            }
            this._triggerResize();
        },

        setData: function (data) {
            if (data)
                this.data = data;

            this.data = this.data || {};
        },

        reset: function (parentCodes, lookupText) {
            this.options.parentCodes = parentCodes;
            this._setParentCodes();
            this.$textBox.val(lookupText);
            this.$lookUpList.data("loaded", false);
            this.$lookUpList.html('');
            this.$browseList.data("loaded", false);
            this.$browseList.html('');
            this._selectedItem = {};
        },

        bindList: function (listContainer, data, options) {
            options = options || {};
            if (data && data.length) {
                if (data.length == 1 && data[0].Error) {
                    listContainer.html($dj.formatError({ code: data[0].Code, message: data[0].message }));
                }
                else {
                    if (options.browse) {
                        listContainer.html(this.templates.browseList({ data: data }));
                    }
                    else {
                        listContainer.html(this.templates.searchList({ data: data }));
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
    $.plugin('dj_TaxonomySearchBrowse', DJ.UI.TaxonomySearchBrowse);

    $dj.debug('Registered DJ.UI.TaxonomySearchBrowse (extends DJ.UI.Component)');
