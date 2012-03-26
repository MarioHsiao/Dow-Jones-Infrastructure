/**
* jQuery plugin for text-overflow:ellipsis firefox support
*
* @category   jQuery Plugin
* @author     Ronald Edgecomb II <Ronald.EdgecombII@dowjones.com>
* @author     Philippe Arcand <philippe.arcand@dowjones.com>
* @copyright  2011 Dow Jones & Company Inc.
* @version    SVN: $Id: jquery.ellipsis.js 4029 2012-02-10 21:29:46Z parcand $
*/

(function ($) {
    $.fn.ellipsis = function (options) {
        // 	Options default values
        var defaults = {
            intervalDelay: 1,
            beforeEach: function () { },
            afterEach: function () { },
            beforeAll: function () { },
            afterAll: function () { },
            onFail: function () { }
        }

        // Properties
        var options = $.extend(defaults, options),
			self = this,
			$ellipsis = [],
			ellipsisIndex = 0,
			ellipsisInterval = '';

        /**
        * Plugin Initialization. (Executed automatically)
        */
        init = function () {
            options.beforeAll();

            if ($.browser.mozilla) {
                var ff_version = parseFloat($.browser.version);

                if (ff_version >= 2 && ff_version < 7) {
                    $ellipsis = self;
                    ellipsisIndex = 0;
                    // Since the conversion requires a lot of processing power, 
                    // we set a delay between each element to prevent the browser from freezing.
                    ellipsisInterval = setInterval(convertNext, options.intervalDelay);

                } else {
                    options.onFail('XML Behavior Used For FF < 4.x and Native Support For FF >= 7.x ');
                    options.afterAll();
                    // Maintain Chainability
                    return self.each(function () { });
                }

            } else {
                options.onFail('Native Browser Support');
                options.afterAll();
                // Maintain Chainability
                return self.each(function () { });
            }
        }

        /**
        * Convert next element
        */
        convertNext = function () {
            if (ellipsisIndex < $ellipsis.length) {
                var $ellipsisEl = $ellipsis.eq(ellipsisIndex);
                options.beforeEach($ellipsisEl);
                firefoxEllipsis($ellipsisEl.get(0));
                options.afterEach($ellipsisEl);
            } else {
                clearInterval(ellipsisInterval);
                options.afterAll();
                // Maintain Chainability
                return self.each(function () { });
            }

            ellipsisIndex++;
        }

        /**
        * Ellipsis emulation (firefox only)
        */
        firefoxEllipsis = function (el) {
            // Reset the element if it had been previously processed.
            $(el).removeClass('ff-ellipsis').data('calculatedWidth', '');

            var $tmp = $(el).clone();

            $tmp.css({
                width: 'auto',
                maxWidth: 'none',
                overflow: 'visible',
                display: 'inline',
                visibility: 'hidden'
            });

            $(el).after($tmp);

            var calculatedWidth = $tmp.get(0).offsetWidth;
            $tmp.remove();

            if (calculatedWidth > el.offsetWidth) {
                $(el).data('calculatedWidth', calculatedWidth);
                $(el).html($(el).text().replace(/\s+/g, '&nbsp;')).addClass('ff-ellipsis');
            }
        }

        // Initialize Plugin
        init();
    };
})(jQuery);