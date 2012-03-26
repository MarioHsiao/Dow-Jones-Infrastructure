/// <reference path="jquery.js" />
/// <reference path="common.js" />
//! ServiceProxy.js
this.JSON = this.JSON || {};

if (!this.JSON.parseDatesInObj) {
    JSON.parseDatesInObj = function (jsonObj) {
        var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
        var reMsAjax = /^\/Date\((d|-|.*)\)[\/|\\]$/;

        try {
            for (var prop in jsonObj) {
                if (jsonObj.hasOwnProperty(prop)) {
                    var val = jsonObj[prop];
                    if (typeof val == 'object') {
                        JSON.parseDatesInObj(val);
                    }
                    else if ($dj.isString(val)) {
                        var a = reISO.exec(val);
                        if (a) {
                            jsonObj[prop] = new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));
                            return;
                        }
                        a = reMsAjax.exec(val);
                        if (a) {
                            var b = a[1].split(/[-+,.]/);
                            jsonObj[prop] = new Date(b[0] ? +b[0] : 0 - +b[1]);
                            return;
                        }
                    }
                }
            }
        } catch (e) {
            // orignal error thrown has no error message so rethrow with message
            throw new Error("Dates in JSON Object could not be parsed");
            return null;
        }
    }
}


$dj.registerNamespace('DJ.Services');

DJ.Services.PlatformServiceProxy = Class.extend({

    /// <summary>
    ///     Calls a WCF/ASMX service and returns the result.
    /// </summary>
    /// <remarks>
    ///     Based on Rick Strahl's ServiceProxy.js:
    ///     http://www.west-wind.com/weblog/posts/896411.aspx
    /// </remarks>
    /// <param name="method" type="string">The method of the service to call</param>
    /// <param name="queryParams" type="object">An object that represents the parameters to pass {symbol:"msft",years:2}       
    /// <param name="callback" type="function">Function called on success. 
    ///     Receives a single parameter of the parsed result value</param>
    /// <param name="errorCallback" type="function">Function called on failure. 
    ///     Receives a single error object with Message property</param>
    /// <param name="bare" type="boolean">Set to true if response is not a WCF/ASMX style 'wrapped' object</param>
    invoke: function (params) {
        if (params && (params.url || ("/")).substr(0, 1) != "/") {
            var hostname = params.host || [window.location.protocol, "//", window.location.host].join("");
            params.url = [hostname, '<%= AppSetting("PlatformProxyBaseUrl", "/PlatformProxy.asmx") %>?url=', escape(params.url)].join("");
        }

        this._invokeInternal(params);
    },

    _normalizeResult: function (result) {
        if (result != null) {
            if (result.d) {
                return result.d;
            }
            return result;
        }
        return null;
    },

    _invokeInternal: function (params) {

        $dj.debug('Invoking web service ' + params.url);

        var onSuccess = params.onSuccess || $dj.delegate(this, this.handleSuccess);
        var onComplete = params.onComplete || $dj.delegate(this, this.handleComplete);
        var onError = params.onError || $dj.delegate(this, this.handleError);

        var method = params.method || 'GET';
        var data = params.queryParams;
        var dataType = params.dataType || 'json';
        var contentType = 'application/x-www-form-urlencoded';
        var headers = this._buildHeaders(params);

        if (method !== 'GET') {
            data = $dj.isString(params.data) ? params.data : JSON.stringify(params.data);
            contentType = 'application/json';
        }

        var normalizeResult = this._normalizeResult;
        var timeOutHandler = DJ.Services.PlatformServiceProxy.SessionTimeoutHandler;
        var checkForSessionTimeout = this._checkForSessionTimeout;

        $.ajax({
            url: params.url,
            data: data,
            type: method,
            contentType: contentType,
            headers: headers,
            timeout: 180000,
            dataType: dataType,
            processData: (method === 'GET'),
            async: (params.async === undefined || params.async === null) || params.async,
            success: function (res) {
                var result = normalizeResult(res);
                onSuccess(result);
            },
            error: function (xhr, status, serverMessage) {
                var err = $dj.getError(xhr)
                        || { code: -1, message: status + " " + serverMessage };

                var isSessionTimeout = (err.code == -2147176633);

                if (isSessionTimeout && timeOutHandler) {
                    timeOutHandler(err);
                }
                else {
                    onError(err, xhr, serverMessage);
                }
            },
            complete: onComplete
        });
    },

    handleError: function (error, sender, response) {
        var parsedError = $dj.getError(error);
        $dj.debug('*** SERVICE PROXY ERROR - No handler attached *** ' + $dj.formatError(parsedError) || '[unknown]');
        $dj.debug('Attach your own onError event handler while doing invoke() to avoid seeing this message.');
    },

    handleComplete: function (jqXHR, textStatus) {
        $dj.debug('Service call completed. Results:', jqXHR, textStatus);
        $dj.debug('Attach your own onComplete event handler while doing invoke() to avoid seeing this message.');
    },

    handleSuccess: function (res) {
        $dj.debug('Service call successful. Results:', res);
        $dj.debug('Attach your own onSuccess event handler while doing invoke() to avoid seeing this message.');
    },

    _buildHeaders: function (params) {
        if (!params) {
            return;
        }

        var controlData = params.controlData || {};

        var preferences = params.preferences || { contentLanguages: [] };

        //TODO: revisit this logic, API needs actual text value instead of number
        var clockType = preferences && preferences.clockType === 1 ? "TwentyFourHours" : "TwelveHours";
        var timeZone = preferences && preferences.timeZone;

        var contentLanguages = preferences.contentLanguages || [];


        var contentLanguagesXml = "<contentLanguage>" + contentLanguages.join("</contentLanguage><contentLanguage>") + "</contentLanguage>";
        var interfaceLanguage = preferences.interfaceLanguage;

        var headers = {

            credentials: "<credentials>" +
                            this._createXmlTag("accessPointCode", controlData.AccessPointCode) +
                            this._createXmlTag("accessPointCodeUsage", controlData.AccessPointCodeUsage) +
                            this._createXmlTag("proxyUserId", controlData.ProxyUserId) +
                            this._createXmlTag("proxyUserNamespace", controlData.ProxyProductId) +
                            this._createXmlTag("remoteAddress", controlData.IpAddress) +
                            this._createXmlTag("token", controlData.SessionID) +
                            this._createXmlTag("tokenType", "sessionId") +
                         "</credentials>",

            preferences: "<preferences>" +
                            this._createXmlTag("clockType", clockType) +
                            this._createXmlTag("contentLanguages", contentLanguagesXml) +
                            this._createXmlTag("interfaceLanguage", interfaceLanguage) +
                            this._createXmlTag("timeZone", timeZone) +
                         "</preferences>"
        };

        if (controlData.Debug)
            headers["debug"] = true;

        return headers;
    },

    _createXmlTag: function (tagName, tagValue) {
        if (tagValue == null) {
            return "";
        }

        return "<" + tagName + ">" + tagValue + "</" + tagName + ">";
    },

    EOF: null
});

DJ.Services.PlatformServiceProxy.SessionTimeoutHandler = function () {
    // Set the default timeout handler to simply refresh the page
    window.location.reload();
};

$dj.proxy = $dj.proxy || new DJ.Services.PlatformServiceProxy();