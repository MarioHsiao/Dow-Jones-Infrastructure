//!
// DowJones Common
//


// Simple JavaScript Inheritance
// By John Resig http://ejohn.org/blog/simple-javascript-inheritance/
// MIT Licensed.
//

/// <reference path="jquery.js" />
(function () {
    var initializing = false, fnTest = /xyz/.test(function () { xyz; }) ? /\b_super\b/ : /.*/;  //ignore jslint
    // The base Class implementation (does nothing)
    this.Class = function () { };

    // Create a new Class that inherits from this class
    Class.extend = function newClass(prop) {
        var _super = this.prototype;

        // Instantiate a base class (but only create the instance,
        // don't run the init constructor)
        initializing = true;
        var prototype = new this();
        initializing = false;

        prop.templates = prop.templates || {};

        // Copy the properties over onto the new prototype
        for (var name in prop) {
            // Check if we're overwriting an existing function
            prototype[name] =
                   typeof prop[name] === "function" && 
                   typeof _super[name] === "function" && 
                   fnTest.test(prop[name]) ? (function (name, fn) {
                        return function () {
                            var tmp = this._super;

                            // Add a new ._super() method that is the same method
                            // but on the super-class
                            this._super = _super[name];

                            // The method only need to be bound temporarily, so we
                            // remove it when we're done executing
                            var ret = fn.apply(this, arguments);
                            this._super = tmp;

                            return ret;
                        };
                    }(name, prop[name])) : prop[name];
        }

        // The dummy class constructor
        function Class() {
            try {
                // All construction is actually done in the init method
                if (!initializing && this.init) {
                    this.init.apply(this, arguments);
                }
            } catch (ex) {
                if (console && console.log) {
                    console.log('**** Error initializing component! ****', this, ex);
                }
            }
        }

        // Populate our constructed prototype object
        Class.prototype = prototype;

        // Enforce the constructor to be what we expect
        Class.constructor = Class;

        // And make this class extendable
        Class.extend = newClass;

        return Class;
    };
}());

