/// <reference path="jquery.js" />
/// <reference path="common.js" />
//! ServiceProxy.js
$dj.registerNamespace('DJ.Services');



/*

jQuery ajax extensions

*/

(function ($) {

    // define the default timeout handler and expose a property that apps can override
    DJ.Services.PlatformServiceProxy = DJ.Services.PlatformServiceProxy || {}; 
    if(!DJ.Services.PlatformServiceProxy.SessionTimeoutHandler || 
        typeof DJ.Services.PlatformServiceProxy.SessionTimeoutHandler !== 'function') {
        DJ.Services.PlatformServiceProxy.SessionTimeoutHandler = function () {
                // Set the default timeout handler to simply refresh the page
                window.location.reload();
        };
    }
    if(!DJ.Services.PlatformServiceProxy.ForcedSessionExpireHandler || 
        typeof DJ.Services.PlatformServiceProxy.ForcedSessionExpireHandler !== 'function') {
        DJ.Services.PlatformServiceProxy.ForcedSessionExpireHandler = function () {
                // Set the default timeout handler to simply refresh the page
                window.location.reload();
        };
    }

    var defaultHandlers = {
        success: function (res) {
            $dj.debug('Service call successful. Results:', res);
            $dj.debug('Attach your own success event handler to avoid seeing this message.');
        },
        error: function (error, sender, response) {
            var parsedError = $dj.getError(error);
            $dj.debug('*** SERVICE PROXY ERROR - No handler attached *** ' + $dj.formatError(parsedError) || '[unknown]');
            $dj.debug('Attach your own error event handler to avoid seeing this message.');
        },
        complete: function (jqXHR, textStatus) {
            $dj.debug('Service call completed. Results:', jqXHR, textStatus);
            $dj.debug('Attach your own complete event handler to avoid seeing this message.');
        },
        abort: function (jqXHR, textStatus) {
            $dj.debug('***** SERVICE CALL ABORT ******');
        }
        //sessionTimeout: DJ.Services.PlatformServiceProxy.SessionTimeoutHandler,
        //forcedSessionExpire: DJ.Services.PlatformServiceProxy.ForcedSessionExpireHandler
    };


    var normalizeResult = function (result) {
        if(!result) { return null; }
        
        return result.d ? result.d : result;
    };

    // setup defaults. these can be overriden at individual request level
    $.ajaxSetup({
        timeout: 180000
        //type: 'GET',                                          // default in jQuery
        //contentType: 'application/x-www-form-urlencoded',     // default in jQuery
        //async: true,                                          // default in jQuery
    });

    $.ajaxPrefilter(function (options, originalOptions, jqXHR) {
        // do not send headers etc for external calls (anything outside of REST API domain)
        if(originalOptions.dataType === 'jsonp') { return; }
        
        // its a same domain call, possible to a controller action. do nothing
        if(options.crossDomain === false) { return; }

        // for REST API calls, crossDomain is true and datatype is != jsonp
        // proceed with adding headers and handlers

        var data = originalOptions.data;

        var onSessionTimeout = $.isFunction(originalOptions.sessionTimeout) ? originalOptions.sessionTimeout : DJ.Services.PlatformServiceProxy.SessionTimeoutHandler;
        var onForcedSessionExpire = $.isFunction(originalOptions.forcedSessionExpire) ? originalOptions.forcedSessionExpire : DJ.Services.PlatformServiceProxy.ForcedSessionExpireHandler;
        var onSuccess = $.isFunction(originalOptions.success) ? originalOptions.success : $dj.delegate(this, defaultHandlers.success);
        var onComplete = $.isFunction(originalOptions.complete) ? originalOptions.complete : $dj.delegate(this, defaultHandlers.complete);
        var onError = $.isFunction(originalOptions.error) ? originalOptions.error : $dj.delegate(this, defaultHandlers.error);
        var onAbort = $.isFunction(originalOptions.abort) ? originalOptions.abort : $dj.delegate(this, defaultHandlers.abort);

        var isGet = (originalOptions.type || options.type || 'GET').toUpperCase() === 'GET';
        if (!isGet) {
            data = $dj.isString(originalOptions.data) ? originalOptions.data : JSON.stringify(originalOptions.data);
            options.contentType = 'application/json';
            options.dataType = 'json';
            options.processData = false;
        }
        else if(originalOptions.data && !$dj.isString(originalOptions.data)) {
            data = $.param(originalOptions.data);
            options.processData = false;
        }
        else {
            if(originalOptions.data){
                options.processData = true;
            }
        }

        if (originalOptions.url.substr(0, 1) !== "/") {
            var hostname = originalOptions.host || (window.location.protocol + "//" + window.location.host);
            options.url = hostname + '<%= AppSetting("PlatformProxyBaseUrl", "/PlatformProxy.asmx") %>?url=' + escape(originalOptions.url);
            options.crossDomain = false;
        }

        $.extend(options, {
            headers: $dj.serializeGlobalHeaders(),
            data: data,
            success: function (res) {
                    var result = normalizeResult(res);
                    onSuccess(result);
                },
            error: function (xhr, status, serverMessage) {
                    if(status === "abort" || xhr.readyState !== 4) {
                        // Let the complete handler handle aborts
						$dj.warn('DJ.Services.PlatformServiceProxy::error: Ajax call aborted');
                        return;
                    }

                    var err = $dj.getError(xhr) || { code: -1, message: status + " " + serverMessage };

                    var isSessionTimeout = (err.code === -2147176633);
                    var isForcedSessionExpire = (err.code === -2147176629);

                    if (isSessionTimeout) {
                        onSessionTimeout(err);
                    }
                    else if(isForcedSessionExpire){
                        onForcedSessionExpire(err);
                    }
                    else {
                        onError(err, xhr, serverMessage);
                    }
                },
            complete: function(xhr, status) {
                    if(status === "abort" || xhr.readyState !== 4) {
						$dj.warn('DJ.Services.PlatformServiceProxy::complete: Ajax call aborted');
                        onAbort(xhr, status);
                    }
                    else {
                        onComplete(xhr, status);
                    }
                }
        });
            
    });
    
}(jQuery));