/*
* jQuery JSONP Core Plugin 2.1.4 (2010-11-17)
* 
* http://code.google.com/p/jquery-jsonp/
*
* Copyright (c) 2010 Julian Aubourg
*
* This document is licensed as free software under the terms of the
* MIT License: http://www.opensource.org/licenses/mit-license.php
*/
(function ($, setTimeout) {

    // ###################### UTILITIES ##

    // Noop
    function noop() {
    }

    // Generic callback
    function genericCallback(data) {
        lastValue = [data];
    }

    // Add script to document
    function appendScript(node) {
        head.insertBefore(node, head.firstChild);
    }

    // Call if defined
    function callIfDefined(method, object, parameters) {
        return method && method.apply(object.context || object, parameters);
    }

    // Give joining character given url
    function qMarkOrAmp(url) {
        return /\?/.test(url) ? "&" : "?";
    }

    var // String constants (for better minification)
		STR_ASYNC = "async",
		STR_CHARSET = "charset",
		STR_EMPTY = "",
		STR_ERROR = "error",
		STR_JQUERY_JSONP = "_jqjsp",
		STR_ON = "on",
		STR_ONCLICK = STR_ON + "click",
		STR_ONERROR = STR_ON + STR_ERROR,
		STR_ONLOAD = STR_ON + "load",
		STR_ONREADYSTATECHANGE = STR_ON + "readystatechange",
		STR_REMOVE_CHILD = "removeChild",
		STR_SCRIPT_TAG = "<script/>",
		STR_SUCCESS = "success",
		STR_TIMEOUT = "timeout",

    // Shortcut to jQuery.browser
		browser = $.browser,

    // Head element (for faster use)
		head = $("head")[0] || document.documentElement,
    // Page cache
		pageCache = {},
    // Counter
		count = 0,
    // Last returned value
		lastValue,

    // ###################### DEFAULT OPTIONS ##
		xOptionsDefaults = {
		    //beforeSend: undefined,
		    //cache: false,
		    callback: STR_JQUERY_JSONP,
		    //callbackParameter: undefined,
		    //charset: undefined,
		    //complete: undefined,
		    //context: undefined,
		    //data: "",
		    //dataFilter: undefined,
		    //error: undefined,
		    //pageCache: false,
		    //success: undefined,
		    //timeout: 0,
		    //traditional: false,		
		    url: location.href
		};

    // ###################### MAIN FUNCTION ##
    function jsonp(xOptions) {

        // Build data with default
        xOptions = $.extend({}, xOptionsDefaults, xOptions);

        // References to xOptions members (for better minification)
        var completeCallback = xOptions.complete,
			dataFilter = xOptions.dataFilter,
			callbackParameter = xOptions.callbackParameter,
			successCallbackName = xOptions.callback,
			cacheFlag = xOptions.cache,
			pageCacheFlag = xOptions.pageCache,
			charset = xOptions.charset,
			url = xOptions.url,
			data = xOptions.data,
			timeout = xOptions.timeout,
			pageCached,

        // Abort/done flag
			done = 0,

        // Life-cycle functions
			cleanUp = noop;

        // Create the abort method
        xOptions.abort = function () {
            !done++ && cleanUp();
        };

        // Call beforeSend if provided (early abort if false returned)
        if (callIfDefined(xOptions.beforeSend, xOptions, [xOptions]) === false || done) {
            return xOptions;
        }

        // Control entries
        url = url || STR_EMPTY;
        data = data ? ((typeof data) == "string" ? data : $.param(data, xOptions.traditional)) : STR_EMPTY;

        // Build final url
        url += data ? (qMarkOrAmp(url) + data) : STR_EMPTY;

        // Add callback parameter if provided as option
        callbackParameter && (url += qMarkOrAmp(url) + encodeURIComponent(callbackParameter) + "=?");

        // Add anticache parameter if needed
        !cacheFlag && !pageCacheFlag && (url += qMarkOrAmp(url) + "_" + (new Date()).getTime() + "=");

        // Replace last ? by callback parameter
        url = url.replace(/=\?(&|$)/, "=" + successCallbackName + "$1");

        // Success notifier
        function notifySuccess(json) {
            !done++ && setTimeout(function () {
                cleanUp();
                // Pagecache if needed
                pageCacheFlag && (pageCache[url] = { s: [json] });
                // Apply the data filter if provided
                dataFilter && (json = dataFilter.apply(xOptions, [json]));
                // Call success then complete
                callIfDefined(xOptions.success, xOptions, [json, STR_SUCCESS]);
                callIfDefined(completeCallback, xOptions, [xOptions, STR_SUCCESS]);
            }, 0);
        }

        // Error notifier
        function notifyError(type) {
            !done++ && setTimeout(function () {
                // Clean up
                cleanUp();
                // If pure error (not timeout), cache if needed
                pageCacheFlag && type != STR_TIMEOUT && (pageCache[url] = type);
                // Call error then complete
                callIfDefined(xOptions.error, xOptions, [xOptions, type]);
                callIfDefined(completeCallback, xOptions, [xOptions, type]);
            }, 0);
        }

        // Check page cache
        pageCacheFlag && (pageCached = pageCache[url])
			? (pageCached.s ? notifySuccess(pageCached.s[0]) : notifyError(pageCached))
			:
        // Initiate request
			setTimeout(function (script, scriptAfter, timeoutTimer) {

			    if (!done) {

			        // If a timeout is needed, install it
			        timeoutTimer = timeout > 0 && setTimeout(function () {
			            notifyError(STR_TIMEOUT);
			        }, timeout);

			        // Re-declare cleanUp function
			        cleanUp = function () {
			            timeoutTimer && clearTimeout(timeoutTimer);
			            script[STR_ONREADYSTATECHANGE]
							= script[STR_ONCLICK]
							= script[STR_ONLOAD]
							= script[STR_ONERROR]
							= null;
			            head[STR_REMOVE_CHILD](script);
			            scriptAfter && head[STR_REMOVE_CHILD](scriptAfter);
			        };

			        // Install the generic callback
			        // (BEWARE: global namespace pollution ahoy)
			        window[successCallbackName] = genericCallback;

			        // Create the script tag
			        script = $(STR_SCRIPT_TAG)[0];
			        script.id = STR_JQUERY_JSONP + count++;

			        // Set charset if provided
			        if (charset) {
			            script[STR_CHARSET] = charset;
			        }

			        // Callback function
			        function callback(result) {
			            (script[STR_ONCLICK] || noop)();
			            result = lastValue;
			            lastValue = undefined;
			            result ? notifySuccess(result[0]) : notifyError(STR_ERROR);
			        }

			        // IE: event/htmlFor/onclick trick
			        // One can't rely on proper order for onreadystatechange
			        // We have to sniff since FF doesn't like event & htmlFor... at all
			        if (browser.msie) {

			            script.event = STR_ONCLICK;
			            script.htmlFor = script.id;
			            script[STR_ONREADYSTATECHANGE] = function () {
			                /loaded|complete/.test(script.readyState) && callback();
			            };

			            // All others: standard handlers
			        } else {

			            script[STR_ONERROR] = script[STR_ONLOAD] = callback;

			            browser.opera ?

			            // Opera: onerror is not called, use synchronized script execution
							((scriptAfter = $(STR_SCRIPT_TAG)[0]).text = "jQuery('#" + script.id + "')[0]." + STR_ONERROR + "()")

			            // Firefox: set script as async to avoid blocking scripts (3.6+ only)
							: script[STR_ASYNC] = STR_ASYNC;

			            ;
			        }

			        // Set source
			        script.src = url;

			        // Append main script
			        appendScript(script);

			        // Opera: Append trailing script
			        scriptAfter && appendScript(scriptAfter);
			    }

			}, 0);

        return xOptions;
    }

    // ###################### SETUP FUNCTION ##
    jsonp.setup = function (xOptions) {
        $.extend(xOptionsDefaults, xOptions);
    };

    // ###################### INSTALL in jQuery ##
    $.jsonp = jsonp;

})(DJ.jQuery, setTimeout);

