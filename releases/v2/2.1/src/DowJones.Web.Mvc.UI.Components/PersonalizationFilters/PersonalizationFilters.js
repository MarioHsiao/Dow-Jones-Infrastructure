/*
*  Personalization Filters Control
*/
var fiiDescriptorsCache;
(function ($) {

    DJ.UI.PersonalizationFilters = DJ.UI.Component.extend({

        selectors: {
            lookUpsContainer: 'div.lookUpsContainer',
            taxonomySearchBrowse: 'div.dj_TaxonomySearchBrowse',
            editInputContainer: 'div.dj_edit-input-wrap',
            textBoxContainer: 'div.dj_text-element-wrapper',
            textBox: 'input.ac_input',
            labelContainer: 'label.dj_edit-label',
            labelName: 'span.dj_name',
            modalContent: 'div.modal-content',
            modalClose: 'p.dj_modal-close',
            footer: 'div.footer',
            doneBtn: 'span.dj_btn-primary',
            cancelBtn: 'span.dj_btn-secondary',
            filterPill: 'ul.dj_pill-list',
            removePill: 'span.remove'
        },

        filterLens: {
            None: 0,
            Industry: 1,
            Region: 2
        },

        filterLevel: {
            Page: 0,
            Module: 1
        },

        filterType: {
            Industry: 0,
            Region: 1,
            Keyword: 2
        },

        filterDetails: [
            {
                name: 'Industry',
                autoCompleteText: "<%= Token('industrySuggestText') %>",
                subAutoCompleteText: "<%= Token('subIndustrySuggestText') %>",
                title: "<%= Token('industryLabel') %>",
                subTitle: "<%= Token('subIndustry') %>",
                lookup: true,
                lookupTitle: "<%= Token('searchIndustry') %>"
            },

            {
                name: 'Region',
                autoCompleteText: "<%= Token('regionSuggestText') %>",
                subAutoCompleteText: "<%= Token('subRegionSuggestText') %>",
                title: "<%= Token('regionLabel') %>",
                subTitle: "<%= Token('subRegion') %>",
                lookup: true,
                lookupTitle: "<%= Token('searchRegion') %>"
            },

            {
                name: 'Keyword',
                autoCompleteText: "<%= Token('keywordSuggestText') %>",
                title: "<%= Token('keywords') %>",
                lookup: false
            }
        ],

        events: {
            filterSelectClick: 'filterSelectClick.dj.PersonalizationFilters'
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "PersonalizationFilters" }, meta);
            this._super(element, $meta);

            this.setData(this.data);
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnAutoSuggestItemSelect: $dj.delegate(this, this._onAutoSuggestItemSelect),
                OnAutoSuggestSearch: $dj.delegate(this, this._onAutoSuggestSearch)
            });
        },

        _initializeEventHandlers: function () {
            var headingToken = "<%= Token('module') %>";
            var filterInstructions = "<%= Token('moduleFilterInstructions') %>";

            if (this.options.filterLevel == this.filterLevel.Page) {
                headingToken = "<%= Token('page') %>";
                filterInstructions = "<%= Token('pageFilterInstructions') %>";
            }
            //set the disabled state
            this._disabled = this.options.disabled;
            //Set the main template
            this.$element.append(this.templates.main({ headingToken: headingToken, filterInstructions: filterInstructions }));
            this.$elementChildren = this.$element.children();
            this.$editInputContent = this.$elementChildren.filter(this.selectors.editInputContainer);
            //Industry Filter
            this.$industryInputContainer = this.$editInputContent.eq(0).children(this.selectors.textBoxContainer);
            this.$industryLabelContainer = this.$editInputContent.eq(0).children(this.selectors.labelContainer);
            this.$industryLabel = this.$industryLabelContainer.children(this.selectors.labelName);
            this.$industryInput = this.$industryInputContainer.children(this.selectors.textBox);
            this._industryInputId = this.$element.attr("id") + "_industry_textBox";
            this.$industryInput.attr("id", this._industryInputId);
            //Region Filter
            this.$regionInputContainer = this.$editInputContent.eq(1).children(this.selectors.textBoxContainer);
            this.$regionLabelContainer = this.$editInputContent.eq(1).children(this.selectors.labelContainer);
            this.$regionLabel = this.$regionLabelContainer.children(this.selectors.labelName);
            this.$regionInput = this.$regionInputContainer.children(this.selectors.textBox);
            this._regionInputId = this.$element.attr("id") + "_region_textBox";
            this.$regionInput.attr("id", this._regionInputId);
            //Keyword Filter
            this.$keywordInputContainer = this.$editInputContent.eq(2).children(this.selectors.textBoxContainer);
            this.$keywordLabelContainer = this.$editInputContent.eq(2).children(this.selectors.labelContainer);
            this.$keywordLabel = this.$keywordLabelContainer.children(this.selectors.labelName);
            this.$keywordInput = this.$keywordInputContainer.children(this.selectors.textBox);
            this._keywordInputId = this.$element.attr("id") + "_keyword_textBox";
            this.$keywordInput.attr("id", this._keywordInputId);
            //Initalize array of filters
            this.filterCollection = [];
            for (var i = 0; i < this.filterDetails.length; i++) 
            {         
                this.filterCollection.push({isSelected: false,selectedCode: "",selectedText: ""});
            } 
        },

        //begin functions for the industry/region lookup and browse
        _searchBrowse: function (filterType, title, lookupText) {
            var lookUpId = this.$element.attr("id") + "_" + filterType;
            var lookUp = $("#" + lookUpId);
            var scLC;
            if (!lookUp.length) {
                lookUp = this._getLookUpView(lookUpId);
                var data = {};

                //Initialize the Search Categories Look Up component
                scLC = $("#" + lookUpId).dj_TaxonomySearchBrowse({
                    options: { taxonomyType: filterType,
                        taxonomyServiceUrl: this.options.dataServiceUrl,
                        parentCodes: (filterType == this.filterType.Industry) ? this._getParentCodes(this.filterLens.Industry) : this._getParentCodes(this.filterLens.Region),
                        lookupText: lookupText
                    }, data: data
                });

                //On Resize
                $dj.subscribe(scLC.events.onResize, function () { $().overlay.rePosition(); });
            }
            else {
                //Find the component and set the data
                scLC = $("#" + lookUpId).findComponent(DJ.UI.TaxonomySearchBrowse);
                scLC.reset((filterType == this.filterType.Industry) ? this._getParentCodes(this.filterLens.Industry) : this._getParentCodes(this.filterLens.Region), lookupText);
                //Set the LookUp tab as active
                scLC.setActiveTab(0);
                //On Resize
                $dj.subscribe(scLC.events.onResize, function () { $().overlay.rePosition(); });
            }

            //Hide all the search loopups in the container
            this.$lookUpsContainer.children(":last").children().children(this.selectors.modalContent).children(this.selectors.taxonomySearchBrowse).hide();

            //Set the title
            this.$lookUpsContainer.children(':first').children("h3").html(title);

            //Show the current search lookup
            $("#" + lookUpId).show();

            this.$lookUpsContainer.data("lookUpId", lookUpId).data("filterType", filterType)
            .overlay({
                closeOnEsc: true,
                onShow: $dj.delegate(this, function () { this._onLookUpShow(filterType, scLC); })
            });
        },

        _onLookUpShow: function (filterType, scLC) {
            scLC.focusOnTextBox();
            //Hack - Only for IE7
            if ($.browser.msie && ($.browser.version == 7)) {
                this.$lookUpsContainer.find('div.tab-content').addClass('hidden');
                scLC.setActiveTab(0);
            }
        },

        _onLookUpDoneClick: function () {
            this._closeModal('$lookUpsContainer');
            var scLC = $("#" + this.$lookUpsContainer.data("lookUpId")).findComponent(DJ.UI.TaxonomySearchBrowse);
            var item = scLC.getSelectedItem();
            if (!$.isEmptyObject(item)) {
                this._onItemSelect(item);
                this.publish(this.events.filterSelectClick, this._getFilterSelectEventData("search", item));
            }
        },

        _onLookUpCancelClick: function () {
            this._closeModal('$lookUpsContainer');
            var scLC = $("#" + this.$lookUpsContainer.data("lookUpId")).findComponent(DJ.UI.TaxonomySearchBrowse);
        },

        _getLookUpView: function (lookUpId) {
            if (!this.$lookUpsContainer) {
                this.$lookUpsContainer = this._getModal("lookUpsContainer",
                                            $dj.delegate(this, this._onLookUpDoneClick),
                                            $dj.delegate(this, this._onLookUpCancelClick));
                this.$lookUpsContainer.addClass("dj_lookup");
            }

            return $("<div />")
                   .attr({ "id": lookUpId, "class": "dj_TaxonomySearchBrowse ui-component" })
                   .prependTo(this.$lookUpsContainer.children(":last").children().children(this.selectors.modalContent));
        },

        _getModal: function (idSuffix, doneHandler, cancelHandler, title, cancelText, doneText) {
            var id = this.$element.attr("id") + "_" + idSuffix;
            $(this.templates.modalDialog()).attr("id", id).appendTo(this.$element);
            var $modal = $("#" + id);
            var $footer = $modal.children(":last").children().children(this.selectors.modalContent).children(this.selectors.footer);
            var $doneBtn = $footer.children(this.selectors.doneBtn);
            var $cancelBtn = $footer.children(this.selectors.cancelBtn);
            //Done click
            $doneBtn.click(doneHandler);
            //Cancel click
            $cancelBtn.click(cancelHandler);

            if (title) {
                $modal.children(':first').children("h3").html(title);
            }

            if (doneText) {
                $doneBtn.html(doneText);
            }

            if (cancelText) {
                $cancelBtn.html(cancelText);
            }

            return $modal;
        },

        _closeModal: function ($modal) {
            $().overlay.hide("#" + this[$modal].attr("id"));
        },
        //end functions for the industry/region lookup and browse

        _onItemSelect: function (filter) {
            this._setFilter(filter.type, filter);
            if (filter.type == this.filterType.Industry) {
                this._bindSelectedFilter(filter, this.$industryInputContainer, 0);
            }
            else if (filter.type == this.filterType.Region) {
                this._bindSelectedFilter(filter, this.$regionInputContainer, 1);
            }
        },

        _onAutoSuggestItemSelect: function (item) {
            if (item) {
                var filter = {};
                switch (item.controlType) {
                    case "Industry":
                        filter = { "code": item.code, "desc": item.descriptor, "type": this.filterType.Industry };
                        this._onItemSelect(filter);
                        break;
                    case "Region":
                        filter = { "code": item.code, "desc": item.descriptor, "type": this.filterType.Region };
                        this._onItemSelect(filter);
                        break;
                    case "Keyword":
                        filter = { "code": "", "desc": item.word, "type": this.filterType.Keyword };
                        this.filterCollection[this.filterType.Keyword].selectedText = item.word;
                        break;
                }
                this.publish(this.events.filterSelectClick, this._getFilterSelectEventData("autocomplete", filter));
            }
        },

        _getFilterSelectEventData: function (source, filter) {
            return { "source": source, "type": this.filterDetails[filter.type].name, "data": filter };
        },

        _onAutoSuggestSearch: function (data) {
            var lookupText = "";
            var filterType;
            if (data) {
                lookupText = data.text;
                switch (data.autocompletionType) {
                    case "Region":
                        filterType = this.filterType.Region;
                        this._searchBrowse(filterType, this.filterDetails[filterType].title, lookupText);
                        break;
                    case "Industry":
                        filterType = this.filterType.Industry;
                        this._searchBrowse(filterType, this.filterDetails[filterType].title, lookupText);
                        break;
                }
            }
        },

        _bindFilterClick: function (filterContainer, filterIndex, filterType) {
            var $filterPill = this.$editInputContent.eq(filterIndex).children(this.selectors.filterPill);
            if ($filterPill) {
                $filterPill.find('.remove').bind('click', { pfc: this, filterType: filterType, filterContainer: filterContainer }, function (event) {
                    $(this).parents('.dj_pill-list').first().remove();
                    $(event.data.filterContainer).removeClass('hide');
                    var filterControl = event.data.pfc;
                    if (event.data.filterType == filterControl.filterType.Industry) {
                        filterControl._clearFilter(filterControl.filterType.Industry);
                        filterControl.$industryInput.val("").blur();
                    }
                    else if (event.data.filterType == filterControl.filterType.Region) {
                        filterControl._clearFilter(filterControl.filterType.Region);
                        filterControl.$regionInput.val("").blur();
                    }
                });
            }
        },

        _bindSelectedFilter: function (filterData, filterContainer, filterIndex) {
            $(filterContainer).addClass('hide');
            if (this.$editInputContent.eq(filterIndex).children(this.selectors.filterPill)) {
                //if filter is already selected, then remove and add
                this.$editInputContent.eq(filterIndex).children('.dj_pill-list').first().remove();
            }
            $(filterContainer).before(this.templates.filterPill({ filter: filterData ,disabled:this._disabled}));
            if(!this._disabled){
                this._bindFilterClick(filterContainer, filterIndex, filterData.type);
            }
        },

        _getSearchOptions: function (filterType) {
            var suggestOptions = { maxResults: '10', status: 'active', interfaceLanguage: ($dj.globalHeaders.preferences.interfaceLanguage || 'en') };

            switch (filterType) {
                case this.filterType.Industry:
                    $.extend(suggestOptions, { 'filterParentCodes': this._getParentFilters(this.filterLens.Industry) });
                    break;
                case this.filterType.Region:
                    $.extend(suggestOptions, { 'filterParentCodes': this._getParentFilters(this.filterLens.Region) });
                    break;
            }

            return suggestOptions;
        },

        _getParentFilters: function (lens) {
            var parentFilters = "";
            if (this.data && this.data.parentCodes && $.isArray(this.data.parentCodes) && this.data.parentCodes.length > 0 && this.data.lensType == lens) {
                parentFilters = this.data.parentCodes.join("|");
            }
            return parentFilters;
        },

        _getParentCodes: function (lens) {
            var parentCodes = "";
            if (this.data && this.data.parentCodes && this.data.lensType == lens) {
                parentCodes = this.data.parentCodes;
            }
            return parentCodes;
        },

        _initializeAutoSuggest: function (type, controlId) {
            if (this.options.suggestServiceUrl) {
                var autoSuggestObj = {
                    url: this.options.suggestServiceUrl,
                    controlId: controlId,
                    controlClassName: "djAutoCompleteSource",
                    resultsClass: "dj_emg_autosuggest_results",
                    resultsOddClass: "dj_emg_autosuggest_odd",
                    resultsEvenClass: "dj_emg_autosuggest_even",
                    resultsOverClass: "dj_emg_autosuggest_over",
                    viewAllClass: "dj_emg_autosuggest_view_all",
                    autocompletionType: this.filterDetails[type].name,
                    selectFirst: true,
                    fillInputOnKeyUpDown: true,
                    options: this._getSearchOptions(type),
                    useSessionId: $dj.globalHeaders.credentials.token,
                    onItemSelect: this._delegates.OnAutoSuggestItemSelect
                };

                if (this.filterDetails[type].lookup) {
                    $.extend(autoSuggestObj, { showViewAll: true });
                    $.extend(autoSuggestObj, { tokens: { viewAllTkn: this.filterDetails[type].lookupTitle} });
                    $.extend(autoSuggestObj, { onViewAllClick: this._delegates.OnAutoSuggestSearch });
                }

                if (np && np.web && np.web.widgets && np.web.widgets.autocomplete) {
                    np.web.widgets.autocomplete(autoSuggestObj);
                }
            }
        },

        _bindTextBoxHandlers: function (filterType, controlId, isSub) {
            var textValue = (isSub) ? this.filterDetails[filterType].subAutoCompleteText : this.filterDetails[filterType].autoCompleteText;
            $('#' + controlId).unbind();
            $("#" + controlId).waterMark({
                waterMarkClass: 'dj_text-placeholder',
                waterMarkText: textValue
            });

            this._initializeAutoSuggest(filterType, controlId);
        },

        _bindFilters: function () {
            if (this.data.lensType == this.filterLens.Industry) {
                this.$industryLabel.text(this.filterDetails[this.filterType.Industry].subTitle);
                this.$regionLabel.text(this.filterDetails[this.filterType.Region].title);
            }
            else if (this.data.lensType == this.filterLens.Region) {
                this.$industryLabel.text(this.filterDetails[this.filterType.Industry].title);
                this.$regionLabel.text(this.filterDetails[this.filterType.Region].subTitle);
            }
            else {
                this.$industryLabel.text(this.filterDetails[this.filterType.Industry].title);
                this.$regionLabel.text(this.filterDetails[this.filterType.Region].title);
            }
            this.$industryInput.val(this.filterCollection[this.filterType.Industry].selectedText);
            this.$regionInput.val(this.filterCollection[this.filterType.Region].selectedText);
            this.$keywordInput.val(this.filterCollection[this.filterType.Keyword].selectedText);
            //bind autocomplete
            this._bindTextBoxHandlers(this.filterType.Industry, this._industryInputId, (this.data.lensType == this.filterLens.Industry));
            this._bindTextBoxHandlers(this.filterType.Region, this._regionInputId, (this.data.lensType == this.filterLens.Region));
            this._bindTextBoxHandlers(this.filterType.Keyword, this._keywordInputId, false);
            this._disableInput(this._disabled);
            //remove the watermark class if there is a value in the keyword textbox other that watermark text
            if($.trim(this.$keywordInput.val()) != this.filterDetails[this.filterType.Keyword].autoCompleteText) {
                this.$keywordInput.removeClass("dj_text-placeholder");
            }
            //bind selected filters
            var filter = {};
            if (this.filterCollection[this.filterType.Industry].isSelected) {
                filter = this._getFilter(this.filterType.Industry);
                this._bindSelectedFilter(filter, this.$industryInputContainer, 0);
            }
            if (this.filterCollection[this.filterType.Region].isSelected) {
                filter = this._getFilter(this.filterType.Region);
                this._bindSelectedFilter(filter, this.$regionInputContainer, 1);
            }
        },

        _clearFilter: function (filterType) {
            // NN - null check
            if (this.filterCollection[filterType]) {
                this.filterCollection[filterType].isSelected = false;
                this.filterCollection[filterType].selectedCode = "";
                this.filterCollection[filterType].selectedText = "";
            }
        },

        _clearFilters: function () {
            this._clearFilter(this.filterType.Industry);
            this._clearFilter(this.filterType.Region);
            this.filterCollection[this.filterType.Keyword].selectedText = "";
            this.$editInputContent.eq(0).children('.dj_pill-list').first().remove();
            this.$industryInputContainer.removeClass('hide');
            this.$editInputContent.eq(1).children('.dj_pill-list').first().remove();
            this.$regionInputContainer.removeClass('hide');
        },

        _getFilter: function (filterType) {
            var filter;
            if (filterType == this.filterType.Keyword) {
                var keyword = $.trim(this.$keywordInput.val());
                if (keyword.length > 0 && keyword != this.filterDetails[filterType].autoCompleteText) {
                    filter = { "code": $.trim(this.$keywordInput.val()), "desc": $.trim(this.$keywordInput.val()), "type": filterType };
                }
            }
            else if (this.filterCollection[filterType].isSelected) {
                filter = { "code": this.filterCollection[filterType].selectedCode, "desc": this.filterCollection[filterType].selectedText, "type": filterType };
            }
            return filter;
        },

        _setFilter: function (filterType, filter) {
            if (filter && filter.code) {
                this.filterCollection[filterType].isSelected = true;
                this.filterCollection[filterType].selectedCode = filter.code;
                this.filterCollection[filterType].selectedText = filter.desc;
            }
        },

        _unbindFilterClick: function (filterIndex) {
            this.$editInputContent.eq(filterIndex).find(this.selectors.removePill).unbind("click");
            this.$editInputContent.eq(filterIndex).find(this.selectors.filterPill).addClass("disabled");
        },

        _unbindFiltersClick: function () {
            this._unbindFilterClick(0);
            this._unbindFilterClick(1);
        },

        _bindFiltersClick: function () {
            this.$editInputContent.eq(0).find(this.selectors.filterPill).removeClass("disabled");
            this.$editInputContent.eq(1).find(this.selectors.filterPill).removeClass("disabled");
            this._bindFilterClick(this.$industryInputContainer, 0, this.filterType.Industry);
            this._bindFilterClick(this.$regionInputContainer, 1, this.filterType.Region);
        },

        _disableInput: function(disable){
            if (disable) {
                this.$industryInputContainer.addClass("disabled");
                this.$regionInputContainer.addClass("disabled");
                this.$keywordInputContainer.addClass("disabled");
                this.$industryInput.val("").attr("disabled", "disabled");
                this.$regionInput.val("").attr("disabled", "disabled");
                if($.trim(this.$keywordInput.val()) == this.filterDetails[this.filterType.Keyword].autoCompleteText) {
                    this.$keywordInput.val("").attr("disabled", "disabled");
                }
                else {
                    this.$keywordInput.attr("disabled", "disabled");
                }
            }
            else {
                this.$industryInputContainer.removeClass("disabled");
                this.$regionInputContainer.removeClass("disabled");
                this.$keywordInputContainer.removeClass("disabled");
                this.$industryInput.removeAttr("disabled").blur();
                this.$regionInput.removeAttr("disabled").blur();
                this.$keywordInput.removeAttr("disabled").blur();
            }
        },

        _disableFilters: function(disable){
            if (disable) {
                this._unbindFiltersClick();
            }
            else {
                this._bindFiltersClick();
            }
        },

        clear: function () {
            this._clearFilters();
        },

        setData: function (data, isAjaxCallFired) {
            if (data) {
                this.data = data;
            }
            this.data = this.data || {};
            this._clearFilters();
            if (this.data) {
				fiiDescriptorsCache = fiiDescriptorsCache || {};
				
				var fiiCodeRequestParams;
				
				if (this.data.industryFilter && _.isEmpty(this.data.industryFilter.desc)) {
					if (fiiDescriptorsCache[this.data.industryFilter.code.toLowerCase()]) {
						this.data.industryFilter.desc = fiiDescriptorsCache[this.data.industryFilter.code.toLowerCase()];
					}					
					else if (isAjaxCallFired) {
						// ajax call is fired and we still don't have descriptor for this code, put the code in descriptor
						$dj.debug("Could not get descriptor for code: " + this.data.industryFilter.code);
						this.data.industryFilter.desc = fiiDescriptorsCache[this.data.industryFilter.code.toLowerCase()] = this.data.industryFilter.code;
					}
					else {
					    fiiCodeRequestParams = fiiCodeRequestParams || { };
						fiiCodeRequestParams.industryCodes = this.data.industryFilter.code;
					}
				}
				
				if (this.data.regionFilter && _.isEmpty(this.data.regionFilter.desc)) {
					if (fiiDescriptorsCache[this.data.regionFilter.code]) {
						this.data.regionFilter.desc = fiiDescriptorsCache[this.data.regionFilter.code.toLowerCase()];
					}
					else if (isAjaxCallFired) {
						// ajax call is fired and we still don't have descriptor for this code, put the code in descriptor
						$dj.debug("Could not get descriptor for code: " + this.data.regionFilter.code);
						this.data.regionFilter.desc = fiiDescriptorsCache[this.data.regionFilter.code.toLowerCase()] = this.data.regionFilter.code;
					}
					else {
					    fiiCodeRequestParams = fiiCodeRequestParams || { };
						fiiCodeRequestParams.regionCodes = this.data.regionFilter.code;
					}
				}
				
				if (fiiCodeRequestParams) {
					fiiCodeRequestParams.language = $dj.globalHeaders.preferences.interfaceLanguage || 'en';
					$.ajax({
						url: this.options.fiiCodeServiceUrl,
						data: fiiCodeRequestParams,
						success: $dj.delegate(this, function(result) {
							for(var i = 0; i < result.FIICodes.length; i++) {
								var fiiCode = result.FIICodes[i];
								if (!_.isEmpty(fiiCode.Value)) {
									fiiDescriptorsCache[fiiCode.Code.toLowerCase()] = fiiCode.Value;
								}
							}
						}),
						error: $dj.delegate(this, function(result) {
							$dj.debug("FIICode service call failed: " + result);	
						}),
						complete: $dj.delegate(this, function() {
							this.setData(this.data, true);
						})
					});
					return;
				}				
			
                if (typeof (this.data.lensType) == 'string') {
                    this.data.lensType = this.filterLens[this.data.lensType];
                }
                this._setFilter(this.filterType.Industry, this.data.industryFilter);
                this._setFilter(this.filterType.Region, this.data.regionFilter);
                if (this.data.keywordFilter) {
                    this.filterCollection[this.filterType.Keyword].selectedText = this.data.keywordFilter;
                }
            }
            this._bindFilters();
        },
		
        getFilters: function () {
            var filters = {
                industryFilter: this._getFilter(this.filterType.Industry),
                regionFilter: this._getFilter(this.filterType.Region),
                keywordFilter: this._getFilter(this.filterType.Keyword)
            };
            return filters;
        },

        getQueryFilters: function () {
            var filters = this.getFilters();
            return $.map(filters, function (filter) {
                if (!$.isEmptyObject(filter)) {
                    return { text: filter.code, type: filter.type, descriptor: filter.desc };
                }
            });
        },


        disable : function (disable) {
            //function to set the disabled property and disable the component
            this._disabled = disable;
            this._disableInput(disable);
            this._disableFilters(disable);
        },

        updateLens: function (lensType, parentCodes) {
            var data = this.data || {};
            data.lensType = lensType;
            if (typeof (data.lensType) == 'string') {
                data.lensType = this.filterLens[data.lensType];
            }
            data.parentCodes = parentCodes;
            data.industryFilter = {};
            data.regionFilter = {};
            data.keywordFilter = "";
            this.setData(data);
        }

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_PersonalizationFilters', DJ.UI.PersonalizationFilters);

    $dj.debug('Registered DJ.UI.PersonalizationFilters (extends DJ.UI.Component)');

})(jQuery);
