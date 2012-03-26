/// <reference name="jquery.js" assembly="DowJones.Web.Mvc" />
/// <reference name="common.js" assembly="DowJones.Web.Mvc" />
/*!
**  Dow Jones UI: Abstract Canvas Module
*/
$dj.registerNamespace("DJ.UI");

DJ.UI.AbstractCanvasModule = DJ.UI.Component.extend({

	defaults: {
		cssClass: 'canvas-module'
	},

	_canvas: null,

	//baseline module objects
	_module: null,
	_moduleId: null,
	_moduleHead: null,
	_moduleBody: null,
	_moduleTitle: null,

	// baseline objects in the head of a module
	_optionTrigger: null,
	_editMenuTrigger: null,
	_removeTrigger: null,
	_refreshTrigger: null,

	// baseline objects in the body of a module
	_loadingArea: null,
	$editArea: null,
	$contentArea: null,

	_toggleSpeed: "fast",

	_baseEvents: {
		canvasModuleRemove: 'CanvasModule.RemoveModuleRequest',
		canvasModuleRemoveSuccess: 'CanvasModule.RemoveModuleSuccess',
		canvasModuleRemoveFailure: 'CanvasModule.RemoveModuleFailure'
	},

	// Constructor
	init: function (element, meta) {
		var $meta = $.extend({ name: "AbstractCanvasModule" }, meta);

		if ($meta.owner) {
			this._set_Owner($meta.owner);
		}
		else if ($meta.ownerId) {
			this._set_Owner($($meta.ownerId).get(0));
		}

		this._super(element, $meta);

		$.extend(this.events, this._baseEvents);

	},


	dispose: function () {
		this._super();

		this._canvas = null;

		//baseline module objects
		this._module = null;
		this._moduleType = null;
		this._moduleHead = null;
		this._moduleBody = null;

		// baseline objects in the head of a module
		this._editMenuTrigger = null;
		this._removeTrigger = null;
		this._refreshTrigger = null;

		// baseline objects in the body of a module
		this._loadingArea = null;
		this.$editArea = null;
		this.$contentArea = null;
		this._footerArea = null;

		this._delegates.fireOnEditTrigger = null;
		this._delegates.fireOnRemoveTrigger = null;

		this._toggleSpeed = null;
	},


	fireOnEditMenuCommandTrigger: function (action, evt) {
		switch (action) {

			case 'move-up':
			case 'move-down':
				this.get_Canvas().moveModule(this, action.substring('move-'.length));
				break;

			case 'remove':
				this._publish(this.events.canvasModuleRemove, { module: this });
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
		if (this._editor) { this._editor.onShow(); }
	},

	fireOnEditCloseTrigger: function (e) {
		this.showModuleArea();
	},

	fireOnRefreshTrigger: function (e) {
		this.refreshData();
	},

	fireOnRemoveSuccess: function (result, userContext, methodName) {
		this.$element.hide();

		var error = $dj.getError(result);

		if (!error) {
			this._publish(this.events.canvasModuleRemoveSuccess, {
				ReturnCode: 0,
				StatusMessage: "",
				canvasId: this.get_CanvasId()
			});
			this.$element.remove();
			$("#dj_tooltip").hide();
		}
		else {
			this._publish(this.events.canvasModuleRemoveFailure, error);
		}

		return false;
	},

	fireOnSaveAndCloseEditArea: function () {
		this.showModuleArea();
	},

	get_CanvasId: function () {
		return this.get_Canvas().get_canvasId();
	},

	getData: function () {
		this.$contentArea.showLoading();
		this._super();
	},

	refreshData: function () {
		this.getData();
		$dj.debug('Override this method if you need to do any pre or post getData() stuff, such as resetting pager or invalidating any cached results');
	},

	remove: function () {
		this.get_Canvas().deleteModule(
            this.get_moduleId(),
            this._delegates.fireOnRemoveSuccess,
            function (err) { $dj.debug('Error removing module: ' + JSON.stringify(err)); }
        );
	},

	showContentArea: function () {
		// global flag to detrmin whether or not to show/hide message area
		this.hasMessages = false;

		$(this._loadingArea).hide();
		$(this._messageArea).hide();
		this.hideEditArea();
		this.$contentArea.hideLoading();
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
		$(this._loadingArea).hide();
		$(this.$editArea).animate({ // show edit panel
			height: 'toggle'
		});
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

	showErrorMessage: function (error) {
		var parsedError = $dj.getError(error);

		if (!parsedError)
			return;

		var formattedError = $dj.formatError(parsedError);

		//        $(this._messageArea).html(formattedError);

		//        this.showMessageArea();
		this.showMessage(formattedError);
	},

	showMessage: function (message) {
		// global flag to detrmin whether or not to show/hide message area
		this.hasMessages = true;
		$(this._messageArea).html(message);
		this.showMessageArea();
	},

	showLoadingArea: function () {
		$(this._loadingArea).show();
		this.$contentArea.hide();
		$(this._messageArea).hide();
		this.$editArea.hide();
	},

	showMessageArea: function () {
		$(this._loadingArea).hide();
		this.$contentArea.hide();
		this.hideEditArea();
		$(this._messageArea).show('highlight');
	},

	/// <summary>
	/// Generic function to determine whether to show content area or message area
	/// </summary>
	showModuleArea: function () {
		if (this.hasMessages) {
			this.showMessageArea();
		}
		else {
			this.showContentArea();
		}
	},

	get_Canvas: function () {
		return this._canvas;
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


	_initializeElements: function () {
		// get baseline module objects
		this._module = this.element;
		this._moduleHead = $(".dj_module-header", this._module).get(0);
		this._moduleTitle = $("H3", this._moduleHead).get(0);
		this.$editArea = $(".module-edit-options", this._module);
		this._moduleBody = $(".dj_module-core", this._module).get(0);

		// get baseline objects in the body of a module
		this.$contentArea = $(".dj_module-content", this._moduleBody);
		this._loadingArea = $(".dj_module-loading-area", this._moduleBody).get(0);
		this._messageArea = $(".dj_module-message-area", this._moduleBody).get(0);
		this._optionsArea = $(".dj_module-options-area", this._moduleBody).get(0);
		this._footerArea = $(".dj_module-footer", this._moduleBody).get(0);

		this._editMenu = $(".dj_module-editMenu", this._module).get(0);

		// get trigger objects in the body of a module
		this._editMenuTrigger = $(".actions-container", this._moduleHead).get(0);
		this._saveEditsTrigger = this.$editArea.find('div.button-pane').find('.dc_btn-save').get(0);
		this._closeEditsTrigger = this.$editArea.find('div.button-pane').find('.dc_btn-cancel').get(0);
		this._removeTrigger = $(".dj_module-remove", this._moduleHead).get(0);
		this._refreshTrigger = $(".dj_module-refresh", this._moduleHead).get(0);


		// If no canvas has been specified (via _set_Owner), try to actively find one
		if (this._canvas === null) {
			this._canvas = this.$element.parents('div.dj_canvas').findComponent(DJ.UI.AbstractCanvas);
		}

		this._initializeMenus();

		// initialize editor if we have an Editor defined
		if (this.$editArea.length > 0) {
			this.hasEditor = true;
			this._editor = this._editor || this.$editArea.find('.dj_Editor').findComponent(DJ.UI.AbstractCanvasModuleEditor);
		}

	},

	_initializeDelegates: function () {
		this._delegates = $.extend({}, {
			fireOnEditCloseTrigger: $dj.delegate(this, this.fireOnEditCloseTrigger),
			fireOnEditTrigger: $dj.delegate(this, this.fireOnEditTrigger),
			fireOnEditMenuCommandTrigger: $dj.delegate(this, this.fireOnEditMenuCommandTrigger),
			fireOnRefreshTrigger: $dj.delegate(this, this.fireOnRefreshTrigger),
			fireOnRemoveSuccess: $dj.delegate(this, this.fireOnRemoveSuccess),
			fireOnSaveAndCloseEditArea: $dj.delegate(this, this.fireOnSaveAndCloseEditArea)
		});
	},


	_reloadMenu: function () {

		var $modules = $('.module', '#dashboard').not('.select-a-module'),
			index = $modules.index(this._module),
			menuItems = $('.settings', this._moduleHead).menu("option", 'items');

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

		$('.settings', this._moduleHead).menu("option", 'items', menuItems);

	},

	_initializeMenus: function () {
		var handleMenuCommand = this._delegates.fireOnEditMenuCommandTrigger;
		var reloadMenuCommand = $dj.delegate(this, this._reloadMenu);

		$('.settings', this._moduleHead).menu({
			show: function (event, ui) {
				reloadMenuCommand();
			},
			items: [
                {
                	name: "<%= Token('moduleMenuMoveUp') %>",
                	id: 'move-up',
                	onClick: function (evt) { return handleMenuCommand('move-up', evt); }
                }, {
                	name: "<%= Token('moduleMenuMoveDown') %>",
                	id: 'move-down',
                	onClick: function (evt) { return handleMenuCommand('move-down', evt); }
                }, {
                	name: "<%= Token('moduleMenuRemove') %>",
                	onClick: function (evt) { return handleMenuCommand('remove', evt); }
                }, {
                	type: 'separator'
                }, {
                	name: "<%= Token('moduleMenuEdit') %>",
                	onClick: function (evt) { return handleMenuCommand('edit', evt); }
                }
            ]
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
	},


	_set_Owner: function (value) {
		this._canvas = value;
	},


	_publish: function (/* string */eventName, /* object */data) {
		this.get_Canvas().publish(eventName, this, this._appendModuleData(data));
	},


	_subscribe: function (/* string */eventName, /* function() */handler) {
		this.get_Canvas().subscribe(eventName, handler);
	},


	_appendModuleData: function (innerData) {
		return {
			moduleId: this.options.moduleId,
			moduleType: this.options.moduleType,
			position: this.options.position,
			innerData: innerData,
			canvasId: this.get_Canvas().get_canvasId()
		};
	},

	EOF: null

});

DJ.UI.AbstractCanvasModule.GetParentModule = function (childElement) {
    return $(childElement).parents('.module').findComponent(DJ.UI.AbstractCanvasModule);
};