//  
///  Global jQuery Extensions
//
(function ($) {

    //  The "inheritance plugin" model
    //  http://alexsexton.com/?p=51
    // Modified by Framework team
    $.plugin = function (name, object) {
        $.fn[name] = function (options) {
            var instance = $.data(this[0], name, new object(this[0], options));
            return instance;
        };
    };

    // Custom :data() selector
    $.expr[':'].data = function(elem, counter, params) {
        if(!elem || !params) 
            return false;

        var query = params[3];
        if(query) 
        {
            var split = query.split('=');

            var data = $(elem).data(split[0]);
            if(data) {
                // If the query was just checking to see if the
                // field existed, then we're good!
                if(split.length == 1) 
                    return true;
                
                return (data+'') == split[1];
            }
        }

        return false;
    };

    $.extend($.fn, {

        filterByData: function(key, value) {
            return this.filter([':data("', key, '=', value, '")'].join('')); 
        },
        
        findComponent: function (componentTypeOrName) {
        /// <summary>
        ///     Finds a View Component's instance given the Type or the plugin name.
        /// </summary> 
        /// <param name="componentTypeOrName" type="String|DJ.UI.Component">     
        ///     An expression to search with.     
        /// </param> 
        /// <example>
        ///     
        ///     // Create a view component such as PortalHeadlineList
        ///     DJ.UI.PortalHeadlineList = DJ.UI.Component.extend({ ... });
        ///     $.plugin('dj_PortalHeadlineList', DJ.UI.PortalHeadlineList);
        ///
        ///     // returns the ViewComponent object by the type
        ///     $('#PortalList1').findComponent(DJ.UI.PortalHeadlineList);
        ///        
        ///     // returns the ViewComponent object by plugin name
        ///     $('#PortalList1').findComponent('dj_PortalHeadlineList');
        ///
        /// </example>
            return ($dj.isString(componentTypeOrName) ? 
                this._getComponent(componentTypeOrName) : this._findComponent(componentTypeOrName));
        },

        _findComponent: function (componentType) {
            var component = null;

            try {
                $.each(this.data() || [], function(i, datum) {
                    if(component !== null) { return; }

                    if(datum instanceof componentType) {
                        component = datum;
                    }
                });
            } catch (e) {
            }

            return component;
        },

        _getComponent: function (pluginName) {
            return this.data(pluginName);
        },

        dj_dropDownMenu: function (originalOptions, handler) {
            var options = $.extend({
                trigger: $('.trigger', this),
                menu: $(this)
            }, originalOptions);

            var menu = $(options.menu);

            if (menu.length === 0) {
                throw "Cannot initialize menu without a target menu element";
            }

            menu
                .css({ 'position': 'fixed', "z-index": '100' })
                .mouseleave(function () { $(this).hide("fast"); })
                .click(function () { $(this).hide("fast"); });

            // add click and mouse-over events
            $("li", menu).each(function (i, menuItem) {
                var $menuItem = $(menuItem);
                var command = $menuItem.data('command');

                // If no command was supplied via data, use the href
                if (!command) {
                    var href = $('a', $menuItem).attr('href');
                    if (href && href[0] === '#') {
                        command = href.substring(1);
                        $menuItem.data('command', command);
                        $('a', $menuItem).attr('href', 'void(0)');
                    }
                }

                $menuItem
                    .hover(
                        function (e) { $(this).addClass("dj_mouseover"); },
                        function (e) { $(this).removeClass("dj_mouseover"); }
                    )
                    .click(function (e) {
                        if (handler) { handler(command, $menuItem); }
                        else { $dj.debug('Menu command (with no handler): ' + command); }
                    });
            });

            $(options.trigger).click(function () {
                var offset = $(this).offset() || {};
                var w = (offset.left - menu.outerWidth(true) + $(this).outerWidth(true)) + "px";
                var h = offset.top + $(this).height() + "px";

                menu.css({ "left": w, "top": h }).slideToggle("fast");
            });

            return $(this);
        },


        dj_snippetTooltip: function (className, containerName) {
            return this.each(function () {
                var text = $(this).attr("title");
                $(this).attr("title", "");
                if (text !== undefined && text !== "") {
                    $(this).hover(function (e) {
                        var parentTag = $(this).closest("." + containerName).get(0);
                        $(this).attr("title", "");
                        if (parentTag && $(parentTag).data("options").displaySnippets === "hover" || $(parentTag).data("options").displaySnippets === "inline") {
                            $("body").append("<div id='dj_snippetTooltip' class=\"" + className + "\" style='position: absolute; z-index: 100; display: none;'>" + text + "</div>");
                            var $tObj = $("#dj_snippetTooltip");
                            var cRight = e.pageX + 12 + $tObj.outerWidth() - $dj.getHorizontalScroll();
                            var cBottom = e.pageY + 12 + $tObj.outerHeight() - $dj.getVerticalScroll();
                            var tipX = e.pageX + 12;
                            var tipY = e.pageY + 12;
                            if (cRight >= $dj.getClientWidth()) {
                                tipX = e.pageX - 12 - $tObj.outerWidth();
                            }
                            if (cBottom >= $dj.getClientHeight()) {
                                tipY = e.pageY - 12 - $tObj.outerHeight();
                            }

                            $tObj.css("left", tipX).css("top", tipY).css("z-index", "1000000");
                            $tObj.stop(true, true).fadeIn("fast");
                        }

                    }, function (e) {
                        $("#dj_snippetTooltip").remove();
                        $(this).attr("title", text);
                        e.stopPropagation();
                    });
                    $(this).mousemove(function (e) {
                        var $tObj = $("#dj_snippetTooltip");
                        var cRight = e.pageX + 12 + $tObj.outerWidth() - $tObj.scrollLeft();
                        var cBottom = e.pageY + 12 + $tObj.outerHeight() - $tObj.scrollTop();
                        var tipX = e.pageX + 12;
                        var tipY = e.pageY + 12;
                        if (cRight >= $(window).width()) {
                            tipX = e.pageX - 12 - $tObj.outerWidth();
                        }
                        if (cBottom >= $(window).height()) {
                            tipY = e.pageY - 12 - $tObj.outerHeight();
                        }
                        $tObj.css("left", tipX).css("top", tipY);
                    });
                }
            });
        },

        // Simple Tooltip plugin
        dj_simpleTooltip: function (className) {
            return this.each(function () {
                var sText = $(this).attr("title");
                $(this).attr("title", "");
                if (sText !== undefined) {
                    $(this).hover(function (e) {
                        var enable = $(this).data('enableSimpleTooltip');
                        if (enable === undefined || enable === true) {
                            $(this).attr("title", "");
                            var tipX = e.pageX + 12;
                            var tipY = e.pageY + 12;
                            var tObj = $("#dj_tooltip").get(0);
                            var $tObj = null;
                            if (tObj) {
                                $(tObj).removeClass()
                               .addClass(className)
                               .html(sText);
                            }
                            else {
                                $("body").append("<div id='dj_tooltip' class=\"" + 
                                                className + "\" style='position: absolute; z-index: 100; display: none;'><div class=\"border\">" + 
                                                sText + "</div></div>");
                            }

                            $tObj = $("#dj_tooltip");
                            $tObj.data("triggeringElement", $(this));
                            if ($.fn.bgiframe) {
                                $tObj.bgiframe();
                            }
                            var cRight = e.pageX + 12 + $tObj.outerWidth() - $dj.getHorizontalScroll();
                            var cBottom = e.pageY + 12 + $tObj.outerHeight() - $dj.getVerticalScroll();
                            tipX = e.pageX + 12;
                            tipY = e.pageY + 12;
                            if (cRight >= $dj.getClientWidth()) {
                                tipX = e.pageX - 12 - $tObj.outerWidth();
                            }
                            if (cBottom >= $dj.getClientHeight()) {
                                tipY = e.pageY - 12 - $tObj.outerHeight();
                            }
                            $tObj.css("left", tipX).css("top", tipY).css("z-index", "1000000");
                            $tObj.stop(true, true).fadeIn("fast");
                        }
                    }, function (e) {
                        var enable = $(this).data('enableSimpleTooltip');
                        if (enable === undefined || enable === true) {
                            $("#dj_tooltip").stop(true, true).fadeOut("fast");
                            $(this).attr("title", sText);
                        }
                    });
                    $(this).mousemove(function (e) {
                        var enable = $(this).data('enableSimpleTooltip');
                        if (enable === undefined || enable === true) {
                            var $tObj = $("#dj_tooltip");
                            if ($tObj && $tObj.length > 0) {
                                var cRight = e.pageX + 12 + $tObj.outerWidth() - $dj.getHorizontalScroll();
                                var cBottom = e.pageY + 12 + $tObj.outerHeight() - $dj.getVerticalScroll();
                                var tipX = e.pageX + 12;
                                var tipY = e.pageY + 12;
                                if (cRight >= $dj.getClientWidth()) {
                                    tipX = e.pageX - 12 - $tObj.outerWidth();
                                }
                                if (cBottom >= $dj.getClientHeight()) {
                                    tipY = e.pageY - 12 - $tObj.outerHeight();
                                }
                                $tObj.css("left", tipX).css("top", tipY);
                            }
                        }
                    });
                }
            });
        },

        showLoading: function () {
            var loadingDiv = this._getLoading();
            try {
                loadingDiv.css({ "height": this.outerHeight(), "width": this.outerWidth(), "left": 0, "top": 0 }).show();
                var l = this.offset().left - loadingDiv.offset().left, t = this.offset().top - loadingDiv.offset().top;
                loadingDiv.css({ "left": l, "top": t });
            }
            catch (e) {
                if (loadingDiv) {
                    loadingDiv.hide();
            }
            }
        },

        hideLoading: function () {
            var loadingDiv = this._getLoading();
            if(loadingDiv) {
                loadingDiv.hide();
            }
        },

        _getLoading: function() {
            var loadingDiv = this.data('loadingDiv');
            if (!loadingDiv) {
                loadingDiv = $('.loadingProgress', this);

                if(loadingDiv.length == 0)
                    loadingDiv = $('<div class="loadingProgress"></div>').appendTo(this);

                this.data('loadingDiv', loadingDiv);
            }

            return loadingDiv;
        },

        waterMark: function (options) {
            var $this = null;
            return this.each(function () {
                $this = $(this);
                if ($this.val() === options.waterMarkText || $this.val() === "") {
                    $this.addClass(options.waterMarkClass).val(options.waterMarkText);
                }

                $this.focus(function () {
                    $(this).filter(function () {
                        return $(this).val() === "" || $(this).val() === options.waterMarkText;
                    }).val("").removeClass(options.waterMarkClass);

                })
                .blur(function () {
                    $(this).filter(function () {
                        return $(this).val() === "";
                    })
                    .addClass(options.waterMarkClass)
                    .val(options.waterMarkText);
                });

            });
        }

    });

}(jQuery));


