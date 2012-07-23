/**
* jQuery headline carousel
* 
* Author: Ron Edgecomb
* Copyright 2010-2011 Dow Jones & Company Inc.
*
* Depends:
*   jquery.ui.core.js
*   jquery.ui.widget.js
*
*/

    $.widget("ui.headlineCarousel", {
        options: {
            height: 64, 							// Carousel Height
            width: 'auto', 						// Fixed Width or Autocalcuated
            scrollLeft: '.scroll-left', 				// Selector for scroll left control
            scrollRight: '.scroll-right', 			// Selector for scroll right control
            scrollDisabledClass: 'disabled', 					// Selector for disabled scroll control
            scrollClickMove: 1, 							// Number of items to move
            //scrollContinuous: 	false,							// Whether or not to scroll as true carousel
            //startPosition: 		0,								// index of LI to start at
            carouselWrap: '.dj_headline_carousel-wrap'	// Selector for element wrapping carousel
        },

        items: [], // Carousel Items

        /**
        * Plugin Initialization. (Executed automatically)
        */
        _init: function () {
            var self = this,
				el = self.element,
				o = self.options;

            $(el).addClass('dj_headline_carousel-widget');

            self.height = o.height;
            self.width = parseInt((o.width == 'auto') ? $(el).find(o.carouselWrap).width() : o.width);

            self._getItems();
            self.render();

        },

        _create: function () {
            var self = this,
				el = self.element,
				o = self.options;

            el.data('originalElement', $(el).html());

            $(el).find(o.scrollLeft).bind('mousedown', function (e) {
                if (!$(this).hasClass(o.scrollDisabledClass)) {
                    self.moveLeft();
                }
            });

            $(el).find(o.scrollRight).bind('mousedown', function (e) {
                if (!$(this).hasClass(o.scrollDisabledClass)) {
                    self.moveRight();
                }
            });

        },

        _getItems: function () {
            var self = this,
				el = self.element,
				o = self.options,
				items = [];

            $(el).find(o.carouselWrap + ' > ul > li').each(function (index) {

                var item = this,
					$item = $(this);

                $item.attr('carouselid', index);

                // Force absolute positioning and prevent white-space wraping while the element width is being calculated.

                $item.css('position', 'absolute');
                $item.css('white-space', 'nowrap');

                items.push({

                    id: index,
                    el: item,
                    $el: $item,
                    width: $item.outerWidth(true),
                    position: index

                });

                // Restore relative positioning.
                $item.css('position', 'relative');

            });

            self.items = items;

        },

        /*
        get the carousel id of the first item in complete view
        */
        _getFirstInView: function () {

            var self = this,
				el = self.element,
				o = self.options,
				firstItem = 0;

            if (self.wrapPosition.left == 0) {

                firstItem = 0;

            } else {

                var leftOffset = Math.abs(self.wrapPosition.left);

                $.each(self.items, function (key, val) {

                    leftOffset -= val.width;
                    if (leftOffset < 5) { //5px tolerance....

                        firstItem = val.position + 1;
                        return false;

                    }

                });

            }

            return firstItem;

        },

        /*
        get the carousel id of the last item in complete view
        */
        _getLastInView: function () {

            var self = this,
				el = self.element,
				o = self.options,
				lastItem = 0;

            var leftOffset = Math.abs(self.wrapPosition.left),
				itemSpace = leftOffset + self.width;

            $.each(self.items, function (key, val) {

                if ((itemSpace - val.width) < -5) { //5px tolerance....

                    lastItem = val.position - 1;
                    return false;

                }
                lastItem = val.position;
                itemSpace -= val.width;

            });

            return lastItem;

        },

        _setCarouselMinWidth: function () {

            var self = this,
				el = self.element,
				o = self.options,
				width = 1; //1px buffer for good measure

            $.each(self.items, function (key, val) {

                var itemWidth = parseInt(val.width);

                if (itemWidth > 0)
                    width += itemWidth;

            });

            $(el).find(o.carouselWrap + ' > ul').css('minWidth', width);

        },

        _getWrapPosition: function () {
            var self = this,
				el = self.element,
				o = self.options;

            return $(el).find(o.carouselWrap + ' > ul').position();

        },

        destroy: function () {
            var self = this,
				el = self.element,
				o = self.options;

            $(el).addClass('dj_headline_carousel-widget').html(el.data('originalElement'));
            $.Widget.prototype.destroy.apply(this, arguments);

        },

        render: function () {
            var self = this,
				el = self.element,
				o = self.options;

            self._setCarouselMinWidth();
            self._updateCarouselData();

        },

        _updateCarouselData: function () {

            var self = this,
				el = self.element,
				o = self.options;

            self.wrapPosition = self._getWrapPosition();

            $(el).find(o.carouselWrap + ' > ul > li').removeClass('first-in-view last-in-view');
            $(o.scrollLeft + ', ' + o.scrollRight, $(el)).removeClass(o.scrollDisabledClass);

            self.firstInView = self._getFirstInView();
            self.$firstInView = $(el).find(o.carouselWrap + ' > ul > li[carouselid="' + self.firstInView + '"]');

            self.lastInView = self._getLastInView();
            self.$lastInView = $(el).find(o.carouselWrap + ' > ul > li[carouselid="' + self.lastInView + '"]');

            self.$firstInView.addClass('first-in-view');
            self.$lastInView.addClass('last-in-view');

            if (self.firstInView == 0) {
                $(el).find(o.scrollLeft).addClass(o.scrollDisabledClass);
            }

            if (self.lastInView == (self.items.length - 1)) {
                $(el).find(o.scrollRight).addClass(o.scrollDisabledClass);
            }

        },

        moveLeft: function () {
            var self = this,
				el = self.element,
				o = self.options;

            if (self.firstInView > 0) {

                var wrapOffset = Math.abs(self.wrapPosition.left),
					firstOffset = self.$firstInView.position().left,
					nextId = ((self.firstInView - o.scrollClickMove) < 0) ? 0 : (self.firstInView - o.scrollClickMove),
					$nextItem = $(el).find(o.carouselWrap + ' > ul > li[carouselid="' + nextId + '"]'),
					moveOffset = parseInt($nextItem.position().left);
                operator = (moveOffset >= 0) ? '-' : '';

                self._moveCarousel(operator + moveOffset);

            }

        },

        moveRight: function () {
            var self = this,
				el = self.element,
				o = self.options;

            if (self.lastInView != (self.items.length - 1)) {
                var wrapOffset = Math.abs(self.wrapPosition.left),
					lastOffset = self.$lastInView.position().left,
					nextId = ((self.lastInView + o.scrollClickMove) > (self.items.length - 1)) ? (self.items.length - 1) : (self.lastInView + o.scrollClickMove),
					$nextItem = $(el).find(o.carouselWrap + ' > ul > li[carouselid="' + nextId + '"]'),
					moveOffset = parseInt(($nextItem.position().left + self.items[nextId].width) - self.width);
                operator = (moveOffset >= 0) ? '-' : '';
                self._moveCarousel(operator + moveOffset);

            }

        },

        _moveCarousel: function (moveOffset) {
            var self = this,
				el = self.element,
				o = self.options;
            $(el).find(o.carouselWrap + ' > ul').animate({
                left: moveOffset
            }, 'slow', function () {
                self._updateCarouselData();
            });

        }

    });
