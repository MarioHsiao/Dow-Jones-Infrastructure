/**
*  Factiva Newsletter
*
* @category   Core
* @author     Ron Edgecomb II <ronald.edgecombii@dowjones.com>
* @copyright  2012 Dow Jones & Company Inc.
*/

if (!window['DJGlobal']) {
    window["DJGlobal"] = {};
}

window["isMobile"] = false;
(function (a, b) { if (/android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|e\-|e\/|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(\-|2|g)|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) window["isMobile"] = true; })(navigator.userAgent || navigator.vendor || window.opera);
var ua = navigator.userAgent;
window["devices"] = {
    iPhone: ua.match(/(iPhone)/i),
    iPad: ua.match(/(iPad)/i),
    iPod: ua.match(/(iPod)/i),
    blackberry: ua.match(/BlackBerry/i),
    android: ua.match(/Android/i),
    isMobile: window.isMobile
};

var DJGlobal = window["DJGlobal"];
DJGlobal.windowObjs = {};
DJGlobal.devices = window.devices;
DJGlobal.windowDefaultOptions = { form: null, url: null, features: null, scrollbar: true, toolbar: false, location: true, statusbar: true, menubar: false, resizable: true, width: -1, height: -1, left: -1, top: -1, windowName: null, returnWindowObj: false };
DJGlobal.CloseWindow = function (windowName) {
    if (windowName && DJGlobal.windowObjs[windowName]) {
        try {
            if (DJGlobal.windowObjs[windowName].close)
                DJGlobal.windowObjs[windowName].close();
            DJGlobal.windowObjs[windowName] = null;
        } catch (e) {
            DJGlobal.windowObjs[windowName] = null;
        }
    }
};

DJGlobal.ResizeAndPositionWin = function (win, w, h, l, t, url) {
    setTimeout(function () {
        try {
            try {
                win.resizeTo(w, h);
                win.moveTo(l, t);
                win.focus();
            } catch (e) {
            }
            if (url) {
                win.location.href = url;
            }
        } catch (e) {
        }
    }, 250);
};

DJGlobal.NewWindow = function (options) {
    if (!options) {
        return false;
    }

    if (typeof (options) == 'string') {
        options = { url: options };
    }

    options = jQuery.extend({}, DJGlobal.windowDefaultOptions, options);
    var w = -1, h = -1, l = -1, t = -1;
    var f = options.features;
    if (!f) {
        f = "";
        f += "toolbar=" + ((options.toolbar) ? "yes" : "no");
        f += ",scrollbars=" + ((options.scrollbar) ? "yes" : "no");
        f += ",location=" + ((options.location) ? "1" : "0");
        f += ",statusbar=" + ((options.statusbar) ? "1" : "0");
        f += ",menubar=" + ((options.menubar) ? "1" : "0");
        f += ",resizable=" + ((options.resizable) ? "1" : "0");
        w = options.width;
        h = options.height;
        l = options.left;
        t = options.top;
    } else {
        try {
            $.each($.trim(f).split(","), function (i, v) {
                v = $.trim(v).split("=");
                if (v[0].toLowerCase() == "width" && !isNaN(v[1]))
                    w = parseInt(v[1]);
                else if (v[0].toLowerCase() == "height" && !isNaN(v[1]))
                    h = parseInt(v[1]);
                else if (v[0].toLowerCase() == "left" && !isNaN(v[1]))
                    l = parseInt(v[1]);
                else if (v[0].toLowerCase() == "top" && !isNaN(v[1]))
                    t = parseInt(v[1]);
            });
        } catch (e) {
        }
    }

    if (w < 0) {
        w = screen.width - 200;
        f += "width=" + w;
    }

    if (h < 0) {
        h = screen.height - 100;
        f += "height=" + h;
    }

    if (l < 0) {
        l = (screen.width - w) / 2;
        f += "left=" + l;
    }

    if (t < 0) {
        t = (screen.height - h) / 2;
        f += "top=" + t;
    }

    var win;

    if (options.url) {
        DJGlobal.CloseWindow(options.windowName);
        try {
            if (Modernizr.touch || DJGlobal.devices.isMobile) {
                win = window.open(options.url, "_blank");
                if (options.windowName) {
                    DJGlobal.windowObjs[options.windowName] = win;
                }
            } else {
                win = window.open("about:blank", options.windowName || "_blank", f);
                // start workaround for a window focus problem with Google’s Chrome browser.
                if (navigator.userAgent.indexOf('Chrome/') > 0) {
                    if (win) {
                        win.close();
                        win = window.open("about:blank", options.windowName || "_blank", f);
                    }
                }
                // end workaround
                DJGlobal.ResizeAndPositionWin(win, w, h, l, t, options.url);
                if (options.windowName) {
                    DJGlobal.windowObjs[options.windowName] = win;
                }
            }
        } catch (e) {
        }
    } else if (options.form) {
        var windowName = options.windowName || (options.form.name || "window" + Math.floor(Math.random() * 10000));
        DJGlobal.CloseWindow(windowName);
        try {
            win = window.open("", windowName, f);
            DJGlobal.ResizeAndPositionWin(win, w, h, l, t);
            DJGlobal.windowObjs[windowName] = win;
            setTimeout(function () {
                var oldTarget = options.form.target;
                options.form.target = windowName;
                options.form.submit();
                options.form.target = oldTarget;
            }, 400);
        } catch (e) {
        }
    } else {
        return false;
    }
    if (options.returnWindowObj)
        return win;
    return false;
};

