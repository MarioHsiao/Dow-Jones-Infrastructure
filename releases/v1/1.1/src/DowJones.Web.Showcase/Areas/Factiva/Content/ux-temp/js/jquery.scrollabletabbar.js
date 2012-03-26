/*
 * jQuery scrollableTabBar
 * 
 * Author: Philippe Arcand
 * Copyright 2011 Dow Jones & Company Inc.
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 *	 jquery.menu.js
 *
 */





 (function ($, undefined) {
     var tabDrag = false;
     var mouseDown = false;
     var mouseX = 0;
     var mouseY = 0;
    // alert($('#tabCount').val());
   // alert($('#isSubscribeOn').val());
     //Whether to show add tab or not 

     var actionTabsArray = new Array();
     function labelActionItems(itemIconClass, itemlabel, itemOnMouseDown) { this.iconClass = itemIconClass; this.label = itemlabel; this.onMouseDown = itemOnMouseDown; }
     if ($('#isSubscribeOn').val()=="False") {
         actionTabsArray[actionTabsArray.length] = new labelActionItems('fi_d-double-arrow', '', function (e) { var self = $(e.currentTarget).parent().data('pluginInstance'); self.attachTabsListMenu($(e.currentTarget).find('.fi')) });

         if ($('#tabCount').val() == 1) {
                    actionTabsArray[actionTabsArray.length] = new labelActionItems('', "<%=Token('addPage')%>", function (e) { e.preventDefault(); window.location.href = $('#pathname').val() + "Pages/Add"; });
           }
         else {
                 actionTabsArray[actionTabsArray.length] = new labelActionItems('fi_circle-plus', '', function (e) { e.preventDefault(); window.location.href = $('#pathname').val() + "Pages/Add"; });
              }
      

     }
     else {
         actionTabsArray[actionTabsArray.length] = new labelActionItems('fi_d-double-arrow', '', function (e) { var self = $(e.currentTarget).parent().data('pluginInstance'); self.attachTabsListMenu($(e.currentTarget).find('.fi')) });
     }


     $.widget("ui.scrollableTabBar", {
         tabCount: 0,
         options: {
             dragScrollSpeed: 5, 					// The Scroll Speed when you Drag a Tab
             selectedTabGutter: 70, 				// Left/Right padding value (scrolling)
             activeTabIndex: 0, 					// The Index of the Active Tab
             tabs: [], 							// Tabs Object
             /**
             * Tabs structure example:
             *	{
             *		label: 'A Tab',					// Text label to be displayed in the tab
             *  	tabClass: 'special-tab',		// Custom Class
             *		onMouseDown: function(){},		// Callback function
             *		menuOptions: []					// Menu Item Options (see: jquery.menu for more details)
             *  },
             */
             newTab: {}, 							// Options for dynamically created tabs
             actionItems: [], 					// Custom Action Items
             /**
             * Action items structure example:
             *	{
             *		label: 'My Action Item',		// Text label to be displayed in the action item
             *		side:  'left',					// 'left' or 'right'(default) Display the action item on the right or left of the scrollable area
             *  	actionItemClass: 'selected',	// Custom Class
             *		iconClass: 'fi_circle-plus',	// Custom Class for the icon
             *		onMouseDown: function(){},		// Callback function
             *  },
             */
             onSelect: function () { } 				// CallBack Function
         },
         actionItems: actionTabsArray

         ,

         /**
         * Plugin Initialization. (Executed automatically)
         */
         _create: function () {
             var self = this,
			el = self.element,
			o = self.options;
             $(el).addClass('scrollabletabbar').addClass('clearfix');
             $(el).prepend($('<div class="tabs-fade-left"></div><div class="tabs-fade-right"></div>'));
             $(el).append($('<div class="actionitems-container actionitems-container-left"><ul class="actionitems"></ul></div><div class="tabs-container"><ul class="tabs"></ul></div><div class="actionitems-container actionitems-container-right"><ul class="actionitems"></ul></div>'));

             // Bind Live Events
             $(el).find('.tab').live('mousedown', function (e) {
                 self.selectTab(this);
             });
             $(document).mousemove(function (e) {
                 mouseX = e.pageX
                 mouseY = e.pageY
             });

             // Add Arrow Interactions
             $(document).bind('mouseup', function (e) {
                 mouseDown = false;
             });

             $(el).find('.tabs-fade-left').bind('mousedown', function () {
                 mouseDown = true;
                 self.mouseDownLoop(function () { self.scrollRight() });
             });

             $(el).find('.tabs-fade-right').bind('mousedown', function () {
                 mouseDown = true;
                 self.mouseDownLoop(function () { self.scrollLeft() });
             });

             // Render Tabs & Action Items
             self.renderActionItems();
             self.renderTabs();

             // Disable Selection
             $(el).find('.tabs').disableSelection();
             $(el).find('.actionitem').disableSelection();
             $(el).find('.tab').disableSelection();
             $(el).find('.tabs-fade-left, .tabs-fade-right').disableSelection();

             // Instantiate the ui.sortable plugin
             $(el).find('.tabs').sortable({
                 helper: "clone",
                 containment: $(el).find(".tabs-container"),
                 tolerance: "pointer",
                 axis: 'x',
                 scroll: false,
                 start: function (event, ui) {
                     tabDrag = true;
                     self.onTabDrag();
                 },
                 stop: function (event, ui) {
                     tabDrag = false;

                     //Reordered Tabs Caputering
                     var result = new Array();
                     $('ul.tabs').children().each(function (i) {
                         result.push($(this).attr('id'));
                     });
                     var postData = { pageIds: result };
                     $.ajax({
                         type: "POST",
                         url: ($('#pathname').val()) + "AJAX/ReorderTabs",
                         data: postData,
                         success: function (data) {
                             // alert(data.Result);
                         },
                         error: function (data) {
                             // alert(data.Result);
                         },
                         dataType: "json",
                         traditional: true
                     });










                 }
             });

             // Fix Container Sizes
             self.fixContainerSizes();

             // Render Tabs Effects
             self.renderTabsFadeEffect();

         },

         /**
         * Render Tabs
         */
         renderTabs: function () {
             var self = this,
			el = self.element,
			o = self.options;

             for (var i in o.tabs) {
                 if (typeof o.tabs[i] == 'object') {
                     // alert(o.tabs[i].id);
                     self.addTab(o.tabs[i].label, o.tabs[i].tabClass, o.tabs[i].menuOptions, o.tabs[i].onMouseDown, o.tabs[i].selectTab, o.tabs[i].id);
                 }
             }

             //Fix Container Sizes
             self.fixContainerSizes();

             // Set the active Tab
             if (o.activeTabIndex >= 0) {
                 self.selectTab($(el).find('.tabs').find('.tab-container:eq(' + o.activeTabIndex + ')').find('.tab'));
             }
         },

         /**
         * Add Tab
         *
         * @param	label		Tab text
         * @param	tabClass	CSS Class Name
         */
         addTab: function (label, tabClass, menuOptions, onMouseDown, selectTab, id) {
             var self = this,
			el = self.element,
			o = self.options;
             var opts = new Object;
             //  alert("--TODO selected tab--" + selectTab);
             var tabContainer = $('<li class="tab-container"><div class="tab"><div class="tab-content"><span class="label"></span> <span class="icon"><span class="fi fi_gear"></span></span></div></div></li>');
             $(el).find('.tabs').append(tabContainer);

             // Set Tab Label
             $(tabContainer).find('.label').text(label);
             opts.label = label;

             // Add Custom Tab Class
             if (tabClass) {
                 $(tabContainer).addClass(tabClass)
                 opts.tabClass = tabClass;
             }

             // alert(id);

             // Add Id
             if (id) {
                 $(tabContainer).attr('id', id).bind('click', function () {
                     // alert('User clicked' + id);
                     window.location.href = $('#pathname').val() + "Pages/Index/" + id;

                 });
             }






             // Add Id
             //$(tabContainer).attr('id','tab-'+self.tabCount);
             // self.tabCount++;




             // Bind Events
             opts.onMouseDown = onMouseDown;

             $(tabContainer).data('pluginInstance', self);
             $(tabContainer).data('o', opts);

             if (onMouseDown) {
                 $(tabContainer).find('.tab').bind('mousedown', function (e) {



                     var gearIcon = $(e.target).parents('.menu').data('targetElem');
                     var id = $(gearIcon).parents('.tab-container').attr('id');
                     //alert(id);
                     window.location.href = $('#pathname').val() + "Pages/Index/" + id;
                     return false;
                     //  $(this).parent().data('o').onMouseDown(e);
                 });
             }

             // Bind Tab Gear Menu
             if (menuOptions) {
                 $(tabContainer).find('.fi_gear').menu(menuOptions);
             }

             // Fix Container Sizes
             self.fixContainerSizes();

             // Select Tab
             if (selectTab) {
                 self.selectTab($(tabContainer).find('.tab'));
             }
         },

         /**
         * Render Action Items
         */
         renderActionItems: function () {
             var self = this,
			el = self.element,
			o = self.options;

             o.actionItems = $.merge(self.actionItems, o.actionItems);

             for (var i in o.actionItems) {
                 if (typeof o.actionItems[i] == 'object') {
                     var actionItemContainer = $('<li class="actionitem-container"><div class="actionitem"><div class="actionitem-content"/></div></li>');

                     // Distribute on the left or the right
                     if (o.actionItems[i].side == 'left') {
                         $(el).find('.actionitems-container-left .actionitems').append(actionItemContainer);
                     }
                     else {
                         $(el).find('.actionitems-container-right .actionitems').append(actionItemContainer);
                     }


                     // Add Label
                     if (o.actionItems[i].label) {
                         $('.actionitem-content', actionItemContainer).append('<span class="label">' + o.actionItems[i].label + '</span>');
                     }

                     // Add Custom ActionItem Class
                     if (o.actionItems[i].actionItemClass) {
                         $(actionItemContainer).addClass(o.actionItems[i].actionItemClass);
                     }

                     // Add Custom Icon Class
                     if (o.actionItems[i].iconClass) {
                         $('.actionitem-content', actionItemContainer).append('<span class="icon"><span class="fi"/></span>').find('.fi').addClass(o.actionItems[i].iconClass);
                     }

                     // Add Id
                     if (o.actionItems[i].id) {
                         $(actionItemContainer).attr('id', o.actionItems[i].id);
                     }

                     // Bind Events
                     $(actionItemContainer).data('pluginInstance', self);
                     $(actionItemContainer).data('o', o.actionItems[i]);

                     if (o.actionItems[i].onMouseDown) {
                         $(actionItemContainer).find('.actionitem').bind('mousedown', function (e) {
                             $(this).parent().data('o').onMouseDown(e);
                         });
                     }
                 }
             }
         },

         /**
         * Select Tab
         *
         * @param 	elem	The Tab Element
         */
         selectTab: function (elem) {
             var self = this,
			el = self.element,
			o = self.options;

             //IE7 rendering bug fix
             if (!$(el).find('.tabs').find('.tab-container.active').length) {
                 $(el).find('.tabs').find('.tab-container').last().addClass('active');

                 // Calculate the icon width
                 var iconWidth = $(el).find('.tabs').find('.tab-container').last().find('.icon').outerWidth(true);
                 $(el).find('.tabs').find('.tab-container').last().width($(el).find('.tabs').find('.tab-container').last().width() + iconWidth);
                 self.fixContainerSizes();

                 // Cleanup the width modifications
                 $(el).find('.tabs').css('width', '');
                 $(el).find('.tabs').find('.tab-container').css('width', '');

                 // Fix Container Sizes
                 self.fixContainerSizes();
             }

             $(el).find('.tabs').find('.tab-container').removeClass('active');
             $(elem).parent('.tab-container').addClass('active');

             var marginValue = parseInt($(el).find(".tabs").css('margin-left').replace("px", ""));
             var elementRight = $(elem).parent().offset().left + $(elem).parent().outerWidth(true);
             var elementLeft = $(elem).parent().offset().left
             var tabsContainerRight = $(el).find(".tabs-container").offset().left + $(el).find(".tabs-container").width();
             var tabsContainerLeft = $(el).find(".tabs-container").offset().left;

             // Scroll Right
             if (elementRight > tabsContainerRight) {
                 var newMarginValue = ((tabsContainerRight - elementRight) + marginValue);
                 if ((($(el).find(".tabs").width() + newMarginValue) - $(el).find(".tabs-container").width()) > o.selectedTabGutter) {
                     newMarginValue -= o.selectedTabGutter;
                 }
                 $(el).find(".tabs").clearQueue().animate({
                     'margin-left': newMarginValue + 'px'
                 }, 500);
             }
             // Scroll Left
             if (elementLeft < tabsContainerLeft) {
                 var newMarginValue = ((tabsContainerLeft - elementLeft) + marginValue);
                 if (Math.abs(newMarginValue) > o.selectedTabGutter) {
                     newMarginValue += o.selectedTabGutter;
                 }
                 $(el).find(".tabs").clearQueue().animate({
                     'margin-left': newMarginValue + 'px'
                 }, 500);
             }

             // Fix Container Sizes
             self.fixContainerSizes();

             // Trigger custom Callback event
             o.onSelect();
         },

         /**
         * Render Tabs List Menu
         *
         * @param	element		The element on which the menu will be attached
         */
         attachTabsListMenu: function (elem) {
             var self = this,
			el = self.element,
			o = self.options;

             var menuItems = new Array();

             $(el).find('.tab-container').each(function (i, tabcontainer) {
                 var menuItem = new Object();
                 menuItem.name = $(this).find('.label').text();
                 if ($(this).hasClass('active')) {
                     menuItem.checked = true;
                 }
                 menuItem.onClick = function () {
                     self.selectTab($(tabcontainer).find('.tab'));
                 }
                 menuItems.push(menuItem);
             });


             $(elem).menu({
                 autoOpen: false,
                 items: menuItems
             });
             $(elem).menu('show');
         },

         /**
         * onTabDrag
         */
         onTabDrag: function () {
             var self = this,
			el = self.element,
			o = self.options;

             var tabsContainerRight = $(el).find(".tabs-container").offset().left + $(el).find(".tabs-container").width();
             var tabsContainerLeft = $(el).find(".tabs-container").offset().left;

             if (tabDrag == true) {
                 window.setTimeout(function () { self.onTabDrag() }, 10);
             }
             if (mouseX > tabsContainerRight) {
                 self.scrollLeft();
             }
             else if (mouseX < tabsContainerLeft) {
                 self.scrollRight();
             }
             else {
                 $(el).find(".tabs").find(".ui-sortable-helper").css('display', 'block');
             }
         },

         /**
         * Scroll Left
         */
         scrollLeft: function () {
             var self = this,
			el = self.element,
			o = self.options;

             var marginValue = parseInt($(el).find(".tabs").css('margin-left').replace("px", ""));
             if (Math.abs(marginValue) < ($(el).find(".tabs").width() - $(el).find(".tabs-container").width())) {
                 $(el).find(".tabs").css('margin-left', (marginValue - o.dragScrollSpeed) + 'px');
                 $(el).find(".tabs").find(".ui-sortable-helper").css('display', 'none');
             }
         },

         /**
         * Scroll Right
         */
         scrollRight: function () {
             var self = this,
			el = self.element,
			o = self.options;

             var marginValue = parseInt($(el).find(".tabs").css('margin-left').replace("px", ""));
             if (marginValue < 0) {
                 $(el).find(".tabs").css('margin-left', (marginValue + o.dragScrollSpeed) + 'px');
                 $(el).find(".tabs").find(".ui-sortable-helper").css('display', 'none');
             }
         },

         /**
         * mouseDownLoop
         *
         * @param	action			Anonymus function
         * @param	loopSpeed		Speed
         */
         mouseDownLoop: function (action) {
             var self = this,
			el = self.element,
			o = self.options;

             action();
             if (mouseDown == true) {
                 window.setTimeout(function () { self.mouseDownLoop(action) }, 10);
             }
         },

         /**
         * Render Tabs Fade Effect
         */
         renderTabsFadeEffect: function () {
             var self = this,
			el = self.element,
			o = self.options;


             var marginValue = parseInt($(el).find(".tabs").css('margin-left').replace("px", ""));

             // Left Fade
             if (marginValue < 0) {
                 $(el).find(".tabs-fade-left").fadeIn();
             }
             else {
                 $(el).find(".tabs-fade-left").fadeOut();
                 $(el).find(".tabs").css('margin-left', '');
             }

             // Right Fade (~3px tolerance - Firefox Fix)
             var tabsContainerRight = Math.ceil($(el).find(".tabs-container").offset().left + $(el).find(".tabs-container").width());
             var lastTabRight = Math.ceil($(el).find(".tab-container").last().offset().left + $(el).find(".tab-container").last().outerWidth(true));
             if ((tabsContainerRight >= (lastTabRight - 3))) {
                 $(el).find(".tabs-fade-right").fadeOut();
             }
             else {
                 $(el).find(".tabs-fade-right").fadeIn();

             }

             window.setTimeout(function () { self.renderTabsFadeEffect() }, 60);

         },

         /**
         * Fix Container Sizes
         */
         fixContainerSizes: function () {
             var self = this,
			el = self.element,
			o = self.options;

             var tabBarWidth = $(el).outerWidth(true);
             var actionsItemsContainerWidth = $(el).find('.actionitems-container-left').outerWidth(true) + $(el).find('.actionitems-container-right').outerWidth(true);

             $(el).find(".tabs-container").css('width', (tabBarWidth - actionsItemsContainerWidth - 1) + "px");

             var tabsWidth = 0;
             $(el).find(".tabs-container li").each(function () {
                 tabsWidth += $(this).outerWidth(true);
             });

             $(el).find(".tabs").css('width', tabsWidth + 'px');
             $(el).find(".tabs-fade-left").css('left', $(el).find(".tabs-container").position().left + 'px');
             $(el).find(".tabs-fade-right").css('left', $(el).find(".tabs-container").position().left + $(el).find(".tabs-container").width() - $(el).find(".tabs-fade-right").width() + 'px');
         }

     })
 })(jQuery);