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


    // determine the supported request method once
    (function setRequestMethod() {
        if (typeof JSON !== "undefined" && JSON.parse) {
            if (typeof XMLHttpRequest !== "undefined" && "withCredentials" in new XMLHttpRequest()) {
                requestMethod = 'xhr-cors';
            }
            else if ((/{\s*\[native code\]\s*}/).test(JSON.parse.toString())
              && typeof XDomainRequest !== "undefined" /*&& scheme != 'https://'*/) {
                requestMethod = 'xdr';
            }
        }
    })();

    // build the 'sendRequest' function based on standards support
    if (requestMethod === 'xhr-cors') {
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
    }
    else if (requestMethod === 'xdr') {
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
    }
    else {  // default to jsonp
        sendRequest = function (url, callback, settings) {
            if (!(url && callback && settings)) { return; }

            // create script block
            var script = document.createElement("script"),
                jsonpCallback = "jsonp_" + getSalt(),
                done = FALSE;

            url = url.replace(/\&$/, "") + "&callback=" + jsonpCallback;

            window[jsonpCallback] = function (tmp) {
                callback(settings, tmp);

                // Garbage collect
                window[jsonpCallback] = undefined;
                try { delete window[jsonpCallback]; } catch (e) { }
                if (head) { head.removeChild(script); }
            };

            script.src = url;
            head.appendChild(script);
        };
    }

    DJ.crossDomain = sendRequest;
