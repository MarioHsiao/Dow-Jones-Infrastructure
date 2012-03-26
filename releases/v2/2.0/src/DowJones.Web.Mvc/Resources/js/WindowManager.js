$(function () {

    DJ.WindowManager = {

        _windows: {},

        defaults: {
            form: null,
            url: null,
            features: null,
            scrollbar: true,
            toolbar: false,
            location: true,
            statusbar: true,
            menubar: false,
            resizable: true,
            width: -1,
            height: -1,
            left: -1,
            top: -1,
            windowName: null,
            returnWindowObj: false
        },

        closeWindow: function (windowName) {
            if (windowName && this._windows[windowName]) {
                try {
                    if (this._windows[windowName].close)
                        this._windows[windowName].close();
                    this._windows[windowName] = null;
                }
                catch (e) { this._windows[windowName] = null; }
            }
        },

        openWindow: function (options) {
            if (!options)
                return;

            if (typeof (options) == 'string') {
                options = { url: options };
            }

            options = jQuery.extend({}, this.defaults, options);

            var w = -1, h = -1, l = -1, t = -1;
            var f = options.features;
            if (!f) {
                //Build the features of the window if not specified
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
            }
            else {
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

                } catch (e) { }
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

            //    w += 32;
            //    h += 96;
            var win;
            if (options.url) {
                this.closeWindow(options.windowName);
                try {
                    win = window.open("about:blank", options.windowName || "", f);

                    this.resizeAndPosition(win, w, h, l, t, options.url);

                    if (options.windowName)
                        this._windows[options.windowName] = win;
                }
                catch (e) { }
            }
            else if (options.form) {
                var windowName = options.windowName || (options.form.name || "window" + Math.floor(Math.random() * 10000));
                this.closeWindow(windowName);
                try {
                    win = window.open("", windowName, f);

                    this.resizeAndPosition(win, w, h, l, t);

                    this._windows[windowName] = win;
                    setTimeout(function () {
                        var oldTarget = options.form.target;
                        options.form.target = windowName;
                        options.form.submit();
                        options.form.target = oldTarget;
                    }, 400);
                }
                catch (e) { }
            }
            else {
                return false;
            }

            if (options.returnWindowObj)
                return win;

            return false;
        },

        resizeAndPosition: function (win, w, h, l, t, url) {
            setTimeout(function () {
                try {
                    try {
                        win.resizeTo(w, h);
                        win.moveTo(l, t);
                        win.focus();
                    }
                    catch (e) { }
                    if (url)
                        win.location.href = url;
                }
                catch (e) { }
            }, 0);
        }

    }

    $dj.WindowManager = DJ.WindowManager;
    $dj.closeWindow = DJ.WindowManager.closeWindow;
    $dj.openWindow = DJ.WindowManager.openWindow;
    $dj.resizeAndPosition = DJ.WindowManager.resizeAndPosition;
});