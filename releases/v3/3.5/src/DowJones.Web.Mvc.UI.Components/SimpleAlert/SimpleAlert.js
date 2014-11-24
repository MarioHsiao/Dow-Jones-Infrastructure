/*
*  Simple Alert Control
*/

DJ.UI.SimpleAlert = DJ.UI.Component.extend({
    selectors: {
        title: 'div.title',
        folderName: 'input.folderName',
        sourceList: 'select.sourceList',
        searchText: 'input.searchText',
        emailAddress: 'input.emailAddress',
        emailFormat: 'select.emailFormat',
        deliveryTime: 'select.deliveryTime',
        resDispFormat: 'select.resDispFormat',
        removeDuplicate: 'select.removeDuplicate',
        saveBtn: 'a.dc_btn-1',
        cancelBtn: 'a.dc_btn-3',
        newsFilter: 'tr.newsFilter',
        newsFilterContainer: 'td:last',
        filterLabel: 'span.filterLabel',
        filterClose: 'span.icon-close',
        filter: 'span.filter',
        selectedOption: 'option:selected',
        includeSM: 'input.includeSM',
        filterCloseEx: 'span.fi-two.fi_remove.fi_d-gear',
        resDspFmtInfIcon: 'span.resDspFmtInfIcon',
        enableHightLightSearch: 'input:radio',
        clickedHighLightYes: 'input:radio[name=grp1]'
    },

    defaults: {
        enableExclude: false,
        cssClass: 'dj_SimpleAlert'
    },

    events: {
        onSaveClick: 'onSaveClick.dj.SimpleAlert',
        onCancelClick: 'onCancelClick.dj.SimpleAlert',
        onRDFInfoIconClick: 'onRDFInfoIconClick.dj.SimpleAlert'
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "SimpleAlert" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        //Bind the layout template
        $(this.$element).html(this.templates.layout({ highlightFieldEnabled: this.data.highlightFieldEnabled }));

        this._initializeControls();

        this.setData();
    },

    _initializeDelegates: function () {
        this._super();
        $.extend(this._delegates, {
            OnSaveClick: $dj.delegate(this, this._onSaveClick),
            OnCancelClick: $dj.delegate(this, this._onCancelClick),
            OnRDFInfoIconClick: $dj.delegate(this, this._onRDFInfoIconClick),
            OnDeliveryTimeChange: $dj.delegate(this, this._onDeliveryTimeChange),
            OnResultDisplayChange: $dj.delegate(this, this._onResultDisplayChange)
        });
    },

    _initializeControls: function () {
        this.$input = {
            folderName: $(this.selectors.folderName, this.$element),
            sourceList: $(this.selectors.sourceList, this.$element),
            searchText: $(this.selectors.searchText, this.$element),
            emailAddress: $(this.selectors.emailAddress, this.$element),
            emailFormat: $(this.selectors.emailFormat, this.$element),
            deliveryTime: $(this.selectors.deliveryTime, this.$element),
            removeDuplicate: $(this.selectors.removeDuplicate, this.$element),
            includeSM: $(this.selectors.includeSM, this.$element),
            resDispFormat: $(this.selectors.resDispFormat, this.$element),
            enableHightLightSearch: $(this.selectors.enableHightLightSearch, this.$element),
            clickedHighLightYes: $(this.selectors.clickedHighLightYes, this.$element)
        }
        this.$title = $(this.selectors.title, this.$element);
        this.$newsFilter = $(this.selectors.newsFilter, this.$element);
        this.$newsFilterContainer = $(this.selectors.newsFilterContainer, this.$newsFilter);
    },

    _initializeEventHandlers: function () {
        $(this.selectors.saveBtn, this.$element).unbind('click').click(this._delegates.OnSaveClick);
        $(this.selectors.cancelBtn, this.$element).unbind('click').click(this._delegates.OnCancelClick);
        $(this.selectors.resDspFmtInfIcon, this.$element).unbind('click').click(this._delegates.OnRDFInfoIconClick);
    },

    _clearData: function () {
        this.$title.hide();
        this.$input.folderName.val('');
        this.$input.sourceList.html('');
        this.$input.searchText.val('');
        this.$input.emailAddress.val('').attr("disabled", true);
        this.$input.emailFormat.html('').attr("disabled", true);
        this.$input.deliveryTime.html('');
        this.$input.resDispFormat.html('');
        this.$input.sourceList.html('');
        this.$newsFilterContainer.html('');
        this.$newsFilter.hide();
        //this.$input.enableHightLightSearch.attr("disabled", true);
    },

    _bindNewsFilter: function () {
        var me = this;
        if (me.data.newsFilter && me.data.newsFilter.length > 0) {
            this.$newsFilterContainer
                .html(this.templates.newsFilter({ filterItems: me.data.newsFilter }))
                .find(this.selectors.filterClose)
                .bind('click', function () {
                    me._onFilterClose(this);
                });
            this.$newsFilter.show();
        }
    },

    _bindNewsFilterEx: function () {
        var me = this;
        $(me.$element).newsFilterPillMenuEx({ 'removeFilter': me._onFilterRemove, 'excludeFilter': me._onFilterExclude, 'includeFilter': me._onFilterInclude });
        if (me.data.newsFilter && me.data.newsFilter.length > 0) {
            this.$newsFilterContainer
                .html(this.templates.newsFilterEx({ filterItems: me.data.newsFilter }))
                .find(this.selectors.filterCloseEx)
                .bind('click', function () {
                    var $el = $(this).closest('div').parent();
                    me._onFilterRemove($el);
                });
            this.$newsFilter.show();
        }
    },

    setData: function (data) {
        if (data)
            this.data = data;

        this._clearData();

        //Title
        if (this.options.title) {
            this.$title.html(this.options.title).show();
        }

        if (this.data) {

            var d = this.data;

            //SourceList
            this.$input.sourceList.append(this.templates.options({ items: d.sourceList })).val(d.selectedSource);

            //SearchText
            this.$input.searchText.val(d.searchText || '');

            //EmailAddress
            this.$input.emailAddress.val(d.emailAddress || '');

            //EmailFormat
            this.$input.emailFormat.append(this.templates.options({ items: d.emailFormats }))
                .change(this._delegates.OnResultDisplayChange).val(d.selectedEmailFormat); 

            //DeliveryTimes
            this.$input.deliveryTime.append(this.templates.options({ items: d.deliveryTimes }))
                .change(this._delegates.OnDeliveryTimeChange).val(d.selectedDeliveryTime);

            //Duplicates
            this.$input.removeDuplicate.append(this.templates.options({ items: d.duplicates }));

            //Results Display Format
            this.$input.resDispFormat.append(this.templates.options({ items: d.resultsDisplayFormats }))
                .change(this._delegates.OnResultDisplayChange).val(d.selectedDisplayFormat);

            //Include Social Media
            if (d.includeSocialMedia) {
                this.$input.includeSM.attr("checked", "checked");
            }

            if (!this.options.enableExclude) {
                //News Fitler
                this._bindNewsFilter();
            } else {
                //News Fitler whic includes Exclude
                this._bindNewsFilterEx();
            }

            if (d.selectedDeliveryTime != this.$input.deliveryTime[0].options[0].value) {
                this.$input.emailAddress.attr("disabled", false);
                this.$input.emailFormat.attr("disabled", false);
            }


            if ($('#grpyes').length > 0) {
                $('#grpyes').attr("checked", "checked");
            }

            if (this.$input.resDispFormat.length) {
                if ((d.selectedDisplayFormat != this.$input.resDispFormat[0].options[0].value && d.selectedDisplayFormat != this.$input.resDispFormat[0].options[1].value) && (d.selectedEmailFormat != this.$input.emailFormat[0].options[0].value)) {
                    this.$input.enableHightLightSearch.attr("disabled", false);
                } else {
                    this.$input.enableHightLightSearch.attr("disabled", true);
                    $('#grpyes').removeAttr("checked", "checked");
                    $('#grpno').attr("checked", "checked");
                }
            }

            this._initializeEventHandlers();
        }
    },

    validateInput: function () {
        if ($.trim(this.$input.folderName.val()) == "") {
            alert("<%= Token('enterFldrNameUpto25Chars')%>", $dj.delegate(this, function () { this.$input.folderName.focus(); }));
            this.$input.folderName.focus();
            return false;
        }
        if ($dj.hasIllegalChar(this.$input.folderName.val())) {
            alert("<%= Token('illegalChar-1')%> (<, >, &, #, \\, %, +, |) <%= Token('illegalChar-2')%>", $dj.delegate(this, function () { this.$input.folderName.focus(); }));
            this.$input.folderName.focus();
            return false;
        }
        if (this.$input.emailAddress.is(":enabled")) {
            if ($.trim(this.$input.emailAddress.val()) == "") {
                alert("<%= Token('enterEmailAddrForDelivery')%>", $dj.delegate(this, function () { this.$input.emailAddress.focus(); }));
                this.$input.emailAddress.focus();
                return false;
            }
            if (!$dj.validateEmail(this.$input.emailAddress.val())) {
                alert("<%= Token('invalidEmail')%> <%= Token('profValidEmail')%>", $dj.delegate(this, function () { this.$input.emailAddress.focus(); }));
                this.$input.emailAddress.focus();
                return false;
            }
        }
        if ($.trim(this.$input.searchText.val()) == '' && !this._hasNewsFilter()) {
            alert("<%= Token('noSearchStrMsg')%>", $dj.delegate(this, function () { this.$input.searchText.focus(); }));
            this.$input.searchText.focus();
            return false;
        }
        return true;
    },

    _hasNewsFilter: function () {
        if (!this.data.newsFilter) {
            return false;
        }

        var items = this.data.newsFilter;
        if (items.length == 0) {
            return false;
        }

        var counter = 0, filters;
        for (var i = 0; i < items.length; i++) {
            if (items[i]) {
                if (items[i].removed) {
                    continue;
                }
                if (items[i].filter && items[i].filter.length > 0) {
                    filters = items[i].filter;
                    for (var j = 0; j < filters.length; j++) {
                        if (filters[j]) {
                            if (filters[j].removed) {
                                continue;
                            }
                            if (filters[j].type != 'Sources') {
                                counter++;
                            }
                        }
                    }
                }
            }
        }

        if (counter == 0) {
            return false;
        }
        return true;
    },

    _onSaveClick: function () {
        if (this.validateInput()) {
            //Create the data to be passed to the save event handler
            var higlightTextOptionChecked = false;
           
            if (this.$input.clickedHighLightYes.is(':enabled')) {
                    if ($('input:radio[name=grp1]:checked').val() != "" && $('input:radio[name=grp1]:checked').val() == "Yes") {
                        higlightTextOptionChecked = true;
                    }
                }
            
            var obj = {
                alertName: this.$input.folderName.val(),
                searchText: this.$input.searchText.val(),
                newsFilter: this._getNewsFilter(),
                emailAddress: this.$input.emailAddress.val(),
                selectedEmailFormat: this.$input.emailFormat.val(),
                selectedDeliveryTime: this.$input.deliveryTime.val(),
                selectedDuplicate: this.$input.removeDuplicate.val(),
                selectedSource: this.$input.sourceList.val(),
                selectedSourceDesc: this.$input.sourceList.find(this.selectors.selectedOption).text(),
                includeSocialMedia: this.$input.includeSM.is(':checked'),
                selectedDisplayFormat: this.$input.resDispFormat.val(),
                enabledEmailHighLight: higlightTextOptionChecked
        };

            //Trigger the save event and pass the data
            this.$element.triggerHandler(this.events.onSaveClick, { alertObj: obj });
        }
    },

    _onCancelClick: function () {
        //Trigger the cancel event
        this.$element.triggerHandler(this.events.onCancelClick);
    },

    _onRDFInfoIconClick: function() {
        //Trigger the info icon click event
        this.$element.triggerHandler(this.events.onRDFInfoIconClick, { rdfInfoIconSel: this.selectors.resDspFmtInfIcon });
    },

    _getNewsFilter: function () {
        var nf = null;
        if (this.data.newsFilter) {
            this.data.newsFilter = _.select(this.data.newsFilter, function (item) { return !item.removed; });
            _.each(this.data.newsFilter, function (item, index) {
                item.filter = _.select(item.filter, function (filter) { return !filter.removed; });
            });
            nf = this.data.newsFilter;
        }
        return nf;
    },

    _onFilterClose: function (elem) {
        var $elem = $(elem), fIndex = parseInt($elem.attr("fIndex")), iIndex = parseInt($elem.attr("iIndex")), $filter = $elem.closest(this.selectors.filter);
        var allFiltersRemoved = false, allItemsRemoved = false;
        this.data.newsFilter[fIndex].filter[iIndex].removed = true;
        allItemsRemoved = (_.select(this.data.newsFilter[fIndex].filter, function (item) { return item.removed == true; })).length == this.data.newsFilter[fIndex].filter.length;
        if (allItemsRemoved) {
            $filter.closest("div").remove();
            this.data.newsFilter[fIndex].removed = true;
            allFiltersRemoved = (_.select(this.data.newsFilter, function (item) { return item.removed == true; })).length == this.data.newsFilter.length;
            if (allFiltersRemoved) {
                this.$newsFilter.hide();
            }
        }
        $filter.remove();
    },

    _onFilterRemove: function (elem, args) {
        var $simpleAlertContainer = $(elem).closest('.dj_SimpleAlert'),
            alertData = $simpleAlertContainer.data("data"),
            mode = $(elem).attr('mode'),
            $gearBtn = $('span.fi_d-gear', elem),
            $alertSource = ($(elem).closest('li')).prev('li.dj_alert-source'),
            $connectAndPillWrap = $(elem).closest('li.connectionAndPillWrap'),
            pillWrap = $('.filterPillWrap[mode="' + mode + '"]', $connectAndPillWrap.nextAll('li.connectionAndPillWrap')),
            nextGearItems = $('span.fi_d-gear', $connectAndPillWrap.nextAll('li.connectionAndPillWrap')),
            fIndex = parseInt($gearBtn.attr("fIndex")),
            iIndex = parseInt($gearBtn.attr("iIndex")),
            filterLength = 0,
            excludeFilterLength = 0;

        switch (mode.toLowerCase()) {
            case "and":
            case "remove":
                alertData.newsFilter[fIndex].filter.splice(iIndex, 1);
                break;
            case "not":
                alertData.newsFilter[fIndex].excludeFilter.splice(iIndex, 1);
                break;
        }

        $.each($("span[findex=" + fIndex + "]", pillWrap), function (idx, val) {
            var newIIndex = parseInt($(val).attr('iindex')) - 1;
            $(val).attr('iindex', newIIndex + "");
            $(val).closest('li.connectionAndPillWrap').attr('iindex', newIIndex + "");
        });

        if (alertData.newsFilter[fIndex] && alertData.newsFilter[fIndex].filter) {
            filterLength = alertData.newsFilter[fIndex].filter.length;
        }

        if (alertData.newsFilter[fIndex] && alertData.newsFilter[fIndex].excludeFilter) {
            excludeFilterLength = alertData.newsFilter[fIndex].excludeFilter.length;
        }

        if (filterLength === 0 && excludeFilterLength === 0) {
            alertData.newsFilter.splice(fIndex, 1)
            $alertSource.remove();
            //Reset the source index

            $.each(nextGearItems, function (idx, val) {
                var newFIndex = parseInt($(val).attr('findex')) - 1;
                $(val).attr('findex', newFIndex + "");
                $(val).closest('li.connectionAndPillWrap').attr('findex', newFIndex + "");
            });
        }
        $connectAndPillWrap.remove();
        //Delete the whole NewsFilter <tr> when there is no more news filter
        if (alertData.newsFilter.length === 0) {
            $('tr.newsFilter').hide();
        }
        $simpleAlertContainer.data("data", alertData);
    },

    _onFilterInclude: function (elem) {
        var $simpleAlertContainer = $(elem).closest('.dj_SimpleAlert'),
            alertData = $simpleAlertContainer.data("data"),
            mode = $(elem).attr('mode'),
            $alertSource = ($(elem).closest('li')).prev('li.dj_alert-source'),
            $connectAndPillWrap = $(elem).closest('li.connectionAndPillWrap'),
            $filterConnection = $(elem).siblings("span.filterConnection"),
            $gearBtn = $('span.fi_d-gear', elem),
            fIndex = parseInt($gearBtn.attr("findex")),
            iIndex = parseInt($gearBtn.attr("iindex"));

        if (!alertData.newsFilter[fIndex].filter) {
            alertData.newsFilter[fIndex].filter = [];
        }
        //Insert and Remove from filter and excludeFilter
        alertData.newsFilter[fIndex].filter.splice(alertData.newsFilter[fIndex].filter.length, 0, alertData.newsFilter[fIndex].excludeFilter[iIndex])
        alertData.newsFilter[fIndex].excludeFilter.splice(iIndex, 1);


        $(elem).attr('mode', 'and');
        $connectAndPillWrap.attr('mode', 'and');
        pillWrapAnd = $('li.connectionAndPillWrap[mode="and"][findex="' + fIndex + '"]', $(elem).closest('.dj_SimpleAlert'));
        pillWrapNot = $('li.connectionAndPillWrap[mode="not"][findex="' + fIndex + '"]', $(elem).closest('.dj_SimpleAlert'));
        $.each($("span[findex=" + fIndex + "]", pillWrapAnd), function (idx, val) {
            $(val).attr('iindex', idx + "");
            var currentLi = $(val).closest('li.connectionAndPillWrap');
            $(currentLi).attr('iindex', idx + "");
            if (idx === alertData.newsFilter[fIndex].filter.length - 1) {
                $(currentLi).insertAfter($(currentLi).siblings('li[mode="and"][findex="' + fIndex + '"]').last());
            }
        });
        $.each($("span[findex=" + fIndex + "]", pillWrapNot), function (idx, val) {
            $(val).attr('iindex', idx + "");
            $(val).closest('li.connectionAndPillWrap').attr('iindex', idx + "");
        });

        $filterConnection.remove();

        $simpleAlertContainer.data("data", alertData);
    },

    _onFilterExclude: function (elem) {
        var $simpleAlertContainer = $(elem).closest('.dj_SimpleAlert'),
            alertData = $simpleAlertContainer.data("data"),
            mode = $(elem).attr('mode'),
            $alertSource = ($(elem).closest('li')).prev('li.dj_alert-source'),
            $connectAndPillWrap = $(elem).closest('li.connectionAndPillWrap'),
            $filterConnectionHtml = "<span class='filterConnection'>" +
                                    "<span class='connectionText connectionTextNot'><%= Token('notLabel') %></span>" +
                                    "</span>";
        $gearBtn = $('span.fi_d-gear', elem),
            fIndex = parseInt($gearBtn.attr("findex")),
            iIndex = parseInt($gearBtn.attr("iindex"));

        if (!alertData.newsFilter[fIndex].excludeFilter) {
            alertData.newsFilter[fIndex].excludeFilter = [];
        }
        var exFilterLength = alertData.newsFilter[fIndex].excludeFilter.length;
        //Insert and Remove from excludeFilter and filter
        alertData.newsFilter[fIndex].excludeFilter.splice(exFilterLength, 0, alertData.newsFilter[fIndex].filter[iIndex]);
        alertData.newsFilter[fIndex].filter.splice(iIndex, 1);

        $(elem).attr('mode', 'temp');
        $connectAndPillWrap.attr('mode', 'temp');
        pillWrapAnd = $('li.connectionAndPillWrap[mode="and"][findex="' + fIndex + '"]', $(elem).closest('.dj_SimpleAlert'));
        pillWrapNot = $('li.connectionAndPillWrap[mode="not"][findex="' + fIndex + '"]', $(elem).closest('.dj_SimpleAlert'));
        $.each($("span[findex=" + fIndex + "]", pillWrapAnd), function (idx, val) {
            $(val).attr('iindex', idx + "");
            $(val).closest('li.connectionAndPillWrap').attr('iindex', idx + "");
        });
        $.each($("span[findex=" + fIndex + "]", pillWrapNot), function (idx, val) {
            $(val).attr('iindex', idx + "");
            var currentLi = $(val).closest('li.connectionAndPillWrap');
            $(currentLi).attr('iindex', idx + "");
        });

        $(elem).attr('mode', 'not');
        $connectAndPillWrap.attr('mode', 'not').attr('iindex', exFilterLength);
        $gearBtn.attr('iindex', exFilterLength);
        $connectAndPillWrap.prepend($filterConnectionHtml);
        $connectAndPillWrap.insertAfter($connectAndPillWrap.nextAll('li.connectionAndPillWrap[findex="' + fIndex + '"]').last());
        $simpleAlertContainer.data("data", alertData);
    },

    _onDeliveryTimeChange: function () {
        if (this.$input.deliveryTime[0].selectedIndex == 0) {
            this.$input.emailAddress.attr("disabled", true);
            this.$input.emailFormat.attr("disabled", true);
        }
        else {
            this.$input.emailAddress.attr("disabled", false);
            this.$input.emailFormat.attr("disabled", false);
        }
    },
    _onResultDisplayChange: function () {
        if (this.$input.resDispFormat.length) {
            if (this.$input.resDispFormat[0].selectedIndex == 0 || this.$input.resDispFormat[0].selectedIndex == 1 || this.$input.emailFormat[0].selectedIndex == 0) {
                $('#grpyes').removeAttr("checked", "checked");
                $('#grpno').attr("checked", "checked");
                this.$input.enableHightLightSearch.attr("disabled", true);
            } else {
                this.$input.enableHightLightSearch.attr("disabled", false);
                $('#grpno').removeAttr("checked", "checked");
                $('#grpyes').attr("checked", "checked");
            }
        }
    }
});

    // Declare this class as a jQuery plugin
    $.plugin('dj_SimpleAlert', DJ.UI.SimpleAlert);

    $dj.debug('Registered DJ.UI.SimpleAlert (extends DJ.UI.Component)');
