
if (typeof window.DJ == 'undefined' || !window.DJ) {
    window.DJ = {};
}
(function () {
    var v = false;
    if (typeof jQuery != "undefined") {
        v = jQuery().jquery.split(".");
        v = v[0] == "1" && (v[1] === "4" || v[1] === "5")
    }
    DJ.$ = jQuery;
})();
(function () {
    function v(a, c, b) {
        var d = [], g, e;
        for (e in w) {
            g = w[e];
            if (g.eventKey && g.eventKey == a || !g.eventKey)
                if (g.widgetId && g.widgetId == c || !g.widgetId)
                    if (g.widgetType && g.widgetType == b || !g.widgetType)
                        d.push(g)
        }
        return d
    }
    function P(a) {
        K += 1;
        return a[D] = K
    }
    function G(a) {
        function c() {
            if (e)
                throw "The event has already been attached.";
        }
        if (!(this instanceof G))
            return new G(a);
        var b = x, d = x, g = x, e = x;
        a[D] || P(a);
        this.on = function (i) {
            c();
            b = i;
            return this
        };
        this.forWidget = function (i) {
            c();
            d = i;
            return this
        };
        this.forType = function (i) {
            c();
            g = i;
            return this
        };
        this.attach = function () {
            c();
            var i = [b, d, g, a[D]].join("::");
            w[i] = { eventKey: b, widgetId: d, widgetType: g, func: a };
            e = z;
            return this
        };
        this.detach = function () {
            var i = [b, d, g, a[D]].join("::");
            if (w[i]) {
                try {
                    delete w[i]
                } catch (f) {
                }
                e = x
            }
            return this
        };
        this.detachAll = function () {
            for (var i = v(b, d, g) ; i.length > 0;) {
                var f = i.shift();
                f = [f.eventKey, f.widgetId, f.widgetType, f.func[D]].join("::");
                if (w[f])
                    try {
                        delete w[f]
                    } catch (h) {
                    }
            }
            return this
        }
    }
    var m, t, z = true, x = false;
    m = window.DJ;
    t = m.$;
    m.Util = {
        extend: function (a, c, b) {
            for (var d in c)
                if (b !==
                z || b === z && typeof a[d] === "undefined")
                    a[d] = c[d];
            return a
        }, arrayRemove: function (a, c, b) {
            b = a.slice((b || c) + 1 || a.length);
            a.length = c < 0 ? a.length + c : c;
            return a.push.apply(a, b)
        }, sortAlphabetic: function (a) {
            a.sort(function (c, b) {
                var d = c.Name.toLowerCase(), g = b.Name.toLowerCase();
                if (d < g)
                    return -1;
                if (d > g)
                    return 1;
                return 0
            })
        }, populate: function (a, c) {
            for (var b in c)
                if (typeof a[b] !== "undefined")
                    a[b] = c[b];
            return a
        }, unique: function (a) {
            var c = [], b = {}, d;
            for (d = 0; d < a.length; d++)
                if (b[a[d]] !== z) {
                    b[a[d]] = z;
                    c.push(a[d])
                }
            return c
        },
        isMouseLeaveOrEnter: function (a, c) {
            for (var b = a.relatedTarget ? a.relatedTarget : a.type == "mouseout" ? a.toElement : a.fromElement; b && b != c;)
                b = b.parentNode;
            return b != c
        }, truncate: function (a, c) {
            if (a.length > c) {
                a = a.substring(0, c);
                for (a = a.replace(/\w+$/, "") ; a.match(/[,\-\s]+$/) ;) {
                    a = a.replace(/\s+$/, "");
                    a = a.replace(/[,\-]+$/, "")
                }
                a += "..."
            }
            return a
        }, trim: function (a) {
            a = typeof a === "string" ? a : "";
            return a.replace(/^\s*/, "").replace(/\s*$/, "")
        }, parseQuerystring: function (a) {
            a = a.split("&");
            for (var c = {}, b, d = 0; d < a.length; d++)
                if ((b =
                a[d].split("=")) && b.length > 1)
                    c[b[0]] = unescape(b[1]);
            return c
        }, applyMask: function (a, c) {
            if (typeof c === "string" && a) {
                var b = /(.*)\{n:(\d+)\}(.*)/, d = /(.*)\{b:(\d+)(:(.*)?)?\}(.*)/, g = /(.*)\{d:(.+?)\}(.*)/, e;
                if (c.search(b) > -1) {
                    e = typeof a === "number" ? a : parseFloat(a);
                    if (isNaN(e))
                        return a;
                    b = c.match(b);
                    a = b[1] + e.toFixed(parseInt(b[2])) + b[3]
                }
                if (c.search(d) > -1) {
                    e = typeof a === "number" ? a : parseFloat(a);
                    if (isNaN(e))
                        return a;
                    b = c.match(d);
                    d = ["M", "B", "T", "Q"];
                    b[4] = m.Util.trim(b[4]);
                    if (b[4] != "") {
                        g = b[4].split(",");
                        for (var i in d)
                            if (m.Util.trim(g[i]) !=
                            "")
                                d[i] = g[i]
                    }
                    cnt = 0;
                    for (e /= 1E6; e / 1E3 >= 1 && cnt < 3;) {
                        e /= 1E3;
                        cnt++
                    }
                    a = b[1] + e.toFixed(parseInt(b[2])) + d[cnt] + b[5]
                } else if (c.search(g) > -1) {
                    e = a;
                    if (typeof e === "string")
                        e = m.Date.parse(e);
                    if (e.constructor === Date) {
                        b = c.match(g);
                        a = b[1] + m.Date.format(e, b[2]) + b[3]
                    }
                }
            }
            return a
        }, keys: function (a) {
            a = typeof a === "object" ? a : {};
            var c = [], b;
            for (b in a)
                a.hasOwnProperty(b) && c.push(b);
            return c
        }, vals: function (a) {
            a = typeof a === "object" ? a : {};
            var c = [], b;
            for (b in a)
                a.hasOwnProperty(b) && c.push(a[b]);
            return c
        }, hasKey: function (a, c) {
            for (var b in a)
                if (a.hasOwnProperty(b))
                    if (b ===
                    c)
                        return z;
            return x
        }, rx: new function () {
            var a = {};
            this.get = function (c, b) {
                if (b == undefined)
                    b = "g";
                var d = a[c + "$$" + b];
                if (!d) {
                    d = RegExp(c, b);
                    a[c] = d
                }
                return d
            }
        }, validateNumericValue: function (a, c, b, d) {
            if (a == undefined || isNaN(a = parseInt(a, 10)))
                a = c;
            if (a !== 0)
                a = Math.min(Math.max(a, b), d);
            return a
        }, sprintf: function () {
            var a = arguments, c = 0, b = function (e, i, f, h) {
                f || (f = " ");
                i = e.length >= i ? "" : Array(1 + i - e.length >>> 0).join(f);
                return h ? e + i : i + e
            }, d = function (e, i, f, h, k, j) {
                var l = h - e.length;
                if (l > 0)
                    e = f || !k ? b(e, h, j, f) : e.slice(0, i.length) +
                    b("", l, "0", true) + e.slice(i.length);
                return e
            }, g = function (e, i, f, h, k, j, l) {
                e = e >>> 0;
                f = f && e && { "2": "0b", "8": "0", "16": "0x" }[i] || "";
                e = f + b(e.toString(i), j || 0, "0", false);
                return d(e, f, h, k, l)
            };
            return a[c++].replace(/%%|%(\d+\$)?([-+\'#0 ]*)(\*\d+\$|\*|\d+)?(\.(\*\d+\$|\*|\d+))?([scboxXuidfegEG])/g, function (e, i, f, h, k, j, l) {
                if (e == "%%")
                    return "%";
                var q, u, n = false;
                u = "";
                var r = k = false;
                q = " ";
                for (var o = f.length, p = 0; f && p < o; p++)
                    switch (f.charAt(p)) {
                        case " ":
                            u = " ";
                            break;
                        case "+":
                            u = "+";
                            break;
                        case "-":
                            n = true;
                            break;
                        case "'":
                            q =
                            f.charAt(p + 1);
                            break;
                        case "0":
                            k = true;
                            break;
                        case "#":
                            r = true
                    }
                h = h ? h == "*" ? +a[c++] : h.charAt(0) == "*" ? +a[h.slice(1, -1)] : +h : 0;
                if (h < 0) {
                    h = -h;
                    n = true
                }
                if (!isFinite(h))
                    throw Error("sprintf: (minimum-)width must be finite");
                j = j ? j == "*" ? +a[c++] : j.charAt(0) == "*" ? +a[j.slice(1, -1)] : +j : "fFeE".indexOf(l) > -1 ? 6 : l == "d" ? 0 : undefined;
                i = i ? a[i.slice(0, -1)] : a[c++];
                switch (l) {
                    case "s":
                        l = String(i);
                        n = n;
                        h = h;
                        k = k;
                        q = q;
                        if (j != null)
                            l = l.slice(0, j);
                        return d(l, "", n, h, k, q);
                    case "c":
                        l = String.fromCharCode(+i);
                        n = n;
                        h = h;
                        k = k;
                        if (j != null)
                            l = l.slice(0,
                            j);
                        return d(l, "", n, h, k, void 0);
                    case "b":
                        return g(i, 2, r, n, h, j, k);
                    case "o":
                        return g(i, 8, r, n, h, j, k);
                    case "x":
                        return g(i, 16, r, n, h, j, k);
                    case "X":
                        return g(i, 16, r, n, h, j, k).toUpperCase();
                    case "u":
                        return g(i, 10, r, n, h, j, k);
                    case "i":
                    case "d":
                        q = parseInt(+i, 10);
                        e = q < 0 ? "-" : u;
                        i = e + b(String(Math.abs(q)), j, "0", false);
                        return d(i, e, n, h, k);
                    case "e":
                    case "E":
                    case "f":
                    case "F":
                    case "g":
                    case "G":
                        q = +i;
                        e = q < 0 ? "-" : u;
                        u = ["toExponential", "toFixed", "toPrecision"]["efg".indexOf(l.toLowerCase())];
                        l = ["toString", "toUpperCase"]["eEfFgG".indexOf(l) %
                        2];
                        i = e + Math.abs(q)[u](j);
                        return d(i, e, n, h, k)[l]();
                    default:
                        return e
                }
            })
        }, cookies: function () {
            function a(c, b, d) {
                var g;
                g = "";
                if (d) {
                    g = new Date;
                    g.setTime(g.getTime() + d * 24 * 60 * 60 * 1E3);
                    g = "; expires=" + g.toGMTString()
                }
                document.cookie = c + "=" + b + g + "; path=/"
            }
            return {
                create: a, read: function (c) {
                    var b = c + "=", d = document.cookie.split(";");
                    c = {};
                    var g, e;
                    for (g = 0; g < d.length; g++) {
                        for (e = d[g]; e.charAt(0) == " ";)
                            e = e.substring(1, e.length);
                        if (e.indexOf(b) == 0) {
                            b = e.substring(b.length, e.length).split("&");
                            for (d = 0; d < b.length; d++) {
                                g = unescape(b[d]).split("=");
                                c[g[0]] = g[1]
                            }
                            return c
                        }
                    }
                    return null
                }, erase: function (c) {
                    a(c, "", -1)
                }
            }
        }()
    };
    m.ViewEngine = function () {
        function a(f) {
            var h = [];
            f = f.replace(d, " ").split("</template>");
            for (var k = 0; k < f.length; k++) {
                var j = f[k].match(g), l = j && j[1] || "";
                if (l)
                    h.push({ name: l, html: j && j[2] || "" })
            }
            return h
        }
        function c(f) {
            var h = {}, k = { djContent: "content", djData: "data", djEvent: "event", djPartial: "partial" };
            f = t("<div/>").append(f);
            for (var j in k)
                if (k.hasOwnProperty(j))
                    h[k[j]] = f.find("[" + j + "]");
            return h
        }
        function b(f, h) {
            f.text = f.text.replace(RegExp("\\bdjData\\b",
            "g"), function () {
                var l = h.length;
                h.push(f.data);
                return 'djData="' + l + '"'
            });
            for (var k = 0; k < f.children.length; k++)
                b(f.children[k], h);
            var j = 0;
            f.text = f.text.replace(RegExp("__djPartial__", "g"), function () {
                return f.children[j++].text
            })
        }
        var d = /[\r\t\n]/g, g = /<template[^>]*\bname="([a-zA-z0-9]+)"[^>]*>(.*)/, e = { start: "<#", end: "#>", interpolate: /<#=(.+?)#>/g }, i = RegExp("'(?=[^" + e.end.substr(0, 1) + "]*" + e.end.replace(/([.*+?^${}()|[\]\/\\])/g, "\\$1") + ")", "g");
        return {
            parse: function (f) {
                var h = {}, k = {};
                f = a(f);
                for (var j = 0; j <
                f.length; j++) {
                    var l = f[j], q = l.name;
                    l = new Function("obj", "var __p=[],print=function(){__p.push.apply(__p,arguments);};with(obj){__p.push('" + l.html.replace(/[\r\t\n]/g, " ").replace(i, "\t").split("'").join("\\'").split("\t").join("'").replace(e.interpolate, "',$1,'").split(e.start).join("');").split(e.end).join("__p.push('") + "');}return __p.join('');");
                    k[q] = function (u) {
                        return function (n, r, o) {
                            r = t.extend({}, r, { __self: o });
                            r = u.call(r, n);
                            o.text = r;
                            o.data = n;
                            return r
                        }
                    }(l, q);
                    h[q] = function (u, n) {
                        function r(o, p) {
                            var E =
                            this.__self, A = { parent: E, children: [] };
                            k[o](p, this, A);
                            E.children.push(A);
                            return "__djPartial__"
                        }
                        return function (o, p, E) {
                            function A(I) {
                                return Q[I] || t([])
                            }
                            var R = +new Date;
                            p.p = r;
                            var s = { parent: null, children: [] }, B = [];
                            k[n](o, p, s);
                            b(s, B);
                            var L = (o = t.trim(s.text)) ? t(o) : t([]), Q = c(L), F = A("data");
                            for (o = 0; o < F.length; o++) {
                                p = F[o];
                                s = p.attributes.djData.value;
                                t.data(p, "djData", B[s])
                            }
                            B = A("event");
                            for (o = 0; o < B.length; o++) {
                                var S = B.eq(o);
                                p = B[o];
                                s = p.attributes.djEvent.value;
                                p = s.split("|");
                                for (s = 0; s < p.length; s++) {
                                    var C = m.Util.trim(p[s]).split(":");
                                    if (C.length == 2) {
                                        F = m.Util.trim(C[0] || "").split(",");
                                        C = m.Util.trim(C[1] || "").split(",");
                                        for (var M = [], N = [], y = 0; y < F.length; y++) {
                                            var H = m.Util.trim(F[y]);
                                            H && M.push(H)
                                        }
                                        for (y = 0; y < C.length; y++)
                                            (H = m.Util.trim(C[y])) && N.push(H);
                                        (function (I, O) {
                                            S.bind(I.join(" "), function (T) {
                                                for (var U = t.data(this, "djData"), J = 0; J < O.length; J++)
                                                    m.Notify.exec(O[J], E, { el: this, data: U }, T);
                                                return x
                                            })
                                        })(M, N)
                                    }
                                }
                            }
                            m.Notify.exec(m.Notify.events.templateRendered, E, [n, +new Date - R]);
                            return {
                                html: function () {
                                    return L
                                }, hooks: A
                            }
                        }
                    }(l, q)
                }
                return h
            }, Context: {
                f: {
                    date: function (f,
                            h) {
                        return m.Util.applyMask(h, ["{d:", "}"].join(f))
                    }, number: function (f, h) {
                        return m.Util.applyMask(h, ["{b:", "}"].join(f))
                    }, sprintf: function () {
                        return m.Util.sprintf.apply(this, arguments)
                    }, truncate: function (f, h) {
                        return m.Util.truncate(f, h)
                    }, trim: function (f) {
                        return m.Util.trim(f)
                    }
                }
            }
        }
    }();
    var w = {}, K = 0, D = "djNotify_" + +new Date;
    G.exec = function (a, c, b, d) {
        if (typeof c == "string")
            c = { id: c, type: "Framework" };
        if (c && c.id) {
            var g = [];
            g = v(a, c.id, c.type);
            d = !!d && d || t.Event(a);
            d.event = a;
            for (d.widget = c; g.length > 0;) {
                a = g.shift();
                (fn = a.func) && typeof fn == "function" && a.func(d, b)
            }
        }
    };
    m.Notify = G;
    m.Notify.events = {
        calloutHiding: "calloutHiding", calloutShowing: "calloutShowing", dataReceived: "dataReceived", dataRequested: "dataRequested", dataTransformed: "dataTransformed", error: "error", headlineClicked: "headlineClicked", itemSelected: "itemSelected", log: "log", longRequest: "longRequest", templateRendered: "templateRendered", timezoneOffsetChanged: "timezoneOffsetChanged", viewRendered: "viewRendered", widgetCreated: "widgetCreated", widgetLoaded: "widgetLoaded",
        widgetUnloaded: "widgetUnloaded", pause: "pause", resume: "resume", togglePause: "togglePause", refresh: "refresh"
    }
})();
(function (g) {
    function y() {
        if (typeof arguments[0] == "object") {
            if (arguments.length > 0 && arguments[1] === true)
                this.settings = {};
            DJ.Util.extend(this.settings, arguments[0])
        } else if (arguments.length == 2 && typeof arguments[0] == "string")
            this.settings[arguments[0]] = arguments[1];
        else
            return;
        this.init()
    }
    function z(a) {
        if (a && a.getData)
            this.provider = a;
        else
            DJ.Notify.exec(e.error, DJ.Widgets.items[this.id], "Invalid Provider")
    }
    function p(a, b, d) {
        var c, f;
        c = a.obj.view && document.getElementById(a.obj.view) || DJ.Widgets.html[b];
        if (typeof c != "string" && c.nodeType && c.nodeType == 1)
            c = c.nodeName == "TEXTAREA" && c.value || c.innerHTML;
        c = DJ.ViewEngine.parse(c);
        if (f = d(a.id, a.obj, c)) {
            DJ.Widgets.items[a.id] = g.extend({ id: a.id, type: b, settings: a.obj, instance: f }, f.pub);
            if (typeof DJ.Widgets.items[a.id].set == "undefined")
                DJ.Widgets.items[a.id].set = function () {
                    y.apply(f, arguments)
                };
            if (typeof DJ.Widgets.items[a.id].setProvider == "undefined")
                DJ.Widgets.items[a.id].setProvider = function (m) {
                    z.apply(f, [m])
                };
            DJ.Notify.exec(e.widgetCreated, DJ.Widgets.items[a.id]);
            f.init && f.init();
            DJ.Notify.exec(e.widgetLoaded, DJ.Widgets.items[a.id])
        }
    }
    function q(a, b) {
        var d, c, f = {};
        if (b) {
            g.each(b, function (m, A) {
                f[m.toLowerCase()] = A
            });
            b = f;
            d = b.container ? g("#" + b.container) : g(document.body);
            c = b.id = b.id || "djw-" + Math.ceil(Math.random() * 1E6);
            d.append('<div id="' + c + '" class="djWidgetContainer"></div>');
            DJ.Notify.exec(e.log, i, ["add widget %s to %o", a, d[0]]);
            if (b && b.debug)
                r = s;
            d = { id: c, obj: b };
            if (typeof j[a] != "undefined")
                p(d, a, j[a]);
            else {
                c = h[a];
                if (c === l)
                    c = [];
                c.push(d);
                h[a] = c;
                if (t) {
                    n && clearTimeout(n);
                    n = setTimeout(u, 1)
                }
            }
        } else
            DJ.Notify.exec(e.error, { id: "DJ.Widgets", widget: a }, "Widget was not appropriately defined.")
    }
    function u() {
        var a = [], b;
        for (b in h)
            if (h.hasOwnProperty(b))
                if (typeof h[b] != "undefined" && (typeof j[b] == "undefined" || j[b] != "loading")) {
                    a.push(b);
                    j[b] = "loading"
                }
        if (a.length > 0) {
            b = DJ.Widgets.sessionId() != l && DJ.Widgets.sessionId().length > 0 ? [k, "loader.js?sessionid=" + DJ.Widgets.sessionId() + "&w=", a.join(",")].join("") : DJ.Widgets.encryptedToken() != l && DJ.Widgets.encryptedToken().length > 0 ? [k, "loader.js?token=" +
                DJ.Widgets.encryptedToken() + "&w=", a.join(",")].join("") : [k, "loader.js?w=", a.join(",")].join("");
            if (r)
                b += "&debug=1";
            b = b;
            DJ.Notify.exec(e.log, i, ["GetScript : %s", b]);
            var d = document.head || document.getElementsByTagName("head")[0] || document.documentElement, c = document.createElement("script");
            c.src = b;
            d.appendChild(c);
            DJ.Notify.exec(e.log, i, ["Loader Called %o", a])
        }
    }
    var s = true, k = "", v = "", w = "", j = {}, n, h = {}, x = {}, t = false, r = false, l, o = false;
    g("<div/>");
    var i = { id: "DJ.Widgets", type: "Framework" }, e = DJ.Notify.events;
    DJ.Widgets = {
        sessionId: function (a) {
            if (a)
                v = a;
            return v
        }, encryptedToken: function (a) {
            if (a)
                w = a;
            return w
        }, timezoneOffset: function (a) {
            if (a) {
                o = parseFloat(a) || 0;
                DJ.Notify.exec(e.timezoneOffsetChanged, i, o)
            }
            return o
        }, basePath: function (a) {
            if (a)
                k = a;
            return k
        }, assetPath: function () {
            return k + "assets/"
        }, localize: function (a) {
            a && DJ.Util.extend(x, a);
            return x
        }, items: {}, add: function (a, b) {
            if (arguments.length > 0)
                q(a, b);
            else {
                var d = /\/(\w*)\.js#(.*)$/.exec(g("script:last").attr("src"));
                if (d && d.length > 2) {
                    b = DJ.Util.parseQuerystring(d[2]);
                    a = d[1];
                    a = a !== "widget" ? a : b.w ? b.w : false
                }
                a && b && q(a, b)
            }
        }, remove: function (a) {
            var b = DJ.Widgets.items[a], d;
            if (b) {
                DJ.Notify.exec(e.widgetUnloaded, b);
                if (d = b.instance) {
                    d.dispose && d.dispose();
                    delete b.instance
                }
                delete DJ.Widgets.items[a]
            }
        }, initialize: function () {
            DJ.Notify.exec(e.log, i, ["DJ.Widgets Initialized"]);
            u();
            t = s
        }, widgetLoaded: function (a, b) {
            var d = h[a], c;
            j[a] = b;
            if (d !== l && d.length > 0)
                for (c = 0; c < d.length; c++)
                    p(d[c], a, b);
            h[a] = l
        }, html: {}
    };
    (function (a) {
        DJ.Notify.exec(e.log, i, ["getStylesheet : %s", a]);
        g("head").prepend(['<link rel="stylesheet" type="text/css" href="',
            a, '" />'].join(""))
    })("http://widgets.dowjones.com/Widgets/2.0/styles/allwidgets.less");
    g(DJ.Widgets.initialize);
    g(window).unload(function () {
        for (var a in DJ.Widgets.items)
            DJ.Widgets.remove(a)
    })
})(DJ.$);

DJ.Widgets.basePath('http://widgets.dowjones.com/Widgets/2.0/');
DJ.Widgets.encryptedToken('S001WF92XV72cbbMXmsNXmnMpMvNTAsOTMm5DByMa3G2DJqMsFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUEA');
(function () {
    function h(a) {
        a = "0" + a.toString();
        return a.substr(a.length - 2)
    }
    function p(a, b) {
        switch (b) {
            case 3:
                return l[a];
            case 2:
                return h(a + 1);
            case 1:
                return a + 1;
            default:
                return m[a]
        }
    }
    function q(a, b) {
        switch (b) {
            case 3:
                return a;
            case 2:
                return h((a > 12 ? a - 12 : a) || 12);
            case 1:
                return (a > 12 ? a - 12 : a) || 12;
            default:
                return h(a)
        }
    }
    function j(a, b) {
        return b === 2 ? h(a) : a
    }
    function n(a) {
        var b = false;
        if (typeof a === "string") {
            if (a = a.match(/\/Date\((-?[\d]+)\)\//)) {
                b = parseInt(a[1], 10);
                b = new Date(b)
            }
        } else if (a.constructor === Date)
            b = a;
        return b
    }
    var m = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], l = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"], k = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    DJ.Date = {
        parse: n, format: function (a, b) {
            if (typeof a === "string")
                a = n(a);
            if (!(a instanceof Date))
                return "";
            var e = a, f = DJ.Widgets.timezoneOffset(), o = [], d = 0, g, c;
            if (f !== false && typeof f == "number")
                e = new Date(e.getTime() + f * 36E5);
            for (f = 0; f < b.length; f +=
            d) {
                g = b.charAt(f);
                d = b;
                c = f;
                for (var r = g, i = c + 1, s = d.length; i < s && d.charAt(i) === r;)
                    i += 1;
                d = i - c;
                switch (g) {
                    case "M":
                        c = p(e.getMonth(), d);
                        break;
                    case "D":
                    case "d":
                        a: {
                            g = e.getDate();
                            c = e.getDay();
                            switch (d) {
                                case 3:
                                    c = k[c].substr(0, 3);
                                    break a;
                                case 2:
                                    c = h(g);
                                    break a;
                                case 1:
                                    c = g;
                                    break a;
                                default:
                                    c = k[c]
                            }
                        }
                        break;
                    case "Y":
                    case "y":
                        c = j(e.getFullYear(), d);
                        break;
                    case "H":
                    case "h":
                        c = q(e.getHours(), d);
                        break;
                    case "N":
                    case "n":
                    case "m":
                        c = j(e.getMinutes(), d);
                        break;
                    case "S":
                    case "s":
                        c = j(e.getSeconds(), d);
                        break;
                    case "T":
                    case "t":
                        c = e.getHours() >
                        11 ? "PM" : "AM";
                        c = d < 2 && c.substring(0, 1) || c;
                        c = g === "t" && c.toLowerCase() || c;
                        break;
                    default:
                        c = g;
                        d = 1
                }
                o.push(c)
            }
            return o.join("")
        }, addMinutes: function (a, b) {
            return b * 6E4 && new Date(a.valueOf() + b * 6E4) || a
        }, addHours: function (a, b) {
            return b * 36E5 && new Date(a.valueOf() + b * 36E5) || a
        }, addDays: function (a, b) {
            return b * 864E5 && new Date(a.valueOf() + b * 864E5) || a
        }, monthNames: m, monthAbbreviations: l, weekDayNames: k
    }
})();
(function (t) {
    function u() {
        return Math.ceil(Math.random() * 1E12)
    }
    function v(c) {
        function e(a) {
            a: {
                var b = c.url, h = c.required, k = c.optional;
                if (b.match(/^https?:\/\//))
                    b = b;
                else {
                    var i = y.shift();
                    y.push(i);
                    b = j + i + p + b
                }
                b = b;
                i = {};
                var q = DJ.Widgets.sessionId(), n = DJ.Widgets.encryptedToken(), r, g, l;
                for (r = 0; r < h.length; r += 1) {
                    g = h[r];
                    if (a[g] === o) {
                        DJ.Notify.exec(C.error, D, { Message: "Required field missing : " + g });
                        a = E;
                        break a
                    }
                    if (t.isArray(a[g]))
                        a[g] = a[g].join("|");
                    l = DJ.Util.rx.get("\\{" + g.toLowerCase() + "\\}");
                    if (b.match(l))
                        b =
                        b.replace(l, a[g]);
                    else
                        i[g] = a[g]
                }
                for (h = 0; h < k.length; h += 1) {
                    g = k[h];
                    if (a[g] !== o && a[g] != null) {
                        if (t.isArray(a[g]))
                            a[g] = a[g].join("|");
                        l = DJ.Util.rx.get("\\{" + g.toLowerCase() + "\\}");
                        if (b.match(l))
                            b = b.replace(l, a[g]);
                        else
                            i[g] = a[g]
                    }
                }
                if (q)
                    i.sessionid = q;
                else if (n)
                    i.encryptedtoken = n;
                if (a.id)
                    if ((a = DJ.Widgets.items[a.id]) && a.type)
                        i.ep = "widgets_20_" + a.type;
                b += t.param(i, F);
                a = b
            }
            return a
        }
        function f(a, b, h) {
            if (a && DJ.Widgets.items[b.id])
                b && typeof b.scope !== "undefined" ? a.call(b.scope, h, b) : a(h, b)
        }
        var d = {};
        this.getData =
        function (a, b, h) {
            var k = e(b), i = k.substring(k.indexOf("?"));
            !h && d[i] && (c.cacheDuration === -1 || d[i].date + c.cacheDuration * 1E3 > +new Date) ? setTimeout(function () {
                f(a, b, d[i].data)
            }, 2) : w(k, function (q, n) {
                d[i] = { date: +new Date, data: n };
                f(a, q, n)
            }, b)
        };
        this.buildUrl = e
    }
    function z(c) {
        var e = A[c], f = m[c];
        if (!e) {
            e = new v(f);
            A[c] = e
        }
        return e
    }
    var F = true, E = false, o, y = ["api.dowjones.com", "api1.dowjones.com", "api2.dowjones.com", "api3.dowjones.com", "api4.dowjones.com"], x = document.head || document.getElementsByTagName("head")[0] || document.documentElement, p = "/api/1.0/", j = "http://", B = j + "rtoperator.dowjones.com/api/0.1/", A = {}, m, s =
    "jsonp", w, D = { id: "DJ.Widgets", type: "Framework" }, C = DJ.Notify.events;
    if (typeof JSON !== "undefined" && JSON.parse)
        if (typeof XMLHttpRequest !== "undefined" && "withCredentials" in new XMLHttpRequest)
            s = "xhr-cors";
        else if (/{\s*\[native code\]\s*}/.test(JSON.parse.toString()) && typeof XDomainRequest !== "undefined")
            s = "xdr";
    w = s === "xhr-cors" ? function (c, e, f) {
        c += "&_=" + u();
        var d = new XMLHttpRequest;
        d.open("get", c, true);
        d.onload = function () {
            var a = d.responseText;
            if (JSON && JSON.parse && a) {
                a = JSON.parse(a);
                e(f, a)
            }
        };
        d.onerror = function () {
            e(f,
            { Error: [{ Message: "Data request error occurred (" + d.status + ")." }] })
        };
        d.send()
    } : s === "xdr" ? function (c, e, f) {
        c += "&_=" + u();
        var d = new XDomainRequest;
        d.open("get", c);
        d.onload = function () {
            var a = d.responseText;
            if (JSON && JSON.parse && a) {
                a = JSON.parse(a);
                e(f, a)
            }
        };
        d.onerror = function () {
            e(f, { Error: [{ Message: "Data request error occurred (" + d.status + ")." }] })
        };
        d.onprogress = function () {
        };
        d.ontimeout = function () {
        };
        d.send()
    } : function (c, e, f) {
        if (c && e && f) {
            var d = document.createElement("script"), a = "jsonp_" + u();
            c = c.replace(/\&$/,
            "") + "&callback=" + a;
            window[a] = function (b) {
                e(f, b);
                window[a] = o;
                try {
                    delete window[a]
                } catch (h) {
                }
                x && x.removeChild(d)
            };
            d.src = c;
            x.appendChild(d)
        }
    };
    m = {
        ArticleHtml: { url: "Content/Article/HtmlContent/json?id={articleid}&", required: ["articleid"], optional: ["parts"], cacheDuration: -1 }, RelatedContent: { url: "Content/Related/json?id={articleid}&", required: ["articleid"], optional: ["sourcegenre", "records"], cacheDuration: 120 }, SuggestAuthenticate: {
            url: j + "suggest.factiva.com/authenticate/1.0/registerUsingSessionId?format=json&",
            required: ["suggestauthenticate", "sid"], optional: [""], cacheDuration: -1
        }, SuggestAuthenticateWithToken: { url: j + "suggest.factiva.com/authenticate/1.0/registerUsingEncryptedKey?format=json&eid={encryptedtoken}&", required: ["suggestauthenticate", "encryptedtoken"], optional: [""], cacheDuration: -1 }, SuggestSearch: {
            url: j + "suggest.factiva.com/search/1.0/{suggestcategory}?format=json&maxresults={count}&", required: ["suggestcontext", "searchtext", "count", "suggestcategory"], optional: ["categories", "types", "filterlanguages", "statuses",
                "status", "filternewscoded", "dataset", "filteradr", "filterprivate", "includejobs", "includegroups", "interfacelanguage", "filterparentcodes"], cacheDuration: -1
        }, OpenAccessHeadlines: { url: j + "$OpenAccessDomain$" + p + "Content/Headlines/Topic/{clientid}/{topic}/json?T={token}&overrideHttpStatus=true&", required: ["token", "clientid", "topic"], optional: [], cacheDuration: 0 }, OpenAccessArticle: {
            url: j + "$OpenAccessDomain$" + p + "Content/Article/{clientid}/{articleid}/json?T={token}&overrideHttpStatus=true&", required: ["token",
            "clientid", "articleid"], optional: [], cacheDuration: 0
        }, OpenAccessRegister: { url: j + "$OpenAccessDomain$" + p + "Content/Auth/Register/{clientid}/json?T={token}&overrideHttpStatus=true&", required: ["token", "clientid", "register"], optional: [], cacheDuration: 120 }, ContentSearch: {
            url: "Content/search/json?", required: ["querystring"], optional: ["parts", "searchresultpartlist", "sourcegenre", "records", "sortby", "sortorder", "offset", "startdate", "enddate", "daysrange", "searchmode", "duplicationmode", "alldates", "maxbuckets", "blacklist",
            "languagecode", "snippettype"], cacheDuration: 120
        }, ContentCollection: { url: "Content/collection/json?name={collectionname}&", required: ["collectionname"], optional: ["parts", "records", "snippettype"], cacheDuration: 120 }, AlertsContent: { url: "Content/alert/json?name={alertname}&id={alertid}&", required: [], optional: ["alertid", "alertname", "sourcegenre", "records", "sortby", "sortorder", "offset"], cacheDuration: 120 }, RealTimeHeadlinesSearch: {
            url: "RealTimeHeadlines/alert/json?searchString={realtimequery}&", required: ["realtimequery"],
            optional: ["records", "alertcontext", "timeout", "languagecode"], cacheDuration: 0
        }, RealTimeHeadlinesTopic: { url: "RealTimeHeadlines/sharedAlert/json?topic={realtimetopic}&", required: ["realtimetopic"], optional: ["records", "alertcontext", "timeout", "languagecode"], cacheDuration: 0 }, LowLatencySearch: { url: B + "RealTimeHeadlines/alert/json?searchString={realtimequery}&", required: ["realtimequery"], optional: ["records", "alertcontext", "timeout", "languagecode"], cacheDuration: 0 }, LowLatencyTopic: {
            url: B + "RealTimeHeadlines/sharedAlert/json?topic={realtimetopic}&",
            required: ["realtimetopic"], optional: ["records", "alertcontext", "timeout", "languagecode"], cacheDuration: 0
        }, RealTimeHeadlinesSharedTopic: { url: "RealTimeHeadlines/sharedqueuecontent/json?topic={realtimesharedtopic}&", required: ["realtimesharedtopic"], optional: ["records", "fetchorder", "timeout", "languagecode"], cacheDuration: 0 }, RealTimeArticleHtml: { url: "RealTimeHeadlines/htmlArticle/json?id={realtimearticleid}&", required: ["realtimearticleid"], optional: ["parts"], cacheDuration: -1 }, EventFilters: {
            url: "private/events/filters/json?type={filtertype}",
            required: ["type"], optional: [""], cacheDuration: -1
        }, EventSearch: { url: "private/events/search/ex/json?", required: ["segment"], optional: ["eventid", "startdate", "starttime", "enddate", "endtime", "daysrange", "offset", "chronologicalorder", "lastupdateddatetime", "includeevent", "records", "keyevent", "country", "region", "eventcode", "keywordcode", "keywords", "fcode", "ticker", "isin"], cacheDuration: 0 }, EventUpdate: {
            url: "private/events/id/ex/date/json?id={eventid}&", required: ["eventid"], optional: ["lastupdateddatetime"],
            cacheDuration: 0
        }, HeatMap: { url: "private/heatmap/json?", required: ["timeseries", "datasettype", "varianttypes", "movingaverages"], optional: ["datasetcodes"], cacheDuration: 12 }, Executives: { url: "Executives/json?id={executiveid}&", required: ["executiveid"], optional: ["parts"], cacheDuration: -1 }, ExecutiveSearch: { url: "Executives/search/json?", required: [], optional: ["firstname", "lastname", "company", "region", "country", "state", "city", "industry", "position", "records"], cacheDuration: -1 }, Companies: {
            url: "Company/json?id={companyid}&",
            required: ["companyid", "symbology"], optional: ["parts"], cacheDuration: -1
        }, CompanySearch: { url: "Company/search/json?", required: [], optional: ["name", "ownershipType", "region", "country", "state", "city", "marketcap", "industry", "records"], cacheDuration: -1 }, Industries: { url: "Industries/json?id={industryid}&", required: ["industryid"], optional: [], cacheDuration: -1 }, Radar: { url: "NewsRadar.svc/json?", required: ["entityid", "subjectid", "symbology"], optional: [], cacheDuration: 300 }, RadarEx: {
            url: "NewsRadar/Ex/json?", required: ["entityid",
                "symbology"], optional: ["subjectid", "customquery", "customqueryname"], cacheDuration: 300
        }, SimpleSearch: { url: "Suggest/search/json?", required: ["searchterm"], optional: ["count", "type"], cacheDuration: 0 }, Alerts: { url: "Alert/{type}/json?{type}={alertid}&", required: ["type", "alertid"], optional: [], cacheDuration: 120 }, ProductAlerts: { url: "Alert/json?", required: ["producttype"], optional: ["records", "offset", "parts"], cacheDuration: 120 }, Quote: {
            url: "MarketData/Quote/json?code={code}&symbology={symbology}&", required: ["symbology",
            "code"], optional: ["parts"], cacheDuration: 0
        }, RelationshipMapping: { url: "RelationshipMapping/relationshipMapping/json?sourceCode={source}&targetCode={target}&", required: ["source", "target"], optional: ["sourcescheme", "targetscheme", "degree", "status"], cacheDuration: -1 }, Triggers: { url: "Trigger/search/json?type={type}&", required: ["type"], optional: ["records", "offset", "sortby", "sortorder", "industrycode", "organizationcode", "executivecode", "regioncode", "category", "changetype", "daysrange", "fromdate", "todate"], cacheDuration: 300 },
        TextImage: { url: DJ.Widgets.assetPath() + "RadarCategory.ashx?", required: ["text"], optional: ["angle", "color", "font", "size", "style", "bgcolor"], cacheDuration: -1 }, CompanyChart: { url: "Chart/DataSeries?startDate={startdate}&endDate={enddate}&code={code}&symbology={symbology}&frequency={frequency}&", required: ["startdate", "enddate", "code", "symbology", "frequency"], optional: [], cacheDuration: 0 }, ConvertToBinary: { url: "Content/ConvertToBinary/xml?", required: ["id", "documentformat"], optional: [], cacheDuration: -1 }, ezVideo: {
            url: j +
                "factiva.ramp.com/viewMedia.jsp?e={episode}", required: ["episode"], optional: [], cacheDuration: -1
        }, ezRedirect: { url: j + "factiva.ramp.com/redirect?e={episode}", required: ["episode"], optional: [], cacheDuration: -1 }, ezEmbedSmall: { url: j + "factiva.ramp.com/widgets/1811?includewrapper=false&isIframeBound=true&isNativeSite=false&ep={episode}&", required: ["episode"], optional: ["width", "height"], cacheDuration: -1 }, ezEmbedLarge: {
            url: j + "factiva.ramp.com/widgets/1853?includewrapper=false&isIframeBound=true&isNativeSite=false&episode={episode}&", required: ["episode"], optional: ["width",
                "height"], cacheDuration: -1
        }
    };
    DJ.Data = {
        Provider: v, getProvider: function (c) {
            var e;
            if (typeof c == "string")
                e = z(c);
            else if (c.provider && c.provider in m)
                e = new v(m[c.provider]);
            else {
                var f, d, a, b, h;
                e = 0;
                var k, i;
                for (a in m) {
                    h = 0;
                    if (b = m[a]) {
                        if (b.required) {
                            i = b.required;
                            for (f = 0; f < i.length; f += 1) {
                                d = i[f];
                                if (c[d] === o)
                                    h = -1E3;
                                h += 3
                            }
                        }
                        if (b.optional) {
                            b = b.optional;
                            for (f = 0; f < b.length; f += 1) {
                                d = b[f];
                                if (c[d] !== o && c[d] != null)
                                    h += 1
                            }
                        }
                        if (e < h) {
                            k = a;
                            e = h
                        }
                    }
                }
                if (k) {
                    c = z(k);
                    c.score = e;
                    e = c
                } else
                    e = null
            }
            return e
        }, setRequestFunction: function (c) {
            if (c &&
            typeof c == "function")
                w = function (e, f, d) {
                    c(e, f, d)
                }
        }
    }
})(DJ.$);


function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function getWidgetsOfType() {
    var types = _.toArray(arguments);
    if (types && types.length > 0) {
        return _.select(DJ.Widgets.items, function(w){ return _.indexOf(types, w.type) > -1; });
    }
    return [];
}

// iOS scale bug fix
MBP.scaleFix();
