/*!
 * AutoSuggestComponent
 */

DJ.UI.AutoSuggest = DJ.UI.Component.extend({

    /*
    * Properties
    */

    // Default options
    defaults: {
        debug: false,
        cssClass: 'AutoSuggestComponent'
    },

    //Global variables
    prototypeObj: {
        suggestContextObj: { 'callInitiated': false, 'authToken': null },
        events: { 'promoteEventRegistered ': false,
            'infoEventRegistered ': false,
            'discontEventRegistered ': false,
            'notEventRegistered ': false,
            'viewMorePrivateMarketsEventRegistered ': false
        }
    },

    //Events
    events: {
        // jQuery events are namespaced as <event>.<namespace>
        itemSelect: "itemSelect.dj.AutoSuggest",
        freeTextSelect: "Select.dj.AutoSuggest",
        viewAllClick: "viewAllClick.dj.AutoSuggest",
        viewMorePrivateMarketsClick: "viewMorePrivateMarketsClick.dj.AutoSuggest",
        infoClick: "infoClick.dj.AutoSuggest",
        promoteClick: "promoteClick.dj.AutoSuggest",
        notClick: "notClick.dj.AutoSuggest",
        discontClick: "discontClick.dj.AutoSuggest"
    },

    // Localization/Templating tokens
    tokens: {
        // name: Data     // add more defaults here separated by comma
        companyTkn: "${companyTkn}",
        executiveTkn: "${executiveTkn}",
        industryTkn: "${industryTkn}",
        industry_naceTkn: "${industry_naceTkn}",
        industry_naicsTkn: "${industry_naicsTkn}",
        industry_sicTkn: "${industry_sicTkn}",
        sourceTkn: "${sourceTkn}",
        keywordTkn: "${keywordTkn}",
        privateMarketCompanyHeaderTkn: "${privateMarketCompanyHeaderTkn}",
        privateMarketIndustryHeaderTkn: "${privateMarketIndustryHeaderTkn}",
        privateMarketRegionHeaderTkn: "${privateMarketRegionHeaderTkn}",
        region_allTkn: "${region_allTkn}",
        region_countryTkn: "${region_countryTkn}",
        region_stateOrProvinceTkn: "${region_stateOrProvinceTkn}",
        region_metropolitanAreaTkn: "${region_metropolitanAreaTkn}",
        region_subNationalRegionTkn: "${region_subNationalRegionTkn}",
        region_supraNationalRegionTkn: "${region_supranationalRegionTkn}",
        region_subSupraNationalRegionTkn: "${region_subSupranationalRegionTkn}",
        newssubjectTkn: "${newsSubjectTkn}",
        infoTitleTknPre: "${infoTitleTknPre}",
        infoTitleTknPost: "${infoTitleTknPost}",
        promoteTitleTkn: "${promoteTitleTkn}",
        notTitleTkn: "${notTitleTkn}",
        sourcefamilyTkn: "${sourcefamilyTkn}",
        symbolTkn: "${symbolTkn}",
        publicationTkn: "${publicationTkn}",
        webpageTkn: "${webpageTkn}",
        multimediaTkn: "${multimediaTkn}",
        pictureTkn: "${pictureTkn}",
        blogTkn: "${blogTkn}",
        disContTkn: "${disContTkn}",
        viewAllTkn: "${viewAllTkn}",
        viewAllPreTkn: "${viewAllPreTkn}",
        viewAllPostTkn: "${viewAllPostTkn}",
        screeningTextPreTkn: "${screeningTextPreTkn}",
        screeningTextPostTkn: "${screeningTextPostTkn}",
        helpLabelTkn: "${helpLabelTkn}",
        privateMarketCompanyViewMoreTkn: "${privateMarketCompanyViewMoreTkn}",
        privateMarketIndustryViewMoreTkn: "${privateMarketIndustryViewMoreTkn}",
        privateMarketRegionViewMoreTkn: "${privateMarketRegionViewMoreTkn}"
    },

    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "AutoSuggest" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        //Bind the template
        $(this.$element).html(this.templates.success);

        $('.dj_AutoSuggest', element).attr('id', this.options.controlId);

        // TODO: Add custom initialization code like the following:
        // this._testButton = $('.testButton', element).get(0);
        var suggestSettings = $.extend( {
            url: this.options.suggestServiceUrl,
            controlId: this.options.controlId,
            autocompletionType: this.options.autocompletionType,
            authType: this.options.authType,
            authTypeValue: this.options.authTypeValue,
            options: typeof(this.options.serviceOptions) === "object" ? this.options.serviceOptions : $.parseJSON(this.options.serviceOptions),
            columns: this.options.columns,
            tokens: typeof(this.options.tokens) === "object" ? this.options.tokens : $.parseJSON(this.options.tokens),
            fillInputOnKeyUpDown: this.options.fillInputOnKeyUpDown,
            eraseInputOnItemSelect: this.options.eraseInputOnItemSelect,
            inputClass: this.options.controlClassName,
            searchBtnId: ('searchBtnId' in this.options) ? this.options.searchBtnId : "",
            resultContainer: ('resultContainer' in this.options) ? this.options.resultContainer : "",
            selectFirst: this.options.selectFirst,
            showViewAll: this.options.showViewAll,
            showSearchText: this.options.showSearchText,
            showHelp: this.options.showHelp
        }, this.options);

        this.initialize(suggestSettings);
    },

    /*
    * Public methods
    */
    initialize: function (settings) {
        if (settings != undefined) {
            var targetObj = $('#' + settings.controlId);
            //Throw Exception
            if (!targetObj || targetObj.type != 'text') {
                //document.write("<input id=\"" + settings.controlId + "\" name=\"" + settings.controlId + "\" class=\"" + settings.controlClassName + "\" type=\"text\"/>");
            }

            //Enable/Disable Speech Input | Default: disabled
            this._initSpeechInput(settings, targetObj);

            //Initialize Autosuggest
            this._initAutoComplete.call(this, settings);
        }
    },

    /*
    * Private methods
    */
    //Return an object which gives Data depending on autosuggestion type
    _getValueByAutoSuggestType: function () {
        return {
            author: "formalName",
            outlet: "formalName",
            executive: "completeName",
            company: "value",
            privatemarketcompany: "companyName",
            privatemarketindustry: "industryName",
            privatemarketregion: "regionName",
            region: "descriptor",
            publishercity: "formalName",
            publishermetadata: "name",
            newssubject: "descriptor",
            industry: "descriptor",
            industry_nace: "descriptor",
            industry_sic: "descriptor",
            industry_naics: "descriptor",
            source: "formalName",
            keyword: "word",
            region_all: "descriptor",
            region_country: "descriptor",
            region_stateorprovince: "descriptor",
            region_metropolitanarea: "descriptor",
            region_subnationalregion: "descriptor",
            region_supranationalregion: "descriptor",
            region_subsupranationalregion: "descriptor",
            calendarkeyword: "word",
            symbol: "ticker"
        };
    },

    //Object which gives name depending on autosuggestion type
    _getNameByAutoSuggestType: function () {
        return {
            author: "author",
            outlet: "outlet",
            executive: "executive",
            company: "company",
            region: "region",
            publishercity: "publisherCity",
            publishermetadata: "publisherData",
            newssubject: "newsSubject",
            industry: "industry",
            industry_nace: "industry",
            industry_sic: "industry",
            industry_naics: "industry",
            source: "source",
            keyword: "keyWord",
            region_all: "region",
            region_country: "region",
            region_stateorprovince: "region",
            region_metropolitanarea: "region",
            region_subnationalregion: "region",
            region_supranationalregion: "region",
            region_subsupranationalregion: "region",
            calendarkeyword: "keyword",
            calendarcompany: "calendarCompany",
            symbol: "symbol"
        };
    },

    //Build parameters
    _setExtraParams: function (settings) {
        var paramsObj = {
            format: 'json',
            maxResults: 10,
            autocompletionType: settings.autocompletionType,
            suggestContext: settings.authTypeValue,
            searchText: function () {
                return $("#" + settings.controlId).val();
            },
            showViewAllPrivateMarkets: $.isFunction(settings.onViewMorePrivateMarketsClick)
        };
        
        if (settings.tokens === undefined || settings.tokens === null) {
            settings.tokens = {};
        }
        settings.tokens = $.extend({}, this.tokens, settings.tokens);
        return $.extend({}, paramsObj, settings.options);
    },

    //Set CSS Default Classes
    _setCssDefaults: function (settings) {
        if (settings.resultsClass == undefined) {
             settings.resultsClass = "dj_emg_autosuggest_results";
        }
        
        if (settings.resultsEvenClass == undefined) {
            settings.resultsEvenClass = "dj_emg_autosuggest_even";
        }

        if (settings.resultsOddClass == undefined) {
            settings.resultsOddClass = "dj_emg_autosuggest_odd";
        }

        if (settings.resultsOverClass == undefined) {
            settings.resultsOverClass = "dj_emg_autosuggest_over";
        }

        if (settings.viewAllClass == undefined) {
            settings.viewAllClass = "dj_emg_autosuggest_viewall";
        }
    },

    //Function to get Autosuggest List Item
    _getFormattedRow: function (settings, row, defaultVal) {
        var getValueByAutoSuggestType = this._getValueByAutoSuggestType();
        var t = [];
        var tokens = settings.tokens;
        var extraTknInfo = row[getValueByAutoSuggestType[row.controlType.toLowerCase()]];
        var infoTitleTknArrPreVal, infoTitleTknArrPostVal, newInfoTitleTkn;
        if ((tokens.infoTitleTknPre != undefined) && (tokens.infoTitleTknPost != undefined) && (tokens.infoTitleTknPre != null) && (tokens.infoTitleTknPost != null)) {
            var infoTitleTknStr = new String(tokens.infoTitleTkn);
            var infoTitleTknArr = infoTitleTknStr.split('|');
            if (tokens.infoTitleTkn != undefined) {
                infoTitleTknArrPreVal = (infoTitleTknArr[0] != undefined) ? infoTitleTknArr[0] : "";
                infoTitleTknArrPostVal = (infoTitleTknArr[2] != undefined) ? infoTitleTknArr[2] : "";
                if (infoTitleTknArr[1] === "true") {
                    newInfoTitleTkn = infoTitleTknArrPreVal + " " + extraTknInfo + " " + infoTitleTknArrPostVal;
                }
                else {
                    newInfoTitleTkn = infoTitleTknArrPreVal + " " + infoTitleTknArrPostVal;
                }
            }
            else {
                newInfoTitleTkn = tokens.infoTitleTknPre + " " + tokens.infoTitleTknPost;
            }
        }
        else {
            newInfoTitleTkn = tokens.infoTitleTknPre + " " + tokens.infoTitleTknPost;
        }

        var colArr;
        var eventObj = {
            "onInfoClick": $.isFunction(settings.onInfoClick),
            "onPromoteClick": $.isFunction(settings.onPromoteClick),
            "onNotClick": $.isFunction(settings.onNotClick)
        };
        var eventCount = 0;
        for (var i in eventObj) {
            if (eventObj[i] === true) {
                eventCount = eventCount + 1;
            }
        }

        if (settings.columns !== undefined) {
            colArr = settings.columns.split("|");
        }

        if (!settings.columns || colArr.length === 1) {
            t[t.length] = "<td>";
            var sourceClass = "";
            var sourceToken = "";
            if ((settings.autocompletionType.toLowerCase() === "source")) {
                sourceClass = " acSource_" + row.type;
                switch (row.type.toLowerCase()) {
                    case "sourcefamily":
                        sourceToken = settings.tokens["sourcefamilyTkn"];
                        break;
                    case "publication":
                        sourceToken = settings.tokens["publicationTkn"];
                        break;
                    case "webpage":
                        sourceToken = settings.tokens["webpageTkn"];
                        break;
                    case "multimedia":
                        sourceToken = settings.tokens["multimediaTkn"];
                        break;
                    case "picture":
                        sourceToken = settings.tokens["pictureTkn"];
                        break;
                    case "blog":
                        sourceToken = settings.tokens["blogTkn"];
                        break;
                }
            }

            t[t.length] = " <div class=\"ac_descriptor" + sourceClass + "\">";
            if (settings.autocompletionType.toLowerCase() === "source") {
                t[t.length] = "<span title=\"" + sourceToken + "\" class=\"" + sourceClass + "\">" + sourceToken + ":</span> ";
            }
            t[t.length] = defaultVal;

            if (settings.autocompletionType.toLowerCase() === "author" && row.isPrimaryOutlet) {
                t[t.length] = " <span class=\"ac_diasambiguityOutlet\">(";
                t[t.length] = row.outletName;
                t[t.length] = ")</span>";
            }

            if (settings.autocompletionType.toLowerCase() === "executive" && row.companyName
                && row.companyName.length > 0 && row.completeName.toLowerCase().indexOf(row.companyName.toLowerCase()) < 0) {
                t[t.length] = " <span class=\"ac_diasambiguityOutlet\">(";
                t[t.length] = row.companyName;
                t[t.length] = ")</span>";
            }

            t[t.length] = "</div>";

            if ((settings.autocompletionType != "KeyWord") && $.isFunction(settings.onInfoClick)) {

                t[t.length] = "<a href=\"javascript:void(0)\" class=\"ac_info\" title=\"" + newInfoTitleTkn + "\">information</a>";
            }

            if ($.isFunction(settings.onPromoteClick)) {
                t[t.length] = "<a href=\"javascript:void(0)\" class=\"ac_promote\" title=\"" + tokens.promoteTitleTkn + "\">promote</a>";
            }

            if ($.isFunction(settings.onNotClick)) {
                t[t.length] = "<a href=\"javascript:void(0)\" class=\"ac_not\" title=\"" + tokens.notTitleTkn + "\">not</a>";
            }

            if ((settings.autocompletionType.toLowerCase() === "source")) {
                if (row.status.toLowerCase() === "discont") {
                    t[t.length] = "<a href=\"javascript:void(0)\" class=\"ac_discont\" title=\"" + tokens.disContTkn + "\">discontinued</a>";
                }
            }
            if (settings.autocompletionType.toLowerCase() === "author") {
                if (!row.isActive) {
                    t[t.length] = "<a href=\"javascript:void(0)\" class=\"ac_discont\" title=\"" + tokens.disContTkn + "\">discontinued</a>";
                }
            }


            t[t.length] = "</td>";
        }
        else {
            for (var c = 0; c < colArr.length; c++) {
                t[t.length] = "<td>" + row[colArr[c]] + "</td>";
            }
            if (eventCount > 0) {
                t[t.length] = "<td>";
                for (var e in eventObj) {
                    switch (e.toLowerCase()) {
                        case "oninfoclick":
                            if ((settings.autocompletionType != "KeyWord") && (eventObj[e] === true)) {
                                t[t.length] = "<a href=\"javascript:void(0)\" class=\"ac_info\" title=\"" + newInfoTitleTkn + "\">information</a>";
                            }
                            break;
                        case "onpromoteclick":
                            if (eventObj[e] === true) {
                                t[t.length] = "<a href=\"javascript:void(0)\" class=\"ac_promote\" title=\"" + tokens.promoteTitleTkn + "\">promote</a>";
                            }
                            break;
                        case "onnotclick":
                            if (eventObj[e] === true) {
                                t[t.length] = "<a href=\"javascript:void(0)\" class=\"ac_not\"" + tokens.notTitleTkn + "\">not</a>";
                            }
                            break;
                    }
                }
                t[t.length] = "</td>";
            }
        }
        return t.join("");
    },

    //Function to get Formatted Category Row
    _getFormattedCategoryRow: function (settings, row) {
        var t = [];
        var getValueByAutoSuggestType = this._getValueByAutoSuggestType();
        var rowValue = row[getValueByAutoSuggestType[row.controlType.toLowerCase()]];
        t[t.length] = "<td><div class=\"ac_descriptor\">";
        t[t.length] = rowValue;
        switch (row.controlType.toLowerCase()) {
            case "executive":
                if (row.companyName && row.companyName.length > 0 && row.completeName.toLowerCase().indexOf(row.companyName.toLowerCase())<0) {
                    t[t.length] = " <span class=\"ac_executiveCompany\">(";
                   t[t.length] = row.companyName;
                   t[t.length] = ")</span>";
               }
                break;
            case "symbol":
                if (row.company && row.company.length > 0) {
                    t[t.length] = " <span class=\"ac_\" >&nbsp;&nbsp;&nbsp;";
                    t[t.length] = row.company;
                    t[t.length] = "</span>";
                }
                break;
        }
        t[t.length] = "</div></td>";
        return t.join("");
    },

    //Function to get Authentication Token
    _getSuggestContextAndProcessRequest: function (settings) {
        var isUrlGenerated = false;
        var self = this;
        //No valid authentication token. So generate a token based on sessionid or encrypted key
        if (settings.url.indexOf("/Search/") > 0) {
            var authenticationUrl = settings.url.replace("/Search/", "/Authenticate/");
            switch (settings.authType.toLowerCase()) {
                case "sessionid":
                    if ($.trim(settings.authTypeValue).length > 0) {
                        authenticationUrl = authenticationUrl + "/" + "RegisterUsingSessionId?SID=" + settings.authTypeValue;
                        isUrlGenerated = true;
                    }
                case "encryptedtoken":
                    if ($.trim(settings.authTypeValue).length > 0) {
                        authenticationUrl = authenticationUrl + "/" + "RegisterUsingEncryptedKey?eid=" + settings.authTypeValue;
                        isUrlGenerated = true;
                    }
            }

            if (isUrlGenerated === true) {
                //Call the transaction and get the authentication token
                DJ.crossDomain({
                    url: authenticationUrl,
                    callbackParameter: "callback",
                    success: function (data) {
                        if (data.error != undefined) {
                            if ($.isFunction(settings.onError)) {
                                settings.onError(data.error);
                            }
                        }
                        else {
                            settings.authTypeValue = self._URLDecode(data.key);
                            self._processRequest.call(self, settings);
                        }
                    }
                });
            }
        }
    },

    //Function to decode a URL (This should be moved to common js file)
    _URLDecode: function (encodedString) {
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

    //Function to get parsed category rows
    _processCategories: function (data, settings, rows) {
        data = data.category;
        var getValueByAutoSuggestType = this._getValueByAutoSuggestType(),
            getNameByAutoSuggestType = this._getNameByAutoSuggestType(),
            catArr = settings.options.categories.split("|");
        
        for (var dtCat = 0; dtCat < data.length; dtCat++) {
            var acName = getNameByAutoSuggestType[catArr[dtCat].toLowerCase()],
                acValue = getValueByAutoSuggestType[catArr[dtCat].toLowerCase()],
                catData = data[dtCat][acName];
            for (var m = 0; m < catData.length; m++) {
                var t = catData[m],
                    headerTkn = catArr[dtCat] + "Tkn";
                t.controlType = catArr[dtCat];
                t.groupHeading = settings.tokens[headerTkn];
                t.isCategory = true;
                var categoryRow = { data: t, value: t[acValue], result: t[acValue] };
                rows.push(categoryRow);
            }
        }
    },

    //Function to get parsed private markets rows
    _processPrivateMarkets: function (data, settings, rows) {
        var getValueByAutoSuggestType = this._getValueByAutoSuggestType();
        var categoryTypes = settings.options["types"].split("|");
        for (var cattype = 0; cattype < categoryTypes.length; cattype++) {
            var categoryType = categoryTypes[cattype].toLowerCase();
            for (var pmCount = 0; pmCount < data.privateMarket.length; pmCount++) {
                if (data.privateMarket[pmCount]["__type"].toLowerCase().indexOf(categoryType) > -1) {
                    //populate the rows
                    var pmType = "privateMarket" + categoryType.charAt(0).toUpperCase() + categoryType.substring(1);
                    var headerTkn = pmType + "HeaderTkn";
                    var footerLinkTkn = pmType + "ViewMoreTkn";
                    var privateMarketItemsByTypeCount = data.privateMarket[pmCount][pmType].length;
                    //Begin of for pop rows
                    for (var i = 0; i < privateMarketItemsByTypeCount; i++) {
                        var pmDataObj = data.privateMarket[pmCount][pmType][i];
                        pmDataObj.controlType = pmType;
                        pmDataObj.groupHeading = settings.tokens[headerTkn];
                        pmDataObj.groupFooterLinkText = settings.tokens[footerLinkTkn];
                        pmDataObj.isCategory = true;
                        pmDataObj.isLastInCategory = false;
                        if (i === privateMarketItemsByTypeCount - 1) {
                            pmDataObj.isLastInCategory = true;
                        }
                        var categoryRow = { data: pmDataObj, value: pmDataObj[getValueByAutoSuggestType[pmType.toLowerCase()]], result: pmDataObj[getValueByAutoSuggestType[pmType.toLowerCase()]] };
                        rows.push(categoryRow);
                    }  //end of for pop rows 
                    break;
                } //end of if
            } //end of for private markets type
        } // end of for loops - different category types ( company | industry | region  )
    },

    //Function to get parsed rows
    _getParsedRows: function (autoCompletionType, data, rows, settings) {
        var getValueByAutoSuggestType = this._getValueByAutoSuggestType();
        var getNameByAutoSuggestType = this._getNameByAutoSuggestType();
        switch (autoCompletionType.toLowerCase()) {
            case "categories":
                this._processCategories(data, settings, rows);
                break;
            case "privatemarkets":
                this._processPrivateMarkets(data, settings, rows);
                break;
            default:
                var acName = getNameByAutoSuggestType[autoCompletionType.toLowerCase()];
                var acValue = getValueByAutoSuggestType[autoCompletionType.toLowerCase()];
                data = data[acName];
                for (var i = 0; i < data.length; i++) {
                    var t = data[i];
                    t.controlType = autoCompletionType;
                    rows[i] = { data: t, value: t[acValue], result: t[acValue] };
                }
        }
        return rows;
    },

    //Function to get formatted item
    _getFormattedItem: function (autoCompletionType, settings, row) {
        var getValueByAutoSuggestType = this._getValueByAutoSuggestType();
        var acValue = getValueByAutoSuggestType[autoCompletionType.toLowerCase()];
        var formattedItem;
        switch (autoCompletionType.toLowerCase()) {
            case "categories":
            case "privatemarkets":
                formattedItem = this._getFormattedCategoryRow(settings, row);
                break;
            default:
                formattedItem = this._getFormattedRow(settings, row, row[acValue]);
        }
        return formattedItem;
    },
    
    _postProcessData: function (response, term) {
        var self = this,
           settings = self.globalSettingsObj;
        // check to see if company request in in response
        var catArr = _.compact(settings.options.categories.split("|")),
            companyIdx = _.indexOf(catArr, 'company'),
            symbolIdx = _.indexOf(catArr, 'symbol'),
            data = response.category;

        // Clean up symbol data
        var index = $.inArray("symbol", catArr);
        if (symbolIdx > -1) {
            var tempSymbol = data.splice(data.length - 1, 1);
            data.splice(index, 0, tempSymbol[0]);
        }

        if (companyIdx > -1 && settings.includeCompanyScreening) {
            var companyData = _.find(data, function(val) {
                return val.company;
            });
            
            // company node is always returned.
            if (companyData) {
                companyData.company.push({
                    subControlType: "screening",
                    value: settings.tokens.screeningTextPreTkn + "<strong>" + $.trim(term) + "</strong>" + settings.tokens.screeningTextPostTkn,
                    query: $.trim(term),
                });
            } 
        }
        
        return response;
    },

    //Function to process request (makes a REST API asynchronous call)
    _processRequest: function (settings) {
        var self = this;
        //Set Default CSS Classes
        self._setCssDefaults(settings);
        $("#" + settings.controlId)._djAutocomplete(settings.url + "/" + settings.autocompletionType + "", {
            dataType: 'jsonp',
            parse: function (data, term) {
                self.globalSettingsObj = settings;
                if (data.error != undefined) {
                    if ($.isFunction(settings.onError)) {
                        settings.onError(data.error);
                    }
                    return null;
                }
                else {
                    if ($.isFunction(settings.getCount)) {
                        if (data && (data.count > -1)) {
                            settings.getCount(data.count);
                        }
                    }
                    var rows = [];
                    return self._getParsedRows(settings.autocompletionType, self._postProcessData(data, term), rows, settings);
                }
            },
            formatItem: function (row) { //  parameters are  (row, i, n )
                if (row) {
                    return self._getFormattedItem(settings.autocompletionType, settings, row);
                }
                return null;                
            },
            extraParams: self._setExtraParams(settings),
            resultsClass: settings.resultsClass,
            resultsEvenClass: settings.resultsEvenClass,
            resultsOddClass: settings.resultsOddClass,
            resultsOverClass: settings.resultsOverClass,
            viewAllClass: settings.viewAllClass,
            showViewAll: settings.showViewAll,
            showSearchText: settings.showSearchText,
            viewAllText: settings.tokens.viewAllTkn,
            viewAllTextPre: settings.tokens.viewAllTknPre,
            viewAllTextPost: settings.tokens.viewAllTknPost,
            highlight: settings.highlight,
            showHelp: settings.showHelp,
            searchBtnId: settings.searchBtnId,
            resultsContainerId:settings.resultsContainerId,
            helpLabelText: settings.tokens.helpLabelTkn,
            selectFirst: settings.selectFirst,
            inputClass:  settings.inputClass,
            eraseInputOnItemSelect: settings.eraseInputOnItemSelect,
            fillInputOnKeyUpDown: settings.fillInputOnKeyUpDown,
            autoFill: ((settings.autocompletionType.toLowerCase() === 'keyword' || settings.autocompletionType.toLowerCase() === 'calendarkeyword') && settings.autoFill) ? true : false
        });

        var suggestControl = $("#" + settings.controlId);

        //Event handlers
        //OnItemSelect EventHandler
//        $("#" + settings.controlId)._djResult(function (e, result) {
//            self.publish(self.events.itemSelect, result);
//            e.stopPropagation();
//            return false;
        //        });

        if ($.isFunction(settings.onItemSelect)) {
            $("#" + settings.controlId)._djResult(function(e, result) {
                self.globalSettingsObj.onItemSelect(result);
                e.stopPropagation();
                return false;
            });
        } else {
            $("#" + settings.controlId)._djResult(function(e, result) {
                self.publish(self.events.itemSelect, result);
                e.stopPropagation();
                return false;
            });
        }
        

        //OnFreeText EventHandler
        if ($.isFunction(settings.onFreeTextEnter)) {
            $("#" + settings.controlId)._djFreeText(function (e, result) {
                self.globalSettingsObj.onFreeTextEnter(result);
                e.stopPropagation();
                return false;
            });
        } else {
            $("#" + settings.controlId)._djFreeText(function (e, result) {
                self.publish(self.events.freeTextSelect, result);
                e.stopPropagation();
                return false;
            });
        }

        if (('searchBtnId' in settings) && (settings.searchBtnId.length > 0) && $('#' + settings.searchBtnId) && $.isFunction(settings.onFreeTextEnter)) {
            $('#' + settings.searchBtnId).on("click", function () {
                suggestControl.trigger(jQuery.Event('keydown', { which: 13, keyCode: 13 }));
            });
        }
        
        //onViewAllClick EventHandler
        if ($.isFunction(settings.onViewAllClick)) {
            $("#" + settings.controlId)._djViewAll(function(e, data) {
                if (data) {
                    data.autocompletionType = self.globalSettingsObj.autocompletionType;
                    data.options = (self.globalSettingsObj.options) ? (self.globalSettingsObj.options) : null;
                }
                self.globalSettingsObj.onViewAllClick(data);
                e.stopPropagation();
                return false;
            });
        } else {
            $("#" + settings.controlId)._djViewAll(function (e, result) {
                if (result) {
                    result.autocompletionType = self.globalSettingsObj.autocompletionType;
                    result.options = self.globalSettingsObj.options;
                }
                self.publish(self.events.viewAll, result);
                e.stopPropagation();
                return false;
            });
        }

        //OnViewMorePrivateMarketsClick EventHandler
        if ($.isFunction(settings.onViewMorePrivateMarketsClick)) {
            if (this.prototypeObj.events["viewMorePrivateMarketsEventRegistered "] === false) {
                $('a.ac_viewMore').on('click', function (e) {
                    var parentTrTag = $(this).closest("tr").get(0);
                    var parentContainerDiv = $(this).closest("div." + self.globalSettingsObj.resultsClass).get(0);
                    var data = $(parentTrTag).data("ac_cat_data");
                    suggestControl.focus();
                    self.globalSettingsObj.onViewMorePrivateMarketsClick(data);
                    $(parentContainerDiv).show();
                    e.stopPropagation();
                    return false;
                });
                self.prototypeObj.events["viewMorePrivateMarketsEventRegistered "] = true;
            }
        }
        else {
            if (this.prototypeObj.events["viewMorePrivateMarketsEventRegistered "] === false) {
                $('a.ac_viewMore').on('click', function (e) {
                    var parentTrTag = $(this).closest("tr").get(0);
                    var parentContainerDiv = $(this).closest("div." + self.globalSettingsObj.resultsClass).get(0);
                    var data = $(parentTrTag).data("ac_cat_data");
                    suggestControl.focus();
                    self.publish(self.events.viewMorePrivateMarketsClick, data);
                    $(parentContainerDiv).show();
                    e.stopPropagation();
                    return false;
                });
                self.prototypeObj.events["viewMorePrivateMarketsEventRegistered "] = true;
            }
        }

        //OnInfoClick EventHandler
        if ($.isFunction(settings.onInfoClick)) {
            if (this.prototypeObj.events["infoEventRegistered "] === false) {
                $('a.ac_info').on('click', function (e) {
                    var parentTrTag = $(this).closest("tr").get(0);
                    var parentContainerDiv = $(this).closest("div." + self.globalSettingsObj.resultsClass).get(0);
                    var data = $(parentTrTag).data("ac_data").data;
                    suggestControl.focus();
                    self.globalSettingsObj.onInfoClick(data);
                    $(parentContainerDiv).show();
                    e.stopPropagation();
                    return false;
                });
                self.prototypeObj.events["infoEventRegistered "] = true;
            }
        }
        else {
            if (this.prototypeObj.events["infoEventRegistered "] === false) {
                $('a.ac_info').on('click', function (e) {
                    var parentTrTag = $(this).closest("tr").get(0);
                    var parentContainerDiv = $(this).closest("div." + self.globalSettingsObj.resultsClass).get(0);
                    var data = $(parentTrTag).data("ac_data").data;
                    suggestControl.focus();
                    self.publish(self.events.infoClick, data);
                    $(parentContainerDiv).show();
                    e.stopPropagation();
                    return false;
                });
                self.prototypeObj.events["infoEventRegistered "] = true;
            }
        }

        //OnPromoteClick EventHandler
        if ($.isFunction(settings.onPromoteClick)) {
            if (this.prototypeObj.events["promoteEventRegistered "] === false) {
                $('a.ac_promote').on('click', function(e) {
                    var parentTrTag = $(this).closest("tr").get(0);
                    var parentContainerDiv = $(this).closest("div." + self.globalSettingsObj.resultsClass).get(0);
                    var data = $(parentTrTag).data("ac_data").data;
                    suggestControl.focus();
                    self.globalSettingsObj.onPromoteClick(data);
                    $(parentContainerDiv).show();
                    e.stopPropagation();
                    return false;
                });
                self.prototypeObj.events["promoteEventRegistered "] = true;
            }
        } else {
            if (this.prototypeObj.events["promoteEventRegistered "] === false) {
                $('a.ac_promote').on('click', function (e) {
                    var parentTrTag = $(this).closest("tr").get(0);
                    var parentContainerDiv = $(this).closest("div." + self.globalSettingsObj.resultsClass).get(0);
                    var data = $(parentTrTag).data("ac_data").data;
                    suggestControl.focus();
                    self.globalSettingsObj.onPromoteClick(data);
                    $(parentContainerDiv).show();
                    e.stopPropagation();
                    return false;
                });
                self.prototypeObj.events["promoteEventRegistered "] = true;
            }
        }

        //OnNotClick EventHandler
        if ($.isFunction(settings.onNotClick)) {
            if (this.prototypeObj.events["notEventRegistered "] === false) {
                $('a.ac_not').on('click', function (e) {
                    var parentTrTag = $(this).closest("tr").get(0);
                    var parentContainerDiv = $(this).closest("div." + self.globalSettingsObj.resultsClass).get(0);
                    var data = $(parentTrTag).data("ac_data").data;
                    suggestControl.focus();
                    self.globalSettingsObj.onNotClick(data);
                    $(parentContainerDiv).show();
                    e.stopPropagation();
                    return false;
                });
                self.prototypeObj.events["notEventRegistered "] = true;
            }
        }

        //OnDiscontClick EventHandler
        if ($.isFunction(settings.onDiscontClick)) {
            if (this.prototypeObj.events["discontEventRegistered "] === false) {
                $('a.ac_discont').on('click', function (e) {
                    var parentTrTag = $(this).closest("tr").get(0);
                    var parentContainerDiv = $(this).closest("div." + self.globalSettingsObj.resultsClass).get(0);
                    var data = $(parentTrTag).data("ac_data").data;
                    suggestControl.focus();
                    self.globalSettingsObj.onDiscontClick(data);
                    $(parentContainerDiv).show();
                    e.stopPropagation();
                    return false;
                });
                self.prototypeObj.events["discontEventRegistered "] = true;
            }
        }
    },

    _initSpeechInput: function (settings, targetObj) {
        //Enable/Disable Speech Input | Default: disabled
        if (settings.speechInput && settings.onSpeechChange) {
            $(targetObj).attr("x-webkit-speech", "x-webkit-speech");
            $(targetObj).bind("webkitspeechchange", function () {
                settings.onSpeechChange(this.value);
            });
        }
    },

    //Initialize Autocomplete
    _initAutoComplete: function (jsonObj) {
        if (jsonObj) {
            // We can use jQuery 1.4.2+ here
            var autosuggestPrototype = this.prototypeObj;
            var suggestContext;
            if (jsonObj.authType.toLowerCase() === "suggestcontext") {
                suggestContext = jsonObj.authTypeValue;
            }
            if (suggestContext === undefined || $.trim(suggestContext).length === 0) {
                if (autosuggestPrototype.suggestContextObj.callInitiated != true) {
                    this._getSuggestContextAndProcessRequest(jsonObj);
                }
                else if (autosuggestPrototype.suggestContextObj.authToken != null) {
                    jsonObj.authTypeValue = this._URLDecode(autosuggestPrototype.suggestContextObj.authToken);
                    this._processRequest.call(this, jsonObj);
                }
            }
            else {
                jsonObj.authTypeValue = this._URLDecode(suggestContext);
                this._processRequest.call(this, jsonObj);
            }
        }
    }
});

// Declare this class as a jQuery plugin
$.plugin('dj_AutoSuggest', DJ.UI.AutoSuggest);
$dj.debug('Registered DJ.UI.AutoSuggest (extends DJ.UI.Component)');