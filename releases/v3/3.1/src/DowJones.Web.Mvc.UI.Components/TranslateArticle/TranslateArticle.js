/*!
* TranslateArticleControl
*/
    DJ.UI.TranslateArticle = DJ.UI.Component.extend({
        // Default options
        defaults: {
            debug: false,
            cssClass: 'TranslateArticle'
            // ,name: value     // add more defaults here separated by comma
        },

        // Localization/Templating tokens
        tokens: {
        //name: value add more defaults here separated by comma
    },


    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "TranslateArticle" }, meta);
        this._super(element, $meta);
        this.container = element;
        this.$container = $(element);

        this.loadStar();
        this.initialize();
    },

    loadStar: function () {
        $("#starify").children().not(":input").hide();
        // Create stars from :radio boxes
        $("#starify").stars({
            cancelShow: false,
            showTitles: true
        });
        $("#dj_translate_ratestatus").css("display", "inline");
    },

    initialize: function () {
        var me = this;
        $("div.ui-stars-star").each(function () {
            $(this).find("a").text('');
        });

        $("a.dj_translate_disclaimer_link").click(function () {
            $("#translateDisclaimerFull").show();
            $("#translateDisclaimer").hide();
        });

        $("a.dj_translate_TranslateMore").click(function () {
            me.TranslateMore();
        });

        $("a.dj_translate_TranslateAll").click(function () {
            me.TranslateAll();
        });

        $(".ui-stars-star").click(function () {
            me.UpdateRatings();
        });

        me.TranslateMore();
    },

    TranslateMore: function () {
        translateOption = "more";
        var divToBeTranslated = $("#originalContent div:first-child");
        var textToBeTranslated = "";
        if (divToBeTranslated) {
            textToBeTranslated = divToBeTranslated.html();
            /* If there are any img or a tags in the original content
            Save their original src and href into a collection and replace the translated version
            with originals here after coming back from translate service */
            originalImgSrcCol = [];
            originalAHrefCol = [];
            this.SaveOriginalLinks(divToBeTranslated, originalImgSrcCol, originalAHrefCol);
            $("#translateButtons").css("display", "none");
            $("#translatingMessage").css("display", "block");

            this.Translate(textToBeTranslated);
        }
    },

    SaveOriginalLinks: function (containerDiv, imageLinks, anchorLinks) {
        var imgChilds = containerDiv.find("img");
        if (imgChilds.length > 0) {
            for (var i = 0; i < imgChilds.length; i++) {
                imageLinks.push($(imgChilds[i]).attr("src"));
            }
        }
        var aChilds = containerDiv.find("a");
        if (aChilds.length > 0) {
            for (var j = 0; j < aChilds.length; j++) {
                imageLinks.push($(aChilds[j]).attr("href"));
            }
        }
    },

    UpdateRatings: function () {
        this.ANo = $("#AccessionNo").text();
        this.WordCount = $("#WordCount").text();
        this.SourceName = $("#SourceName").text();
        this.Rating = $("input[name=vote]").val();
        this._productPrefix = $("#PP").text();

        var reqDelegate = {
            "SourceName": this.SourceName,
            "SourceLanguage": this.options.SourceLanguage,
            "TargetLanguage": this.options.TargetLanguage,
            "WordCount": this.WordCount,
            "AccessionNumber": this.ANo,
            "Rating": this.Rating
        }

        var credentials = {
            "sessionId": this.options.SessionID,
            "accessPointCode": this.options.AccessPointCode,
            "interfaceLanguage": this.options.InterfaceLanguage,
            "productPrefix": this.options.ProductPrefix,
            "pageCacheKey": this.options.PageCacheKey
        }

        var requestDelegate = {
            'requestDelegate': reqDelegate,
            'credentials': credentials
        };

        $.ajax({
            type: "POST",
            url: this.options.TranslationServiceUrl + "/UpdateRatings",
            data: JSON.stringify(requestDelegate),
            dataType: "json",
            context: $('#TranslateArticle'),
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                $("#dj_translate_ratestatus").text("Rated:");
            },
            error: function (msg) {

            }
        });
    },

    Translate: function (sourceText) {
        //initialize request variables
        this.inputFormat = $("#InputFormat").text();
        this._productPrefix = $("#PP").text();
        var reqDelegate = {
            "SourceText": sourceText,
            "SourceLanguage": this.options.SourceLanguage,
            "TargetLanguage": this.options.TargetLanguage,
            "InputFormat": this.inputFormat
        };
        var credentials = {
            "sessionId": this.options.SessionID,
            "accessPointCode": this.options.AccessPointCode,
            "interfaceLanguage": this.options.InterfaceLanguage,
            "productPrefix": this.options.ProductPrefix,
            "pageCacheKey": this.options.PageCacheKey
        }
        var requestDelegate = {
            'requestDelegate': reqDelegate,
            'credentials': credentials
        };

        $.ajax({
            type: "POST",
            url: this.options.TranslationServiceUrl + "/TranslateArticle",
            data: JSON.stringify(requestDelegate),
            dataType: "json",
            context: $('#TranslateArticle'),
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                var translatedContentDiv = $("#translatedContent")[0];

                $("#originalContent").children().each(function (i) {
                    if (i == 0 && translateOption == "more") {
                        $(this).remove();
                        return;
                    }
                    else if (translateOption == "all")
                        $(this).remove();
                });

                /* put the response in a container div so we can use jQuery to replace
                src and href links with their originals */
                var translatedContentObj = $("<div>" + msg.d.TranslatedText + "</div>");
                if (originalImgSrcCol.length > 0) {
                    var translatedImgs = translatedContentObj.find("img");
                    for (var i = 0; i < originalImgSrcCol.length; i++) {
                        $(translatedImgs[i]).attr("src", originalImgSrcCol[i]);
                    }
                }
                if (originalAHrefCol.length > 0) {
                    var translatedAnchors = translatedContentObj.find("a");
                    for (var j = 0; j < originalAHrefCol.length; j++) {
                        $(translatedAnchors[j]).attr("href", originalAHrefCol[j]);
                    }
                }
                translatedContentDiv.innerHTML += translatedContentObj.html();

                $("#translateButtons").show();
                if ($("#originalContent").children().length == 0)
                    $("#translateButtons").hide();
                $("#translatingMessage").hide();
                $("#translatingMessageLargeArticle").hide();
            },
            error: function (msg) {
                var translatedContentDiv = $("#translatedContent")[0];
                translatedContentDiv.innerHTML += "Unable to process:-" + msg.d.StatusMessage;
                var originalContentDiv = $("#originalContent")[0];
                $("#translateButtons").show();
                if ($("#originalContent").children().length == 0)
                    $("#translateButtons").hide();
                $("#translatingMessage").hide();
            }
        });
    },

    TranslateAll: function () {
        translateOption = "all";
        var me = this;
        var textToBeTranslated = "";
        if ($("#originalContent").children().length > 0) {
            originalImgSrcCol = [];
            originalAHrefCol = [];
            $("#originalContent").children().each(function () {
                textToBeTranslated += $(this).html();
                me.SaveOriginalLinks($(this), originalImgSrcCol, originalAHrefCol);
            });

            $("#translateButtons").hide();
            $("#translatingMessage").show();
            $("#translatingMessageLargeArticle").show();
            this.Translate(textToBeTranslated);
        }
    },

    EOF: null

});

// Declare this class as a jQuery plugin
$.plugin('dj_TranslateArticle', DJ.UI.TranslateArticle);
