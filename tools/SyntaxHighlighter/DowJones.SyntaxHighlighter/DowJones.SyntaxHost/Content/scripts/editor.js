(function ($, ace) {
    var djCore = window.djCore = window.djCore || {};

    if (!String.prototype.contains) {
        String.prototype.contains = function() {
            return String.prototype.indexOf.apply(this, arguments) !== -1;
        };
    }

    String.prototype.startsWith = function (str) {
        return (str === this.substr(0, str.length));
    }
    String.prototype.ltrim = function () {
        var trimmed = this.replace(/^\s+/g, '');
        return trimmed;
    };
    String.prototype.rtrim = function () {
        var trimmed = this.replace(/\s+$/g, '');
        return trimmed;
    };
    Date.prototype._pad = function(number) {
        var r = String(number);
        if (r.length === 1) {
            r = '0' + r;
        }
        return r;
    }

    Date.prototype.ToFactivaIsoString = function() {
        return this.getUTCFullYear()
            + this._pad(this.getUTCMonth() + 1)
            + this._pad(this.getUTCDate());
    }
   
    
    djCore.Editor = function () {
        var langTools,
            editor,
            u = djCore.utils,
            loggerNamespace =  'djCore.Editor',
            validCategories = 'company|executive|newssubject|keyword|source|region_all|region_country|region_subSupraNationalRegion|region_stateOrProvince|industry|industry_nace|industry_sic|industry_naics'.split('|');

        var catMap = {
            fds: 'company',
            co: 'symbol',
            in: 'industry',
            ns: 'newssubject',
            sc: 'source',
            rst: 'source',
            au: 'author',
            pe: 'executive',
            re: 'region_all'
        }

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

        var dateOperators = [
            {
                name: "after",
                value: "date after",
                snippet: "date after -1",
                score: 1,
                meta: "date-operator"
            },
            {
                name: "before",
                value: "date before",
                snippet: "date before +1",
                score: 1,
                meta: "date-operator"
            },
            {
                name: "from_to",
                value: "date from to",
                snippet: "date from +1 to -1",
                score: 1,
                meta: "date-operator"
            },
            {
                name: "exact",
                value: "date exact",
                snippet: "date " + new Date().ToFactivaIsoString(),
                score: 1,
                meta: "date-operator"
            }
        ];

        var coreOperators = [
            { name: "or", value: "or", score: 1, meta: "operator" },
            { name: "and", value: "and", score: 1, meta: "operator" },
            { name: "not", value: "not", score: 1, meta: "operator" },
            { name: "adjacency", value: "adjacency", snippet: "adj1", score: 1, meta: "operator" },
            { name: "same", value: "same", snippet: "same", score: 1, meta: "operator" }, 
            { name: "near", value: "near", snippet: "near1", score: 1, meta: "operator" },
            { name: "atleast", value: "atleast", snippet: "atleast1", score: 1, meta: "operator" },
            { name: "word count", value: "word count", snippet: "wc>0", score: 1, meta: "operator" }
        ];

        var languageList = [
            { name: "English", value: "English", snipet: "en", score: 1, meta: "language" },
            { name: "French", value: "French",  snipet: "en", score: 1, meta: "language" },
            { name: "German", value: "German", snipet: "de", score: 1, meta: "language" },
            { name: "Spanish", value: "Spanish", snipet: "es", score: 1, meta: "language" },
            { name: "Portuguese", value: "Portuguese", snipet: "pt", score: 1, meta: "language" },
            { name: "Italian", value: "Italian", snipet: "it", score: 1, meta: "language" },
            { name: "Japanese", value: "Japanese", snipet: "js", score: 1, meta: "language" },
            { name: "Russian", value: "Russian", snipet: "ru", score: 1, meta: "language" },
            { name: "Chinese-Simplified", value: "Chinese-Simplified", snipet: "zhcn", score: 1, meta: "language" },
            { name: "Chinese-Tradional", value: "Chinese-Tradional", snipet: "zhtn", score: 1, meta: "language" },
            { name: "Korean", value: "Korean", snipet: "kn", score: 1, meta: "language" },
            { name: "English", value: "English", snipet: "en", score: 1, meta: "language" }
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
            self._disableEditorCommands();
            self._initializeAutocomplete();

            self.editor.commands.on("afterExec", function (e) {
                //if (e.command.name == "insertstring" && /^[\w|\s|=.]$/.test(e.args)) {
               /* switch (e.command.name.toLowerCase()) {
                    case 'insertstring':
                        if (e.args && e.args.length <= 1) {
                            self.editor.execCommand("startAutocomplete");
                        }
                        break;
                    case 'space':
                    //case 'backspace':
                        self.editor.execCommand("startAutocomplete");
                        break;
                }*/
            });

            var autoCompleter = {
                getCategory: function (session, pos) {
                    var r = pos.row,
                      c = pos.column,
                      curToken = session.getTokenAt(r, c);
                    if (curToken.start === 0) {
                        return undefined;
                    }

                    log("getCategory");
                    log(curToken);
                    if (curToken) {
                        if (curToken.type == "paren.lparen" ||
                            curToken.type == "keyword.equals" || 
                            curToken.type == 'keyword.operator' || 
                            curToken.type == 'text') {
                            return this.getCategory(session, { row: pos.row, column: curToken.start - 1 });
                        }
                        if (curToken.type == "keyword.fii") {
                            
                            return curToken.value;
                        }
                    }
                    return undefined;
                },

                getCompletions: function (ed, session, pos, prefix, callback) {
                    var r = pos.row,
                        c = pos.column,
                        curToken = session.getTokenAt(r, c),
                        includeCategory = true,
                        category = this.getCategory(session, { row: pos.row, column: curToken.start - 1 });

                    if (curToken.type === 'text') {
                        if (category === 'la') {
                            callback(null, languageList);
                            return;
                        }
                    }

                    if (curToken.type === 'phrase' || curToken.token === 'keyword.equals') {
                        callback(null, []);
                        return;
                    }

                    if (curToken.type === 'text') {
                        var query = $.trim(curToken.value),
                             opts = {
                                 id: o.id,
                                 url: self.settings.url,
                                 extraParams: self._setExtraParams(query)
                             }

                        if (query.startsWith('"') || query.startsWith('/') ||
                            query.startsWith('-') || query.startsWith('+')) {
                            return;
                        }

                        // write code to find category
                        if (category) {
                            opts.extraParams.categories = catMap[category];
                            includeCategory = false;
                        }
                        
                        if (query && query.length > 0) {
                            deferredRequest(
                                query,
                                opts,
                                function(data) {
                                    callback(null, self._filter(opts.extraParams.categories.split('|'), data, includeCategory));
                                },
                                function(err) { callback(null, coreOperators.concat(dateOperators)); }
                            );
                            return;
                        }
                    }

                    if (prefix.length === 0) {
                        callback(null, coreOperators.concat(dateOperators));
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

            __snippetMapper: function (cat, code, name, includeCategory) {

                if (!includeCategory) {
                    switch (cat.toLowerCase()) {
                        case 'executive':
                        case 'keyword':
                            break;
                        default:
                            return code;
                    }
                }

                switch(cat.toLowerCase()) {
                    case 'company':
                        return 'fds=' + code;
                    case 'newssubject':
                        return 'ns=' + code;
                    case 'region':
                        return 're=' + code;
                    case 'industry':
                        return 'in=' + code;
                    case 'language':
                        return 'la=' + code;
                    case 'source':
                    case 'outlet':
                        return 'rst=' + code;
                    case 'author':
                        return 'au=' + code;
                    case 'executive':
                        if (code) {
                            return 'pe=' + code;
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

            _filter: function (categories, parsedResponse, includeCategory) {
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
                                el.snippet = self.__snippetMapper(category, el.code, el.name, includeCategory);
                                items.push(el);
                            });
                        }
                    }
                });
               
                return items;
            },

            _disableEditorCommands: function () {
                var self = this;

                var items = [
                    { name: 'unfind',               winKey: 'Ctrl-F',   macKey: 'Command-F' },
                    { name: 'uncenter_selection',   winKey: 'Ctrl-L',   macKey: 'Command-L' },
                    { name: 'remove_settings',      winKey: 'Ctrl-,',   macKey: 'Command-,' }
                ];

                $.each(items, function (i, item) {
                    log(item.name)
                    self.editor.commands.addCommands([
                        {
                            name: item.name,
                            bindKey: {
                                win: item.winKey,
                                mac: item.macKey
                            },
                            exec: function() {
                                return false;
                            },
                            readOnly: true
                        }
                    ]);
                });
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
                    
                    if (token.type == 'text') {
                        //log(token);
                    }
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