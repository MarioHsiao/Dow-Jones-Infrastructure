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
        removeDuplicate: 'select.removeDuplicate',
        saveBtn: 'a.dc_btn-1',
        cancelBtn: 'a.dc_btn-3',
        newsFilter: 'tr.newsFilter',
        newsFilterContainer: 'td:last',
        filterLabel: 'span.filterLabel',
        filterClose: 'span.icon-close',
        filterNot: '.pillOption.not',
        filterAnd: '.pillOption.and',
        filterRemove: '.pillOption.remove',
        filter: 'span.filter',
        selectedOption: 'option:selected'
    },

    defaults: {
        enableExclude: false
    },

    events: {
        onSaveClick: 'onSaveClick.dj.SimpleAlert',
        onCancelClick: 'onCancelClick.dj.SimpleAlert'
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "SimpleAlert" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        //Bind the layout template
        $(this.$element).html(this.templates.layout);

        this._initializeControls();

        this.setData();
    },

    _initializeDelegates: function () {
        this._super();
        $.extend(this._delegates, {
            OnSaveClick: $dj.delegate(this, this._onSaveClick),
            OnCancelClick: $dj.delegate(this, this._onCancelClick),
            OnDeliveryTimeChange: $dj.delegate(this, this._onDeliveryTimeChange)
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
            removeDuplicate: $(this.selectors.removeDuplicate, this.$element)
        }
        this.$title = $(this.selectors.title, this.$element);
        this.$newsFilter = $(this.selectors.newsFilter, this.$element);
        this.$newsFilterContainer = $(this.selectors.newsFilterContainer, this.$newsFilter);
    },

    _initializeEventHandlers: function () {
        $(this.selectors.saveBtn, this.$element).unbind('click').click(this._delegates.OnSaveClick);
        $(this.selectors.cancelBtn, this.$element).unbind('click').click(this._delegates.OnCancelClick);
    },

    _clearData: function () {
        this.$title.hide();
        this.$input.folderName.val('');
        this.$input.sourceList.html('');
        this.$input.searchText.val('');
        this.$input.emailAddress.val('').attr("disabled", true);
        this.$input.emailFormat.html('').attr("disabled", true);
        this.$input.deliveryTime.html('');
        this.$input.sourceList.html('');
        this.$newsFilterContainer.html('');
        this.$newsFilter.hide();
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
        if (me.data.newsFilter && me.data.newsFilter.length > 0) {
            this.$newsFilterContainer
                .html(this.templates.newsFilterEx({ filterItems: me.data.newsFilter }))
                .find(this.selectors.filterClose)
                .bind('click', function () {
                    me._onFilterClose(this);
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
            this.$input.emailFormat.append(this.templates.options({ items: d.emailFormats })).val(d.selectedEmailFormat);

            //DeliveryTimes
            this.$input.deliveryTime.append(this.templates.options({ items: d.deliveryTimes }))
                .change(this._delegates.OnDeliveryTimeChange).val(d.selectedDeliveryTime);

            //Duplicates
            this.$input.removeDuplicate.append(this.templates.options({ items: d.duplicates }));

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
            var obj = {
                alertName: this.$input.folderName.val(),
                searchText: this.$input.searchText.val(),
                newsFilter: this._getNewsFilter(),
                emailAddress: this.$input.emailAddress.val(),
                selectedEmailFormat: this.$input.emailFormat.val(),
                selectedDeliveryTime: this.$input.deliveryTime.val(),
                selectedDuplicate: this.$input.removeDuplicate.val(),
                selectedSource: this.$input.sourceList.val(),
                selectedSourceDesc: this.$input.sourceList.find(this.selectors.selectedOption).text()
            };

            //Trigger the save event and pass the data
            this.$element.triggerHandler(this.events.onSaveClick, { alertObj: obj });
        }
    },

    _onCancelClick: function () {
        //Trigger the cancel event
        this.$element.triggerHandler(this.events.onCancelClick);
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

    _onDeliveryTimeChange: function () {
        if (this.$input.deliveryTime[0].selectedIndex == 0) {
            this.$input.emailAddress.attr("disabled", true);
            this.$input.emailFormat.attr("disabled", true);
        }
        else {
            this.$input.emailAddress.attr("disabled", false);
            this.$input.emailFormat.attr("disabled", false);
        }
    }
});

    // Declare this class as a jQuery plugin
    $.plugin('dj_SimpleAlert', DJ.UI.SimpleAlert);

    $dj.debug('Registered DJ.UI.SimpleAlert (extends DJ.UI.Component)');
