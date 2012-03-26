/*
 * jQuery labeledScrollPane
 * 
 * Author: Philippe Arcand
 * Copyright 2011 Dow Jones & Company Inc.
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 *
 * This plugin also relies on the jscrollpane plugin.
 */
(function ($, undefined) {

    $.widget("ui.labeledScrollPane", {
        options: {
            onScroll: function () { }
        },
        scrollWatcher: '',

        /**
        * Plugin Initialization. (Executed automatically)
        */
        _init: function () {
            var self = this,
			el = self.element;
            o = self.options;

            if (!$(el).parent().hasClass('labeledscrollpane-container')) {
                $(el).wrap('<div class="labeledscrollpane-container" />');
                $(el).parent().append('<div class="labelbar"></div>');
            }

            var elContainer = $(el).parent();
            self.scrollWatcher = self.delayTimer(100);
            var dragShadow = $('<div class="jspDrag-shadow"></div>');

            // Initialize the jscrollpane plugin
            $(el).unbind().bind(
					'jsp-initialised',
					function (event, isScrollable) {
					    // Memory Housekeeping

					    $(elContainer).find('.labelbar-label').remove();

					    // Add Labels
					    $(el).find('.labeledscrollpane-group').each(function () {
					        var groupX = ($(this).children().first().position().left);
					        var label = ('<span class="labelbar-label" groupx="' + groupX + '" groupname="' + $(this).attr('groupname') + '"><div class="opacity-filter"></div>' + $(this).attr('groupname') + '</span>');
					        $(elContainer).find('.labelbar').append(label);
					    });

					    $(elContainer).find('.opacity-filter').each(function () {
					        $(this).width($(this).parent().width());
					        $(this).height($(this).parent().height());
					        $(this).css('top', ($(this).parent().position().top) + 'px');
					        $(this).css('left', ($(this).parent().position().left) + 10 + 'px');
					    });


					    //$(elContainer).find('.labelbar-label').first().addClass('active');

					    $(elContainer).find('.labelbar-label').bind('click', function () {
					        var api = el.data('jsp');
					        api.scrollToX($(this).attr('groupx'), true);
					        /*$(this).addClass('active');
					        $(this).siblings('.labelbar-label').removeClass('active',500);*/
					    });

					    // Add Drag Div Shadow (A.K.A. Fake Drag Div)
					    $(el).find(".jspTrack").prepend(dragShadow);
					    $(dragShadow).width($(el).find('.jspDrag').width());

					}).bind(
					'jsp-scroll-x',
					function (event, scrollPositionX, isAtLeft, isAtRight) {
					    $(dragShadow).css('left', $(el).find('.jspDrag').position().left + 'px');
					    if (!scrollPositionX) scrollPositionX = 0;
					    self.updateLabelsStatus(scrollPositionX);
					    o.onScroll();
					});

            // Adjust Content Width
            var contentWidth = 0;
            $(el).find('.labeledscrollpane-item').each(function () {
                contentWidth += $(this).width();
            });
            $(el).find('.labeledscrollpane-content').width(contentWidth);

            $(el).jScrollPane({}); //moved the plugin call here so that width will be set properly at this time -pete
        },

        updateLabelsStatus: function (scrollPositionX) {
            var self = this,
			el = self.element;
            o = self.options;
            var elContainer = $(el).parent();

            self.scrollWatcher(function () {

                scrollPostionLeft = scrollPositionX;
                scrollPositionRight = $(el).width() + scrollPostionLeft;

                $(el).find('.labeledscrollpane-group').each(function () {
                    var groupLeft = ($(this).children().first().position().left);
                    var groupRight = $(this).width() + groupLeft;
                    $(elContainer).find('.labelbar-label[groupname="' + $(this).attr('groupname') + '"]').attr("style", "");
                    if ((groupLeft >= scrollPostionLeft && groupLeft < scrollPositionRight) || (groupRight > scrollPostionLeft && groupRight <= scrollPositionRight) || (groupLeft < scrollPostionLeft && groupRight > scrollPositionRight)) {
                        $(elContainer).find('.labelbar-label[groupname="' + $(this).attr('groupname') + '"]').addClass("active", 300);

                    }
                    else {
                        $(elContainer).find('.labelbar-label[groupname="' + $(this).attr('groupname') + '"]').removeClass("active", 300);
                    }
                });

            });
        },

        delayTimer: function (delay) {
            var timer;
            return function (fn) {
                timer = clearTimeout(timer);
                if (fn) {
                    timer = setTimeout(function () {
                        fn();
                    }, delay);
                }
                return timer;
            }
        }

    });
})(jQuery);

/**
 *	Prevent Event Propagation (aka Event Bubbling)
 *
 *	@param	e	Event Object
 */
function preventEventPropagation(e){
	if (e && e.stopPropagation) //if stopPropagation method supported
		e.stopPropagation()
	else
		event.cancelBubble=true
}					