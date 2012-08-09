//!
// DowJones Common
//
/// <reference path="jquery.js" />
/// <reference path="require.js" />
/// <reference path="underscore.js" />

// Get local instances of require() and define()
var dj_define = define;
var dj_require = require;


// Initialize global DJ object
if (!window['DJ']) {
    window['DJ'] = {
        config: {
            debug: false
        }
    };
}

// Define DJ
var DJ = window['DJ'];
dj_define('DJ', [], function () { return DJ; });


// Define jQuery module with our jQuery
DJ.jQuery = jQuery.noConflict(true);
dj_define('jquery', [], function () { return DJ.jQuery; });
dj_define('$', [], function () { return DJ.jQuery; });


// Define underscore
DJ.underscore = _.noConflict();
dj_define('underscore', ['jquery'], function () { return DJ.underscore; });
dj_define('_', ['jquery'], function () { return DJ.underscore; });


// If we have JSON, define() it
if (window['JSON']) {
    dj_define('JSON', function () { return window['JSON']; });
}
    // Otherwise - if this is an ancient browser - grab a JSON implementation
else {
    dj_require(['JSON']);
}


//
// $dj:  Custom Global Functions
//
(function ($) {

    var $dj = {

        define: dj_define,
        require: dj_require,

        callback: function (handler, context) {
            var callbackName = handler;

            if ($.isFunction(handler)) {

                if (context) {
                    handler = $dj.delegate(context, handler);
                }

                callbackName = 'dj_' + $.expando;
                window[callbackName] = handler;
            }

            return callbackName;
        },

        clone: function (source) {
            return $.extend(true, {}, source);
        },

        dateFormat: {
            MMDDCCYY: 0,
            DDMMCCYY: 1,
            CCYYMMDD: 2
        },

        // keeping for backward compatibility. all calls should be either info, warn or error
        debug: function () {
            if (DJ.config.debug) {
                this._log.apply(this, [].slice.call(arguments).concat('debug'));
            }
        },

        info: function () {
            if (DJ.config.debug) {
                this._log.apply(this, [].slice.call(arguments).concat('info'));
            }
        },

        warn: function () {
            this._log.apply(this, [].slice.call(arguments).concat('warn'));
        },

        error: function () {
            this._log.apply(this, [].slice.call(arguments).concat('error'));
        },

        _log: function () {
            if (!window) {
                // fail silently
                return;
            }

            var args = [].slice.call(arguments),
				level = args.pop(),
				console = window.console,
				loggerService = console && (console[level] || console.log || function () { });

            if (loggerService) {
                if ('function' === typeof loggerService) {  // Chrome, Firebug
                    loggerService.apply(console, args);
                }
                else {  // IE 8 treats console.log differently
                    loggerService(args.join(' '));
                }
            }

            // for older opera
            if (window.opera) {
                window.opera.postError.apply(window.opera, args);
            }

            // nikhilK's web development helper for IE http://projects.nikhilk.net/WebDevHelper/
            if (window.debugService) {
                window.debugService.trace.apply(window.debugService, args);
            }
        },


        delegate: function (context, handler, customData) {
            if (!handler) {
                if ($.isFunction(context)) {
                    handler = context;
                }
                else {
                    $dj.error('Invalid delegate handler');
                }
            }

            return function () {
                var args = (customData !== undefined) ? [].concat(customData, [].slice.call(arguments)) : arguments;
                return handler.apply(context, args);
            };
        },

        loadStylesheet: function (url) {
            var link = document.createElement("link");
            link.type = "text/css";
            link.rel = "stylesheet";
            link.href = url;
            document.getElementsByTagName("head")[0].appendChild(link);
        },

        registerNamespace: function (namespacePath) {
            var parts = namespacePath.split('.');

            var ns = window;
            for (var i = 0; i < parts.length; i++) {
                if (!ns[parts[i]]) {
                    ns[parts[i]] = {};
                }
                ns = ns[parts[i]];
            }

            $dj.debug('Registered namespace: ' + namespacePath);
        },

        trim: function (str, doNotRemoveSpecialChars) {
            if (!doNotRemoveSpecialChars) {
                for (var i = 0; i < str.length; i++) {
                    if (str.charCodeAt(i) <= 32) {
                        str = str.substring(0, i) + " " + str.substr(i + 1);
                    }
                }
            }
            str = str.replace(/^[\s]+/g, "");
            str = str.replace(/[\s]+$/g, "");
            return str;
        },

        getClientWidth: function () {
            var v = 0, d = document, w = window;
            if ((!d.compatMode || d.compatMode === 'CSS1Compat') && d.documentElement && d.documentElement.clientWidth)
            { v = d.documentElement.clientWidth; }
            else if (d.body && d.body.clientWidth)
            { v = d.body.clientWidth; }
            else if (this.def(w.innerWidth, w.innerHeight, d.height)) {
                v = w.innerWidth;
                if (d.height > w.innerHeight) { v -= 16; }
            }
            return v;
        },

        getClientHeight: function () {
            var v = 0, d = document, w = window;
            if ((!d.compatMode || d.compatMode === 'CSS1Compat') && d.documentElement && d.documentElement.clientHeight)
            { v = d.documentElement.clientHeight; }
            else if (d.body && d.body.clientHeight)
            { v = d.body.clientHeight; }
            else if (this.def(w.innerWidth, w.innerHeight, d.width)) {
                v = w.innerHeight;
                if (d.width > w.innerWidth) { v -= 16; }
            }
            return v;
        },

        getHorizontalScroll: function () {
            var w = window, d = window.document;
            var offset = 0;
            if (w.pageXOffset) { // All but IE
                offset = w.pageXOffset;
            }
            else if (d.documentElement &&  // IE6 w/ doctype
						 d.documentElement.scrollLeft) {
                offset = d.documentElement.scrollLeft;
            }
            else if (d.body.scrollLeft) { // IE4,5,6(w/o doctype)
                offset = d.body.scrollLeft;
            }
            if (this.isNum(offset)) {
                return offset;
            }
            return 0;
        },

        getVerticalScroll: function () {
            var w = window, d = window.document;
            var offset = 0;
            if (w.pageYOffset) { // All but IE
                offset = w.pageYOffset;
            }
            else if (d.documentElement &&  // IE6 w/ doctype
						d.documentElement.scrollTop) {
                offset = d.documentElement.scrollTop;
            }
            else if (d.body.scrollTop) { // IE4,5,6(w/o doctype)
                offset = d.body.scrollTop;
            }
            if (this.isNum(offset)) {
                return offset;
            }
            return 0;
        },

        isNum: function () {
            for (var i = 0; i < arguments.length; ++i) {
                if (isNaN(arguments[i]) || typeof (arguments[i]) !== 'number') {
                    return false;
                }
            }
            return true;
        },

        htmlEncode: function (txt) {
            return $("<div/>").text(txt).html();
        },

        htmlDecode: function (html) {
            return $("<div/>").html(html).text();
        },

        timer: function (interval, callback, originalOptions) {
            var options = $.extend({ reset: 500, _isStopped: false, _timerID: null, _userCallback: callback }, originalOptions); // Create options for the default reset value
            interval = interval || options.reset;

            if (!callback) { return false; }

            var timer = function (interval, callback) {
                // Only used by internal code to call the callback
                this.internalCallback = function () {
                    // Invoke the user-defined callback
                    if (options._userCallback !== null) {
                        options._userCallback();
                    }
                };

                this.start = function () {
                    // Set the interval time
                    this.interval = interval;
                    options._timerID = window.setTimeout(this.internalCallback, this.interval);
                    options._isStopped = false;
                };

                // Clears any timers
                this.stop = function () {
                    window.clearTimeout(options._timerID);
                    options._timerID = null;
                    options._isStopped = true;
                };

                //Check whether the timer is stopped
                this.isStopped = function () { return options._isStopped; };
            };

            // Create a new timer object
            return new timer(interval, callback);
        },

        isString: function (val) {
            return val && (val.constructor === String);
        },

        replace: function (el, regex, replacement) {
            var current = $(el).val() || '';
            var updated = current.replace(regex, replacement || '');

            $(el).val(updated);

            return updated;
        },

        sanitizeJsonString: function (jsonString) {
            if (!$dj.isString(jsonString)) { throw 'Input is not a string'; }

            return jsonString.replace('\"', '\\\"');
        },

        // [Obsolete("Use serializeConfig instead")]
        serializeGlobalHeaders: function () {
            return this.serializeConfig();
        },

        serializeConfig: function () {
            var config = DJ.config;

            if (!config) { return null; }

            var headers = {
                credentials: JSON.stringify(config.credentials),
                preferences: JSON.stringify(config.preferences),
                product: config.productId
            };

            if (config.credentials.Debug) { headers.debug = true; }

            return headers;
        },

        queryParameter: function (name) {
            var match = new RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        },

        hasIllegalChar: function (str) {
            if (str) {
                return str.match(/[<>&#\\%+|]/);
            }
            return true;
        },

        validateEmail: function (email) {
            //To keep it consistent with dotcom and other products use the below validation login for email.
            return ($.trim(email).length <= 80 &&
					email.indexOf("@") !== -1 &&
					email.indexOf(".") !== -1 &&
					email.indexOf("@") === email.lastIndexOf("@") &&
					email.indexOf(" ") === -1);
        },

        hideSimpleTooltip: function () {
            try {
                var $tObj = $("#dj_tooltip");
                if ($tObj.length && $tObj.is(":visible")) {
                    $tObj.data("triggeringElement").attr("title", $tObj.hide().html());
                }
            }
            catch (e) { }
        },

        progressIndicator: {
            display: function (strMessage, hideProgressBar, fadeInTime) {
                strMessage = strMessage || '<%= Token("loading") %>...';

                var firstTime = $dj.progressIndicator.init();
                var progress = $('#dj_progressIndicator');
                progress.data("options", arguments);

                progress.find('td').css("white-space", "nowrap").html(strMessage);

                if (!firstTime) {
                    progress.parent().show().css({ "visible": "hidden", "width": "auto" });
                }

                var width = progress.width();
                if (width < 100) {
                    progress.width(100);
                }
                else {
                    progress.width(width);
                }

                if (!firstTime) {
                    progress.parent().hide();
                }

                if (!fadeInTime) {
                    fadeInTime = 0;
                }

                progress.overlay({ background: false, closeOnEsc: true, fadeInTime: fadeInTime, fadeOutTime: 0, hideSelect: false });
            },

            message: function (strMessage) {
                if ($('#dj_progressIndicator').length > 0) {
                    var options = $('#dj_progressIndicator').data("options");
                    options[0] = strMessage;
                    this.display.apply(this, options);
                }
                else {
                    this.display(strMessage);
                }
            },

            hide: function (delayTimer) {
                if (delayTimer) {
                    window.setTimeout(function () {
                        $().overlay.hide('#dj_progressIndicator');
                    }, delayTimer);
                }
                else {
                    $().overlay.hide('#dj_progressIndicator');
                }
            },

            init: function () {
                if ($('#dj_progressIndicator').size() < 1) {
                    var arrHtml = '<div class="loadingProgress progressIndicator" id="dj_progressIndicator">' +
									  '<table height="100%" cellspacing="0" cellpadding="0" width="100%">' +
											'<tr><td align="center" valign="middle"></td></tr>' +
									  '</table>' +
									'</div>';
                    $(document.body).append(arrHtml);
                    return true;
                }
                return false;
            }
        },

        confirmDialog: function (options) {

            options = $.extend({}, {
                msg: 'Are you sure?',
                yesText: "<%=Token('yes')%>",
                noText: "<%=Token('no')%>",
                context: null,
                yesClickHandler: null,
                yesClickParams: null,
                noClickHandler: null,
                noClickParams: null,
                title: null
            }, options);

            if ((!options.yesClickHandler) || typeof (options.yesClickHandler) !== 'function') {
                return;
            }

            if (!options.context) {
                options.context = this.confirmDialog.caller;
            }
            var confirmDialog = $('#confirmDialog'), firstTime = false, modalContent;
            if (confirmDialog.length === 0) {
                var markup = ['<div id="confirmDialog" class="dj_modal"">',
								'<div class="dj_modal-header">',
									'<h3 class="dj_modal-title"></h3>',
									'<p class="dj_modal-close" onclick="$().overlay.hide(\'#confirmDialog\');">&nbsp;</p>',
								'</div>',
								'<div class="dj_modal-content">',
									'<h4></h4>',
									'<span class="dj_btn"></span>',
									'<span class="dj_btn dj_btn-grey no-margin dj_modal-close"></span>',
								'</div>',
							'</div>'].join('');

                $(document.body).append(markup);
                confirmDialog = $('#confirmDialog');
                firstTime = true;
            }

            if (options.title) {
                confirmDialog.children(".dj_modal-header").show().children("h3").html(options.title);
                confirmDialog.removeClass("paddingTop_20px");
            }
            else {
                confirmDialog.children(".dj_modal-header").hide();
                confirmDialog.addClass("paddingTop_20px");
            }

            modalContent = confirmDialog.children(".dj_modal-content");

            modalContent.children("span:first").html(options.yesText).unbind('click')
			.click(function () {
			    $().overlay.hide('#confirmDialog');
			})
			.click(options.yesClickParams, options.yesClickHandler);

            modalContent.children("span:last").html(options.noText).unbind('click')
			.click(function () {
			    $().overlay.hide('#confirmDialog');
			})
			.click(options.noClickParams, options.noClickHandler);

            modalContent.children("h4").css("white-space", "nowrap").html(options.msg + '');

            if (!firstTime) {
                confirmDialog.parent().show().css({ "visisble": "hidden", "width": "auto" });
            }

            confirmDialog.width("auto");
            var width = confirmDialog.width();

            if (width < 300) {
                confirmDialog.width(300);
            }
            else if (width > $(window).width() - 300) {
                modalContent.children("h4").css("white-space", "normal");
                confirmDialog.width($(window).width() - 300);
            }
            else {
                confirmDialog.width(width);
            }

            if (!firstTime) {
                confirmDialog.parent().hide();
            }

            confirmDialog.overlay({ bgcolor: '#555', background: true, closeOnEsc: true, fadeInTime: 100, fadeOutTime: 10 });
        },

        validDate: function (date, dateFormat, returnDateFormat) {
            try {
                var retVal = false;
                if (date) {
                    date = $.trim(date);
                    var sep = "/";
                    if (date.indexOf("-") > -1) {
                        sep = "-";
                        date = date.replace(/-/g, "/");
                    }
                    var validformat = /^\d{2}\/\d{2}\/\d{4}$/;
                    if (dateFormat !== null && dateFormat === $dj.dateFormat.CCYYMMDD) {//ISO date format
                        validformat = /^\d{8}$/;
                    }

                    if (!validformat.test(date)) {
                        retVal = false;
                    }
                    else { //Detailed check for valid date ranges
                        var mIndex = 0, dIndex = 1, yIndex = 2, dateParts = date.split("/");
                        if (dateFormat !== null && dateFormat !== $dj.dateFormat.MMDDCCYY) {
                            if (dateFormat === $dj.dateFormat.CCYYMMDD) {//ISO date format
                                dateParts = [date.substring(0, 4), date.substring(4, 6), date.substring(6)];
                                mIndex = 1;
                                dIndex = 2;
                                yIndex = 0;
                            }
                            else {//DDMMCCYY
                                mIndex = 1;
                                dIndex = 0;
                                yIndex = 2;
                            }
                        }

                        var monthfield = parseInt(dateParts[mIndex], 10);
                        var dayfield = parseInt(dateParts[dIndex], 10);
                        var yearfield = parseInt(dateParts[yIndex], 10);
                        var dayobj = new Date(yearfield, monthfield - 1, dayfield);
                        if ((dayobj.getMonth() + 1 !== monthfield) || (dayobj.getDate() !== dayfield) || (dayobj.getFullYear() !== yearfield)) {
                            retVal = false;
                        }
                        else {

                            if (returnDateFormat !== null) {
                                if (returnDateFormat == $dj.dateFormat.MMDDCCYY) {
                                    retVal = dayobj.format("mm" + sep + "dd" + sep + "yyyy");
                                }
                                else if (returnDateFormat == $dj.dateFormat.DDMMCCYY) {
                                    retVal = dayobj.format("dd" + sep + "mm" + sep + "yyyy");
                                }
                                else {//CCYYMMDD - No sep for ISO data format
                                    retVal = dayobj.format("yyyymmdd");
                                }
                            }
                            else {
                                retVal = true;
                            }
                        }
                    }
                }
                return retVal;
            }
            catch (e) {
                return false;
            }
        },

        maxZIndex: 99999//Used for plugins to control the zIndex of elements
    };

    DJ.$dj = $dj;

})(DJ.jQuery);

DJ.$dj.define('$dj', ['jquery'], DJ.$dj);

(function ($, $dj) {
    /* 
     * Original script by Josh Fraser (http://www.onlineaspect.com)
     * Continued and maintained by Jon Nylander at https://bitbucket.org/pellepim/jstimezonedetect
     *
     * Provided under the Do Whatever You Want With This Code License.
     */
    /**
     * Namespace to hold all the code for timezone detection.
     */
    var jstz = {};
    jstz.HEMISPHERE_SOUTH = 'SOUTH';
    jstz.HEMISPHERE_NORTH = 'NORTH';
    jstz.HEMISPHERE_UNKNOWN = 'N/A';
    jstz.olson = {};

    /**
     * A simple object containing information of utc_offset, which olson timezone key to use, 
     * and if the timezone cares about daylight savings or not.
     * 
     * @constructor
     * @param {string} offset - for example '-11:00'
     * @param {string} olson_tz - the olson Identifier, such as "America/Denver"
     * @param {boolean} uses_dst - flag for whether the time zone somehow cares about daylight savings.
     */
    jstz.TimeZone = function (offset, olson_tz, uses_dst, f_tz_code) {
        this.utc_offset = offset;
        this.olson_tz = olson_tz;
        this.uses_dst = uses_dst;
        this.f_tz_code = f_tz_code;
    };

    /**
     * Prints out the result.
     * But before it does that, it calls this.ambiguity_check.
     */
    jstz.TimeZone.prototype.display = function () {
        this.ambiguity_check();
        var response_text = '<b>UTC-offset</b>: ' + this.utc_offset + '<br/>';
        response_text += '<b>Zoneinfo key</b>: ' + this.olson_tz + '<br/>';
        response_text += '<b>Zone uses DST</b>: ' + (this.uses_dst ? 'yes' : 'no') + '<br/>';

        return response_text;
    };

    /**
     * Checks if a timezone has possible ambiguities. I.e timezones that are similar.
     * 
     * If the preliminary scan determines that we're in America/Denver. We double check
     * here that we're really there and not in America/Mazatlan.
     * 
     * This is done by checking known dates for when daylight savings start for different
     * timezones.
     */
    jstz.TimeZone.prototype.ambiguity_check = function () {
        var ambiguity_list, length, i, tz;
        ambiguity_list = jstz.olson.ambiguity_list[this.olson_tz];

        if (typeof (ambiguity_list) === 'undefined') {
            return;
        }

        length = ambiguity_list.length;
        i = 0;

        for (; i < length; i += 1) {
            tz = ambiguity_list[i];

            if (jstz.date_is_dst(jstz.olson.dst_start_dates[tz])) {
                this.olson_tz = tz;
                return;
            }
        }
    };

    /**
     * Checks whether a given date is in daylight savings time.
     * 
     * If the date supplied is after june, we assume that we're checking
     * for southern hemisphere DST.
     * 
     * @param {Date} date
     * @returns {boolean}
     */
    jstz.date_is_dst = function (date) {
        var date_offset, base_offset;
        base_offset = ((date.getMonth() > 5 ? jstz.get_june_offset()
										    : jstz.get_january_offset()));

        date_offset = jstz.get_date_offset(date);

        return (base_offset - date_offset) !== 0;
    };

    /** 
     * Gets the offset in minutes from UTC for a certain date.
     * 
     * @param date
     * @returns {number}
     */
    jstz.get_date_offset = function (date) {
        return -date.getTimezoneOffset();
    };

    /**
     * This function does some basic calculations to create information about 
     * the user's timezone.
     * 
     * Returns a primitive object on the format
     * {'utc_offset' : -9, 'dst': 1, hemisphere' : 'north'}
     * where dst is 1 if the region uses daylight savings.
     * 
     * @returns {Object}  
     */
    jstz.get_timezone_info = function () {
        var january_offset, june_offset, diff;
        january_offset = jstz.get_january_offset();
        june_offset = jstz.get_june_offset();
        diff = january_offset - june_offset;

        if (diff < 0) {
            return {
                'utc_offset': january_offset,
                'dst': 1,
                'hemisphere': jstz.HEMISPHERE_NORTH
            };
        } else if (diff > 0) {
            return {
                'utc_offset': june_offset,
                'dst': 1,
                'hemisphere': jstz.HEMISPHERE_SOUTH
            };
        }

        return {
            'utc_offset': january_offset,
            'dst': 0,
            'hemisphere': jstz.HEMISPHERE_UNKNOWN
        };
    };

    jstz.get_january_offset = function () {
        return jstz.get_date_offset(new Date(2011, 0, 1, 0, 0, 0, 0));
    };

    jstz.get_june_offset = function () {
        return jstz.get_date_offset(new Date(2011, 5, 1, 0, 0, 0, 0));
    };

    /**
     * Uses get_timezone_info() to formulate a key to use in the olson.timezones dictionary.
     * 
     * Returns a primitive object on the format:
     * {'timezone': TimeZone, 'key' : 'the key used to find the TimeZone object'}
     * 
     * @returns Object 
     */
    jstz.determine_timezone = function () {
        var timezone_key_info, hemisphere_suffix, tz_key;
        timezone_key_info = jstz.get_timezone_info();
        hemisphere_suffix = '';

        if (timezone_key_info.hemisphere === jstz.HEMISPHERE_SOUTH) {
            hemisphere_suffix = ',s';
        }

        tz_key = timezone_key_info.utc_offset + ',' + timezone_key_info.dst + hemisphere_suffix;

        return { 'timezone': jstz.olson.timezones[tz_key], 'key': tz_key };
    };

    /**
     * The keys in this dictionary are comma separated as such:
     * 
     * First the offset compared to UTC time in minutes.
     *  
     * Then a flag which is 0 if the timezone does not take daylight savings into account and 1 if it does.
     * 
     * Thirdly an optional 's' signifies that the timezone is in the southern hemisphere, only interesting for timezones with DST.
     * 
     * The values of the dictionary are TimeZone objects.
     */
    jstz.olson.timezones = {
        '-720,0': new jstz.TimeZone('-12:00', 'Etc/GMT+12', false, "-12:00|1"),              // Mapping:    Dataline Standard Time
        '-660,0': new jstz.TimeZone('-11:00', 'Pacific/Pago_Pago', false, "-11:00|1"),       // Mapping:    Samoa Standard Time
        '-600,1': new jstz.TimeZone('-11:00', 'America/Adak', true, "+00:00|1"),             // GMT
        '-660,1,s': new jstz.TimeZone('-11:00', 'Pacific/Apia', true, "+00:00|1"),             // GMT
        '-600,0': new jstz.TimeZone('-10:00', 'Pacific/Honolulu', false, "-10:00|1"),        // Mapping:    Hawaiian Standard Time
        '-570,0': new jstz.TimeZone('-10:30', 'Pacific/Marquesas', false, "+00:00|1"),       // GMT
        '-540,0': new jstz.TimeZone('-09:00', 'Pacific/Gambier', false, "+00:00|1"),         // GMT
        '-540,1': new jstz.TimeZone('-09:00', 'America/Anchorage', true, "-09:00|1"),        // Mapping:    Alaskan Standard Time
        '-480,1': new jstz.TimeZone('-08:00', 'America/Los_Angeles', true, "-08:00|1"),      // Mapping:    Pacific Standard Time
        '-480,0': new jstz.TimeZone('-08:00', 'Pacific/Pitcairn', false, "+00:00|1"),        // GMT
        '-420,0': new jstz.TimeZone('-07:00', 'America/Phoenix', false, "-07:00|2"),         // Mapping:    Mountain Standard Time
        '-420,1': new jstz.TimeZone('-07:00', 'America/Denver', true, "-07:00|1"),           // Mapping:    US Mountain Standard Time
        '-360,0': new jstz.TimeZone('-06:00', 'America/Guatemala', false, "-06:00|3"),       // Mapping:    Central Standard Time (Mexico)
        '-360,1': new jstz.TimeZone('-06:00', 'America/Chicago', true, "-06:00|1"),          // Mapping:    Central Standard Time
        '-360,1,s': new jstz.TimeZone('-06:00', 'Pacific/Easter', true, "+00:00|1"),           // GMT
        '-300,0': new jstz.TimeZone('-05:00', 'America/Bogota', false, "-04:00|3"),          // Mapping:     S.A. Pacific Standard Time
        '-300,1': new jstz.TimeZone('-05:00', 'America/New_York', true, "-05:00|1"),         // Mapping:     Eastern Standard Time
        '-270,0': new jstz.TimeZone('-04:30', 'America/Caracas', false, "-04:00|2"),          // No Mapping:  SA Western Standard Time
        '-240,1': new jstz.TimeZone('-04:00', 'America/Halifax', true, "-04:00|1"),          // Mapping:     Atlantic Standard Time
        '-240,0': new jstz.TimeZone('-04:00', 'America/Santo_Domingo', false, "-04:00|1"),   // No Mapping:  Atlantic Standard Time
        '-240,1,s': new jstz.TimeZone('-04:00', 'America/Asuncion', true, "-04:00|3"),         // Mapping:     Pacific S.A. Standard Time
        '-210,1': new jstz.TimeZone('-03:30', 'America/St_Johns', true, "-03:30|1"),         // Mapping:     Newfoundland Standard Time
        '-180,1': new jstz.TimeZone('-03:00', 'America/Godthab', true, "-03:00|3"),          // Mapping:     Greenland Standard Time
        '-180,0': new jstz.TimeZone('-03:00', 'America/Argentina/Buenos_Aires', false, "-03:00|2"),      // Mapping:    SA Eastern Standard Time
        '-180,1,s': new jstz.TimeZone('-03:00', 'America/Montevideo', true, "-03:00|1"),       // Mapping:     E. South America Standard Time mapping to brasil do to economy.
        '-120,0': new jstz.TimeZone('-02:00', 'America/Noronha', false, "-02:00|1"),         // No Mapping:  Mid-Atlantic Standard Time  
        '-120,1': new jstz.TimeZone('-02:00', 'Etc/GMT+2', true, "-02:00|1"),                // Mapping:     Mid-Atlantic Standard Time  
        '-60,1': new jstz.TimeZone('-01:00', 'Atlantic/Azores', true, "-01:00|2"),          // Mapping:     Azores Standard Time                  
        '-60,0': new jstz.TimeZone('-01:00', 'Atlantic/Cape_Verde', false, "-01:00|2"),     // Mapping:     Cape Verde Standard Time
        '0,0': new jstz.TimeZone('00:00', 'Etc/UTC', false, "+00:00|1"),                 // Mapping:     Greenwich Standard Time
        '0,1': new jstz.TimeZone('00:00', 'Europe/London', true, "+00:00|2"),            // Mapping:     GMT Standard Time
        '60,1': new jstz.TimeZone('+01:00', 'Europe/Berlin', true, "+01:00|1"),            // Mapping:     Central Europe Standard Time
        '60,0': new jstz.TimeZone('+01:00', 'Africa/Lagos', false, "+01:00|5"),            // Mapping:     W. Central Africa Standard Time
        '60,1,s': new jstz.TimeZone('+01:00', 'Africa/Windhoek', true, "+03:00|4"),          // Mapping:     E. Africa Standard Time
        '120,1': new jstz.TimeZone('+02:00', 'Asia/Beirut', true, "+02:00|1"),              // Mapping:     E. Europe Standard Time
        '120,0': new jstz.TimeZone('+02:00', 'Africa/Johannesburg', false, "+02:00|4"),     // Mapping:     South Africa Standard Time
        '180,1': new jstz.TimeZone('+03:00', 'Europe/Moscow', true, "+03:00|1"),            // Mapping:     Russian Standard Time
        '180,0': new jstz.TimeZone('+03:00', 'Asia/Baghdad', false, "+03:00|2"),            // Mapping:     Arabic Standard Time
        '210,1': new jstz.TimeZone('+03:30', 'Asia/Tehran', true, "+03:30|1"),              // Mapping:     Iran Standard Time
        '240,0': new jstz.TimeZone('+04:00', 'Asia/Dubai', false, "+04:00|1"),              // Mapping:     Arabian Standard Time
        '240,1': new jstz.TimeZone('+04:00', 'Asia/Yerevan', true, "+04:00|2"),            // Mapping:     Caucasus Standard Time
        '270,0': new jstz.TimeZone('+04:30', 'Asia/Kabul', false, "+04:30|1"),              // Mapping:     Afghanistan Standard Time
        '300,1': new jstz.TimeZone('+05:00', 'Asia/Yekaterinburg', true, "+05:00|2"),       // Mapping:     Ekaterinburg Standard Time
        '300,0': new jstz.TimeZone('+05:00', 'Asia/Karachi', false, "+05:00|1"),            // Mapping:     West Asia Standard Time
        '330,0': new jstz.TimeZone('+05:30', 'Asia/Kolkata', false, "+05:30|1"),            // Mapping:     India Standard Time
        '345,0': new jstz.TimeZone('+05:45', 'Asia/Kathmandu', false, "+05:45|1"),          // Mapping:     Nepal Standard Time
        '360,0': new jstz.TimeZone('+06:00', 'Asia/Dhaka', false, "+06:00|2"),              // Mapping:     Central Asia Standard Time
        '360,1': new jstz.TimeZone('+06:00', 'Asia/Omsk', true, "+06:00|2"),                // No Mapping:  Central Asia Standard Time
        '390,0': new jstz.TimeZone('+06:30', 'Asia/Rangoon', false, "+06:30|1"),            // Mapping:     Myanmar Standard Time
        '420,1': new jstz.TimeZone('+07:00', 'Asia/Krasnoyarsk', true, "+07:00|2"),         // Mapping:     North Asia Standard Time
        '420,0': new jstz.TimeZone('+07:00', 'Asia/Jakarta', false, "+07:00|1"),            // Mapping:     SE Asia Standard Time
        '480,0': new jstz.TimeZone('+08:00', 'Asia/Shanghai', false, "+08:00|2"),           // Mapping:     China Standard Time
        '480,1': new jstz.TimeZone('+08:00', 'Asia/Irkutsk', true, "+08:00|3"),            // No Mapping:  North Asia East Standard Time
        '525,0': new jstz.TimeZone('+08:45', 'Australia/Eucla', true, "+08:00|4"),          // No Mapping:  W. Australia Standard Time
        '525,1,s': new jstz.TimeZone('+08:45', 'Australia/Eucla', true, "+08:00|4"),          // No Mapping:  W. Australia Standard Time
        '540,1': new jstz.TimeZone('+09:00', 'Asia/Yakutsk', true, "+09:00|3"),             // Mapping:     Yakutsk Standard Time
        '540,0': new jstz.TimeZone('+09:00', 'Asia/Tokyo', false, "+09:00|1"),              // Mapping:     Tokyo Standard Time
        '570,0': new jstz.TimeZone('+09:30', 'Australia/Darwin', false, "+09:30|2"),        // Mapping:     AUS Central Standard Time
        '570,1,s': new jstz.TimeZone('+09:30', 'Australia/Adelaide', true, "+09:30|1"),       // Mapping:     Cen. Australia Standard Time
        '600,0': new jstz.TimeZone('+10:00', 'Australia/Brisbane', false, "+10:00|1"),      // Mapping:     E. Australia Standard Time
        '600,1': new jstz.TimeZone('+10:00', 'Asia/Vladivostok', true, "+10:00|5"),         // Mapping:     Vladivostok Standard Time
        '600,1,s': new jstz.TimeZone('+10:00', 'Australia/Sydney', true, "+10:00|4"),         // Mapping:     AUS Eastern Standard Tim
        '630,1,s': new jstz.TimeZone('+10:30', 'Australia/Lord_Howe', true, "+10:00|4"),      // No Mapping:  AUS Eastern Standard Tim
        '660,1': new jstz.TimeZone('+11:00', 'Asia/Kamchatka', true, "+11:00|1"),           // No Mapping:  Central Pacific Standard Time
        '660,0': new jstz.TimeZone('+11:00', 'Pacific/Noumea', false, "+11:00|1"),          // No Mapping:  Central Pacific Standard Time
        '690,0': new jstz.TimeZone('+11:30', 'Pacific/Norfolk', false, "+11:00|1"),         // No Mapping:  Central Pacific Standard Time
        '720,1,s': new jstz.TimeZone('+12:00', 'Pacific/Auckland', true, "+12:00|1"),         // Mapping:     New Zealand Standard Time
        '720,0': new jstz.TimeZone('+12:00', 'Pacific/Tarawa', false, "+12:00|2"),          // Mapping:     Fiji Standard Time
        '765,1,s': new jstz.TimeZone('+12:45', 'Pacific/Chatham', true, "+12:00|1"),          // No Mapping:  New Zealand Standard Time
        '780,0': new jstz.TimeZone('+13:00', 'Pacific/Tongatapu', false, "+13:00|1"),       // Mapping:     Tonga Standard Time
        '840,0': new jstz.TimeZone('+14:00', 'Pacific/Kiritimati', false, "+13:00|1")       // No Mapping:  Tonga Standard Time
    };

    /**
     * This object contains information on when daylight savings starts for
     * different timezones.
     * 
     * The list is short for a reason. Often we do not have to be very specific
     * to single out the correct timezone. But when we do, this list comes in
     * handy.
     * 
     * Each value is a date denoting when daylight savings starts for that timezone.
     */
    jstz.olson.dst_start_dates = {
        'America/Denver': new Date(2011, 2, 13, 3, 0, 0, 0),
        'America/Mazatlan': new Date(2011, 3, 3, 3, 0, 0, 0),
        'America/Chicago': new Date(2011, 2, 13, 3, 0, 0, 0),
        'America/Mexico_City': new Date(2011, 3, 3, 3, 0, 0, 0),
        'Atlantic/Stanley': new Date(2011, 8, 4, 7, 0, 0, 0),
        'America/Asuncion': new Date(2011, 9, 2, 3, 0, 0, 0),
        'America/Santiago': new Date(2011, 9, 9, 3, 0, 0, 0),
        'America/Campo_Grande': new Date(2011, 9, 16, 5, 0, 0, 0),
        'America/Montevideo': new Date(2011, 9, 2, 3, 0, 0, 0),
        'America/Sao_Paulo': new Date(2011, 9, 16, 5, 0, 0, 0),
        'America/Los_Angeles': new Date(2011, 2, 13, 8, 0, 0, 0),
        'America/Santa_Isabel': new Date(2011, 3, 5, 8, 0, 0, 0),
        'America/Havana': new Date(2011, 2, 13, 2, 0, 0, 0),
        'America/New_York': new Date(2011, 2, 13, 7, 0, 0, 0),
        'Asia/Gaza': new Date(2011, 2, 26, 23, 0, 0, 0),
        'Asia/Beirut': new Date(2011, 2, 27, 1, 0, 0, 0),
        'Europe/Minsk': new Date(2011, 2, 27, 3, 0, 0, 0),
        'Europe/Istanbul': new Date(2011, 2, 27, 7, 0, 0, 0),
        'Asia/Damascus': new Date(2011, 3, 1, 2, 0, 0, 0),
        'Asia/Jerusalem': new Date(2011, 3, 1, 6, 0, 0, 0),
        'Africa/Cairo': new Date(2011, 3, 29, 4, 0, 0, 0),
        'Asia/Yerevan': new Date(2011, 2, 27, 4, 0, 0, 0),
        'Asia/Baku': new Date(2011, 2, 27, 8, 0, 0, 0),
        'Pacific/Auckland': new Date(2011, 8, 26, 7, 0, 0, 0),
        'Pacific/Fiji': new Date(2010, 11, 29, 23, 0, 0, 0),
        'America/Halifax': new Date(2011, 2, 13, 6, 0, 0, 0),
        'America/Goose_Bay': new Date(2011, 2, 13, 2, 1, 0, 0),
        'America/Miquelon': new Date(2011, 2, 13, 5, 0, 0, 0),
        'America/Godthab': new Date(2011, 2, 27, 1, 0, 0, 0)
    };

    /**
     * The keys in this object are timezones that we know may be ambiguous after
     * a preliminary scan through the olson_tz object.
     * 
     * The array of timezones to compare must be in the order that daylight savings
     * starts for the regions.
     */
    jstz.olson.ambiguity_list = {
        'America/Denver': ['America/Denver', 'America/Mazatlan'],
        'America/Chicago': ['America/Chicago', 'America/Mexico_City'],
        'America/Asuncion': ['Atlantic/Stanley', 'America/Asuncion', 'America/Santiago', 'America/Campo_Grande'],
        'America/Montevideo': ['America/Montevideo', 'America/Sao_Paulo'],
        'Asia/Beirut': ['Asia/Gaza', 'Asia/Beirut', 'Europe/Minsk', 'Europe/Istanbul', 'Asia/Damascus', 'Asia/Jerusalem', 'Africa/Cairo'],
        'Asia/Yerevan': ['Asia/Yerevan', 'Asia/Baku'],
        'Pacific/Auckland': ['Pacific/Auckland', 'Pacific/Fiji'],
        'America/Los_Angeles': ['America/Los_Angeles', 'America/Santa_Isabel'],
        'America/New_York': ['America/Havana', 'America/New_York'],
        'America/Halifax': ['America/Goose_Bay', 'America/Halifax'],
        'America/Godthab': ['America/Miquelon', 'America/Godthab']
    };
    $dj.timeZone = jstz;

})(DJ.jQuery, DJ.$dj);

//
// The core "DJ Components":  DJ.Component, DJ.UI.Component
//
(function ($, $dj) {

    // Simple JavaScript Inheritance
    // By John Resig http://ejohn.org/blog/simple-javascript-inheritance/
    // MIT Licensed.
    var initializing = false, fnTest = /xyz/.test(function () { xyz; }) ? /\b_super\b/ : /.*/;  //ignore jslint
    // The base Class implementation (does nothing)
    this.Class = function () { };

    // Create a new Class that inherits from this class
    Class.extend = function newClass(prop) {
        var _super = this.prototype;

        // Instantiate a base class (but only create the instance,
        // don't run the init constructor)
        initializing = true;
        var prototype = new this();
        initializing = false;

        prop.templates = prop.templates || {};

        // Copy the properties over onto the new prototype
        for (var name in prop) {
            // Check if we're overwriting an existing function
            prototype[name] =
				   typeof prop[name] === "function" &&
				   typeof _super[name] === "function" &&
				   fnTest.test(prop[name]) ? (function (name, fn) {
				       return function () {
				           var tmp = this._super;

				           // Add a new ._super() method that is the same method
				           // but on the super-class
				           this._super = _super[name];

				           // The method only need to be bound temporarily, so we
				           // remove it when we're done executing
				           var ret = fn.apply(this, arguments);
				           this._super = tmp;

				           return ret;
				       };
				   }(name, prop[name])) : prop[name];
        }

        // The dummy class constructor
        function Class() {
            try {
                // All construction is actually done in the init method
                if (!initializing && this.init) {
                    this.init.apply(this, arguments);
                }
            } catch (ex) {
                $dj.error('Error initializing component:', ex.toString(), this);
                if(ex.stack) $dj.debug(ex.stack);
            }
        }

        // Populate our constructed prototype object
        Class.prototype = prototype;

        // Enforce the constructor to be what we expect
        Class.constructor = Class;

        // And make this class extendable
        Class.extend = newClass;

        return Class;
    };



    DJ.Component = Class.extend({

        //
        // Properties
        //
        data: {},
        defaults: {},
        options: {},
        _delegates: {},


        //
        // Initialization
        //
        init: function (meta) {
            var $meta = $.extend({ name: 'Component' }, meta);

            this.data = $meta.data;
            this.defaults = $.extend(true, {}, this.defaults, $meta.defaults);
            this.options = $.extend(true, {}, this.options, this.defaults);
            this.options = $.extend(true, {}, this.options, $meta.options);

            this.name = $meta.name;

            // generate auto getter/setter for properties in options
            this._createAccessors(this.options);

            this._initializeDelegates();
        },

        _createAccessors: function (propertyBag) {
            // declare a local one so that we're immune to changes to the global javascript 'undefined'
            var UNDEFINED;

            if (propertyBag === UNDEFINED || propertyBag === null) {
                return;     // nothing to create
            }
            for (var propName in propertyBag) {
                if (this["get_" + propName] === undefined) { // do not override a user defined getter
                    this["get_" + propName] = (function (prop) { return function () { return propertyBag[prop]; }; }(propName));
                }

                if (this["set_" + propName] === undefined) { // do not override a user defined setter
                    this["set_" + propName] = (function (prop) { return function (value) { propertyBag[prop] = value; }; }(propName));
                }
            }

        },


        //
        // Public methods
        //
        dispose: function () {
            for (var delegate in this._delegates) {
                this._delegates[delegate] = null;
            }
        },

        getData: function () {
            return this._getData();
        },

        setData: function (value) {
            this.data = value;
            this._setData(value);
        },

        toString: function () {
            return this.name;
        },

        _getData: function () {
            return this.data;
        },

        _setData: function (value) {
            // Overridable
        },

        _debug: function (message) {
            $dj.debug(this.name + '>> ' + message);
        },

        _initializeDelegates: function () {
            this._delegates = {};
        },

        _initializeEventHandlers: function () {
            this._debug('Implement _initializeEventHandlers to bind event handlers to elements');
        }

    });

    $dj.info('Registered DJ.Component');


    $.extend(DJ, { UI: {} });

    DJ.UI.Component = DJ.Component.extend({

        //
        // Properties
        //
        defaults: {
            cssClass: 'ui-component'
        },

        eventHandlers: {},



        //
        // Initialization
        //
        init: function (element, meta) {
            var $meta = $.extend({ name: "UIComponent" }, meta);

            this.element = element;
            this.$element = $(element);

            if (element) {
                $meta.name = this.element.id || this.element.name;
            }

            try {
                var owner = $meta.owner || this.$element.parent().get(0);
                this.setOwner(owner);
            } catch (ex) {
            }

            this._super($meta);

            if ($meta["templates"])
                this.templates = $.extend({}, this.templates, $meta.templates);

            $.extend(this.tokens, $meta.tokens);
            this.eventHandlers = $.extend(true, {}, this.eventHandlers, $meta.eventHandlers);

            this.$element.data("options", this.options);

            this.$element.data("data", this.data);

            this.$element.addClass(this.options.cssClass);
            this._addBaseClass();

            this._initializeElements(this.$element);

            this._initializeEventHandlers();

            // declare an instance variable for all internal subscribers
            this._eventSubscribers = {};
        },


        _mapperR: function (handlerName, stack) {
            // check if empty string
            if (!handlerName || handlerName === '' || (handlerName.replace(/\s/g, '') === '')) { return null; }

            var buf = handlerName.split('.');
            stack = stack || window;
            return (buf.length === 1) ? stack[buf[0]] : this._mapperR(buf.slice(1).join('.'), stack[buf[0]]);
        },


        //
        // Public methods
        //

        appendData: function (value) {
            var startTime = new Date();

            this._appendData(value);

            this._debug('appendData:' + (new Date().getTime() - startTime.getTime()));
        },

        getId: function () {
            return (this.element) ? this.element.id : null;
        },

        getOwner: function () {
            // Lazy evaluation of $ -> DJ.UI.Component owner, if applicable
            if (this._owner instanceof $) {
                var owner = this._owner.findComponent(DJ.UI.Component);
                if (owner)
                    this._owner = owner;
            }

            return this._owner;
        },

        on: function (/* string */ event, /* function */ handler) {
            if (!$dj.isString(event)) {
                $dj.warn(this.name + '>> on() handler not subscribed: "event" must be a string.');
                return;
            }

            if (typeof handler !== 'function') {
                $dj.warn(this.name + '>> on() handler not subscribed: "handler" is not a valid function.');
                return;
            }

            var handlers = this._eventSubscribers[event] ? this._eventSubscribers[event] : (this._eventSubscribers[event] = []);
            handlers.push(handler);

            return this;
        },


        off: function (/* string */ event) {
            if (!$dj.isString(event)) {
                $dj.warn(this.name + '>> off() event parameter must be a string. Handlers will not be removed.');
                return;
            }

            if (this._eventSubscribers[event]) {
                delete this._eventSubscribers[event];
            }
        },


        notifyInstanceSubscribers: function (/* string */ event, /* object */ args) {
            var subscribers = this._eventSubscribers[event];
            if (!subscribers) {
                return;
            }
            for (var i = 0; i < subscribers.length; i++) {
                var subscriber = subscribers[i];
                if (subscriber &&
                    typeof subscriber === 'function') {
                    setTimeout((function (s) {
                        return function () {
                            s.apply(this, [args]);
                            $dj.info(this.name + '>> Notified "', s.name || 'anonymous function', '"');
                        };
                    }(subscriber)));
                }
            }
        },

        publish: function (/* string */eventName, /* object */args) {
            $dj.info(this.name + '>> Publish:', this._owner || window, eventName);
            var publish = (this._owner && this._owner._innerPublish && this._owner._innerPublish instanceof Function) ? this._owner._innerPublish : $dj.publish;
            publish.call(this._owner || window, eventName, args);
            this.notifyInstanceSubscribers(eventName, args);
            return this;
        },

        subscribe: function (/* string */eventName, /* function() */handler) {
            if (this._owner && this._owner.subscribe) {
                this._owner.subscribe(eventName, handler);
            }

            return this;
        },

        setData: function (value) {
            this._super(value);
            this._clear();
        },

        setOwner: function (value) {
            if (!value) {
                this._owner = null;
                return this;
            }

            var owner = value;
            
            // If value is not a Component object, convert it to one
            if (!(owner instanceof DJ.UI.Component)) {

                // If value is not a jQuery object, convert it to one
                if (!(owner instanceof $)) {
                    owner = $(owner);
                }

                // Convert the jQuery object to a DJ.UI.Component
                owner = owner.findComponent(DJ.UI.Component);
            }

            this._owner = owner || value;

            return this;
        },

        toString: function () {
            return this.getId() || this._super();
        },


        //
        // Protected methods
        //

        _getHashKey: function () {
            return new Date().getTime();
        },

        _addBaseClass: function () {
            var baseClassName = this.options.baseClassName;
            if (baseClassName && baseClassName.length > 0 && !$.isArray(baseClassName)) {
                $(this.element).addClass(this.options.baseClassName);
            }
        },

        _appendData: function (value) {
            this._debug('TODO: Implement _appendData function');
        },

        _clear: function () {
            $(this.element).empty();
        },

        _initializeElements: function (ctx) {
            this._debug('TODO: Implement _initializeElements function to lookup html elements and cache them at component level');
        },

        EOF: {}

    });

    $dj.info('Registered DJ.UI.Component (extends DJ.Component)');


    return DJ.UI.Component;

})(DJ.jQuery, DJ.$dj);


/* DJ.add() */
(function ($, $dj) {
    var add = function (name, config) {

        var validateConfig = function (configuration) {
            var errors = [];

            if (configuration === undefined) {
                errors.push('configuration not defined');
                return errors;
            }

            // container can either be a DOM element or an id string
            // if it's a string, resolve it to a DOM element
            if (configuration.container) {
                if (typeof configuration.container === "string")
                    configuration.container = document.getElementById(configuration.container);
            }

            if (configuration.container === null) {
                errors.push('container not found');
            }

            return errors;
        };

        var getTypeHandle = function (handlerName, stack) {
            // check if empty string
            if (!handlerName || handlerName === '' || (handlerName.replace(/\s/g, '') === '')) { return null; }

            var buf = handlerName.split('.');
            stack = stack || window;
            return (buf.length === 1) ? stack[buf[0]] : getTypeHandle(buf.slice(1).join('.'), stack[buf[0]]);
        };

        var wireUpEventHandlers = function (instance, eventHandlers) {
            if (!instance || !instance.on || ! typeof (instance.on) === "function") {
                $dj.warn('Cannot wire up eventHandlers - either instance is null, or not of type DJ.UI.Component or ".on()" is not a function.');
                return;
            }

            for (var event in eventHandlers) {
                var handler = eventHandlers[event];
                instance.on(event, handler);
            }
        };

        // Assume the 'DJ.UI' namespace if not specified
        if (name.indexOf('.') < 0) {
            name = "DJ.UI." + name;
        }


        var promise = $.Deferred();

        var notify = function (state) {
            promise.notify({ state: state, name: name, config: config });
        };

        notify('Validating');
        var validationErrors = validateConfig(config);
        if (validationErrors && validationErrors.length > 0) {
            $dj.error('Error validating DJ.Add configuration ', validationErrors);
            promise.reject(validationErrors);
        } else {
            notify('Validated');

            var createInstance = function () {
                var type = getTypeHandle(name);
                var instance = new type(config.container, config);
                wireUpEventHandlers(instance, config.eventHandlers);

                notify('Loaded');
                promise.resolve(instance);

                return instance;
            };

            var onError = function (err) {
                notify('Failed');
                promise.reject(err);
            };

            // Retrieve and create an instance of the module
            notify('Loading');
            $dj.require(['DJ', '$', '$dj', '_', 'JSON', name], createInstance, onError);
        }

        return promise;
    };

    DJ.add = add;
}(DJ.jQuery, DJ.$dj));



//!
// Date Handling Extensions
//

DJ.$dj.require(['JSON', '$dj'], function (JSON, $dj) {
    /********* ParseDatesInObject **********/

    if (!JSON.parseDatesInObj) {
        JSON.parseDatesInObj = function (jsonObj) {
            try {
                for (var prop in jsonObj) {
                    if (jsonObj.hasOwnProperty(prop)) {
                        var val = jsonObj[prop];
                        if (typeof val === 'object') {
                            JSON.parseDatesInObj(val);
                        }
                        else if ($dj.isString(val)) {
                            var dateValue = JSON.parseDate(val);
                            if (dateValue) {
                                jsonObj[prop] = dateValue;
                            }
                            //return;
                        }
                    }
                }
            } catch (e) {
                // orignal error thrown has no error message so rethrow with message
                throw new Error("Dates in JSON Object could not be parsed");
            }
        };
    }

    if (!JSON.parseDate) {
        JSON.parseDate = function (dateString) {
            var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
            var reMsAjax = /^\/Date\((d|-|.*)\)[\/|\\]$/;

            try {
                if ($dj.isString(dateString)) {
                    var a = reISO.exec(dateString);
                    if (a) {
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));
                    }
                    a = reMsAjax.exec(dateString);
                    if (a) {
                        var b = a[1].split(/[-+,.]/);
                        return new Date(b[0] ? +b[0] : 0 - +b[1]);
                    }
                }
            } catch (e) {
                throw new Error("Date could not be parsed");
            }

            return dateString;
        };
    }
});


