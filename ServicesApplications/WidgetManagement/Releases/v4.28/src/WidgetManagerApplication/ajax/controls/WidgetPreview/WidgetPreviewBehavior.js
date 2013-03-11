/*  JQUERY Util functions
*
* Copyright (c) 2006-2008 Sam Collett (http://www.texotela.co.uk)
* Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
* and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
*
* Version 2.2.3
* Demo: http://www.texotela.co.uk/code/jquery/select/
*
* $LastChangedDate$
* $Rev$
*
*/

; (function($) {

    /**
    * Adds (single/multiple) options to a select box (or series of select boxes)
    *
    * @name     addOption
    * @author   Sam Collett (http://www.texotela.co.uk)
    * @type     jQuery
    * @example  $("#myselect").addOption("Value", "Text"); // add single value (will be selected)
    * @example  $("#myselect").addOption("Value 2", "Text 2", false); // add single value (won't be selected)
    * @example  $("#myselect").addOption({"foo":"bar","bar":"baz"}, false); // add multiple values, but don't select
    *
    */
    $.fn.addOption = function() {
        var add = function(el, v, t, sO) {
            var option = document.createElement("option");
            option.value = v, option.text = t;
            // get options
            var o = el.options;
            // get number of options
            var oL = o.length;
            if (!el.cache) {
                el.cache = {};
                // loop through existing options, adding to cache
                for (var i = 0; i < oL; i++) {
                    el.cache[o[i].value] = i;
                }
            }
            // add to cache if it isn't already
            if (typeof el.cache[v] == "undefined") el.cache[v] = oL;
            el.options[el.cache[v]] = option;
            if (sO) {
                option.selected = true;
            }
        };

        var a = arguments;
        if (a.length == 0) return this;
        // select option when added? default is true
        var sO = true;
        // multiple items
        var m = false;
        // other variables
        var items, v, t;
        if (typeof (a[0]) == "object") {
            m = true;
            items = a[0];
        }
        if (a.length >= 2) {
            if (typeof (a[1]) == "boolean") sO = a[1];
            else if (typeof (a[2]) == "boolean") sO = a[2];
            if (!m) {
                v = a[0];
                t = a[1];
            }
        }
        this.each(
		function() {
		    if (this.nodeName.toLowerCase() != "select") return;
		    if (m) {
		        for (var item in items) {
		            add(this, item, items[item], sO);
		        }
		    }
		    else {
		        add(this, v, t, sO);
		    }
		}
	);
        return this;
    };

    /**
    * Add options via ajax
    *
    * @name     ajaxAddOption
    * @author   Sam Collett (http://www.texotela.co.uk)
    * @type     jQuery
    * @param    String url      Page to get options from (must be valid JSON)
    * @param    Object params   (optional) Any parameters to send with the request
    * @param    Boolean select  (optional) Select the added options, default true
    * @param    Function fn     (optional) Call this function with the select object as param after completion
    * @param    Array args      (optional) Array with params to pass to the function afterwards
    * @example  $("#myselect").ajaxAddOption("myoptions.php");
    * @example  $("#myselect").ajaxAddOption("myoptions.php", {"code" : "007"});
    * @example  $("#myselect").ajaxAddOption("myoptions.php", {"code" : "007"}, false, sortoptions, [{"dir": "desc"}]);
    *
    */
    $.fn.ajaxAddOption = function(url, params, select, fn, args) {
        if (typeof (url) != "string") return this;
        if (typeof (params) != "object") params = {};
        if (typeof (select) != "boolean") select = true;
        this.each(
		function() {
		    var el = this;
		    $.getJSON(url,
				params,
				function(r) {
				    $(el).addOption(r, select);
				    if (typeof fn == "function") {
				        if (typeof args == "object") {
				            fn.apply(el, args);
				        }
				        else {
				            fn.call(el);
				        }
				    }
				}
			);
		}
	);
        return this;
    };

    /**
    * Removes an option (by value or index) from a select box (or series of select boxes)
    *
    * @name     removeOption
    * @author   Sam Collett (http://www.texotela.co.uk)
    * @type     jQuery
    * @param    String|RegExp|Number what  Option to remove
    * @param    Boolean selectedOnly       (optional) Remove only if it has been selected (default false)   
    * @example  $("#myselect").removeOption("Value"); // remove by value
    * @example  $("#myselect").removeOption(/^val/i); // remove options with a value starting with 'val'
    * @example  $("#myselect").removeOption(/./); // remove all options
    * @example  $("#myselect").removeOption(/./, true); // remove all options that have been selected
    * @example  $("#myselect").removeOption(0); // remove by index
    * @example  $("#myselect").removeOption(["myselect_1","myselect_2"]); // values contained in passed array
    *
    */
    $.fn.removeOption = function() {
        var a = arguments;
        if (a.length == 0) return this;
        var ta = typeof (a[0]);
        var v, index;
        // has to be a string or regular expression (object in IE, function in Firefox)
        if (ta == "string" || ta == "object" || ta == "function") {
            v = a[0];
            // if an array, remove items
            if (v.constructor == Array) {
                var l = v.length;
                for (var i = 0; i < l; i++) {
                    this.removeOption(v[i], a[1]);
                }
                return this;
            }
        }
        else if (ta == "number") index = a[0];
        else return this;
        this.each(
		function() {
		    if (this.nodeName.toLowerCase() != "select") return;
		    // clear cache
		    if (this.cache) this.cache = null;
		    // does the option need to be removed?
		    var remove = false;
		    // get options
		    var o = this.options;
		    if (!!v) {
		        // get number of options
		        var oL = o.length;
		        for (var i = oL - 1; i >= 0; i--) {
		            if (v.constructor == RegExp) {
		                if (o[i].value.match(v)) {
		                    remove = true;
		                }
		            }
		            else if (o[i].value == v) {
		                remove = true;
		            }
		            // if the option is only to be removed if selected
		            if (remove && a[1] === true) remove = o[i].selected;
		            if (remove) {
		                o[i] = null;
		            }
		            remove = false;
		        }
		    }
		    else {
		        // only remove if selected?
		        if (a[1] === true) {
		            remove = o[index].selected;
		        }
		        else {
		            remove = true;
		        }
		        if (remove) {
		            this.remove(index);
		        }
		    }
		}
	);
        return this;
    };

    /**
    * Sort options (ascending or descending) in a select box (or series of select boxes)
    *
    * @name     sortOptions
    * @author   Sam Collett (http://www.texotela.co.uk)
    * @type     jQuery
    * @param    Boolean ascending   (optional) Sort ascending (true/undefined), or descending (false)
    * @example  // ascending
    * $("#myselect").sortOptions(); // or $("#myselect").sortOptions(true);
    * @example  // descending
    * $("#myselect").sortOptions(false);
    *
    */
    $.fn.sortOptions = function(ascending) {
        var a = typeof (ascending) == "undefined" ? true : !!ascending;
        this.each(
		function() {
		    if (this.nodeName.toLowerCase() != "select") return;
		    // get options
		    var o = this.options;
		    // get number of options
		    var oL = o.length;
		    // create an array for sorting
		    var sA = [];
		    // loop through options, adding to sort array
		    for (var i = 0; i < oL; i++) {
		        sA[i] = {
		            v: o[i].value,
		            t: o[i].text
		        }
		    }
		    // sort items in array
		    sA.sort(
				function(o1, o2) {
				    // option text is made lowercase for case insensitive sorting
				    o1t = o1.t.toLowerCase(), o2t = o2.t.toLowerCase();
				    // if options are the same, no sorting is needed
				    if (o1t == o2t) return 0;
				    if (a) {
				        return o1t < o2t ? -1 : 1;
				    }
				    else {
				        return o1t > o2t ? -1 : 1;
				    }
				}
			);
		    // change the options to match the sort array
		    for (var i = 0; i < oL; i++) {
		        o[i].text = sA[i].t;
		        o[i].value = sA[i].v;
		    }
		}
	);
        return this;
    };
    /**
    * Selects an option by value
    *
    * @name     selectOptions
    * @author   Mathias Bank (http://www.mathias-bank.de), original function
    * @author   Sam Collett (http://www.texotela.co.uk), addition of regular expression matching
    * @type     jQuery
    * @param    String|RegExp value  Which options should be selected
    * can be a string or regular expression
    * @param    Boolean clear  Clear existing selected options, default false
    * @example  $("#myselect").selectOptions("val1"); // with the value 'val1'
    * @example  $("#myselect").selectOptions(/^val/i); // with the value starting with 'val', case insensitive
    *
    */
    $.fn.selectOptions = function(value, clear) {
        var v = value;
        var vT = typeof (value);
        var c = clear || false;
        // has to be a string or regular expression (object in IE, function in Firefox)
        if (vT != "string" && vT != "function" && vT != "object") return this;
        this.each(
		function() {
		    if (this.nodeName.toLowerCase() != "select") return this;
		    // get options
		    var o = this.options;
		    // get number of options
		    var oL = o.length;
		    for (var i = 0; i < oL; i++) {
		        if (v.constructor == RegExp) {
		            if (o[i].value.match(v)) {
		                o[i].selected = true;
		            }
		            else if (c) {
		                o[i].selected = false;
		            }
		        }
		        else {
		            if (o[i].value == v) {
		                o[i].selected = true;
		            }
		            else if (c) {
		                o[i].selected = false;
		            }
		        }
		    }
		}
	);
        return this;
    };

    /**
    * Copy options to another select
    *
    * @name     copyOptions
    * @author   Sam Collett (http://www.texotela.co.uk)
    * @type     jQuery
    * @param    String to  Element to copy to
    * @param    String which  (optional) Specifies which options should be copied - 'all' or 'selected'. Default is 'selected'
    * @example  $("#myselect").copyOptions("#myselect2"); // copy selected options from 'myselect' to 'myselect2'
    * @example  $("#myselect").copyOptions("#myselect2","selected"); // same as above
    * @example  $("#myselect").copyOptions("#myselect2","all"); // copy all options from 'myselect' to 'myselect2'
    *
    */
    $.fn.copyOptions = function(to, which) {
        var w = which || "selected";
        if ($(to).size() == 0) return this;
        this.each(
		function() {
		    if (this.nodeName.toLowerCase() != "select") return this;
		    // get options
		    var o = this.options;
		    // get number of options
		    var oL = o.length;
		    for (var i = 0; i < oL; i++) {
		        if (w == "all" || (w == "selected" && o[i].selected)) {
		            $(to).addOption(o[i].value, o[i].text);
		        }
		    }
		}
	);
        return this;
    };

    /**
    * Checks if a select box has an option with the supplied value
    *
    * @name     containsOption
    * @author   Sam Collett (http://www.texotela.co.uk)
    * @type     Boolean|jQuery
    * @param    String|RegExp value  Which value to check for. Can be a string or regular expression
    * @param    Function fn          (optional) Function to apply if an option with the given value is found.
    * Use this if you don't want to break the chaining
    * @example  if($("#myselect").containsOption("val1")) alert("Has an option with the value 'val1'");
    * @example  if($("#myselect").containsOption(/^val/i)) alert("Has an option with the value starting with 'val'");
    * @example  $("#myselect").containsOption("val1", copyoption).doSomethingElseWithSelect(); // calls copyoption (user defined function) for any options found, chain is continued
    *
    */
    $.fn.containsOption = function(value, fn) {
        var found = false;
        var v = value;
        var vT = typeof (v);
        var fT = typeof (fn);
        // has to be a string or regular expression (object in IE, function in Firefox)
        if (vT != "string" && vT != "function" && vT != "object") return fT == "function" ? this : found;
        this.each(
		function() {
		    if (this.nodeName.toLowerCase() != "select") return this;
		    // option already found
		    if (found && fT != "function") return false;
		    // get options
		    var o = this.options;
		    // get number of options
		    var oL = o.length;
		    for (var i = 0; i < oL; i++) {
		        if (v.constructor == RegExp) {
		            if (o[i].value.match(v)) {
		                found = true;
		                if (fT == "function") fn.call(o[i], i);
		            }
		        }
		        else {
		            if (o[i].value == v) {
		                found = true;
		                if (fT == "function") fn.call(o[i], i);
		            }
		        }
		    }
		}
	);
        return fT == "function" ? this : found;
    };

    /**
    * Returns values which have been selected
    *
    * @name     selectedValues
    * @author   Sam Collett (http://www.texotela.co.uk)
    * @type     Array
    * @example  $("#myselect").selectedValues();
    *
    */
    $.fn.selectedValues = function() {
        var v = [];
        this.find("option:selected").each(
		function() {
		    v[v.length] = this.value;
		}
	);
        return v;
    };

    /**
    * Returns options which have been selected
    *
    * @name     selectedOptions
    * @author   Sam Collett (http://www.texotela.co.uk)
    * @type     jQuery
    * @example  $("#myselect").selectedOptions();
    *
    */
    $.fn.selectedOptions = function() {
        return this.find("option:selected");
    };

})(jQuery);
if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();

