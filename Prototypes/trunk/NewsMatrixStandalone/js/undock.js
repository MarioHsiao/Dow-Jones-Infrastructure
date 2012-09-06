var widgetCompanies = "radar90Test_companies";
var widgetSubjects = "radar90Test_subjects";
window.defaultCompanies = window.defaultCompanies || "AMD|ARMCOS|CGE|GMACUS|AMXLE|AMKEL|ARASRV|CMPCRD|BSCHL|BELO|BBDR|BOYDG|BRWICK|PRMUS|CHSPKE|ELLMED|CONPOW|CHSIHO|COOPIN|CSCSIN|SUZFOD|DELCP|DDSA|ECHOSP|FDATC|FRDMO|FOILC|FCALEI|CIUT|GANINC|RLNRSC|COLHSP|HMANAS|MARIOT|INTLST|ILFC|PIELE|APMT|HOVN|KAUFBH|ENRONL|LENC|LVLTCM|STRLV|LBMED|LTDI|LOUPAC|MBIAPK|MRTRAU|MGIC|MGMG|NORFOR|NOVCHE|NRGENG|OLINC|OWENIL|TNCPKG|PRKDRL|PHH|PARPAR|GEONCO|DONNRR|CMACF|TANDIE|RLGYCP|RSDNCC|RTEAID|RCCL|SAGRHO|PFFTT|SNMINA|SEAGT|WRGRCE|SROEAC|SMFODS|AMEGFC|UNITEL|SPF|SUNGDS|SUNOL|SVU|NME|TSPTC|AESCOR|GDYRR|HERTZ|HRTZGP|MCCLAT|NMANM|NYT|TOYRUS|TRWA|BURCP|URENT|USXMAR|UHSIB|BATCH|WHYPCK|VALORT";
window.defaultSubjects = window.defaultSubjects || "CACTIO|C16|C02|C411|GFINC|C12|MCPDBT|CRECAL";

$(function () {
    window.moveTo(100, 100);

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
                    $('#newsMatrixContainer').fadeIn();
                });
            });
        }
    });
});