/*
* Date Format 1.2.3
* (c) 2007-2009 Steven Levithan <stevenlevithan.com>
* MIT license
*
* Includes enhancements by Scott Trenda <scott.trenda.net>
* and Kris Kowal <cixar.com/~kris.kowal/>
*
* Accepts a date, a mask, or a date and a mask.
* Returns a formatted version of the given date.
* The date defaults to the current date/time.
* The mask defaults to __dateFormat.masks.default.
*/

var __dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
		timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
		timezoneClip = /[^-+\dA-Z]/g,
		pad = function (val, len) {
		    val = String(val);
		    len = len || 2;
		    while (val.length < len) val = "0" + val;
		    return val;
		};

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc, languageCode) {
        var dF = __dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        if (languageCode) {
            mask = dF.getDateFormatBasedOnLang(mask, languageCode);
        }
        else {
            mask = String(dF.masks[mask] || mask || dF.masks["default"]);
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
			d = date[_ + "Date"](),
			D = date[_ + "Day"](),
			m = date[_ + "Month"](),
			y = date[_ + "FullYear"](),
			H = date[_ + "Hours"](),
			M = date[_ + "Minutes"](),
			s = date[_ + "Seconds"](),
			L = date[_ + "Milliseconds"](),
			o = utc ? 0 : date.getTimezoneOffset(),
			flags = {
			    d: d,
			    dd: pad(d),
			    ddd: dF.i18n.dayNames[D],
			    dddd: dF.i18n.dayNames[D + 7],
			    m: m + 1,
			    mm: pad(m + 1),
			    mmm: dF.i18n.monthNames[m],
			    mmmm: dF.i18n.monthNames[m + 12],
			    yy: String(y).slice(2),
			    yyyy: y,
			    h: H % 12 || 12,
			    hh: pad(H % 12 || 12),
			    H: H,
			    HH: pad(H),
			    M: M,
			    MM: pad(M),
			    s: s,
			    ss: pad(s),
			    l: pad(L, 3),
			    L: pad(L > 99 ? Math.round(L / 10) : L),
			    t: H < 12 ? "a" : "p",
			    tt: H < 12 ? "<%= Token('smallAm') %>" : "<%= Token('smallPm') %>",
			    T: H < 12 ? "A" : "P",
			    TT: H < 12 ? "<%= Token('capitalAm') %>" : "<%= Token('capitalPm') %>",
			    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
			    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
			    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
			};

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

// Some common format strings
__dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    longDate: "dddd, dd mmmm yyyy",
    shortDate: "dd-mmm-yyyy",
    standardDate: "d mmmm yyyy",
    dateMonth: "d - mmm",
    jakoch: {
        "default": "yyyy 年 m 月 d 日",
        longDate: "yyyy 年 m 月 d 日",
        shortDate: "yyyy 年 m 月 d 日",
        standardDate: "yyyy 年 m 月 d 日",
        dateMonth: "mmm - d"
    }
};

__dateFormat.getDateFormatBasedOnLang = function (mask, lang) {
    if (mask && lang) {
        switch (lang.toLowerCase()) {
            case "ja":
            case "zhtw":
            case "zhcn":
            case "zh-tw":
            case "zh-cn":
                {
                    return __dateFormat.masks.jakoch[mask] || __dateFormat.masks.jakoch["default"];
                }
            default:
                {
                    return __dateFormat.masks[mask] || __dateFormat.masks["default"];
                }
        }
    } else {
        return null;
    }
};

// Internationalization strings
__dateFormat.i18n = {
    //    dayNames: [
    //		"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
    //		"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    //	],
    //    monthNames: [
    //		"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
    //		"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    //	]
    dayNames: [
		"<%= Token('sSun') %>", "<%= Token('sMon') %>", "<%= Token('sTue') %>", "<%= Token('sWed') %>", "<%= Token('sThu') %>", "<%= Token('sFri') %>", "<%= Token('sSat') %>",
		"<%= Token('sunday') %>", "<%= Token('monday') %>", "<%= Token('tuesday') %>", "<%= Token('wednesday') %>", "<%= Token('thursday') %>", "<%= Token('friday') %>", "<%= Token('saturday') %>"
    ],
    monthNames: [
		"<%= Token('sJan') %>", "<%= Token('sFeb') %>", "<%= Token('sMar') %>", "<%= Token('sApr') %>", "<%= Token('sMay') %>", "<%= Token('sJun') %>", "<%= Token('sJul') %>", "<%= Token('sAug') %>", "<%= Token('sSep') %>", "<%= Token('sOct') %>", "<%= Token('sNov') %>", "<%= Token('sDec') %>",
		"<%= Token('january') %>", "<%= Token('february') %>", "<%= Token('march') %>", "<%= Token('april') %>", "<%= Token('may') %>", "<%= Token('june') %>", "<%= Token('july') %>", "<%= Token('august') %>", "<%= Token('september') %>", "<%= Token('october') %>", "<%= Token('november') %>", "<%= Token('december') %>"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc, languageCode) {
    return __dateFormat(this, mask, utc, languageCode);
};
