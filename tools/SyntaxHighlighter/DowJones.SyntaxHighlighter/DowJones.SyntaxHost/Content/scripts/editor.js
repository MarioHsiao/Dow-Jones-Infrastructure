(function ($, ace) {
    var djCore = window.djCore = window.djCore || {};
    
    djCore.Editor = function () {
        var langTools, editor;
        var dSettings = {
            url: '//suggest.int.factiva.com/Search/1.0',
            symbolDataUrl: '//data.dowjones.com/autocomplete/data',
            dataType: 'jsonp',
            authType: 'SuggestContext',
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
            suggestContext: ''
        };

        function djEditor(o) {
            var self = this, 
                u = djCore.utils;

            self.settings = u.mixin({}, o.settings, dSettings);
            self.serviceOptions = u.mixin({}, o.serviceOptions, dServiceOptions);
            langTools = ace.require("ace/ext/language_tools");
            editor = ace.edit(o.id);
            editor.setTheme(o.theme);
            editor.getSession().setMode(o.mode);
            editor.setOptions({
                enableBasicAutocompletion: true,
                enableSnippets: false
            });
           
            var rhymeCompleter = {
                getCompletions: function (ed, session, pos, prefix, callback) {
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

                    var t = new djCore.Transport("http://rhymebrain.com/talk?function=getRhymes&word=%QUERY%");

                    t.get(prefix,
                        function (wordList) {
                            // wordList like [{"word":"flow","freq":24,"score":300,"flags":"bc","syllables":"1"}]
                            callback(null, wordList.map(function (ea) {
                                return { name: ea.word, value: ea.word, score: ea.score, meta: "rhyme" }
                            }));
                        });
                }
            }

            langTools.addCompleter(rhymeCompleter);
        }

        return djEditor;

        

        //Initialize Autocomplete
        function init() {
            var self = this,
                settings = self.settings,
                serviceOptions = self.serviceOptions;

            if (settings && serviceOptions) {
                var autosuggestPrototype = self.prototypeObj;
                var suggestContext;
                if (settings.authType.toLowerCase() === "suggestcontext") {
                    suggestContext = serviceOptions.suggestContext;
                }

                if (suggestContext === undefined || $.trim(suggestContext).length === 0) {
                    if (autosuggestPrototype.suggestContextObj.callInitiated !== true) {
                        getSuggestContextAndProcessRequest();
                    } else if (autosuggestPrototype.suggestContextObj.authToken) {
                        serviceOptions.suggestContext = djCore.utils.urlDecode(autosuggestPrototype.suggestContextObj.authToken);
                        finalizeInit();
                    }
                } else {
                    serviceOptions.suggestContext = djCore.utils.urlDecode(suggestContext);
                    finalizeInit();
                }
            }
        };


        function finalizeInit() {
            
        }

        function initEvents() {
            console.log(this);
            
            editor.getSession().on("change", function (e) {
                logEvent('change', e);
            });

            editor.getSession().on("tokenizerUpdate", function (e) {
                logEvent('tokenizerUpdate', e);
            });

            editor.on("blur", function (e) {
                logEvent('blur', e);
            });

            editor.on("tokenizerUpdate", function (e) {
                logEvent('tokenizerUpdate', e);
            });
        }

        function getSuggestContextAndProcessRequest() {
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
                    crossDomain({
                        url: authenticationUrl,
                        callbackParameter: "callback",
                        success: function (data) {
                            if (data.error) {
                                /*if ($.isFunction(options.onError)) {
                                    //options.onError(data.error);
                                }*/
                            } else {
                                serviceOptions.authToken = djCore.utils.urlDecode(data.key);
                                finalizeInit();
                            }
                        }
                    });
                }
            };
        }

        function log(obj) {
            if (console && console.log) {
                console.log(obj);
            }
        }

        function logEvent(name, data) {
            log({
                name: name,
                data: data
            });
        }
    }();
}(jQuery, ace));