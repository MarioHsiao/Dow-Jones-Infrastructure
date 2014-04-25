(function ($, ace) {
    var djCore = window.djCore = window.djCore || {};
    
    djCore.Editor = function () {
        var langTools,
            editor,
            u = djCore.utils,
            loggerNamespace =  'djCore.Editor',
            validCategories = 'company|executive|newssubject|keyword|source|region_all|region_country|region_subSupraNationalRegion|region_stateOrProvince|industry|industry_nace|industry_sic|industry_naics'.split('|');

        var broker = {
            suggestContextObj: {
                'callInitiated': false, 
                'authToken':    null
            }
        };

        var dSettings = {
            url: '//suggest.factiva.com/Search/1.0',
            symbolDataUrl: '//data.dowjones.com/autocomplete/data',
            dataType: 'jsonp',
            authType: 'SuggestContext', // SuggestContext|SessionId|EncryptedToken;
            intercept: false,
            interceptList: [{category: 'source', prefix: 'rst:'}]
        }

        var dServiceOptions = {
            maxResults: 3,
            categories: 'company|executive|newssubject|source|keyword',
            it: 'stock',
            companyFilterSet: 'newsCodedAbt|newsCodedOccr|noADR',
            executiveFilterSet: 'newsCoded',
            showMatchingWord: 'true',
            autocompletionType: 'Categories',
            format: 'json',
            //suggestContext: 'YPC0P9uW1Y2BkE2YOeeHKT6yJEztjrrCTVui_2FK1veWMkRIpQfmgFse9HKCVrSLXFGfuqgoD_2F8vukMMT88DTMsYM_2F_2F3jHLBbM|2'
        };

        //Initialize Autocomplete
        function djEditor(o) {
            var self = this;
            self._o = o;
            self.$editor = $('#' + o.id);
            self._o.originalHeight = self.$editor.height();
            log(self._o.originalHeight);
            self.transport = new djCore.Transport("http://rhymebrain.com/talk?function=getRhymes&word=%QUERY%");
            self.settings = u.mixin({}, dSettings, o.settings);
            self.serviceOptions = u.mixin({}, dServiceOptions, o.serviceOptions);
            self.langTools = ace.require("ace/ext/language_tools");
            self.editor = ace.edit(o.id);
            self.editor.getSession().setMode(o.mode);
            self.editor.setOptions({
                enableBasicAutocompletion: true,
                enableSnippets: false,
                theme: o.theme
            });

            self.editor.renderer.setShowGutter(o.showGutter !== false);      // default is false
            self.editor.getSession().setUseWrapMode(o.useWrapMode !== true); // default is true

           
            log(self.settings);
            log(self.serviceOptions);
            log('calling: _initEvents');
            self._initEvents();
            log('calling: _initializeAutocomplete');
            self._initializeAutocomplete();

           /* self.editor.commands.on("afterExec", function (e) {
               /* log("afterExec");
                log(e.command.name + "::>" + /^[\w.]$/.test(e.args));
                if (e.command.name == "insertstring" && /^[\w.]$/.test(e.args)) {
                    
                log(e.args);
                    self.editor.execCommand("startAutocomplete");
                }#1#
            });
*/
            var autoCompleter = {
                getCompletions: function (ed, session, pos, prefix, callback) {
                    log('prefix:' + prefix);
                    var success = function (data) {
                        log('callback data')
                        log(data);
                        callback(null, [
                            {
                                name: "_or",
                                value: "or",
                                score: 1,
                                meta: "operator"
                            },
                            {
                                name: "_and",
                                value: "and",
                                score: 1,
                                meta: "operator"
                            },
                            {
                                name: "_not",
                                value: "not",
                                score: 1,
                                meta: "operator"
                            }
                        ]); return;
                    };

                    var failure = function(error) {
                        
                    };

                    if (prefix.length === 0) {
                        callback(null, [
                            {
                                name: "or",
                                value: "or",
                                score: 1,
                                meta: "operator"
                            },
                            {
                                name: "and",
                                value: "and",
                                score: 1,
                                meta: "operator"
                            },
                            {
                                name: "not",
                                value: "not",
                                score: 1,
                                meta: "operator"
                            }
                        ]); return;
                    }
                    var opts = {
                        id: o.id,
                        url: self.settings.url,
                        extraParams: self._setExtraParams(prefix)
                }
                    deferredRequest(prefix, opts, success, failure);
                }
            } 
            self.langTools.addCompleter(autoCompleter);
        }

        u.mixin(djEditor.prototype, {
            _initEvents: function () {
                var self = this;

                self.editor.getSession().on("change", function(e) {
                    logEvent('change', e);
                    self.updateHeight();
                });

                self.editor.getSession().on("tokenizerUpdate", function (e) {
                    logEvent('tokenizerUpdate', e);
                });

                self.editor.on("blur", function (e) {
                    logEvent('blur', e);
                });

                self.editor.on("tokenizerUpdate", function (e) {
                    logEvent('tokenizerUpdate', e);
                });
            },

            _initializeAutocomplete: function () {
                var self = this,
				    settings = self.settings,
				    serviceOptions = self.serviceOptions;
                if (settings && serviceOptions) {
                    var suggestContext;
                    if (settings.authType.toLowerCase() === "suggestcontext") {
                        suggestContext = serviceOptions.suggestContext;
                    }

                    if (suggestContext === undefined || $.trim(suggestContext).length === 0) {
                        if (broker.suggestContextObj.callInitiated !== true) {
                            self._getSuggestContextAndProcessRequest();
                        } else if (broker.suggestContextObj.authToken) {
                            serviceOptions.suggestContext = u.urlDecode(broker.suggestContextObj.authToken);
                            self._finalizeInit();
                        }
                    } else {
                        serviceOptions.suggestContext = u.urlDecode(suggestContext);
                        self._finalizeInit();
                    }
                }
            },

            _finalizeInit: function () {
                var self = this,
                  settings = self.settings,
                  serviceOptions = self.serviceOptions;

                log('done --> initialization');
            },

            updateHeight: function() {
                var self = this,
                 settings = self.settings,
                 serviceOptions = self.serviceOptions;

                var newHeight =
                    self.editor.getSession().getScreenLength()
                    * self.editor.renderer.lineHeight
                    + self.editor.renderer.scrollBar.getWidth();

                if (newHeight == self._o.originalHeight) {
                    return;
                }

                if (newHeight > self._o.originalHeight) {
                    self.$editor.height(newHeight + "px");
                    self.editor.resize();
                    return;
                }

                if (newHeight < self._o.originalHeight) {
                    self.$editor.height(self._o.originalHeight);
                    self.editor.resize();
                }
            },

            //Build parameters
            _setExtraParams: function (text) {
                var self = this,
                  settings = self.settings,
                  serviceOptions = self.serviceOptions;

                var paramsObj = {
                    format: 'json',
                    maxResults: 10,
                    autocompletionType: settings.autocompletionType,
                    searchText: text
                };
                var p = $.extend({}, paramsObj, serviceOptions);
                log(p);
                return p;
            },

            _getSuggestContextAndProcessRequest: function () {
                var self = this,
                    settings = self.settings,
                    serviceOptions = self.serviceOptions,
                    isUrlGenerated = false;

                //No valid authentication token. So generate a token based on sessionid or encrypted key
                if (settings.url.indexOf("/Search/") > 0) {
                    var authenticationUrl = settings.url.replace("/Search/", "/Authenticate/");
                    switch (settings.authType.toLowerCase()) {
                        case "sessionid":
                            if ($.trim(serviceOptions.authToken).length > 0) {
                                authenticationUrl = authenticationUrl + "/" + "RegisterUsingSessionId?SID=" + serviceOptions.authToken;
                                isUrlGenerated = true;
                            }
                            break;
                        case "encryptedtoken":
                            if ($.trim(serviceOptions.authToken).length > 0) {
                                authenticationUrl = authenticationUrl + "/" + "RegisterUsingEncryptedKey?eid=" + serviceOptions.authToken;
                                isUrlGenerated = true;
                            }
                            break;
                    }

                    if (isUrlGenerated === true) {
                        //Call the transaction and get the authentication token
                        $.jsonp({
                            url: authenticationUrl,
                            callbackParameter: "callback",
                            success: function (data) {
                                if (data.error) {
                                    /*if ($.isFunction(options.onError)) {
                                        //options.onError(data.error);
                                    }*/
                                } else {
                                    serviceOptions.suggestContext = u.urlDecode(data.key);
                                    self._finalizeInit();
                                }
                            }
                        });
                    }
                }
            }
        });
        
        return djEditor;

        function deferredRequest(term, options, success, failure) {
            if (!options.matchCase)
                term = term.toLowerCase();
            //var cdata = cache.load(term);
            // receive the cached data
            /*if (cdata && cdata.length) {
                success(term, cdata);*/
                // if an AJAX url has been supplied, try loading the data now
            /*  } else*/

            log('term>' + term);
            log(options);
            if ((typeof options.url == "string") && (options.url.length > 0)) {
                var extraParams = {
                    timestamp: + new Date()
                };
                $.each(options.extraParams, function (key, param) {
                    extraParams[key] = typeof param == "function" ? param() : param;
                });
                if (extraParams.autocompletionType == "Categories" && (options.extraParams.categories.toLowerCase().indexOf("symbol") >= 0)) {
                    options.url = options.url + "/" + extraParams.autocompletionType + "";
                    $.when(requestSuggestService(options, extraParams, options.id))
                        .done(function (data, symbols) {
                            if (data.error != undefined) {
                            } else {
                                if (symbols != null && symbols.error == null)
                                    data.category.push(symbols);
                                success({ term : term, data: data });
                            }
                        })
                        .fail(function () {
                            var error = { error: "Could not make jsonp call." };
                            failure(error);
                        });

                } else {
                    options.url = options.url + "/" + extraParams.autocompletionType + "";
                    $.when(requestSuggestService(options,extraParams, options.id))
                        .done(function (data) {
                            success({ term: term, data: data });
                        })
                        .fail(function () {
                            var error = { error: "Could not make jsonp call." };
                            failure(error);
                        });
                }

            } else {
                failure('unable to find items');
            }
        };

        function requestSuggestService(options, params, editorId) {
            var dfd = new $.Deferred();

            log(params);

            if (params.autocompletionType == "Categories") {
                var catArr = params.categories.split("|");
                catArr = $.grep(catArr, function (n) { return (n); });
                var index = $.inArray("symbol", catArr);
                if (index >= 0) {
                    catArr.splice(index, 1);
                }
                params.categories = catArr.join("|");
                if (catArr.length == 0) {
                    var data = { category: [], host: "", httpStatus: 200, version: "1.0" };
                    return dfd.resolve(data);
                }
            }

            $.jsonp({
                // try to leverage ajaxQueue plug-in to abort previous requests
                mode: "abort",
                cache: false,
                // limit abortion to this input
                port: "autocomplete" + editor,
                dataType: options.dataType,
                callbackParameter: "callback",
                url: options.url,
                data: params,
                success: function (data) {
                    dfd.resolve(data);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    dfd.reject(jqXhr);
                }
            });
            return dfd.promise();
        }

        function log(obj) {
            djCore.logger.debug(obj);
        }

        function logEvent(name, data) {
            log({
                name: name,
                data: data
            });
        }
    }();
}(jQuery, ace));