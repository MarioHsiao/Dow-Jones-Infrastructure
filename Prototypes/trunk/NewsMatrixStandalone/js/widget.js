var top, left;
var widgetHeightCookieName = "radar90Test_Widget_Height";
var widgetLeftCookieName = "radar90Test_Widget_Left";
var widgetTopCookieName = "radar90Test_Widget_Top";
var widgetCompanies = "radar90Test_companies";
var widgetSubjects = "radar90Test_subjects";
var loadingSelector = "#loading";
var radarSelector = "#newsMatrixContainer";
var noCompaniesSelector = "#noCompanies";
var autoSuggestInputSelector = "#autoSuggestSearchInput";
var resizeWidth = 450;
var cOptions = { expires: 365, path: "/" };

window.defaultHeight = window.defaultHeight || 700;
window.defaultCompanies = window.defaultCompanies || "AMD|ARMCOS|CGE|GMACUS|AMXLE|AMKEL|ARASRV|CMPCRD|BSCHL|BELO|BBDR|BOYDG|BRWICK|PRMUS|CHSPKE|ELLMED|CONPOW|CHSIHO|COOPIN|CSCSIN|SUZFOD|DELCP|DDSA|ECHOSP|FDATC|FRDMO|FOILC|FCALEI|CIUT|GANINC|RLNRSC|COLHSP|HMANAS|MARIOT|INTLST|ILFC|PIELE|APMT|HOVN|KAUFBH|ENRONL|LENC|LVLTCM|STRLV|LBMED|LTDI|LOUPAC|MBIAPK|MRTRAU|MGIC|MGMG|NORFOR|NOVCHE|NRGENG|OLINC|OWENIL|TNCPKG|PRKDRL|PHH|PARPAR|GEONCO|DONNRR|CMACF|TANDIE|RLGYCP|RSDNCC|RTEAID|RCCL|SAGRHO|PFFTT|SNMINA|SEAGT|WRGRCE|SROEAC|SMFODS|AMEGFC|UNITEL|SPF|SUNGDS|SUNOL|SVU|NME|TSPTC|AESCOR|GDYRR|HERTZ|HRTZGP|MCCLAT|NMANM|NYT|TOYRUS|TRWA|BURCP|URENT|USXMAR|UHSIB|BATCH|WHYPCK|VALORT";
window.defaultSubjects = window.defaultSubjects || "CACTIO|C16|C02|C411|GFINC|C12|MCPDBT|CRECAL";

$(function () {
    var height = window.parseInt($.cookie(widgetHeightCookieName) || defaultHeight);
    left = window.parseInt($.cookie(widgetLeftCookieName) || 50);
    top = window.parseInt($.cookie(widgetTopCookieName) || 50);
    window.resizeTo(resizeWidth, height);
    window.moveTo(left, top);
    var companySuggest = {
        url: 'http://suggest.factiva.com/Search/1.0',
        controlId: 'autoSuggestSearchInput',
        autocompletionType: "Company",
        useEncryptedKey: "S001WF92XV72cbbMXmsNXmnMpMvNTAsOTMm5DByMa3G2DJqMsFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUEA",
        selectFirst: true,
        onItemSelect: function (data) {
            var tCompanies = $.cookie(widgetCompanies) || defaultCompanies;
            var temp = tCompanies.split("|");
            temp.push(data.code);
            temp = _.uniq(temp).join("|");

            if (tCompanies != temp) {
                $.cookie(widgetCompanies, $.trim(temp));
                window.itemToAdd = {
                    CompanyName: data.value,
                    InstrumentReference: {
                        FCode: data.code
                    }
                };
            }
            else {
                alert("Company already added");
            }
        }
    }
    djV3.web.widgets.autocomplete(companySuggest);

    renderNewsMatrix();

    monitorWindowPosition();
    $.cookie(widgetCompanies, $.trim($.cookie(widgetCompanies)) || defaultCompanies, cOptions);

    $("#customize").on("click", function () {
        $('#viewPage').fadeOut(function () {
            $('#editPage').show();
        });
        
    });

    $('#companyList').on('click', '.djContentLink', function () {
        var querystring = 'fds:' + $(this).data('fcode');

        if ($(this).data('SubjectCode')) {
            querystring += " AND ns:" + $(this).data('subjectcode');
        }

        var url = "headlines.html?querystring=" + escape(querystring);
        var windowName = "newsMatrixHeadlines";
        window.open(url, windowName, "scrollbars=yes,height=500,width=500").focus();
    });

    $('#companyList').on('click', '.djRemoveContentLink', function () {
        var tCompanies = $.cookie(widgetCompanies) || defaultCompanies;
        var temp = tCompanies.split("|");
        var $target = $(this);
        var fcode = $target.data("fcode");
        temp = _.without(temp, fcode).join("|");
        $.cookie(widgetCompanies, $.trim(temp), cOptions);
        $target.parent('li').hide('slow', function () { $target.remove() });
    });

    $('#djSave').click(function () {
        renderNewsMatrix();
    });


    $('#djAdd').click(function () {
        if (!itemToAdd) {
            return;
        }

        var company = ['<li><a href="#" class="djRemoveContentLink" title="Click to remove ',
                        itemToAdd.CompanyName,
                        '" data-fcode="',
                        itemToAdd.InstrumentReference.FCode,
                        '"><i class="icon-remove"></i></a>',
                        '<a href="#" class="djContentLink" title="',
                        itemToAdd.CompanyName,
                        '" data-fcode="',
                        itemToAdd.InstrumentReference.FCode,
                        '" data-subjectCode="',
                        (itemToAdd.Radar && itemToAdd.Radar.SubjectCode) || '',
                        '">',
                        itemToAdd.CompanyName,
                        '</a></li>'].join('');

        $(company).prependTo($('#companyList')).slideDown('slow', function () {
            $(autoSuggestInputSelector).val("");
        });
    });

    $('#undock').click(function () {
        window.open('undock.html', 'newsMatrixUndock', 'scrollbars=yes,height=600,width=199,location=0,resizable=0');
        return false;
    });

    // attach event to clear
    $("#confirmClear").on("click", function () {
        clearCompanies();
    });
});

