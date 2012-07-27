(function ($) {

    DJ.UI.AuthorsSearchResults = DJ.UI.Component.extend({
        selectors: {
            pageSize: "#author-list-pageSize",
            firstIndex: "#author-list-firstIndex",
            sortBy: "#author-list-sortBy",
            sortOrder: "#author-list-sortOrder",
            selectedEntityIds: "#author-list-selectedEntityIds"
        },


        init: function (element, meta) {
            this._super(element, $.extend({ name: "AuthorsSearchResults" }, meta));
        },

        _initializeDelegates: function () {
            this._super();
            this._delegates = $.extend(this._delegates, {
                OnPagerClick: $dj.delegate(this, this._onPagerClick),
                OnSortClick: $dj.delegate(this, this._onSortClick)
            });
        },

        _initializeElements: function () {
            this.$pageSize = $(this.selectors.pageSize, this.$element);
            this.$firstIndex = $(this.selectors.firstIndex, this.$element);
            this.$sortBy = $(this.selectors.sortBy, this.$element);
            this.$sortOrder = $(this.selectors.sortOrder, this.$element);
            this.$selectedEntityIds = $(this.selectors.selectedEntityIds, this.$element);
        },

        _initializeEventHandlers: function () {
            $dj.subscribe('pagerClick.dj.CompositeAuthor', this._delegates.OnPagerClick);
            $dj.subscribe('sort.dj.AuthorList', this._delegates.OnSortClick);
        },

        _onPagerClick: function (args) {
            this.$pageSize.val(args.pageSize);
            this.$firstIndex.val(args.index);
            this.$sortBy.val(args.sortBy);
            this.$sortOrder.val(args.sortOrder);
            this.$selectedEntityIds.val(args.selectedEntityIds);

            $dj.publish('pagerClick.dj.SearchResults');
        },

        _onSortClick: function (args) {
            this.$pageSize.val(args.pageSize);
            this.$firstIndex.val(0);
            this.$sortBy.val(args.sortBy);
            this.$sortOrder.val(args.sortOrder);
            this.$selectedEntityIds.val(args.selectedEntityIds);

            $dj.publish('pagerClick.dj.SearchResults');
        },

        EOF: true
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_AuthorsSearchResults', DJ.UI.AuthorsSearchResults);

    $dj.debug('Registered DJ.UI.AuthorsSearchResults (extends DJ.UI.Component)');

})(jQuery);