/**
* Popup Balloon (callout) Plugin
*
* @category   jQuery UI Widget
* @author     Philippe Arcand <Philippe.Arcand@dowjones.com>
* @author     Ron Edgecomb <Ronald.EdgecombII@dowjones.com>
* @copyright  2011 Dow Jones & Company Inc.
* @version    SVN: $Id: jquery.popupballoon.js 2117 2011-07-08 16:49:46Z redgecomb $
* @dependency jquery.ui.core.js
* @dependency jquery.ui.widget.js
*/

var mouseX = 0;
var mouseY = 0;

(function ($, undefined) {

    $(document).mousemove(function (e) {
        mouseX = e.pageX;
        mouseY = e.pageY;
    });

    $.widget("ui.popupBalloon", {
        options: {
            //*** existing options ***//

            popupClass: '', 				// Custom popup css class
            width: 400, 					// The width of the popup. Value can be an integer or 'auto'
            height: 130, 				// The height of the popup. Value can be an integer or 'auto'

            positionX: null, 			// Absolute X position of the popup. Optional. Note: leaving it to null will force the popup to use the target element position instead.
            positionY: null, 			// Absolute Y position of the popup. Optional .Note: leaving it to null will force the popup to use the target element position instead.

            title: '', 					// Title
            content: '', 				// Content (HTML)
            jScrollPaneEnabled: true, 		// Enable the jScrollPane plugin for custom scrollbar style
            insetMargin: 0, 				// Postion of the popup relative to the element edges
            animationDistance: 50, 			// Distance for the translation animation
            positioningMode: 'normal', 		// Positioning Mode ('normal', 'cursor')

            onHide: function () { }, 		// Callback function
            onShow: function () { }, 		// Callback function

            //*** new options ***//

            xAlign: 'right', 				//Horizontal starting point for X coordinate relative to element triggering popup, ('left','right','center')
            xOffset: 0, 					// Offset value to shift inital X coordinate used in positioning popup

            yAlign: 'center', 				//Vertical starting point for Y coordinate relative to element triggering popup, ('top','center','bottom')
            yOffset: 0, 					// Offset value to shift inital Y coordinate used in positioning popup

            popupPosition: 'right', 		// Popup position relative to the click ('top','right','bottom','left'). This is treated more so as the preferred location, left and top may be replaced with right and bottom respectively based on available space.
            popupAlign: 'center', 		// Alignment of popup relative to click. Options for left and right positioned ('top','bottom','center'). Options for top and bottom positioned ('right','left','center').

            arrowEnabled: true, 			// Enable arrow in callout, arrow will be positioned based on popupPosition	

            animateDisplayEnabled: true, 	// Enable the animation of the popup balloon display
            animateMoveEnabled: true, 		// Enable the animation of the popup balloon moving
            animateHideEnabled: true, 		// Enable the animation of the popup balloon hiding
            animationSpeed: 300,

            interactionMode: 'none'			// Interaction Mode ('none', 'hover', 'custom')
        },

        /**
        * Plugin Creation. (Executed automatically)
        */
        _create: function () {
            var self = this,
				el = self.element,
				o = self.options;

            if (!$.iDevices) {
                self.iDevices = {
                    iPad: (navigator.userAgent.indexOf('iPad') != -1)
                };
            } else {
                self.iDevices = $.iDevices;
            }

            self.preferences = {
                popupPosition: o.popupPosition,
                popupalign: o.popupAlign
            };

            window.setTimeout(function () {
                $("body").bind('mousedown', function (e) {
                    self.hide();
                    e.stopPropagation();
                });
            }, 20);
        },

        /**
        * Plugin Initialization. (Executed automatically)
        */
        _init: function () {
            var self = this,
			el = self.element,
			o = self.options;
            self.visible = false;

            //if the display animation is disabled, zero out the animation distance
            if (!o.animateDisplayEnabled) {
                o.animationDistance = 0;
            }

            //reset based on original preferences, the space may now be availabe to accommodate				
            o.popupPosition = self.preferences.popupPosition;
            o.popupalign = self.preferences.popupalign;

            switch (o.interactionMode) {

                case 'custom':
                case 'none':
                    if ($('.popup-balloon').length) {
                        // Use the timestamp to detect if the popup is linked to the target element
                        if ($(el).data('timestamp') == $('.popup-balloon').data('timestamp')) {
                            self.hide();
                        } else {
                            self.show();
                        }
                    } else {
                        self.show();
                    }
                    break;
                case 'hover':
                    $(el).bind('mouseenter', function () {
                        $(el).addClass('.popup-trigger')
                        clearTimeout($.data(this, 'bubbleTimeout'));
                        var ibubbleIsOpened = $(this).data("ibubbleIsOpened");
                        if (!ibubbleIsOpened) {
                            $(this).data("ibubbleIsOpened", true);

                            $('.popup-trigger').each(function () {
                                $(this).removeData("ibubbleIsOpened");
                                clearTimeout($(this).data("bubbleTimeout"));
                            });

                            self.show();
                        }
                    });

                    $(el).bind('mouseleave', function () {
                        $(this).data("bubbleTimeout", window.setTimeout(function () {
                            var ibubbleIsOpened = $(el).data("ibubbleIsOpened");

                            if (ibubbleIsOpened) {
                                $(el).removeData("ibubbleIsOpened");
                                $(el).popupBalloon('hide');
                                clearTimeout($(el).data("bubbleTimeout"));
                            }
                        }, 300));
                    });

                    break;
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
                        // Has title
                        if (o.title) {
                            self.popupBox.addClass('with-title');
                        }

                        // Has Arrow
                        if (o.arrowEnabled) {
                            self.popupBox.addClass('with-arrow');
                        }
                        break;
                    case 'title':
                        self.popupBox.find('.title').text(value);
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
                    default:
                        o[key] = value;
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

            // TimeStamp
            var timestamp = new Date().getTime();

            // Hide previously opened instances
            if (el.data("popbox")) {
                self._hide();
            }

            // Create popup
            var popbox = $('<div class="popup-balloon"><div class="balloon-arrow"></div><div class="popup-body"><div class="content"></div></div></div>');

            popbox.data("controller", el);
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
            }

            // Has Arrow
            if (o.arrowEnabled) {
                popbox.addClass('with-arrow');
            }

            $('body').append(popbox);

            // Enable jScrollPane
            if (o.jScrollPaneEnabled) {
                popbox.find('.content').jScrollPane({
                    showArrows: true,
                    autoReinitialise: true,
                    verticalDragMaxHeight: 52,
                    verticalGutter: 10,
                    horizontalGutter: 20
                });
            }

            

            

            // Calculate/Fix Size
            var headerHeight = popbox.find('.header .title').outerHeight(),
				contentHeight = o.height - headerHeight;

            popbox.find('.popup-body, .header .title').outerWidth(o.width);
            popbox.find('.content').outerHeight(contentHeight);

            popbox.attr({
                'position': o.popupPosition,
                'popupalign': o.popupAlign
            });

            // Position & Show Element			
            var coordinates = self._getCoordinates(popbox),
				x = coordinates.x,
				y = coordinates.y;

            // Bind Mouse Events
            popbox
				.bind('mousedown', function (e) {
				    e.stopPropagation();
				    self._trigger('popupMouseDown', e, popbox);
				})
				.bind('mouseenter', function (e) {
				    self._trigger('popupMouseEnter', e, popbox);
				})
				.bind('mouseleave', function (e) {
				    self._trigger('popupMouseLeave', e, popbox);
				});

            if (o.interactionMode == 'hover') {
                popbox.bind('mouseenter', function () {
                    clearTimeout($(el).data("bubbleTimeout"));
                });
                popbox.bind('mouseleave', function () {
                    $(this).data("bubbleTimeout", window.setTimeout(function () {
                        var ibubbleIsOpened = $(el).data("ibubbleIsOpened");

                        if (ibubbleIsOpened) {
                            $(el).removeData("ibubbleIsOpened");
                            $(el).popupBalloon('hide');
                            clearTimeout($(el).data("bubbleTimeout"));
                        }
                    }, 300));
                });
            }

            el.data("popbox", popbox);
            this.popupBox = popbox;

            self.positionElement(popbox, x, y);

            this.visible = true;

            // Execute callback function
            self._trigger('onShow');
        },

        /**
        *  Hide (remove) popbox elements from the page. (internal function)
        */
        _hide: function (noAnimation) {
            var self = this,
				el = self.element,
				o = self.options;

            // Remove popbox elements
            var $selector = (o.interactionMode == 'none') ? $('.popup-balloon') : el.data("popbox");

            if ($selector) {

                // Remove all menu instances
                if (o.animateHideEnabled) {
                    $selector.fadeOut(o.animationSpeed, function () {
                        $(this).remove();
                    });
                } else {
                    $selector.remove();
                }

            }
        },

        /**
        *  Hide (remove) popbox elements from the page. (public function)
        */
        hide: function () {
            var self = this;

            self._hide();
            self.visible = false;

            // Execute callback function
            self._trigger('onHide');
        },

        /**
        *  Position the popbox element
        *
        *  @param	elem	Element
        *  @param	x	x coordinate
        *  @param	y	y coordinate
        */
        positionElement: function (elem, x, y) {

            var self = this,
				el = self.element,
				o = self.options,

				animateX = (o.popupPosition == 'right') ? o.animationDistance : (o.popupPosition == 'left') ? (o.animationDistance * -1) : 0,
				animateY = (o.popupPosition == 'bottom') ? o.animationDistance : (o.popupPosition == 'top') ? (o.animationDistance * -1) : 0;

            if (o.animateDisplayEnabled) {

                $(elem).css({
                    top: (y + animateY) + 'px',
                    left: (x + animateX) + 'px',
                    display: 'block'

                });

                $(elem).stop().animate({
                    left: '-=' + animateX,
                    top: '-=' + animateY
                }, o.animationSpeed, function () {
                    $(this).css({
                        opacity: '',
                        zoom: '',
                        filter: ''
                    });
                    self.scrollDocument();
                });

            } else {

                $(elem).stop().css({
                    top: y + 'px',
                    left: x + 'px',
                    display: 'block',
                    opacity: '',
                    zoom: '',
                    filter: ''
                });
                self.scrollDocument();
            }


        },

        /**
        *  Update the position of the current popbox element
        *
        *  @param	posX	X coordinate of popup box
        *  @param	posY	Y coordinate of popup box
        */
        updatePosition: function (posX, posY) {

            var self = this,
				el = self.element,
				o = self.options,
				$popbox = el.data("popbox");

            o.positionX = posX;
            o.positionY = posY;

            //reset based on original preferences, the space may now be availabe to accommodate				
            o.popupPosition = self.preferences.popupPosition;
            o.popupalign = self.preferences.popupalign;

            var coordinates = self._getCoordinates($popbox),
				x = coordinates.x,
				y = coordinates.y;

            $popbox.attr({
                'position': o.popupPosition,
                'popupalign': o.popupAlign
            });

            if (o.animateMoveEnabled) {
                $popbox.stop().animate({
                    top: y,
                    left: x,
                    opacity: 1
                }, o.animationSpeed, function () {
                    $(this).css({
                        opacity: '',
                        zoom: '',
                        filter: ''
                    });
                    self.scrollDocument();
                });
            } else {
                $popbox.css({
                    top: y,
                    left: x,
                    opacity: '',
                    zoom: '',
                    filter: ''
                });
                self.scrollDocument();
            }

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
            var self = this,
				$callout = this.popupBox,
	            popupOffset = $callout.offset(),
	            popupHeight = $callout.outerHeight(),
	            winH = $(window).height(),
	            scrollTop = (self.iDevices.iPad ? window.pageYOffset : $(document).scrollTop()),
	            scrollValue = 0;

            if ((winH + scrollTop - popupOffset.top) < popupHeight) {
                scrollValue = (popupHeight - (winH - popupOffset.top)) + 1;
            }
            else if (scrollTop > 0 && (scrollTop - popupOffset.top) > 0) {
                scrollValue = popupOffset.top - 1;
            }

            if (scrollValue) {
                $('html,body').stop().animate({ scrollTop: scrollValue }, 500);
            }
        },

        getPopupBox: function () {
            return this.popupBox;
        },

        isVisible: function () {
            return this.visible;
        },

        /**
        *  Position the arrow based on configuration and available space
        *
        */
        _getCoordinates: function (elem) {

            var self = this,
				el = self.element,
				o = self.options,

				elemHeight = $(elem).outerHeight(),
				elemWidth = $(elem).outerWidth(),

				$arrow = $(elem).find('.balloon-arrow'),
				arrowWidth = (o.arrowEnabled) ? $arrow.outerWidth() : 0,
				arrowHeight = (o.arrowEnabled) ? $arrow.outerHeight() : 0;

            if ((o.positionX || o.positionX === 0) || (o.positionY || o.positionY === 0)) {
                var x = o.positionX,
					y = o.positionY;
            } else if (o.positioningMode == 'cursor') {
                var x = mouseX,
					y = mouseY;
            } else {

                switch (o.xAlign) {
                    case 'left':
                        var x = $(el).offset().left;
                        break;

                    case 'right':
                        var x = $(el).offset().left + $(el).outerWidth();
                        break;

                    case 'center':
                    default:
                        var x = $(el).offset().left + ($(el).outerWidth() / 2);
                        break;

                }

                switch (o.yAlign) {
                    case 'top':
                        var y = $(el).offset().top;
                        break;

                    case 'bottom':
                        var y = $(el).offset().top + $(el).outerHeight();
                        break;

                    case 'center':
                    default:
                        var y = $(el).offset().top + ($(el).outerHeight() / 2);
                        break;

                }

            }

            x += o.xOffset;
            y += o.yOffset;

            // update X based on arrow location
            switch (o.popupPosition) {
                case 'top':

                    //look for beginning of page
                    if ((y - arrowHeight - elemHeight - o.animationDistance + o.insetMargin) < 0) {

                        //adjust y position
                        y = (y + arrowHeight - o.insetMargin);
                        //update option
                        o.popupPosition = 'bottom';
                        //add proper arrow class
                        $(elem).attr('position', 'bottom');
                    } else {
                        //adjust y position
                        y = (y - arrowHeight - elemHeight + o.insetMargin);
                        //add proper arrow class
                        $(elem).attr('position', 'top');
                    }

                    break;

                case 'left':
                    // If the element appears outside of the window, align it to the left
                    if ((x - arrowWidth - elemWidth - o.animationDistance + o.insetMargin) < 0) {
                        //adjust x position
                        x = (x + arrowWidth - o.insetMargin);
                        //update option
                        o.popupPosition = 'right';
                        //add proper arrow class
                        $(elem).attr('position', 'right');
                    } else {
                        //adjust x position
                        x = (x - arrowWidth - elemWidth + o.insetMargin);
                        //add proper arrow class
                        $(elem).attr('position', 'left');
                    }

                    break;

                case 'bottom':

                    //look for end of page
                    if ((y + arrowHeight + elemHeight + o.animationDistance - o.insetMargin) > $(document).height()) {
                        //adjust y position
                        y = (y - arrowHeight - elemHeight + o.insetMargin);
                        //update option
                        o.popupPosition = 'top';
                        //add proper arrow class
                        $(elem).attr('position', 'top');
                    } else {
                        //adjust y position
                        y = (y + arrowHeight - o.insetMargin);
                        //add proper arrow class
                        $(elem).attr('position', 'bottom');
                    }

                    break;

                case 'right':
                default:
                    // If the element appears outside of the window, align it to the right
                    if ((x + arrowWidth + elemWidth + o.animationDistance - o.insetMargin) > $(window).width()) {
                        //adjust x position
                        x = (x - arrowWidth - elemWidth + o.insetMargin);
                        //update option
                        o.popupPosition = 'left';
                        //add proper arrow class
                        $(elem).attr('position', 'left');
                    } else {
                        //adjust x position
                        x = (x + arrowWidth - o.insetMargin);
                        //add proper arrow class
                        $(elem).attr('position', 'right');
                    }

                    break;
            }

            var popupFits = false,
				fitTries = 0;

            // some x and y tweaking based on arrow alignment
            if (o.arrowEnabled) {

                //reset the positioning
                $arrow.css({
                    top: '',
                    right: '',
                    bottom: '',
                    left: ''
                });

                while (!popupFits && fitTries < 3) {

                    fitTries++;

                    switch (o.popupAlign) {
                        case 'top':

                            var alignY = y - (arrowHeight / 2);

                            if ((alignY + elemHeight) <= $(document).height()) {

                                popupFits = true;

                                //adjust y position
                                y = alignY;

                                //assumes left or right position
                                $(elem).attr('popupalign', 'top');
                                $arrow.css('top', 0);

                            } else {

                                o.popupAlign = 'center';

                            }

                            break;

                        case 'right':

                            var alignX = x - (elemWidth - (arrowWidth / 2));

                            if (alignX > 0) {

                                popupFits = true;

                                //adjust x position
                                x = alignX;

                                //assumes top or bottom position
                                $(elem).attr('popupalign', 'right');
                                $arrow.css('right', 0);

                            } else {

                                o.popupAlign = 'left';

                            }

                            break;

                        case 'bottom':

                            var alignY = y - (elemHeight - (arrowHeight / 2));

                            if (alignY > 0) {

                                popupFits = true;

                                //adjust y position
                                y = alignY;

                                //assumes left or right position
                                $(elem).attr('popupalign', 'bottom');
                                $arrow.css('bottom', 0);

                            } else {

                                o.popupAlign = 'top';

                            }

                            break;

                        case 'left':

                            var alignX = x - (arrowWidth / 2);

                            if ((alignX + elemWidth) <= $(window).width()) {

                                popupFits = true;

                                //adjust x position
                                x = alignX;

                                //assumes top or bottom position
                                $(elem).attr('popupalign', 'left');
                                $arrow.css('left', 0);

                            } else {

                                o.popupAlign = 'center';

                            }

                            break;

                        case 'center':
                        default:

                            if (o.popupPosition == 'left' || o.popupPosition == 'right') {

                                var alignY = y - (elemHeight / 2);

                                if ((alignY + elemHeight) <= $(document).height() && alignY > 0) {

                                    popupFits = true;

                                    //adjust y position
                                    y = alignY;

                                    //assumes top or bottom position
                                    $arrow.css('top', ((elemHeight / 2) - (arrowHeight / 2)) + 'px');

                                    //assumes left or right position
                                    $(elem).attr('popupalign', 'center');

                                } else {

                                    o.popupAlign = 'bottom';

                                }

                            } else if (o.popupPosition == 'top' || o.popupPosition == 'bottom') {

                                var alignX = x - (elemWidth / 2);

                                if ((alignX + elemWidth) <= $(window).width() && alignX > 0) {

                                    popupFits = true;

                                    //adjust x position
                                    x -= (elemWidth / 2);

                                    $arrow.css('left', ((elemWidth / 2) - (arrowWidth / 2)) + 'px');

                                    //assumes top or bottom position
                                    $(elem).attr('popupalign', 'center');

                                } else {

                                    o.popupAlign = 'right';

                                }

                            }

                            break;
                    }

                }

            } else {

                while (!popupFits && fitTries < 3) {

                    fitTries++;

                    switch (o.popupAlign) {
                        case 'top':

                            if ((y + elemHeight) <= $(document).height()) {

                                popupFits = true;

                                //assumes left or right position
                                $(elem).attr('popupalign', 'top');

                            } else {

                                o.popupAlign = 'center';

                            }

                            break;

                        case 'right':

                            var alignX = x - elemWidth;

                            if (alignX > 0) {

                                popupFits = true;

                                //adjust x position
                                x = alignX;
                                //assumes top or bottom position
                                $(elem).attr('popupalign', 'right');

                            } else {

                                o.popupAlign = 'left';

                            }

                            break;

                        case 'bottom':

                            var alignY = y - elemHeight;

                            if (alignY > 0) {

                                popupFits = true;

                                //adjust y position
                                y = alignY;

                                //assumes left or right position
                                $(elem).attr('popupalign', 'bottom');

                            } else {

                                o.popupAlign = 'top';

                            }

                            break;

                        case 'left':

                            if ((x + elemWidth) <= $(window).width()) {

                                popupFits = true;

                                //assumes top or bottom position
                                $(elem).attr('popupalign', 'left');

                            } else {

                                o.popupAlign = 'center';

                            }

                            break;

                        case 'center':
                        default:

                            if (o.popupPosition == 'left' || o.popupPosition == 'right') {

                                var alignY = y - (elemHeight / 2);

                                if ((alignY + elemHeight) <= $(document).height() && alignY > 0) {

                                    popupFits = true;

                                    //adjust y position
                                    y = alignY;

                                    //assumes left or right position
                                    $(elem).attr('popupalign', 'center');

                                } else {

                                    o.popupAlign = 'bottom';

                                }

                            } else if (o.popupPosition == 'top' || o.popupPosition == 'bottom') {

                                var alignX = x - (elemWidth / 2);

                                if ((alignX + elemWidth) <= $(window).width() && alignX > 0) {

                                    popupFits = true;

                                    //adjust x position
                                    x -= (elemWidth / 2);

                                    //assumes top or bottom position
                                    $(elem).attr('popupalign', 'center');

                                } else {

                                    o.popupAlign = 'right';

                                }

                            }

                            break;
                    }

                }

            }

            return { x: x, y: y };

        }

    });

})(jQuery);