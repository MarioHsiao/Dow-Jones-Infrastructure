(function ($) {
    var djCore = window.djCore = window.djCore || {};

    djCore.config = djCore.config || {
        debug: true
    };

    djCore.version = "0.0.0";

    djCore.logger = {
        // keeping for backward compatibility. all calls should be either info, warn or error
        debug: function () {
            if (djCore.config && djCore.config.debug === true) {
                this._log.apply(this, [].slice.call(arguments).concat('debug'));
            }
        },

        info: function () {
            if (djCore.config && (djCore.config.info === true || djCore.config.debug === true)) {
                this._log.apply(this, [].slice.call(arguments).concat('info'));
            }
        },

        warn: function () {
            this._log.apply(this, [].slice.call(arguments).concat('warn'));
        },

        error: function () {
            this._log.apply(this, [].slice.call(arguments).concat('error'));
        },

        extractLineNumberFromStack: function (stack) {
            /// <summary>
            /// Get the line/filename detail from a Webkit stack trace.  See http://stackoverflow.com/a/3806596/1037948
            /// </summary>
            /// <param name="stack" type="String">the stack string</param>

            // correct line number according to how Log().write implemented
            var line = stack.split('\n')[3];
            // fix for various display text
            line = (line.indexOf(' (') >= 0 ?
                line.split(' (')[1].substring(0, line.length - 1)
                : line.split('at ')[1]
                );
            return line;
        },

        _log: function () {
            if (!window) {
                // fail silently
                return;
            }

            var args = [].slice.call(arguments),
                level = args.pop(),
                console = window.console,
                loggerService = console && (console[level] || console.table || console.log || function () { });

            if (loggerService) {
                if ('function' === typeof loggerService) { // Chrome, Firebug
                    /*	var isChrome = navigator.userAgent.indexOf("Chrome") !== -1;
                        if (isChrome) {
                            var stack = new Error().stack;
                            console.log(stack);
                            var file = stack.split("\n")[2].split("/")[4].split("?")[0];
                            var line = stack.split("\n")[2].split(":")[5];
                            var append = file + ":" + line;
                            args.push(append);
                        }*/
                    loggerService.apply(console, args);
                } else { // IE 8 treats console.log differently
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
        }
    };

    djCore.utils = {
        registerNamespace: function (namespacePath) {
            var parts = namespacePath.split('.');

            var ns = window;
            for (var i = 0; i < parts.length; i++) {
                if (!ns[parts[i]]) {
                    ns[parts[i]] = {};
                }
                ns = ns[parts[i]];
            }

            djCore.logger.debug('Registered namespace: ' + namespacePath);
        },
        browser: {
            isMsie: function () {
                var match = /(msie) ([\w.]+)/i.exec(navigator.userAgent);
                return match ? parseInt(match[2], 10) : false;
            },
            isChrome: function () {
                return navigator.userAgent.indexOf("Chrome") !== -1;
            }
        },
        isSecure: function () {
            return window.location.protocol === 'https';
        },
        isBlankString: function (str) {
            return !str || /^\s*$/.test(str);
        },
        escapeRegExChars: function (str) {
            return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
        },
        isString: function (obj) {
            return typeof obj === "string";
        },
        isNumber: function (obj) {
            return typeof obj === "number";
        },
        isArray: $.isArray,
        isFunction: $.isFunction,
        isObject: $.isPlainObject,
        isUndefined: function (obj) {
            return typeof obj === "undefined";
        },
        clone: function (obj) { return JSON.parse(JSON.stringify(obj)); },
        startsWith: function (str, s) {
            return str.slice(0, s.length) == s;
        },
        endsWith: function (str, s) {
            return str.slice(-s.length) == s;
        },
        bind: $.proxy,
        bindAll: function (obj) {
            var val;
            for (var key in obj) {
                $.isFunction(val = obj[key]) && (obj[key] = $.proxy(val, obj));
            }
        },
        indexOf: function (haystack, needle) {
            for (var i = 0; i < haystack.length; i++) {
                if (haystack[i] === needle) {
                    return i;
                }
            }
            return -1;
        },
        each: $.each,
        map: $.map,
        filter: $.grep,
        every: function (obj, test) {
            var result = true;
            if (!obj) {
                return true;
            }
            $.each(obj, function (key, val) {
                if (!(result = test.call(null, val, key, obj))) {
                    return false;
                }
                return true;
            });
            return !!result;
        },
        some: function (obj, test) {
            var result = false;
            if (!obj) {
                return false;
            }
            $.each(obj, function (key, val) {
                if (result = test.call(null, val, key, obj)) {
                    return false;
                }
                return true;
            });
            return !!result;
        },
        mixin: $.extend,
        urlDecode: function (encodedString) {
            var output = encodedString,
                myregexp = /(%[^%]{2})/;
            var binVal, thisString, match;

            while (((match = myregexp.exec(output))) && (match.length > 1) && (match[1] !== '')) {
                binVal = parseInt(match[1].substr(1), 16);
                thisString = String.fromCharCode(binVal);
                output = output.replace(match[1], thisString);
            }
            return output;
        },
        parseQueryString: function (str) {
            if (str) {
                if (str === '') { return {}; }
                var a = str.split('&');
                var out = {};
                for (var i = 0, len = a.length; i < len; ++i)
                {
                    var p = a[i].split('=');
                    if (p.length != 2) { continue; }
                    out[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
                }
                return out;
            }
            return {};
        },
        debounce: function (func, wait, immediate) {
            var timeout, result;
            return function () {
                var context = this,
                    args = arguments,
                    later, callNow;
                later = function () {
                    timeout = null;
                    if (!immediate) {
                        result = func.apply(context, args);
                    }
                };
                callNow = immediate && !timeout;
                clearTimeout(timeout);
                timeout = setTimeout(later, wait);
                if (callNow) {
                    result = func.apply(context, args);
                }
                return result;
            };
        },
        throttle: function (func, wait) {
            var context, args, timeout, result, previous, later;
            previous = 0;
            later = function () {
                previous = new Date();
                timeout = null;
                result = func.apply(context, args);
            };
            return function () {
                var now = new Date(),
                    remaining = wait - (now - previous);
                context = this;
                args = arguments;
                if (remaining <= 0) {
                    clearTimeout(timeout);
                    timeout = null;
                    previous = now;
                    result = func.apply(context, args);
                } else if (!timeout) {
                    timeout = setTimeout(later, remaining);
                }
                return result;
            };
        },
        tokenizeQuery: function (str) {
            return $.trim(str).toLowerCase().split(/[\s]+/);
        },
        tokenizeText: function (str) {
            return $.trim(str).toLowerCase().split(/[\s\-_]+/);
        },
        getProtocol: function () {
            return location.protocol;
        },
        noop: function () { }
    };

    djCore.RequestCache = function () {
        function requestCache(o) {
            djCore.utils.bindAll(this);
            o = o || {};
            this.sizeLimit = o.sizeLimit || 10;
            this.cache = {};
            this.cachedKeysByAge = [];
        }
        djCore.utils.mixin(requestCache.prototype, {
            get: function (url) {
                return this.cache[url];
            },
            set: function (url, resp) {
                var requestToEvict;
                if (this.cachedKeysByAge.length === this.sizeLimit) {
                    requestToEvict = this.cachedKeysByAge.shift();
                    delete this.cache[requestToEvict];
                }
                this.cache[url] = resp;
                this.cachedKeysByAge.push(url);
            }
        });
        return requestCache;
    }();

    djCore.Transport = function () {
        var pendingRequestsCount = 0, pendingRequests = {}, maxPendingRequests, requestCache;
        function transport(o) {
            djCore.utils.bindAll(this);
            o = djCore.utils.isString(o) ? {
                url: o
            } : o;
            requestCache = requestCache || new djCore.RequestCache();
            maxPendingRequests = djCore.utils.isNumber(o.maxParallelRequests) ? o.maxParallelRequests : maxPendingRequests || 6;
            this.url = o.url;
            this.wildcard = o.wildcard || "%QUERY";
            this.filter = o.filter;
            this.replace = o.replace;
            this.ajaxSettings = {
                type: "get",
                cache: o.cache,
                timeout: o.timeout,
                dataType: o.dataType || "json",
                beforeSend: o.beforeSend
            };
            this._get = (/^throttle$/i.test(o.rateLimitFn) ? djCore.utils.throttle : djCore.utils.debounce)(this._get, o.rateLimitWait || 300);
        }

        djCore.utils.mixin(transport.prototype, {
            _get: function (url, cb) {
                var that = this;
                if (belowPendingRequestsThreshold()) {
                    this._sendRequest(url).done(done);
                } else {
                    this.onDeckRequestArgs = [].slice.call(arguments, 0);
                }
                function done(resp) {
                    var data = that.filter ? that.filter(resp) : resp;
                    cb && cb(data);
                    requestCache.set(url, resp);
                }
            },
            _sendRequest: function (url) {
                var that = this, jqXhr = pendingRequests[url];
                if (!jqXhr) {
                    incrementPendingRequests();
                    jqXhr = pendingRequests[url] = $.ajax(url, this.ajaxSettings).always(always);
                }
                return jqXhr;
                function always() {
                    decrementPendingRequests();
                    pendingRequests[url] = null;
                    if (that.onDeckRequestArgs) {
                        that._get.apply(that, that.onDeckRequestArgs);
                        that.onDeckRequestArgs = null;
                    }
                }
            },
            get: function (query, cb) {
                var that = this, encodedQuery = encodeURIComponent(query || ""), url, resp;
                cb = cb || djCore.utils.noop;
                url = this.replace ? this.replace(this.url, encodedQuery) : this.url.replace(this.wildcard, encodedQuery);
                if (resp = requestCache.get(url)) {
                    djCore.utils.defer(function () {
                        cb(that.filter ? that.filter(resp) : resp);
                    });
                } else {
                    this._get(url, cb);
                }
                return !!resp;
            }
        });
        return transport;
        function incrementPendingRequests() {
            pendingRequestsCount++;
        }
        function decrementPendingRequests() {
            pendingRequestsCount--;
        }
        function belowPendingRequestsThreshold() {
            return pendingRequestsCount < maxPendingRequests;
        }
    }();
}(jQuery));