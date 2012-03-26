/*!
* Alert Editor
*/

(function ($) {

    DJ.UI.AlertEditor = DJ.UI.Component.extend({

        /*
        * Properties
        */
        selectors: {
            filters: '.alert-filters',
            form: '.save-as-alert-form',
            name: '.alert-name',
            email: '.alert-email',
            format: '.alert-format',
            delivery: '.alert-delivery',
            deliveryMethod: '.alert-delivery-method',
            deliveryTime: 'ul.inline-checklist li',
            duplicates: '.alert-duplicates',
            pressClips: '.alert-press-clips',
            toggleSwitch: '.dj_toggle-switch',
            footer: '.footer',
            cancelBtn: '.dj_btn-drk-gray',
            saveBtn: '.dj_btn-blue',
            freeText: '.freeText',
            filterPill: '.dj_pill',
            filterRemove: '.remove',
            filtersWrap: '.filters-wrap',
            filterGroup: '.filter-group',
            filterList: '.filter-list',
            pillList: '.dj_pill-list',
            languageFilter: 'ul.languageFilter',
            radioBtn: 'label.radio, input:radio'
        },

        events: {
            onSaveClick: 'onSaveClick.dj.AlertEditor',
            onCancelClick: 'onCancelClick.dj.AlertEditor'
        },

        filterDetails: {
            company: {
                text: "<%= Token('companyLabel') %>",
                pluralText: "<%= Token('companies') %>"
            },
            author: {
                text: "<%= Token('author') %>",
                pluralText: "<%= Token('authors') %>"
            },
            executive: {
                text: "<%= Token('executive') %>",
                pluralText: "<%= Token('executives') %>"
            },
            subject: {
                text: "<%= Token('subject') %>",
                pluralText: "<%= Token('subjects') %>"
            },
            industry: {
                text: "<%= Token('industry') %>",
                pluralText: "<%= Token('industries') %>"
            },
            region: {
                text: "<%= Token('regionLabel') %>",
                pluralText: "<%= Token('regions') %>"
            },
            source: {
                text: "<%= Token('sourceLabel') %>",
                pluralText: "<%= Token('sources') %>"
            },
            keyword: {
                text: "<%= Token('keywords') %>"
            }
        },

        removeDuplicate: {
            Off: "0",
            Similar: "1",
            VirtuallyIdentical: "2"
        },

        deliveryTimes: {
            None: "0",
            Morning: "1",
            Afternoon: "2",
            Both: "3",
            Continuous: "4",
            EarlyMorning: "5"
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "AlertEditor" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this.setData(this.data);
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnDeliveryTimeChange: $dj.delegate(this, this._onDeliveryTimeChange)
            });
        },

        _initializeControls: function () {

            // Set the main template;
            this.$element.append(this.templates.main());

            this.$alertForm = this.$element.children(this.selectors.form).children("fieldset");

            this.$alertName = this.$alertForm.children(this.selectors.name).children("input");
            this.$freeText = this.$alertForm.children(this.selectors.freeText).children("p");
            this.$filters = this.$alertForm.children(this.selectors.filters);
            this.$alertEmail = this.$alertForm.children(this.selectors.email).children("input");
            this.$alertFormatDD = this.$alertForm.children(this.selectors.format).children("select");
            this.$alertDelivery = this.$alertForm.children(this.selectors.delivery);
            this.$alertDeliveryMethod = this.$alertForm.children(this.selectors.deliveryMethod);
            this.$pressClips = this.$alertForm.children(this.selectors.pressClips).children(this.selectors.toggleSwitch);
            this.$removeDuplicate = this.$alertForm.children(this.selectors.duplicates).children(this.selectors.toggleSwitch);

            var $footer = this.$element.children(this.selectors.footer);
            this.$saveBtn = $footer.children(this.selectors.saveBtn);
            this.$cancelBtn = $footer.children(this.selectors.cancelBtn);
        },

        _initializeEventHandlers: function () {

            this._initializeControls();

            var me = this;

            this.$cancelBtn.unbind('click').click(function () {
                me.publish(me.events.onCancelClick, me);
            });

            this.$saveBtn.unbind('click').click(function () {
                me.publish(me.events.onSaveClick, me.getAlertRequestObject());
            });

            // Bind Alert E-mail Formats
            this._bindAlertEmailFormats(this.options.emailFormats);
            this.$alertFormatDD.selectbox();

            // Toggle Duplicates Switch(For Remove Duplicates and Press Clippings Only)
            this.$alertForm.delegate(this.selectors.toggleSwitch, 'click', function () {
                var $this = $(this);
                me._toggleSwitch($this, $this.children("span.switch").hasClass("on"));
            });

            //Filters remove handler
            this.$filters.delegate(this.selectors.filterRemove, 'click', function () {
                var $elem = $(this), $li = $elem.closest(me.selectors.filterPill), $pillList;

                if ($li.parent().is(me.selectors.languageFilter)) {//Language filter
                    $pillList = $elem.closest(me.selectors.pillList);

                    //Remove the pill
                    $li.remove();

                    //Show All languages if all the languages are removed
                    if ($pillList.children().length == 0) {
                        $pillList.html('<li data-code="alllang" class="dj_pill"><span class="filter"><%= Token("allLanguages") %></span></li>');
                    }
                }
                else {
                    var $filterList = $elem.closest(me.selectors.filterList);
                    $pillList = $filterList.closest(me.selectors.pillList);

                    //Remove the pill
                    $li.remove();

                    //Remove the filterGroup if all filters are removed
                    if ($filterList.children().length == 0) {
                        $filterList.closest(me.selectors.filterGroup).remove();
                    }

                    //Check if all filter groups are removed
                    if ($pillList.children().length == 0) {
                        //Hide the filters section
                        $pillList.closest(me.selectors.filtersWrap).slideUp(200);
                    }
                }
            });

            //Delivery Time
            this.$alertDelivery.delegate(this.selectors.deliveryTime, 'click', function () {
                $(this).find('span.inline-checkbox').toggleClass('checked');
                me._onDeliveryTimeChange();
                return false;
            });

            //Deliery Method
            this.$alertDeliveryMethod.delegate(this.selectors.radioBtn, 'click', function () {
                var $radio = $(this).is('input') ? $(this) : $(this).prev();
                $radio.prop('checked', true);
                me._onDeliveryMethodChange();
            });
        },

        _bindSearchQuery: function (searchQuery) {
            if (searchQuery) {

                if (this.options.searchCriteriaEditable) {//Alert created using Simple search
                    //Show textbox for free text
                    if (!this.$freeText.is('input')) {
                        this.$freeText.append("<input type='text' maxlength='500' />");
                        this.$freeText = this.$freeText.find('input');
                    }
                    this.$freeText.val(searchQuery.freeText).closest('div').show();
                }
                else { // Alert created using Advanced search
                    searchQuery.freeText ? this.$freeText.html(searchQuery.freeText).closest('div').show() : this.$freeText.closest('div').hide();
                }

                this._bindFilters(searchQuery);
            }
            else {
                this.$freeText.closest('div').hide();
                this.$filters.hide();
            }
        },

        _bindFilters: function (searchQuery) {
            if (searchQuery.filters || searchQuery.newsFilters || searchQuery.contentLanguages) {
                this.$filters.html(this.templates.filters({ data: searchQuery, fDetails: this.filterDetails, languageList: this.options.languageList, newsFilterEditable: this.options.searchCriteriaEditable })).show();
            }
            else {
                this.$filters.hide();
            }
        },

        _bindAlertEmailFormats: function (frmt) {
            this.$alertFormatDD.empty();
            if (frmt) {
                for (var i = 0; i < frmt.length; i++) {
                    this.$alertFormatDD.append('<option value="' + frmt[i].code + '">' + frmt[i].desc + '</option>');
                }
            }
        },

        _setDeliveryTime: function (dlvryTimes) {
            var me = this, $deliveryTimes = this.$alertDelivery.find('span.inline-checkbox').removeClass('checked');

            $.each((dlvryTimes || []), function (i, dlvryTime) {
                if (dlvryTime == me.deliveryTimes.EarlyMorning) {
                    $deliveryTimes.filter('.earlyMorning').addClass('checked');
                }
                else if (dlvryTime == me.deliveryTimes.Morning) {
                    $deliveryTimes.filter('.morning').addClass('checked');
                }
                else if (dlvryTime == me.deliveryTimes.Afternoon) {
                    $deliveryTimes.filter('.afternoon').addClass('checked');
                }
            });

            this._onDeliveryTimeChange();
        },

        _setDeliveryMethod: function (dlvryTimes) {
            if (dlvryTimes && dlvryTimes.length === 1
                && (dlvryTimes[0] == this.deliveryTimes.Continuous || dlvryTimes[0] == this.deliveryTimes.None)) {
                if (dlvryTimes[0] == this.deliveryTimes.Continuous) {//Continuous
                    this.$alertDeliveryMethod.find('input:eq(2)').prop('checked', true);
                }
                else {//None - Online Delivery
                    this.$alertDeliveryMethod.find('input:eq(0)').prop('checked', true);
                }
            }
            else {
                this.$alertDeliveryMethod.find('input:eq(1)').prop('checked', true); //Scheduled
                this._setDeliveryTime(dlvryTimes);
            }
            this._onDeliveryMethodChange();
        },

        _toggleSwitch: function ($elem, ON) {
            var $switch = $elem.children("span.switch");
            ON ? $switch.removeClass("on") : $switch.addClass("on");
            $switch.html($elem.find('> .text-behind > span:' + ($switch.hasClass("on") ? 'last' : 'first')).html());
        },

        _onDeliveryTimeChange: function () {
            if (this.$alertDelivery.find('span.checked').length == 0) {
                this.$alertEmail.attr("disabled", true);
                this.$alertFormatDD.attr("disabled", true);
                //This is to apply disabled look to the span element with selectbox plugin wraps the select element
                this.$alertFormatDD.parent().addClass("disabled-select");
            }
            else {
                this.$alertEmail.attr("disabled", false);
                this.$alertFormatDD.attr("disabled", false);
                //This is to remove disabled look to the span element with selectbox plugin wraps the select element
                this.$alertFormatDD.parent().removeClass("disabled-select");
            }
        },

        _onDeliveryMethodChange: function () {
            if (this.$alertDeliveryMethod.find('input:checked').val() == "scheduled") {
                this.$alertDelivery.show();
            }
            else if (this.$alertDeliveryMethod.find('input:checked').val() == "continuous") {
                this.$alertEmail.attr("disabled", false);
                this.$alertFormatDD.attr("disabled", false);
                //This is to remove disabled look to the span element with selectbox plugin wraps the select element
                this.$alertFormatDD.parent().removeClass("disabled-select");

            }
            else {
                this.$alertDelivery.hide();
                //Uncheck all the delivery times
                this._setDeliveryTime([]);
            }
        },

        _getNewsFilters: function () {
            var f = {};
            var nfArr = ['company', 'executive', 'author', 'industry', 'subject', 'region', 'source', 'keyword'];
            var $filterGroups = this.$filters.find(this.selectors.filterGroup);
            if ($filterGroups.length > 0) {
                var me = this, $filterGroup, $this, type, desc, category;
                $.each($filterGroups, function () {
                    $filterGroup = $(this);
                    category = $filterGroup.data("type");
                    if (category == 'keyword') {
                        f.keyword = [];
                        $.each($filterGroup.children(me.selectors.filterList).children(), function () {
                            f.keyword.push($.trim($(this).text()));
                        });
                    }
                    else {
                        f[category] = [];
                        $.each($filterGroup.children(me.selectors.filterList).children(), function () {
                            $this = $(this);
                            desc = $.trim($this.text());
                            f[category].push({ code: ($this.data("code") || desc), desc: desc });
                        });
                    }
                });
            }
            return f;
        },

        _getLanguageFilters: function () {
            var f = [];
            var langs = this.$filters.find(this.selectors.languageFilter).first().children();
            $.each(langs, function () {
                f.push($(this).data('code'));
            });
            return ((f.length == 1) && (f[0] == "alllang")) ? [] : f;
        },

        _getDeliveryTimes: function () {
            var dlvryTimes = [], $this, me = this;
            var dlvryMethod = this.$alertDeliveryMethod.find('input:checked').val();
            if (dlvryMethod == "scheduled") {
                this.$alertDelivery.find('span.checked').each(function () {
                    $this = $(this);
                    if ($this.hasClass('earlyMorning')) {
                        dlvryTimes.push(me.deliveryTimes.EarlyMorning);
                    }
                    else if ($this.hasClass('morning')) {
                        dlvryTimes.push(me.deliveryTimes.Morning);
                    }
                    else if ($this.hasClass('afternoon')) {
                        dlvryTimes.push(me.deliveryTimes.Afternoon);
                    }
                });
            }
            else if(dlvryMethod == "continuous"){
                dlvryTimes.push(me.deliveryTimes.Continuous);
            }
            else{
                dlvryTimes.push(me.deliveryTimes.None);
            }
            return dlvryTimes;
        },

        getAlertRequestObject: function () {
            var request = { isValid: false }, nf;

            var errorMsgs = [];

            var alertName = $.trim(this.$alertName.val());
            if (!alertName) { // Alert Name
                errorMsgs.push("<%= Token('enterAlertName') %>");
            }
            else if ($dj.hasIllegalChar(alertName)) {
                errorMsgs.push("<%=Token('illegalChar-1')%>" + " <>&#\\%+|");
            }

            var alertEmail = $.trim(this.$alertEmail.val());
            if (this.$alertEmail.is(":enabled")) {
                if (!alertEmail) { // Alert E-mail
                    errorMsgs.push("<%= Token('enterAlertEmail') %>");
                }
                else if (!$dj.validateEmail(alertEmail)) {
                    errorMsgs.push("<%= Token('enterValidAlertEmail') %>");
                }
            }

            //If delivery method is Scheduled and no delivery time is selected
            if ((this.$alertDeliveryMethod.find('input:checked').val() == "scheduled")
                && (this.$alertDelivery.find('span.checked').length == 0)) {
                errorMsgs.push("<%= Token('cmSelectDeliveryTime') %>");
            }

            //If search criteria is editable then we need to validate freetext and news filter
            if (this.options.searchCriteriaEditable) {
                nf = this._getNewsFilters();
                if (($.trim(this.$freeText.val()) == '')) {
                    if ($.isEmptyObject(nf)) {
                        errorMsgs.push("<%= Token('noSearchStrMsg')%>");
                    }
                    else {
                        var hasValidFilter = false;
                        $.each(nf, function (key, val) {
                            if (key != 'source' && val) {
                                hasValidFilter = true;
                                return false;
                            }
                        });
                        if (!hasValidFilter) {
                            errorMsgs.push("<%= Token('noSearchStrMsg')%>");
                        }
                    }
                }
            }

            if (errorMsgs.length == 0) {
                request.properties = {
                    alertId: this.data.properties.alertId,
                    alertName: this.$alertName.val(),
                    emailAddress: escape(alertEmail).replace(/\+/g, '%2B'),
                    documentFormat: this.$alertFormatDD.val(),
                    newDeliveryTimes: this._getDeliveryTimes(),
                    removeDuplicate: (this.$removeDuplicate.find("span.switch").hasClass("on")
                                                        ? this.removeDuplicate.Off : this.removeDuplicate.VirtuallyIdentical),
                    productType: this.data.properties.productType,
                    pressClipsOnly: !(this.$pressClips.find("span.switch").hasClass("on"))
                };

                //If the Alert is created using Simple search
                if (this.options.searchCriteriaEditable) {
                    request.searchQuery = request.searchQuery || {};
                    request.searchQuery.freeText = this.$freeText.val();
                    request.searchQuery.contentLanguages = this._getLanguageFilters();

                    request.searchQuery.newsFilters = nf;
                }
                else {
                    request.searchQuery = this.data.searchQuery;
                }
            }
            else {
                request.error = "- " + errorMsgs.join("\n- ");
            }

            request.isValid = (errorMsgs.length == 0);
            return request;
        },

        setFocusOnAlertName: function () {
            this.$alertName.focus();
            this.$alertFormatDD.change(); // Just to reset the dropdown width;
        },

        setData: function (data) {
            this.data = data || {};
            this.data.properties = this.data.properties || { pressClipsOnly: true };

            var d = this.data;
            var alertProperties = d.properties;

            this._bindSearchQuery(d.searchQuery);

            this.$alertName.val(alertProperties.alertName);
            if (alertProperties.emailAddress) {
                this.$alertEmail.val(unescape(alertProperties.emailAddress).replace(/%2B/g, '+'));
            }
            else {
                this.$alertEmail.val('');
            }
            this.$alertFormatDD.val(alertProperties.documentFormat).change();

            //This will take care of settting deliery times as well
            this._setDeliveryMethod(alertProperties.newDeliveryTimes || [this.deliveryTimes.None]);

            //Remove Duplicate
            this._toggleSwitch(this.$removeDuplicate, (alertProperties.removeDuplicate != this.removeDuplicate.Off));

            //Press Clippings Only
            if (this.options.pressClipsEnabled) {
                this._toggleSwitch(this.$pressClips, alertProperties.pressClipsOnly);
                this.$pressClips.closest('div.dj_form-field').show();
            }
            else {
                this.$pressClips.closest('div.dj_form-field').hide();
            }
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_AlertEditor', DJ.UI.AlertEditor);

    $dj.debug('Registered DJ.UI.AlertEditor (extends DJ.UI.Component)');

})(jQuery);