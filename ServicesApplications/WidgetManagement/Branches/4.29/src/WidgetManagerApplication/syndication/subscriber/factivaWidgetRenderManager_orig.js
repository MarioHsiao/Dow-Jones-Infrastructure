/* Start of Preview Render Manager */
if (!window.FactivaWidgetRenderManager) {
    FactivaWidgetRenderManager = (function () {
        var instance = null;
        function PrivateConstructor() {
            var _initialized = false;
            this.getInitialized = function () {
                return _initialized;
            };
            this.setInitialized = function (value) {
                _initialized = value;
            };
            var _index = 0;
            this.getIndex = function () {
                return _index;
            };
            this.setIndex = function (value) {
                _index = value;
            };
            var _data = null;
            this.getData = function () {
                return _data;
            };
            this.setData = function (value) {
                _data = value;
            };
            var _fontFamilies = {
                0: "Arial, Helvetica, sans-serif",
                1: "&quot;Comic Sans MS&quot;, cursive",
                2: "&quot;Courier New&quot;, Courier, monospace",
                3: "Geneva, &quot;MS Sans Serif&quot;, sans-serif",
                4: "Georgia, serif",
                5: "Helvetica, Arial, sans-serif",
                6: "&quot;Times New Roman&quot;, serif",
                7: "&quot;Trebuchet MS&quot;, sans-serif",
                8: "Verdana, Arial, Helvetica, sans-serif"
            };
            this.getFontFamilies = function () {
                return _fontFamilies;
            };
            var _fontSizes = {
                0: {
                    "cssValue": "xx-small",
                    "titleFontSize": 9,
                    "contentFontSize": 8
                },
                1: {
                    "cssValue": "x-small",
                    "titleFontSize": 10,
                    "contentFontSize": 9
                },
                2: {
                    "cssValue": "small",
                    "titleFontSize": 11,
                    "contentFontSize": 10
                },
                3: {
                    "cssValue": "medium",
                    "titleFontSize": 12,
                    "contentFontSize": 11
                },
                4: {
                    "cssValue": "large",
                    "titleFontSize": 13,
                    "contentFontSize": 12
                },
                5: {
                    "cssValue": "x-large",
                    "titleFontSize": 14,
                    "contentFontSize": 13
                },
                6: {
                    "cssValue": "xx-large",
                    "titleFontSize": 15,
                    "contentFontSize": 14
                }
            };
            this.getFontSizes = function () {
                return _fontSizes;
            };
            var _colorTemplates = {
                1: { // DJ CORPORATE
                    "fontFamily": 6,
                    "fontSize": 4,
                    "mainColor": "#999999",
                    "mainFontColor": "#FFFFFF",
                    "accentColor": "#CCCCCC",
                    "accentFontColor": "#A24E04",
                    "chartBarColor": "#101AE3"
                },
                2: { // DJ DARK BLUE
                    "fontFamily": 8,
                    "fontSize": 4,
                    "mainColor": "#313B56",
                    "mainFontColor": "#F7F7F7",
                    "accentColor": "#5D6681",
                    "accentFontColor": "#DCDADA",
                    "chartBarColor": "#BDD546"
                },
                3: { // DJ GREEN
                    "fontFamily": 4,
                    "fontSize": 4,
                    "mainColor": "#71AC2B",
                    "mainFontColor": "#000000",
                    "accentColor": "#9CD756",
                    "accentFontColor": "#333333",
                    "chartBarColor": "#A2A2A2"
                },
                4: { // DJ GREY
                    "fontFamily": 8,
                    "fontSize": 4,
                    "mainColor": "#999999",
                    "mainFontColor": "#000000",
                    "accentColor": "#CCCCCC",
                    "accentFontColor": "#666666",
                    "chartBarColor": "#4988C0"
                },
                5: { // DJ RED
                    "fontFamily": 4,
                    "fontSize": 4,
                    "mainColor": "#940403",
                    "mainFontColor": "#FFFFFF",
                    "accentColor": "#D6D6D6",
                    "accentFontColor": "#940403",
                    "chartBarColor": "#00BCE9"
                }
            };
            this.getColorTemplates = function () {
                return _colorTemplates;
            };
            /* Browser library or piece*/
            var xMac, xSfr3, xOp7Up, xOp6Dn, xIE4Up, xIE4, xIE5, xIE6, xIE7, xNN4, xUA = navigator.userAgent.toLowerCase();
            this.xIsSafari3 = function () { if (xUA.indexOf('safari') > 0) { var is_minor = parseFloat(xUA.substring(xUA.indexOf('safari') + 7, xUA.indexOf('safari') + 12)); var is_major = parseInt(is_minor); if (is_major >= 500) { return true; } else { return false; } } return false; }; xSfr3 = this.xIsSafari3();
            if (window.opera) { var i = xUA.indexOf('opera'); if (i != -1) { var v = parseInt(xUA.charAt(i + 6)); xOp7Up = v >= 7; xOp6Dn = v < 7; } } else if (navigator.vendor != 'KDE' && document.all && xUA.indexOf('msie') != -1) { xIE4Up = parseFloat(navigator.appVersion) >= 4; xIE4 = xUA.indexOf('msie 4') != -1; xIE5 = xUA.indexOf('msie 5') != -1; xIE6 = xUA.indexOf('msie 6') != -1; xIE7 = xUA.indexOf('msie 7') != -1; } else if (document.layers) { xNN4 = true; } xMac = xUA.indexOf('mac') != -1;
            /* Start Helper Library */
            this.xAddEventListener = function (e, eT, eL, cap) { if (!(e = this.xGetElementById(e))) return; eT = eT.toLowerCase(); if (e.addEventListener) e.addEventListener(eT, eL, cap || false); else if (e.attachEvent) e.attachEvent('on' + eT, eL); else { var o = e['on' + eT]; e['on' + eT] = typeof o == 'function' ? function (v) { o(v); eL(v); } : eL; } };
            this.xBackground = function (e, c, i) { if (!(e = this.xGetElementById(e))) return ''; var bg = ''; if (e.style) { if (this.xStr(c)) { e.style.backgroundColor = c; } if (this.xStr(i)) { e.style.backgroundImage = (i != '') ? 'url(' + i + ')' : null; } bg = e.style.backgroundColor; } return bg; };
            this.xClientHeight = function () { var v = 0, d = document, w = window; if (d.compatMode == 'CSS1Compat' && !w.opera && d.documentElement && d.documentElement.clientHeight) { v = d.documentElement.clientHeight; } else if (d.body && d.body.clientHeight) { v = d.body.clientHeight; } else if (this.xDef(w.innerWidth, w.innerHeight, d.width)) { v = w.innerHeight; if (d.width > w.innerWidth) v -= 16; } return v; };
            this.xClientWidth = function () { var v = 0, d = document, w = window; if (d.compatMode == 'CSS1Compat' && !w.opera && d.documentElement && d.documentElement.clientWidth) { v = d.documentElement.clientWidth; } else if (d.body && d.body.clientWidth) { v = d.body.clientWidth; } else if (this.xDef(w.innerWidth, w.innerHeight, d.height)) { v = w.innerWidth; if (d.height > w.innerHeight) v -= 16; } return v; };
            this.xClip = function (e, t, r, b, l) { if (!(e = this.xGetElementById(e))) return; if (e.style) { if (xNum(l)) e.style.clip = 'rect(' + t + 'px ' + r + 'px ' + b + 'px ' + l + 'px)'; else e.style.clip = 'rect(0 ' + parseInt(e.style.width) + 'px ' + parseInt(e.style.height) + 'px 0)'; } };
            this.xColor = function (e, s) { if (!(e = this.xGetElementById(e))) return ''; var c = ''; if (e.style && this.xDef(e.style.color)) { if (this.xStr(s)) e.style.color = s; c = e.style.color; } return c; };
            this.xCreateElement = function (sTag) { if (document.createElement) return document.createElement(sTag); else return null; };
            this.xDef = function () { for (var i = 0; i < arguments.length; ++i) { if (typeof (arguments[i]) == 'undefined') return false; } return true; };
            this.xDisplay = function (e, s) { if ((e = this.xGetElementById(e)) && e.style && this.xDef(e.style.display)) { if (this.xStr(s)) { try { e.style.display = s; } catch (ex) { e.style.display = ''; } } return e.style.display; } return null; };
            this.xGetComputedStyle = function (oEle, sProp, bInt) { var s, p = 'undefined'; var dv = document.defaultView; if (dv && dv.getComputedStyle) { s = dv.getComputedStyle(oEle, ''); if (s) p = s.getPropertyValue(sProp); } else if (oEle.currentStyle) { var i, c, a = sProp.split('-'); sProp = a[0]; for (i = 1; i < a.length; ++i) { c = a[i].charAt(0); sProp += a[i].replace(c, c.toUpperCase()); } p = oEle.currentStyle[sProp]; } else return null; return bInt ? (parseInt(p) || 0) : p; };
            this.xGetElementById = function (e) { if (typeof (e) == 'string') { if (document.getElementById) e = document.getElementById(e); else if (document.all) e = document.all[e]; else e = null; } return e; };
            this.xGetElementsByTagName = function (t, p) { var list = null; t = t || '*'; p = p || document; if (p.getElementsByTagName) { list = p.getElementsByTagName(t); if (t == '*' && (!list || !list.length)) list = p.all; } else { if (t == '*') list = p.all; else if (p.all && p.all.tags) list = p.all.tags(t); } return list || []; };
            this.xHasPoint = function (e, x, y, t, r, b, l) { if (!this.xNum(t)) { t = r = b = l = 0; } else if (!this.xNum(r)) { r = b = l = t; } else if (!this.xNum(b)) { l = r; b = t; } var eX = this.xPageX(e), eY = this.xPageY(e); return (x >= eX + l && x <= eX + this.xWidth(e) - r && y >= eY + t && y <= eY + this.xHeight(e) - b); };
            this.xHeight = function (e, h) { if (!(e = this.xGetElementById(e))) return 0; if (this.xNum(h)) { if (h < 0) h = 0; else h = Math.round(h); } else h = -1; var css = this.xDef(e.style); if (e == document || e.tagName.toLowerCase() == 'html' || e.tagName.toLowerCase() == 'body') { h = this.xClientHeight(); } else if (css && this.xDef(e.offsetHeight) && this.xStr(e.style.height)) { if (h >= 0) { var pt = 0, pb = 0, bt = 0, bb = 0; if (document.compatMode == 'CSS1Compat') { var gcs = this.xGetComputedStyle; pt = gcs(e, 'padding-top', 1); if (pt !== null) { pb = gcs(e, 'padding-bottom', 1); bt = gcs(e, 'border-top-width', 1); bb = gcs(e, 'border-bottom-width', 1); } else if (this.xDef(e.offsetHeight, e.style.height)) { e.style.height = h + 'px'; pt = e.offsetHeight - h; } } h -= (pt + pb + bt + bb); if (isNaN(h) || h < 0) return; else e.style.height = h + 'px'; } h = e.offsetHeight; } else if (css && this.xDef(e.style.pixelHeight)) { if (h >= 0) e.style.pixelHeight = h; h = e.style.pixelHeight; } return h; };
            this.xHide = function (e) { return this.xVisibility(e, 0); };
            this.xLeft = function (e, iX) { if (!(e = this.xGetElementById(e))) return 0; var css = this.xDef(e.style); if (css && this.xStr(e.style.left)) { if (this.xNum(iX)) e.style.left = iX + 'px'; else { iX = parseInt(e.style.left); if (isNaN(iX)) iX = this.xGetComputedStyle(e, 'left', 1); if (isNaN(iX)) iX = 0; } } else if (css && this.xDef(e.style.pixelLeft)) { if (this.xNum(iX)) e.style.pixelLeft = iX; else iX = e.style.pixelLeft; } return iX; };
            this.xMoveTo = function (e, x, y) { this.xLeft(e, x); this.xTop(e, y); };
            this.xNum = function () { for (var i = 0; i < arguments.length; ++i) { if (isNaN(arguments[i]) || typeof (arguments[i]) != 'number') return false; } return true; };
            this.xOffsetLeft = function (e) { if (!(e = this.xGetElementById(e))) return 0; if (this.xDef(e.offsetLeft)) return e.offsetLeft; else return 0; };
            this.xOffsetTop = function (e) { if (!(e = this.xGetElementById(e))) return 0; if (xDef(e.offsetTop)) return e.offsetTop; else return 0; };
            this.xOpacity = function (e, o) { var set = this.xDef(o); if (!(e = this.xGetElementById(e))) return 2; if (this.xStr(e.style.opacity)) { if (set) e.style.opacity = o + ''; else o = parseFloat(e.style.opacity); } else if (this.xStr(e.style.filter)) { if (set) e.style.filter = 'alpha(opacity=' + (100 * o) + ')'; else if (e.filters && e.filters.alpha) { o = e.filters.alpha.opacity / 100; } } else if (this.xStr(e.style.MozOpacity)) { if (set) e.style.MozOpacity = o + ''; else o = parseFloat(e.style.MozOpacity); } else if (this.xStr(e.style.KhtmlOpacity)) { if (set) e.style.KhtmlOpacity = o + ''; else o = parseFloat(e.style.KhtmlOpacity); } return isNaN(o) ? 1 : o; };
            this.xPageX = function (e) { if (!(e = this.xGetElementById(e))) return 0; var x = 0; while (e) { if (this.xDef(e.offsetLeft)) x += e.offsetLeft; e = this.xDef(e.offsetParent) ? e.offsetParent : null; } return x; };
            this.xPageY = function (e) { if (!(e = this.xGetElementById(e))) return 0; var y = 0; while (e) { if (this.xDef(e.offsetTop)) y += e.offsetTop; e = this.xDef(e.offsetParent) ? e.offsetParent : null; } return y; };
            this.xParent = function (e, bNode) { if (!(e = this.xGetElementById(e))) return null; var p = null; if (!bNode && this.xDef(e.offsetParent)) p = e.offsetParent; else if (this.xDef(e.parentNode)) p = e.parentNode; else if (this.xDef(e.parentElement)) p = e.parentElement; return p; };
            this.xParentNode = function (ele, n) { while (ele && n--) { ele = ele.parentNode; } return ele; };
            this.xRemoveEventListener = function (e, eT, eL, cap) { if (!(e = this.xGetElementById(e))) return; eT = eT.toLowerCase(); if (e.removeEventListener) e.removeEventListener(eT, eL, cap || false); else if (e.detachEvent) e.detachEvent('on' + eT, eL); else e['on' + eT] = null; };
            this.xResizeTo = function (e, w, h) { this.xWidth(e, w); this.xHeight(e, h); };
            this.xScrollLeft = function (e, bWin) { var offset = 0; if (!this.xDef(e) || bWin || e == document || e.tagName.toLowerCase() == 'html' || e.tagName.toLowerCase() == 'body') { var w = window; if (bWin && e) w = e; if (w.document.documentElement && w.document.documentElement.scrollLeft) offset = w.document.documentElement.scrollLeft; else if (w.document.body && this.xDef(w.document.body.scrollLeft)) offset = w.document.body.scrollLeft; } else { e = this.xGetElementById(e); if (e && this.xNum(e.scrollLeft)) offset = e.scrollLeft; } return offset; };
            this.xScrollTop = function (e, bWin) { var offset = 0; if (!this.xDef(e) || bWin || e == document || e.tagName.toLowerCase() == 'html' || e.tagName.toLowerCase() == 'body') { var w = window; if (bWin && e) w = e; if (w.document.documentElement && w.document.documentElement.scrollTop) offset = w.document.documentElement.scrollTop; else if (w.document.body && this.xDef(w.document.body.scrollTop)) offset = w.document.body.scrollTop; } else { e = this.xGetElementById(e); if (e && this.xNum(e.scrollTop)) offset = e.scrollTop; } return offset; };
            this.xShow = function (e) { return this.xVisibility(e, 1); };
            this.xStr = function (s) { for (var i = 0; i < arguments.length; ++i) { if (typeof (arguments[i]) != 'string') return false; } return true; };
            this.xTabPanelGroup = function (id, w, h, th, clsTP, clsTG, clsTD, clsTS, index) // object prototype
            {
                // Private Methods
                function onClick() {
                    paint(this);
                    return false;
                }
                function onFocus() {
                    paint(this);
                    return false;
                }
                function paint(tab) {
                    tab.className = clsTS;
                    tab.style.zIndex = highZ++;
                    panels[tab.xTabIndex].style.display = 'block';
                    if (selectedIndex != tab.xTabIndex) {
                        panels[selectedIndex].style.display = 'none';
                        tabs[selectedIndex].className = clsTD;
                        selectedIndex = tab.xTabIndex;
                        // keep track of the selected tab so updatePreview doesnt reset to tab 1
                        emg_selectedTab = selectedIndex + 1;
                    }

                    // dynamic height resize hook for IGoogle portal pages
                    if (typeof (_IG_AdjustIFrameHeight) != "undefined" && typeof (_IG_AdjustIFrameHeight) == "function") {
                        window.setTimeout("_IG_AdjustIFrameHeight()", 1);
                    }
                }

                // Private Properties
                var panelGrp, tabGrp, panels, tabs, highZ, selectedIndex;

                // Public Methods
                this.select = function (n) {
                    if (n && n <= tabs.length) {
                        var t = tabs[n - 1];
                        t.onclick();
                    }
                };
                this.addedListener = false;
                this.onResize = function (newW, newH) {
                    var x = 0, i;
                    tabGrp[0].leftCarIndex = (tabGrp[0].leftCarIndex > 0) ? tabGrp[0].leftCarIndex : 0;

                    if (newW) {
                        w = newW;
                        this.xWidth(panelGrp, w);
                    }
                    else w = this.xWidth(panelGrp);

                    if (newH) {
                        h = newH;
                        this.xHeight(panelGrp, h);
                    }
                    else h = this.xHeight(panelGrp);

                    // resize the tab panel group
                    this.xResizeTo(tabGrp[0], w, th + 2);

                    // calculate the width of a tab
                    var tw = Math.floor((w - tabs.length * 2) / tabs.length);

                    // keep track of the tab sizes to ensure the inner tab group container size is correct
                    var tabSizeArray = [];
                    for (i = 0; i < tabs.length; i++) {

                        if (!xIE4Up) this.xWidth(tabs[i], "auto");
                        //this.xWidth(tabs[i], "auto");

                        // size the tab group container
                        if (tw <= this.xWidth(tabs[i])) {
                            tabSizeArray.push(this.xWidth(tabs[i]));
                            if (xUA.indexOf('mozilla') != -1 && xUA.indexOf('msie') == -1 && typeof (_IG_AdjustIFrameHeight) != "undefined")
                                this.xResizeTo(tabs[i], tabs[i].clientWidth, th + 2);
                            else
                                this.xResizeTo(tabs[i], this.xWidth(tabs[i]), th);
                        }
                        else {
                            tabSizeArray.push(tw);
                            this.xResizeTo(tabs[i], tw, th);
                        }

                        tabs[i].xTabIndex = i;
                        tabs[i].onclick = onClick;
                        panels[i].style.display = 'none';
                    }

                    for (var u = 0, sum = 0; u < tabSizeArray.length; sum += tabSizeArray[u++]);

                    // resize the inner tab group container
                    this.xResizeTo(tabGrp[0], sum, th + 2);

                    // add the carousel buttons if needed
                    var leftTab = this.xGetElementById("fctv_tabLeft" + index);
                    var rightTab = this.xGetElementById("fctv_tabRight" + index);

                    // size the left and right carousel buttons
                    this.xHeight(leftTab, th);
                    this.xHeight(rightTab, th);

                    var outerTabContainer = this.xGetElementById("fctv_tabgroup_outer" + index);
                    if (sum > w) {

                        if (leftTab != null) {
                            leftTab.style.display = "block";
                            if (xIE6) leftTab.style.display = "inline";
                            leftTab.style.visibility = "visible";
                            leftTab.className = "tabLeft";
                            this.xWidth(leftTab, 22);
                        }

                        if (rightTab != null) {
                            rightTab.style.display = "block";
                            if (xIE6) rightTab.style.display = "inline";
                            rightTab.style.visibility = "visible";
                            rightTab.className = "tabRight";
                            this.xWidth(rightTab, 22);
                        }

                        if (outerTabContainer != null) {
                            outerTabContainer.style.width = "100%";
                            outerTabContainer.style.marginLeft = "22px";
                            outerTabContainer.style.marginRight = "22px";
                            if (!xIE6)
                                this.xLeft(outerTabContainer, 0);
                            else
                                outerTabContainer.style.display = "inline";
                        }

                        // this.xResizeTo(outerTabContainer, outerTabContainer.clientWidth-44, th+2);

                        if ((xIE4Up) && typeof (_IG_AdjustIFrameHeight) != "undefined") {
                            this.xResizeTo(outerTabContainer, outerTabContainer.clientWidth, th + 2);
                        }
                        else {
                            this.xResizeTo(outerTabContainer, outerTabContainer.clientWidth - 44, th + 2);
                        }
                    } else {
                        // calculate the gutter width for both sides
                        if (xUA.indexOf('mozilla') != -1)
                            var gutterWidth = (this.xWidth(outerTabContainer) - sum) / 2;
                        else
                            var gutterWidth = (this.xWidth(outerTabContainer) - sum) / 4;

                        if (leftTab != null) {
                            leftTab.style.display = "inline-block";
                            leftTab.style.visibility = "visible";
                            leftTab.className = "tabLeftGutter";
                            this.xResizeTo(leftTab, gutterWidth, th);
                        }

                        if (rightTab != null) {
                            rightTab.style.display = "inline-block";
                            rightTab.style.visibility = "visible";
                            rightTab.className = "tabRightGutter";
                            this.xResizeTo(rightTab, gutterWidth, th);
                        }

                        if (outerTabContainer != null) {
                            outerTabContainer.style.width = "100%";
                            outerTabContainer.style.marginLeft = "0px";
                            outerTabContainer.style.marginRight = "0px";

                            if (xIE4Up || xUA.indexOf('safari') != -1 || xUA.indexOf('chrome') != -1) this.xLeft(outerTabContainer, 0);
                            else this.xLeft(outerTabContainer, gutterWidth);
                            if (typeof (_IG_AdjustIFrameHeight) == "function") this.xLeft(outerTabContainer, gutterWidth);
                        }

                        tabGrp[0].style.left = "0";
                    }

                    highZ = i;
                    // tabs[selectedIndex].onclick();
                    if (tabs != null) {
                        tabs[selectedIndex].onclick();
                    }
                };

                // Constructor Code
                this.xTabPanelGroup.instances[id] = this;
                panelGrp = this.xGetElementById(id);
                if (!panelGrp) { return null; }
                panels = this.xGetElementsByClassName(clsTP, panelGrp);
                tabs = this.xGetElementsByClassName(clsTD, panelGrp);
                tabGrp = this.xGetElementsByClassName(clsTG, panelGrp);
                if (!panels || !tabs || !tabGrp || panels.length != tabs.length || tabGrp.length != 1) { return null; }
                selectedIndex = 0;

                if (!FactivaWidgetRenderManager.getInstance().xTabPanelGroup.instances[id].addedListener) {
                    this.xAddEventListener(window, 'resize',
                    function () {

                        /*
                        if (!xIE4Up && !FactivaWidgetRenderManager.getInstance().xTabPanelGroup.instances[id].addedListener)
                        updateAlertHeadlineWidgetPreview();
                        */

                        for (var j = 0; j < tabs.length; j++) {
                            // resize the tabs to auto so when onResize is called the proper width will calculate
                            if (!xIE4Up) FactivaWidgetRenderManager.getInstance().xWidth(tabs[i], "auto");
                        }

                        var cDiv = FactivaWidgetRenderManager.getInstance().xGetElementById("fctv_tabgroup_outer" + index);
                        var leftBtn = FactivaWidgetRenderManager.getInstance().xGetElementById("fctv_tabLeft" + index);
                        var rightBtn = FactivaWidgetRenderManager.getInstance().xGetElementById("fctv_tabRight" + index);
                        var cDivRight = FactivaWidgetRenderManager.getInstance().xLeft(cDiv) + FactivaWidgetRenderManager.getInstance().xWidth(cDiv);
                        var tDivRight = FactivaWidgetRenderManager.getInstance().xLeft(tabGrp[0]) + FactivaWidgetRenderManager.getInstance().xWidth(tabGrp[0]);

                        // make this condition have a radius to account for browser differences in positioning
                        if (tDivRight < cDivRight && leftBtn.className != "tabLeftGutter") {
                            FactivaWidgetRenderManager.getInstance().xLeft(tabGrp[0], tabGrp[0].offsetLeft + (cDivRight - tDivRight));
                            for (var k = 0; k < tabs.length; k++) {
                                // reset the left carousel index: find which tab is not 
                                // showing completely starting from the left set to that index

                                if ((tabGrp[0].offsetLeft + tabs[k].offsetLeft) < FactivaWidgetRenderManager.getInstance().xWidth(leftBtn)) {
                                    var newK = (k == 0) ? 0 : k - 1;

                                    tabGrp[0].leftCarIndex = newK;
                                    tabGrp[0].edgeDistance = FactivaWidgetRenderManager.getInstance().xWidth(tabs[newK]) - (tabGrp[0].offsetLeft + tabs[k].offsetLeft);
                                    rightBtn.style.backgroundColor = "#DDDDDD";
                                }

                            } // for
                        }

                        if (FactivaWidgetRenderManager.getInstance().xWidth(cDiv) > FactivaWidgetRenderManager.getInstance().xWidth(tabGrp[0])) {
                            tabGrp[0].leftCarIndex = 0;
                            tabGrp[0].edgeDistance = null;
                            rightBtn.onclick = function () { FactivaWidgetRenderManager.getInstance().moveLeftTab(index); return false; };
                            rightBtn.style.backgroundColor = "#FFFFFF";
                            leftBtn.style.backgroundColor = "#FFFFFF";
                        }
                        else {
                            if (leftBtn) {
                                leftBtn.style.backgroundColor = "#FFFFFF";

                                if (tabGrp[0].edgeDistance == null) {
                                    if (tabGrp[0].leftCarIndex <= 0) {
                                        leftBtn.style.backgroundColor = "#DDDDDD";
                                    }
                                }
                            }
                        }

                        if (rightBtn && rightBtn.onclick != null && leftBtn.style.backgroundColor == "#FFFFFF") {
                            rightBtn.style.backgroundColor = "#FFFFFF";
                            leftBtn.style.backgroundColor = "#DDDDDD";
                        }

                        FactivaWidgetRenderManager.getInstance().xTabPanelGroup.instances[id].onResize(0, 0);
                        FactivaWidgetRenderManager.getInstance().xTabPanelGroup.instances[id].addedListener = true;
                    },
                  false);
                }

                this.onResize(w, h);

                this.xAddEventListener(window, 'unload',
                function winUnload() {
                    for (var i = 0; i < tabs.length; ++i) {
                        tabs[i].onfocus = tabs[i].onclick = tabs[i].xTabIndex = null;
                        panels[i] = null;
                    }
                    highZ = null;
                    tabGrp = null;
                    panelGrp = null;
                    selectedIndex = null;
                    FactivaWidgetRenderManager.getInstance().xTabPanelGroup.instances[id] = null;

                }, false
              );
            };
            this.xTabPanelGroup.instances = [];
            this.xTop = function (e, iY) { if (!(e = this.xGetElementById(e))) return 0; var css = this.xDef(e.style); if (css && this.xStr(e.style.top)) { if (this.xNum(iY)) e.style.top = iY + 'px'; else { iY = parseInt(e.style.top); if (isNaN(iY)) iY = this.xGetComputedStyle(e, 'top', 1); if (isNaN(iY)) iY = 0; } } else if (css && this.xDef(e.style.pixelTop)) { if (this.xNum(iY)) e.style.pixelTop = iY; else iY = e.style.pixelTop; } return iY; };
            this.xVisibility = function (e, bShow) { if (!(e = this.xGetElementById(e))) return null; if (e.style && this.xDef(e.style.visibility)) { if (this.xDef(bShow)) e.style.visibility = bShow ? 'visible' : 'hidden'; return e.style.visibility; } return null; };
            this.xWidth = function (e, w) { if (!(e = this.xGetElementById(e))) return 0; if (this.xNum(w)) { if (w < 0) w = 0; else w = Math.round(w); } else w = -1; var css = this.xDef(e.style); if (e == document || e.tagName.toLowerCase() == 'html' || e.tagName.toLowerCase() == 'body') { w = this.xClientWidth(); } else if (css && this.xDef(e.offsetWidth) && this.xStr(e.style.width)) { if (w >= 0) { var pl = 0, pr = 0, bl = 0, br = 0; if (document.compatMode == 'CSS1Compat') { var gcs = this.xGetComputedStyle; pl = gcs(e, 'padding-left', 1); if (pl !== null) { pr = gcs(e, 'padding-right', 1); bl = gcs(e, 'border-left-width', 1); br = gcs(e, 'border-right-width', 1); } else if (this.xDef(e.offsetWidth, e.style.width)) { e.style.width = w + 'px'; pl = e.offsetWidth - w; } } w -= (pl + pr + bl + br); if (isNaN(w) || w < 0) return; else e.style.width = w + 'px'; } w = e.offsetWidth; } else if (css && this.xDef(e.style.pixelWidth)) { if (w >= 0) e.style.pixelWidth = w; w = e.style.pixelWidth; } return w; };
            this.xZIndex = function (e, uZ) { if (!(e = this.xGetElementById(e))) return 0; if (e.style && this.xDef(e.style.zIndex)) { if (this.xNum(uZ)) e.style.zIndex = uZ; uZ = parseInt(e.style.zIndex); } return uZ; };
            this.xGetElementsByClassName = function (c, p, t, f) { var found = []; var re = new RegExp('\\b' + c + '\\b', 'i'); var list = this.xGetElementsByTagName(t, p); for (var i = 0; i < list.length; ++i) { if (list[i].className && list[i].className.search(re) != -1) { found[found.length] = list[i]; if (f) f(list[i]); } } return found; };
            this.xGetElementsByTagName = function (t, p) { var list = null; t = t || '*'; p = p || document; if (p.getElementsByTagName) { list = p.getElementsByTagName(t); if (t == '*' && (!list || !list.length)) list = p.all; } else { if (t == '*') list = p.all; else if (p.all && p.all.tags) list = p.all.tags(t); } return list || []; };
            this.xInnerHtml = function (e, h) { if (!(e = this.xGetElementById(e)) || !this.xStr(e.innerHTML)) return null; var s = e.innerHTML; if (this.xStr(h)) { e.innerHTML = h; } return s; };
            this.xNextSib = function (e, t) { if (!(e = this.xGetElementById(e))) return; var s = e ? e.nextSibling : null; if (t) while (s && s.nodeName.toLowerCase() != t.toLowerCase()) { s = s.nextSibling; } else while (s && s.nodeType != 1) { s = s.nextSibling; } return s; };
            this.xParentNode = function (ele, n) { while (ele && n--) { ele = ele.parentNode; } return ele; };
            this.xPrevSib = function (e, t) { if (!(e = this.xGetElementById(e))) return; var s = e ? e.previousSibling : null; if (t) while (s && s.nodeName.toLowerCase() != t.toLowerCase()) { s = s.previousSibling; } else while (s && s.nodeType != 1) { s = s.previousSibling; } return s; };
            this.xWalkEleTree = function (n, f, d, l, b) { if (typeof l == 'undefined') l = 0; if (typeof b == 'undefined') b = 0; var v = f(n, l, b, d); if (!v) return 0; if (v == 1) { for (var c = n.firstChild; c; c = c.nextSibling) { if (c.nodeType == 1) { if (!l) ++b; if (!this.xWalkEleTree(c, f, d, l + 1, b)) return 0; } } } return 1; };
            this.xWalkTree = function (n, f) { f(n); for (var c = n.firstChild; c; c = c.nextSibling) { if (c.nodeType == 1) this.xWalkTree(c, f); } };
            this.xGetElementsByName = function (name, parentEle, tagName, fn) { var found = []; var re = new RegExp('\\b' + name + '\\b', 'i'); var list = this.xGetElementsByTagName(tagName, parentEle); for (var i = 0; i < list.length; ++i) { if (list[i].name.search(re) != -1) { found[found.length] = list[i]; if (fn) fn(list[i]); } } return found; };
            this.xRemoveChild = function (oParent, oChild) { if (oParent.removeChild) return oParent.removeChild(oChild); else return null; };
            this.xInsertBefore = function (oParent, oNode, oChild) { if (oParent.insertBefore) return oParent.insertBefore(oNode, oChild); };
            this.xCloneNode = function (oNode) { if (oNode.cloneNode) return oNode.cloneNode(true); else return null; };
            this.xCloneLightNode = function (oNode) { if (oNode.cloneNode) return oNode.cloneNode(false); else return null; };
            this.xRemoveChildren = function (oNode) { if (typeof (oNode) != 'undefined' && oNode != null) { return; } var len = oNode.childNodes.length; while (oNode.hasChildNodes()) { this.xRemoveChild(oNode, oNode.firstChild); } };
            this.xParentElement = function (node, tagName, className) { for (; node != null; node = node.parentNode) { if (className && className.length > 0) { if (node.nodeType == 1 && node.nodeName.toUpperCase() == tagName.toUpperCase() && (node.className == className || node.id == className)) return node; } else if (node.nodeType == 1 && node.nodeName.toUpperCase() == tagName.toUpperCase()) return node; } return null; };
            this.xFindChildNode = function (nodeObj, tn, cn) { cn = (cn) ? CStr(cn) : ""; for (var i = 0; i < nodeObj.childNodes.length; i++) { switch (nodeObj.childNodes[i].nodeType) { case 1: tVal = null; if (nodeObj.childNodes[i].tagName == tn.toUpperCase() && (nodeObj.childNodes[i].className == cn || nodeObj.childNodes[i].id == cn)) { return nodeObj.childNodes[i]; } else { tVal = findChildNode(nodeObj.childNodes[i], tn, cn); if (tVal) return tVal; } break; } } };
            this.xInnerText = function (oNode) { if (oNode.innerText) return oNode.innerText; var str = ""; for (var i = 0; i < oNode.childNodes.length; i++) { switch (oNode.childNodes.item(i).nodeType) { case 1: str += xInnerText(oNode.childNodes[i]); break; case 3: str += oNode.childNodes[i].nodeValue; break; } } return str; };
            /* End Helper Library*/
            /* function to fire in a new window the article preview*/
            var xChildWindow = [];
            this.xWinOpen = function (sUrl, w, h, winName) { var winWidth = 800; var winHeight = 600; if (w && w != null) winWidth = w; if (h && h != null) winHeight = h; if (!winName) { winName = "_factivaWidgetArticleWindow"; } var features = "left=0,top=0,width=" + winWidth + ",height=" + winHeight + ",location=0,menubar=1,resizable=1,scrollbars=1,status=1,toolbar=0"; if (xChildWindow[winName] && !xChildWindow[winName].closed) { xChildWindow[winName].location.href = sUrl; try { xChildWindow[winName].resizeTo(winWidth, winHeight); } catch (e) { } } else { xChildWindow[winName] = window.open(sUrl, winName, features); try { xChildWindow[winName].resizeTo(winWidth, winHeight); } catch (e) { } } try { xChildWindow[winName].focus(); } catch (e) { } return false; };

            /* Flash Object File */
            if (typeof com == "undefined") var com = new Object(); if (typeof com.deconcept == "undefined") com.deconcept = new Object(); if (typeof com.deconcept.util == "undefined") com.deconcept.util = new Object(); if (typeof com.deconcept.FlashObjectUtil == "undefined") com.deconcept.FlashObjectUtil = new Object(); com.deconcept.FlashObject = function (swf, id, w, h, ver, c, useExpressInstall, quality, xiRedirectUrl, redirectUrl, detectKey) { if (!document.createElement || !document.getElementById) return; this.DETECT_KEY = detectKey ? detectKey : 'detectflash'; this.skipDetect = com.deconcept.util.getRequestParameter(this.DETECT_KEY); this.params = new Object(); this.variables = new Object(); this.attributes = []; this.useExpressInstall = useExpressInstall; if (swf) this.setAttribute('swf', swf); if (id) this.setAttribute('id', id); if (w) this.setAttribute('width', w); if (h) this.setAttribute('height', h); if (ver) this.setAttribute('version', new com.deconcept.PlayerVersion(ver.toString().split("."))); this.installedVer = com.deconcept.FlashObjectUtil.getPlayerVersion(this.getAttribute('version'), useExpressInstall); if (c) this.addParam('bgcolor', c); var q = quality ? quality : 'high'; this.addParam('quality', q); this.addParam('allowScriptAccess', "always"); this.addParam('wmode', 'transparent'); var xir = (xiRedirectUrl) ? xiRedirectUrl : window.location; this.setAttribute('xiRedirectUrl', xir); this.setAttribute('redirectUrl', ''); if (redirectUrl) this.setAttribute('redirectUrl', redirectUrl); };
            com.deconcept.FlashObject.prototype = { setAttribute: function (name, value) { this.attributes[name] = value; }, getAttribute: function (name) { return this.attributes[name]; }, addParam: function (name, value) { this.params[name] = value; }, getParams: function () { return this.params; }, addVariable: function (name, value) { this.variables[name] = value; }, getVariable: function (name) { return this.variables[name]; }, getVariables: function () { return this.variables; }, createParamTag: function (n, v) { var p = document.createElement('param'); p.setAttribute('name', n); p.setAttribute('value', v); return p; }, getVariablePairs: function () { var variablePairs = []; var key; var variables = this.getVariables(); for (key in variables) { if (key != "extend") variablePairs.push(key + "=" + variables[key]); } return variablePairs; }, getFlashHTML: function () {
                var flashNode = ""; if (navigator.plugins && navigator.mimeTypes && navigator.mimeTypes.length) {// netscape plugin architecture
                    if (this.getAttribute("doExpressInstall")) this.addVariable("MMplayerType", "PlugIn"); flashNode = '<embed type="application/x-shockwave-flash" src="' + this.getAttribute('swf') + '" width="' + this.getAttribute('width') + '" height="' + this.getAttribute('height') + '"'; flashNode += ' id="' + this.getAttribute('id') + '" name="' + this.getAttribute('id') + '" '; var params = this.getParams(); for (var key in params) { if (key != "extend") flashNode += [key] + '="' + params[key] + '" '; } var pairs = this.getVariablePairs().join("&"); if (pairs.length > 0) { flashNode += 'flashvars="' + pairs + '"'; } flashNode += '/>';
                } else {// PC IE
                    if (this.getAttribute("doExpressInstall")) this.addVariable("MMplayerType", "ActiveX"); flashNode = '<object id="' + this.getAttribute('id') + '" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="' + this.getAttribute('width') + '" height="' + this.getAttribute('height') + '">'; flashNode += '<param name="movie" value="' + this.getAttribute('swf') + '" />'; var params = this.getParams(); for (var key in params) { if (key != "extend") flashNode += '<param name="' + key + '" value="' + params[key] + '" />'; } var pairs = this.getVariablePairs().join("&"); if (pairs.length > 0) { flashNode += '<param name="flashvars" value="' + pairs + '" />'; } flashNode += "</object>";
                } return flashNode;
            }, write: function (elementId) {
                if (this.useExpressInstall) {// check to see if we need to do an express install
                    var expressInstallReqVer = new com.deconcept.PlayerVersion([6, 0, 65]); if (this.installedVer.versionIsValid(expressInstallReqVer) && !this.installedVer.versionIsValid(this.getAttribute('version'))) { this.setAttribute('doExpressInstall', true); this.addVariable("MMredirectURL", escape(this.getAttribute('xiRedirectUrl'))); document.title = document.title.slice(0, 47) + " - Flash Player Installation"; this.addVariable("MMdoctitle", document.title); }
                } else { this.setAttribute('doExpressInstall', false); } if (this.skipDetect || this.getAttribute('doExpressInstall') || this.installedVer.versionIsValid(this.getAttribute('version'))) { var n = (typeof elementId == 'string') ? document.getElementById(elementId) : elementId; n.innerHTML = this.getFlashHTML(); } else { if (this.getAttribute('redirectUrl') != "") { document.location.replace(this.getAttribute('redirectUrl')); } }
            }
            };
            com.deconcept.FlashObjectUtil.getPlayerVersion = function (reqVer, xiInstall) {
                var PlayerVersion = new com.deconcept.PlayerVersion(0, 0, 0); if (navigator.plugins && navigator.mimeTypes.length) { var x = navigator.plugins["Shockwave Flash"]; if (x && x.description) { PlayerVersion = new com.deconcept.PlayerVersion(x.description.replace(/([a-z]|[A-Z]|\s)+/, "").replace(/(\s+r|\s+b[0-9]+)/, ".").split(".")); } } else {
                    try { var axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash"); for (var i = 3; axo != null; i++) { axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash." + i); PlayerVersion = new com.deconcept.PlayerVersion([i, 0, 0]); } } catch (e) { } if (reqVer && PlayerVersion.major > reqVer.major) return PlayerVersion; // version is ok, skip minor detection
                    if (!reqVer || ((reqVer.minor != 0 || reqVer.rev != 0) && PlayerVersion.major == reqVer.major) || PlayerVersion.major != 6 || xiInstall) { try { PlayerVersion = new com.deconcept.PlayerVersion(axo.GetVariable("$version").split(" ")[1].split(",")); } catch (e) { } }
                } return PlayerVersion;
            };
            com.deconcept.PlayerVersion = function (arrVersion) { this.major = parseInt(arrVersion[0]) || 0; this.minor = parseInt(arrVersion[1]) || 0; this.rev = parseInt(arrVersion[2]) || 0; };
            com.deconcept.PlayerVersion.prototype.versionIsValid = function (fv) { if (this.major < fv.major) return false; if (this.major > fv.major) return true; if (this.minor < fv.minor) return false; if (this.minor > fv.minor) return true; if (this.rev < fv.rev) return false; return true; };
            com.deconcept.util = { getRequestParameter: function (param) { var q = document.location.search || document.location.hash; if (q) { var startIndex = q.indexOf(param + "="); var endIndex = (q.indexOf("&", startIndex) > -1) ? q.indexOf("&", startIndex) : q.length; if (q.length > 1 && startIndex > -1) { return q.substring(q.indexOf("=", startIndex) + 1, endIndex); } } return ""; } };
            if (Array.prototype.push == null) { Array.prototype.push = function (item) { this[this.length] = item; return this.length; } };
            var getQueryParamValue = com.deconcept.util.getRequestParameter; var FlashObject = com.deconcept.FlashObject;

            this.xBuildManualNewsletterWorkspaceWidget = function (tEle, result, showTitle, integrateFontCSS) {
                var startTime = new Date();
                var serverTime;
                var metaRandomnumber = Math.floor(Math.random() * 100001);
                if (result != null && result.Definition != null) {
                    var sb = [];
                    serverTime = result.ElapsedTime;
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumItemsPerSection;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean') ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);

                    sb[sb.length] = "<div id=\"scriptPreview\"  style=\"" + this.getOuterContainerStyle(bgColor,
                        fontFamily,
                        contentFontSize,
                        mColor,
                        contentColor) + "\">";

                    //folderName, bgColor, fontFamily, fontSize, fontColor
                    if (showTitle) {
                        sb[sb.length] = this.getWidgetTitle(widgetName,
                            mColor,
                            titleFontSize,
                            mFontColor,
                            fontFamily,
                            (result.Literals) ? result.Literals.Icon : null);

                        sb[sb.length] = "<div id=\"fctv_border\"  style=\"" + this.getBorderStyle(bgColor,
                            fontFamily,
                            contentFontSize,
                            mColor,
                            contentColor) + "\">";
                    }

                    if (result.ReturnCode != 0 && tEle) {
                        this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                        return;
                    }
                    if (result.Data != null && result.Data.ManualNewsletterWorkspaceInfo != null) {
                        if (result.Data.ManualNewsletterWorkspaceInfo.Count > 0) {
                            for (var i = 0; i < result.Data.ManualNewsletterWorkspaceInfo.Count; i++) {
                                sb[sb.length] = this.getSectionTitle(result.Data.ManualNewsletterWorkspaceInfo.Sections[i],
                                    aColor,
                                    titleFontSize,
                                    aFontColor,
                                    fontFamily);
                                var randomnumber = Math.floor(Math.random() * 100001);
                                var _count = result.Data.ManualNewsletterWorkspaceInfo.Sections[i].Headlines.length;
                                var _headlines = result.Data.ManualNewsletterWorkspaceInfo.Sections[i].Headlines;
                                var _curDivCntrId = "manualNewsletterWorkspace_" + randomnumber;


                                sb[sb.length] = "<div id=\"" + _curDivCntrId + "\" class=\"fctv_MnlWrkSpcHeadlineCont\" style=\"" + this.getInnerContainerStyle(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor) + "\">";
                                if (_count > numHeadlines) {
                                    sb[sb.length] = this.getPagingBar(numHeadlines,
                                        _count,
                                        result.Literals,
                                        "#e5e5e0",
                                        contentFontSize,
                                        "#333333",
                                        fontFamily,
                                        _curDivCntrId);
                                }
                                for (var j = 0; j < _count; j++) {
                                    var showHeadline = (j < numHeadlines);
                                    var _headline = _headlines[j];
                                    sb[sb.length] = this.getHeadline(bgColor,
                                        fontFamily,
                                        contentFontSize,
                                        contentColor,
                                        auxInfoColor,
                                        snippetColor,
                                        _headline,
                                        showHeadline,
                                        showSnippet,
                                        result.Literals);
                                }
                                if (_count > numHeadlines && numHeadlines > 1) {
                                    sb[sb.length] = this.getPagingBar(numHeadlines,
                                        _count,
                                        result.Literals,
                                        "#e5e5e0",
                                        contentFontSize,
                                        "#333333",
                                        fontFamily,
                                        _curDivCntrId);
                                }
                                if (result.Data.ManualNewsletterWorkspaceInfo.Sections[i].Type == "Sub")
                                    sb[sb.length] = "</div></div>";
                                else
                                    sb[sb.length] = "</div>";
                            }
                        }
                        else if (result.Literals != null && result.Literals.NoResults != null) {
                            sb[sb.length] = this.getNoResultsPanel(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor,
                                    result.Literals.NoResults);
                        }
                    }

                    if (showTitle) {
                        sb[sb.length] = "</div>";
                    }
                    // output the copyright
                    if (result.Literals != null && result.Literals.CopyRight != null) {
                        sb[sb.length] = this.getCopyRightPanel(
                                metaRandomnumber,
                                mColor,
                                _fontFamilies[0],
                                _fontSizes[1].contentFontSize + "px",
                                contentColor,
                                result.Literals.CopyRight,
                                result.Literals.BrandingBadge,
                                result.Literals.MarketingSiteUrl,
                                result.Literals.MarketingSiteTitle,
                                showTitle);
                    }
                    sb[sb.length] = "</div>";
                    this.xInnerHtml(tEle, sb.join(""));
                }
                else {
                    this.xInnerHtml(tEle, "Service is unavailable.");
                }
                // dynamic height resize hook for IGoogle portal pages
                if (typeof (_IG_AdjustIFrameHeight) != "undefined" && typeof (_IG_AdjustIFrameHeight) == "function") {
                    window.setTimeout("_IG_AdjustIFrameHeight()", 1);
                }
                var endTime = new Date();
                this.debugMsg("metaRandomnumber: " + metaRandomnumber + " Rendering Time: " + (endTime.getTime() - startTime.getTime()) + "ms; ServerTime: " + serverTime + "ms");
                this.xInnerHtml(metaRandomnumber + "", "{r:" + (endTime.getTime() - startTime.getTime()) + ";s:" + serverTime + "}");
            };

            this.xBuildAutomaticWorkspaceWidget = function (tEle, result, showTitle, integrateFontCSS) {
                var startTime = new Date();
                var serverTime;
                var metaRandomnumber = Math.floor(Math.random() * 100001);
                if (result != null && result.Definition != null) {
                    var sb = [];
                    serverTime = result.ElapsedTime;
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumOfHeadlines;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean') ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);

                    sb[sb.length] = "<div id=\"scriptPreview\"  style=\"" + this.getOuterContainerStyle(bgColor,
                        fontFamily,
                        contentFontSize,
                        mColor,
                        contentColor) + "\">";


                    //folderName, bgColor, fontFamily, fontSize, fontColor
                    if (showTitle) {
                        sb[sb.length] = this.getWidgetTitle(widgetName,
                            mColor,
                            titleFontSize,
                            mFontColor,
                            fontFamily,
                            (result.Literals) ? result.Literals.Icon : null);

                        sb[sb.length] = "<div id=\"fctv_border\"  style=\"" + this.getBorderStyle(bgColor,
                            fontFamily,
                            contentFontSize,
                            mColor,
                            contentColor) + "\">";
                    }

                    if (result.ReturnCode != 0 && tEle) {
                        this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                        return;
                    }

                    if (result.Data != null && result.Data.AutomaticWorkspaceInfo != null) {
                        if (result.Data.AutomaticWorkspaceInfo.Count > 0) {
                            var randomnumber = Math.floor(Math.random() * 100001);
                            var _count = result.Data.AutomaticWorkspaceInfo.Count;
                            var _headlines = result.Data.AutomaticWorkspaceInfo.Headlines;
                            var _curDivCntrId = "autoWorkspace_" + randomnumber;

                            sb[sb.length] = "<div id=\"" + _curDivCntrId + "\" class=\"fctv_AutoWrkSpcHeadlineCont\" style=\"" + this.getInnerContainerStyle(bgColor,
                                fontFamily,
                                contentFontSize,
                                contentColor) + "\">";
                            if (_count > numHeadlines) {
                                sb[sb.length] = this.getPagingBar(numHeadlines,
                                    _count,
                                    result.Literals,
                                    "#e5e5e0",
                                    contentFontSize,
                                    "#333333",
                                    fontFamily,
                                    _curDivCntrId);
                            }
                            for (var j = 0; j < _count; j++) {
                                var showHeadline = (j < numHeadlines);
                                var _headline = _headlines[j];
                                sb[sb.length] = this.getHeadline(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor,
                                    auxInfoColor,
                                    snippetColor,
                                    _headline,
                                    showHeadline,
                                    showSnippet,
                                    result.Literals);
                            }
                            if (_count > numHeadlines && numHeadlines > 1) {
                                sb[sb.length] = this.getPagingBar(numHeadlines,
                                    _count,
                                    result.Literals,
                                    "#e5e5e0",
                                    contentFontSize,
                                    "#333333",
                                    fontFamily,
                                    _curDivCntrId);
                            }
                            sb[sb.length] = "</div>";
                        }
                        else if (result.Literals != null && result.Literals.NoResults != null) {
                            sb[sb.length] = this.getNoResultsPanel(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor,
                                    result.Literals.NoResults);
                        }
                    }

                    if (showTitle) {
                        sb[sb.length] = "</div>";
                    }
                    // output the copyright
                    if (result.Literals != null && result.Literals.CopyRight != null) {
                        sb[sb.length] = this.getCopyRightPanel(
                                metaRandomnumber,
                                mColor,
                                _fontFamilies[0],
                                _fontSizes[1].contentFontSize + "px",
                                contentColor,
                                result.Literals.CopyRight,
                                result.Literals.BrandingBadge,
                                result.Literals.MarketingSiteUrl,
                                result.Literals.MarketingSiteTitle,
                                showTitle);
                    }
                    sb[sb.length] = "</div>";
                    this.xInnerHtml(tEle, sb.join(""));
                }
                else {
                    this.xInnerHtml(tEle, "Service is unavailable.");
                }
                // dynamic height resize hook for IGoogle portal pages
                if (typeof (_IG_AdjustIFrameHeight) != "undefined" && typeof (_IG_AdjustIFrameHeight) == "function") {
                    window.setTimeout("_IG_AdjustIFrameHeight()", 1);
                }
                var endTime = new Date();
                this.debugMsg("metaRandomnumber: " + metaRandomnumber + " Rendering Time: " + (endTime.getTime() - startTime.getTime()) + "ms; ServerTime: " + serverTime + "ms");
                this.xInnerHtml(metaRandomnumber + "", "{r:" + (endTime.getTime() - startTime.getTime()) + ";s:" + serverTime + "}");
            };

            this.xBuildAlertWidget = function (tEle, result, showTitle, integrateFontCSS) {
                var startTime = new Date();
                var serverTime;
                var metaRandomnumber = Math.floor(Math.random() * 100001);
                if (result != null && result.Definition != null) {
                    var sb = [];
                    serverTime = result.ElapsedTime;
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumOfHeadlines;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);

                    // append styles for the tabs using DHTML
                    this.appendTabStyles(tEle,
                        (showTitle) ? mColor : "#FFFFFF",   //outside border color
                        mColor,                             //tab border color
                        showTitle,
                        aColor,
                        bgColor,
                        (result.Literals) ? result.Literals.CarouselLeft : null,
                        (result.Literals) ? result.Literals.CarouselRight : null);

                    sb[sb.length] = "<div id=\"scriptPreview\" style=\"" + this.getOuterContainerStyle(bgColor,
                        fontFamily,
                        contentFontSize,
                        (showTitle) ? mColor : "#FFFFFF",
                        contentColor) + "\">";

                    //folderName, bgColor, fontFamily, fontSize, fontColor
                    if (showTitle) {
                        sb[sb.length] = this.getWidgetTitle(widgetName,
                            mColor,
                            titleFontSize,
                            mFontColor,
                            fontFamily,
                            (result.Literals) ? result.Literals.Icon : null);
                    }

                    if (result.ReturnCode != 0) {
                        this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                        return;
                    }
                    else {
                        if (result.Definition.DiscoveryTabs != null) {
                            var discoveryTabs = result.Definition.DiscoveryTabs;
                            var activeCount = 0;

                            for (var i = 0; i < discoveryTabs.length; i++) {
                                var currentTab = discoveryTabs[i];
                                if (currentTab.Active == true) {
                                    activeCount++;
                                }
                            }
                            // get the tabs for preview view
                            sb[sb.length] = this.getTabPanelGroup(mColor
                                , result.Literals
                                , discoveryTabs
                                , activeCount);
                        }

                        for (var i = 0; i < discoveryTabs.length; i++) {
                            var currentTab = discoveryTabs[i];
                            if (currentTab.Active == true) {
                                if (currentTab.Id == "headlines") {
                                    sb[sb.length] = this.getAlertHeadlinesPanel(tEle,
                                        result,
                                        showTitle,
                                        integrateFontCSS);
                                }
                                if (currentTab.Id == "companies") {
                                    sb[sb.length] = this.getAlertCompaniesPanel(tEle,
                                        result,
                                        showTitle,
                                        integrateFontCSS);
                                }
                                if (currentTab.Id == "subjects") {
                                    sb[sb.length] = this.getAlertSubjectsPanel(tEle,
                                        result,
                                        showTitle,
                                        integrateFontCSS);
                                }
                                if (currentTab.Id == "industries") {
                                    sb[sb.length] = this.getAlertIndustriesPanel(tEle,
                                        result,
                                        showTitle,
                                        integrateFontCSS);
                                }
                                if (currentTab.Id == "executives") {
                                    sb[sb.length] = this.getAlertExecutivesPanel(tEle,
                                        result,
                                        showTitle,
                                        integrateFontCSS);
                                }
                                if (currentTab.Id == "regions") {
                                    sb[sb.length] = this.getAlertRegionsPanel(tEle,
                                        result,
                                        showTitle,
                                        integrateFontCSS);
                                }
                            }
                        }

                        // close tab panel group div
                        sb[sb.length] = "</div>";
                    }

                    // output the copyright
                    if (result.Literals != null && result.Literals.CopyRight != null) {
                        sb[sb.length] = this.getCopyRightPanel(
                                metaRandomnumber,
                                mColor,
                                _fontFamilies[0],
                                _fontSizes[1].contentFontSize + "px",
                                contentColor,
                                result.Literals.CopyRight,
                                result.Literals.BrandingBadge,
                                result.Literals.MarketingSiteUrl,
                                result.Literals.MarketingSiteTitle,
                                showTitle);
                    }

                    // close script preview div
                    sb[sb.length] = "</div>";
                    this.xInnerHtml(tEle, sb.join(""));

                    // only instantiate the tabs if the data is returned successfully
                    if (result.ReturnCode == 0) {
                        var tabHeight;
                        switch (result.Definition.FontSize) {
                            case 0:
                                tabHeight = 20 * (result.Definition.FontSize + 1);
                                break;
                            case 1:
                                tabHeight = 10 * (result.Definition.FontSize + 1.5);
                                break;
                            case 2:
                                tabHeight = 8 * (result.Definition.FontSize + 1);
                                break;
                            case 3:
                                tabHeight = 7 * (result.Definition.FontSize + 1);
                                break;
                            case 4:
                                tabHeight = 5 * (result.Definition.FontSize + 1);
                                break;
                            case 5:
                                tabHeight = 5 * (result.Definition.FontSize + 1) - 3;
                                break;
                            case 6:
                                tabHeight = 4 * (result.Definition.FontSize + 1);
                                break;
                            default:
                                break;
                        }

                        var index = this.getIndex();
                        this.xTabPanelGroup("fctv_tabpanelgroup" + index, 0, 0, tabHeight, "tabPanel", "tabGroup", "tabDefault", "tabSelected", this.getIndex());
                        this.setIndex(index + 1);
                    }

                }
                else {
                    this.xInnerHtml(tEle, "Service is unavailable.");
                }
                // dynamic height resize hook for IGoogle portal pages
                if (typeof (_IG_AdjustIFrameHeight) != "undefined" && typeof (_IG_AdjustIFrameHeight) == "function") {
                    window.setTimeout("_IG_AdjustIFrameHeight()", 1);
                }

                var endTime = new Date();
                this.debugMsg("metaRandomnumber: " + metaRandomnumber + " Rendering Time: " + (endTime.getTime() - startTime.getTime()) + "ms; ServerTime: " + serverTime + "ms");
                this.xInnerHtml(metaRandomnumber + "", "{r:" + (endTime.getTime() - startTime.getTime()) + ";s:" + serverTime + "}");
            };

            this.debugMsg = function (msg) {
                if (window.console && window.console.log) {
                    window.console.log(msg);
                }
                if (window.opera) {
                    window.opera.postError(msg);
                }
                if (window.debugService) {
                    window.debugService.trace(msg);
                }
            };

            this.getCopyRightPanel = function (metaRandomnumber, bgColor, fontFamily, fontSize, fontColor, token, badge, marketingUrl, title, showTitle) {
                var t = [];
                t[t.length] = "<div id=\"fctv_copyRight\" style=\"" +                    //"background-color:" + bgColor +
                        ";padding:3px 5px" +
                        ";color:" + fontColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:normal" +
                        ";zoom:1" +
                        ";background-color:#FFF;" +
                        ";overflow:hidden;";
                if (showTitle) {
                    t[t.length] = ";border-left: solid 1px " + bgColor +
                                ";border-right: solid 1px " + bgColor;
                }
                t[t.length] = "\">";

                if (badge) {
                    t[t.length] = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" summary=\"\">";
                    t[t.length] = "<tr><td colspan=\"2\"><b style=\"background-color:#999;display:block;height:1px;line-height:1px;overflow:hidden;width:100%;\"></b></td></tr>";
                    t[t.length] = "<tr><td align=\"left\" valign=\"bottom\">";
                    t[t.length] = "<a href=\"" + marketingUrl + "\" target=\"_new\" title=\"" + title + "\">";
                    t[t.length] = "<img src=\"" + badge + "\" border=\"0\" />";
                    t[t.length] = "</a>";
                    t[t.length] = "</td>";
                    t[t.length] = "<td align=\"right\" valign=\"bottom\" width=\"100%\">";
                    t[t.length] = "<div id=\"fctv_copyRight\" style=\"" +
                        ";padding-bottom:2px" +
                        ";color:" + fontColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:normal;" +
                        "\">";
                    t[t.length] = "<span id=\"" + metaRandomnumber + "\" style=\"color:#FFF;\"></span>";
                    t[t.length] = token;
                    t[t.length] = "</div></td>";
                    t[t.length] = "</tr></table>";
                }
                else {
                    t[t.length] = "<span id=\"" + metaRandomnumber + "\" style=\"color:#FFF;\"></span>";
                    t[t.length] = token;
                }
                t[t.length] = "</div>";

                if (showTitle) {
                    t[t.length] = "<b style=\"display:block;font-family:Arial;width:100%;\">";
                    t[t.length] = "<b style=\"" +
                        "background-color: #FFF;" +
                        ";border-left: solid 1px " + bgColor +
                        ";border-right: solid 1px " + bgColor +
                        ";margin:0 1px" +
                        ";line-height:1px;height:2px;overflow:hidden;display:block;" +
                        "\"></b>";
                    t[t.length] = "<b style=\"" +
                        "background-color: #FFF;" +
                        ";border-left: solid 2px " + bgColor +
                        ";border-right: solid 2px " + bgColor +
                        ";margin:0 2px" +
                        ";line-height:1px;height:1px;overflow:hidden;display:block;" +
                        "\"></b>";
                    t[t.length] = "<b style=\"" +
                        "background-color:" + bgColor +
                        ";border-left: solid 1px " + bgColor +
                        ";border-right: solid 1px " + bgColor +
                        ";margin:0 4px" +
                        ";line-height:1px;height:1px;overflow:hidden;display:block;" +
                        "\"></b>";
                    t[t.length] = "</b>";
                }
                return t.join("");
            };

            this.getLinkStyles = function () {
                var sb = [];
                sb[sb.length] = [];
            };

            this.getNoResultsPanel = function (bgColor, fontFamily, fontSize, fontColor, token) {
                return "<div id=\"fctv_headline\" style=\"" +
                        "background-color:" + bgColor +
                        ";padding:3px 5px" +
                        ";color:" + fontColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:normal" +
                        "\">" + token + "</div>";
            };

            this.getHeadline = function (bgColor, fontFamily, fontSize, fontColor, auxInfoColor, snippetColor, headline, showHeadline, showSnippet, literals) {
                debugger;
                var sb = [];
                sb[sb.length] = "<div class=\"fctv_headlineCont\" style=\"display:" + ((showHeadline && showHeadline == true) ? "block" : "none") + "\">";
                var tImg = "";
                var defaultPadding = "3px 3px";
                if (this.xStr(headline.IconUrl) && headline.IconUrl != null && headline.IconUrl.length > 0) {

                    tImg = headline.IconUrl;
                }
                sb[sb.length] = "<div class=\"fctv_healineIcon\" style=\"position:relative; background:" + bgColor + " url(" + tImg + ") no-repeat 3px 3px;\">";
                sb[sb.length] = "<div id=\"fctv_headline\" style=\"" +
                        ";padding:3px 5px" +
                        ";margin-left:15px" +
                        ";padding-bottom:0" +
                        ";color:" + fontColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:normal\">";

                // Add Importance                    
                if (literals && this.xStr(literals.ImportanceFlagUrl) && literals.ImportanceFlagUrl != null && literals.ImportanceFlagUrl.length > 0) {
                    switch (headline.Importance) {
                        // Normal            
                        case 0:
                        default:
                            //sb[sb.length] = "<span class=\"fctv_importance\" style=\"padding-right:0.8em;padding-left:0.3em;font-weight:bold;background:url(" + literals.ImportanceFlagUrl + ") #e5e5e0 no-repeat right 50%; padding-bottom:0.1em;color:#30332d;padding-top:0.1em;\">" + literals.Hot +  "</span>&nbsp;";     
                            break;
                        // Hot            
                        case 1:
                            sb[sb.length] = "<span class=\"fctv_importance\" style=\"border: solid 1px #000; padding-right:0.3em;padding-left:0.3em;font-weight:bold; padding-bottom:0.1em;color:#30332d;padding-top:0.1em;\">" + literals.Hot + "</span>&nbsp;";
                            break;
                        // New            
                        case 2:
                            sb[sb.length] = "<span class=\"fctv_importance\" style=\"border: solid 1px #000; padding-right:0.3em;padding-left:0.3em;font-weight:bold; padding-bottom:0.1em;color:#30332d;padding-top:0.1em;\">" + literals.New + "</span>&nbsp;";
                            break;
                        // MustRead            
                        case 3:
                            sb[sb.length] = "<span class=\"fctv_importance\" style=\"border: solid 1px #000; padding-right:0.3em;padding-left:0.3em;font-weight:bold; padding-bottom:0.1em;color:#30332d;padding-top:0.1em;\">" + literals.MustRead + "</span>&nbsp;";
                            break;
                    }
                }

                sb[sb.length] = ((!(xSfr3 && xMac)) ?
                    "<a class=\"factivaLink\" href=\"" + headline.Url + "\" target=\"_new\" onclick=\"try{if(FactivaWidgetRenderManager && FactivaWidgetRenderManager.getInstance()) {FactivaWidgetRenderManager.getInstance().xWinOpen(this.href);return false;}}catch(e){return true;}\" style=\"font-weight:bold;color:" + fontColor + ";text-decoration:none;\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" title=\"" :
                    "\"><a class=\"factivaLink\" href=\"" + headline.Url + "\" target=\"_new\" style=\"font-weight:bold;color:" + fontColor + ";text-decoration:none;\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" title=\"") +
                    ((!showSnippet) ? headline.Snippet.replace("\"", "&quot;") : headline.Text.replace("\"", "&quot;")) +
                    "\">" + ((headline.TruncText != null && headline.TruncText.length > 0) ? headline.TruncText : headline.Text) + "</a></div>";

                /* 
                sb[sb.length] = "<a class=\"factivaLink\" href=\"javascript:void(0)\" onclick=\"return false;\" style=\"font-weight:bold;color:" + fontColor + ";text-decoration:none;\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" title=\"" + 
                ((!showSnippet) ?  headline.Snippet.replace("\"","&quot;") : headline.Text.replace("\"","&quot;")) + 
                "\">" + ((headline.TruncText != null && headline.TruncText.length > 0 ) ? headline.TruncText : headline.Text) + "</a></div>";   
                */

                var t = [];
                if (this.xStr(headline.SrcName) && headline.SrcName != null && headline.SrcName.length > 0) {
                    t[t.length] = headline.SrcName;
                }
                if (this.xStr(headline.PubDateTime) && headline.PubDateTime != null && headline.PubDateTime.length > 0) {
                    (t.length > 0) ? t[t.length] = ", " : "";
                    t[t.length] = headline.PubDateTime;
                }
                if (this.xStr(headline.WordCount) && headline.WordCount != null && headline.WordCount.length > 0) {
                    (t.length > 0) ? t[t.length] = ", " : "";
                    t[t.length] = headline.WordCount;
                }
                if (this.xStr(headline.Lang) && headline.Lang != null && headline.Lang.length > 0) {
                    (t.length > 0) ? t[t.length] = ", " : "";
                    t[t.length] = headline.Lang;
                }

                if (t.length > 0) {
                    sb[sb.length] = "<div id=\"fctv_auxInfo\" style=\"" +
                            ";padding:3px 5px" +
                            ";padding-top:2px" +
                            ";margin-left:15px;" +
                            ";padding-bottom:0" +
                            ";color:" + auxInfoColor +
                            ";font-size:" + fontSize +
                            ";font-family:" + fontFamily +
                            ";font-weight:normal" +
                            "\">";
                    sb[sb.length] = t.join("");
                    sb[sb.length] = "</div>";

                }
                /* Add Snippet */
                if (showSnippet) {
                    sb[sb.length] = "<div id=\"fctv_snippet\" style=\"" +
                        ";padding:3px 5px" +
                        ";padding-top:2px" +
                        ";margin-left:15px;" +
                        ";color:" + snippetColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:normal" +
                        "\">" + headline.Snippet + "</div>";
                }
                if (this.xStr(headline.Comment) && headline.Comment != null && headline.Comment.length > 0) {
                    sb[sb.length] = "<div id=\"fctv_comment\" style=\"" +
                        ";padding:3px 5px" +
                        ";padding-top:2px" +
                        ";margin-left:15px;" +
                        ";color:" + fontColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:normal" +
                        "\">" + headline.Comment + "</div>";
                }
                sb[sb.length] = "</div>"; // Icon Holder
                sb[sb.length] = "</div>"; // Container
                return sb.join("");
            };

            this.getOuterContainerStyle = function (bgColor, fontFamily, fontSize, borderColor, fontColor) {
                return "background-color:" + bgColor +
                    ";font-family:" + fontFamily +
                    ";font-size:" + fontSize +
                    ";color:" + fontColor +
                    ";font-weight:normal" +
                //";border:solid 1px " + borderColor +       
                    ";";
            };

            this.getBorderStyle = function (bgColor, fontFamily, fontSize, borderColor, fontColor) {
                return "background-color:" + bgColor +
                    ";font-family:" + fontFamily +
                    ";font-size:12" + //fontSize +
                    ";color:" + fontColor +
                    ";font-weight:normal" +
                    ";border-left:solid 1px " + borderColor +
                    ";border-right:solid 1px " + borderColor +
                    ";zoom:1;";
            };

            // add result.Literals to this function when available from backend
            this.getTabPanelGroup = function (mColor, _literals, _discoveryTabs, activeCount) {
                var t = [];
                t[t.length] = "<div id=\"fctv_tabpanelgroup" + this.getIndex() + "\" class=\"tabPanelGroup\" >";
                t[t.length] = (activeCount != 1) ? "<div style=\"display:block;padding-bottom:10px;position:relative;zoom:1;\" >" :
                                                   "<div style=\"position:absolute;height:0px;left:0px;top:0px;width:0px;\" >";
                t[t.length] = this.getTabPanels(_literals, _discoveryTabs);
                t[t.length] = "</div>";
                return t.join("");
            };

            this.getTabPanels = function (_literals, _discoveryTabs) {
                var t = [];

                t[t.length] = "<div id=\"fctv_tabLeft" + this.getIndex() + "\" class=\"tabLeft\" " +
                              " onclick=\"FactivaWidgetRenderManager.getInstance().moveRightTab(" + this.getIndex() + ");return false;\"></div>";
                t[t.length] = "<div id=\"fctv_tabgroup_outer" + this.getIndex() + "\" class=\"tabGroupOuterContainer\">";
                t[t.length] = "<div id=\"fctv_tabgroup" + this.getIndex() + "\" class=\"tabGroup\">";

                // only draw the tabs we need using definition
                for (var i = 0; i < _discoveryTabs.length; i++) {
                    var currentTab = _discoveryTabs[i];

                    if (currentTab.Active == true) {
                        if (currentTab.Id == "headlines")
                            t[t.length] = "<a id=\"" + currentTab.Id + "_tab\" href=\"javascript:void(0)\" style=\"outline:none;\" title=\"" + _literals.Headlines + "\" class=\"tabDefault\">&nbsp;&nbsp;" + _literals.Headlines + "&nbsp;&nbsp;</a>";
                        if (currentTab.Id == "companies")
                            t[t.length] = "<a id=\"" + currentTab.Id + "_tab\" href=\"javascript:void(0)\" style=\"outline:none;\" title=\"" + _literals.Companies + "\" class=\"tabDefault\">&nbsp;&nbsp;" + _literals.Companies + "&nbsp;&nbsp;</a>";
                        if (currentTab.Id == "subjects")
                            t[t.length] = "<a id=\"" + currentTab.Id + "_tab\" href=\"javascript:void(0)\" style=\"outline:none;\" title=\"" + _literals.Subjects + "\" class=\"tabDefault\">&nbsp;&nbsp;" + _literals.Subjects + "&nbsp;&nbsp;</a>";
                        if (currentTab.Id == "industries")
                            t[t.length] = "<a id=\"" + currentTab.Id + "_tab\" href=\"javascript:void(0)\" style=\"outline:none;\" title=\"" + _literals.Industries + "\" class=\"tabDefault\">&nbsp;&nbsp;" + _literals.Industries + "&nbsp;&nbsp;</a>";
                        if (currentTab.Id == "executives")
                            t[t.length] = "<a id=\"" + currentTab.Id + "_tab\" href=\"javascript:void(0)\" style=\"outline:none;\" title=\"" + _literals.Executives + "\" class=\"tabDefault\">&nbsp;&nbsp;" + _literals.Executives + "&nbsp;&nbsp;</a>";
                        if (currentTab.Id == "regions")
                            t[t.length] = "<a id=\"" + currentTab.Id + "_tab\" href=\"javascript:void(0)\" style=\"outline:none;\" title=\"" + _literals.Regions + "\" class=\"tabDefault\">&nbsp;&nbsp;" + _literals.Regions + "&nbsp;&nbsp;</a>";
                    }
                }
                t[t.length] = "&nbsp;</div>";
                t[t.length] = "</div>";
                t[t.length] = "<div id=\"fctv_tabRight" + this.getIndex() + "\" class=\"tabRight\" " +
                              " onclick=\"FactivaWidgetRenderManager.getInstance().moveLeftTab(" + this.getIndex() + ");return false;\"></div>";
                return t.join("");
            };

            this.appendTabStyles = function (tEle, mColor, tabmColor, showTitle, aColor, bgColor, leftCarouselIcon, rightCarouselIcon) {
                var rgb = this.unpack(aColor);
                var hsl = this.RGBToHSL(rgb);

                var tabStyles = "#" + this.xGetElementById(tEle).id + " .tabPanelGroup{overflow:hidden;zoom:1" +
                                    ";border-left:1px solid " + mColor +
                                    ";border-right:1px solid " + mColor +
                                    ";} " +
                                "#" + this.xGetElementById(tEle).id + " .tabPanel{display:inline-block;overflow:hidden;position:relative;width:100%;}" +
                                "#" + this.xGetElementById(tEle).id + " .tabGroupOuterContainer{display:inline-block;margin-bottom:0px;overflow:hidden;position:relative;width:100%;}" +
                                "#" + this.xGetElementById(tEle).id + " .tabLeft{" +
                                    "background: url(" + leftCarouselIcon + ") no-repeat center center" +
                                    ";background-color: #DDDDDD" +
                                    ";border:1px solid " + tabmColor +
                //";border-top:none" +
                                    ";border-top: " + (showTitle ? "none" : "1px solid " + tabmColor) +
                                    ";cursor:pointer" +
                                    ";display:none" +
                //";float:left" +
                                    ";left:0px" +
                                    ";outline:none" +
                                    ";position:absolute" +
                                    ";width:20px" +
                                    ";top:0px" +
                                    ";visibility:hidden" +
                                ";} " +
                                "#" + this.xGetElementById(tEle).id + " .tabLeftGutter{" +
                                "border-bottom:1px solid " + tabmColor +
                                ";background-color:#FFFFFF" +
                                ";filter:alpha(opacity=65)" +
                //";float:left" +
                                ";opacity:0.65" +
                                ";outline:none" +
                                ";left:0px" +
                                ";position:absolute" +
                                ";top:0px" +
                                ";} " +
                                "#" + this.xGetElementById(tEle).id + " .tabRight{" +
                                    "background: url(" + rightCarouselIcon + ") no-repeat center center" +
                                    ";border:1px solid " + tabmColor +
                //";border-top:none" +
                                    ";border-top: " + (showTitle ? "none" : "1px solid " + tabmColor) +
                                    ";cursor:pointer" +
                                    ";display:none" +
                //";float:left" +
                                    ";right:0px" +
                                    ";outline:none" +
                                    ";position:absolute" +
                                    ";width:20px" +
                                    ";top:0px" +
                                    ";visibility:hidden" +
                                ";} " +
                                "#" + this.xGetElementById(tEle).id + " .tabRightGutter{" +
                                "border-bottom:1px solid " + tabmColor +
                                ";background-color:#FFFFFF" +
                                ";filter:alpha(opacity=65)" +
                //";float:left" +
                                ";opacity:0.65" +
                                ";outline:none" +
                                ";right:0px" +
                                ";position:absolute" +
                                ";top:0px" +
                                ";} " +
                                "#" + this.xGetElementById(tEle).id + " .tabGroup{display:block;margin:0 auto;overflow:hidden;position:relative;}" +
                                "#" + this.xGetElementById(tEle).id + " .tabGroup a.tabDefault:link" +
                                ", #" + this.xGetElementById(tEle).id + " .tabGroup a.tabDefault:visited" +
                                ", #" + this.xGetElementById(tEle).id + " .tabGroup a.tabDefault:active" +
                                ", #" + this.xGetElementById(tEle).id + " .tabDefault{background:" + aColor + " none repeat scroll 0 0" +
                                    ";border:1px solid " + tabmColor +
                //";border-top:none" +
                                    ";border-top: " + (showTitle ? "none" : "1px solid " + tabmColor) +
                                    ";color:" + (hsl[2] > 0.5 ? "#000" : "#fff") +
                                    ";cursor:pointer" +
                                    ";display:inline-block" +
                                    ";filter:alpha(opacity=65)" +
                //";float:left" +
                                    ";font-weight:bold" +
                   	                ";opacity:0.65" +
                                    ";padding-top:5px" +
                                    ";text-align:center" +
                                    ";text-decoration:none" +
                                    ";zoom:1" +
                                    ";} " +
                                "#" + this.xGetElementById(tEle).id + " .tabGroup a.tabDefault:hover" +
                                ", #" + this.xGetElementById(tEle).id + " .tabDefault:hover{background:" + bgColor + " none repeat scroll 0 0" +
                                    ";border:1px solid " + tabmColor +
                //";border-top:none" +
                                    ";border-top: " + (showTitle ? "none" : "1px solid " + tabmColor) +
                                    ";border-bottom:1px solid white" +
                                    ";color: #333" +
                                    ";cursor:pointer" +
                                    ";display:inline-block" +
                                    ";filter:alpha(opacity=100)" +
                //";float:left" +
                                    ";font-weight:bold" +
                   	                ";opacity:1.0" +
                                    ";padding-top:5px" +
                                    ";text-align:center" +
                                    ";text-decoration:none" +
                                    ";zoom:1" +
                                    ";} " +
                                "#" + this.xGetElementById(tEle).id + " .tabGroup a.tabSelected:link" +
                                ", #" + this.xGetElementById(tEle).id + " .tabGroup a.tabSelected:visited" +
                                ", #" + this.xGetElementById(tEle).id + " .tabGroup a.tabSelected:active" +
                                ", #" + this.xGetElementById(tEle).id + " .tabGroup a.tabSelected:hover" +
                                ", #" + this.xGetElementById(tEle).id + " .tabSelected{background:" + bgColor + " none repeat scroll 0 0" +
                                    ";border-left:1px solid " + tabmColor +
                                    ";border-right:1px solid " + tabmColor +
                //";border-top:none" +
                                    ";border-top: " + (showTitle ? "none" : "1px solid " + tabmColor) +
                                    ";border-bottom:1px solid white" +
                                    ";color:#000" +
                                    ";cursor:pointer" +
                                    ";display:inline-block" +
                //";float:left" +
                                    ";font-weight:bold" +
                                    ";padding-top:5px" +
                                    ";text-align:center" +
                                    ";text-decoration:none" +
                                    ";zoom:1" +
                                    ";} ";

                var headID = document.getElementsByTagName("head")[0];
                var TabCSS = this.xGetElementById("tabStyles");
                if (TabCSS != null) {
                    //headID.removeChild(TabCSS);
                }

                var cssNode = document.createElement("style");
                cssNode.id = "tabStyles";
                cssNode.setAttribute("type", "text/css");
                if (cssNode.styleSheet) {// IE
                    cssNode.styleSheet.cssText = tabStyles;
                } else {// w3c
                    var cssText = document.createTextNode(tabStyles);
                    cssNode.appendChild(cssText);
                }
                headID.appendChild(cssNode);
            };

            this.getAlertHeadlinesPanel = function (tEle, result, showTitle, integrateFontCSS) {
                if (result != null && result.Definition != null) {
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumOfHeadlines;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);
                }

                var t = [];
                t[t.length] = "<div id=\"alertHeadlinesPanel\" class=\"tabPanel\">";

                if (result.ReturnCode != 0 && tEle) {
                    this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                    return;
                }

                if (result.Data != null && result.Data.Count > 0) {
                    for (var i = 0; i < result.Data.Count; i++) {
                        var _alert = result.Data.Alerts[i];
                        if (_alert.Status.Code == 0) {
                            t[t.length] = this.getAlertTitle(_alert,
                                result.Literals,
                                aColor,
                                titleFontSize,
                                aFontColor,
                                fontFamily);
                            if (_alert.HeadlineCount > 0) {
                                /* Process the headlines */
                                var randomnumber = Math.floor(Math.random() * 100001);
                                var _curDivCntrId = "folder_" + _alert.Id + "_" + randomnumber;
                                t[t.length] = "<div id=\"" + _curDivCntrId + "\" class=\"fctv_AlertHeadlineCont\" style=\"" + this.getInnerContainerStyle(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor) + "\">";
                                if (_alert.HeadlineCount > numHeadlines && numHeadlines != 0) {
                                    t[t.length] = this.getPagingBar(numHeadlines,
                                        _alert.HeadlineCount,
                                        result.Literals,
                                        "#e5e5e0",
                                        contentFontSize,
                                        "#333333",
                                        fontFamily,
                                        _curDivCntrId,
                                        _alert);
                                }
                                for (var j = 0; j < _alert.HeadlineCount; j++) {
                                    var showHeadline = (j < numHeadlines);
                                    var _headline = _alert.Headlines[j];
                                    t[t.length] = this.getHeadline(bgColor,
                                        fontFamily,
                                        contentFontSize,
                                        contentColor,
                                        auxInfoColor,
                                        snippetColor,
                                        _headline,
                                        showHeadline,
                                        showSnippet,
                                        result.Literals);
                                }
                                if (_alert.HeadlineCount > numHeadlines && numHeadlines != 0 && numHeadlines > 1) {
                                    t[t.length] = this.getPagingBar(numHeadlines,
                                        _alert.HeadlineCount,
                                        result.Literals,
                                        "#e5e5e0",
                                        contentFontSize,
                                        "#333333",
                                        fontFamily,
                                        _curDivCntrId,
                                        _alert);
                                }
                                t[t.length] = "</div>";
                            }
                            else if (result.Literals != null && result.Literals.NoResults != null) {
                                t[t.length] = this.getNoResultsPanel(bgColor,
                                        fontFamily,
                                        contentFontSize,
                                        contentColor,
                                        result.Literals.NoResults);
                            }
                        }
                    }
                }
                t[t.length] = "</div>";
                return t.join("");
            };
            this.sayswho = function () {
                debugger;
                var N = navigator.appName, ua = navigator.userAgent, tem;
                var M = ua.match(/(opera|chrome|safari|firefox|msie)\/?\s*(\.?\d+(\.\d+)*)/i);
                if (M && (tem = ua.match(/version\/([\.\d]+)/i)) != null) M[2] = tem[1];
                M = M ? [M[1], M[2]] : [N, navigator.appVersion, '-?'];
                return M;
            };
            this.addDiscoveryChart = function (chartdata, chartimage) {
                debugger;
                var isIE = false;
                var browserN = this.sayswho()[0];
                if (browserN == "msie" || browserN == "MSIE") {
                    isIE = true;
                }

                if (chartdata != null && chartdata.data.length > 0) {

                    var t = [];
                    var discoveryChart = new DiscoveryChart();
                    var htmldata = discoveryChart.RenderDiscoveryChart(chartdata.data, isIE);
                    if (xIE4Up) {
                        t[t.length] = "<div id=\"discoveryChart\" class=\"cd_cont discovery-wrapper\"style=\"margin:10px auto 10px auto" +
                                ";position:relative" +
                                ";text-align:center" +
                                ";font-size: 11px;" +
";padding-bottom: 5px;" +
"border-bottom: 1px solid #f0f0f0;" +
"position: relative" +
                                ";\">" + htmldata + "</div>";
                    } else {
                        t[t.length] = "<div id=\"discoveryChart\" class=\"cd_cont discovery-wrapper\"style=\"margin:10px auto 10px auto" +
                                ";position:relative" + ";font-size: 11px;" +
";padding-bottom: 5px;" +
"border-bottom: 1px solid #f0f0f0" +
                                ";width:" + chartdata.width + "px" +
                                ";\">" + htmldata + "</div>";
                    }

                    //Apply ellipsis only in Firefox browser
                    //                    if ($.browser.mozilla) {
                    //                        var ver = parseFloat($.browser.version);
                    //                        if (ver >= 2 && ver < 7) {
                    //                            t[t.length].find('.ellipsis').ellipsis({ intervalDelay: 0 });
                    //                        }
                    //                    }
                    return t.join("");
                }
            };

            this.addFlashImage = function (chartdata) {
                debugger;
                if (chartdata != null && chartdata.data.length > 0) {
                    if (typeof (FlashObject) != 'undefined') {
                        var fo = new FlashObject(chartdata.chartUri, "temp_swf", chartdata.width, chartdata.height, chartdata.version);
                        var t = [];
                        if (xIE4Up) {
                            t[t.length] = "<div style=\"margin:10px auto 10px auto" +
                                ";position:relative" +
                                ";text-align:center" +
                                ";\">" + fo.getFlashHTML() + "</div>";
                        } else {
                            t[t.length] = "<div style=\"margin:10px auto 10px auto" +
                                ";position:relative" +
                                ";width:" + chartdata.width + "px" +
                                ";\">" + fo.getFlashHTML() + "</div>";
                        }
                        return t.join("");
                    }
                }
            };

            this.addGetThisAlertUrl = function (url, token, mColor) {
                if (url != null) {
                    var t = [];

                    t[t.length] = "<div style=\"display:block;padding:5px 0 5px 0;position:relative\">";
                    t[t.length] = ((!(xSfr3 && xMac)) ?
                    "<a id=\"getAlert\" href=\"" + url + "\"" +
                        " onclick=\"if(FactivaWidgetRenderManager && FactivaWidgetRenderManager.getInstance()) {FactivaWidgetRenderManager.getInstance().xWinOpen(this.href);return false;}\" title=\"" + token + "\"" +
                        " onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\"" +
                        " onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\"" +
                        " style=\"color:" + mColor +
                        ";font-size:10px" +
                        ";font-weight:normal" +
                        ";margin:5px" +
                        ";outline:none" +
                        ";position:relative" +
                        ";text-decoration:none;\">" + token + "</a>" :
                    "<a id=\"getAlert\" href=\"" + url + "\" target=\"_new\" title=\"" + token + "\"" +
                        " onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\"" +
                        " onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\"" +
                        " style=\"color:" + mColor +
                        ";font-size:10px" +
                        ";font-weight:normal" +
                        ";margin:5px" +
                        ";outline:none" +
                        ";position:relative" +
                        ";text-decoration:none;\">" + token + "</a>");

                    /*
                    t[t.length] = "<a id=\"getAlert\" href=\"javascript:void(0)\"" + 
                    " onclick=\"alert(translate('featureIsDisabled'));return false;\"title=\"" + token + "\"" +
                    " onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\"" +
                    " onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\"" +
                    " style=\"color:" + mColor + 
                    ";font-size:10px" +
                    ";font-weight:normal" +
                    ";margin:5px" +
                    ";outline:none" +
                    ";position:relative" +
                    ";text-decoration:none;\">" + token + "</a>";
                    */
                    t[t.length] = "</div>";

                    return t.join("");
                }
            };

            this.getAlertCompaniesPanel = function (tEle, result, showTitle, integrateFontCSS) {
                debugger;
                if (result != null && result.Definition != null) {
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumOfHeadlines;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);
                }

                var t = [];
                t[t.length] = "<div id=\"alertCompaniesPanel\" class=\"tabPanel\">";

                if (result.ReturnCode != 0 && tEle) {
                    this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                    return;
                }
                var chartimage = result.Literals.ChartImage;
                if (result.Data != null && result.Data.Count > 0) {
                    for (var i = 0; i < result.Data.Count; i++) {
                        var _alert = result.Data.Alerts[i];
                        if (_alert.Status.Code == 0) {
                            t[t.length] = this.getAlertTitle(_alert,
                                result.Literals,
                                aColor,
                                titleFontSize,
                                aFontColor,
                                fontFamily);
                            if (_alert.CompaniesChart != null) {
                                t[t.length] = this.addDiscoveryChart(_alert.CompaniesChart.Chart, result.Literals.ChartImage);
                                if (result.Data.Alerts[i].IsGroupFolder == false) {
                                    if (result.Definition.DistributionType == 0 || result.Definition.DistributionType == 1)
                                        t[t.length] = this.addGetThisAlertUrl(_alert.CompaniesChart.GetThisAlertUrl
                                            , result.Literals.GetThisAlert
                                            , mColor);
                                }
                            } else if (result.Literals != null && result.Literals.NoResults != null) {
                                t[t.length] = this.getNoResultsPanel(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor,
                                    result.Literals.NoResults);
                            }
                        }
                    }
                }
                t[t.length] = "</div>";
                return t.join("");
            };

            this.getAlertSubjectsPanel = function (tEle, result, showTitle, integrateFontCSS) {
                if (result != null && result.Definition != null) {
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumOfHeadlines;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);
                }

                var t = [];
                t[t.length] = "<div id=\"alertSubjectsPanel\" class=\"tabPanel\">";

                if (result.ReturnCode != 0 && tEle) {
                    this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                    return;
                }

                if (result.Data != null && result.Data.Count > 0) {
                    for (var i = 0; i < result.Data.Count; i++) {
                        var _alert = result.Data.Alerts[i];
                        if (_alert.Status.Code == 0) {
                            t[t.length] = this.getAlertTitle(_alert,
                                result.Literals,
                                aColor,
                                titleFontSize,
                                aFontColor,
                                fontFamily);
                            if (_alert.SubjectsChart != null) {
                                t[t.length] = this.addDiscoveryChart(_alert.SubjectsChart.Chart, result.Literals.ChartImage);
                                if (result.Data.Alerts[i].IsGroupFolder == false) {
                                    if (result.Definition.DistributionType == 0 || result.Definition.DistributionType == 1)
                                        t[t.length] = this.addGetThisAlertUrl(_alert.SubjectsChart.GetThisAlertUrl
                                            , result.Literals.GetThisAlert
                                            , mColor);
                                }
                            } else if (result.Literals != null && result.Literals.NoResults != null) {
                                t[t.length] = this.getNoResultsPanel(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor,
                                    result.Literals.NoResults);
                            }
                        }
                    }
                }
                t[t.length] = "</div>";
                return t.join("");
            };

            this.getAlertIndustriesPanel = function (tEle, result, showTitle, integrateFontCSS) {
                if (result != null && result.Definition != null) {
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumOfHeadlines;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);
                }

                var t = [];
                t[t.length] = "<div id=\"alertIndustriesPanel\" class=\"tabPanel\">";

                if (result.ReturnCode != 0 && tEle) {
                    this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                    return;
                }

                if (result.Data != null && result.Data.Count > 0) {
                    for (var i = 0; i < result.Data.Count; i++) {
                        var _alert = result.Data.Alerts[i];
                        if (_alert.Status.Code == 0) {
                            t[t.length] = this.getAlertTitle(_alert,
                                result.Literals,
                                aColor,
                                titleFontSize,
                                aFontColor,
                                fontFamily);
                            if (_alert.IndustriesChart != null) {
                                t[t.length] = this.addDiscoveryChart(_alert.IndustriesChart.Chart, result.Literals.ChartImage);
                                if (result.Data.Alerts[i].IsGroupFolder == false) {
                                    if (result.Definition.DistributionType == 0 || result.Definition.DistributionType == 1)
                                        t[t.length] = this.addGetThisAlertUrl(_alert.IndustriesChart.GetThisAlertUrl
                                            , result.Literals.GetThisAlert
                                            , mColor);
                                }
                            } else if (result.Literals != null && result.Literals.NoResults != null) {
                                t[t.length] = this.getNoResultsPanel(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor,
                                    result.Literals.NoResults);
                            }
                        }
                    }
                }
                t[t.length] = "</div>";
                return t.join("");
            };

            this.getAlertExecutivesPanel = function (tEle, result, showTitle, integrateFontCSS) {
                if (result != null && result.Definition != null) {
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumOfHeadlines;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);
                }

                var t = [];
                t[t.length] = "<div id=\"alertExecutivesPanel\" class=\"tabPanel\">";

                if (result.ReturnCode != 0 && tEle) {
                    this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                    return;
                }

                if (result.Data != null && result.Data.Count > 0) {
                    for (var i = 0; i < result.Data.Count; i++) {
                        var _alert = result.Data.Alerts[i];
                        if (_alert.Status.Code == 0) {
                            t[t.length] = this.getAlertTitle(_alert,
                                result.Literals,
                                aColor,
                                titleFontSize,
                                aFontColor,
                                fontFamily);
                            if (_alert.ExecutivesChart != null) {
                                t[t.length] = this.addDiscoveryChart(_alert.ExecutivesChart.Chart, result.Literals.ChartImage);
                                if (result.Data.Alerts[i].IsGroupFolder == false) {
                                    if (result.Definition.DistributionType == 0 || result.Definition.DistributionType == 1)
                                        t[t.length] = this.addGetThisAlertUrl(_alert.ExecutivesChart.GetThisAlertUrl
                                            , result.Literals.GetThisAlert
                                            , mColor);
                                }
                            } else if (result.Literals != null && result.Literals.NoResults != null) {
                                t[t.length] = this.getNoResultsPanel(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor,
                                    result.Literals.NoResults);
                            }
                        }
                    } // for
                }
                t[t.length] = "</div>";
                return t.join("");
            };

            this.getAlertRegionsPanel = function (tEle, result, showTitle, integrateFontCSS) {
                if (result != null && result.Definition != null) {
                    var mColor = result.Definition.MainColor;
                    var mFontColor = result.Definition.MainFontColor;
                    var aColor = result.Definition.AccentColor;
                    var aFontColor = result.Definition.AccentFontColor;
                    var contentColor = result.Definition.ContentFontColor;
                    var snippetColor = result.Definition.SnippetFontColor;
                    var auxInfoColor = result.Definition.AuxInfoFontColor;
                    var bgColor = result.Definition.BackgroundColor;
                    var widgetName = result.Definition.Name;
                    var numHeadlines = result.Definition.NumOfHeadlines;
                    var fontFamily = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontFamilies[result.Definition.FontFamily];
                    var contentFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "inherit" : _fontSizes[result.Definition.FontSize].contentFontSize + "px";
                    var titleFontSize = (typeof (integrateFontCSS) === 'boolean' && integrateFontCSS == true) ? "larger" : _fontSizes[result.Definition.FontSize].titleFontSize + "px";
                    var showSnippet = (result.Definition.DisplayType == 1);
                }

                var t = [];
                t[t.length] = "<div id=\"alertRegionsPanel\" class=\"tabPanel\">";

                if (result.ReturnCode != 0 && tEle) {
                    this.xInnerHtml(tEle, result.ReturnCode + ": " + result.StatusMessage);
                    return;
                }

                if (result.Data != null && result.Data.Count > 0) {
                    for (var i = 0; i < result.Data.Count; i++) {
                        var _alert = result.Data.Alerts[i];
                        if (_alert.Status.Code == 0) {
                            t[t.length] = this.getAlertTitle(_alert,
                                result.Literals,
                                aColor,
                                titleFontSize,
                                aFontColor,
                                fontFamily);
                            if (_alert.RegionsChart != null) {
                                t[t.length] = this.addFlashImage(_alert.RegionsChart.Chart);
                                if (result.Data.Alerts[i].IsGroupFolder == false) {
                                    if (result.Definition.DistributionType == 0 || result.Definition.DistributionType == 1)
                                        t[t.length] = this.addGetThisAlertUrl(_alert.RegionsChart.GetThisAlertUrl
                                            , result.Literals.GetThisAlert
                                            , mColor);
                                }
                            } else if (result.Literals != null && result.Literals.NoResults != null) {
                                t[t.length] = this.getNoResultsPanel(bgColor,
                                    fontFamily,
                                    contentFontSize,
                                    contentColor,
                                    result.Literals.NoResults);
                            }
                        }
                    } // for
                }
                t[t.length] = "</div>";
                return t.join("");
            };

            this.getInnerContainerStyle = function (bgColor, fontFamily, fontSize, fontColor) {
                return "background-color:" + bgColor +
                    ";font-family:" + fontFamily +
                    ";font-size:" + fontSize +
                    ";color:" + fontColor +
                    ";font-weight:normal;overflow:hidden" +
                    ";padding:0 5px" +
                    ";position:relative;";
            };

            this.getWidgetTitle = function (widgetName, bgColor, fontSize, fontColor, fontFamily, icon) {
                var t = [];
                t[t.length] = "<b style=\"font-family:Arial\">";
                t[t.length] = "<b style=\"" +
                        "background-color:" + bgColor +
                        ";margin:0 4px" +
                        ";line-height:1px;height:1px;overflow:hidden;display:block;" +
                        "\"></b>";
                t[t.length] = "<b style=\"" +
                        "background-color:" + bgColor +
                        ";border-left: solid 2px " + bgColor +
                        ";border-right: solid 2px " + bgColor +
                        ";margin:0 2px" +
                        ";line-height:1px;height:1px;overflow:hidden;display:block;" +
                        "\"></b>";
                t[t.length] = "<b style=\"" +
                        "background-color:" + bgColor +
                        ";margin:0 1px" +
                        ";line-height:2px;height:2px;overflow:hidden;display:block;" +
                        "\"></b>";
                t[t.length] = "</b>";
                t[t.length] = "<div id=\"fctv_AlertWidgetTitle\" style=\"" +
                        "background-color:" + bgColor +
                        ";padding:3px 5px" +
                        ";padding-top:0px" +
                        ";color:" + fontColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:bold;\">";
                if (icon) {
                    t[t.length] = "<span style=\"margin-right:5px;\"><img src=\"" + icon + "\" style=\"position:relative;top:3px;\" /></span>";
                    t[t.length] = "<span>" + widgetName + "</span>";
                }
                else {
                    t[t.length] = widgetName;
                }
                t[t.length] = "</div>";
                return t.join("");
            };

            this.getAlertTitle = function (_alert, _literals, bgColor, fontSize, fontColor, fontFamily) {
                var t = [];

                t[t.length] = "<div id=\"fctv_AlertTitle\" style=\"" +
                    "position:relative;zoom:1" +
                    ";background-color:" + bgColor +
                    ";padding:3px 5px" +
                    ";color:" + fontColor +
                    ";font-size:" + fontSize +
                    ";font-family:" + fontFamily +
                    ";font-weight:bold" +
                    "\">";

                t[t.length] = ((!(xSfr3 && xMac)) ?
                    "<a href=\"" + _alert.ViewAllUri + "\" style=\"color:" + fontColor + "; text-decoration:none;\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" onclick=\"if(FactivaWidgetRenderManager && FactivaWidgetRenderManager.getInstance()) {FactivaWidgetRenderManager.getInstance().xWinOpen(this.href);return false;}\" title=\"" + _literals.ViewAll + "\">" + _alert.Name + "</a>" :
                    "<a href=\"" + _alert.ViewAllUri + "\" style=\"color:" + fontColor + "; text-decoration:none;\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" target=\"_new\" title=\"" + _literals.ViewAll + "\">" + _alert.Name + "</a>");
                /*
                t[t.length] = "<a href=\"javascript:void(0)\" style=\"color:" + fontColor + "; text-decoration:none;\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" onclick=\"alert(translate('featureIsDisabled'));return false;\" title=\"" + _literals.ViewAll + "\">" + _alert.Name + "</a>";
                */
                t[t.length] = "</div>";
                return t.join("");
            };

            this.getSectionTitle = function (_section, bgColor, fontSize, fontColor, fontFamily) {
                var t = [];

                if (_section.Type == "Sub") {
                    t[t.length] = "<div style=\"padding-left:20px;\"><div id=\"fctv_AlertTitle\" style=\"" +
                        "position:relative;zoom:1" +
                        ";background-color:" + bgColor +
                        ";padding:3px 5px" +
                        ";color:" + fontColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:bold" +
                        "\">";
                    t[t.length] = "<span style=\"color:" + fontColor + "\">" + _section.Name + "</span>";
                    t[t.length] = "</div>";
                } else {
                    t[t.length] = "<div id=\"fctv_AlertTitle\" style=\"" +
                        "position:relative;zoom:1" +
                        ";background-color:" + bgColor +
                        ";padding:3px 5px" +
                        ";color:" + fontColor +
                        ";font-size:" + fontSize +
                        ";font-family:" + fontFamily +
                        ";font-weight:bold" +
                        "\">";
                    t[t.length] = "<span style=\"color:" + fontColor + "\">" + _section.Name + "</span>";
                    t[t.length] = "</div>";
                }
                return t.join("");
            };

            this.getNext = function (ele, tarDivCntrId, numHeadlines) {
                var _tDiv = this.xGetElementById(tarDivCntrId);
                if (_tDiv) {
                    var _headlines = this.xGetElementsByClassName("fctv_headlineCont", _tDiv, "div");
                    if (_headlines && _headlines.length >= 1) {
                        var curPage = (typeof (_tDiv.CurPage) == "undefined") ? 1 : _tDiv.CurPage;
                        var tPages = Math.ceil(_headlines.length / numHeadlines);
                        var tIndex = this.findPageIndex(_headlines) + numHeadlines;
                        if (curPage >= tPages) {
                            tIndex = 0;
                            curPage = 1;
                        } else curPage++; for (var i = tIndex; (i < (tIndex + numHeadlines) && i < _headlines.length); i++) {
                            this.xDisplay(_headlines[i], "block");
                        }
                        _tDiv.CurPage = curPage;
                        this.updatePageText(_tDiv, tIndex, numHeadlines, _headlines.length);
                    }
                }
                // dynamic height resize hook for IGoogle portal pages
                if (typeof (_IG_AdjustIFrameHeight) != "undefined" && typeof (_IG_AdjustIFrameHeight) == "function") {
                    window.setTimeout("_IG_AdjustIFrameHeight()", 1);
                }
            };

            this.getPrevious = function (ele, tarDivCntrId, numHeadlines) {
                var _tDiv = this.xGetElementById(tarDivCntrId);
                if (_tDiv) {
                    var _headlines = this.xGetElementsByClassName("fctv_headlineCont", _tDiv, "div");
                    if (_headlines && _headlines.length >= 1) {
                        var curPage = (typeof (_tDiv.CurPage) == "undefined") ? 1 : _tDiv.CurPage;
                        var tPages = Math.ceil(_headlines.length / numHeadlines);
                        var tIndex = this.findPageIndex(_headlines) - numHeadlines;
                        if (curPage <= 1) {
                            tIndex = (tPages - 1) * numHeadlines;
                            curPage = tPages;
                        } else curPage--;
                        for (var i = tIndex; (i < (tIndex + numHeadlines) && i < _headlines.length); i++) {
                            this.xDisplay(_headlines[i], "block");
                        }
                        _tDiv.CurPage = curPage;
                        this.updatePageText(_tDiv, tIndex, numHeadlines, _headlines.length);
                    }
                }
                // dynamic height resize hook for IGoogle portal pages
                if (typeof (_IG_AdjustIFrameHeight) != "undefined" && typeof (_IG_AdjustIFrameHeight) == "function") {
                    window.setTimeout("_IG_AdjustIFrameHeight()", 1);
                }
            };

            this.updatePageText = function (tDiv, index, size, max) {
                var pCntrs = this.xGetElementsByClassName("pageText", tDiv, "span");
                if (pCntrs && pCntrs.length > 0) {
                    var token = "&nbsp;" + (index + 1) + "-" + ((index + size > max) ? max : index + size) + " of " + max + "&nbsp;";
                    for (var i = 0; i < pCntrs.length; i++) {
                        this.xInnerHtml(pCntrs[i], token)
                    }
                }
            };

            this.findPageIndex = function (headlines) {
                found = -1;
                for (var j = 0; j < headlines.length; j++) {
                    if (this.xDisplay(headlines[j]) == 'block') {
                        if (found == -1) {
                            found = j;
                        }
                    }
                    this.xDisplay(headlines[j], "none");
                }
                return found;
            };

            this.xViewAlertFolder = function (linkEle, alertEle, moreToken, lessToken, numHeadlines) {
                var _aEle = this.xGetElementById(alertEle);
                var _lEle = this.xGetElementById(linkEle);
                var _sAllEle = this.xGetElementById(_lEle.id.replace("v_", "s_"));
                var _divisor = 1;
                if (_aEle && _lEle) {
                    var _headlines = this.xGetElementsByClassName("fctv_headlineCont", _aEle, "div");
                    if (_headlines && _headlines.length >= 1) {
                        var IsMore = (typeof (_lEle.IsMore) == "undefined") ? true : _lEle.IsMore;
                        if (IsMore) {
                            // set the label
                            this.xHeight(_aEle, _divisor * this.xHeight(_aEle));
                            if (this.xDef(_aEle.style.overflowX)) {

                                _aEle.style.overflowX = "hidden";
                                _aEle.style.overflowY = "scroll";
                            }
                            else {
                                _aEle.style.overflow = "-moz-scrollbars-vertical";
                            }
                            this.xInnerHtml(_lEle, lessToken);
                            _lEle.title = lessToken;
                            _lEle.IsMore = false;
                            for (var j = 0; j < _headlines.length; j++) {
                                this.xDisplay(_headlines[j], "block");
                            }
                            // show the view all link
                            if (_sAllEle) {
                                this.xDisplay(_sAllEle, "inline");
                            }
                        }
                        else {
                            this.xHeight(_aEle, "auto");

                            if (this.xDef(_aEle.style.overflowX)) {
                                _aEle.style.overflowX = "hidden";
                                _aEle.style.overflowY = "hidden";
                            }
                            else {
                                _aEle.style.overflow = "hidden";
                            }
                            this.xInnerHtml(_lEle, moreToken);
                            _lEle.title = moreToken;
                            _lEle.IsMore = true;
                            for (var j = 0; j < _headlines.length; j++) {
                                var showHeadline = (j < numHeadlines);
                                (showHeadline) ? this.xDisplay(_headlines[j], "block") : this.xDisplay(_headlines[j], "none");
                            }
                            // show the view all link
                            if (_sAllEle) {
                                this.xDisplay(_sAllEle, "none");
                            }
                        }
                        // dynamic height resize hook for IGoogle portal pages
                        if (typeof (_IG_AdjustIFrameHeight) != "undefined" && typeof (_IG_AdjustIFrameHeight) == "function") {
                            window.setTimeout("_IG_AdjustIFrameHeight()", 1);
                        }
                    }
                }
            };

            this.getPagingBar = function (maxHeadlines, itemCount, _literals, bgColor, fontSize, fontColor, fontFamily, tarDivCntrId, _alert) {
                var randomnumber = Math.floor(Math.random() * 100001);
                var t = [];
                t[t.length] = "<div style=\"text-align:center; padding: 5px 0px;\">";
                t[t.length] = "<div id=\"fctv_PagingCntr" + randomnumber + "\" class=\"PagingCntr\" style=\"" +
                    "text-align:center;" +
                    ";color:" + fontColor +
                    ";font-size:" + fontSize +
                    ";font-family:" + fontFamily +
                    ";font-weight:normal" +
                    "\">";
                t[t.length] = "<a href=\"javascript:void(0)\" style=\"color:" + fontColor + "; text-decoration:none;\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" onclick=\"try{if(FactivaWidgetRenderManager && FactivaWidgetRenderManager.getInstance()) {FactivaWidgetRenderManager.getInstance().getPrevious(this,'" + tarDivCntrId + "'," + maxHeadlines + ");return false;}}catch(e){return false;}\" title=\"" + _literals.Previous + "\">\u00AB</a>";
                t[t.length] = "<span class=\"pageText\">&nbsp;1-" + maxHeadlines + " of " + itemCount + "&nbsp;</span>";
                t[t.length] = "<a href=\"javascript:void(0)\" style=\"color:" + fontColor + "; text-decoration:none;\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" onclick=\"try{if(FactivaWidgetRenderManager && FactivaWidgetRenderManager.getInstance()) {FactivaWidgetRenderManager.getInstance().getNext(this,'" + tarDivCntrId + "'," + maxHeadlines + ");return false;}}catch(e){return false;}\" title=\"" + _literals.Next + "\">\u00BB</a>";

                if (_alert) {

                    t[t.length] = "<span class=\"fPipe\">&nbsp;|&nbsp;</span>";
                    t[t.length] = ((!(xSfr3 && xMac)) ?
                    "<a href=\"" + _alert.ViewAllUri + "\" style=\"color:" + fontColor + "; text-decoration:none;\" onclick=\"if(FactivaWidgetRenderManager && FactivaWidgetRenderManager.getInstance()) {FactivaWidgetRenderManager.getInstance().xWinOpen(this.href);return false;}\" title=\"" + _literals.ViewAll + "\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\">" + _literals.ViewAll + "</a>" :
                    "<a href=\"" + _alert.ViewAllUri + "\" style=\"color:" + fontColor + "; text-decoration:none;\" target=\"_new\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\" title=\"" + _literals.ViewAll + "\">" + _literals.ViewAll + "</a>");
                    /*
                    t[t.length] = "<span class=\"fPipe\" style=\"color:#999999\">&nbsp;|&nbsp;</span><a href=\"javascript:void(0)\" style=\"color:" + fontColor + "; text-decoration:none;\" onclick=\"alert(translate('featureIsDisabled'));return false;\" title=\"" + _literals.ViewAll + "\" onmouseover=\"FactivaWidgetRenderManager.getInstance().setMouseOver(this);return false;\" onmouseout=\"FactivaWidgetRenderManager.getInstance().setMouseOut(this);return false;\">" + _literals.ViewAll + "</a>";
                    */
                }
                t[t.length] = "</div>";
                t[t.length] = "</div>";
                return t.join("");
            };

            this.moveLeftTab = function (index) {
                // target and container divs
                var _tDiv = this.xGetElementById("fctv_tabgroup" + index);
                var _cDiv = this.xGetElementById("fctv_tabgroup_outer" + index);

                var leftBtn = this.xGetElementById("fctv_tabLeft" + index);
                var rightBtn = this.xGetElementById("fctv_tabRight" + index);

                var _tDivRight = _tDiv.offsetLeft + _tDiv.clientWidth;
                var _cDivRight = _cDiv.clientWidth;

                // get the tabs within the target div
                var tabs = this.xGetElementsByTagName("a", _tDiv);
                var carouselStack = [];
                var rightMostTab;

                if (_tDiv) {
                    // check if the tab container's right pos is greater than the container
                    if (_tDivRight > _cDivRight) {

                        // add each tabs offset left to the carousel positioning stack
                        for (var k = 0; k < tabs.length; carouselStack.push(tabs[k++].offsetLeft));

                        if (_tDiv.leftCarIndex <= tabs.length - 1) {

                            var sum = 0;
                            for (var j = _tDiv.leftCarIndex + 1; j < tabs.length; j++) {
                                sum += tabs[j].clientWidth + 1;
                            }

                            if (sum < _cDivRight) {
                                _tDiv.edgeDistance = (_tDivRight - _cDivRight);
                                this.xLeft(_tDiv, _tDiv.offsetLeft - _tDiv.edgeDistance);
                                rightBtn.onclick = function () { return false; };
                                rightBtn.style.backgroundColor = "#DDDDDD";
                                leftBtn.style.backgroundColor = "#FFFFFF";
                                return;
                            }

                            if (_tDiv.edgeDistance == null) {
                                this.xLeft(_tDiv, -carouselStack[++_tDiv.leftCarIndex] + 1);
                                if (_tDiv.leftCarIndex > 0) {
                                    leftBtn.style.backgroundColor = "#FFFFFF";
                                }
                            }
                        }
                    }
                }
            };

            this.moveRightTab = function (index) {
                // target and container divs
                var _tDiv = this.xGetElementById("fctv_tabgroup" + index);
                var _cDiv = this.xGetElementById("fctv_tabgroup_outer" + index);

                var leftBtn = this.xGetElementById("fctv_tabLeft" + index);
                var rightBtn = this.xGetElementById("fctv_tabRight" + index);

                var _tDivLeft = _tDiv.offsetLeft;
                var _cDivLeft = _cDiv.offsetLeft;

                // get the tabs within the target div
                var tabs = this.xGetElementsByTagName("a", _tDiv);
                var carouselStack = [];

                if (_tDiv) {
                    // check if the tab container's left pos is less than the container
                    if (_tDivLeft < _cDiv.offsetLeft) {

                        for (var k = 0; k < tabs.length; carouselStack.push(tabs[k++].clientWidth));

                        if (_tDiv.leftCarIndex >= 0) {

                            if (_tDiv.edgeDistance != null && _tDiv.edgeDistance > 0) {
                                this.xLeft(_tDiv, _tDiv.offsetLeft + _tDiv.edgeDistance);
                                _tDiv.edgeDistance = null;
                                rightBtn.onclick = function () { FactivaWidgetRenderManager.getInstance().moveLeftTab(index); return false; };
                                rightBtn.style.backgroundColor = "#FFFFFF";
                                if (_tDiv.leftCarIndex <= 0) {
                                    leftBtn.style.backgroundColor = "#DDDDDD";
                                }
                                return;
                            }

                            if (_tDiv.edgeDistance == null) {
                                var temp = _tDiv.leftCarIndex - 1;
                                if (temp == 0)
                                    this.xLeft(_tDiv, _tDiv.offsetLeft + carouselStack[--_tDiv.leftCarIndex] + 1);
                                else
                                    this.xLeft(_tDiv, _tDiv.offsetLeft + carouselStack[--_tDiv.leftCarIndex] + 2);

                                if (_tDiv.leftCarIndex <= 0) {
                                    leftBtn.style.backgroundColor = "#DDDDDD";
                                }
                            }

                            if (_tDiv.leftCarIndex < 0) _tDiv.leftCarIndex = 0;
                        }
                    }
                }
            };

            this.setMouseOut = function (ele) {
                this.xGetElementById(ele).style.textDecoration = "none";
            };

            this.setMouseOver = function (ele) {
                this.xGetElementById(ele).style.textDecoration = "underline";
            };

            // color util functions
            this.RGBToHSL = function (rgb) {
                var min, max, delta, h, s, l;
                var r = rgb[0], g = rgb[1], b = rgb[2];

                min = Math.min(r, Math.min(g, b));
                max = Math.max(r, Math.max(g, b));

                delta = max - min;
                l = (min + max) / 2;
                s = 0;
                if (l > 0 && l < 1) {
                    s = delta / (l < 0.5 ? (2 * l) : (2 - 2 * l));
                }
                h = 0;
                if (delta > 0) {
                    if (max == r && max != g) h += (g - b) / delta;
                    if (max == g && max != b) h += (2 + (b - r) / delta);
                    if (max == b && max != r) h += (4 + (r - g) / delta);
                    h /= 6;
                }
                return [h, s, l];
            };

            this.unpack = function (color) {
                if (color.length == 7) {
                    return [parseInt('0x' + color.substring(1, 3)) / 255,
                    parseInt('0x' + color.substring(3, 5)) / 255,
                    parseInt('0x' + color.substring(5, 7)) / 255];
                }
            };
            // end color util functions

            this.getAlertError = function (folderId, folderName, status, accentColor, fontFamily, fontSize, fontColor, bgColor) {
                switch (status.Code) {
                    case 31065:
                        return "";
                    default:
                        return "<div id=\"fctv_AlertTitle\" style=\"" +
                            "position:relative;zoom:1" +
                            ";background-color:" + accentColor +
                            ";padding:3px 5px" +
                            ";color:" + fontColor +
                            ";font-size:" + fontSize +
                            ";font-family:" + fontFamily +
                            ";font-weight:bold" +
                            "\">" + ((typeof (folderName) == "undefined" || folderName == null) ? translate("error") : folderName) + "</div><div id=\"fctv_Error\" style=\"" +
                            "background-color:" + bgColor +
                            ";padding:3px 5px" +
                            ";color:" + fontColor +
                            ";font-size:" + fontSize +
                            "\">" +
                        /*"<div>" + status.Code + "</div>" +*/
                            "<div style=\"font-weight:normal;padding:3px 5px\">" + status.Message + "</div></div>";
                }
            };

            // Rendering of Automatic WorkspaceWidget
            this.xBuildAutomaticWidget = function (tEle, result, showTitle) {
                if (result != null && result.Definition != null) {
                    var sb = [];
                }
            };
        }

        return new function () {
            this.getInstance = function () {
                if (instance == null) {
                    instance = new PrivateConstructor();
                    instance.constructor = null;
                }
                return instance;
            };
        };
    })();
};
var DiscoveryChart = (function (chartimage) {

    var templateSettings = {
        evaluate: /\{\{([\s\S]+?)\}\}/g,
        interpolate: /\{\{=([\s\S]+?)\}\}/g,
        encode: /\{\{!([\s\S]+?)\}\}/g,
        use: /\{\{#([\s\S]+?)\}\}/g, //compile time evaluation
        define: /\{\{##\s*([\w\.$]+)\s*(\:|=)([\s\S]+?)#\}\}/g, //compile time defs
        conditionalStart: /\{\{\?([\s\S]+?)\}\}/g,
        conditionalEnd: /\{\{\?\}\}/g,
        varname: 'it',
        strip: true,
        append: true
    };

    function resolveDefs(c, block, def) {
        return ((typeof block === 'string') ? block : block.toString())
		.replace(c.define, function (match, code, assign, value) {
		    if (code.indexOf('def.') === 0) {
		        code = code.substring(4);
		    }
		    if (!(code in def)) {
		        if (assign === ':') {
		            def[code] = value;
		        } else {
		            eval("def[code]=" + value);
		        }
		    }
		    return '';
		})
		.replace(c.use, function (match, code) {
		    var v = eval(code);
		    return v ? resolveDefs(c, v, def) : v;
		});
    }

    function GetCharttemplate(tmpl, c, def) {
        debugger;
        c = c || templateSettings;
        var cstart = c.append ? "'+(" : "';out+=(", // optimal choice depends on platform/size of templates
		    cend = c.append ? ")+'" : ");out+='";
        var str = (c.use || c.define) ? resolveDefs(c, tmpl, def || {}) : tmpl;

        str = ("var out='" +
			((c.strip) ? str.replace(/\s*<!\[CDATA\[\s*|\s*\]\]>\s*|[\r\n\t]|(\/\*[\s\S]*?\*\/)/g, '') : str)
			.replace(/\\/g, '\\\\')
			.replace(/'/g, "\\'")
			.replace(c.interpolate, function (match, code) {
			    return cstart + code.replace(/\\'/g, "'").replace(/\\\\/g, "\\").replace(/[\r\t\n]/g, ' ') + cend;
			})
			.replace(c.encode, function (match, code) {
			    return cstart + code.replace(/\\'/g, "'").replace(/\\\\/g, "\\").replace(/[\r\t\n]/g, ' ') + ").toString().replace(/&(?!\\w+;)/g, '&#38;').split('<').join('&#60;').split('>').join('&#62;').split('" + '"' + "').join('&#34;').split(" + '"' + "'" + '"' + ").join('&#39;').split('/').join('&#47;'" + cend;
			})
			.replace(c.conditionalEnd, function (match, expression) {
			    return "';}out+='";
			})
			.replace(c.conditionalStart, function (match, expression) {
			    var code = "if(" + expression + "){";
			    return "';" + code.replace(/\\'/g, "'").replace(/\\\\/g, "\\").replace(/[\r\t\n]/g, ' ') + "out+='";
			})
			.replace(c.evaluate, function (match, code) {
			    return "';" + code.replace(/\\'/g, "'").replace(/\\\\/g, "\\").replace(/[\r\t\n]/g, ' ') + "out+='";
			})
			+ "';return out;")
			.replace(/\n/g, '\\n')
			.replace(/\t/g, '\\t')
			.replace(/\r/g, '\\r')
			.split("out+='';").join('')
			.split("var out='';out+=").join('var out=');

        try {
            return new Function(c.varname, str);
        } catch (e) {
            if (typeof console !== 'undefined') console.log("Could not create a template function: " + str);
            throw e;
        }
    };


    var template = ['<ul class="discovery-items" style="line-height: 13px;zoom: 1;list-style: none;">',
                            '{{ var data = it, mw = 1;',
                                'for (var x=0, w=0, c=0, i = 0, len = data.length;i < len; i++) { ',
                                'x = data[i];w=((i != 0)?((x.value/data[0].value)*170):170); if(w < mw) w=mw; c = x.GT=="sf"? "cItem source-family":"cItem";}}',
                                '<li class="{{=c}}" style="position: relative;margin-bottom: 3px;padding-bottom: 6px;width: 186px;padding-left: 25px;height: 13px;" data-di="{{=i}}" title="{{=x.name }}">', //background change for source family 
                                    '<span class="dj_not" style=" width: 12px;height: 12px;display: block;position: absolute;top: 6px;left: 8px; display: none; cursor: pointer;"title="{{="notTitleTkn"}}"><span></span></span>',
                                    '<span class="discovery-chart" style="display: -moz-inline-stack;display: inline-block;zoom: 1;vertical-align: top;position: relative;">',
                                        '<img  class="plot" src="' + chartimage + '" style="height:4px;zoom: 1;vertical-align: top;width: {{=w}}px;" />', '<br />',
                                        '<span class="ellipsis" style="width: 140px;color: #004c70;text-align: left;margin-right: 3px;margin-top: 2px;display: -moz-inline-stack;display: inline-block;zoom: 1;hasLayout: 1;white-space:nowrap;text-overflow:ellipsis;overflow:hidden;">{{=x.name}}</span>',
                                        '<span class="chart-value" style="color: #999;margin-top: 2px;font-size: 10px;text-align: right;display: inline-block;clear: right;zoom: 1;height: 12px;">{{=x.value}}</span>',
                                    '</span>',
                                '</li>',
                            '{{ } }}',
                        '</ul>',
                        '<span class="morebutton"></span>',
                        '<span class="lessbutton lessbutton-inactive"></span>'
    //Pramod loading symbol if required handle it.
    // , '<span class="loading hide"><img src="../ChartPOC/img/ajax-loader-flat.gif" /></span>'
		    ].join('');
    var templateIE = ['<ul class="discovery-items" style="line-height: 13px;zoom: 1;list-style: none;">',
                            '{{ var data = it, mw = 1;',
                                'for (var x=0, w=0, c=0, i = 0, len = data.length;i < len; i++) { ',
                                'x = data[i];w=((i != 0)?((x.value/data[0].value)*170):170); if(w < mw) w=mw; c = x.GT=="sf"? "cItem source-family":"cItem";}}',
                                '<li class="{{=c}}" style="position: relative;margin-bottom: 3px;padding-bottom: 6px;width: 186px;padding-left: 25px;height: 13px;" data-di="{{=i}}" title="{{=x.name }}">', //background change for source family 
                                    '<span class="dj_not" style=" width: 12px;height: 12px;display: block;position: absolute;top: 6px;left: 8px; display: none; cursor: pointer;"title="{{="notTitleTkn"}}"><span></span></span>',
                                    '<span class="discovery-chart" style="display: -moz-inline-stack;display: inline-block;zoom: 1;vertical-align: top;position: relative;">',
                                        '<img  class="plot" src="' + chartimage + '" style="float:left;height:4px;zoom: 1;vertical-align: top;width: {{=w}}px;" />', '<br />',
                                        '<span class="ellipsis" style="float:left;margin-bottom:2px;width: 140px;color: #004c70;text-align: left;margin-right: 3px;margin-top: 2px;display: -moz-inline-stack;display: inline-block;zoom: 1;hasLayout: 1;white-space:nowrap;text-overflow:ellipsis;overflow:hidden;">{{=x.name}}</span>',
                                        '<span class="chart-value" style="color: #999;margin-top: 2px;font-size: 10px;text-align: right;display: inline-block;clear: right;zoom: 1;height: 12px;">{{=x.value}}</span>',
                                    '</span>',
                                '</li>',
                            '{{ } }}',
                        '</ul>',
                        '<span class="morebutton"></span>',
                        '<span class="lessbutton lessbutton-inactive"></span>'
    //Pramod loading symbol if required handle it.
    // , '<span class="loading hide"><img src="../ChartPOC/img/ajax-loader-flat.gif" /></span>'
		    ].join('');

    return {
        RenderDiscoveryChart: function (chartdata, isIE) {
            debugger;
            var parsedTemplate;
            if (isIE) {
                parsedTemplate = GetCharttemplate(templateIE);
            }
            else {
                parsedTemplate = GetCharttemplate(template);
            }
            return (parsedTemplate(chartdata));
            // var $container = container.html(parsedTemplate(sampleJson));

        }
    };
});