//
// $dj:  Custom Global Functions
//
(function ($) {

    var $dj = {

        callback: function(handler, context) {
            var callbackName = handler;

            if($.isFunction(handler)) {

                if(context) {
                    handler = $dj.delegate(context, handler);
                }

                callbackName = 'dj_'+jQuery.expando;
                window[ callbackName ] = handler;
            }

            return callbackName;
        },

        clone: function(source) {
            return $.extend(true, {}, source);
        },

        dateFormat: {
            MMDDCCYY: 0,
            DDMMCCYY: 1,
            CCYYMMDD: 2
        },

        debug: function(text) {
            if(!this.debugEnabled || !window) {
                return;
            }

            if (window.console && window.console.log) {
                if('function' === typeof window.console.log) {
                    window.console.log.apply(window.console, arguments);
                }
                else {
                    window.console.log([].slice.call(arguments).join(' '));
                }
            }
            if (window.opera) {
                window.opera.postError.apply(window.opera, arguments);
            }
            if (window.debugService) {
                window.debugService.trace.apply(window.debugService, arguments);
            }
        },

        debugEnabled: window['$dj_debugEnabled'] === true,


        delegate: function (context, handler, customData) {
            if(!handler) {
                if($.isFunction(context))
                    handler = context;
                else
                    $dj.debug('Invalid delegate handler');
            };

            return function() {
                var args = (customData !== undefined) ? [].concat(customData, [].slice.call(arguments)) : arguments;
                return handler.apply(context, args);
            };
        },


        registerNamespace: function (namespacePath) {
            var parts = namespacePath.split('.');

            var ns = window;
            for (var i = 0; i < parts.length; i++) {
                if (!ns[parts[i]]) {
                    ns[parts[i]] = {};
                }
                ns = ns[parts[i]];
            }

            $dj.debug('Registered namespace: ' + namespacePath);
        },


        trim: function(str, doNotRemoveSpecialChars) {
            if (!doNotRemoveSpecialChars) {
                for (var i = 0; i < str.length; i++) {
                    if (str.charCodeAt(i) <= 32) {
                        str = str.substring(0, i) + " " + str.substr(i + 1);
                    }
                }
            }
            str = str.replace(/^[\s]+/g, "");
            str = str.replace(/[\s]+$/g, "");
            return str;
        },

        getClientWidth: function() {
                var v = 0, d = document, w = window;
                if ((!d.compatMode || d.compatMode === 'CSS1Compat') && d.documentElement && d.documentElement.clientWidth)
                { v = d.documentElement.clientWidth; }
                else if (d.body && d.body.clientWidth)
                { v = d.body.clientWidth; }
                else if (this.def(w.innerWidth, w.innerHeight, d.height)) {
                    v = w.innerWidth;
                    if (d.height > w.innerHeight) { v -= 16; }
                }
                return v;
            },

        getClientHeight: function() {
                var v = 0, d = document, w = window;
                if ((!d.compatMode || d.compatMode === 'CSS1Compat') && d.documentElement && d.documentElement.clientHeight)
                { v = d.documentElement.clientHeight; }
                else if (d.body && d.body.clientHeight)
                { v = d.body.clientHeight; }
                else if (this.def(w.innerWidth, w.innerHeight, d.width)) {
                    v = w.innerHeight;
                    if (d.width > w.innerWidth) { v -= 16; }
                }
                return v;
            },

        getHorizontalScroll: function() {
                var w = window, d = window.document;
                var offset = 0;
                if (w.pageXOffset) { // All but IE
                    offset = w.pageXOffset;
                }
                else if (d.documentElement &&  // IE6 w/ doctype
                         d.documentElement.scrollLeft) {
                    offset = d.documentElement.scrollLeft;
                }
                else if (d.body.scrollLeft) { // IE4,5,6(w/o doctype)
                    offset = d.body.scrollLeft;
                }
                if (this.isNum(offset)) {
                    return offset;
                }
                return 0;
            },

        getVerticalScroll: function() {
                var w = window, d = window.document;
                var offset = 0;
                if (w.pageYOffset) { // All but IE
                    offset = w.pageYOffset;
                }
                else if (d.documentElement &&  // IE6 w/ doctype
                        d.documentElement.scrollTop) {
                    offset = d.documentElement.scrollTop;
                }
                else if (d.body.scrollTop) { // IE4,5,6(w/o doctype)
                    offset = d.body.scrollTop;
                }
                if (this.isNum(offset)) {
                    return offset;
                }
                return 0;
            },






        isNum: function() {
                for (var i = 0; i < arguments.length; ++i) { 
                    if (isNaN(arguments[i]) || typeof (arguments[i]) !== 'number') {
                        return false; 
                    }
                }
                return true;
            },

        htmlEncode: function(txt) {
                return $("<div/>").text(txt).html();
            },

        htmlDecode: function(html) {
                return $("<div/>").html(html).text();
            },

        timer: function(interval, callback, originalOptions) {
                var options = $.extend({ reset: 500, _isStopped: false, _timerID: null, _userCallback: callback }, originalOptions); // Create options for the default reset value
                interval = interval || options.reset;

	                    if(!callback) { return false; }

                var timer = function(interval, callback) {		                    
                            // Only used by internal code to call the callback
		                    this.internalCallback = function() { 
                                // Invoke the user-defined callback
                                if (options._userCallback !== null) {
                                    options._userCallback();
                                }    
                             };

                            this.start = function(){
                                // Set the interval time
		                        this.interval = interval;
		                        options._timerID = window.setTimeout(this.internalCallback, this.interval);
                                options._isStopped = false;                                
                            };

		                    // Clears any timers
		                    this.stop = function() {
                                window.clearTimeout(options._timerID);
                                options._timerID = null;
                                options._isStopped = true;
                             };

                            //Check whether the timer is stopped
                            this.isStopped = function(){return options._isStopped;};		                    
	                    };

	                    // Create a new timer object
	                    return new timer(interval, callback);
                    },
        
        isString: function(val) {
            return val && (val.constructor === String);
        },
        
        replace: function(el, regex, replacement) {
            var current = $(el).val() || '';
            var updated = current.replace(regex, replacement || '');
            
            $(el).val(updated);
            
            return updated;
        },

        sanitizeJsonString: function (jsonString) {
            if(!$dj.isString(jsonString)) { throw 'Input is not a string'; }

            return jsonString.replace('\"', '\\\"');
        },

        serializeGlobalHeaders: function () {
            var params = $dj.globalHeaders;
        
            if (!params) { return; }

            var headers = {
                credentials: JSON.stringify(params.credentials),
                preferences: JSON.stringify(params.preferences),
                product: params.productId
            };

            if (params.credentials.Debug) { headers.debug = true; }

            return headers;
        },

        queryParameter: function (name) {
            var match = new RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        },

        hasIllegalChar: function (str) {
            if (str) {
                return str.match(/[<>&#\\%+|]/);
            }
            return true;
        },

        validateEmail: function (email) {
            //	        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
            //	        if( !email || ($.trim(email).length > 80) || !emailReg.test(email) ) {
            //		        return false;
            //	        } else {
            //		        return true;
            //	        }

            //To keep it consistent with dotcom and other products use the below validation login for email.
            return ($.trim(email).length <= 80 &&
			        email.indexOf("@") !== -1 &&
			        email.indexOf(".") !== -1 &&
			        email.indexOf("@") === email.lastIndexOf("@") &&
			        email.indexOf(" ") === -1);
        },

        hideSimpleTooltip: function () {
            try {
                var $tObj = $("#dj_tooltip");
                if ($tObj.length && $tObj.is(":visible")) {
                    $tObj.data("triggeringElement").attr("title", $tObj.hide().html());
                }
            }
            catch (e) { }
        },

        progressIndicator: {
            display: function (strMessage, hideProgressBar, fadeInTime) {
                strMessage = strMessage || '<%= Token("loading") %>...';

                var firstTime = $dj.progressIndicator.init();
                var progress = $('#dj_progressIndicator');
                progress.data("options", arguments);
                
                progress.find('td').css("white-space", "nowrap").html(strMessage);

                if (!firstTime) {
                    progress.parent().show().css({ "visible": "hidden", "width": "auto" });
                }

                var width = progress.width();
                if (width < 100) {
                    progress.width(100);
                }
                else {
                    progress.width(width);
                }

                if (!firstTime) {
                    progress.parent().hide();
                }

                if (!fadeInTime) {
                    fadeInTime = 0;
                }

                progress.overlay({ background: false, closeOnEsc: true, fadeInTime: fadeInTime, fadeOutTime: 0, hideSelect: false });
            },
            message: function (strMessage) {
                if ($('#dj_progressIndicator').length > 0) {
                    var options = $('#dj_progressIndicator').data("options");
                    options[0] = strMessage;
                    this.display.apply(this, options);
                }
                else {
                    this.display(strMessage);
                }
            },
            hide: function (delayTimer) {
                if (delayTimer) {
                    window.setTimeout(function () {
                        $().overlay.hide('#dj_progressIndicator');
                    }, delayTimer);
                }
                else {
                    $().overlay.hide('#dj_progressIndicator');
                }
            },
            init: function () {
                if ($('#dj_progressIndicator').size() < 1) {
                    var arrHtml = '<div class="loadingProgress progressIndicator" id="dj_progressIndicator">' +
                                      '<table height="100%" cellspacing="0" cellpadding="0" width="100%">' +
                                            '<tr><td align="center" valign="middle"></td></tr>' +
                                      '</table>' +
                                    '</div>';
                    $(document.body).append(arrHtml);
                    return true;
                }
                return false;
            }
        },

        confirmDialog: function (options) {

            options = jQuery.extend({}, {
                msg: 'Are you sure?',
                yesText: "<%=Token('yes')%>",
                noText: "<%=Token('no')%>",
                context: null,
                yesClickHandler: null,
                yesClickParams: null,
                noClickHandler: null,
                noClickParams: null,
                title: null
            }, options);

            if ((!options.yesClickHandler) || typeof (options.yesClickHandler) !== 'function') {
                return;
            }

            if (!options.context) {
                options.context = this.confirmDialog.caller;
            }
            var confirmDialog = $('#confirmDialog'), firstTime = false, modalContent;
            if (confirmDialog.length === 0) {
                var markup = ['<div id="confirmDialog" class="dj_modal"">',
                                '<div class="dj_modal-header">',
                                    '<h3 class="dj_modal-title"></h3>',
                                    '<p class="dj_modal-close" onclick="$().overlay.hide(\'#confirmDialog\');">&nbsp;</p>',
                                '</div>',
                                '<div class="dj_modal-content">',
                                    '<h4></h4>',
                                    '<span class="dj_btn"></span>',
                                    '<span class="dj_btn dj_btn-grey no-margin dj_modal-close"></span>',
                                '</div>',
                            '</div>'].join('');

                $(document.body).append(markup);
                confirmDialog = $('#confirmDialog');
                firstTime = true;
            }

            if(options.title)
            {
                confirmDialog.children(".dj_modal-header").show().children("h3").html(options.title);
                confirmDialog.removeClass("paddingTop_20px");
            }
            else
            {
                confirmDialog.children(".dj_modal-header").hide();
                confirmDialog.addClass("paddingTop_20px");
            }

            modalContent = confirmDialog.children(".dj_modal-content");

            modalContent.children("span:first").html(options.yesText).unbind('click')
            .click(function () {
                $().overlay.hide('#confirmDialog');
            })
            .click(options.yesClickParams, options.yesClickHandler);

            modalContent.children("span:last").html(options.noText).unbind('click')
            .click(function () {
                $().overlay.hide('#confirmDialog');
            })
            .click(options.noClickParams, options.noClickHandler);

            modalContent.children("h4").css("white-space", "nowrap").html(options.msg + '');

            if (!firstTime) {
                confirmDialog.parent().show().css({ "visisble": "hidden", "width": "auto" });
            }

            confirmDialog.width("auto");
            var width = confirmDialog.width();

            if (width < 300) {
                confirmDialog.width(300);
            }
            else if (width > $(window).width() - 300) {
                modalContent.children("h4").css("white-space", "normal");
                confirmDialog.width($(window).width() - 300);
            }
            else {
                confirmDialog.width(width);
            }

            if (!firstTime) {
                confirmDialog.parent().hide();
            }

            confirmDialog.overlay({ bgcolor: '#555', background: true, closeOnEsc: true, fadeInTime: 100, fadeOutTime: 10 });
        },

        validDate: function(date, dateFormat, returnDateFormat) {
            try{
                var retVal = false;
                if(date){
                    date = $.trim(date);
                    var sep = "/";
                    if(date.indexOf("-") > -1){
                        sep = "-";
                        date = date.replace(/-/g, "/"); 
                    }
                    var validformat=/^\d{2}\/\d{2}\/\d{4}$/;
                    if(dateFormat != null && dateFormat == $dj.dateFormat.CCYYMMDD){//ISO date format
                        validformat=/^\d{8}$/;
                    }
                    
                    if (!validformat.test(date)){
                        retVal = false;
                    }
                    else{ //Detailed check for valid date ranges
                        var mIndex = 0, dIndex = 1, yIndex = 2, dateParts = date.split("/");
                        if(dateFormat != null && dateFormat != $dj.dateFormat.MMDDCCYY){
                            if(dateFormat == $dj.dateFormat.CCYYMMDD){//ISO date format
                                dateParts = [date.substring(0, 4), date.substring(4, 6), date.substring(6)];
                                mIndex = 1;
                                dIndex = 2;
                                yIndex = 0;
                            }
                            else{//DDMMCCYY
                                mIndex = 1;
                                dIndex = 0;
                                yIndex = 2;
                            }                               
                        }

                        var monthfield = parseInt(dateParts[mIndex], 10);
                        var dayfield = parseInt(dateParts[dIndex], 10);
                        var yearfield = parseInt(dateParts[yIndex], 10);
                        var dayobj = new Date(yearfield, monthfield-1, dayfield);
                        if ((dayobj.getMonth() + 1 !== monthfield) || (dayobj.getDate() !== dayfield) || (dayobj.getFullYear() !== yearfield)) {
                            retVal = false;
                        }
                        else {
                            
                            if(returnDateFormat != null){
                                if(dateFormat == $dj.dateFormat.MMDDCCYY){
                                    retVal = dayobj.format("mm" + sep + "dd" + sep + "yyyy");
                                }
                                else if(dateFormat == $dj.dateFormat.DDMMCCYY){
                                    retVal = dayobj.format("dd" + sep + "mm" + sep + "yyyy");
                                }
                                else{//CCYYMMDD - No sep for ISO data format
                                    retVal = dayobj.format("yyyymmdd");
                                }
                            }
                            else{
                                retVal = true;
                            }
                        }
                    }
                }
                return retVal;
            }
            catch(e){
                return false;
            }
        }
    };


    // Register the global variable
    window.DowJones = window.$dj = $dj;
    $.fn.dj = $dj;

}(jQuery));


//
// The core "DJ Components"
// DJ.Component, DJ.UI.Component, and DJ.UI.Component.Canvas
//
(function ($) {

    $dj.registerNamespace('DJ');

    DJ.Component = Class.extend({

        //
        // Properties
        //

        data: { },

        defaults: { },

        options: { },

        _delegates: { },


        //
        // Initialization
        //

        init: function(meta) {
            var $meta = $.extend({ name : 'Component' }, meta);

            this.data       = $meta.data;
            this.defaults   = $.extend(true, {}, this.defaults, $meta.defaults);
            this.options    = $.extend(true, {}, this.options, this.defaults);
            this.options    = $.extend(true, {}, this.options, $meta.options);

            this.name = $meta.name;

            // generate auto getter/setter for properties in options
            this._createAccessors(this.options);

            this._initializeDelegates();
        },

        _createAccessors: function (propertyBag) {
            // declare a local one so that we're immune to changes to the global javascript 'undefined'
            var UNDEFINED;

            if (propertyBag === UNDEFINED || propertyBag === null) {
                return;     // nothing to create
            }
            for (var propName in propertyBag) {
                if (this["get_" + propName] === undefined) { // do not override a user defined getter
                    this["get_" + propName] = (function (prop) { return function () { return propertyBag[prop]; }; } (propName));
                }

                if (this["set_" + propName] === undefined)  { // do not override a user defined setter
                    this["set_" + propName] = (function(prop) { return function(value) {  propertyBag[prop] = value; }; }(propName));
                }
            }

        },


        //
        // Public methods
        //
        dispose: function() {
            for(var delegate in this._delegates) {
                this._delegates[delegate] = null;
            }
        },

        getData: function() {
            return this._getData();
        },

        setData: function(value) {
            this.data = value;
            this._setData(value);
        },

        toString: function() {
            return this.name;
        },

        _getData: function() {
            return this.data;
        },

        _setData: function(value) {
            // Overridable
        },

        _debug: function(message) {
            $dj.debug(this.name + '>> ' + message);
        },

        _initializeDelegates: function() {
            this._delegates = {};
        },

        _initializeEventHandlers: function () {
            this._debug('Implement _initializeEventHandlers to bind event handlers to elements');
        }

    });

    $dj.debug('Registered DJ.Component');


    $dj.registerNamespace('DJ.UI');

    DJ.UI.Component = DJ.Component.extend({

        //
        // Properties
        //

        defaults: { 
            cssClass: 'ui-component'
        },

        eventHandlers: { },



        //
        // Initialization
        //

        init: function(element, meta) {
            var $meta = $.extend({ name: "UIComponent" }, meta);

            this.element = element;
            this.$element = $(element);

            if(element) {
                $meta.name = this.element.id || this.element.name;
            }

            try {
                var owner = $meta.owner || this.$element.parent().get(0);
                this.setOwner(owner);
            } catch(ex) { 
            }

            this._super($meta);

            $.extend(this.tokens, $meta.tokens);
            this.eventHandlers = $.extend(true, {}, this.eventHandlers, $meta.eventHandlers);

            this.$element.data("options", this.options);

            this.$element.data("data", this.data);

            this.$element.addClass(this.options.cssClass);
            this._addBaseClass();

            this._initializeElements(this.$element);

            this._initializeEventHandlers();

            // take name of event handlers (that come as strings) 
            // and turn them to function references
            this.mappedhandlers = {};
            for (var handler in this.eventHandlers) {
                this.mappedhandlers[handler] = this._mapperR(this.eventHandlers[handler]);
            }
            
            // avoid some overhead if no valid handlers are found
            if(!$.isEmptyObject(this.mappedhandlers)) {
                this.$element.bind(this.mappedhandlers);
            }
        },


        _mapperR: function(handlerName, stack) {
            // check if empty string
            if(!handlerName || handlerName === '' || (handlerName.replace(/\s/g,'') === '')) { return null; }

            var buf = handlerName.split('.');
            stack = stack || window;
            return (buf.length === 1) ? stack[buf[0]] : this._mapperR(buf.slice(1).join('.'), stack[buf[0]]);
        },


        //
        // Public methods
        //

        appendData: function (value) {
            var startTime = new Date();
            
            this._appendData(value);

            this._debug('appendData:' + (new Date().getTime() - startTime.getTime()));
        },

        getId: function() {
            return (this.element) ? this.element.id : null;
        },

        getOwner: function () {
            return this._owner;
        },

        publish: function (/* string */eventName, /* object */args) {
            $dj.debug('DJ.UI.Component.Publish:', this._owner || window, eventName);
            var publish = (this._owner && this._owner._innerPublish && this._owner._innerPublish instanceof Function) ? this._owner._innerPublish : $dj.publish;
            publish.call(this._owner || window, eventName, args);

            return this;
        },

        subscribe: function (/* string */eventName, /* function() */handler) {
            if (this._owner && this._owner.subscribe) {
                this._owner.subscribe(eventName, handler);
            }

            return this;
        },

        setData: function (value) {
            this._super(value);
            this._clear();
        },

        setOwner: function (value) {
            if(!value) {
                this._owner = null;
                return;
            }

            var owner = value;

            // Convert a DOM ID to a jQuery object
            if($dj.isString(value)) {
                owner = $(value);
            }

            // Convert a jQuery object to a DJ.UI.Component
            if(value instanceof jQuery) {
                owner = value.findComponent(DJ.UI.Component);
            }

            // Freak out if this isn't a Component
            if(!(value instanceof DJ.UI.Component)) {
                this._debug('Owner is not a DJ.UI.Component - skipping setOwner()');
                return this;
            }

            this._owner = owner;
            
            return this;
        },

        toString: function() {
            return this.getId() || this._super();
        },

        
        //
        // Protected methods
        //

        _getHashKey: function () {
            // TODO: feel free to improvise this to return something better
            return new Date().getTime();
        },

        _addBaseClass: function() {
            var baseClassName = this.options.baseClassName;
            if (baseClassName && baseClassName.length > 0 && !$.isArray(baseClassName)) {
                $(this.element).addClass(this.options.baseClassName);
            }
        },

        _appendData: function (value) {
            this._debug('TODO: Implement _appendData function');
        },

        _clear: function () {
            $(this.element).empty();
        },

        _initializeElements: function (ctx) {
            var sample = 'Implement _initializeElements function to lookup html elements and cache them at component level\n' +
                         '     e.g. this.$industry = $(this.selectors.industry, ctx);\n' +
                         '          where $industry is an html select control and this.selectors.industry = \'select.dj_Lens_Industry\'\n' +
                         '          and ctx is usually this.$element when inside a component';
            this._debug(sample);
        },

        EOF: {}

    });

    $dj.debug('Registered DJ.UI.Component (extends DJ.Component)');


}(jQuery));

/********** overlay plugin **************/
(function ($) {

    $.iDevices = {
        iPad: (navigator.userAgent.indexOf('iPad') !== -1)
    };

    $.fn.overlay = function (options) {
        if (!this.selector) { return; }

        options = $.extend({}, $.fn.overlay._defaults, (options || {}));

        if ($(this.selector).length > 1) {
            $dj.debug('Selector must result in a unique DOM Element.');
            return;
        }

        var overlayIds = $.fn.overlay._getIds(this.selector);
        options.overlayIds = overlayIds;

        if (options.background === true) {
            if ($('#' + overlayIds.background).length === 0) {
                $(document.body).append('<div id="' + overlayIds.background + '"></div>');
                $('#' + overlayIds.background).css({ 'position': 'absolute', 'top': '0px', 'left': '0px', 'display': 'none' });
            }
        }

        //load the overlay div element;
        if ($('#' + overlayIds.overlay).length === 0) {
            $(document.body).append('<div id="' + overlayIds.overlay + '"></div>');
            $('#' + overlayIds.overlay).append($(this.selector));
            $('#' + overlayIds.overlay).css({ 'position': ($.browser.msie && $.browser.version === 6) ? 'absolute' : 'fixed', 'display': 'none' });
        }
        $(this.selector).css({ "display": "block", "visibility": "visible" });

        $(this.selector).data("overlayoptions", options);

        $().overlay.show(this.selector);

        return this;
    };

    $.fn.overlay._overlayCount = 0;
    $.fn.overlay._defaults = {
        background: true,
        bgcolor: '#000000',
        closeOnEsc: false,
        fadeInTime: 500,
        fadeOutTime: 200,
        hideSelect: true,
        onShow: null,
        onHide: null,
        autoScroll: true
    };
    $.fn.overlay._getIds = function (selector) {
        if (selector && $(selector).length > 0) {
            var ids = {};
            var selectorId = $(selector).attr("id");
            if (!selectorId || selectorId === "") {
                selectorId = selector.trim().replace(" ", "_").replace(".", "_");
            }

            ids.background = '__djBackground';
            ids.overlay = selectorId + '__djoverlay';
            return ids;
        }
    };
    $.fn.overlay._activeOverlays = [];
    $.fn.overlay._position = function (selector) {
        var selectorJObj = $(selector);
        var options = selectorJObj.data("overlayoptions");

        $('#' + options.overlayIds.overlay).width("auto");
        selectorJObj.show();
        var intHeight = selectorJObj.outerHeight(true);
        var intWidth = selectorJObj.outerWidth(true);

        var t, l, css;
        var position = (($.browser.msie && $.browser.version === 6) || $.iDevices.iPad) ? 'absolute' : 'fixed';

        if (!intHeight) {
            intHeight = selectorJObj.css("height");
        }

        if (!intWidth) {
            intWidth = selectorJObj.css("width");
        }

        if (typeof (intHeight) === 'string') {
            intHeight = intHeight.replace("px", "");
        }

        if (typeof (intWidth) === 'string') {
            intWidth = intWidth.replace("px", "");
        }

        t = ($(window).height() - intHeight) / 2;
        l = (($(window).width() - intWidth) / 2);

        if (($.browser.msie && $.browser.version === 6) || $.iDevices.iPad) {
            t = t + ($.iDevices.iPad ? window.pageYOffset : $(document).scrollTop());
        }

        css = { 'height': intHeight, 'width': intWidth, 'top': t, 'left': l, 'position': position };

        $('#' + options.overlayIds.overlay).css(css);

        if (options.background && $('#' + options.overlayIds.background).length > 0) {
            $('#' + options.overlayIds.background).css({ 'height': ($(document).height() - 1), 'width': $(window).width() });
        }
    };
    $.fn.overlay.hide = function (selector, callback) {
        if (!selector) { return; }

        var options = $(selector).data("overlayoptions");
        if (!options || $('#' + options.overlayIds.overlay).length === 0) { return; }

        $('#' + options.overlayIds.overlay).fadeOut(options.fadeOutTime, function () {
            $('#' + options.overlayIds.overlay).css("display", "none");
            callback = callback || options.onHide;
            if (callback && $.isFunction(callback)) {
                callback.apply(this);
            }
        });

        $.fn.overlay._activeOverlays = $.grep($.fn.overlay._activeOverlays, function (val) { return val !== selector; });
        if ($.fn.overlay._activeOverlays.length > 0) {
            var prevOverlay = $.fn.overlay._activeOverlays[$.fn.overlay._activeOverlays.length - 1];
            $().overlay.show(prevOverlay, true);
        }
        else {

            if ($('#' + options.overlayIds.background).length > 0) {
                $('#' + options.overlayIds.background).fadeOut(options.fadeOutTime, function () {
                    //Show all the hidden select dropdowns
                    if (options.hideSelect && $.browser.msie && $.browser.version === 6) {
                        $("select").css("visibility", "visible");
                    }
                });
            }

            $(document).unbind("keyup.overlay");
            $(window).unbind("scroll.overlay");
            $(window).unbind("resize.overlay");
        }
    };

    $.fn.overlay.rePosition = function(){
        $(window).trigger("scroll.overlay");
    };

    $.fn.overlay.show = function (selector, retainBackground) {
        if (!selector) { return; }

        var overlayCount = $.fn.overlay._overlayCount;
        var zIndex = 99999;
        var options = $(selector).data("overlayoptions");

        if (options.background) {
            if ($.fn.overlay._activeOverlays.length > 0) {
                if (options.bgcolor !== 'transparent') {
                    $('#' + options.overlayIds.background).css('z-index', (zIndex + overlayCount));
            }
            }
            else {
                $('#' + options.overlayIds.background).css('z-index', (zIndex + overlayCount));
            }
        }

        $('#' + options.overlayIds.overlay).css('z-index', (zIndex + overlayCount + 2));

        //Hide all the select dropdowns on the page except the current overlay container
        if (options.hideSelect && $.browser.msie && $.browser.version === 6) {
            $("select").css("visibility", "hidden");
            $('#' + options.overlayIds.overlay).find("select").css("visibility", "visible");
        }

        if (!retainBackground) {
            if (options.background) {
                if ($.fn.overlay._activeOverlays.length > 0) {
                    if (options.bgcolor !== 'transparent') {
                        $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
                }
                }
                else {
                    if (options.bgcolor !== 'transparent') {
                        $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
                    }
                    else {
                        $('#' + options.overlayIds.background).css({ 'background': '#fff', 'opacity': '0', 'display': 'block' });
                }
            }
            }

            $('#' + options.overlayIds.overlay).fadeIn(options.fadeInTime, options.onShow);
        }
        else {
            if (options.background) {
                if (options.bgcolor !== 'transparent') {
                    $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
                }
                else {
                    $('#' + options.overlayIds.background).css({ 'background': '#fff', 'opacity': '0', 'display': 'block' });
            }
        }
        }

        $.fn.overlay._position(selector);

        $(document).unbind("keyup.overlay");
        $(window).unbind("scroll.overlay").unbind("resize.overlay").bind("resize.overlay", function () {
            $.fn.overlay._position(selector);
        });
        if (options.autoScroll) {
            $(window).bind("scroll.overlay", function () {
                $.fn.overlay._position(selector);
            });
        }

        if (options.closeOnEsc) {
            $(document).bind("keyup.overlay", function (e) {
                if (e.keyCode === 27) {
                    //To fix FF issue which fires this event when we close the fullscreen mode
                    if (typeof $dj !== 'undefined' && $dj.videoPlayerInFullScreen) { return; }
                    $().overlay.hide(selector);
                }
            });
        }

        if (!retainBackground) {
            $.fn.overlay._activeOverlays = $.grep($.fn.overlay._activeOverlays, function (val) { return val !== selector; });
            $.fn.overlay._activeOverlays.push(selector);
        }

        $.fn.overlay._overlayCount += 3;

        if ($.fn.overlay._activeOverlays.length === 0 && !options.background && $('#' + options.overlayIds.background).length > 0) {
            $('#' + options.overlayIds.background).hide();
        }

        $('#' + options.overlayIds.overlay).focus();
    };
} (jQuery));

/********* ParseDatesInObject **********/
var JSON = JSON || {};

if (!JSON.parseDatesInObj) {
    JSON.parseDatesInObj = function (jsonObj) {
        try {
            for (var prop in jsonObj) {
                if (jsonObj.hasOwnProperty(prop)) {
                    var val = jsonObj[prop];
                    if (typeof val === 'object') {
                        JSON.parseDatesInObj(val);
                    }
                    else if ($dj.isString(val)) {
                        var dateValue = JSON.parseDate(val);
                        if (dateValue) {
                            jsonObj[prop] = dateValue;
                        }
                        //return;
                    }
                }
            }
        } catch (e) {
            // orignal error thrown has no error message so rethrow with message
            throw new Error("Dates in JSON Object could not be parsed");
        }
    };
}

if (!JSON.parseDate) {
    JSON.parseDate = function (dateString) {
        var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
        var reMsAjax = /^\/Date\((d|-|.*)\)[\/|\\]$/;

        try {
            if ($dj.isString(dateString)) {
                var a = reISO.exec(dateString);
                if (a) {
                    return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));
                }
                a = reMsAjax.exec(dateString);
                if (a) {
                    var b = a[1].split(/[-+,.]/);
                    return new Date(b[0] ? +b[0] : 0 - +b[1]);
                }
            }
        } catch (e) {
            throw new Error("Date could not be parsed");
        }
    };
}

