/**
* Popup Balloon (callout) Plugin
*
* @category   jQuery UI Widget
* @author     Philippe Arcand <Philippe.Arcand@dowjones.com>
* @author     Ron Edgecomb <Ronald.EdgecombII@dowjones.com>
* @copyright  2011 Dow Jones & Company Inc.
* @version    SVN: $Id: jquery.popupballoon.js 4061 2012-03-01 15:15:57Z parcand $
* @dependency jquery.ui.core.js
* @dependency jquery.ui.widget.js
*/

(function ($, undefined) {

    $.widget("ui.popupBalloon", {
        options: {
            //*** existing options ***//

            popupClass: '', 				// Custom popup css class
            width: 400, 					// The width of the popup. Value can be an integer
            height: 130, 				// The height of the popup. Value can be an integer or 'auto'

            positionX: null, 			// Absolute X position of the popup. Optional. Note: leaving it to null will force the popup to use the target element position instead.
            positionY: null, 			// Absolute Y position of the popup. Optional .Note: leaving it to null will force the popup to use the target element position instead.

            title: '', 					// Title
            content: '', 				// Content (HTML)
            jScrollPaneEnabled: true, 		// Enable the jScrollPane plugin for custom scrollbar style
            insetMargin: 0, 				// Postion of the popup relative to the element edges
            animationDistance: 50, 			// Distance for the translation animation
            positioningMode: 'normal', 		// Positioning Mode ('normal', 'cursor', 'alignTo')

            //*** new options ***//			
            alignTo: $('body'), 			// if positioningMode == alignTo, then use this selector as the positioning target
            popupPosition: 'right', 		// Popup position relative to the click ('top','right','bottom','left'). This is treated more so as the preferred location, left and top may be replaced with right and bottom respectively based on available space.
            popupAlign: 'center', 		// Alignment of popup relative to click. Options for left and right positioned ('top','bottom','center' and 'under-title'). Options for top and bottom positioned ('right','left','center').

            xAlign: 'right', 				//Horizontal starting point for X coordinate relative to element triggering popup, ('left','right','center')
            xOffset: 0, 					// Offset value to shift inital X coordinate used in positioning popup, if the popupPosition option preference is inverted this value is inverts (eg. xOffset * -1)
            xOffsetInverted: 'auto', 		// If the popupPosition option preference is inverted, this offset value can be used as additional offset (eg. move callout to the other side of element). this offset is used in addition to the inverted xOffset

            yAlign: 'center', 				//Vertical starting point for Y coordinate relative to element triggering popup, ('top','center','bottom')
            yOffset: 0, 					// Offset value to shift inital Y coordinate used in positioning popup, if the popupPosition option preference is inverted this value is inverts (eg. yOffset * -1)
            yOffsetInverted: 'auto', 			// If the popupPosition option preference is inverted, this offset value can be used as additional offset (eg. move callout to the other side of element). this offset is used in addition to the inverted yOffset

            arrowEnabled: true, 			// Enable arrow in callout, arrow will be positioned based on popupPosition	

            animateDisplayEnabled: true, 	// Enable the animation of the popup balloon display
            animateMoveEnabled: true, 		// Enable the animation of the popup balloon moving
            animateHideEnabled: true, 		// Enable the animation of the popup balloon hiding
            animationSpeed: 300,

            interactionMode: 'none', 		// Interaction Mode ('none', 'hover', 'custom')

            mouseNearPadding: 10		// Distance around popup balloon which triggers custom popupMouseNear event
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
                popupAlign: o.popupAlign
            };

            if (o.xOffsetInverted == 'auto') {

                o.xOffsetInverted = 0;

                if (o.popupPosition == 'right' && o.xAlign == 'right') {
                    o.xOffsetInverted = parseInt($(el).width()) * -1;
                }

                if (o.popupPosition == 'left' && o.xAlign == 'left') {
                    o.xOffsetInverted = parseInt($(el).width());
                }

            }

            if (o.yOffsetInverted == 'auto') {
                //this could use a little bit of reworking here...

                o.yOffsetInverted = 0;

                if (o.popupPosition == 'top' && o.yAlign == 'top') {
                    o.yOffsetInverted = parseInt($(el).height()) / 2;
                }

                if (o.popupPosition == 'bottom' && o.yAlign == 'bottom') {
                    o.yOffsetInverted = (parseInt($(el).height()) / 2) * -1;
                }

            }

            $(document).mousemove(function (e) {
                self.mouseX = e.pageX;
                self.mouseY = e.pageY;

                self.nearCallout = false;

                if (self.isVisible()) {

                    var mouseNearPadding = o.mouseNearPadding;

                    if ((self.mouseX >= (self.currentX - mouseNearPadding) && self.mouseX <= (self.currentX + self.currentWidth + mouseNearPadding)) && (self.mouseY >= (self.currentY - mouseNearPadding) && self.mouseY <= (self.currentY + self.currentHeight + mouseNearPadding))) {
                        self.nearCallout = true;
                    }

                }

                if (self.nearCallout) {
                    self._trigger('popupMouseNear', e, self.popbox);
                }

            });

            window.setTimeout(function () {
                $("body").bind('mousedown', function (e) {
                    if (!$(e.target).data("popbox")) {
                        self.hide();
                    }
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
            popupAlign = self.preferences.popupAlign;

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

                case 'click':
                    $(el).bind('click', function (e) {
                        if (!self.visible) {
                            self._hide(true);
                            self.show();
                        };
                    });
                    break;

                case 'hover':
                    $(el).bind('mouseenter', function () {
                        clearTimeout($.data(this, 'bubbleTimeout'));
                        var ibubbleIsOpened = $(this).data("ibubbleIsOpened");
                        if (!ibubbleIsOpened) {
                            $(this).data("ibubbleIsOpened", true);
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

            if (self.popbox) {
                switch (key) {
                    case 'popupClass':
                        self.popbox.removeClass().addClass('popup-balloon').addClass(value);
                        // Has title
                        if (o.title) {
                            self.popbox.addClass('with-title');
                        }

                        // Has Arrow
                        if (o.arrowEnabled) {
                            self.popbox.addClass('with-arrow');
                        }
                        o[key] = value;
                        break;

                    case 'title':
                        self.popbox.find('.title').text(value);
                        o[key] = value;
                        break;

                    case 'content':
                        self.popbox.find('.content').empty().html(value);
                        self._setCalloutHeight('auto');
                        if (o.jScrollPaneEnabled) {
                            self.popbox.find('.content').jScrollPane({
                                showArrows: true,
                                autoReinitialise: false,
                                verticalDragMaxHeight: 52,
                                verticalGutter: 10,
                                horizontalGutter: 20
                            });
                        }
                        o[key] = value;
                        break;

                    case 'width':
                        self.popbox.find('.popup-body, .header .title').outerWidth(value);
                        self.currentWidth = value;
                        o[key] = value;
                        break;

                    case 'height':
                        self._setCalloutHeight(value);
                        o[key] = value;
                        break;

                    case 'popupPosition':
                    case 'popupAlign':

                        self.preferences[key] = o[key] = value;
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

            if (self.visible) {
                return;
            }

            // TimeStamp
            var timestamp = new Date().getTime();

            // Hide previously opened instances
            if (el.data("popbox")) {
                self._hide();
            }

            if (o.interactionMode == 'hover') {
                self._hide(true);
            }

            // Create popup
            var popbox = $('<div class="popup-balloon"><div class="balloon-arrow"></div><div class="popup-body"><div class="content"></div></div></div>');

            self.popbox = popbox;

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

            // Calculate/Fix Size
            popbox.find('.popup-body, .header .title').outerWidth(o.width);
            self.currentWidth = o.width;

            self._setCalloutHeight(o.height);

            //reset based on original preferences, the space may now be available to accommodate				
            o.popupPosition = self.preferences.popupPosition;
            o.popupAlign = self.preferences.popupAlign;

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
				.bind('mousemove', function (e) {
				    e.stopPropagation();
				    self._trigger('popupMouseMove', e, popbox);
				})
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
                    $(el).data("bubbleTimeout", window.setTimeout(function () {
                        var ibubbleIsOpened = $(el).data("ibubbleIsOpened");

                        if (ibubbleIsOpened) {
                            $(el).removeData("ibubbleIsOpened");
                            $(el).popupBalloon('hide');
                            clearTimeout($(el).data("bubbleTimeout"));
                        }
                    }, 300));
                });
            }

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

            //set the zIndex
            if (typeof ($dj) == 'undefined') {
                window.$dj = { maxZIndex: 99999 };
            }
            popbox.css("z-index", ++$dj.maxZIndex);

            el.data("popbox", popbox);



            this.visible = true;

            self.positionElement(popbox, x, y, function () { self._trigger('onShow'); });
        },

        /**
        *  Hide (remove) popbox elements from the page. (internal function)
        */
        _hide: function (hideAll) {
            var self = this,
				el = self.element,
				o = self.options;

            // Remove popbox elements
            var $selector = el.data("popbox");

            if (hideAll) {
                var $selector = $('.popup-balloon');
            }

            if ($selector) {

                // Remove all menu instances
                if (o.animateHideEnabled && !hideAll) {
                    $selector.fadeOut(o.animationSpeed, function () {
                        $(this).remove();
                    });
                } else {
                    $selector.each(function () {
                        $(this).data("controller").popupBalloon("hide");
                    });
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
        *  Set the callout content height
        *
        *  @param	height	Integer value
        */
        _setCalloutHeight: function (height) {
            var self = this,
				el = self.element,
				o = self.options;

            if (height == 'auto') {
                self.popbox.find('.content').outerHeight(self.popbox.find('.content').height('').outerHeight());
            } else {
                var headerHeight = self.popbox.find('.header .title').outerHeight(),
					contentHeight = height - headerHeight;

                self.popbox.find('.content').outerHeight(contentHeight);
            }

            self.currentHeight = self.popbox.height();

        },

        /**
        *  Position the popbox element
        *
        *  @param	elem	Element
        *  @param	x	x coordinate
        *  @param	y	y coordinate
        */
        positionElement: function (elem, x, y, callback) {

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
                    self.scrollDocument(callback);
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
                self.scrollDocument(callback);
            }
            self.currentX = x;
            self.currentY = y;
        },

        rePosition: function (callback) {
            var self = this,
				o = self.options,
                $callout = this.popbox,
                $content = $('div.content', $callout),
                $arrow = $('.balloon-arrow', $callout),
                popupOffset = $callout.offset(),
                currArrowPos = $arrow.position(),
                contentH = $content.outerHeight(),
                newContentH = 0,
                sizeDiff = 0;

            if ($arrow.offset().left < $(self.element).offset().left) {
                var cords = self._getCoordinates($callout);

                $($callout).animate({
                    left: (cords.x - $(self.element).outerWidth(true)) + "px"
                }, 200, callback);
            }
            else {
                if ($.isFunction(callback)) {
                    callback();
                }
            }
            // if the content area has children
            if ($content.children().size() > 0) {
                //calculate the total height of all the children within the content div
                $content.children().each(function () {
                    newContentH = newContentH + $(this).outerHeight();
                });

                if (newContentH > contentH) {
                    sizeDiff = newContentH - contentH;
                    if (o.popupAlign != 'top' && o.popupAlign != 'bottom' && o.popupAlign != 'under-title') {
                        $callout.animate({
                            top: popupOffset.top - (sizeDiff / 2) + "px"
                        }, 200);
                        $arrow.animate({
                            top: currArrowPos.top + (sizeDiff / 2) + "px"
                        }, 200);
                    }
                    else if (o.popupAlign == 'bottom') {
                        $callout.animate({
                            top: popupOffset.top - (sizeDiff) + "px"
                        }, 200);
                    }
                    $content.animate({
                        height: newContentH
                    }, 200, function () {
                        // after the animation is complete reset the styles
                        $(this).css({
                            height: "",
                            overflow: "visible"
                        });
                        self.scrollDocument(callback);
                    });
                }
                else {
                    if ($.isFunction(callback)) {
                        callback();
                    }
                }

                $content.children().show();
            }

        },

        scrollDocument: function (callback) {
            var self = this,
				$callout = this.popbox,
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
                $('html,body').stop().animate({ scrollTop: scrollValue }, 500, callback || $.noop());
            }
            else {
                if ($.isFunction(callback)) {
                    callback();
                }
            }
        },

        getpopbox: function () {
            return this.popbox;
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

            var $alignment = $(el);
            if (o.positioningMode == 'alignTo') {
                $alignment = o.alignTo;
            }

            if ((o.positionX || o.positionX === 0) || (o.positionY || o.positionY === 0)) {
                var x = o.positionX,
					y = o.positionY;
            } else if (o.positioningMode == 'cursor') {
                var x = self.mouseX,
					y = self.mouseY;
            } else { //normal and alignTo modes will be controlled here

                switch (o.xAlign) {
                    case 'left':
                        var x = $alignment.offset().left;
                        break;

                    case 'right':
                        var x = $alignment.offset().left + $alignment.outerWidth();
                        break;

                    case 'center':
                    default:
                        var x = $alignment.offset().left + ($alignment.outerWidth() / 2);
                        break;

                }

                switch (o.yAlign) {
                    case 'top':
                        var y = $alignment.offset().top;
                        break;

                    case 'bottom':
                        var y = $alignment.offset().top + $alignment.outerHeight();
                        break;

                    case 'center':
                    default:
                        var y = $alignment.offset().top + ($alignment.outerHeight() / 2);
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
                        y = (y + arrowHeight - o.insetMargin - o.yOffset + (o.yOffset * -1) + o.yOffsetInverted);
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
                        x = (x + arrowWidth - o.insetMargin - o.xOffset + (o.xOffset * -1) + o.xOffsetInverted);
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
                        y = (y - arrowHeight - elemHeight + o.insetMargin - o.yOffset + (o.yOffset * -1) + o.yOffsetInverted);
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
                        x = (x - arrowWidth - elemWidth + o.insetMargin - o.xOffset + (o.xOffset * -1) + o.xOffsetInverted);

                        // make sure that the popup fits
                        if (x < 0) {
                            x = 0
                        }

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
                                $arrow.css('top', '');

                            } else {

                                o.popupAlign = 'center';

                            }

                            break;

                        case 'under-title':

                            var alignY = y - (arrowHeight / 2) - 63;

                            if ((alignY + elemHeight) <= $(document).height()) {

                                popupFits = true;

                                //adjust y position
                                y = alignY;
                                $arrow.css('margin-top', '25px');


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
                                $arrow.css('right', '');

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
                                $arrow.css('bottom', '');

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
                                $arrow.css('left', '');

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
