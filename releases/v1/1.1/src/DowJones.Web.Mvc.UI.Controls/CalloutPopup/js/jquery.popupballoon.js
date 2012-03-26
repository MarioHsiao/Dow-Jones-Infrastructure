/*
* jQuery popupBalloon
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
    var mouseX = 0;
    var mouseY = 0;

    $(document).mousemove(function (e) {
        mouseX = e.pageX
        mouseY = e.pageY
    });

    $.widget("ui.popupBalloon", {
        options: {
            width: 400, // The width of the popup. Value can be an integer or 'auto'
            height: 130, // The height of the popup. Value can be an integer or 'auto'
            positionX: null, // Absolute X position of the popup. Optional. Note: leaving it to null will force the popup to use the target element position instead.
            positionY: null, // Absolute Y position of the popup. Optional .Note: leaving it to null will force the popup to use the target element position instead.
            popupClass: '', // Custom popup css class
            title: '', // Title
            content: '', // Content (HTML)
            jScrollPaneEnabled: true, // Enable the jScrollPane plugin for custom scrollbar style
            insetMargin: 0, // Postion of the popup relative to the element edges
            animationDistance: 50, // Distance for the translation animation
            positioningMode: 'normal', // Positioning Mode ('normal', 'cursor')
            onHide: function () { }, // Callback function
            onShow: function () { } // Callback function
        },

        /**
        * Plugin Creation. (Executed automatically)
        */
        _create: function () {
        },

        /**
        * Plugin Initialization. (Executed automatically)
        */
        _init: function () {
            var self = this,
			el = self.element,
			o = self.options;
            self.visible = false;
            if ($('.popup-balloon').length) {
                if ($(el).data('timestamp') == $('.popup-balloon').data('timestamp')) {
                    self.hide();
                }
                else {
                    self.show();
                }
            }
            else {
                self.show();
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
            if (self.popupBox) {
                switch (key) {
                    case 'popupClass':
                        self.popupBox.removeClass().addClass('popup-balloon').addClass(value);
                        if (o.title) {
                            self.popupBox.addClass('with-title');
                        }
                        break;
                    case 'content':
                        self.popupBox.find('.content').empty().html(value);
                        if (o.jScrollPaneEnabled) {
                            self.popupBox.find('.content').jScrollPane({
                                showArrows: true,
                                autoReinitialise: false,
                                verticalDragMaxHeight: 52,
                                verticalGutter: 10,
                                horizontalGutter: 20
                            });
                        }
                        break;
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

            // Hide previously opened instances
            //            if ($('.popup-balloon').length) {
            //                self._hide(true);
            //            }

            // TimeStamp
            var timestamp = new Date().getTime();

            var popbox = $('<div class="popup-balloon"><div class="balloon-arrow"></div><div class="popup-body"><div class="content"></div></div></div>');

            popbox.data("timestamp", timestamp);
            el.data("timestamp", timestamp);

            // Add Custom Class
            if (o.popupClass) {
                popbox.addClass(o.popupClass);
            }

            // Add Title
            if (o.title) {
                popbox.addClass('with-title');
                popbox.prepend('<div class="header"><div class="title ellipsis">' + o.title + '</div></div>');
            }

            // Add Content
            if (o.content) {
                popbox.find('.content').empty().html(o.content);
                // Enable jScrollPane
                if (o.jScrollPaneEnabled) {
                    popbox.find('.content').jScrollPane({
                        showArrows: true,
                        autoReinitialise: false,
                        verticalDragMaxHeight: 52,
                        verticalGutter: 10,
                        horizontalGutter: 20
                    });
                }
            }

            // Calculate Size
            var headerHeight = popbox.find('.header').height();
            var contentHeight = o.height - headerHeight;

            popbox.find('.popup-body, .title').width(o.width);
            popbox.find('.content').height(contentHeight);

            $('body').append(popbox);

            this.popupBox = popbox;

            // Position & Show Element			
            self.positionElement(true);

            // Bind Mouse Events
            popbox.bind('mousedown', function (e) {
                e.stopPropagation();
            });

            window.setTimeout(function () {
                $("body").bind('mousedown.popupBalloon', function (e) {
                    self.hide();
                    e.stopPropagation();
                    $("body").unbind('mousedown.popupBalloon');
                });
            }, 20);

            this.visible = true;
            // Execute callback function
            o.onShow();
        },

        /**
        *  Hide (remove) popbox elements from the page. (internal function)
        */
        _hide: function (noAnimation) {
            var self = this;
            $('.popup-balloon').fadeOut(200, function () {
                $(this).remove();
                if ($.isFunction(self.options.onHide))
                    self.options.onHide();
            });
        },

        /**
        *  Hide (remove) popbox elements from the page. (public function)
        */
        hide: function () {
            this._hide();
            this.visible = false;
        },

        /**
        *  Position the popbox element
        *
        *  @param	elem	Element
        */
        positionElement: function () {
            this.position(true);
        },

        position: function (animate) {
            var self = this,
			el = self.element,
			o = self.options,
            $elem = self.popupBox;

            var arrowWidth = $elem.find('.balloon-arrow').width();
            var arrowHeight = $elem.find('.balloon-arrow').height();
            var elemHeight = $elem.height();
            var elemWidth = $elem.width();

            // Get coordinates
            if ((o.positionX || o.positionX === 0) || (o.positionY || o.positionY === 0)) {
                var x = o.positionX;
                var y = o.positionY;
            }
            else {
                var x = $(el).offset().left + $(el).width();
                var y = $(el).offset().top + $(el).height() / 2;
            }

            // Normal Positioning Mode
            if (o.positioningMode == 'normal') {
                // Center arrow vertically
                $elem.find('.balloon-arrow').css('top', (elemHeight / 2) - (arrowHeight / 2) + 'px');

                // Center balloon vertically
                $elem.css('top', (y - (elemHeight / 2)) + 'px');

                // Align element to the right
                $elem.find('.balloon-arrow').addClass('arrow-left');
                $elem.css({ 'left': (arrowWidth + x) + o.animationDistance - o.insetMargin + 'px', 'display': 'block', opacity: 0 });

                // If the element appears outside of the window, align it to the left
                if ((x + arrowWidth + elemWidth) > $(window).width()) {
                    $elem.find('.balloon-arrow').removeClass('arrow-left').addClass('arrow-right');
                    if (!$.browser.msie) {
                        if (!isNaN($(el).width())) {
                            x = x - $(el).width();
                        }
                        // Force element to display inside of the viewable area
                        if (x < (arrowWidth + elemWidth)) {
                            x = ($(window).width() / 2);
                        }
                    }

                    if (animate) {
                        $elem.css('left', (x - elemWidth - arrowWidth) - o.animationDistance + o.insetMargin + 'px');
                        $elem.animate({ opacity: 1, 'left': '+=' + o.animationDistance }, 300, function () { $(this).css('zoom', '').css('filter', '') });
                    }
                    else {
                        $elem.css({ 'left': (x - elemWidth - arrowWidth) + o.insetMargin + 'px', 'zoom': '', 'filter': '', opacity: 1 });
                    }
                }
                else {
                    if (animate)
                        $elem.animate({ opacity: 1, 'left': '-=' + o.animationDistance }, 300, function () { $(this).css('zoom', '').css('filter', '') });
                    else
                        $elem.css({ 'left': (arrowWidth + x) - o.insetMargin + 'px', 'zoom': '', 'filter': '', opacity: 1 });
                }
            } // Cursor Positioning Mode
            else if (o.positioningMode == 'cursor') {
                $elem.css({ 'top': (mouseY + 20) + 'px', 'left': (mouseX) + 'px', 'display': 'block', opacity: 0 }).animate({ opacity: 1 });
            }
            self.scrollDocument();
        },

        rePosition: function () {
            var self = this,
                $callout = this.popupBox,
                popupOffset = $callout.offset(),
                currArrowPos = $('.balloon-arrow', $callout).position(),
                contentH = $('div.content', $callout).outerHeight(),
                newContentH = 0,
                sizeDiff = 0;

            // if the content area has children
            if ($('div.content', $callout).children().size() > 0) {
                //calculate the total height of all the children within the content div
                $('div.content', $callout).children().each(function () {
                    newContentH = newContentH + $(this).outerHeight();
                });

                if (newContentH > contentH) {
                    sizeDiff = newContentH - contentH;

                    $($callout).animate({
                        top: popupOffset.top - (sizeDiff / 2) + "px"
                    }, 200);
                    $('.balloon-arrow', $callout).animate({
                        top: currArrowPos.top + (sizeDiff / 2) + "px"
                    }, 200);
                    $('div.content', $callout).animate({
                        height: newContentH
                    }, 200, function () {
                        // after the animation is complete reset the styles
                        $('div.content', $callout).css({
                            height: "",
                            overflow: "visible"
                        });
                        self.scrollDocument();
                    });
                }

                $('div.content', $callout).children().show();
            }

        },

        scrollDocument: function () {
            var $callout = this.popupBox,
            popupOffset = $callout.offset(),
            popupHeight = $callout.outerHeight(),
            winH = $(window).height(),
            scrollTop = ($.iDevices.iPad ? window.pageYOffset : $(document).scrollTop()),
            scrollValue = 0;

            if ((winH + scrollTop - popupOffset.top) < popupHeight) {
                scrollValue = (popupHeight - (winH - popupOffset.top)) + 1;
            }
            else if (scrollTop > 0 && (scrollTop - popupOffset.top) > 0) {
                scrollValue = popupOffset.top - 1;
            }

            if (scrollValue) {
                $('html,body').animate({ scrollTop: scrollValue }, 500);
            }
        },

        getPopupBox: function () {
            return this.popupBox;
        },

        isVisible: function () {
            return this.visible;
        }
    });

})(jQuery);