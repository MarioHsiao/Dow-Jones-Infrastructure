/// <reference name="jquery.js" assembly="DowJones.Web.Mvc" />
/// <reference name="common.js" assembly="DowJones.Web.Mvc" />
/*!
**  Dow Jones UI: Abstract Canvas Module
*/
DJ.UI.AbstractCanvasModule = DJ.UI.CompositeComponent.extend({

    defaults: {
        cssClass: 'canvas-module'
    },
    
    _canvas: null,

    //baseline module objects
    _module: null,
    _moduleId: null,
    _moduleHead: null,
    _moduleTitle: null,

    // baseline objects in the head of a module
    _optionTrigger: null,
    _editMenuTrigger: null,
    _removeTrigger: null,
    _refreshTrigger: null,

    $contentArea: null,
    $contentContainer: null,
    $editArea: null,
    $footerArea: null,
    $messageArea: null,


    _toggleSpeed: "fast",

    _baseEvents: {
        canvasModuleRemove: 'CanvasModule.RemoveModuleRequest',
        canvasModuleRemoveSuccess: 'CanvasModule.RemoveModuleSuccess',
        canvasModuleRemoveFailure: 'CanvasModule.RemoveModuleFailure',
        canvasModuleEditSuccess: 'CanvasModule.EditModuleSuccess'
    },

    // Constructor
    init: function (element, meta) {
        var $meta = $.extend({ name: "AbstractCanvasModule" }, meta);
        this._super(element, $meta);

        this.events = this.events || {};
        $.extend(this.events, this._baseEvents);

        this._registerOwnerOnChildComponents($meta.childComponents);
    },

    _initializeElements: function (element) {
        element = element || this.element;
        this._header = $(".dj_module-header", element).get(0);
        this.$contentArea = $(".dj_module-content", element);
        this.$contentContainer = $(".dj_module-core", element);
        this.$loadingArea = $('.dj_module-loading-area', element);
        this.$footerArea = $(".dj_footer", element);

        this._refreshTrigger = $(".dj_module-refresh", element).get(0);

        // get baseline module objects
        this._module = this.element;
        this._moduleHead = this._header;
        this._moduleTitle = $("H3", this._moduleHead).get(0);
        this.$editArea = $(".module-edit-options", this._module);

        // get baseline objects in the body of a module
        this._optionsArea = $(".dj_module-options-area", this._module).get(0);

        this._editMenu = $(".dj_module-editMenu", this._module).get(0);

        // get trigger objects in the body of a module
        this._editMenuTrigger = $(".actions-container", this._moduleHead).get(0);
        this._saveEditsTrigger = this.$editArea.find('div.button-pane').find('.dc_btn-save').get(0);
        this._closeEditsTrigger = this.$editArea.find('div.button-pane').find('.dc_btn-cancel').get(0);
        this._removeTrigger = $(".dj_module-remove", this._moduleHead).get(0);

        this.$settings = $('.settings', this._moduleHead);
        this._maximizeButton = $('.maximize', this._moduleHead);
        this._minimizeButton = $('.minimize', this._moduleHead);


        // If no canvas has been specified (via _set_Owner), try to actively find one
        if (this._owner === null) {
            this._owner = this.$element.closest('div.dj_Canvas').findComponent(DJ.UI.AbstractCanvas);
        }

        this._initializeMenus();

        // initialize editor if we have an Editor defined
        if (this.$editArea.length > 0) {
            this.hasEditor = true;
            this._editor = this._editor || this.$editArea.find('.dj_Editor').findComponent(DJ.UI.AbstractCanvasModuleEditor);
        }

    },

    _registerOwnerOnChildComponents: function (childComponents) {
        if (!childComponents) { $dj.debug('_registerOwnerOnChildComponents: No child components found'); return; }

        for (var i = 0, len = childComponents.length; i < len; i++) {
            this.$element.find('#' + childComponents[i].id).findComponent(DJ.UI.Component).setOwner(this);
        }
    },

    dispose: function () {
        this._super();

        //baseline module objects
        this._module = null;
        this._moduleType = null;
        this._moduleHead = null;

        // baseline objects in the head of a module
        this._editMenuTrigger = null;
        this._removeTrigger = null;

        this._delegates.fireOnEditTrigger = null;
        this._delegates.fireOnRemoveTrigger = null;

        this._toggleSpeed = null;

        this.$contentArea = null;
        this.$contentContainer = null;
        this.$editArea = null;
        this.$footerArea = null;
        this.$messageArea = null;
    },

    fireOnEditMenuCommandTrigger: function (action, evt) {
        $dj.debug('Menu action:', action);
        switch (action) {

            case 'move-up':
            case 'move-down':
                this.get_Canvas().moveModule(this, action.substring('move-'.length));
                break;

            case 'remove':
                this.publish(this.events.canvasModuleRemove, { module: this });
                break;

            case 'edit':
                this.fireOnEditTrigger();
                break;

            default:
                $dj.debug('Implement menu action: ' + action);
                break;
        }

        evt.stopPropagation();
        return false;
    },

    fireOnEditTrigger: function (e) {
        this.showEditArea();
        if (this._editor) {
            this._editor.onShow();
        }
    },

    fireOnEditShown: function (e) {
        if (this._editor) {
            this._editor.onShown();
        }
    },

    fireOnEditCloseTrigger: function (e) {
        this.showModuleArea();
    },

    fireOnRemoveSuccess: function (result, userContext, methodName) {
        this.$element.hide();

        var error = $dj.getError(result);

        if (!error) {
            this.publish(this.events.canvasModuleRemoveSuccess, {
                ReturnCode: 0,
                StatusMessage: "",
                canvasId: this.get_CanvasId()
            });
            this.$element.remove();
            $("#dj_tooltip").hide();
        }
        else {
            this.publish(this.events.canvasModuleRemoveFailure, error);
        }

        return false;
    },

    fireOnRefreshTrigger: function (e) {
        this.getData(true);
    },

    fireOnSaveAndCloseEditArea: function () {
        this._saveProperties($dj.delegate(this, function () {
            this.publish(this.events.canvasModuleEditSuccess, { moduleId: this.get_moduleId(), module: this });
            this.showModuleArea();
        }));
    },

    get_CanvasId: function () {
        return this.get_Canvas().get_canvasId();
    },

    getData: function (forceCacheRefresh) {
        this._super();
    },

    remove: function () {
        this.get_Canvas().deleteModule(
            this.get_moduleId(),
            this._delegates.fireOnRemoveSuccess,
            function (err) { $dj.debug('Error removing module:', err); }
        );
    },

    hideLoadingArea: function () {
        this.$loadingArea.hide();
    },

    hideContentArea: function () {
        this.$contentArea.hide();
    },

    showContentArea: function () {
        this.hideEditArea();
        this.hideLoadingArea();
        this.$contentArea.show.apply(this.$contentArea, arguments);
    },

    showEditArea: function () {
        if (!this.hasEditor) { return; }

        var coreHeight = $(".dj_module-core", this._module).height(),
            editHeight = $(this.$editArea, this._module).height();

        if (coreHeight < editHeight) {
            $(".dj_module-core", this._module).animate({
                height: editHeight + "px"
            });
        }

        if ($.browser.msie && $.browser.version.indexOf("7.0") == 0) {
            // ie 7, don't slide
            $(this.$editArea).show();
            this._delegates.fireOnEditShown();
        }
        else {
            $(this.$editArea).animate({ // show edit panel
                height: 'toggle'
            }, {
                complete: this._delegates.fireOnEditShown
            });
        }
        $(this._editMenuTrigger).hide(); // hide gear while edit panel is open
    },

    hideEditArea: function () {
        if (!this.hasEditor) { return; }

        var contentHeight = 0;
        $(".dj_module-core", this._module).children().each(function () {
            if ($(this).is(':visible')) {
                contentHeight = contentHeight + $(this).outerHeight(true);
            }
        });

        $(".dj_module-core", this._module).animate({
            height: contentHeight + "px" //animate the core's height back to the content height
        }, function () {
            $(".dj_module-core", this._module).height(""); //after the animation is complete , set the core's height to null
        });

        // only hide if not hidden already. prevent the flickr effect when showXXXArea is called
        if (this.$editArea.is(':visible')) {
            $(this.$editArea).animate({ // hide the edit panel
                height: 'toggle'
            });
        }

        $(this._editMenuTrigger).show(); // show gear
    },

    showMessageArea: function () {
        this.hideEditArea();
        this.$contentArea.hide();

        // Create if it doesn't exist
        if (!this.$messageArea) {
            this.$messageArea =
                $("<div>")
                    .addClass("dj_message-area")
                    .insertBefore(this.$footerArea);
        }

        this.$messageArea.show('highlight');
    },

    hideMessageArea: function () {
        if (this.$messageArea) {
            this.$messageArea.html('').hide();
        }
    },

    showMessage: function (message) {
        this.hideLoadingArea();
        this.showMessageArea();
        this.$messageArea.html(message);
    },

    showErrorMessage: function (error) {
        var parsedError = $dj.getError(error);

        if (!parsedError)
            return;

        var formattedError = $dj.formatError(parsedError);

        this.showMessage(formattedError);
    },

    showLoadingArea: function () {
        this.$loadingArea.show();
        this.hideEditArea();
        this.hideMessageArea();
    },


    /// <summary>
    /// Generic function to determine whether to show content area or message area
    /// </summary>
    showModuleArea: function () {
        this.hideLoadingArea();

        if (this.$messageArea && this.$messageArea.length) {
            this.showMessageArea();
        }
        else {
            this.hideMessageArea();
            this.showContentArea();
        }
    },

    get_Canvas: function () {
        return this._owner;
    },

    get_EditMenu: function () {
        return this._editMenu;
    },

    get_moduleTitle: function () {
        return $(this._moduleTitle).html();
    },

    set_moduleTitle: function (value) {
        $(this._moduleTitle).html(value);
    },

    _initializeDelegates: function () {
        this._delegates = $.extend({}, {
            fireOnMaximize: $dj.delegate(this, this.maximize),
            fireOnMinimize: $dj.delegate(this, this.minimize),
            fireOnEditCloseTrigger: $dj.delegate(this, this.fireOnEditCloseTrigger),
            fireOnEditTrigger: $dj.delegate(this, this.fireOnEditTrigger),
            fireOnEditShown: $dj.delegate(this, this.fireOnEditShown),
            fireOnEditMenuCommandTrigger: $dj.delegate(this, this.fireOnEditMenuCommandTrigger),
            fireOnRefreshTrigger: $dj.delegate(this, this.fireOnRefreshTrigger),
            fireOnRemoveSuccess: $dj.delegate(this, this.fireOnRemoveSuccess),
            fireOnSaveAndCloseEditArea: $dj.delegate(this, this.fireOnSaveAndCloseEditArea)
        });
    },

    _reloadMenu: function () {
        var $modules = $('.module', '#dashboard').not('.select-a-module'),
			index = $modules.index(this._module),
            menu = this.$settings.findComponent(DJ.UI.Menu),
			menuItems = menu.options.items || DJ.UI.AbstractCanvasModule.DefaultMenuItems;

        $.each(menuItems, function (key, item) {
            switch (item.id) {
                case 'move-up':
                    menuItems[key].disabled = (index == 0) ? true : false;
                    break;
                case 'move-down':
                    menuItems[key].disabled = (index == $modules.length - 1) ? true : false;
                    break;
            }

        });
    },

    _initializeMenus: function () {
        var handleMenuCommand = this._delegates.fireOnEditMenuCommandTrigger;
        var reloadMenuCommand = $dj.delegate(this, this._reloadMenu);

        if (this.$settings.length === 0) {
            $dj.debug("Action item container is empty, module is not editable.");
            return;
        }

        var menuItems = this.options.menuItems || DJ.UI.AbstractCanvasModule.DefaultMenuItems;
        this.$settings.dj_menu({
            options: {
                items: menuItems,
                onItemClick: function (e, itemData) {
                    handleMenuCommand(itemData.data.id, e);
                },
                onShow: reloadMenuCommand

            }
        });
    },

    _initializeEventHandlers: function () {
        $(this._refreshTrigger)
            .click(this._delegates.fireOnRefreshTrigger);


        $(this._removeTrigger)
            .mousedown(function (e) { e.stopPropagation(); })
            .click(this._delegates.fireOnRemoveTrigger)
            .dj_simpleTooltip("dj_iconTip");


        $(this._saveEditsTrigger)
            .mousedown(function (e) { e.stopPropagation(); })
            .click(this._delegates.fireOnSaveAndCloseEditArea);

        $(this._closeEditsTrigger)
            .mousedown(function (e) { e.stopPropagation(); })
            .click(this._delegates.fireOnEditCloseTrigger);

        this.$settings.click(function () {
            var menu = $(this).findComponent(DJ.UI.Menu);
            if (menu) { menu.show(); }
            return false;
        });

        this._maximizeButton.click(this._delegates.fireOnMaximize);
        this._minimizeButton.click(this._delegates.fireOnMinimize);
    },

    maximize: function () {
        this.$element.removeClass('minimized');
    },

    minimize: function () {
        this.$element.addClass('minimized');
    },


    publish: function (/* string */eventName, /* object */data) {
        this._super(eventName, this._appendModuleData(data));
    },

    _appendModuleData: function (innerData) {
        return {
            sender: this,
            moduleId: this.options.moduleId,
            moduleType: this.options.moduleType,
            position: this.options.position,
            innerData: innerData,
            canvasId: this.get_Canvas().get_canvasId()
        };
    },
    
    _saveProperties: function (callback) {
        if (this._editor) {
            var props = this._editor.buildProperties();
            this._editor.saveProperties(props, callback);
        }
    },

    EOF: null

});

DJ.UI.AbstractCanvasModule.DefaultMenuItems = [
    { id: 'move-up', label: "<%= Token('moduleMenuMoveUp') %>" },
    { id: 'move-down', label: "<%= Token('moduleMenuMoveDown') %>" },
    { type: 'separator' },
    { id: 'remove', label: "<%= Token('moduleMenuRemove') %>" },
    { id: 'edit', label: "<%= Token('moduleMenuEdit') %>" }
];

DJ.UI.AbstractCanvasModule.GetParentModule = function (childElement) {
    return $(childElement).parents('.module').findComponent(DJ.UI.AbstractCanvasModule);
};
