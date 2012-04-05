/*!
* CompositeAuthor
*/

(function ($) {

	DJ.UI.CompositeAuthor = DJ.UI.CompositeComponent.extend({
		selectors: {
			currentSortBy: "#author_list_sort_by",
			currentSortOrder: "#author_list_sort_order",
			selectedAuthorIds: "#selected_author_ids",
			columnPreferences: ".dj_author-list-name_preferences-link a",
			navigation: ".entity-list-nav",
			goto: ".entity-list-goto",
			gotoPage: ".entity-list-current-page",
			pager: ".entity-list-pager",
			pageSizeSelector: ".entity-list-select-page-size",
			checkboxAll: "input:checkbox[name='dj_author-select-all']",
			checkboxAuthors: "td input:checkbox",
			gearMenu: "span.fi.fi_gear",
			actionsMenu: ".actionsMenu",
			selectedAuthorIds: "#selected_author_ids",
			unselectAllCheckboxes: ".dj_clear-all-btn",
			unselectAllCheckboxesConfirmed: ".dj_btn.dj_btn-green.export.dj_confirm-unselect-checkboxes"
		},

		constants: {
			addToContactListMenuItem: "div.label[data-action='contact-list']",
			createAlertMenuItem: "div.label[data-action='create-alert']",
			printMenuItem: "div.label[data-action='print']",
			exportMenuItem: "div.label[data-action='export']",
			exportAllMenuItem: "div.label[data-action='export-all']",
			deleteMenuItem: "div.label[data-action='delete']",
			emailMenuItem: "div.label[data-action='email']",
			emailAllMenuItem: "div.label[data-action='email-all']",
			createActivityMenuItem: "div.label[data-action='create-activity']",
			createBriefingBookMenuItem: "div.label[data-action='create-briefing-book']",
			maxAuthorsForActionNoEmail: 1000,
			maxAuthorsForEmail: 300,
			maxAuthorsForAlert: 300,
			maxAuthorsForBriefingBook: 10
		},

		events: {
			pagerClick: "pagerClick.dj.CompositeAuthor",
			columnPrefsClick: "columnPrefsClick.dj.CompositeAuthor"
		},

		// Default options
		defaults: {
			debug: false,
			cssClass: "CompositeAuthor"
		},

		init: function (element, meta) {
			var $meta = $.extend({ name: "CompositeAuthor" }, meta);

			// Call the base constructor
			this._super(element, $meta);
		},

		_initializeElements: function (ctx) {
			this.$currentSortBy = ctx.find(this.selectors.currentSortBy);
			this.$currentSortOrder = ctx.find(this.selectors.currentSortOrder);
			this.$selectedAuthorIds = ctx.find(this.selectors.selectedAuthorIds);
			this.$gotoPage = ctx.find(this.selectors.gotoPage);
			this.$checkboxAuthors = ctx.find(this.selectors.checkboxAuthors);
			this.$gearMenu = ctx.find(this.selectors.gearMenu);
			this.$actionsMenu = ctx.find(this.selectors.actionsMenu);
			this.$selectedAuthorIds = ctx.find(this.selectors.selectedAuthorIds);
		},

		_initializeEventHandlers: function () {
			var self = this;

			$dj.subscribe('entityRowSelect.dj.AuthorList', function () {
				var idarr = [];
				if (self.$selectedAuthorIds.val() != "") {
					idarr = self.$selectedAuthorIds.val().split(",");
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
					self.$selectedAuthorIds.val("");
					self.$checkboxAuthors.attr("checked", false);
					$(self.selectors.checkboxAll).attr("checked", false);
					$dj.publish("entityRowSelect.dj.AuthorList", {});
					$.modal.close();
					alert("<%= Token('cmalSelectionsHaveBeenCleared') %>.");
				});
			});

			// first | prev || next | last page navigation;
			this.$element.delegate(this.selectors.navigation, "click", function () {
				var sortBy = self.$currentSortBy.val();
				var sortOrder = self.$currentSortOrder.val();
				var selectedIds = self.$selectedAuthorIds.val();
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
				var selectedIds = self.$selectedAuthorIds.val();
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
				var selectedIds = self.$selectedAuthorIds.val();
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

			// Page size change; pagerClick.dj.CompositeAuthor is enough; no new event is needed;
			this.$element.delegate(this.selectors.pageSizeSelector, "change", function () {
				var newPageSize = $(this).val();
				var sortBy = self.$currentSortBy.val();
				var sortOrder = self.$currentSortOrder.val();
				var selectedIds = self.$selectedAuthorIds.val();
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
				var selectedIds = self.$selectedAuthorIds.val();
				self.publish(self.events.columnPrefsClick, { selectedEntityIds: selectedIds });
				return false;
			});

			// BEGIN CHECKBOX ALL & GEAR MENU //
			// Check/uncheck all checkboxes;
			this.$element.delegate(this.selectors.checkboxAll, "change", function () {
				var $this = $(this);
				var checked = $this.is(":checked")
				self.$checkboxAuthors.attr("checked", checked);

				// author ids persistency;
				var idarr = [];
				if (self.$selectedAuthorIds.val() != "") {
					idarr = self.$selectedAuthorIds.val().split(",");
				}

				self.$checkboxAuthors.each(function () {
					var $chk = $(this);
					var id = $chk.attr("authorlist-aid");
					var idx = $.inArray(id, idarr);
					if (checked == true && idx == -1) {
						idarr.push(id);
					}

					if (checked == false && idx > -1) {
						idarr.splice(idx, 1);
					}
				});

				self.$selectedAuthorIds.val(idarr.join(","));
				$dj.publish("entityRowSelect.dj.AuthorList", {});
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
					if (self.options.totalResultCount > self.constants.maxAuthorsForActionNoEmail) {
						alert("<%= Token('cmalActionExportMaxEntity') %>");
						return;
					}

					$dj.publish('action.dj.CompositeAuthor', {
						action: actionValue,
						selectorType: self.options.allAuthorsSelector.Type,
						selectorValue: self.options.allAuthorsSelector.Value
					});
				}
				else if (actionValue == "email-all") {
					if (self.options.totalResultCount > self.constants.maxAuthorsForEmail) {
						alert("<%= Token('cmalActionEmailMaxEntity') %>");
						return;
					}

					$dj.publish('action.dj.CompositeAuthor', {
						action: actionValue,
						selectorType: self.options.allAuthorsSelector.Type,
						selectorValue: self.options.allAuthorsSelector.Value
					});
				}
				else if (actionValue == "create-alert") {
					var selectedIds = self.$selectedAuthorIds.val();
					var idarr = [];
					if (selectedIds != "") {
						idarr = selectedIds.split(",");
					}
					if (actionValue == "create-alert" && idarr.length > self.constants.maxAuthorsForAlert) {
						var msg = "<%= Token('cmalActionContactListMaxAlert') %>";
						alert(msg);
						return;
					}
					$dj.publish('action.dj.CompositeAuthor', {
						action: actionValue,
						selectedEntityIds: selectedIds
					});
				}
				else if (actionValue == "create-activity") { // only for current page;
					var chk = self.$checkboxAuthors.filter(":checked");
					var id = chk.attr("authorlist-aid");
					$dj.publish('action.dj.CompositeAuthor', {
						action: actionValue,
						selectedAuthorId: id,
						selectedEntityIds: "0,0"
					});
				}
				else if (actionValue == "create-briefing-book") {
					var selectedIds = self.$selectedAuthorIds.val();
					var idarr = [];
					if (selectedIds != "") {
						idarr = selectedIds.split(",");
					}
					if (idarr.length > self.constants.maxAuthorsForBriefingBook) {
						var msg = "<%= Token('cmalActionBriefingBookMaxEntity') %>";
						alert(msg);
						return;
					}
					$dj.publish('action.dj.CompositeAuthor', {
						action: actionValue,
						selectedEntityIds: selectedIds
					});
				}
				else {
					var selectedIds = self.$selectedAuthorIds.val();
					var idarr = [];
					if (selectedIds != "") {
						idarr = selectedIds.split(",");
					}

					if ((actionValue == "email" && idarr.length > self.constants.maxAuthorsForEmail)
						|| idarr.length > self.constants.maxAuthorsForActionNoEmail) {
						var msg = "";
						switch (actionValue) {
							case "contact-list": msg = "<%= Token('cmalActionContactListMaxEntity') %>"; break;
							case "print": msg = "<%= Token('cmalActionPrintMaxEntity') %>"; break;
							case "export": msg = "<%= Token('cmalActionExportMaxEntity') %>"; break;
							case "delete": msg = "<%= Token('cmalActionDeleteMaxEntity') %>"; break;
							case "email": msg = "<%= Token('cmalActionEmailMaxEntity') %>"; break;
						}
						alert(msg);
						return;
					}

					$dj.publish('action.dj.CompositeAuthor', { action: actionValue, selectedEntityIds: selectedIds });
				}
			}).appendTo(document.body);
			// END CHECKBOX ALL & GEAR MENU //
		},

		$onActionsClick: function (self, elem) {
			var idarr = [];
			if (self.$selectedAuthorIds.val() != "") {
				idarr = self.$selectedAuthorIds.val().split(",");
			}

			var authorsSelected = idarr.length;
			var currentPageSelected = self.$checkboxAuthors.filter(":checked").length;
			// show menu;
			var $elem = $(elem), elemOffset = $elem.offset();
			this.$gearMenu.addClass('active');
			this.$actionsMenu.css({
				top: $elem.parent().offset().top + $elem.parent().outerHeight() + "px",
				left: elemOffset.left + "px",
				position: "absolute"
			}).show();

			// Create activity menu item; appears if there is only one check box selected on the current page;
			if (currentPageSelected == 1) {
				this.$actionsMenu.find(self.constants.createActivityMenuItem).parent().removeClass("disabled");
			}
			else {
				this.$actionsMenu.find(self.constants.createActivityMenuItem).parent().addClass("disabled");
			}

			// other menu items (except create alert);
			// export all & email all are always active;
			// other are active only if there is at least one author selected;
			if (authorsSelected == 0 || authorsSelected > 10) {
				this.$actionsMenu.find(self.constants.createBriefingBookMenuItem).parent().addClass("disabled");
			}
			else {
				this.$actionsMenu.find(self.constants.createBriefingBookMenuItem).parent().removeClass("disabled");
			}

			if (authorsSelected == 0) { // only export-all and email-all are active;

				this.$actionsMenu.find(self.constants.createAlertMenuItem).parent().addClass("disabled");

				this.$actionsMenu.find(self.constants.addToContactListMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.printMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.exportMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.exportAllMenuItem).parent().removeClass("disabled");
				this.$actionsMenu.find(self.constants.deleteMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.emailMenuItem).parent().addClass("disabled");
				this.$actionsMenu.find(self.constants.emailAllMenuItem).parent().removeClass("disabled");
			}
			else {

				this.$actionsMenu.find(self.constants.createAlertMenuItem).parent().removeClass("disabled");

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
	$.plugin("dj_CompositeAuthor", DJ.UI.CompositeAuthor);

})(jQuery);