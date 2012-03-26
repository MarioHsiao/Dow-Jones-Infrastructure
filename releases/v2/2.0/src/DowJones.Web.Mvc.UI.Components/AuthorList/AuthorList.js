/*!
DJ.UI.AuthorList plugin
*/

(function ($) {

	DJ.UI.AuthorList = DJ.UI.Component.extend({

		selectors: {
			authorTable: ".dj_data_table-sorter.dj_data_table",
			authorTBody: ".dj_data_table-scroll",
			expandArrow: "div.expandable",
			checkboxAuthors: "td input:checkbox",
			theadSortable: "th.dj_sortable-table-header",
			currentSortBy: "#author_list_sort_by",
			currentSortOrder: "#author_list_sort_order",
			authorName: ".author-name-selector",
			authorArticles: ".author-articles-selector",
			authorOutlet: ".author-outlet-selector",
			selectedAuthorIds: "#selected_author_ids"
		},

		constants: {
			otherOutletRow: "outlet-other",
			hideClass: "hide",
			expandableClass: "expandable",
			expandedClass: "expanded"
		},

		events: {
			onOutletExpandArrowClick: 'onOutletExpandArrowClick.dj.AuthorList'
		},

		defaults: {
			debug: false,
			cssClass: 'AuthorList'
		},

		sortDirections: { ascending: "asc", descending: "desc" },

		init: function (element, meta) {
			var $meta = $.extend({ name: "AuthorList" }, meta);

			// Call the base constructor
			this._super(element, $meta);
		},

		_initializeElements: function (ctx) {
			this.$authorTable = ctx.find(this.selectors.authorTable);
			this.$authorTBody = ctx.find(this.selectors.authorTBody);
			this.$currentSortBy = ctx.find(this.selectors.currentSortBy);
			this.$currentSortOrder = ctx.find(this.selectors.currentSortOrder);
			this.$selectedAuthorIds = ctx.find(this.selectors.selectedAuthorIds);
		},

		_initializeEventHandlers: function () {
			this._super();
			var $container = this.$element, self = this;

			// Show/hide additional rows for author with more than one outlet;
			$container.delegate(self.selectors.expandArrow, "click", function () {
				var $this = $(this);
				var hasExpandableRows = false;
				var tr = $this.parent().parent();
				while (tr.next().hasClass(self.constants.otherOutletRow)) {
					tr.next().toggleClass(self.constants.hideClass);
					hasExpandableRows = true;
					tr = tr.next();
				}

				if (hasExpandableRows && $this.hasClass(self.constants.expandableClass)) {
					$this.toggleClass(self.constants.expandedClass);
				}

				return false;
			});

			// row checkbox on change; save author ids in a hidden input;
			$container.delegate(self.selectors.checkboxAuthors, "change", function () {
				var $this = $(this);
				var checked = $this.is(":checked");
				var id = $this.attr("authorlist-aid");
				var idarr = [];
				if (self.$selectedAuthorIds.val() != "") {
					idarr = self.$selectedAuthorIds.val().split(",");
				}
				var idx = $.inArray(id, idarr);
				if (checked == true && idx == -1) {
					idarr.push(id);
				}

				if (checked == false && idx > -1) {
					idarr.splice(idx, 1);
				}

				self.$selectedAuthorIds.val(idarr.join(","));
			});

			// THead sortable click;
			this.$authorTable.delegate(self.selectors.theadSortable, 'click', function () {
				var $this = $(this);
				var sortBy = $this.data('sort');
				var sortOrder = self.$currentSortOrder.val();
				var selectedIds = self.$selectedAuthorIds.val();
				if (self.$currentSortBy.val() == $this.data('sort')
					&& sortOrder == self.sortDirections.ascending) {
					sortOrder = self.sortDirections.descending;
				}
				else {
					sortOrder = self.sortDirections.ascending;
				}

				$dj.publish('sort.dj.AuthorList', {
					// by rss @ 20120119 begin
					pageSize: self.options.pageSize,
					// by rss @ 20120119 end
					sortBy: sortBy,
					sortOrder: sortOrder,
					selectedEntityIds: selectedIds
				});
				return false;
			});

			// Author name click;
			$container.delegate(self.selectors.authorName, "click", function () {
				var $this = $(this);
				var chk = $this.parent().parent().find("input:checkbox");
				var aid = chk.attr("authorlist-aid");
				var nnid = chk.attr("authorlist-nnid");
				$dj.publish("authorNameClick.dj.AuthorList", { authorId: aid, authorNnid: nnid });
			});

			// Author articles click;
			$container.delegate(self.selectors.authorArticles, "click", function () {
				var $this = $(this);
				var chk = $this.parent().parent().find("input:checkbox");
				var aid = chk.attr("authorlist-aid");
				var nnid = chk.attr("authorlist-nnid");
				$dj.publish("authorArticlesClick.dj.AuthorList", { authorId: aid, authorNnid: nnid });
			});

			// Author outlet name click;
			$container.delegate(self.selectors.authorOutlet, "click", function () {
				var $this = $(this);
				var oid = $this.attr("authorlist-outlet-id");
				$dj.publish("authorOutletClick.dj.AuthorList", { outletId: oid });
			});
		}
	});

	// Declare this class as a jQuery plugin
	$.plugin('dj_AuthorList', DJ.UI.AuthorList);
})(jQuery);