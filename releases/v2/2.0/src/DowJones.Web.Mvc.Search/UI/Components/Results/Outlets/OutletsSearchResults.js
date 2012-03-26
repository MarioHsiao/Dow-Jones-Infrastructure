(function ($) {

	DJ.UI.OutletsSearchResults = DJ.UI.Component.extend({
		selectors: {
			pageSize: "#outlet-list-pageSize",
			firstIndex: "#outlet-list-firstIndex",
			sortBy: "#outlet-list-sortBy",
			sortOrder: "#outlet-list-sortOrder",
			selectedEntityIds: "#outlet-list-selectedEntityIds"
		},

		init: function (element, meta) {
			this._super(element, $.extend({ name: "OutletsSearchResults" }, meta));
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
			$dj.subscribe('pagerClick.dj.CompositeOutlet', this._delegates.OnPagerClick);
			$dj.subscribe('sort.dj.OutletList', this._delegates.OnSortClick);
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
	$.plugin('dj_OutletsSearchResults', DJ.UI.OutletsSearchResults);

	$dj.debug('Registered DJ.UI.OutletsSearchResults (extends DJ.UI.Component)');

})(jQuery);