var TRUE = true,
        FALSE = false,
        NULL = null,
        undefined,
        head = document.head || document.getElementsByTagName("head")[0] || document.documentElement,
        requestMethod = 'jsonp',
        sendRequest;


function getSalt() {
    return Math.ceil(Math.random() * 1000000000000);
}


//determine the supported request method once
(function setRequestMethod() {
    if (DJ.config && /CORS/i.test(DJ.config.crossDomainTransport)) {
        if (typeof JSON !== "undefined" && JSON.parse) {
            if (typeof XMLHttpRequest !== "undefined" && "withCredentials" in new XMLHttpRequest()) {
                requestMethod = 'xhr-cors';
            }
            else if ((/{\s*\[native code\]\s*}/).test(JSON.parse.toString())
                    && typeof XDomainRequest !== "undefined" /*&& scheme != 'https://'*/) {
                requestMethod = 'xdr';
            }
        }
    }
})();

// build the 'sendRequest' function based on standards support
switch (requestMethod) {
    case "xhr-cors":
        sendRequest = function (url, callback, settings) {
            url += "_=" + getSalt();
            var xhr = new XMLHttpRequest(),
                method = "get";
            xhr.open(method, url, true);
            xhr.onload = function (e) {
                var payload = xhr.responseText;
                if (JSON && JSON.parse && payload) {
                    payload = JSON.parse(payload);
                    callback(payload, settings);
                }
            };
            xhr.onerror = function () {
                var payload = { Error: { Message: "Data request error occurred (" + xhr.status + ")."} };
                callback(payload, settings);
            }
            xhr.send();
        };
        break;
    case "xdr":
        sendRequest = function (url, callback, settings) {
            url += "&_=" + getSalt();
            var xhr = new XDomainRequest(),
                method = "get";
            xhr.open(method, url);
            xhr.onload = function () {
                var payload = xhr.responseText;
                if (JSON && JSON.parse && payload) {
                    payload = JSON.parse(payload);
                    callback(payload, settings);
                }
            };
            xhr.onerror = function () {
                var payload = { Error: { Message: "Data request error occurred (" + xhr.status + ")."} };
                callback(payload, settings);
            }
            xhr.onprogress = function () { };
            xhr.ontimeout = function () { };
            xhr.send();
        };
        break;
    default:
        sendRequest = function (crossDomainSettings) {
            //                if (!(url && callback && settings)) { return; }

            //                // create script block
            //                var script = document.createElement("script"),
            //                jsonpCallback = "jsonp_" + getSalt(),
            //                done = FALSE;

            //                url = url.replace(/\&$/, "") + "&callback=" + jsonpCallback;

            //                window[jsonpCallback] = function (tmp) {
            //                    callback(settings, tmp);

            //                    // Garbage collect
            //                    window[jsonpCallback] = undefined;
            //                    try { delete window[jsonpCallback]; } catch (e) { }
            //                    if (head) { head.removeChild(script); }
            //                };

            //                script.src = url;
            //                head.appendChild(script);

            var crossDomainDefaults = {
                callbackParameter: "callback",
                timeout: 30 * 1000
            }

            var xOptions = DJ.jQuery.extend({}, crossDomainDefaults, crossDomainSettings);
            DJ.jQuery.jsonp(xOptions);
        };
}

DJ.crossDomain = sendRequest;
