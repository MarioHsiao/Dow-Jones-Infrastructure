/*!
 * ScrollBar
 *
 * @dependency jquery.ui.core.js
 * @dependency jquery.ui.widget.js
 * @dependency jquery.validate.js
 */
DJ.UI.ScrollBar = DJ.UI.Component.extend({
    // Default options
    defaults: {
        scrollbarY: '',  //ScrollbarY elements
        scrollbarYThumb: '',
        scrollbarX: '', //ScrollbarY elements
        scrollbarXThumb: '',
        elemHeight: '', // Dynamic properties:
        elemWidth: '',
        scrollX: false,
        scrollY: false,
        scrollHeight: '',
        scrollWidth: '',
        scrollTop: '',
        scrollLeft: '',
        maxScrollTop: '',
        maxScrollLeft: '',
        thumbHeight: '',
        dragged: false // Event-related properties:
    },

    // Widget options
    options: {
        minThumbSize: 30, // Minimum size of the scrollbar handle
        autoRefresh: false // Experimental feature based on the DOMSubtreeModified event. (Gegko and Wekbkit only) Setting this value to true will automatically recalculate the scrollbar size when the content is updated.
    },

    selectors: {
        scrollbar: '.dj_scrollbar',
        scrollbarthumb: '.dj_scrollbar-thumb',
        scrollbartrack: '.dj_scrollbar-track'
    },

    classNames: {
        scrollbar: 'dj_scrollbar',
        scrollbarinit: 'dj_scrollbar-init',
        scrollbarios: 'dj_scrollbar-ios',
        scrollbary: 'dj_scrollbar-y',
        scrollbarx: 'dj_scrollbar-x',
        scrollbarthumb: 'dj_scrollbar-thumb',
        scrollbartrack: 'dj_scrollbar-track'
    }, 

    templates: {
        templateX: _.template('<div class="<%= scrollbar %>" style="width:<%= elemWidth %>px; margin-top:<%= elemHeight %>px; position:relative;"><div class="<%= scrollbartrack %>" style="width:<%= elemWidth %>px;"><div class="<%= scrollbarthumb %>"></div></div></div>'),
        templateY: _.template('<div class="<%= scrollbar %>" style="height:<%= elemHeight %>px; margin-left:<%= elemWidth %>px; position:relative;"><div class="<%= scrollbartrack %>" style="height:<%= elemHeight %>px;"><div class="<%= scrollbarthumb %>"></div></div></div>')
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "ScrollBar" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        this._scrollable();
    },

    _scrollable: function () {
        var self = this,
            $parentContainer = this.$element;

        $parentContainer.addClass(self.classNames.scrollbarinit);

        // Browser Detection
        if ($.browser.mozilla) {
            self._renderFireFoxScrollbar();
        }

        // Mobile
        var deviceAgent = navigator.userAgent.toLowerCase();
        var ios = deviceAgent.match(/(iphone|ipod|ipad)/);
        if (ios) {
            $parentContainer.addClass(self.classNames.scrollbarios);
        }
    },

    _initializeElements: function (ctx) {
        //ctx is this.$element
        //this.$scrollbarthumb = ctx.find(this.selectors.scrollbarthumb);
        //this.$scrollbartrack = ctx.find(this.selectors.scrollbartrack);
    },

    /**
    * Render a custom scrollbar
    */
    _renderFireFoxScrollbar: function () {
        var self = this,
			$parentContainer = self.$element,
			o = self.options;

        self.elemHeight = $parentContainer.height();
        self.elemWidth = $parentContainer.width();

        // X-axis
        if ($parentContainer.hasClass(self.classNames.scrollbarx)) {
            self.scrollX = true;
            var paddingBottom = parseInt($parentContainer.css('padding-bottom').replace(/[^-\d\.]/g, ''));

            $parentContainer.height(self.elemHeight + 15)
                            .width(self.elemWidth);

            //var scrollbarX = $.validator.format('<div class="{0}" style="width:{1}px; margin-top:{2}px; position:relative;"><div class="{3}" style="width:{1}px;"><div class="{4}"></div></div></div>', self.selectors.scrollbar.replace(".", ""), self.elemWidth, self.elemHeight, self.selectors.scrollbartrack.replace(".", ""), self.selectors.scrollbarthumb.replace(".", ""));
            //self.selectors.scrollbar.replace(".", ""), self.elemWidth, self.elemHeight, self.selectors.scrollbartrack.replace(".", ""), self.selectors.scrollbarthumb.replace(".", "")
            var scrollbarX = self.templates.templateX({
                                                        scrollbar: self.classNames.scrollbar,
                                                        elemWidth: self.elemWidth, 
                                                        elemHeight: self.elemHeight,
                                                        scrollbartrack: self.classNames.scrollbartrack,
                                                        scrollbarthumb: self.classNames.scrollbarthumb
                                                      });

            $parentContainer.css('position', 'absolute')
                            .css('clip', 'rect(0px ' + self.elemWidth + 'px ' + (self.elemHeight + paddingBottom) + 'px 0px)')
                            .after(scrollbarX);

            self.scrollbarX = $parentContainer.siblings(self.selectors.scrollbar).first();
            self.scrollbarXThumb = $(self.scrollbarX).find(self.selectors.scrollbarthumb);
            self.scrollbarXTrack = $(self.scrollbarX).find(self.selectors.scrollbartrack);

            // Prevent Scrollbar selection
            self._disableSelection($(self.scrollbarX));
            self._disableSelection($(self.selectors.scrollbarthumb));
            self._disableSelection($(self.selectors.scrollbartrack));

            self.scrollbarXThumb.bind('click', function (event) {
                event.stopPropagation();
            })

            // Track clicks
            self.scrollbarXTrack.bind('click', function (event) {
                self.dragged = true;

                var position = 0;
                var mouseX = event.pageX - $(this).offset().left;
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
                $parentContainer.animate({ scrollLeft: ((position * self.maxScrollLeft) / (self.elemWidth - self.thumbWidth)) }, 200);
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
                $parentContainer.bind('DOMSubtreeModified', function () {
                    self._updateThumbSize('x');
                    self._updateThumbPosition('x');
                });
            }

            $parentContainer.scroll(function () { if (!self.dragged) { self._updateThumbPosition('x') } });
            self._updateThumbSize('x');
            self._updateThumbPosition('x');
        }
        // Y-axis
        if ($parentContainer.hasClass(self.classNames.scrollbary)) {
            self.scrollY = true;
            var paddingRight = parseInt($parentContainer.css('padding-right').replace(/[^-\d\.]/g, ''));

            $parentContainer.width(self.elemWidth + 5);

            //var scrollbarY = $.validator.format('<div class="{0}" style="height:{1}px; margin-left:{2}px; position:relative;"><div class="{3}" style="height:{1}px;"><div class="{4}"></div></div></div>', self.selectors.scrollbar.replace(".", ""), self.elemHeight, self.elemWidth, self.selectors.scrollbartrack.replace(".", ""), self.selectors.scrollbarthumb.replace(".", ""));
            var scrollbarY = self.templates.templateY({
                                                        scrollbar: self.classNames.scrollbar,
                                                        elemHeight: self.elemHeight,
                                                        elemWidth: self.elemWidth,
                                                        scrollbartrack: self.classNames.scrollbartrack,
                                                        scrollbarthumb: self.classNames.scrollbarthumb
                                                     });

            $parentContainer.css('position', 'absolute')
                            .css('clip', 'rect(0px ' + (self.elemWidth - paddingRight) + 'px ' + self.elemHeight + 'px 0px)')
                            .after(scrollbarY);

            self.scrollbarY = $parentContainer.siblings(self.selectors.scrollbar).first();
            self.scrollbarYThumb = $(self.scrollbarY).find(self.selectors.scrollbarthumb);
            self.scrollbarYTrack = $(self.scrollbarY).find(self.selectors.scrollbartrack);

            // Prevent Scrollbar selection
            self._disableSelection($(self.scrollbarY));
            self._disableSelection($(self.selectors.scrollbarthumb));
            self._disableSelection($(self.selectors.scrollbartrack));

            self.scrollbarYThumb.bind('click', function (event) {
                event.stopPropagation();
            })

            // Track clicks
            self.scrollbarYTrack.bind('click', function (event) {
                self.dragged = true;

                var position = 0;
                var mouseY = event.pageY - $(this).offset().top;
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
                $parentContainer.animate({ scrollTop: ((position * self.maxScrollTop) / (self.elemHeight - self.thumbHeight)) }, 200);
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
                $parentContainer.bind('DOMSubtreeModified', function () {
                    self._updateThumbSize('y');
                    self._updateThumbPosition('y');
                });
            }

            $parentContainer.scroll(function () { if (!self.dragged) { self._updateThumbPosition('y') } });
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
			el = self.element;

        // X-axis
        if (axis == 'x') {
            var thumbLeft = $(self.scrollbarXThumb).css('left').replace(/[^-\d\.]/g, '');
            $(el).scrollLeft((thumbLeft * self.maxScrollLeft) / (self.elemWidth - self.thumbWidth));
        }

        // Y-axis
        if (axis == 'y') {
            var thumbTop = $(self.scrollbarYThumb).css('top').replace(/[^-\d\.]/g, '');
            $(el).scrollTop((thumbTop * self.maxScrollTop) / (self.elemHeight - self.thumbHeight));
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
			el = $(self.element),
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
                $(el).parent().width(self.elemWidth - 5);
                self.scrollbarY.css('display', 'block');
                self.thumbHeight = Math.round(self.elemHeight * (self.elemHeight / self.scrollHeight));
                if (self.thumbHeight < o.minThumbSize) self.thumbHeight = o.minThumbSize;
            }
            else {
                $(el).parent().css('width', '');
                self.scrollbarY.css('display', 'none');
            }
            $(self.scrollbarYThumb).height(self.thumbHeight);
        }
    },

    _disableSelection: function (el) {
        //console.log(el);
        return el.each(function () {
            $(el).attr('unselectable', 'on')
    				   .css({
    				       '-ms-user-select': 'none',
    				       '-moz-user-select': 'none',
    				       '-webkit-user-select': 'none',
    				       'user-select': 'none'
    				   })
    				   .each(function () {
    				       el.onselectstart = function () { return false; };
    				   });
        });
    },

    /**
    * Force a scrollbar refresh
    */
    refresh: function () {
        if (this.scrollX) {
            this._updateThumbSize('x');
            this._updateThumbPosition('x');
        }
        else if (this.scrollY) {
            this._updateThumbSize('y');
            this._updateThumbPosition('y');
        }
    }

    /* end of the scroll functionality */
});


// Declare this class as a jQuery plugin
$.plugin('dj_ScrollBar', DJ.UI.ScrollBar);
$dj.debug("ScrollBar");