/// <reference name="MicrosoftAjaxTimer.debug.js" />
/// <reference name="MicrosoftAjaxWebForms.debug.js" />
/// <reference name="AjaxControlToolkit.ExtenderBase.BaseScripts.js" assembly="AjaxControlToolkit" />

Type.registerNamespace('EMG.widgets.ui.ajax.controls.WidgetPreview');

EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs = function(widgetDelegate) {
    /// <summary>
    /// Event arguments for the RatingBehavior's rated event
    /// </summary>
    /// <param name="rating" type="Number" integer="true">
    /// Rating
    /// </param>
    EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs.initializeBase(this);
    this._widgetDelegate = widgetDelegate;
}
EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs.prototype = {
    get_WidgetDelegate : function() {
        /// <value type="Number" integer="true">
        /// Rating
        /// </value>
        return this._widgetDelegate;
    }
}
EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs.registerClass('EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs', Sys.EventArgs);


EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior = function(element) {
    EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior.initializeBase(this, [element]);
    
    // boolean values
    this._addedWidgets = false;

    // Property Variables
    this._deleteToken = "";
    this._editToken = "";
    this._previewToken = "";
    this._dateToken = "";
    this._loadingToken = "";
    this._emptyImage = "";
    this._backToken = "";
    this._publishToken = "";
    this._noWidgetsToken = "";
    this._nameToken = "";
    this._typeToken = "";
    this._alertToken = "";
    this._newsletterToken = "";
    this._workspaceToken = "";
    this._resultsPerPageToken = "";
    
    this._loadingArea = null;
    this._contentArea = null;
    this._tableArea = null;
    this._messageArea = null;
    this._previewArea = null;
    this._previewContentArea = null;
    this._flashArea = null;
    this._noWidgetsArea = null;
    this._previewPopupExtender = null;
    
    this._firstImage = "";
    this._lastImage = "";
    this._nextImage = "";
    this._prevImage = "";
    this._firstToken = "";
    this._previousToken = "";
    this._nextToken = "";
    this._lastToken = "";
    
    // Array of Widgets
    this._widgets = [];
    
    // The _widgetEditHandler is a reference to the event handler that all the widget
    // elements of our widgets will be wired up to
    this._widgetEditHandler = null;
    // The _widgetPreviewHandler is a reference to the event handler that all the widget
    // elements of our widgets will be wired up to
    this._widgetPreviewHandler = null;
    // The _widgetDeleteHandler is a reference to the event handler that all the widget
    // elements of our widgets will be wired up to
    this._widgetDeleteHandler = null;
    // The _previewBackHandler is a reference to the event handler that all the widget
    // preview back will be assigned to 
    this._previewBackHandler = null;
    // The _widgetPublishHandler is a reference to the event handler that all the widget
    // preview back will be assigned to 
    this._widgetPublishHandler = null;
    
}
EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior.prototype = {
    initialize: function() {
        EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior.callBaseMethod(this, 'initialize');

        // Wrap the content in a new element
        var root = this.get_element();
        // Create the handlers used by the control objects
        this._widgetEditHandler = Function.createDelegate(this, this._widgetEdit);
        this._widgetPreviewHandler = Function.createDelegate(this, this._widgetPreview);
        this._widgetDeleteHandler = Function.createDelegate(this, this._widgetDelete);
        this._widgetPublishHandler = Function.createDelegate(this, this._widgetPublish);
        this._previewBackHandler = Function.createDelegate(this, this._previewBack);

        // Add the loading area to the widget
        this._addLoadingArea(root);
        this._addContentArea(root);
        this._addTableArea(this._contentArea);
        this._addPreviewArea(root);
        this._addMessageArea(root);
        this._addFlashArea(root);
        this._addNoWidgetsArea(root);
        this._previewPopupExtender = this._getPreviewPopupExtender();

        CommonToolkitScripts.setVisible(this._noWidgetsArea, false);
        CommonToolkitScripts.setVisible(this._loadingArea, false);
        CommonToolkitScripts.setVisible(this._messageArea, false);
        CommonToolkitScripts.setVisible(this._contentArea, false);
        CommonToolkitScripts.setVisible(this._previewArea, false);
        CommonToolkitScripts.setVisible(this._flashArea, false);

    },

    _addNoWidgetsArea: function(root) {
        if (root) {
            var noWidgets = document.createElement('div');
            noWidgets.innerHTML = this._noWidgetsToken;
            Sys.UI.DomElement.addCssClass(noWidgets, "ajax__widget_noWidgets");
            root.appendChild(noWidgets);
            this._noWidgetsArea = noWidgets;
        }
    },

    _addLoadingArea: function(root) {
        if (root) {
            var loadingContent = document.createElement('div');
            loadingContent.innerHTML = this._loadingToken + "...";
            Sys.UI.DomElement.addCssClass(loadingContent, "ajax__widget_loading");
            root.appendChild(loadingContent);
            this._loadingArea = loadingContent;
        }
    },

    _addContentArea: function(root) {
        if (root) {
            var contentArea = document.createElement('div');
            Sys.UI.DomElement.addCssClass(contentArea, "ajax__widget_content_area");
            root.appendChild(contentArea);
            this._contentArea = contentArea;
        }
    },

    _addTableArea: function(contentArea) {
        if (contentArea) {

            // Create the table container
            var table = document.createElement('table');
            table.id = "widgetListTable";
            Sys.UI.DomElement.addCssClass(table, "tablesorter");

            // Create the table header container
            var tableHeader = document.createElement('thead');

            // Create a row for the header
            var tableHeaderRow = document.createElement('tr');

            // Create column for each item in the list
            var tableHeaderName = document.createElement('th');
            tableHeaderName.innerHTML = this.get_nameToken();

            var tableHeaderType = document.createElement('th');
            tableHeaderType.innerHTML = this.get_typeToken();

            var tableHeaderDate = document.createElement('th');
            tableHeaderDate.innerHTML = this.get_dateToken();

            var tableHeaderControls = document.createElement('th');
            tableHeaderControls.innerHTML = "&nbsp";

            // Append the column names to the table header row
            tableHeaderRow.appendChild(tableHeaderName);
            tableHeaderRow.appendChild(tableHeaderType);
            tableHeaderRow.appendChild(tableHeaderDate);
            tableHeaderRow.appendChild(tableHeaderControls);
            tableHeader.appendChild(tableHeaderRow);
            table.appendChild(tableHeader);

            // Create the table body container
            var tableBody = document.createElement('tbody');
            table.appendChild(tableBody);

            this._tableArea = table;
        }
    },

    _addPreviewArea: function(root) {
        if (root) {
            var previewArea = document.createElement('div');
            Sys.UI.DomElement.addCssClass(previewArea, "ajax__widget_preview_area");
            root.appendChild(previewArea);
            this._previewArea = previewArea;

            // Add backControl
            var cntrlArea = document.createElement('div');
            Sys.UI.DomElement.addCssClass(cntrlArea, "ajax__widget_preview_control");
            previewArea.appendChild(cntrlArea);

            var headerArea = document.createElement('div');
            Sys.UI.DomElement.addCssClass(headerArea, "ajax__widget_preview_control_header");
            cntrlArea.appendChild(headerArea);

            var backArea = document.createElement('a');
            backArea.href = "javascript:void(0)";
            Sys.UI.DomElement.addCssClass(backArea, "ajax__widget_preview_control_back_link");
            Sys.UI.DomElement.addCssClass(backArea, "button");
            backArea.innerHTML = "<span>" + this._backToken + "</span>";
            $addHandler(backArea, "click", this._previewBackHandler);
            headerArea.appendChild(backArea);

            var contentArea = document.createElement('div');
            Sys.UI.DomElement.addCssClass(contentArea, "ajax__widget_preview_control_content");
            cntrlArea.appendChild(contentArea);
            this._previewContentArea = contentArea;
        }
    },

    _getPreviewPopupExtender: function() {
        var element = this.get_element();
        var parent = $get("wPreviewContainer");
        var content = this.get_PreviewArea();

        var extender = Sys.UI.Behavior.getBehaviorByName(element, 'PopupBehavior');

        if (!extender && AjaxControlToolkit.PopupBehavior) {
            extender = $create(AjaxControlToolkit.PopupBehavior,
            { parentElement: parent }, null, null, content);
        }
        return extender;
    },

    _addFlashArea: function(root) {
        if (root) {
            var flashArea = document.createElement('div');
            Sys.UI.DomElement.addCssClass(flashArea, "ajax__widget_flash_area");
            root.appendChild(flashArea);
            this._flashArea = flashArea;
        }
    },

    _addMessageArea: function(root) {
        if (root) {
            var messageArea = document.createElement('div');
            Sys.UI.DomElement.addCssClass(messageArea, "ajax__widget_message_area");
            root.appendChild(messageArea);
            this._messageArea = messageArea;
        }
    },

    showContentArea: function() {
        CommonToolkitScripts.setVisible(this._contentArea, true);
        CommonToolkitScripts.setVisible(this._loadingArea, false);
        CommonToolkitScripts.setVisible(this._previewArea, false);
        CommonToolkitScripts.setVisible(this._messageArea, false);
        CommonToolkitScripts.setVisible(this._flashArea, false);
        CommonToolkitScripts.setVisible(this._noWidgetsArea, false);
    },

    showFlashArea: function() {
        CommonToolkitScripts.setVisible(this._flashArea, true);
        CommonToolkitScripts.setVisible(this._loadingArea, false);
        CommonToolkitScripts.setVisible(this._previewArea, false);
        CommonToolkitScripts.setVisible(this._messageArea, false);
        CommonToolkitScripts.setVisible(this._noWidgetsArea, false);
        CommonToolkitScripts.setVisible(this._contentArea, false);
    },

    showPreviewArea: function() {
        CommonToolkitScripts.setVisible(this._previewArea, false);
        CommonToolkitScripts.setVisible(this._loadingArea, false);
        CommonToolkitScripts.setVisible(this._contentArea, true);
        CommonToolkitScripts.setVisible(this._messageArea, false);
        CommonToolkitScripts.setVisible(this._noWidgetsArea, false);
        CommonToolkitScripts.setVisible(this._flashArea, false);
    },

    showMessageArea: function() {
        CommonToolkitScripts.setVisible(this._messageArea, true);
        CommonToolkitScripts.setVisible(this._loadingArea, false);
        CommonToolkitScripts.setVisible(this._contentArea, false);
        CommonToolkitScripts.setVisible(this._previewArea, false);
        CommonToolkitScripts.setVisible(this._noWidgetsArea, false);
        CommonToolkitScripts.setVisible(this._flashArea, false);
    },

    showLoadingArea: function() {
        CommonToolkitScripts.setVisible(this._loadingArea, true);
        CommonToolkitScripts.setVisible(this._messageArea, false);
        CommonToolkitScripts.setVisible(this._contentArea, false);
        CommonToolkitScripts.setVisible(this._previewArea, false);
        CommonToolkitScripts.setVisible(this._noWidgetsArea, false);
        CommonToolkitScripts.setVisible(this._flashArea, false);
    },

    showNoWidgetsArea: function() {
        CommonToolkitScripts.setVisible(this._noWidgetsArea, true);
        CommonToolkitScripts.setVisible(this._loadingArea, false);
        CommonToolkitScripts.setVisible(this._messageArea, false);
        CommonToolkitScripts.setVisible(this._contentArea, false);
        CommonToolkitScripts.setVisible(this._previewArea, false);
        CommonToolkitScripts.setVisible(this._flashArea, false);
    },

    disposeWidget: function(widget, removeFromDOM) {
        if (widget && widget.content) {

            // find the index of the widget to delete in the widgets array
            var trueWidgetIndex = Array.indexOf(this._widgets, widget);
            Array.removeAt(this._widgets, trueWidgetIndex);
            var id = widget.content.delegateId;

            // widget.animation = null;
            widget.delegateObj = null;
            widget.content._original = null;
            widget.content._previewCtrl = null;
            widget.content._deleteCtrl = null;
            widget.content._editCtrl = null;
            widget.content._publishCtrl = null;
            widget.content.delegateId = null;
            widget.content = null;

            if (removeFromDOM) {
                // set the page size to the max and clear any filters to delete
                $(this._tableArea).trigger("setPageSize", [500]);
                $(this._tableArea).trigger("clearFilter");
                $get("typeFilterCtrl").selectedIndex = 0;
                $get("searchTextInputCtrl").value = "";

                var config = $(this._tableArea).trigger("getConfig")[0].config;

                for (var i = this._tableArea.tBodies[0].rows.length - 1; i >= 0; i--) {
                    if (this._tableArea.tBodies[0].rows[i].id == id) {

                        var trueRowIndex = Array.indexOf(this._tableArea.tBodies[0].rows, this._tableArea.tBodies[0].rows[i]);
                        this._tableArea.tBodies[0].deleteRow(trueRowIndex);

                        if (this._tableArea.tBodies[0].rows.length > 0) {
                            $(this._tableArea).trigger("reconstruct");
                            $(this._tableArea).trigger("update");
                            $(this._tableArea).trigger("appendCache");
                            $(this._tableArea).trigger("sorton", [config.sortList]);
                        }
                    }
                }
            }

        }
    },

    removeWidgetById: function(id) {
        for (var i = this._widgets.length - 1; i >= 0; i--) {
            var widget = this._widgets[i];
            if (widget && widget.content && widget.delegateObj && widget.content.delegateId == id) {
                this.disposeWidget(widget, true);
                break;
            }
        }
        // update the index on all widgets
        // for(var i=0; i<= this._widgets.length; i++) {
        //    widget._index = i;
        // }
    },

    removeAllWidgets: function() {
        for (var i = this._widgets.length - 1; i > 0; i--) {
            this.disposeWidget(this._widgets[i], false);
        }
    },

    dispose: function() {

        // Wipe the _widgets collection.  We're careful to wipe any expando properties
        // which could cause memory leaks in IE6.

        this.removeAllWidgets();
        this._widgets = null;

        // wipe out the events
        this._widgetEditHandler = null;
        this._widgetPreviewHandler = null;
        this._widgetDeleteHandler = null;
        this._widgetPublishHandler = null;
        this._previewBackHandler = null;

        // clean up
        this._loadingArea = null;
        this._contentArea = null;
        this._tableArea = null;
        this._messageArea = null;
        this._previewArea = null;
        this._flashArea = null;

        this._previewContentArea = null;

        // clean up string properties
        this._deleteToken = null;
        this._editToken = null;
        this._previewToken = null;
        this._dateToken = null;
        this._loadingToken = null;
        this._emptyImage = null;
        this._backToken = null;
        this._nameToken = null;
        this._typeToken = null;
        this._alertToken = null;
        this._newsletterToken = null;
        this._workspaceToken = null;
        this._resultsPerPageToken = null;

        this._firstImage = null;
        this._lastImage = null;
        this._nextImage = null;
        this._prevImage = null;
        this._firstToken = null;
        this._previousToken = null;
        this._nextToken = null;
        this._lastToken = null;

        $clearHandlers(this.get_element());
        EMG.Toolkit.Web.TableSorterBehavior.callBaseMethod(this, 'dispose');
        EMG.Toolkit.Web.TableSorterPagingBehavior.callBaseMethod(this, 'dispose');
        EMG.Toolkit.Web.TableSorterFilterBehavior.callBaseMethod(this, 'dispose');
        EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior.callBaseMethod(this, 'dispose');
    },

    _previewBack: function(evenetArgs) {
        var h = this.get_events().getHandler('previewBack');
        //Only Invoke the event if any handler is registered beofre     
        if (h) h(this, new EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs(null));
    },

    _widgetEdit: function(eventArgs) {
        var _delegate = eventArgs.target.widgetDelegate;
        var h = this.get_events().getHandler('widgetEdit');
        //Only Invoke the event if any handler is registered beofre     
        if (h) h(this, new EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs(_delegate));
    },

    _widgetDelete: function(eventArgs) {
        var _delegate = eventArgs.target.widgetDelegate;
        var h = this.get_events().getHandler('widgetDelete');
        //Only Invoke the event if any handler is registered beofre     
        if (h) h(this, new EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs(_delegate));
    },

    _widgetPreview: function(eventArgs) {
        var _delegate = eventArgs.target.widgetDelegate;
        var h = this.get_events().getHandler('widgetPreview');
        //Only Invoke the event if any handler is registered beofre     
        if (h) h(this, new EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs(_delegate));
    },

    _widgetPublish: function(eventArgs) {
        var _delegate = eventArgs.target.widgetDelegate;
        var h = this.get_events().getHandler('widgetPublish');
        //Only Invoke the event if any handler is registered beofre     
        if (h) h(this, new EMG.widgets.ui.ajax.controls.WidgetPreviewEventArgs(_delegate));
    },

    findParentElement: function(node, tagName, className) {
        /* USE: TO FIND THE PARENT OF A NODE USING TAGNAME AND CLASSNAME AS THE SEARCH CRETERIA. */
        /* NOTE: ONLY RETURNS THE FIRST NODE FOUND */
        for (; node != null; node = node.parentNode) {
            if (className && className.length > 0) {
                if (node.nodeType == 1 &&
			        node.nodeName.toUpperCase() == tagName.toUpperCase() &&
			        (node.className == className || node.id == className)
			        ) {
                    return node;
                }
            }
            else if (node.nodeType == 1 && node.nodeName.toUpperCase() == tagName.toUpperCase()) {
                return node;
            }
        }
        return null;
    },

    addWidget: function(widgetDelegate) {
        if (!this._addedWidgets) {
            this._addedWidgets = true;
        }
        // Wrap the content in a new element
        var root = this.get_element();
        // Show content area
        this.showContentArea();

        if (this._contentArea == null) {
            this._addContentArea();
        }
        var contentArea = this._contentArea;
        var tableArea = this._tableArea;

        // Empty out any content that may be in container 
        if (this._widgets.length == 0) {
            contentArea.appendChild(tableArea);
        }

        // Set the tableBody var to append rows to the list
        var tableBody = tableArea.lastChild;

        // Add a new row to the table body to fill with widget data
        var tableBodyRow = document.createElement('tr');
        tableBodyRow.id = widgetDelegate.Id;
        tableBody.appendChild(tableBodyRow);

        // Create Name container
        var name = document.createElement('td');
        if (document.all || name.innerText) {
            name.innerText = widgetDelegate.Name;
        } else {
            name.textContent = widgetDelegate.Name;
        }

        // Create Type container
        var type = document.createElement('td');
        switch (widgetDelegate.Type) {
            case 0:
                type.innerHTML = this.get_alertToken();
                break;
            case 1:
                type.innerHTML = this.get_workspaceToken();
                break;
            case 2:
                type.innerHTML = this.get_newsletterToken();
                break;
        }

        // Create Date container
        var cDate = document.createElement('td');
        if (widgetDelegate.CreationDateTime == widgetDelegate.LastModifiedDateTime) {
            cDate.innerHTML = "<span style=\"display:none;visibility:hidden\">" + widgetDelegate.LastModifiedTicks + "</span>" + widgetDelegate.CreationDateTime;
        }
        else {
            cDate.innerHTML = "<span style=\"display:none;visibility:hidden\">" + widgetDelegate.LastModifiedTicks + "</span>" + widgetDelegate.LastModifiedDateTime;
        }

        // Create Controls container
        var controlsContainer = document.createElement('td');

        var editCtrl = document.createElement('a');
        editCtrl.innerHTML = this._editToken;
        editCtrl.href = "#";
        editCtrl.widgetDelegate = widgetDelegate;
        $addHandler(editCtrl, "click", this._widgetEditHandler);
        Sys.UI.DomElement.addCssClass(editCtrl, "ajax__widget_control_link");

        var deleteCtrl = document.createElement('a');
        deleteCtrl.innerHTML = this._deleteToken;
        deleteCtrl.href = "#";
        deleteCtrl.widgetDelegate = widgetDelegate;
        $addHandler(deleteCtrl, "click", this._widgetDeleteHandler);
        Sys.UI.DomElement.addCssClass(deleteCtrl, "ajax__widget_control_link");

        // Add Preview Control
        var previewCtrl = document.createElement('a');
        previewCtrl.innerHTML = this._previewToken;
        previewCtrl.href = "#";
        previewCtrl.widgetDelegate = widgetDelegate;
        $addHandler(previewCtrl, "click", this._widgetPreviewHandler);
        Sys.UI.DomElement.addCssClass(previewCtrl, "ajax__widget_control_link");

        // Add Publish Control
        var publishCtrl = document.createElement('a');
        publishCtrl.innerHTML = this._publishToken;
        publishCtrl.href = "#";
        publishCtrl.widgetDelegate = widgetDelegate;
        $addHandler(publishCtrl, "click", this._widgetPublishHandler);
        Sys.UI.DomElement.addCssClass(publishCtrl, "ajax__widget_control_link");

        var pipe = document.createElement('span');
        pipe.innerHTML = "|";
        Sys.UI.DomElement.addCssClass(pipe, "ajax__widget_control_pipe");

        // Add delete/preview/edit 
        controlsContainer.appendChild(editCtrl);
        controlsContainer.appendChild(pipe.cloneNode(true));
        controlsContainer.appendChild(previewCtrl);
        controlsContainer.appendChild(pipe.cloneNode(true));
        controlsContainer.appendChild(publishCtrl);
        controlsContainer.appendChild(pipe.cloneNode(true));
        controlsContainer.appendChild(deleteCtrl);

        // Append table row child nodes
        tableBodyRow.appendChild(name);
        tableBodyRow.appendChild(type);
        tableBodyRow.appendChild(cDate);
        tableBodyRow.appendChild(controlsContainer);
        tableBodyRow.delegateId = widgetDelegate.Id;

        // Create the new pane object
        var widget = {};
        widget.delegateObj = widgetDelegate;
        widget.content = tableBodyRow;

        // Remove any style facets (possibly) automatically applied by
        // CSS selectors so they don't interfere with UI/layout
        contentArea.style.border = '';
        contentArea.style.margin = '';
        contentArea.style.padding = '';

        // Add the new pane at the bottom of the accordion
        Array.add(this._widgets, widget);

    },

    addWidgets: function(widgetDelegate) {
        if (!this._addedWidgets) {
            this._addedWidgets = true;
        }
        // Wrap the content in a new element
        var root = this.get_element();
        // Show content area
        this.showContentArea();

        if (this._contentArea == null) {
            this._addContentArea();
        }
        var contentArea = this._contentArea;
        var tableArea = this._tableArea;

        // Empty out any content that may be in container 
        if (this._widgets.length == 0) {
            contentArea.appendChild(tableArea);
        }

        // Set the tableBody var to append rows to the list
        var tableBody = tableArea.lastChild;

        // start for loop logic here
        for (var i = 0; i < widgetDelegate.length; i++) {
            // Add a new row to the table body to fill with widget data
            var tableBodyRow = document.createElement('tr');
            tableBodyRow.id = widgetDelegate[i].Id;
            tableBody.appendChild(tableBodyRow);

            // Create Name container
            var name = document.createElement('td');
            if (document.all || name.innerText) {
                name.innerText = widgetDelegate[i].Name;
            } else {
                name.textContent = widgetDelegate[i].Name;
            }

            // Create Type container
            var type = document.createElement('td');
            switch (widgetDelegate[i].Type) {
                case 0:
                    type.innerHTML = this.get_alertToken();
                    break;
                case 1:
                    type.innerHTML = this.get_workspaceToken();
                    break;
                case 2:
                    type.innerHTML = this.get_newsletterToken();
                    break;
            }

            // Create Date container
            var cDate = document.createElement('td');
            if (widgetDelegate[i].CreationDateTime == widgetDelegate[i].LastModifiedDateTime) {
                cDate.innerHTML = "<span style=\"display:none;visibility:hidden\">" + widgetDelegate[i].LastModifiedTicks + "</span>" + widgetDelegate[i].CreationDateTime;
            }
            else {
                cDate.innerHTML = "<span style=\"display:none;visibility:hidden\">" + widgetDelegate[i].LastModifiedTicks + "</span>" + widgetDelegate[i].LastModifiedDateTime;
            }

            // Create Controls container
            var controlsContainer = document.createElement('td');

            var editCtrl = document.createElement('a');
            editCtrl.innerHTML = this._editToken;
            editCtrl.href = "#";
            editCtrl.widgetDelegate = widgetDelegate[i];
            $addHandler(editCtrl, "click", this._widgetEditHandler);
            Sys.UI.DomElement.addCssClass(editCtrl, "ajax__widget_control_link");

            var deleteCtrl = document.createElement('a');
            deleteCtrl.innerHTML = this._deleteToken;
            deleteCtrl.href = "#";
            deleteCtrl.widgetDelegate = widgetDelegate[i];
            $addHandler(deleteCtrl, "click", this._widgetDeleteHandler);
            Sys.UI.DomElement.addCssClass(deleteCtrl, "ajax__widget_control_link");

            // Add Preview Control
            var previewCtrl = document.createElement('a');
            previewCtrl.innerHTML = this._previewToken;
            previewCtrl.href = "#";
            previewCtrl.widgetDelegate = widgetDelegate[i];
            $addHandler(previewCtrl, "click", this._widgetPreviewHandler);
            Sys.UI.DomElement.addCssClass(previewCtrl, "ajax__widget_control_link");

            // Add Publish Control
            var publishCtrl = document.createElement('a');
            publishCtrl.innerHTML = this._publishToken;
            publishCtrl.href = "#";
            publishCtrl.widgetDelegate = widgetDelegate[i];
            $addHandler(publishCtrl, "click", this._widgetPublishHandler);
            Sys.UI.DomElement.addCssClass(publishCtrl, "ajax__widget_control_link");

            var pipe = document.createElement('span');
            pipe.innerHTML = "|";
            Sys.UI.DomElement.addCssClass(pipe, "ajax__widget_control_pipe");

            // Add delete/preview/edit 
            controlsContainer.appendChild(editCtrl);
            controlsContainer.appendChild(pipe.cloneNode(true));
            controlsContainer.appendChild(previewCtrl);
            controlsContainer.appendChild(pipe.cloneNode(true));
            controlsContainer.appendChild(publishCtrl);
            controlsContainer.appendChild(pipe.cloneNode(true));
            controlsContainer.appendChild(deleteCtrl);

            // Append table row child nodes
            tableBodyRow.appendChild(name);
            tableBodyRow.appendChild(type);
            tableBodyRow.appendChild(cDate);
            tableBodyRow.appendChild(controlsContainer);
            tableBodyRow.delegateId = widgetDelegate[i].Id;

            // Create the new pane object
            var widget = {};
            widget.delegateObj = widgetDelegate[i];
            widget.content = tableBodyRow;

            // Remove any style facets (possibly) automatically applied by
            // CSS selectors so they don't interfere with UI/layout
            contentArea.style.border = '';
            contentArea.style.margin = '';
            contentArea.style.padding = '';

            // Add the new pane at the bottom of the accordion
            Array.add(this._widgets, widget);    
        }
    },

    // Bindable Events
    add_widgetEdit: function(handler) {
        this.get_events().addHandler("widgetEdit", handler);
    },
    remove_widgetEdit: function(handler) {
        this.get_events().removeHandler("widgetEdit", handler);
    },

    // Bindable Events
    add_widgetDelete: function(handler) {
        this.get_events().addHandler("widgetDelete", handler);
    },
    remove_widgetDelete: function(handler) {
        this.get_events().removeHandler("widgetDelete", handler);
    },

    // Bindable Events
    add_widgetPreview: function(handler) {
        this.get_events().addHandler("widgetPreview", handler);
    },
    remove_widgetPreview: function(handler) {
        this.get_events().removeHandler("widgetPreview", handler);
    },

    // Bindable Events
    add_previewBack: function(handler) {
        this.get_events().addHandler("previewBack", handler);
    },
    remove_previewBack: function(handler) {
        this.get_events().removeHandler("previewBack", handler);
    },

    // Bindable Events
    add_widgetPublish: function(handler) {
        this.get_events().addHandler("widgetPublish", handler);
    },
    remove_widgetPublish: function(handler) {
        this.get_events().removeHandler("widgetPublish", handler);
    },

    // Preivew Popup Extender
    get_previewPopupExtender: function() {
        return this._previewPopupExtender;
    },

    // Tokens
    get_deleteToken: function() {
        return this._deleteToken;
    },
    set_deleteToken: function(value) {
        this._deleteToken = value;
    },
    get_editToken: function() {
        return this._editToken;
    },
    set_editToken: function(value) {
        this._editToken = value;
    },
    get_previewToken: function() {
        return this._previewToken;
    },
    set_previewToken: function(value) {
        this._previewToken = value;
    },
    get_dateToken: function() {
        return this._dateToken;
    },
    set_dateToken: function(value) {
        this._dateToken = value;
    },
    get_loadingToken: function() {
        return this._loadingToken;
    },
    set_loadingToken: function(value) {
        this._loadingToken = value;
    },
    get_backToken: function() {
        return this._backToken;
    },
    set_backToken: function(value) {
        this._backToken = value;
    },
    get_noWidgetsToken: function() {
        return this._noWidgetsToken;
    },
    set_noWidgetsToken: function(value) {
        this._noWidgetsToken = value;
    },
    get_publishToken: function() {
        return this._publishToken;
    },
    set_publishToken: function(value) {
        this._publishToken = value;
    },
    get_nameToken: function() {
        return this._nameToken;
    },
    set_nameToken: function(value) {
        this._nameToken = value;
    },
    get_typeToken: function() {
        return this._typeToken;
    },
    set_typeToken: function(value) {
        this._typeToken = value;
    },
    get_alertToken: function() {
        return this._alertToken;
    },
    set_alertToken: function(value) {
        this._alertToken = value;
    },
    get_newsletterToken: function() {
        return this._newsletterToken;
    },
    set_newsletterToken: function(value) {
        this._newsletterToken = value;
    },
    get_workspaceToken: function() {
        return this._workspaceToken;
    },
    set_workspaceToken: function(value) {
        this._workspaceToken = value;
    },
    get_emptyImage: function() {
        return this._emptyImage;
    },
    set_emptyImage: function(value) {
        this._emptyImage = value;
    },
    get_PreviewArea: function() {
        return this._previewContentArea;
    },
    get_FlashArea: function() {
        return this._flashArea;
    },
    get_firstImage: function() {
        return this._firstImage;
    },
    set_firstImage: function(value) {
        this._firstImage = value;
    },
    get_lastImage: function() {
        return this._lastImage;
    },
    set_lastImage: function(value) {
        this._lastImage = value;
    },
    get_nextImage: function() {
        return this._nextImage;
    },
    set_nextImage: function(value) {
        this._nextImage = value
    },
    get_prevImage: function() {
        return this._prevImage;
    },
    set_prevImage: function(value) {
        this._prevImage = value;
    },
    get_firstToken: function() {
        return this._firstToken;
    },
    set_firstToken: function(value) {
        this._firstToken = value;
    },
    get_previousToken: function() {
        return this._previousToken;
    },
    set_previousToken: function(value) {
        this._previousToken = value;
    },
    get_nextToken: function() {
        return this._nextToken;
    },
    set_nextToken: function(value) {
        this._nextToken = value;
    },
    get_lastToken: function() {
        return this._lastToken;
    },
    set_lastToken: function(value) {
        this._lastToken = value;
    },
    get_resultsPerPageToken: function() {
        return this._resultsPerPageToken;
    },
    set_resultsPerPageToken: function(value) {
        this._resultsPerPageToken = value;
    },
    _dummy: function() {
    }
}
EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior.registerClass('EMG.widgets.ui.ajax.controls.WidgetPreview.WidgetPreviewBehavior', AjaxControlToolkit.BehaviorBase);
