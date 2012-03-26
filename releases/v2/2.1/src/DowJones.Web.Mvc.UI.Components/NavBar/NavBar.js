/*!
* NavBar
*
*/

(function ($) {

    DJ.UI.NavBar = DJ.UI.Component.extend({

        /*
        * Properties
        */

        // Default options
        defaults: {
            dragScrollSpeed: ($.browser.msie ? 40 : 20),    // The Scroll Speed when you Drag a Tab
            selectedTabGutter: 70
        },

        selectors: {
            tabContainer: '.tab-container',
            tabsContainer: '.tabs-container',
            actionItems: '.actionitems',
            actionItemsContainer: 'div.actionitems-container',     // overall container
            actionItemContainer: 'li.actionitem-container',     // individual tabs - these are the <li> elements inside the <ul>
            actionItemsContainerRight: '.actionitems-container-right',
            allContainers: '.actionitem-container,.tab-container',
            rightActionItems: '.actionitems-container-right .actionitems',
            menuItemContainer: '.dj_menuItemContainer',
            tabs: '.tabs',
            tab: '.tab',
            leftTabFader: '.tabs-fade-left',
            rightTabFader: '.tabs-fade-right',
            faders: '.tabs-fade-left, .tabs-fade-right',
            menuTab: '.dj_menuTab',
            actionItem: '.dj_actionItem',
            uiSortableHelper: '.ui-sortable-helper'
        },

        events: {
            // jQuery events are namespaced as <event>.<namespace>
            tabDragStop: "tabDragStop.dj.NavBar",
            tabClick: 'tabClick.dj.NavBar',
            tabSelect: 'tabSelect.dj.NavBar',
            actionItemClick: 'actionItemClick.dj.NavBar'
        },


        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "NavBar" }, meta);

            // Call the base constructor
            this._super(element, $meta);


            // Fix Container Sizes - make those hanging tabs appear right
            this.fixContainerSizes();


            // nothing to fade if there is no or single tab
            if (!this.data
               || !this.data.tabs
               || this.data.tabs.length <= 1) {
                this.$rightTabFader.addClass('hide');
            }


            this.assignTabData();

            // Disable Selection
            this.$tabsContainer.disableSelection();
            this.$leftTabFader.disableSelection();
            this.$rightTabFader.disableSelection();

            // arrange for drag and drop and re-ordering of tabs
            this.makeSortable();

            // find the selected tab and set it as selected
            this.highlightSelectedTab();

            // Render Tabs Effects
            this.renderTabsFadeEffect();

            // cannot use slide due to a limitation in IE7
            // if element's display is set to none, its width comes as 0. 
            // so fixContainerSizes doesn't adjust actionItems container properly. 
            // workaround is to use visibility hidden and then animate

            // only for IE7
            this.$actionItemsContainer.css({ visibility: "visible" }).show();
            //this.$actionItemsContainer.show('slide', {}, 500);

            //render 
            if (this.$element.find(".tabs-fade-right").css("display") == "block" ||
                this.$element.find(".tabs-fade-left").css("display") == "block") {

                // show the tabs dropdown button
                this.$menuTab.show();
            }
            else {
                // no need to show the tabs dropdown button
                this.$menuTab.hide();
            }

        },


        _initializeElements: function (ctx) {
            var $el = this.$element;
            this.$tabContainer = $el.find(this.selectors.tabContainer);
            this.$tabsContainer = $el.find(this.selectors.tabsContainer);
            this.$allContainers = $el.find(this.selectors.allContainers);

            // action items
            this.$actionItems = $el.find(this.selectors.actionItems)
            this.$actionItemsContainer = $el.find(this.selectors.actionItemsContainer);
            this.$actionItemContainers = $el.find(this.selectors.actionItemContainer);
            this.$actionItemsContainerRight = $el.find(this.selectors.actionItemsContainerRight);
            this.$rightActionItems = $el.find(this.selectors.rightActionItems);

            this.$tabs = $el.find(this.selectors.tabs);
            this.$leftTabFader = $el.find(this.selectors.leftTabFader);
            this.$rightTabFader = $el.find(this.selectors.rightTabFader);
            this.$menuTab = $el.find(this.selectors.menuTab);

        },

        _initializeDelegates: function () {
            this._delegates = $.extend({}, {
                onActionItemClick: $dj.delegate(this, this._onActionItemClick)
            });
        },


        _initializeEventHandlers: function () {
            var self = this,
                $el = this.$element;

            this.$menuTab.delegate(this.selectors.actionItem, 'click', function (e) {
                var $container = $(this).closest(self.selectors.menuItemContainer);
                self.onActionItemClick($container, this);
                return false;

            });

            $el.delegate(this.selectors.actionItem, 'click', function (e) {
                var $container = $(this).closest(self.selectors.menuItemContainer);
                self.onActionItemClick($container, this);
                return false;
            })
            .delegate(this.selectors.actionItemContainer, 'click', function (e) {
                var $container = $(this);
                if (!self.isTabSelected($container) && self.isTabSelectable($container)) {
                    $el.triggerHandler(self.events.tabClick, { id: $container.data('id'), data: $container.data('tabData') });
                    self.selectTab(this, true);
                }
                return false;
            })
            .delegate(this.selectors.leftTabFader, 'mousedown', function () {
                self.mouseDown = true;
                self.mouseDownLoop(function () { self.scrollRight(); self.renderTabsFadeEffect(); });
            })
            .delegate(this.selectors.rightTabFader, 'mousedown', function () {
                self.mouseDown = true;
                self.mouseDownLoop(function () { self.scrollLeft(); self.renderTabsFadeEffect(); });
            })
            .delegate(this.selectors.tabContainer, 'click', function (e) {
                var $container = $(this);
                if (!self.isTabSelected($container) && self.isTabSelectable($container)) {
                    $el.triggerHandler(self.events.tabClick, { id: $container.data('id'), data: $container.data('tabData') });
                    self.selectTab(this);
                }
                return false;
            })
            .delegate(this.selectors.tabsContainer, 'click', function (e) {
                self.mouseX = e.pageX;
            })
            .delegate(this.selectors.faders, 'mouseup mouseleave', function (e) {
                self.mouseDown = false;
                $dj.debug('In faders mouseup mouseleave', e.type);
            });
        },


        _onActionItemClick: function (e) {
        },



        /*
        * Public methods
        */

        makeSortable: function () {
            var self = this,
                $el = this.$element;

            // Instantiate the ui.sortable plugin
            this.$tabs.sortable({
                helper: "clone",
                containment: this.$tabs,
                tolerance: "pointer",
                axis: 'x',
                scroll: false,
                start: function (event, ui) {
                    self.tabDrag = true;
                    self.onTabDrag();
                },
                stop: function (event, ui) {

                    self.menuItems = null;
                    self.tabDrag = false;
                    //Reordered Tabs Caputering
                    var result = [];
                    _.each(self.$tabs.children(), function (child) {
                        var id = $(child).data('id');
                        if (id) {
                            result.push(id);
                        }
                    });

                    $dj.debug('After tab drag, order of tabs:', result);
                    if (result.length > 1) {
                        $el.triggerHandler(self.events.tabDragStop, { tabIds: result });
                    }
                }
            });

            // one off case since _initializeElements fires before init
            this.$uiSortableHelper = $el.find(this.selectors.uiSortableHelper);
        },


        onActionItemClick: function ($container, elem) {
            if (!$container || $container.length === 0) {
                $dj.debug(this.name, ': onActionItemClick - Container is null or undefined');
                return;
            }

            var $elem = $(elem);
            // check if menuitems are specified. if yes, show them
            var tabData = $container.data('tabData');
            var menuItems = tabData && tabData.menuItems;

            if (menuItems) {
                this.showMenu($elem, menuItems);
            }
            else if ($container.data('id') === 'menu-tab') {     // special case for tab list
                this.showMenu($elem, this.getTabsMenuItems(), $dj.delegate(this, this.onMenuTabClick), true);
            }
            else if ($container.data('id') === 'add-tab') {     // special case for add tab
                this.$element.triggerHandler(this.events.tabClick, { id: 'add-tab' });
            }
            else { $dj.debug(this.name, '.onActionItemClick: No Menuitems defined'); }


        },

        /// Triggers when menu tab's dropdown button is clicked
        onMenuTabClick: function (e, item) {
            $dj.debug('Received menu item click', e, item);
            if (item && item.data && item.data.metaData && item.data.metaData.tabElem) {
                this.selectTab(item.data.metaData.tabElem);
            }
        },


        showMenu: function ($container, menuItems, eventHandler, trackActiveItem) {
            if (!menuItems) { $dj.debug('showMenu: Cannot show Menu - Menuitems were null or undefined'); return; }

            var menu = $container.findComponent(DJ.UI.Menu);

            if (menu) {
                menu.show();
            }
            else {
                var useCustomEvents = eventHandler && typeof eventHandler === 'function';
                menu = $container.dj_menu({
                    options: {
                        items: menuItems,
                        menuClass: 'iconified',
                        raiseGlobalEvents: !useCustomEvents,
                        trackActiveItem: trackActiveItem !== undefined ? trackActiveItem : false,
                        onItemClick: useCustomEvents ? eventHandler : null
                    }
                });

                menu.show();

                /*if (useCustomEvents) {
                menu.setOwner(this);
                $container.bind('itemClick.dj.menu', eventHandler);
                }*/
            }
        },


        getTabsMenuItems: function () {
            var menuItems;
            _.each(this.$tabs.children(), function (tab) {
                if (!menuItems) { menuItems = []; }
                var tabData = $(tab).data('tabData');
                menuItems.push({
                    id: tabData.id,
                    label: tabData.label,
                    checked: $(tab).hasClass('active'),
                    metaData: { tabElem: $(tab) }
                });
            });

            return menuItems;
        },


        fixContainerSizes: function () {
            var tabsWidth = this.getTabsInnerWidth(),
                tabBarWidth = this.$element.width(),
                calculatedTabBarWidth = 0,
                calculatedActionItemsWidth = 0;

            // if (tabsWidth > tabBarWidth) {      // we've more tabs than what we can fit on screen; seed it off actionItems
            if ((tabsWidth + this.getActionItemsWidth() + 1) > tabBarWidth) {

                calculatedActionItemsWidth = this.getActionItemsWidth() + 1;
                calculatedTabBarWidth = tabBarWidth - calculatedActionItemsWidth - 1;
            }
            else {      // less tabs; make actionItems fill rest of the space
                calculatedTabBarWidth = tabsWidth;
                calculatedActionItemsWidth = tabBarWidth - tabsWidth;
            }


            this.$rightActionItems.css('width', calculatedActionItemsWidth + 'px');

            // Set tabs container width
            this.$tabsContainer.css('width', calculatedTabBarWidth + 'px');
            this.$tabs.css('width', tabsWidth + 1 + 'px');

            // Position tab fade arrows
            this.$leftTabFader.css('left', this.$tabsContainer.position().left + 'px');
            this.$rightTabFader.css('left', this.$tabsContainer.position().left + this.$tabsContainer.width() - this.$rightTabFader.width() + 'px');
        },


        getActionItemsWidth: function () {
            var actionItemsWidth = 0;
            // need the loop to calculate correct width for IE7. Every other browser returns correct width
            // with .outerWidth() only; i.e. this.$actionItemsContainer.outerWidth(true);
            //if ($.browser.version === "7.0") {
            if ($.browser.msie) {
                _.each(this.$actionItemContainers, function (actionItemContainer) {
                    actionItemsWidth += $(actionItemContainer).outerWidth(true);
                });
            }
            else {
                actionItemsWidth = this.$actionItemsContainer.outerWidth(true);
            }

            return actionItemsWidth;
        },

        /**
        * Render Tabs
        */
        assignTabData: function () {
            var tabsData = this.data && this.data.tabs,
                tabs = this.$tabs.children(),
                actionItemsData = this.data && this.data.actionItems,
                actionItems = this.$actionItems.children();

            // assign tab data to each tab
            _.each(tabsData, function (tabData, i) {
                $(tabs[i]).data('tabData', tabData);
            });

            // repeat for action items
            _.each(actionItemsData, function (actionItemData, i) {
                $(actionItems[i]).data('tabData', actionItemData);
            });

        },


        highlightSelectedTab: function () {
            _.any(this.$tabs.children(), function (tab) {
                if ($(tab).data('is-selectable') === 'True') {
                    var tabData = $(tab).data('tabData');
                    if (tabData && tabData.isSelected) {
                        this.scrollTabIntoView($(tab));
                        return true;
                    }
                }
            }, this);

            // look for action items
            _.any(this.$actionItems.children(), function (tab) {
                if ($(tab).data('is-selectable') === 'True') {
                    var tabData = $(tab).data('tabData');
                    if (tabData && tabData.isSelected) {
                        this.scrollTabIntoView($(tab));
                        return true;
                    }
                }
            }, this);

        },


        /**
        * Select Tab
        *
        * @param     elem    The Tab Element
        */
        selectTab: function (tabContainer, fixed) {
            var $tabContainer = $(tabContainer);

            // if tab is not selectable, do nothing
            if ($tabContainer.data('is-selectable') !== 'True') { return; }

            this.$allContainers.removeClass('active');
            $tabContainer.addClass('active');

            if (!fixed) {
                this.scrollTabIntoView($tabContainer);
            }

            this.$element.triggerHandler(this.events.tabSelect, { id: $tabContainer.data('id'), data: $tabContainer.data('tabData') });
        },


        isTabSelected: function ($tab) {
            return $tab && $tab.hasClass('active');
        },


        isTabSelectable: function ($tab) {
            return $tab && $tab.data('is-selectable') === "True";
        },


        scrollTabIntoView: function ($tabContainer) {
            var marginValue = parseInt(this.$tabs.css('margin-left').replace('px', ""), 10);
            var elementLeft = $tabContainer.offset().left;
            var elementRight = elementLeft + $tabContainer.width();
            var tabsContainerLeft = this.$tabsContainer.offset().left;
            var tabsContainerRight = tabsContainerLeft + this.$tabsContainer.width() + 1;
            var newMarginValue;

            // Scroll Right
            if (elementRight > tabsContainerRight) {
                newMarginValue = ((tabsContainerRight - elementRight) + marginValue);
                if (((this.$tabs.width() + newMarginValue) - this.$tabsContainer.width()) > this.options.selectedTabGutter) {
                    newMarginValue -= this.options.selectedTabGutter;
                }
                this.$tabs.clearQueue().animate({
                    'margin-left': newMarginValue + 'px'
                }, 500, $dj.delegate(this, this.renderTabsFadeEffect));
            }

            // Scroll Left
            if (elementLeft < tabsContainerLeft) {
                newMarginValue = ((tabsContainerLeft - elementLeft) + marginValue);
                if (Math.abs(newMarginValue) > this.options.selectedTabGutter) {
                    newMarginValue += this.options.selectedTabGutter;
                }
                this.$tabs.clearQueue().animate({
                    'margin-left': newMarginValue + 'px'
                }, 500, $dj.delegate(this, this.renderTabsFadeEffect));
            }

            // without this call, faders don't appear 
            this.fixContainerSizes();
        },


        /**
        * onTabDrag
        */
        onTabDrag: function () {
            var self = this;
            var tabsContainerLeft = this.$tabsContainer.offset().left;
            var tabsContainerRight = tabsContainerLeft + this.$tabsContainer.width();

            if (this.tabDrag === true) {
                window.setTimeout(function () { self.onTabDrag(); }, 10);
            }
            if (self.mouseX > tabsContainerRight) {
                this.scrollLeft();
            }
            else if (self.mouseX < tabsContainerLeft) {
                this.scrollRight();
            }
            else {
                this.$uiSortableHelper.css('display', 'block');
            }
        },

        /**
        * Scroll Left
        */
        scrollLeft: function () {
            var marginValue = parseInt(this.$tabs.css('margin-left').replace('px', ''), 10);
            if (Math.abs(marginValue) < (this.$tabs.width() - this.$tabsContainer.width())) {
                this.$tabs.css('margin-left', (marginValue - this.options.dragScrollSpeed) + 'px');
                this.$uiSortableHelper.css('display', 'none');
            }
        },

        /**
        * Scroll Right
        */
        scrollRight: function () {
            var marginValue = parseInt(this.$tabs.css('margin-left').replace('px', ""), 10);
            if (marginValue < 0) {
                this.$tabs.css('margin-left', (marginValue + this.options.dragScrollSpeed) + 'px');
                this.$uiSortableHelper.css('display', 'none');
            }
        },

        /**
        * mouseDownLoop
        *
        * @param    action            Anonymus function
        * @param    loopSpeed        Speed
        */
        mouseDownLoop: function (action) {
            var self = this;
            action();
            if (self.mouseDown === true) {
                window.setTimeout(function () { self.mouseDownLoop(action); }, 10);
            }
        },

        /**
        * Render Tabs Fade Effect
        */
        renderTabsFadeEffect: function () {
            var $el = this.$element;

            // there are no tabs to fade
            if (this.$tabs.children().length === 0) { return; }


            var marginValue = parseInt(this.$tabs.css('margin-left').replace('px', ""), 10);

            // Left Fade
            if (marginValue < 0) {
                $el.find(".tabs-fade-left:hidden").fadeIn();
            }
            else {
                $el.find(".tabs-fade-left:visible").fadeOut();
                this.$tabs.css('margin-left', '');
            }

            // Right Fade (~3px tolerance - Firefox Fix)
            var tabsContainerRight = Math.ceil(this.$tabsContainer.offset().left + this.$tabsContainer.width());
            var $lastTab = this.$tabContainer.last();
            var lastTabRight = Math.ceil($lastTab.offset().left + $lastTab.outerWidth(true));
            if ((tabsContainerRight >= (lastTabRight - 3))) {
                $el.find(".tabs-fade-right:visible").fadeOut();
            }
            else {
                $el.find(".tabs-fade-right:hidden").fadeIn();
            }



        },

        /**
        * Calculate tabbar inner width
        */
        getTabsInnerWidth: function () {
            var $el = this.$element;

            var tabsWidth = 0;
            $el.find(".tabs li").each(function () {
                tabsWidth += $(this).outerWidth(true);
            });

            return tabsWidth;
        }



    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_NavBar', DJ.UI.NavBar);


})(jQuery);