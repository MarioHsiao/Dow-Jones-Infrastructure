/**
* Scrollbar Plugin
*
* @category   jQuery UI Widget
* @author     Philippe Arcand <Philippe.Arcand@dowjones.com>
* @copyright  2012 Dow Jones & Company Inc.
* @version    SVN: $Id: jquery.scrollbar.js parcand $
* @dependency jquery.ui.core.js
* @dependency jquery.ui.widget.js
* @dependency jquery.validate.js
*/

(function ($, undefined) {

    $.widget("ui.scrollbar", {
        // Scrollbar elements:
        scrollbarY: '',
        scrollbarYThumb: '',
        scrollbarYTrack: '',
        scrollbarX: '',
        scrollbarXThumb: '',
        scrollbarXTrack: '',

        // Dynamic properties:
        scrollX: false,
        scrollY: false,
        elemHeight: '',
        elemWidth: '',
        scrollHeight: '',
        scrollWidth: '',
        scrollTop: '',
        scrollLeft: '',
        maxScrollTop: '',
        maxScrollLeft: '',
        thumbHeight: '',

        // Event-related properties:
        dragged: false,

        // Widget options
        options: {
            minThumbSize: 30, // Minimum size of the scrollbar handle
            autoRefresh: false // Experimental feature based on the DOMSubtreeModified event. 
            //(Gegko and Wekbkit only) Setting this value to true will automatically recalculate the scrollbar size when the content is updated.
        },

        /**
        * Plugin Creation. (Executed automatically)
        */
        _create: function () {
            var self = this,
				el = self.element,
				o = self.options;

            el.attr("alt", "bejing bol");
            el.addClass('dj_scrollbar-init');

            // Browser Detection
            if ($.browser.mozilla) {
                self._renderScrollbar();
            }

            // Mobile
            var deviceAgent = navigator.userAgent.toLowerCase();
            var ios = deviceAgent.match(/(iphone|ipod|ipad)/);
            if (ios) {
                el.addClass('dj_scrollbar-ios');
            }
        },

        /**
        * Plugin Initialization. (Executed automatically)
        */
        _init: function () {
            var self = this,
				el = self.element,
				o = self.options;
        },

        /**
        * Force a scrollbar refresh
        */
        refresh: function () {
            var self = this,
				el = self.element,
				o = self.options;

            if (self.scrollX) {
                self._updateThumbSize('x');
                self._updateThumbPosition('x');
            }
            else if (self.scrollY) {
                self._updateThumbSize('y');
                self._updateThumbPosition('y');
            }
        },

        /**
        * Render a custom scrollbar
        */
        _renderScrollbar: function () {
            var self = this,
				el = self.element,
				o = self.options;

            self.elemHeight = el.height();
            self.elemWidth = el.width();

            // X-axis
            if (el.hasClass('dj_scrollbar-x')) {
                self.scrollX = true;
                var paddingBottom = parseInt(el.css('padding-bottom').replace(/[^-\d\.]/g, ''));

                el.height(self.elemHeight + 5);
                el.width(self.elemWidth);

                var scrollbarX = $('<div class="dj_scrollbar" style="width:' + self.elemWidth + 'px; margin-top:' + self.elemHeight + 'px; position:relative;"><div class="dj_scrollbar-track" style="width:' + self.elemWidth + 'px;"><div class="dj_scrollbar-thumb"></div></div></div>')

                el.css('position', 'absolute')
                el.css('clip', 'rect(0px ' + self.elemWidth + 'px ' + (self.elemHeight + paddingBottom) + 'px 0px)')

                el.after(scrollbarX);

                self.scrollbarX = el.siblings('.dj_scrollbar').first();
                self.scrollbarXThumb = $(self.scrollbarX).find('.dj_scrollbar-thumb');
                self.scrollbarXTrack = $(self.scrollbarX).find('.dj_scrollbar-track');

                // Prevent Scrollbar selection
                $(self.scrollbarX).disableSelection();
                $(self.scrollbarXThumb).disableSelection();
                $(self.scrollbarXTrack).disableSelection();

                self.scrollbarXThumb.bind('click', function (e) {
                    e.stopPropagation();
                })

                // Track clicks
                self.scrollbarXTrack.bind('click', function (e) {
                    self.dragged = true;

                    var position = 0;
                    var mouseX = e.pageX - $(self.scrollbarXTrack).offset().left;
                    var thumbLeft = parseInt($(self.scrollbarXThumb).css('left').replace(/[^-\d\.]/g, ''));

                    if (mouseX < thumbLeft) {
                        position = thumbLeft - self.thumbWidth;
                    } else if (mouseX > (thumbLeft + self.thumbWidth)) {
                        position = thumbLeft + self.thumbWidth;
                    }
                    if (position < 0) {
                        position = 0;
                    }
                    else if ((position + self.thumbWidth) > self.elemWidth) {
                        position = self.elemWidth - self.thumbWidth;
                    }

                    // Scrolling animation
                    $(self.scrollbarXThumb).animate({ left: position }, 200, function () { self.dragged = false });
                    $(el).animate({ scrollLeft: ((position * self.maxScrollLeft) / (self.elemWidth - self.thumbWidth)) }, 200);
                });

                $(self.scrollbarXThumb).draggable({
                    containment: 'parent',
                    scroll: false,
                    start: function () { self.dragged = true; },
                    stop: function () { self.dragged = false; },
                    drag: function () {
                        self._updateScrollingPosition('x');
                    }
                });

                if (o.autoRefresh) {
                    $(el).bind('DOMSubtreeModified', function () {
                        self._updateThumbSize('x');
                        self._updateThumbPosition('x');
                    });
                }

                el.scroll(function () { if (!self.dragged) { self._updateThumbPosition('x') } });
                self._updateThumbSize('x');
                self._updateThumbPosition('x');
            }
            // Y-axis
            if (el.hasClass('dj_scrollbar-y')) {
                self.scrollY = true;
                var paddingRight = parseInt(el.css('padding-right').replace(/[^-\d\.]/g, ''));

                el.width(self.elemWidth + 5);  //MA: width of the container.

                var scrollbarY = $('<div class="dj_scrollbar" style="height:' + self.elemHeight + 'px; margin-left:' + self.elemWidth + 'px; position:relative;"><div class="dj_scrollbar-track" style="height:' + self.elemHeight + 'px;"><div class="dj_scrollbar-thumb"></div></div></div>');

                el.css('position', 'absolute');
                el.css('clip', 'rect(0px ' + (self.elemWidth - paddingRight) + 'px ' + self.elemHeight + 'px 0px)');

                el.after(scrollbarY);

                self.scrollbarY = el.siblings('.dj_scrollbar').first();
                self.scrollbarYThumb = $(self.scrollbarY).find('.dj_scrollbar-thumb');
                self.scrollbarYTrack = $(self.scrollbarY).find('.dj_scrollbar-track');

                // Prevent Scrollbar selection
                $(self.scrollbarY).disableSelection();
                $(self.scrollbarYThumb).disableSelection();
                $(self.scrollbarYTrack).disableSelection();

                self.scrollbarYThumb.bind('click', function (e) {
                    e.stopPropagation();
                })

                // Track clicks
                self.scrollbarYTrack.bind('click', function (e) {
                    self.dragged = true;

                    var position = 0;
                    var mouseY = e.pageY - $(self.scrollbarYTrack).offset().top;
                    var thumbTop = parseInt($(self.scrollbarYThumb).css('top').replace(/[^-\d\.]/g, ''));

                    if (mouseY < thumbTop) {
                        position = thumbTop - self.thumbHeight;
                    } else if (mouseY > (thumbTop + self.thumbHeight)) {
                        position = thumbTop + self.thumbHeight;
                    }
                    if (position < 0) {
                        position = 0;
                    }
                    else if ((position + self.thumbHeight) > self.elemHeight) {
                        position = self.elemHeight - self.thumbHeight;
                    }

                    // Scrolling Animation
                    $(self.scrollbarYThumb).animate({ top: position }, 200, function () { self.dragged = false });
                    $(el).animate({ scrollTop: ((position * self.maxScrollTop) / (self.elemHeight - self.thumbHeight)) }, 200);
                });

                $(self.scrollbarYThumb).draggable({
                    containment: 'parent',
                    scroll: false,
                    start: function () { self.dragged = true; },
                    stop: function () { self.dragged = false; },
                    drag: function () {
                        self._updateScrollingPosition('y');
                    }
                });

                if (o.autoRefresh) {
                    $(el).bind('DOMSubtreeModified', function () {
                        self._updateThumbSize('y');
                        self._updateThumbPosition('y');
                    });
                }

                el.scroll(function () { if (!self.dragged) { self._updateThumbPosition('y') } });
                self._updateThumbSize('y');
                self._updateThumbPosition('y');
            }
        },

        /**
        * Update the scrolling position
        *
        * @param	string		axis	Scrollbar axis (Possible values: 'x', 'y')		
        */
        _updateScrollingPosition: function (axis) {
            var self = this,
				el = self.element,
				o = self.options;

            // X-axis
            if (axis == 'x') {
                var thumbLeft = $(self.scrollbarXThumb).css('left').replace(/[^-\d\.]/g, '');
                el.scrollLeft((thumbLeft * self.maxScrollLeft) / (self.elemWidth - self.thumbWidth));
            }

            // Y-axis
            if (axis == 'y') {
                var thumbTop = $(self.scrollbarYThumb).css('top').replace(/[^-\d\.]/g, '');
                el.scrollTop((thumbTop * self.maxScrollTop) / (self.elemHeight - self.thumbHeight));
            }
        },

        /**
        * Update the scrollbar thumb position
        *
        * @param	string		axis	Scrollbar axis (Possible values: 'x', 'y')		
        */
        _updateThumbPosition: function (axis) {
            var self = this,
				el = self.element,
				o = self.options;

            self.scrollTop = $(el).scrollTop();
            self.scrollLeft = $(el).scrollLeft();

            // X-axis
            if (axis == 'x') {
                $(self.scrollbarXThumb).css('left', ((self.scrollLeft * (self.elemWidth - self.thumbWidth)) / self.maxScrollLeft) + 'px');
            }
            // Y-axis
            if (axis == 'y') {
                $(self.scrollbarYThumb).css('top', ((self.scrollTop * (self.elemHeight - self.thumbHeight)) / self.maxScrollTop) + 'px');
            }
        },

        /**
        * Update the scrollbar thumb size
        *
        * @param	string		axis	Scrollbar axis (Possible values: 'x', 'y')		
        */
        _updateThumbSize: function (axis) {
            var self = this,
				el = self.element,
				o = self.options;

            self.scrollHeight = $(el)[0].scrollHeight;
            self.scrollWidth = $(el)[0].scrollWidth;
            self.maxScrollTop = self.scrollHeight - self.elemHeight;
            self.maxScrollLeft = self.scrollWidth - self.elemWidth;
            self.thumbHeight = 0;
            self.thumbWidth = 0;

            // X-axis
            if (axis == 'x') {
                self.thumbWidth = Math.round(self.elemWidth * (self.elemWidth / self.scrollWidth));
                if (self.thumbWidth < o.minThumbSize) self.thumbWidth = o.minThumbSize;
                $(self.scrollbarXThumb).width(self.thumbWidth);
            }
            // Y-axis
            if (axis == 'y') {
                if ((self.elemHeight / self.scrollHeight) < 1) {
                    el.parent().width(self.elemWidth - 5);
                    self.scrollbarY.css('display', 'block');
                    self.thumbHeight = Math.round(self.elemHeight * (self.elemHeight / self.scrollHeight));
                    if (self.thumbHeight < o.minThumbSize) self.thumbHeight = o.minThumbSize;
                }
                else {
                    el.parent().css('width', '');
                    self.scrollbarY.css('display', 'none');
                }

                $(self.scrollbarYThumb).height(self.thumbHeight);
            }
        },

        /**
        * Option Setter
        *
        * @param	key		The option name/key
        * @param	value	The option value
        */
        _setOption: function (key, value) {
            var self = this,
				el = self.element,
				o = self.options;
        }

    }); //end of  $.widget

})(jQuery);

// Disable Selection Plugin
//(function ($) {

//    $.fn.disableSelection = function () {
//        return this.each(function () {
//            $(this).attr('unselectable', 'on')
//				   .css({
//				       '-ms-user-select': 'none',
//				       '-moz-user-select': 'none',
//				       '-webkit-user-select': 'none',
//				       'user-select': 'none'
//				   })
//				   .each(function () {
//				       this.onselectstart = function () { return false; };
//				   });
//        });
//    };

//})(jQuery);