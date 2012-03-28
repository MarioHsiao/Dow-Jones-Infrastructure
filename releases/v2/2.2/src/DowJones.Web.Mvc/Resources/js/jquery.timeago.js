/**
* Timeago is a jQuery plugin that makes it easy to support automatically
* updating fuzzy timestamps (e.g. "4 minutes ago" or "about 1 day ago").
*
* @name timeago
* @version 0.10.0
* @requires jQuery v1.2.3+
* @author Ryan McGeary
* @license MIT License - http://www.opensource.org/licenses/mit-license.php
*
* For usage and examples, visit:
* http://timeago.yarp.com/
*
* Copyright (c) 2008-2011, Ryan McGeary (ryanonjavascript -[at]- mcgeary [*dot*] org)
*/
(function ($) {
	$.timeago = function (timestamp) {
		if (timestamp instanceof Date) {
			return inWords(timestamp);
		} else if (typeof timestamp === "string") {
			return inWords($.timeago.parse(timestamp));
		} else {
			return inWords($.timeago.datetime(timestamp));
		}
	};
	var $t = $.timeago;

	$.extend($.timeago, {
		settings: {
			refreshMillis: 60000,
			allowFuture: false,
			strings: {
				prefixAgo: null,
				prefixFromNow: null,
				suffixAgo: "ago",
				suffixFromNow: "from now",
				seconds: "less than a minute",
				minute: "about a minute",
				minutes: "%d minutes",
				hour: "about an hour",
				hours: "about %d hours",
				day: "a day",
				days: "%d days",
				month: "about a month",
				months: "%d months",
				year: "about a year",
				years: "%d years",
				numbers: []
			},
			debug: false
		},
		inWords: function (distanceMillis) {
			var $l = this.settings.strings;
			var prefix = $l.prefixAgo;
			var suffix = $l.suffixAgo;
			if (this.settings.allowFuture) {
				if (distanceMillis < 0) {
					prefix = $l.prefixFromNow;
					suffix = $l.suffixFromNow;
				}
			}

			var seconds = Math.abs(distanceMillis) / 1000;
			var minutes = seconds / 60;
			var hours = minutes / 60;
			var days = hours / 24;
			var years = days / 365;

			function substitute(stringOrFunction, number) {
				var string = $.isFunction(stringOrFunction) ? stringOrFunction(number, distanceMillis) : stringOrFunction;
				var value = ($l.numbers && $l.numbers[number]) || number;
				return string.replace(/%d/i, value);
			}

			var words = seconds < 45 && substitute($l.seconds, Math.round(seconds)) ||
        seconds < 90 && substitute($l.minute, 1) ||
        minutes < 45 && substitute($l.minutes, Math.round(minutes)) ||
        minutes < 90 && substitute($l.hour, 1) ||
        hours < 24 && substitute($l.hours, Math.round(hours)) ||
        hours < 48 && substitute($l.day, 1) ||
        days < 30 && substitute($l.days, Math.floor(days)) ||
        days < 60 && substitute($l.month, 1) ||
        days < 365 && substitute($l.months, Math.floor(days / 30)) ||
        years < 2 && substitute($l.year, 1) ||
        substitute($l.years, Math.floor(years));

			return $.trim([prefix, words, suffix].join(" "));
		},
		parse: function (iso8601) {
			var s = $.trim(iso8601);
			s = s.replace(/\.\d\d\d+/, ""); // remove milliseconds
			s = s.replace(/-/, "/").replace(/-/, "/");
			s = s.replace(/T/, " ").replace(/Z/, " UTC");
			s = s.replace(/([\+\-]\d\d)\:?(\d\d)/, " $1$2"); // -04:00 -> -0400
			return new Date(s);
		},
		datetime: function (elem) {
			// jQuery's `is()` doesn't play well with HTML5 in IE
			var isTime = $(elem).get(0).tagName.toLowerCase() === "time"; // $(elem).is("time");
			var iso8601 = isTime ? $(elem).attr("datetime") : $(elem).attr("title");
			return $t.parse(iso8601);
		}
	});

	$.fn.timeago = function (selector, refreshOnceOnly) {
		var self = this;

		(selector ? $(selector, self) : self).each(refresh);

		if (!refreshOnceOnly) {
			var $s = $t.settings;
			if ($s.refreshMillis > 0) {
				setTimeout(function () {
					doRefresh(selector, self, $s.refreshMillis);
				}, $s.refreshMillis);
			}
		}
		return self;
	};

	function doRefresh(selector, ctx, refreshMillis) {
		/*var elems = [], filteredCtx;
		// if a selector was passed, check if the ctx is in the dom
		if (selector && elementIsInDOM(ctx)) {
		elems = $(selector, ctx);
		filteredCtx = ctx;
		}
		else {
		elems = _.filter(ctx, function (el) {
		return elementIsInDOM(el);
		});
		filteredCtx = elems;
		}

		// we got valid elems
		if (elems.length !== 0) {
		for (var i = 0, len = elems.length; i < len; i++) {
		refresh.call(elems[i]);
		}

		if ($t.settings.debug) {
		console.log(ctx[0].tagName, 'timeago - refreshed');
		}

		setTimeout(function () {
		doRefresh(selector, filteredCtx, refreshMillis);
		}, refreshMillis);
		}
		else {
		if ($t.settings.debug) {
		console.log('timeago - no valid elements, stopping refresh on:', selector, ctx);
		}

		}*/

		//var elems = (selector && elementIsInDOM(ctx)) ? $(selector, ctx) : ctx;

		var filtered,
			newCtx = ctx;
		// if a selector was passed, check if the ctx is in the dom
		if (selector && elementIsInDOM(ctx)) {
			filtered = $(selector, ctx);
		}
		else {
			filtered = _.filter(ctx, function (el) {
				return elementIsInDOM(el);
			});

			// if element got removed from DOM, update the array for subsequent calls
			if (filtered.length !== ctx.length) {
				newCtx = (ctx instanceof jQuery) ? $(filtered) : filtered;
			}
		}

		// we got valid elems
		if (filtered.length !== 0) {
			for (var i = 0, len = filtered.length; i < len; i++) {
				refresh.call(filtered[i]);
			}

			if ($t.settings.debug) { console.log($(newCtx)[0].tagName, 'timeago - refreshed'); }

			setTimeout(function () {
				doRefresh(selector, newCtx, refreshMillis);
			}, refreshMillis);
		}
		else if ($t.settings.debug) {
			console.log('timeago - no valid elements, stopping refresh on:', selector, ctx);
		}

	}

	function refresh() {
		var data = prepareData(this);
		if (!isNaN(data.datetime)) {
			$(this).text(inWords(data.datetime));
		}
		return this;
	}

	$.fn.timeago.refresh = function () {
		var self = this;
		(this.selector ? self.find(this.selector) : self).each(refresh);
	};

	function prepareData(element) {
		element = $(element);
		if (!element.data("timeago")) {
			element.data("timeago", { datetime: $t.datetime(element) });
			var text = $.trim(element.text());
			if (text.length > 0) {
				element.attr("title", text);
			}
		}
		return element.data("timeago");
	}

	function inWords(date) {
		return $t.inWords(distance(date));
	}

	function distance(date) {
		return (new Date().getTime() - date.getTime());
	}


	function elementIsInDOM(el) {
		if (!el) { return false; }

		var eld = el instanceof jQuery ? el[0] : el;

		if (eld.id) {
			return document.getElementById(eld.id) !== null;
		}

		/* as long as the element is not document, and there is a parent element */
		while (eld != document && eld.parentNode) {
			/* jump to the parent element */
			eld = eld.parentNode;
		}
		/* at this stage, the parent is found. If null, the uppermost parent element */
		/* is not document, and therefore the element is not part of the document */
		return eld == document;
	}

	// fix for IE6 suckage
	document.createElement("abbr");
	document.createElement("time");
} (jQuery));
