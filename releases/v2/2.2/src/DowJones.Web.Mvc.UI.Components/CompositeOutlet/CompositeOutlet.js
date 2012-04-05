/*!
 * CompositeOutlet
 */

(function ($) {

	DJ.UI.CompositeOutlet = DJ.UI.Component.extend({

		selectors: {

			currentSortBy: "#outlet_list_sort_by",
			currentSortOrder: "#outlet_list_sort_order",
			columnPreferences: ".dj_author-list-name_preferences-link a",
			selectedOutletIds: "#selected_outlet_ids",
			navigation: ".entity-list-nav",
			goto: ".entity-list-goto",
			gotoPage: ".entity-list-current-page",
			pager: ".entity-list-pager",
			pageSizeSelector: ".entity-list-select-page-size",
			// new from 2012-01-24; Checkbox ALL & gear menu.
			checkboxAll: "input:checkbox[name='dj_outlet-select-all']",
			checkboxOutlets: "td input:checkbox",
			gearMenu: "span.fi.fi_gear",
			actionsMenu: ".actionsMenu",
			unselectAllCheckboxes: ".dj_clear-all-btn",
			unselectAllCheckboxesConfirmed: ".dj_btn.dj_btn-green.export.dj_confirm-unselect-checkboxes"
		},

		constants: {
			addToContactListMenuItem: "div.label[data-action='contact-list']",
			printMenuItem: "div.label[data-action='print']",
			exportMenuItem: "div.label[data-action='export']",
			exportAllMenuItem: "div.label[data-action='export-all']",
			deleteMenuItem: "div.label[data-action='delete']",
			emailMenuItem: "div.label[data-action='email']",
			emailAllMenuItem: "div.label[data-action='email-all']",
			maxOutletsForActionNoEmail: 1000,
			maxOutletsForEmail: 300
		},

		events: {
			pagerClick: "pagerClick.dj.CompositeOutlet",
			columnPrefsClick: "columnPrefsClick.dj.CompositeOutlet"
		},

		// Default options
		defaults: {
			debug: false,
			cssClass: "CompositeOutlet"
		},

		init: function (element, meta) {
			var $meta = $.extend({ name: "CompositeOutlet" }, meta);

			// Call the base constructor
			this._super(element, $meta);
		},

		_initializeElements: function (ctx) {
			this.$currentSortBy = ctx.find(this.selectors.currentSortBy);
			this.$currentSortOrder = ctx.find(this.selectors.currentSortOrder);
			this.$selectedOutletIds = ctx.find(this.selectors.selectedOutletIds);

			this.$checkboxOutlets = ctx.find(this.selectors.checkboxOutlets);
			this.$gearMenu = ctx.find(this.selectors.gearMenu);
			this.$actionsMenu = ctx.find(this.selectors.actionsMenu);
		},

		_initializeEventHandlers: function () {
			var self = this;

			$dj.subscribe('entityRowSelect.dj.OutletList', function () {
				var idarr = [];
				if (self.$selectedOutletIds.val() != "") {
					idarr = self.$selectedOutletIds.val().split(",");
				}

				if (idarr.length < 2) {
					if ($(".dj_list-select-count").hasClass("hide") == false) {
						$(".dj_list-select-count").addClass("hide");
					}

					$(".dj_list-select-count span.count").html("");
				}
				else {
					if ($(".dj_list-select-count").hasClass("hide") == true) {
						$(".dj_list-select-count").removeClass("hide");
					}

					$(".dj_list-select-count span.count").html(idarr.length);
				}
			});

			// unselect all;
			this.$element.delegate(this.selectors.unselectAllCheckboxes, "click", function () {
				$.dj.modal({
					title: $("#dj_clear_selection .dj_modal-title").clone(true),
					content: $("#dj_clear_selection .dj_modal-content").clone(true),
					minWidth: 460,
					maxWidth: 460,
					minHeight: 180,
					maxHeight: 180,
					modalClass: 'dj_modal-comm-field',
					jscrollpaneEnabled: false
				});
				$(self.selectors.unselectAllCheckboxesConfirmed).click(function () {
					self.$selectedOutletIds.val("");
					self.$checkboxOutlets.attr("checked", false);
					$(self.selectors.checkboxAll).attr("checked", false);
					$dj.publish("entityRowSelect.dj.OutletList", {});
					$.modal.close();
					alert("<%= Token('cmalSelectionsHaveBeenCleared') %>.");
				});
			});

			// first | prev || next | last page navigation;
			this.$element.delegate(this.selectors.navigation, "click", function () {
				var sortBy = self.$currentSortBy.val();
				var sortOrder = self.$currentSortOrder.val();
				var selectedIds = self.$selectedOutletIds.val();
				var direction = $(this).attr("direction");
				var recordIndex = 0;
				switch (direction) {
					case "first":
						recordIndex = 0;
						break;
					case "prev":
						recordIndex = self.options.firstResultIndex - self.options.pageSize;
						break;
					case "next":
						recordIndex = self.options.lastResultIndex;
						break;
					case "last":
						var remainder = self.options.totalResultCount % self.options.pageSize;
						var quotient = (self.options.totalResultCount - remainder) / self.options.pageSize;
						recordIndex = quotient * self.options.pageSize;
						break;
				}

				self.publish(self.events.pagerClick, {
					pageSize: self.options.pageSize,
					index: recordIndex,
					sortBy: sortBy,
					sortOrder: sortOrder,
					selectedEntityIds: selectedIds
				});
				return false;
			});

			// Go to page navigation;
			this.$element.delegate(this.selectors.goto, "click", function () {
				var page = $(this).prev().val(); // must match input!
				if (isNaN(parseInt(page))) {
					alert("enter an integer number");
					return false;
				}
				if (page < 1 || page > self.options.totalPages) {
					alert("select page between 1 and " + self.options.totalPages);
					return false;
				}
				var sortBy = self.$currentSortBy.val();
				var sortOrder = self.$currentSortOrder.val();
				var selectedIds = self.$selectedOutletIds.val();
				var index = (page - 1) * self.options.pageSize;
				self.publish(self.events.pagerClick, {
					pageSize: self.options.pageSize,
					index: index,
					sortBy: sortBy,
					sortOrder: sortOrder,
					selectedEntityIds: selectedIds
				});
				return false;
			});

			// Google like page navigation;
			this.$element.delegate(this.selectors.pager, "click", function () {
				var page = $(this).html();
				if (isNaN(parseInt(page))) {
					alert("enter an integer number");
					return false;
				}
				var sortBy = self.$currentSortBy.val();
				var sortOrder = self.$currentSortOrder.val();
				var selectedIds = self.$selectedOutletIds.val();
				var index = (page - 1) * self.options.pageSize;
				self.publish(self.events.pagerClick, {
					pageSize: self.options.pageSize,
					index: index,
					sortBy: sortBy,
					sortOrder: sortOrder,
					selectedEntityIds: selectedIds
				});
				return false;
			});

			// Page size change; pagerClick.dj.CompositeOutlet is enough; no new event is needed;
			this.$element.delegate(this.selectors.pageSizeSelector, "change", function () {
				var newPageSize = $(this).val();
				var sortBy = self.$currentSortBy.val();
				var sortOrder = self.$currentSortOrder.val();
				var selectedIds = self.$selectedOutletIds.val();
				self.publish(self.events.pagerClick, {
					pageSize: newPageSize,
					index: 0,
					sortBy: sortBy,
					sortOrder: sortOrder,
					selectedEntityIds: selectedIds
				});
				return false;
			});

			// Click on preferences link;
			this.$element.delegate(this.selectors.columnPreferences, "click", function () {
				var selectedIds = self.$selectedOutletIds.val();
				self.publish(self.events.columnPrefsClick, { selectedEntityIds: selectedIds });
				return false;
			});

			// Check/uncheck all checkboxes;
			this.$element.delegate(this.selectors.checkboxAll, "change", function () {
				var $this = $(this);
				var checked = $this.is(":checked")
				self.$checkboxOutlets.attr("checked", checked);

				// outlet ids persistency;
				var idarr = [];
				if (self.$selectedOutletIds.val() != "") {
					idarr = self.$selectedOutletIds.val().split(",");
				}

				self.$checkboxOutlets.each(function () {
					var $chk = $(this);
					var id = $chk.attr("outletlist-aid");
					var idx = $.inArray(id, idarr);
					if (checked == true && idx == -1) {
						idarr.push(id);
					}

					if (checked == false && idx > -1) {
						idarr.splice(idx, 1);
					}
				});

				self.$selectedOutletIds.val(idarr.join(","));
				$dj.publish("entityRowSelect.dj.OutletList", {});
			});

			// Actions menu;
			this.$element.delegate(this.selectors.gearMenu, "click", function (e) {
				e.stopPropagation(); // Need to stop the event propagation;
				self.$onActionsClick(self, this);
			});

			// Actions menu item select;
			this.$actionsMenu.delegate('.label', 'click', function () {
				if ($(this).parent().hasClass("disabled")) return;
				var actionValue = $(this).data("action");

				if (actionValue == "export-all") {
					if (self.options.totalResultCount > self.constants.maxOutletsForActionNoEmail) {
						alert("<%= Token('cmalActionExportMaxEntity') %>");
						return;
					}

					$dj.publish('action.dj.CompositeOutlet', {
						action: actionValue,
						selectorType: self.options.allOutletsSelector.Type,
						selectorValue: self.options.allOutletsSelector.Value
					});
				}
				else if (actionValue == "email-all") {
					if (self.options.totalResultCount > self.constants.maxOutletsForEmail) {
						alert("<%= Token('cmalActionEmailMaxEntity') %>");
						return;
					}

					$dj.publish('action.dj.CompositeOutlet', {
						action: actionValue,
						selectorType: self.options.allOutletsSelector.Type,
						selectorValue: self.options.allOutletsSelector.Value
					});
				}
				else {
					var selectedIds = self.$selectedOutletIds.val();
					var idarr = [];
					if (selectedIds != "") {
						idarr = selectedIds.split(",");
					}

					if ((actionValue == "email" && idarr.length > self.constants.maxOutletsForEmail)
						|| idarr.length > self.constants.maxOutletsForActionNoEmail) {
						var msg = "";
						switch (actionValue) {
							case "contact-list": msg = "<%= Token('cmalActionOutletListMaxEntity') %>"; break;
							case "print": msg = "<%= Token('cmalActionPrintMaxEntity') %>"; break;
							case "export": msg = "<%= Token('cmalActionExportMaxEntity') %>"; break;
							case "delete": msg = "<%= Token('cmalActionDeleteMaxEntity') %>"; break;
							case "email": msg = "<%= Token('cmalActionEmailMaxEntity') %>"; break;
						}
						alert(msg);
						return;
					}

					$dj.publish('action.dj.CompositeOutlet', { action: actionValue, selectedEntityIds: selectedIds });
				}

			}).appendTo(document.body);
		},

		$onActionsClick: function (self, elem) {
			var idarr = [];
			if (self.$selectedOutletIds.val() != "") {
				idarr = self.$selectedOutletIds.val().split(",");
			}

			var outletsSelected = idarr.length;
			var currentPageSelected = self.$checkboxOutlets.filter(":checked").length;
			// show menu;
			var $elem = $(elem), elemOffset = $elem.offset();
			this.$gearMenu.addClass('active');
			this.$actionsMenu.css({
				top: $elem.parent().offset().top + $elem.parent().outerHeight() + "px",
				left: elemOffset.left + "px",
				position: "absolute"
			}).show();


			// Create alert menu item; appears if there is only one check box selected on the current page;
			if (currentPageSelected == 1) {
				this.$actionsMenu.find(self.constants.createAlertMenuItem).parent().removeClass("disabled");
			}
			else {
				this.$actionsMenu.find(self.constants.createAlertMenuItem).parent().addClass("disabled");
			}

			// other menu items (except create alert);
			// export all & email all are always active;
			// other are active only if there is at least one outlet selected; 
			if (outletsSelected == 0) { // only export-all and email-all are active;
				this.$actionsMenu.find(self.constants.addToContactListMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.printMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.exportMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.exportAllMenuItem).parent().removeClass("disabled");
				this.$actionsMenu.find(self.constants.deleteMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.emailMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.emailAllMenuItem).parent().removeClass("disabled");
			}
			else {
				this.$actionsMenu.find(self.constants.addToContactListMenuItem).parent().removeClass("disabled");
				this.$actionsMenu.find(self.constants.printMenuItem).parent().removeClass("disabled");
				this.$actionsMenu.find(self.constants.exportMenuItem).parent().removeClass("disabled");
				this.$actionsMenu.find(self.constants.exportAllMenuItem).parent().removeClass("disabled");
				this.$actionsMenu.find(self.constants.deleteMenuItem).parent().removeClass("disabled");
				this.$actionsMenu.find(self.constants.emailMenuItem).parent().removeClass("disabled");
				this.$actionsMenu.find(self.constants.emailAllMenuItem).parent().removeClass("disabled");
			}

			$(document).unbind('mousedown.Actions').bind('mousedown.Actions').click($dj.delegate(this, function () {
				this.$gearMenu.removeClass('active');
				this.$actionsMenu.hide();
				$(document).unbind('mousedown.Actions');
			}));
		}
	});

	// Declare this class as a jQuery plugin
	$.plugin('dj_CompositeOutlet', DJ.UI.CompositeOutlet);

})(jQuery);