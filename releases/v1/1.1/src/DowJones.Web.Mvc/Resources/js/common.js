//!
// DowJones Common
//


// Simple JavaScript Inheritance
// By John Resig http://ejohn.org/blog/simple-javascript-inheritance/
// MIT Licensed.
//

/// <reference path="jquery.js" />
(function () {
    var initializing = false, fnTest = /xyz/.test(function () { xyz; }) ? /\b_super\b/ : /.*/;
    // The base Class implementation (does nothing)
    this.Class = function () { };

    // Create a new Class that inherits from this class
    Class.extend = function (prop) {
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
                   typeof prop[name] == "function"
                && typeof _super[name] == "function"
                && fnTest.test(prop[name])
                    ? (function (name, fn) {
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
                    })(name, prop[name])
                    : prop[name];
        }

        // The dummy class constructor
        function Class() {
            // All construction is actually done in the init method
            if (!initializing && this.init)
                this.init.apply(this, arguments);
        }

        // Populate our constructed prototype object
        Class.prototype = prototype;

        // Enforce the constructor to be what we expect
        Class.constructor = Class;

        // And make this class extendable
        Class.extend = arguments.callee;

        return Class;
    };
})();

//  
///  Global jQuery Extensions
//
(function ($) {

    //  The "inheritance plugin" model
    ///  http://alexsexton.com/?p=51
    //
    $.plugin = function (name, object) {
        $.fn[name] = function (options) {
            var args = Array.prototype.slice.call(arguments, 1);
            return this.each(function () {
                var instance = $.data(this, name, new object(this, options));
            });
        };
    };


    $.extend($.fn, {
        
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
                    if(component != null) return;

                    if(datum instanceof componentType)
                        component = datum;
                });
            } catch (e) {
            }

            return component;
        },

        _getComponent: function (pluginName) {
            return this.data(pluginName);
        },

        dj_dropDownMenu: function (options, handler) {
            var options = $.extend({
                trigger: $('.trigger', this),
                menu: $(this)
            }, options);

            var menu = $(options.menu);

            if (menu.length == 0)
                throw "Cannot initialize menu without a target menu element";

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
                    if (href && href[0] == '#') {
                        command = href.substring(1);
                        $menuItem.data('command', command);
                        $('a', $menuItem).attr('href', 'javascript: void(0)');
                    }
                }

                $menuItem
                    .hover(
                        function (e) { $(this).addClass("dj_mouseover"); },
                        function (e) { $(this).removeClass("dj_mouseover"); }
                    )
                    .click(function (e) {
                        if (handler) { handler(command, $menuItem); }
                        else $dj.debug('Menu command (with no handler): ' + command);
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


        dj_snippetTooltip: function(className, containerName) {
                                                                                                                                                                                            return this.each(function() {
            var text = $(this).attr("title");
            var wHeight = Math.max($(window))
            $(this).attr("title", "");
            if (text != undefined && text != "") {
                $(this).hover(function(e) {
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

                }, function(e) {
                    $("#dj_snippetTooltip").remove();
                    $(this).attr("title", text);
                    e.stopPropagation();
                });
                $(this).mousemove(function(e) {
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
            };
        });
        },

        // Simple Tooltip plugin
        dj_simpleTooltip: function(className) {
                                                                                                                                                                                                                                                                            return this.each(function() {
            var sText = $(this).attr("title");
            $(this).attr("title", "");
            if (sText != undefined) {
                $(this).hover(function(e) {
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
                            $("body").append("<div id='dj_tooltip' class=\"" + className + "\" style='position: absolute; z-index: 100; display: none;'><div class=\"border\">" + sText + "</div></div>");
                        }

                        $tObj = $("#dj_tooltip");
                        if ($.fn.bgiframe) {
                            $tObj.bgiframe();
                        }
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
                }, function(e) {
                    var enable = $(this).data('enableSimpleTooltip');
                    if (enable === undefined || enable === true) {
                        $("#dj_tooltip").stop(true, true).fadeOut("fast");
                        $(this).attr("title", sText);
                    }
                });
                $(this).mousemove(function(e) {
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
            };
        });

            return $(this);
        },

        showLoading: function () {
            try {
                var loadingDiv = this.data('loadingDiv');
                if(!loadingDiv) {
                    loadingDiv =  $('<div class="loadingProgress"></div>').appendTo(this);
                    this.data('loadingDiv', loadingDiv);
                }
                loadingDiv.css({ "height": this.outerHeight(), "width": this.outerWidth(), "left": 0, "top": 0}).show();
                var l = this.offset().left - loadingDiv.offset().left, t = this.offset().top - loadingDiv.offset().top;
                loadingDiv.css({ "left": l, "top": t});
            } 
            catch (e) {
                if(loadingDiv)
                    loadingDiv.hide();
            }       
        },

        hideLoading: function () {
            if(this.data('loadingDiv')) { this.data('loadingDiv').hide(); }
        }

    });

})(jQuery);


//
// $dj:  Custom Global Functions
//
(function ($) {

    var $dj = {

        clone: function(source) {
            return $.extend(true, {}, source);
        },

        cStr: function(vVariant) {
            try {
                var str = new String(vVariant).toString();
                if (str == "undefined" || str == "null")
                    str = "";
            }
            catch (e) { str = ""; }
            return this.trim(str);
        },


        debug: function(text) {
            if(!this.debugEnabled || !window) 
                return;

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

        debugEnabled: false,


        delegate: function (context, handler, customData) {
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
                if ((!d.compatMode || d.compatMode == 'CSS1Compat') && d.documentElement && d.documentElement.clientWidth)
                { v = d.documentElement.clientWidth; }
                else if (d.body && d.body.clientWidth)
                { v = d.body.clientWidth; }
                else if (this.def(w.innerWidth, w.innerHeight, d.height)) {
                    v = w.innerWidth;
                    if (d.height > w.innerHeight) v -= 16;
                }
                return v;
            },

        getClientHeight: function() {
                var v = 0, d = document, w = window;
                if ((!d.compatMode || d.compatMode == 'CSS1Compat') && d.documentElement && d.documentElement.clientHeight)
                { v = d.documentElement.clientHeight; }
                else if (d.body && d.body.clientHeight)
                { v = d.body.clientHeight; }
                else if (this.def(w.innerWidth, w.innerHeight, d.width)) {
                    v = w.innerHeight;
                    if (d.width > w.innerWidth) v -= 16;
                }
                return v;
            },

        getHorizontalScroll: function() {
                var d = document, w = window;
                var offset = 0;
                if (w.pageXOffset) { // All but IE
                    offset = w.pageXOffset;
                }
                else if (w.document.documentElement &&  // IE6 w/ doctype
                         w.document.documentElement.scrollLeft) {
                    offset = w.document.documentElement.scrollLeft;
                }
                else if (w.document.body.scrollLeft) { // IE4,5,6(w/o doctype)
                    offset = w.document.body.scrollLeft;
                }
                if (this.isNum(offset)) {
                    return offset;
                }
                return 0;
            },

        getVerticalScroll: function() {
                var d = document, w = window;
                var offset = 0;
                if (w.pageYOffset) { // All but IE
                    offset = w.pageYOffset;
                }
                else if (w.document.documentElement &&  // IE6 w/ doctype
                        w.document.documentElement.scrollTop) {
                    offset = w.document.documentElement.scrollTop;
                }
                else if (w.document.body.scrollTop) { // IE4,5,6(w/o doctype)
                    offset = w.document.body.scrollTop;
                }
                if (this.isNum(offset)) {
                    return offset;
                }
                return 0;
            },

        isNum: function() {
                for (var i = 0; i < arguments.length; ++i) { if (isNaN(arguments[i]) || typeof (arguments[i]) != 'number') return false; }
                return true;
            },

        htmlEncode: function(txt) {
                return $("<div/>").text(txt).html();
            },

        htmlDecode: function(html) {
                return $("<div/>").html(html).text();
            },

        timer: function(interval, callback, options) {
	                    var options = $.extend({ reset: 500, _isStopped: false, _timerID: null, _userCallback: callback }, options); // Create options for the default reset value
	                    var interval = interval || options.reset;

	                    if(!callback) { return false; }

	                    timer = function(interval, callback) {		                    
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

        sanitizeJsonString: function (jsonString) {
            if(!$dj.isString(jsonString)) throw 'Input is not a string';

            return jsonString.replace('\"', '\\\"');
        },

        queryParameter: function (name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }

    };


    // Register the global variable
    window["DowJones"] = window["$dj"] = $dj;
    $.fn.dj = $dj;

})(jQuery);


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
            var undefined;

            if (propertyBag === undefined || propertyBag === null) {
                return;     // nothing to create
            }
            for (var propName in propertyBag) {
                if (this["get_" + propName] === undefined) { // do not override a user defined getter
                    this["get_" + propName] = function (prop) { return function () { return propertyBag[prop]; } } (propName);
                }

                if (this["set_" + propName] === undefined)  { // do not override a user defined setter
                    this["set_" + propName] = (function(prop) { return function(value) {  propertyBag[prop] = value; }; })(propName);
                }
            }

        },


        //
        // Public methods
        //
        dispose: function() {
            for(delegate in this._delegates) {
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
            $dj.debug('Implement _initializeEventHandlers to bind event handlers to elements');
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

        tokens: { },

        //
        // Initialization
        //

        init: function(element, meta) {
            var $meta = $.extend({ name: "UIComponent" }, meta);

            this.element = element;
            this.$element = $(element);

            if(element)
                $meta.name = this.element.id || this.element.name;

            this._super($meta);

            $.extend(this.tokens, $meta.tokens);
            this.eventHandlers = $.extend(true, {}, this.eventHandlers, $meta.eventHandlers);

            this.$element.data("options", this.options);
            this.$element.data("tokens", this.tokens);
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
            };
            
            // avoid some overhead if no valid handlers are found
            if(!$.isEmptyObject(this.mappedhandlers)) 
                this.$element.bind(this.mappedhandlers);
        },


        _mapperR: function(handlerName, stack) {
            // check if empty string
            if(!handlerName || handlerName === '' || (handlerName.replace(/\s/g,'') === '')) return null;

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

        paint: function () {
            var startTime = new Date();

            this._paint();

            this._debug('paint:' + (new Date().getTime() - startTime.getTime()));
        },

        setData: function (value) {
            this._super(value);
            this._clear();
            this.paint();
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
            var sample = ['Implement _initializeElements function to lookup html elements and cache them at component level'
                        , '     e.g. this.$industry = $(this.selectors.industry, ctx);'
                        , '          where $industry is an html select control and this.selectors.industry = \'select.dj_Lens_Industry\''
                        , '          and ctx is usually this.$element when inside a component'].join(' \n');
            this._debug(sample);
        },

        _paint: function () {
            this._debug('TODO: Implement _paint function');
        }


    });

    $dj.debug('Registered DJ.UI.Component (extends DJ.Component)');


})(jQuery);

/********** overlay plugin **************/
(function ($) {

    $.iDevices = {
        iPad: (navigator.userAgent.indexOf('iPad') != -1)
    }

    $.fn.overlay = function (options) {
        if (!this.selector)
            return;

        options = $.extend({}, $.fn.overlay._defaults, (options || {}));

        if ($(this.selector).length > 1) {
            $dj.debug('Selector must result in a unique DOM Element.');
            return;
        }

        var overlayIds = $.fn.overlay._getIds(this.selector);
        options.overlayIds = overlayIds;

        if (options.background == true) {
            if ($('#' + overlayIds.background).length == 0) {
                $(document.body).append('<div id="' + overlayIds.background + '"></div>');
                $('#' + overlayIds.background).css({ 'position': 'absolute', 'top': '0px', 'left': '0px', 'display': 'none' });
            }
        }

        //load the overlay div element;
        if ($('#' + overlayIds.overlay).length == 0) {
            $(document.body).append('<div id="' + overlayIds.overlay + '"></div>');
            $('#' + overlayIds.overlay).append($(this.selector));
            $('#' + overlayIds.overlay).css({ 'position': ($.browser.msie && $.browser.version == 6) ? 'absolute' : 'fixed', 'display': 'none' });
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
        onHide: null
    };
    $.fn.overlay._getIds = function (selector) {
        if (selector && $(selector).length > 0) {
            var ids = {};
            var selectorId = $(selector).attr("id");
            if (!selectorId || selectorId == "")
                selectorId = selector.trim().replace(" ", "_").replace(".", "_");

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
        var position = (($.browser.msie && $.browser.version == 6) || $.iDevices.iPad) ? 'absolute' : 'fixed';

        if (!intHeight)
            intHeight = selectorJObj.css("height");

        if (!intWidth)
            intWidth = selectorJObj.css("width");

        if (typeof (intHeight) == 'string')
            intHeight = intHeight.replace("px", "");

        if (typeof (intWidth) == 'string')
            intWidth = intWidth.replace("px", "");

        t = ($(window).height() - intHeight) / 2;
        l = (($(window).width() - intWidth) / 2);

        if (($.browser.msie && $.browser.version == 6) || $.iDevices.iPad) {
            t = t + ($.iDevices.iPad ? window.pageYOffset : $(document).scrollTop());
        }

        css = { 'height': intHeight, 'width': intWidth, 'top': t, 'left': l, 'position': position };

        $('#' + options.overlayIds.overlay).css(css);

        if (options.background && $('#' + options.overlayIds.background).length > 0) {
            $('#' + options.overlayIds.background).css({ 'height': ($(document).height() - 1), 'width': $(window).width() });
        }
    };
    $.fn.overlay.hide = function (selector, callback) {
        if (!selector)
            return;

        var options = $(selector).data("overlayoptions");
        if (!options || $('#' + options.overlayIds.overlay).length == 0)
            return;

        $('#' + options.overlayIds.overlay).fadeOut(options.fadeOutTime, function () {
            $('#' + options.overlayIds.overlay).css("display", "none");
            callback = callback || options.onHide;
            if (callback && $.isFunction(callback)) {
                callback.apply(this);
            }
        });

        $.fn.overlay._activeOverlays = $.grep($.fn.overlay._activeOverlays, function (val) { return val != selector; });
        if ($.fn.overlay._activeOverlays.length > 0) {
            var prevOverlay = $.fn.overlay._activeOverlays[$.fn.overlay._activeOverlays.length - 1];
            $().overlay.show(prevOverlay, true);
        }
        else {

            if ($('#' + options.overlayIds.background).length > 0) {
                $('#' + options.overlayIds.background).fadeOut(options.fadeOutTime, function () {
                    //Show all the hidden select dropdowns
                    if (options.hideSelect && $.browser.msie && $.browser.version == 6) {
                        $("select").css("visibility", "visible");
                    }
                });
            }

            $(document).unbind("keyup.overlay");
            $(window).unbind("scroll.overlay");
            $(window).unbind("resize.overlay");
        }
    };

    $.fn.overlay.show = function (selector, retainBackground) {
        if (!selector)
            return;

        var overlayCount = $.fn.overlay._overlayCount;
        var zIndex = 99999;
        var options = $(selector).data("overlayoptions");

        if (options.background) {
            if ($.fn.overlay._activeOverlays.length > 0) {
                if (options.bgcolor != 'transparent')
                    $('#' + options.overlayIds.background).css('z-index', (zIndex + overlayCount));
            }
            else {
                $('#' + options.overlayIds.background).css('z-index', (zIndex + overlayCount));
            }
        }

        $('#' + options.overlayIds.overlay).css('z-index', (zIndex + overlayCount + 2));

        //Hide all the select dropdowns on the page except the current overlay container
        if (options.hideSelect && $.browser.msie && $.browser.version == 6) {
            $("select").css("visibility", "hidden");
            $('#' + options.overlayIds.overlay).find("select").css("visibility", "visible");
        }

        if (!retainBackground) {
            if (options.background) {
                if ($.fn.overlay._activeOverlays.length > 0) {
                    if (options.bgcolor != 'transparent')
                        $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
                }
                else {
                    if (options.bgcolor != 'transparent')
                        $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
                    else
                        $('#' + options.overlayIds.background).css({ 'background': '#fff', 'opacity': '0', 'display': 'block' });
                }
            }

            $('#' + options.overlayIds.overlay).fadeIn(options.fadeInTime, options.onShow);
        }
        else {
            if (options.background) {
                if (options.bgcolor != 'transparent')
                    $('#' + options.overlayIds.background).css({ 'background': options.bgcolor, 'opacity': '0.5' }).fadeIn(options.fadeInTime);
                else
                    $('#' + options.overlayIds.background).css({ 'background': '#fff', 'opacity': '0', 'display': 'block' });
            }
        }

        $.fn.overlay._position(selector);

        $(document).unbind("keyup.overlay");
        $(window).unbind("scroll.overlay").unbind("resize.overlay").bind("resize.overlay", function () {
            $.fn.overlay._position(selector);
        }).bind("scroll.overlay", function () {
            $.fn.overlay._position(selector);
        });

        if (options.closeOnEsc) {
            $(document).bind("keyup.overlay", function (e) {
                if (e.keyCode == 27) {
                    if (typeof ($dj) != 'undefined' && $dj.videoPlayerInFullScreen)//To fix FF issue which fires this event when we close the fullscreen mode
                        return;
                    $().overlay.hide(selector);
                }
            });
        }

        if (!retainBackground) {
            $.fn.overlay._activeOverlays = $.grep($.fn.overlay._activeOverlays, function (val) { return val != selector; });
            $.fn.overlay._activeOverlays.push(selector);
        }

        $.fn.overlay._overlayCount += 3;

        if ($.fn.overlay._activeOverlays.length == 0 && !options.background && $('#' + options.overlayIds.background).length > 0)
            $('#' + options.overlayIds.background).hide();

        $('#' + options.overlayIds.overlay).focus();
    };
})(jQuery);

//#region ..:: Flash Object ::..

//
// Flash Object
//
if (typeof com == "undefined") var com = new Object();
if (typeof com.deconcept == "undefined") com.deconcept = new Object();
if (typeof com.deconcept.util == "undefined") com.deconcept.util = new Object();
if (typeof com.deconcept.FlashObjectUtil == "undefined") com.deconcept.FlashObjectUtil = new Object();
com.deconcept.FlashObject = function (swf, id, w, h, ver, c, useExpressInstall, quality, xiRedirectUrl, redirectUrl, detectKey) {
    if (!document.createElement || !document.getElementById) return;
    this.DETECT_KEY = detectKey ? detectKey : 'detectflash';
    this.skipDetect = com.deconcept.util.getRequestParameter(this.DETECT_KEY);
    this.params = new Object();
    this.variables = new Object();
    this.attributes = new Array();
    this.useExpressInstall = useExpressInstall;

    if (swf) this.setAttribute('swf', swf);
    if (id) this.setAttribute('id', id);
    if (w) this.setAttribute('width', w);
    if (h) this.setAttribute('height', h);
    if (ver) this.setAttribute('version', new com.deconcept.PlayerVersion(ver.toString().split(".")));
    this.installedVer = com.deconcept.FlashObjectUtil.getPlayerVersion(this.getAttribute('version'), useExpressInstall);
    if (c) this.addParam('bgcolor', c);
    var q = quality ? quality : 'high';
    this.addParam('quality', q);
    this.addParam('allowScriptAccess', "always");
    var xir = (xiRedirectUrl) ? xiRedirectUrl : window.location;
    this.setAttribute('xiRedirectUrl', xir);
    this.setAttribute('redirectUrl', '');
    if (redirectUrl) this.setAttribute('redirectUrl', redirectUrl);
}
com.deconcept.FlashObject.prototype = {
    setAttribute: function (name, value) {
        this.attributes[name] = value;
    },
    getAttribute: function (name) {
        return this.attributes[name];
    },
    addParam: function (name, value) {
        this.params[name] = value;
    },
    getParams: function () {
        return this.params;
    },
    addVariable: function (name, value) {
        this.variables[name] = value;
    },
    getVariable: function (name) {
        return this.variables[name];
    },
    getVariables: function () {
        return this.variables;
    },
    createParamTag: function (n, v) {
        var p = document.createElement('param');
        p.setAttribute('name', n);
        p.setAttribute('value', v);
        return p;
    },
    getVariablePairs: function () {
        var variablePairs = new Array();
        var key;
        var variables = this.getVariables();
        for (key in variables) {
            if (key != "extend")
                variablePairs.push(key + "=" + variables[key]);
        }
        return variablePairs;
    },
    getFlashHTML: function () {
        var flashNode = "";
        if (navigator.plugins && navigator.mimeTypes && navigator.mimeTypes.length) { //! netscape plugin architecture
            if (this.getAttribute("doExpressInstall")) this.addVariable("MMplayerType", "PlugIn");
            flashNode = '<embed type="application/x-shockwave-flash" src="' + this.getAttribute('swf') + '" width="' + this.getAttribute('width') + '" height="' + this.getAttribute('height') + '"';
            flashNode += ' id="' + this.getAttribute('id') + '" name="' + this.getAttribute('id') + '" ';
            var params = this.getParams();
            for (var key in params) { if (key != "extend") flashNode += [key] + '="' + params[key] + '" '; }
            var pairs = this.getVariablePairs().join("&");
            if (pairs.length > 0) { flashNode += 'flashvars="' + pairs + '"'; }
            flashNode += '/>';
        } else { //! PC IE
            if (this.getAttribute("doExpressInstall")) this.addVariable("MMplayerType", "ActiveX");
            flashNode = '<object id="' + this.getAttribute('id') + '" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="' + this.getAttribute('width') + '" height="' + this.getAttribute('height') + '">';
            flashNode += '<param name="movie" value="' + this.getAttribute('swf') + '" />';
            var params = this.getParams();
            for (var key in params) {
                if (key != "extend")
                    flashNode += '<param name="' + key + '" value="' + params[key] + '" />';
            }
            var pairs = this.getVariablePairs().join("&");
            if (pairs.length > 0) { flashNode += '<param name="flashvars" value="' + pairs + '" />'; }
            flashNode += "</object>";
        }
        return flashNode;
    },
    write: function (elementId) {
        if (this.useExpressInstall) {
            //! check to see if we need to do an express install
            var expressInstallReqVer = new com.deconcept.PlayerVersion([6, 0, 65]);
            if (this.installedVer.versionIsValid(expressInstallReqVer) && !this.installedVer.versionIsValid(this.getAttribute('version'))) {
                this.setAttribute('doExpressInstall', true);
                this.addVariable("MMredirectURL", escape(this.getAttribute('xiRedirectUrl')));
                document.title = document.title.slice(0, 47) + " - Flash Player Installation";
                this.addVariable("MMdoctitle", document.title);
            }
        } else {
            this.setAttribute('doExpressInstall', false);
        }
        if (this.skipDetect || this.getAttribute('doExpressInstall') || this.installedVer.versionIsValid(this.getAttribute('version'))) {
            var n = (typeof elementId == 'string') ? document.getElementById(elementId) : elementId;
            var tStr = this.getFlashHTML();
            //!alert(this.getFlashHTML());
            n.innerHTML = this.getFlashHTML();
        } else {
            if (this.getAttribute('redirectUrl') != "") {
                document.location.replace(this.getAttribute('redirectUrl'));
            }
        }
    }
}

// ---- detection functions ---- 
com.deconcept.FlashObjectUtil.getPlayerVersion = function (reqVer, xiInstall) {
    var PlayerVersion = new com.deconcept.PlayerVersion(0, 0, 0);
    if (navigator.plugins && navigator.mimeTypes.length) {
        var x = navigator.plugins["Shockwave Flash"];
        if (x && x.description) {
            PlayerVersion = new com.deconcept.PlayerVersion(x.description.replace(/([a-z]|[A-Z]|\s)+/, "").replace(/(\s+r|\s+b[0-9]+)/, ".").split("."));
        }
    } else {
        try {
            var axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash");
            for (var i = 3; axo != null; i++) {
                axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash." + i);
                PlayerVersion = new com.deconcept.PlayerVersion([i, 0, 0]);
            }
        } catch (e) { }
        if (reqVer && PlayerVersion.major > reqVer.major) return PlayerVersion; //! version is ok, skip minor detection
        //! this only does the minor rev lookup if the user's major version 
        //! is not 6 or we are checking for a specific minor or revision number
        //! see http://!blog.deconcept.com/2006/01/11/getvariable-setvariable-crash-internet-explorer-flash-6/
        if (!reqVer || ((reqVer.minor != 0 || reqVer.rev != 0) && PlayerVersion.major == reqVer.major) || PlayerVersion.major != 6 || xiInstall) {
            try {
                PlayerVersion = new com.deconcept.PlayerVersion(axo.GetVariable("$version").split(" ")[1].split(","));
            } catch (e) { }
        }
    }
    return PlayerVersion;
}
com.deconcept.PlayerVersion = function (arrVersion) {
    this.major = parseInt(arrVersion[0]) || 0;
    this.minor = parseInt(arrVersion[1]) || 0;
    this.rev = parseInt(arrVersion[2]) || 0;
}
com.deconcept.PlayerVersion.prototype.versionIsValid = function (fv) {
    if (this.major < fv.major) return false;
    if (this.major > fv.major) return true;
    if (this.minor < fv.minor) return false;
    if (this.minor > fv.minor) return true;
    if (this.rev < fv.rev) return false;
    return true;
}
// ---- get value of query string param ---- 
com.deconcept.util = {
    getRequestParameter: function (param) {
        var q = document.location.search || document.location.hash;
        if (q) {
            var startIndex = q.indexOf(param + "=");
            var endIndex = (q.indexOf("&", startIndex) > -1) ? q.indexOf("&", startIndex) : q.length;
            if (q.length > 1 && startIndex > -1) {
                return q.substring(q.indexOf("=", startIndex) + 1, endIndex);
            }
        }
        return "";
    }
}

// add Array.push if needed (ie5) 
//if (Array.prototype.push == null) { Array.prototype.push = function(item) { this[this.length] = item; return this.length; }}

// add some aliases for ease of use/backwards compatibility 
var getQueryParamValue = com.deconcept.util.getRequestParameter;
var FlashObject = com.deconcept.FlashObject;

//#endregion