DJGlobal.positionWindow = function (win) {
};

if (!window["djHeadlineHandler"]) {
    window["djHeadlineHandler"] = {
        init: function () {
            // DJ.subscribe("postprocessing.dj.article", this.postProcessingHandler);
            DJ.subscribe("eLinkClick.dj.Article", this.eLinkClickHandler);
            DJ.subscribe("headlineLinkClick.dj.article", this.articleClickHandler);
            DJ.subscribe("headlineClick.dj.PortalHeadlineList", this.headlineHandler);
            DJ.subscribe("headlineEntryClick.dj.PortalHeadlineList", this.headlineHandler);
            //this.addEvents();
        },
        eLinkClickHandler: function (data) {
            DJGlobal.NewWindow({ url: data.href, windowName: "webArticleWin" });
        },
        postProcessingHandler: function (data) {
            alert(data.type.toUpperCase() + " functionality is not defined yet.");
        },

        articleClickHandler: function (data) {
            DJGlobal.NewWindow({ url: data.externalUri, windowName: "webArticleWin" });
            //window.open(data.externalUri, "_self");
        },

        addEvents: function () {
            $('#editionContent LI').on("click", function (e, data) {
                DJ.publish("newsletter.dj.headlineClick", { headline: $(this).closest('li').data("headline") });
            });
        },

        fireExternalUrl: function (tUrl) {
            if (DJGlobal.devices.isMobile || DJGlobal.devices.iPad) {
                //window.open(tUrl, "_blank");
            }
                /*else if (navigator.userAgent.indexOf('Chrome/') > 0) {
                window.open(tUrl, "_blank");
                }*/
            else {
                DJGlobal.NewWindow({ url: tUrl, windowName: "webArticleWin" });
            }
        },

        addIcons: function () {
            var pHeadlines = $('.dj_headlineListContainer');
            $.each(pHeadlines, function () {
                var edContent = $(this).findComponent(DJ.UI.PortalHeadlineList);
                if (edContent) {
                    var entries = $(".dj_entry", edContent.$element);
                    _.each(entries, function (headline, i) {
                        var title = $("h4", headline);
                        if (title) {
                            if ($(headline).hasClass("article")) {
                                $(title).prepend("<span class=\"media-icon article\"></span>");
                            }

                            if ($(headline).hasClass("blog")) {
                                $(title).prepend("<span class=\"media-icon blog\"></span>");
                            }

                            if ($(headline).hasClass("webpage")) {
                                $(title).prepend("<span class=\"media-icon webpage\"></span>");
                            }

                            if ($(headline).hasClass("audio")) {
                                $(title).prepend("<span class=\"media-icon audio\"></span>");
                            }

                            if ($(headline).hasClass("video")) {
                                $(title).prepend("<span class=\"media-icon video\"></span>");
                            }

                            if ($(headline).hasClass("link")) {
                                $(title).prepend("<span class=\"media-icon link\"></span>");
                            }

                            if ($(headline).hasClass("pdf")) {
                                $(title).prepend("<span class=\"media-icon pdf\"></span>");
                            }

                            if ($(headline).hasClass("html")) {
                                $(title).prepend("<span class=\"media-icon html\"></span>");
                            }
                        }
                    });
                }
            });

        },

        headlineHandler: function (data) {
            var headline = data.headline;
            switch (headline.contentCategoryDescriptor) {
                case "customerdoc":
                case "summary":
                    break;
                case "external":
                    if (headline.headlineUrl) {
                        window.open(headline.headlineUrl, "_self");
                        //DJGlobal.NewWindow({ url: headline.headlineUrl, windowName: "webArticleWin" });
                    }
                    break;
                case "link":
                    if (headline.headlineUrl) {
                        window.open(headline.headlineUrl, "_self");
                        //DJGlobal.NewWindow({ url: headline.headlineUrl, windowName: "webArticleWin" });
                    }
                    else if (headline.reference.ref) {
                        window.open(headline.reference.ref, "_self");
                        //DJGlobal.NewWindow({ url: headline.reference.ref, windowName: "webArticleWin" });
                    }
                    break;
                case "blog":
                case "website":
                case "webpage":
                    if (headline.headlineUrl) {
                        window.open(headline.headlineUrl, "_self");
                        //DJGlobal.NewWindow({ url: headline.headlineUrl, windowName: "webArticleWin" });
                    }
                    else if (headline.reference.ref) {
                        window.open(headline.reference.ref, "_self");
                        //DJGlobal.NewWindow({ url: headline.reference.ref, windowName: "webArticleWin" });
                    }
                    break;
                case "multimedia":
                    {
                        //var an = headline.reference.guid, title = headline.title, container = data.mediaContainer, thumbNail = headline.thumbnailImage ? headline.thumbnailImage.uri : "";
                        //DJGlobal.getMultimediaVideos(an, title, container, thumbNail);
                        window.open(headline.headlineUrl, "_self");
                        break;
                    }
                default:
                    window.open(headline.headlineUrl, "_self");
                    break;
            }
        },
        eof: false
    };
}

