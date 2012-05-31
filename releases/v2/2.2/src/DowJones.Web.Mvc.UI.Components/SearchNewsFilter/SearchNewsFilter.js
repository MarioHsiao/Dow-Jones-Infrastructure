/*
*  Search Categories Lookup Control
*/

(function ($) {

    DJ.UI.SearchNewsFilter = DJ.UI.Component.extend({

        selectors: {
            filterPill: 'li.dj_pill',
            filterRemove: '.remove',
            filterGroup: '.filter-group',
            filterList: '.filter-list',
            pillList: '.dj_pill-list'
        },

        events: {
            onFilterRemove: 'onFilterRemove.dj.SearchNewsFilter',
            onAllFiltersRemoved: 'onAllFiltersRemoved.dj.SearchNewsFilter'
        },

        filterCategory: ['unknown', 'company', 'executive', 'author', 'industry', 'subject', 'region', 'source', 'dateRange', 'keyword'],

        init: function (element, meta) {
            var $meta = $.extend({ name: "SearchNewsFilter" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this.setData();
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnFilterRemove: $dj.delegate(this, this._onFilterRemove)
            });
        },

        _initializeControls: function () {


        },

        _initializeEventHandlers: function () {
            this._initializeControls();

            var me = this;

            this.$element.delegate(this.selectors.filterRemove, 'click', function () {
                me._onFilterRemove(this);
            });
        },

        _onFilterRemove: function (elem) {
            var me = this, $elem = $(elem), $li = $elem.closest(this.selectors.filterPill), code = $li.data('code'),
            desc = $.trim($elem.prev().text()), cat = $li.data('type');

            var $filterList = $elem.closest(this.selectors.filterList);
            var $pillList = $filterList.closest(this.selectors.pillList);

            //Remove the pill
            $li.remove();

            this.publish(this.events.onFilterRemove, {
                component: this,
                filter: { Code: code, Name: desc, Category: cat }
            });

            //Remove the filterGroup if all filters are removed
            if ($filterList.children().length == 0) {
                $filterList.closest(this.selectors.filterGroup).remove();
            }

            //publish allFilterRemoved if all the filter groups are removed
            if ($pillList.children().length == 0) {
                this.publish(this.events.onAllFiltersRemoved, { component: this });
            }
        },

        removeSocialMediaFilters: function () {
            var me = this, $filterGroups = this.$element.children().children(this.selectors.filterGroup);
            var index;
            if ($filterGroups.length > 0) {
                $.each(["industry", "region", "subject"], function (i, val) {
                    index = $.inArray(val, me.filterCategory);
                    if (index != -1) {
                        $filterGroups.filter("[data-type=" + index + "]").remove();
                    }
                });

                //publish allFilterRemoved if all the filter groups are removed
                if (this.$element.children().children(this.selectors.filterGroup).length == 0) {
                    this.publish(this.events.onAllFiltersRemoved, { component: this });
                }
            }
        },

        removeAllFilters: function () {
            this.$element.children().remove();
            this.publish(this.events.onAllFiltersRemoved, { component: this });
        },

        getFilters: function () {
            var f = {};
            var $filterGroups = this.$element.children().children(this.selectors.filterGroup);
            if ($filterGroups.length > 0) {
                var me = this, $filterGroup, $this, type, desc, category;
                $.each($filterGroups, function () {
                    $filterGroup = $(this);
                    category = me.filterCategory[$filterGroup.data("type")];
                    if (category == 'keyword') {
                        f.keyword = [];
                        $.each($filterGroup.children(me.selectors.filterList).children(), function () {
                            f.keyword.push($.trim($(this).text()));
                        });
                    }
                    else if (category == 'dateRange') {
                        var $item = $filterGroup.children(me.selectors.filterList).children();
                        f.dateRange = { code: $item.data("code"), desc: $.trim($item.text()) };
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

        setData: function (data) {
            if (data)
                this.data = data;

            //ToDo: Implement client side data binding
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_SearchNewsFilter', DJ.UI.SearchNewsFilter);

    $dj.debug('Registered DJ.UI.SearchNewsFilter (extends DJ.UI.Component)');

})(jQuery);