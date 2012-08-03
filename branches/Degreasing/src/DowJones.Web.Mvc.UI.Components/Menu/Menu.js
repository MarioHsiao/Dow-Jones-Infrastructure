/*
* Menu
*/

    DJ.UI.Menu = DJ.UI.Component.extend({

        /*
        * Properties
        */

        defaults: {
            autoOpen: true, // Set to true if you want the menu to open at initialization time
            raiseGlobalEvents: false,
            trackActiveItem: false        // setting it true will show a checkmark next to the selected item
        },

        events: {
            menuItemClick: 'itemClick.dj.menu',
            show: 'show.dj.menu'
        },


        init: function (element, meta) {
            var $meta = $.extend({ name: "Menu" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this.$menu = this._createMenuElement(this.options.items);

            this._registerEventHandlers();
            $('body').append(this.$menu);
        },


        _registerEventHandlers: function () {
            var self = this;
            this.$menu.delegate('.menuitem', 'mousedown.dj_menu', function (e) {
                if ($(this).data('item').disabled) {
                    e.stopPropagation();
                    return;
                }
                $dj.debug('Triggering', self.events.menuItemClick, $(this).data('item'));

                if (self.options.trackActiveItem) {
                    self.setActive(this);
                }

                self.hide();
                e.stopPropagation();

                var onItemClick = self.options.onItemClick;
                if (onItemClick && typeof onItemClick === 'function') {
                    onItemClick(e, { parent: this.$element, data: $(this).data('item') });
                }
                else if (self.options.raiseGlobalEvents) {
                    self.publish(self.events.menuItemClick, {
                        parent: self.$element,
                        data: $(this).data('item')
                    });
                }
                else {
                    self.$element.triggerHandler(self.events.menuItemClick, {
                        parent: self.$element,
                        data: $(this).data('item')
                    });
                }
            });

        },

        _initializeEventHandlers: function () {
            /*
            var self = this;

            // need to bind to mousedown otherwise draggable event handlers prevent click handler from being called 
            this.$element.delegate('.menuitem', 'mousedown.dj_menu', function (e) {
            $dj.debug('Triggering', self.events.menuItemClick, $(this).data('item'));

            if (self.options.trackActiveItem) {
            self.setActive(this);
            }

            self.hide();
            e.stopPropagation();

            if (self.options.raiseGlobalEvents) {
            self.publish(self.events.menuItemClick, { parent: self.$element, data: $(this).data('item') });
            }
            else {
            //self.$element.triggerHandler(self.events.menuItemClick, { parent: self.$element, data: $(this).data('item') });
                    
            }
            });
            */
        },


        _createMenuElement: function (items) {
            var $menu = $(this.templates.simpleMenu({ menuItems: items }));

            // Add custom class
            if (this.options.menuClass) {
                $menu.addClass(this.options.menuClass);
            }

            var menuitems = $menu.find('.menuitem'),
                menuDataItems = _.select(items, function (item) {
                    return item.type !== 'separator';
                });

            _.each(menuitems, function (menuItem, i) {
                $(menuItem).data('item', menuDataItems[i]);
            }, this);

            return $menu;
        },


        setActive: function (item) {
            var $item = $(item),
                itemData = $item.data('item'),
                otherMenuItems = $item.siblings('.menuitem');

            // reset previous selection
            _.each(otherMenuItems, function (item) {
                var $item = $(item);
                if ($item.data('item') && $item.data('item').checked) {
                    $item.removeClass('checked'); $item.data('item').checked = false;
                }
            });

            $(item).addClass('checked');
            itemData.checked = true;
        },

        show: function () {
            var onShow = this.options.onShow;
            if (onShow && typeof onShow === 'function') { onShow(); }

            var menuitems = this.$menu.find('.menuitem');
            $.each(menuitems, function (i, menuitem) {
                var $mi = $(menuitem);
                if ($mi.data('item').disabled) {
                    $mi.addClass("disabled");
                }
                else {
                    $mi.removeClass("disabled");
                }
            });

            this.positionElement(this.$menu, this.$element, this.options.zIndex);

            var self = this;
            // give the user some grace period if he clicks outside of menu
            window.setTimeout(function () {
                $('body').unbind('mousedown.dj_menu').bind('mousedown.dj_menu', function (e) {
                    self.hide();
                    e.stopPropagation();
                });
            }, 20);

        },


        hide: function () {
            this.$menu.hide();
        },


        positionElement: function ($elem, $parent, zIndex) {
            var parentOffset = $parent.offset(),
             elemWidth = $elem.outerWidth(true);

            // assume 99999 is the highest. this avoids fancy calculations to determine highest z-index on page
            // in case 99999 is not the highest, user can specify a higher number thru options
            $elem.css({
                'position': 'absolute',
                'z-index': zIndex || 99999,
                'left': parentOffset.left + 'px',       // Align element to the left
                'top': parentOffset.top + $parent.height() + 'px'
            });

            $elem.show();

            // If the element appears outside of the window, align it to the right
            if (($elem.offset().left + elemWidth) > $(window).width()) {
                $elem.css('left', (parentOffset.left + $parent.width() - elemWidth) + 'px');
            }
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_menu', DJ.UI.Menu);
