/*
*  Search Categories Lookup Control
*/

(function ($) {

    DJ.UI.TopicEditor = DJ.UI.Component.extend({

        selectors: {
            filters: '.topic-filters',
            form: '.save-as-topic-form',
            name: '.topic-name',
            category: '.topic-category',
            color: '.topic-color',
            customColor: '.custom-color',
            colorPickerTrigger: '.custom-color-picker',
            colorPickerHolder: '.color-picker-holder',
            description: '.topic-description',
            footer: '.footer',
            cancelBtn: '.dj_btn-drk-gray',
            saveBtn: '.dj_btn-blue',
            freeText: '.freeText',
            filterPill: '.dj_pill',
            filterRemove: '.remove',
            filtersWrap: '.filters-wrap',
            filterGroup: '.filter-group',
            filterList: '.filter-list',
            pillList: '.dj_pill-list'
        },

        events: {
            onSaveClick: 'onSaveClick.dj.TopicEditor',
            onCancelClick: 'onCancelClick.dj.TopicEditor'
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

            keyword: {
                text: "<%= Token('keywords') %>"
            }
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "TopicEditor" }, meta);

            this._topicDescMaxLen = 255;
            // Call the base constructor
            this._super(element, $meta);

            this.setData(this.data);
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {

        });
    },

    _initializeControls: function () {

        //Set the main template
        if (this.$element.children().length == 0) {
            this.$element.append(this.templates.main());
        }

        this.$topicForm = this.$element.children(this.selectors.form).children("fieldset");

        this.$freeText = this.$topicForm.children(this.selectors.freeText).children("p");
        this.$filters = this.$topicForm.children(this.selectors.filters);
        this.$topicName = this.$topicForm.children(this.selectors.name).children("input");
        this.$topicCategoryDD = this.$topicForm.children(this.selectors.category).children("select");

        var $topicColor = this.$topicForm.children(this.selectors.color);
        this.$topicColor = $topicColor.children(this.selectors.customColor);
        this.$colorPickerTrigger = $topicColor.children(this.selectors.colorPickerTrigger);
        this.$colorPickerHolder = $topicColor.children(this.selectors.colorPickerHolder);

        this.$topicDescription = this.$topicForm.children(this.selectors.description).children("textarea");

        var $footer = this.$element.children(this.selectors.footer);
        this.$saveBtn = $footer.children(this.selectors.saveBtn);
        this.$cancelBtn = $footer.children(this.selectors.cancelBtn);

    },

    _initializeEventHandlers: function () {

        this._initializeControls();

        var me = this;

        this.$colorPickerHolder.ColorPicker({
            flat: true,
            onSubmit: function (hsb, hex, rgb) {
                me._setCustomColor("#" + hex);
                me.$colorPickerTrigger.click();
            }
        }).click(function (e) {
            e.stopPropagation();
        });

        this.$colorPickerTrigger.bind('click', function (e) {
            if (!me._colorPickerLeftSet) {
                me.$colorPickerHolder.css("left", $(this).position().left + parseInt($(this).css("marginLeft"), 10));
                me._colorPickerLeftSet = true;
            }
            me.$colorPickerHolder.ColorPickerSetColor(me._topicColor).slideToggle(400, function () {
                if ($(this).is(":visible")) {
                    $(document).unbind('click.topicEditor').bind('click.topicEditor', function () {
                        if (me.$colorPickerHolder.is(":visible")) {
                            me.$colorPickerHolder.slideUp(400);
                            $(document).unbind('click.topicEditor');
                        }
                    });
                }
                else {
                    $(document).unbind('click.topicEditor');
                }
            });
            e.stopPropagation();
        });

        this.$cancelBtn.unbind('click').click(function () {
            me.publish(me.events.onCancelClick, me);
        });

        this.$saveBtn.unbind('click').click(function () {
            me.publish(me.events.onSaveClick, me.getTopicRequestObject());
        });

        //Bind topic Categories
        this._bindTopicCategories(this.data.topicCategories);
        this.$topicCategoryDD.selectbox();

        //News filter remove handler
        this.$filters.delegate(this.selectors.filterRemove, 'click', function () {
            var $elem = $(this), $li = $elem.closest(me.selectors.filterPill), code = $li.data('code'),
                desc = $.trim($elem.prev().text());

            var $filterList = $elem.closest(me.selectors.filterList);
            var $pillList = $filterList.closest(me.selectors.pillList);

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
        });

        //Topic Description max length
        this.$topicDescription.bind('keypress focus blur paste', function () {
            if (this.value.length > me._topicDescMaxLen) {
                this.value = this.value.substring(0, me._topicDescMaxLen);
                return false;
            }
        });
    },

    _bindSearchQuery: function (searchQuery) {
        if (searchQuery) {

            if (this.options.searchCriteriaEditable) {//Topic created using Simple search
                //Show textbox for free text
                if (!this.$freeText.is('input')) {
                    this.$freeText.append("<input type='text' maxlength='500' />");
                    this.$freeText = this.$freeText.find('input');
                }
                this.$freeText.val(searchQuery.freeText).closest('div').show();

            }
            else { // Topic created using Advanced search
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
        if (searchQuery && (searchQuery.freeText || searchQuery.filters || searchQuery.newsFilters)) {
            this.$filters.html(this.templates.filters(searchQuery)).show();

            //Hide the filters wrapper if no filters are present
            $.each(this.$filters.children(), function () {
                var $this = $(this);
                if ($this.children('ul').children().length == 0) {
                    $this.hide();
                }
            });
        }
        else {
            this.$filters.hide();
        }
    },

    _bindTopicCategories: function (cat) {
        this.$topicCategoryDD.empty();
        this.$topicCategoryDD.append('<option Data=""><%= Token("selectCategory") %></option>');
        if (cat) {
            for (var i = 0; i < cat.length; i++) {
                this.$topicCategoryDD.append('<option Data="' + cat[i].code + '">' + cat[i].desc + '</option>');
            }
        }
    },

    _getRandomChartColor: function () {
        return '#' + Math.floor(Math.random() * 16777215).toString(16);
    },

    _setCustomColor: function (color) {
        this.$topicColor.css("backgroundColor", color);
        this._topicColor = color;
    },

    _getNewsFilters: function () {
        var f = {};
        var nfArr = ['company', 'executive', 'author', 'industry', 'subject', 'region', 'keyword'];
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
                        f[category].push({ code: ($this.data("code") || desc), desc: desc, codeType: ($this.data("codetype") || '') });
                    });
                }
            });
        }
        return f;
    },

    getTopicRequestObject: function () {
        var request = { isValid: false };

        var errorMsgs = [];
        var topicName = $.trim(this.$topicName.val());

        if (!topicName) {//Topic Name
            errorMsgs.push("<%= Token('enterTopicName') %>");
        }
        else if ($dj.hasIllegalChar(topicName)) {
            //errorMsgs.push("<%= Token('illegalCharsMessage') %>");
            errorMsgs.push("<%=Token('illegalChar-1')%>" + " <>&#\\%+|");
        }

        if (!this.$topicCategoryDD.val()) {//Topic Category
            errorMsgs.push("<%= Token('selectTopicCategory') %>");
        }

        //If search criteria is editable then we need to validate freetext and news filter
        if (this.options.searchCriteriaEditable) {
            nf = this._getNewsFilters();
            if (($.trim(this.$freeText.val()) == '') && $.isEmptyObject(nf)) {
                errorMsgs.push("<%= Token('noSearchStrMsg')%>");
            }
        }

        if (errorMsgs.length == 0) {
            request.properties = {
                topicId: this.data.properties.topicId,
                topicName: this.$topicName.val(),
                chartColor: this._topicColor,
                topicCategory: this.$topicCategoryDD.val(),
                description: this.$topicDescription.val()
            }

            //If the Alert is created using Simple search
            if (this.options.searchCriteriaEditable) {
                request.searchQuery = request.searchQuery || {};
                request.searchQuery.freeText = this.$freeText.val();
                request.searchQuery.newsFilters = nf;
            }
            else {
                request.searchQuery = this.data.searchQuery;
            }
        }
        else {
            request.error = errorMsgs.join("\n- ");
        }

        request.isValid = (errorMsgs.length == 0);
        return request;
    },

    setFocusOnTopicName: function () {
        this.$topicName.focus();
        this.$topicCategoryDD.change(); //Just to reset the category dropdown width
    },

    setData: function (data) {
        this.data = data || {};
        this.data.properties = this.data.properties || {};

        var d = this.data;
        var topicProperties = d.properties;

        if (d.topicCategories) {
            this._bindTopicCategories(d.topicCategories);
        }

        this._bindSearchQuery(d.searchQuery);

        this.$topicName.val(topicProperties.topicName);
        this.$topicCategoryDD.val(topicProperties.topicCategory).change();
        this._setCustomColor(topicProperties.chartColor || this._getRandomChartColor());
        this.$topicDescription.val(topicProperties.description);
    }
});

// Declare this class as a jQuery plugin
$.plugin('dj_TopicEditor', DJ.UI.TopicEditor);

$dj.debug('Registered DJ.UI.TopicEditor (extends DJ.UI.Component)');

})(jQuery);