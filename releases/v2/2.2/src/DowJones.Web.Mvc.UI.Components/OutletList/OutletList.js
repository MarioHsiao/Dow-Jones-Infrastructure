/*!
 * OutletList
 */

(function ($) {

	DJ.UI.OutletList = DJ.UI.Component.extend({

		selectors: {
			outletTable: ".dj_data_table-sorter.dj_data_table",
			outletTBody: ".dj_data_table-scroll",
			checkboxOutlets: "td input:checkbox",
			theadSortable: "th.dj_sortable-table-header",
			currentSortBy: "#outlet_list_sort_by",
			currentSortOrder: "#outlet_list_sort_order",
			outletName: ".outlet-selector",
			selectedOutletIds: "#selected_outlet_ids",
			expandHiddenCellItems: ".dj_show-hide-cell-items",
			hasMediaContact: ".dj_icon.dj_list-icon.dj_list-icon-has-contacts",
			noMediaContact: ".dj_list-icon-no-contacts"
		},

		constants: {
			hideClass: "hide",
			expandableClass: "expandable",
			expandedClass: "expanded",
			expandableCellItems: "div.dj_hidden-cell-item"
		},

		defaults: {
			debug: false,
			cssClass: 'OutletList'
		},

		events: {
			onOutletNameClick: "outletNameClick.dj.OutletList",
			onEntityRowSelect: "entityRowSelect.dj.OutletList",
			onHasMediaContactClick: 'onHasMediaContactClick.dj.OutletList',
			onNoMediaContactClick: 'onNoMediaContactClick.dj.OutletList'
		},

		sortDirections: { ascending: "asc", descending: "desc" },

		init: function (element, meta) {
			var $meta = $.extend({ name: "OutletList" }, meta);

			// Call the base constructor
			this._super(element, $meta);
		},

		_initializeElements: function (ctx) {
			this.$outletTable = ctx.find(this.selectors.outletTable);
			this.$outletTBody = ctx.find(this.selectors.outletTBody);
			this.$checkboxOutlets = ctx.find(this.selectors.checkboxOutlets);
			this.$currentSortBy = ctx.find(this.selectors.currentSortBy);
			this.$currentSortOrder = ctx.find(this.selectors.currentSortOrder);
			this.$selectedOutletIds = ctx.find(this.selectors.selectedOutletIds);
			this.$expandHiddenCellItems = ctx.find(this.selectors.expandHiddenCellItems);
		},

		_initializeEventHandlers: function () {
			this._super();
			var $container = this.$element, self = this;

			// row checkbox on change; save outlet ids in a hidden input;
			$container.delegate(self.selectors.checkboxOutlets, "change", function () {
				var $this = $(this);
				var checked = $this.is(":checked");
				var id = $this.attr("outletlist-aid");
				var idarr = [];
				if (self.$selectedOutletIds.val() != "") {
					idarr = self.$selectedOutletIds.val().split(",");
				}
				var idx = $.inArray(id, idarr);
				if (checked == true && idx == -1) {
					idarr.push(id);
				}

				if (checked == false && idx > -1) {
					idarr.splice(idx, 1);
				}

				self.$selectedOutletIds.val(idarr.join(","));
				$dj.publish(self.events.onEntityRowSelect, {});
			});

			// Show/hide hidden cell items;
			$container.delegate(self.selectors.expandHiddenCellItems, "click", function () {
				var $this = $(this);
				var td = $this.parentsUntil("tr");
				td.find(self.constants.expandableCellItems).toggleClass("hide");

				if ($this.attr("more") == "true") {
					$this.attr("more", "false");
					$this.html("<%= Token('cmalHideCellItems') %>");
				}
				else {
					$this.attr("more", "true");
					$this.html("<%= Token('cmalShowCellItems') %>");
				}
				return false;
			});

			// THead sortable click;
			this.$outletTable.delegate(self.selectors.theadSortable, 'click', function () {
				var $this = $(this);
				var sortBy = $this.data('sort');
				var sortOrder = self.$currentSortOrder.val();
				var selectedIds = self.$selectedOutletIds.val();
				if (self.$currentSortBy.val() == $this.data('sort')
					&& sortOrder == self.sortDirections.ascending) {
					sortOrder = self.sortDirections.descending;
				}
				else {
					sortOrder = self.sortDirections.ascending;
				}

				$dj.publish('sort.dj.OutletList', {
					pageSize: self.options.pageSize,
					sortBy: sortBy,
					sortOrder: sortOrder,
					selectedEntityIds: selectedIds
				});
				return false;
			});

			// Outlet name click;
			$container.delegate(self.selectors.outletName, "click", function () {
				var $this = $(this);
				var chk = $this.parent().parent().find("input:checkbox");
				var oid = chk.attr("outletlist-aid");
				$dj.publish(self.events.onOutletNameClick, { outletId: oid });
			});

			$container.delegate(self.selectors.hasMediaContact, "click", function () {
				var $this = $(this);
				var outletId = $this.parentsUntil("tbody").find("input:checkbox").attr("outletlist-aid");
				if (outletId) {
					$dj.publish(self.events.onHasMediaContactClick, { outletId: outletId });
				}
			});

			$container.delegate(self.selectors.noMediaContact, "click", function () {
				var $this = $(this);
				var outletId = $this.parentsUntil("tbody").find("input:checkbox").attr("outletlist-aid");
				if (outletId) {
					$dj.publish(self.events.onNoMediaContactClick, { outletId: outletId });
				}
			});
		}
	});

	// Declare this class as a jQuery plugin
	$.plugin('dj_OutletList', DJ.UI.OutletList);

})(jQuery);