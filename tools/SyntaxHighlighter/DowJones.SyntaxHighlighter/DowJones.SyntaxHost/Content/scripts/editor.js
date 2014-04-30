(function ($, ace) {
    var djCore = window.djCore = window.djCore || {};

    if (!String.prototype.contains) {
        String.prototype.contains = function () {
            return String.prototype.indexOf.apply(this, arguments) !== -1;
        };
    }
    
    djCore.Editor = function () {
        var langTools,
            editor,
            u = djCore.utils,
            loggerNamespace =  'djCore.Editor',
            validCategories = 'company|executive|newssubject|keyword|source|region_all|region_country|region_subSupraNationalRegion|region_stateOrProvince|industry|industry_nace|industry_sic|industry_naics'.split('|');

        var categoryMap = {
            author: 'author',
            outlet: 'outlet',
            executive: 'executive',
            company: 'company',
            region: 'region',
            publishercity: 'publisherCity',
            publishermetadata: 'publisherData',
            newssubject: 'newsSubject',
            industry: 'industry',
            industry_nace: 'industry',
            industry_sic: 'industry',
            industry_naics: 'industry',
            source: 'source',
            keyword: 'keyWord',
            region_all: 'region',
            region_country: 'region',
            region_stateorprovince: 'region',
            region_metropolitanarea: 'region',
            region_subnationalregion: 'region',
            region_supranationalregion: 'region',
            region_subsupranationalregion: 'region',
            calendarkeyword: 'keyword',
            calendarcompany: 'calendarCompany',
            symbol: 'symbol'
        };

        var operators = [
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
            }, {
                name: "adjacency",
                value: "adjacency",
                snippet: "adj1",
                score: 1,
                meta: "operator"
            }, {
                name: "same",
                value: "same",
                snippet: "same",
                score: 1,
                meta: "operator"
            }, {
                name: "near",
                value: "near",
                snippet: "near1",
                score: 1,
                meta: "operator"
            }, {
                name: "near",
                value: "near",
                snippet: "near1",
                score: 1,
                meta: "operator"
            }, {
                name: "atleast",
                value: "atleast",
                snippet: "atleast1",
                score: 1,
                meta: "operator"
            }, {
                name: "word count",
                value: "word count",
                snippet: "wc>0",
                score: 1,
                meta: "operator"
            }
        ];

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
            interceptList: [{ category: 'source', prefix: 'rst:' }],
            showGutter: false,
            showWrapMode: true,
        }

        var dServiceOptions = {
            maxResults: 10,
            categories: 'company|region_all|executive|newssubject|source|keyword',
            it: 'stock',
            companyFilterSet: 'newsCodedAbt|newsCodedOccr|noADR',
            executiveFilterSet: 'newsCoded',
            showMatchingWord: 'true',
            autocompletionType: 'Categories',
            format: 'json'
        };

        //Initialize Autocomplete
        function djEditor(o) {
            var self = this;
            self._o = o;
            self.$editor = $('#' + o.id);
            self._o.originalHeight = self.$editor.height();
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

            self.editor.renderer.setShowGutter(self.settings.showGutter !== false);      // default is false
            self.editor.getSession().setUseWrapMode(self.settings.showWrapMode === true); // default is true
            self._initEvents();
            self._initializeAutocomplete();

            self.editor.commands.on("afterExec", function (e) {
                //if (e.command.name == "insertstring" && /^[\w|\s|=.]$/.test(e.args)) {
                log(e.command.name);
                log('args>' + e.args);
                switch (e.command.name.toLowerCase()) {
                    case 'insertstring':
                        if (e.args && e.args.length <= 1) {
                            self.editor.execCommand("startAutocomplete");
                        }
                        break;
                    case 'space':
                    case 'backspace':
                        self.editor.execCommand("startAutocomplete");
                        break;
                }
            });

            var autoCompleter = {
                getCompletions: function (ed, session, pos, prefix, callback) {
                    var r = pos.row,
                        c = pos.column,
                        curToken = session.getTokenAt(r, c); 

                    log(curToken);

                    if (curToken.type === 'keyword.equals' || curToken.type === 'comment') {
                        callback(null, []);
                        return;
                    }

                    if (curToken.type === 'text') {
                        var query = curToken.value.replace(/^#\?/, '');

                        if (query.contains('"')) {
                            return;
                        }

                        if (query && query.length > 0) {
                            var opts = {
                                id: o.id,
                                url: self.settings.url,
                                extraParams: self._setExtraParams(query)
                            }

                            deferredRequest(
                                query,
                                opts,
                                function(data) {
                                    callback(null, self._filter(self.serviceOptions.categories.split('|'), data));
                                },
                                function(err) { callback(null, operators); }
                            );
                            return;
                        }
                    }

                    if (prefix.length === 0) {
                        callback(null, operators);
                        return;
                    }
                    
                    return;
                }
            } 
            self.langTools.addCompleter(autoCompleter);
        }
        
        u.mixin(djEditor.prototype, {
            __categoryMapper: function (cat) {
                var self = this;
                return categoryMap[cat];
            },

            setTheme: function (val) {
                var self = this;
                self.editor.setOptions({
                    theme: val
                });
            },

            __snippetMapper: function (cat, code, name) {

                switch(cat.toLowerCase()) {
                    case 'company':
                        return 'fds=' + code + ' /* ' + name + ' */';
                    case 'newssubject':
                        return 'ns=' + code + ' /* ' + name + ' */';
                    case 'region':
                        return 're=' + code + ' /* ' + name + ' */';
                    case 'industry':
                        return 'in=' + code + ' /* ' + name + ' */';
                    case 'language':
                        return 'la=' + code + ' /* ' + name + ' */';
                    case 'source':
                    case 'outlet':
                        return 'rst=' + code + ' /* ' + name + ' */';
                    case 'author':
                        return 'au=' + code + ' /* ' + name + ' */';
                    case 'executive':
                        if (code) {
                            return 'pe=' + code + ' /* ' +  name + ' */';
                        }
                        return '\"' + name + '\"';
                    case 'keyword':
                        return '\"' + name + '\"';
                }
            },

            __scoreMapper: function (cat) {
                switch (cat.toLowerCase()) {
                    case 'company':
                        return 10;
                    case 'newssubject':
                        return 9;
                    case 'region':
                        return 8;
                    case 'industry':
                        return 7;
                    case 'language':
                        return 6;
                    case 'source':
                    case 'outlet':
                        return 5;
                    case 'executive':
                    case 'author':
                        return 4;
                    case 'keyword':
                        return 3;
                }
            },

            _filter: function (categories, parsedResponse) {
                var self = this,
                    items = [];
                var data = _.pick(parsedResponse.data, 'category').category;

                $.each(categories, function (i, category) {
                    category = self.__categoryMapper(category.toLowerCase());
                    var item = data[i];
                    if (item) {
                        var tempResponse = _.pick(item, category);
                        if (tempResponse && tempResponse[category] && tempResponse[category].length && tempResponse[category].length > 0) {
                            var tObj = tempResponse[category];
                            $.each(tObj, function (index, el) {
                                el.name = el.value = el.value || el.descriptor || el.completeName || el.formalName || el.word;
                                el.value = el.name;
                                el.meta = category;
                                el.score = self.__scoreMapper(category);
                                el.snippet = self.__snippetMapper(category, el.code, el.name);
                                items.push(el);
                            });
                        }
                    }
                });
               
                return items;
            },

            _initEvents: function () {
                var self = this;

                self.editor.on('mousemove', function (e) {
                    var position = e.getDocumentPosition();
                    var token = self.editor.session.getTokenAt(position.row, position.column);
                    
                });

                self.editor.on('dblclick', function (e) {
                    var position = e.getDocumentPosition();
                    var token = self.editor.session.getTokenAt(position.row, position.column);
                    log('dblclick')
                    log(token)
                });

                self.editor.getSession().on("change", function(e) {
                    //logEvent('change', e);
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