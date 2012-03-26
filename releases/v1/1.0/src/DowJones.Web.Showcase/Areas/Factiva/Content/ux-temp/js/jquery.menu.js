/*
 * jQuery menu
 * 
 * Author: Philippe Arcand
 * Copyright 2011 Dow Jones & Company Inc.
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 *
 */
 (function ($, undefined) {

     $.widget("ui.menu", {
         options: {
             menuType: 'normal', 	// 'normal' (left click), 'contextMenu' (right click)
             items: '',
             autoOpen: true
             /**
             * Items structure example:
             *	{ 	type: 'item'				// 'item', 'separator'
             *		name: 'Edit',				// item name
             *  	onClick: function(){},		// Call back function
             *  	onMouseDown: function(){},	// Call back function (onClick Alias)
             *		disabled: false				// true/false
             *  },
             */
         },

         /**
         * Plugin Initialization. (Executed automatically)
         */
         _create: function () {
             var self = this,
			el = self.element,
			o = self.options;

             if (o.menuType == 'contextMenu') {
                 $(el).unbind().bind("contextmenu", function (e) {
                     self.show(e.pageX, e.pageY);
                     // Prevent Default Context Menu
                     return false;
                 });
             }
             else {
                 if (o.autoOpen == true) {
                     $(el).unbind().bind('mousedown click', function (e) {
                         self.show();
                         e.stopPropagation();

                         return false;

                     });
                 }

             }
         },

         /**
         * Build the menu based on the options.items object
         */
         show: function () {
             var self = this,
			el = self.element,
			o = self.options;

             self.hide();

             var menu = $('<div class="menu"><div class="menuitems"></div></div>');

             // Attach Target Element to menu
             $(menu).data('targetElem', self.element);

             // Create Menu Items
             for (var i in o.items) {
                 if (typeof o.items[i] == 'object') {
                     menuItem = '';
                     if (o.items[i].type == 'separator') {
                         menuItem = $('<div class="separator"><div>');
                     }
                     else {
                        // alert("--TODO--menu" + o.items[i].id);
                         menuItem = $('<div class="menuitem"><div class="label">' + o.items[i].name + '</div><div>');
                     }
                     $(menuItem).data("item", o.items[i]);

                     // Disabled State
                     if (o.items[i].disabled == true) {
                         $(menuItem).addClass('disabled');
                     }

                     // Checked State
                     if (o.items[i].checked == true) {
                         $(menuItem).addClass('checked');
                     }

                     // Bind onClick / on Mouse Down Events
                     $(menuItem).bind('mousedown', function (e) {


                         if ($(this).data("item").onClick && $(this).data("item").disabled != true) {


                         //    alert("--ToDo--" + $(this).text());
                             // $('#pathname').val() + "Pages/Edit/" + id; 
                             $(this).data("item").onClick(e);
                         }
                         else if ($(this).data("item").onMouseDown && $(this).data("item").disabled != true) {
                             // alert($(this).text());
                             $(this).data("item").onMouseDown(e);
                         }
                         self.hide();
                         e.stopPropagation();
                     });

                     $(menu).find('.menuitems').append(menuItem);
                 }
             }

             $(menu).css('display', 'block');
             $('body').append(menu);

             $(menu).css('left', $(el).offset().left + 'px');
             $(menu).css('top', $(el).offset().top + $(el).height() + 'px');

             self.positionElement(menu);

             window.setTimeout(function () {
                 $("body").bind('mousedown', function (e) {
                     self.hide();
                     e.stopPropagation();
                 });
             }, 20);

         },

         /**
         *  Hide (remove) menu elements from the page.
         */
         hide: function () {
             // Remove all menu instances
             $('.menu').remove();
             $("body").unbind('mousedown');
         },

         /**
         *  Hide (remove) menu elements from the page.
         *
         *  @param	elem	Element
         */
         positionElement: function (elem) {
             var self = this,
			el = self.element,
			o = self.options;

             // Align element to the left
             $(elem).css('left', $(el).offset().left + 'px');
             $(elem).css('top', $(el).offset().top + $(el).height() + 'px');

             // If the element appears outside of the window, align it to the right
             if (($(elem).offset().left + $(elem).width()) > $(window).width()) {
                 $(elem).css('left', (($(el).offset().left + $(el).width()) - $(elem).width()) + 'px');
             }
         }
     })
 })(jQuery);