function renderNewsMatrix() {
    $(loadingSelector).show();
    $('#editPage').hide();

    $.jsonp({
        url: 'http://api.dowjones.com/api/1.0/NewsRadar/Ex/json?callback=?',
        callback: "callback",
        data: {
            entityid: $.trim($.cookie(widgetCompanies)) || defaultCompanies,
            subjectid: $.cookie(widgetSubjects) || defaultSubjects,
            symbology: "fii",
            encryptedtoken: "S001WF92XV72cbbMXmsNXmnMpMvNTAsOTMm5DByMa3G2DJqMsFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUEA"
        },
        success: function (data) {
            DJ.subscribe("dataTransformed.dj.NewsMatrix", function (data) {
                createCompanyEditList(data.MatrixItems);
            });

            DJ.add("NewsMatrix", {
                container: "newsMatrixContainer",
                options: {
                    displayTicker: false,
                    hitcolor: "999",
                    hitsize: "8",
                    windowSize: 6,
                    scrollSize: 5
                },
                eventHandlers: {
                    "matrixItemClicked.dj.NewsMatrix": function (data) {
                        var querystring = 'fds:' + data.InstrumentReference.FCode;

                        if (data.NewsEntity.Radar && data.NewsEntity.Radar.SubjectCode && data.NewsEntity.Radar.SubjectCode != 'ALLNEWS') {
                            querystring += " AND ns:" + data.NewsEntity.Radar.SubjectCode;
                        }

                        var url = "headlines.html?querystring=" + escape(querystring);
                        var windowName = "radar90Headlines";
                        window.open(url, windowName, "scrollbars=yes,height=500,width=500").focus();
                    }
                },
                data: data.ParentNewsEntities
            }).done(function () {
                $('#loading').fadeOut('fast', function () {
                    $('#viewPage').hide().removeClass('notActive').fadeIn();
                });
            });
        }
    });
}

function createCompanyEditList(items) {
    var companyListItems = [];

    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        var company = ['<li><a href="#" class="djRemoveContentLink" title="Click to remove ',
                        item.CompanyName,
                        '" data-fcode="',
                        item.InstrumentReference.FCode,
                        '"><i class="icon-remove"></i></a>',
                        '<a href="#" class="djContentLink" title="',
                        item.CompanyName,
                        '" data-fcode="',
                        item.InstrumentReference.FCode,
                        '" data-subjectCode="',
                        (item.Radar && item.Radar.SubjectCode) || '',
                        '">',
                        item.CompanyName,
                        '</a></li>'].join('');

        companyListItems.push(company);
    }

    $('#companyList').html(companyListItems.join(''));
}

function monitorWindowPosition() {
    if (screenLeft() !== left || screenTop() !== top) {
        $.cookie(widgetLeftCookieName, left = screenLeft(), cOptions);
        $.cookie(widgetTopCookieName, top = screenTop(), cOptions);
    }
    window.setTimeout("monitorWindowPosition()", 1000);
}

function screenLeft() {
    return window.screenX || window.screenLeft;
}

function screenTop() {
    return window.screenY || window.screenTop;
}

$(window).resize(function () {
    $.cookie(widgetHeightCookieName, window.outerHeight, cOptions);
});


function errorHandler() {
    $(loadingSelector).addClass("notActive");
    $(noCompaniesSelector).addClass("notActive");
    $(radarSelector).removeClass("notActive");
    $(".djWidgetFooter").hide();
    $(".djCategories").hide();
    $("#clear").hide();
    $(".entry").hide();
}

function clearCompanies() {
    $(noCompaniesSelector).removeClass("notActive");
    $(loadingSelector).addClass("notActive");
    $(radarSelector).addClass("notActive");
    $.cookie(widgetCompanies, " ", cOptions);
    $('#clearModal').modal('hide');
}

function addCompanyHandler(event, data) {
    var tCompanies = $.cookie(widgetCompanies) || defaultCompanies;
    var temp = tCompanies.split("|");
    temp.push(data.data.Result.fCode);
    temp = _.uniq(temp).join("|");

    var r = DJ.Widgets.items["radar_90"];

    if (r.settings.entityid != temp) {
        r.set({
            "entityid": $.trim(temp)
        });

        $(radarSelector).addClass("notActive");
        $(noCompaniesSelector).addClass("notActive");
        $(loadingSelector).removeClass("notActive");
        $.cookie(widgetCompanies, $.trim(temp), cOptions);
    }
    $(autoSuggestInputSelector).val("");
}