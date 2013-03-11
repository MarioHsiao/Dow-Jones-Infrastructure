// README
//
// There are two steps to adding a property:
//
// 1. Create a member variable to store your property
// 2. Add the get_ and set_ accessors for your property.
//
// Remember that both are case sensitive!
//

Type.registerNamespace('EMG.widgets.ui.ajax.controls.WidgetDesigner');

EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior = function(element) {

    EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior.initializeBase(this, [element]);

    // Client state variables
    this._discoveryTabs = "[{\"Active\":true,\"DisplayCheckbox\":false,\"Id\":\"headlines\",\"Text\":\"Headlines\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"companies\",\"Text\":\"Companies\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"executives\",\"Text\":\"Executives\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"industries\",\"Text\":\"Industries\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"regions\",\"Text\":\"Regions\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"subjects\",\"Text\":\"Subjects\"}]";
    //this._discoveryTabs = "[{\"Active\":true,\"DisplayCheckbox\":false,\"Id\":\"headlines\",\"Text\":\"Headlines\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"companies\",\"Text\":\"Companies\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"executives\",\"Text\":\"Executives\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"industries\",\"Text\":\"Industries\"},{\"Active\":true,\"DisplayCheckbox\":true,\"Id\":\"subjects\",\"Text\":\"Subjects\"}]";
    this._discoveryTabsArea = null;

    // Properties
    this._cancel = "ui-sortable-state-disabled";
    this._containment = "window";
    this._cursor = "move";
    this._placeholder = "ui-sortable-placeholder";
    this._revert = false;
    this._appendInsideControlID = null;

    // Handlers
    this._widgetDesignerUpdateHandler = null;
    this._toggleCheckboxHandler = null;
};

EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior.prototype = {

    initialize : function() {
        EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior.callBaseMethod(this, 'initialize');

        var target = this.get_element().id;
        var appendInsideControlID = this.get_appendInsideControlID();
        
        // Handler functions
        this._toggleCheckboxHandler = Function.createDelegate(this, this._toggleCheckbox);
        this._widgetDesignerUpdateHandler = Function.createDelegate(this, this._widgetDesignerUpdate);
        
        // Get the initial state set by the server control
        if (this._discoveryTabs != null) {
            $get(target).value = this._discoveryTabs;
        }
        
        // Add the list area to the page
        this._discoveryTabsArea = this._addTabs();
        
        $("#"+appendInsideControlID).append(this._discoveryTabsArea);
        $("#"+this.get_id()+"_sortableList").sortable({appendTo: '#wSortContainer'
                                            , cancel: this.get_cancel()
                                            , containment: this.get_containment()
                                            , cursor: this.get_cursor()
                                            , helper: function(e, ui)
                                            {
                                                return $(ui).clone().css({width: ui.outerWidth()});
                                            }
                                            , placeholder: this.get_placeholder()
                                            , revert: this.get_revert()
                                            }).bind('sortupdate', this._widgetDesignerUpdateHandler);

    },

    dispose : function() {

        var target = this.get_element();
    
        // client-state variables
        this._discoveryTabs = null;
        this._discoveryTabsArea = null;

        // properties
        this._cancel = null;
        this._containement = null;
        this._cursor = null;
        this._placeholder = null;
        this._revert = null;
        this._hiddenInputText = null;
        
        // handlers
        this._toggleCheckboxHandler = null;
        this._widgetDesignerUpdateHandler = null;

        $clearHandlers(target);        
        $(target).sortable('destroy');
        EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior.callBaseMethod(this, 'dispose');
    },
    
    // Add tabs
    _addTabs : function() {

        // create a wrapper <ul> element for the <li> elements      
        var unsortedlist = document.createElement("ul"); 
            unsortedlist.id = this.get_id()+"_sortableList";
            Sys.UI.DomElement.addCssClass(unsortedlist, "ui-sortable");

        var tabs = Sys.Serialization.JavaScriptSerializer.deserialize(this._discoveryTabs);
        // for each item in the SortableListItemCollection add a <li> element
        for (var i = 0; i<tabs.length; i++) {
            if (tabs[i] != null){
                var currentTab = tabs[i];
                var tab = this._addTab(currentTab);
                
                // append the current list item added to the list
                unsortedlist.appendChild(tab);
            }
        }
        
        return unsortedlist;
    },
    
    _addTab : function(currentTab) {
        if (currentTab) {
            var listItem = document.createElement("li");
                listItem.id = currentTab.Id;
                listItem.style.width = "80%";

            // determine whether the item is active and apply corresponding css class
            if (currentTab.Active == true)
            {
                Sys.UI.DomElement.addCssClass(listItem, "ui-sortable-state-default");
            }
            else
            {
                Sys.UI.DomElement.addCssClass(listItem, "ui-sortable-state-disabled");
            }
            var checkbox;
            if (currentTab.DisplayCheckbox == true)
            {
                // add a checkbox if needed and determine whether it should be checked
                checkbox = document.createElement("input");
                checkbox.type = "checkbox";
                    if (currentTab.Active == true)
                        checkbox.defaultChecked = true;
                    Sys.UI.DomElement.addCssClass(checkbox, "ui-sortable-checkbox");
                    $addHandler(checkbox, "click", this._toggleCheckboxHandler);
            } else {
                checkbox = document.createElement("input");
                checkbox.type = "checkbox";
                    checkbox.style.display = "block";
                    checkbox.style.visibility = "hidden";
                    checkbox.style.margin = "0px 2px 0px 2px";
            }

            // append the checkbox to the list item element
            listItem.appendChild(checkbox);

            // add the text property to the span within the list item
            var text = document.createElement("div");
                Sys.UI.DomElement.addCssClass(text, "ui-sortable-text");
                text.innerHTML = currentTab.Text;

            // append the text to the list item element
            listItem.appendChild(text);

        }
        return listItem;
    },
    
    // Handlers
    _widgetDesignerUpdate : function(event, ui) {
        var item = ui.item[0];

        if ($(item).hasClass("ui-sortable-state-disabled")) {
            $(item).removeClass("ui-sortable-state-disabled");
            $(item).addClass("ui-sortable-state-default");
            
            // Check the checkbox
            if (item.firstChild.type == "checkbox")
                item.firstChild.checked = true;
        }

        this._setState();
        var h = this.get_events().getHandler("widgetDesignerUpdate");
        //Only Invoke the event if any handler is registered before   
        if (h) h(this);
    },

    _toggleCheckbox : function(e) {
        var checkbox = e.target.parentNode;

        if ($(checkbox).hasClass("ui-sortable-state-default")) {
            $(checkbox).removeClass("ui-sortable-state-default");
            $(checkbox).addClass("ui-sortable-state-disabled");
            this._setState();
            var h = this.get_events().getHandler("widgetDesignerUpdate");
            //Only Invoke the event if any handler is registered before   
            if (h) h(this);
            return;
        }

        if ($(checkbox).hasClass("ui-sortable-state-disabled")) {
            $(checkbox).removeClass("ui-sortable-state-disabled");
            $(checkbox).addClass("ui-sortable-state-default");
            this._setState();
            var h = this.get_events().getHandler("widgetDesignerUpdate");
            //Only Invoke the event if any handler is registered before   
            if (h) h(this);            
            return;
        }
    },

    // Property Accessors      
    get_cancel : function() {
        return this._cancel;
    },
    set_cancel : function(value) {
        this._cancel = value;
    },

    get_containment : function() {
        return this._containment;
    },
    set_containment : function(value) {
        this._containment = value;
    },

    get_cursor : function() {
        return this._cursor;
    },
    set_cursor : function(value) {
        this._cursor = value;
    },
    
    get_appendInsideControlID : function() {
        return this._appendInsideControlID;
    },
    set_appendInsideControlID: function(value) {
        this._appendInsideControlID = value;
    },
    
    get_discoveryTabs : function() {
        return this._discoveryTabs;
    },
    set_discoveryTabs : function(value) {
        this._discoveryTabs = value;
    },
    
    get_placeholder : function() {
        return this._placeholder;
    },
    set_placeholder : function(value) {
        this._placeholder = value;
    },
    
    get_revert : function() {
        return this._revert;
    },
    set_revert : function(value) {
        this._revert = value;
    },
    
    // Handlers
    add_widgetDesignerUpdate : function(handler) {
        this.get_events().addHandler("widgetDesignerUpdate", handler);
    },
    
    remove_widgetDesignerUpdate : function(handler) {
        this.get_events().removeHandler("widgetDesignerUpdate", handler);
    },
       
    // Client state information
    _setState : function() {
    
        // serialize the order of the elements in the list
        var sortOrder = $("#"+this.get_id()+"_sortableList").sortable('toArray');
        
        // object to hold the current state information
        var tabs = new Array();

        for (var i=0; i<sortOrder.length; i++) {
            var tab = new EMG.widgets.ui.ajax.controls.WidgetDesigner.DiscoveryTab();
            var currentTab = $("#"+sortOrder[i]);

            tab.set_id(sortOrder[i].toString());
            tab.set_text($("#"+sortOrder[i]).text());

            if ($(currentTab).hasClass("ui-sortable-state-default"))
                tab.set_active(true);
            else
                tab.set_active(false);
            if (currentTab[0].firstChild.type == "checkbox" && currentTab[0].firstChild.style.visibility == "hidden")
                tab.set_displayCheckbox(false);
            else
                tab.set_displayCheckbox(true);

            tabs.push(tab);
        }
        var json = Sys.Serialization.JavaScriptSerializer.serialize(tabs);
        this.set_discoveryTabs(json);
        this.get_element().value = json;
    },
    
    _dummy : function() {
    }
};
EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior.registerClass('EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior', AjaxControlToolkit.BehaviorBase);

EMG.widgets.ui.ajax.controls.WidgetDesigner.DiscoveryTab = function() {
    this.Active = null;
    this.DisplayCheckbox = null;
    this.Id = null;
    this.Text = null;    
};
EMG.widgets.ui.ajax.controls.WidgetDesigner.DiscoveryTab.prototype = {

    dispose : function() {
        this.Active = null;
        this.DisplayCheckbox = null;
        this.Id = null;
        this.Text = null;        
    },

    get_active : function() {
        return this.Active;
    },
    set_active : function(value) {
        this.Active = value;
    },
    
    get_displayCheckbox : function() {
        return this.DisplayCheckbox;
    },
    set_displayCheckbox : function(value) {
        this.DisplayCheckbox = value;
    },
    
    get_id : function() {
        return this.Id;
    },
    set_id : function(value) {
        this.Id = value;
    },    
    
    get_text : function() {
        return this.Text;
    },
    set_text : function(value) {
        this.Text = value;
    },
    
    _dummy : function() {
    }
};
EMG.widgets.ui.ajax.controls.WidgetDesigner.DiscoveryTab.registerClass('EMG.widgets.ui.ajax.controls.WidgetDesigner.DiscoveryTab', null, Sys.IDisposable);