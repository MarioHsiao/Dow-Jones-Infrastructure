if (!window.FACTIVA_WIDGET_MGR) (function() {

    var initialized = false;

    var pageLoadCallback = function() {
        FACTIVA_WIDGET_MGR.setPageLoaded();
    };

    var pageUnloadCallback = function() {
        FACTIVA_WIDGET_MGR.setPageUnloaded();
    };

    FACTIVA_WIDGET_MGR = {

        libs: {},

        globals: {
            token: "",
            tokenTime: 0,
            widgets: [],
            widgetCount: 0,
            pageLoaded: false,
            pageUnloaded: false,
            pageLoadListeners: [],
            pageUnloadListeners: [],
            panels: [],
            panelCount: 0,
            showPanelMarks: true,
            suppressGetWidget: false,
            enableLogging: false,
            log: "",
            jsns: [],
            cssns: []
        },

        init: function() {
            if (!initialized) {
                initialized = true;
            }
        },

        loadCSSLibrary: function(path, token, look_for, onload) {
            var tns = self.getNamespace(path);
            if (!self.globals.cssns[tns]) {
                self.importCSS(path, token, look_for, onload);
                self.globals.cssns[tns] = tns;
            }
        },

        loadJSLibrary: function(path, token, look_for, onload) {
            var tns = self.getNamespace(path);
            if (!self.globals.jsns[tns]) {
                self.importJS(path, token, look_for, onload);
                self.globals.jsns[tns] = tns;
                return;
            }
            if (onload) self.wait_for_script_load(look_for, onload);
        },

        getNamespace: function(path) {
            var tns = path;
            tns = tns.replace("http://", "");
            tns = tns.replace("https://", "");
            tns = tns.replace(/[\/|.]/gi, "_");
            return tns;
        },

        logMessage: function(msg) {
            if (self.globals.enableLogging) {
                self.globals.log += (msg + "\n");
            }
        },


        load: function(path, token) {

        },

        getHeadElement: function() {
            var head = document.getElementsByTagName("head");
            if (head) {
                return head[0];
            }
            return null;
        },

        alert: function(msg) {
            alert(msg);
        },

        insertWidget: function(dataSrc, panelId) {
            var temp = dataSrc + panelId;
            self.dyn_script(temp, panelId);
        },

        dyn_script: function(url, panelId) {
            var script = document.createElement('script'); // pretty self explanatory
            script.src = url; // sets the scripts source to the specified url
            script.type = "text/javascript";
            if (panelId) { script.id = "factiva_lib_" + panelId; }
            var head = self.getHeadElement();
            script.onload = function() { head.removeChild(script); }; // remove the script from the DOM
            head.appendChild(script); // appends the tag to the head
        },

        load_lib_script: function(url, token, ns) {
            var script = document.createElement('script');
            url = (url.indexOf() > 0) ? url + "?appver=" + token : url + "&appver=" + token;
            script.src = url; // sets the scripts source to the specified url
            script.type = "text/javascript";
            script.charset = "utf-8";
            var head = self.getHeadElement();
            script.onload = function() { head.removeChild(script); }; // remove the script from the DOM
            head.appendChild(script); // appends the tag to the head
        },

        load_lib_css: function(url, token, ns) {
            var link = document.createElement('link');
            url = (url.indexOf() > 0) ? url + "?appver=" + token : url + "&appver=" + token;
            link.href = url; // sets the scripts source to the specified url
            link.type = "text/css";
            link.rel = "stylesheet";
            link.media = "screen";
            var head = self.getHeadElement();
            head.appendChild(link); // appends the tag to the head
        },

        importJS: function(url, token, look_for, onload) {
            var script = document.createElement('script');
            if (token) {
                url = (url.indexOf() > 0) ? url + "?appver=" + token : url + "&appver=" + token;
            }
            script.src = url; // sets the scripts source to the specified url
            script.type = "text/javascript";
            script.charset = "utf-8";

            var head = self.getHeadElement();
            if (head) {
                head.appendChild(script); // appends the tag to the head       
            }
            else {
                document.body.appendChild(script); // appends the tag to the head       
            }
            if (onload) {
                self.wait_for_script_load(look_for, onload);
            }
        },

        importCSS: function(href, token, look_for, onload) {
            var link = document.createElement('link');
            url = (url.indexOf() > 0) ? url + "?appver=" + token : url + "&appver=" + token;
            link.href = url; // sets the scripts source to the specified url
            link.type = "text/css";
            link.rel = "stylesheet";
            link.media = "screen";

            var head = self.getHeadElement();
            if (head) {
                head.appendChild(link); // appends the tag to the head       
            }
            else {
                document.body.appendChild(link); // appends the tag to the head       
            }
            if (onload) {
                self.wait_for_script_load(look_for, onload);
            }
        },

        wait_for_script_load: function(look_for, callback) {
            var interval = setInterval(function() {
                if (eval("typeof " + look_for) != 'undefined') {
                    clearInterval(interval);
                    callback();
                }
            }, 50);
        },

        processAlertHeadlineWidget: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildAlertWidget(panelId, data, true, false);
        },

        processAlertHeadlineWidgetNoTitle: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildAlertWidget(panelId, data, false, false);
        },

        processAlertHeadlineWidgetNoTitleIntegrateFont: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildAlertWidget(panelId, data, false, true);
        },

        processAutomaticWorkspaceWidget: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildAutomaticWorkspaceWidget(panelId, data, true, false);
        },

        processAutomaticWorkspaceWidgetNoTitle: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildAutomaticWorkspaceWidget(panelId, data, false, false);
        },

        processAutomaticWorkspaceWidgetNoTitleIntegrateFont: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildAutomaticWorkspaceWidget(panelId, data, false, true);
        },

        processManualNewsletterWorkspaceWidget: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildManualNewsletterWorkspaceWidget(panelId, data, true, false);
        },

        processManualNewsletterWorkspaceWidgetNoTitle: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildManualNewsletterWorkspaceWidget(panelId, data, false, false);
        },

        processManualNewsletterWorkspaceWidgetNoTitleIntegrateFont: function(data, panelId) {
            FactivaWidgetRenderManager.getInstance().xBuildManualNewsletterWorkspaceWidget(panelId, data, false, true);
        },

        addEvent: function(obj, evType, fn, useCapture) {
            if (obj.addEventListener) {
                obj.addEventListener(evType, fn, useCapture);
                return true;
            } else if (obj.attachEvent) {
                var r = obj.attachEvent("on" + evType, fn);
                return r;
            } else {
                alert("Handler could not be attached");
            }
        },

        removeEvent: function(obj, evType, fn, useCapture) {
            if (obj.removeEventListener) {
                obj.removeEventListener(evType, fn, useCapture);
                return true;
            } else if (obj.detachEvent) {
                var r = obj.detachEvent("on" + evType, fn);
                return r;
            } else {
                alert("Handler could not be removed");
            }
        },

        addPageLoadListener: function(onLoadListener) {
            if (onLoadListener) {
                if (!self.globals.pageLoaded) {
                    self.globals.pageLoadListeners.push(onLoadListener);
                }
                else {
                    try {
                        onLoadListener();
                    }
                    catch (e) {
                        FACTIVA_WIDGET_MGR.logMessage(e);
                    }
                }
            }
        },

        addPageUnloadListener: function(onUnloadListener) {
            if (onUnloadListener) {
                if (!self.globals.pageUnloaded) {
                    self.globals.pageUnloadListeners.push(onUnloadListener);
                }
                else {
                    try {
                        onUnloadListener();
                    }
                    catch (e) {
                        FACTIVA_WIDGET_MGR.logMessage(e);
                    }
                }
            }
        },

        setPageLoaded: function() {
            if (self.globals.pageLoaded) return;
            self.globals.pageLoaded = true;
            self.removeEvent(window, "load", pageLoadCallback);
        },

        setPageUnloaded: function() {
            if (self.globals.pageUnloaded) return;
            self.globals.pageUnloaded = true;
            self.removeEvent(window, "unload", pageUnloadCallback);
        }

    };

    var self = FACTIVA_WIDGET_MGR;
    self.init();

})();

//EOF: widgetManager_orig.js





