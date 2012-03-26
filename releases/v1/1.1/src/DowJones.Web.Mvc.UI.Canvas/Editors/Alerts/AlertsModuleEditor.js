/*!
* AlertsModuleEditor
*   e.g. , "this._imageSize" is generated automatically.
*
*   
*  Getters and Setters are generated automatically for every Client Property during init;
*   e.g. if you have a Client Property called "imageSize" on server side code
*        get_imageSize() and set_imageSize() will be generated during init.
*  
*  These can be overriden by defining your own implementation in the script. 
*  You'd normally override the base implementation if you have extra logic in your getter/setter 
*  such as calling another function or validating some params.
*
*/

(function ($) {

    DJ.UI.AlertsModuleEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

        selectors: {
            moduleName: 'input#txtModuleName',
            description: 'textarea#txtDescription',
            editListAddControls: '.dj_edit-lists-add-controls',
            addBtn: 'a.dc_btn-add',
            userAlertsList: '.user-alerts-list',
            editListsEmptyMsg: '.dj_edit-list-empty-msg',
            alertPrincipalList: 'ul#dj_alert_principal_list',
            overlay: '.alertsOverlay'
        },

        // Default options
        defaults: {
            debug: false,
            cssClass: 'AlertsModuleEditor'
            // ,name: value     // add more defaults here separated by comma
        },

        //Alert Groups

        alertGroups: {
            "0": "Personal",
            "1": "Subscribed",
            "2": "Assigned",
            "3": "Unknown"
        },

        // Localization/Templating tokens
        tokens: {
            // name: value     // add more defaults here separated by comma
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "AlertsModuleEditor" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            this.getCanvas();

            //varialbles
            this.isAlertListPopulated = false;
            this.alertsOverlay = $(this.selectors.overlay, this.$element)[0];
            this.createAlertsOverlayFn = null;
            this.pricipalListArr = [];


            //Initialize Delegates
            $.extend(this._delegates, {
                OnAddAlertClick: $dj.delegate(this, this._onAddAlertClick),
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this._onError),
                OnKeyPressDescription: $dj.delegate(this, this._onKeyPressDescription)
            });
        },

        onShow: function () {
            if (!this.getModule() && this.alertsOverlay) {
                this.alertsOverlay.id = "alertsCreateOverlay";

                //Bind ovelay buttons
                this._bindOverlayButtons();

                //Clear the principalListArr
                this.pricipalListArr.length = 0;

                //Reset Alerts Lists
                this._resetAlertsLists();
            }

            this._initializeFormFields();
            this._populateAlertsList();
        },

        /*
        * Public methods
        */
        alertNSRefreshBinding: function () {
            var self = this;
            $(".editable-item .label", self.$alertPrincipalList).bind('mousedown', function (e) {

                var editableItem = $(this).parent();
                $(editableItem).addClass('focus').addClass('hover');
                $(editableItem).siblings().removeClass('focus');
                self.alertNsQuitEditMode('.focus');
                if ($(editableItem).find('.controls').length < 1) {
                    $(editableItem).append("<div class='controls'><ul class='dc_list'><li class='dc_item'><a href='#' class='dashboard-control dc_btn dc_btn-2 dc_btn-remove'><%= Token("moduleMenuRemove") %></a></li><li class='dc_item'><a href='#' class='dashboard-control dc_btn dc_btn-3 dc_btn-cancel'><%= Token("cancel") %></a></li></ul></div>");
                    $(".editable-item .dc_btn-save", self.$alertPrincipalList).bind('mousedown', function (e) {
                        $(editableItem).data('textValue', $(editableItem).find('input').val());
                        $(editableItem).removeClass('focus');
                        self.alertNsQuitEditMode();
                        e.stopPropagation();
                    });
                    $(".editable-item .dc_btn-cancel", self.$alertPrincipalList).bind('mousedown', function (e) {
                        $(editableItem).removeClass('focus');
                        self.alertNsQuitEditMode();
                        e.stopPropagation();
                    });
                    $(".editable-item .dc_btn-remove", self.$alertPrincipalList).bind('mousedown', function (e) {
                        $(editableItem).remove();
                        $(self.$userAlertsList + "option[value='" + $('span.label', $(editableItem)).html() + "']").removeAttr('disabled', 'disabled');

                        //Remove the item from the array
                        var removedItemId = $('span.label', $(editableItem)).attr("data-id");
                        self.removeItemFromPrincipalArray(self.pricipalListArr, removedItemId);

                        if (self.$alertPrincipalList.children().length < 1) {
                            this.$editListsEmptyMsg.removeClass('hide');
                        } else if ((self.$alertPrincipalList.children().length >= 1) && (self.$alertPrincipalList.children().length < 25)) {
                            $(".list-message", self.$editListAddControls).addClass('hide');
                            $(".add-new-list-item", self.$editListAddControlss).removeClass('hide');
                        }
                        $(editableItem).removeClass('focus');
                        self.alertNsQuitEditMode();
                        e.stopPropagation();
                    });
                }

                e.stopPropagation();
            });
        },

        removeItemFromPrincipalArray: function (arr, removeItem) {
            arr = $.grep(arr, function (value) {
                return value != removeItem;
            });

            this.pricipalListArr = arr;
        },

        alertNsQuitEditMode: function (not) {
            $(".editable-item:not(" + not + ")", this.$alertPrincipalList).each(function () {
                var $label = $(this);
                var $li = $label.closest('li');
                $li.height(20);
                $li.css('height', '');
                $li.removeClass('hover');
                if ($label.find(".controls").length > 0) {
                    $label.find(".controls").remove();
                }
            });
        },

        buildProperties: function () {
            var paramsObj = {
                "title": this.$moduleName.val(),
                "description": this.$description.val(),
                "alerts": this.getAlerts(),
                "pageId": this._canvas.get_canvasId()

                //alerts : {"Personal":[{id:"", name:""}, ],  },
                //isPagePublished: this.getCanvas().isPagePublished"true/false"
            }
            return paramsObj;
        },

        ValidateBuildProperties: function (paramsObj) {
            errObj = {};
            if (paramsObj.alerts.length < 1) { errObj.AlertListCount = 0; }
            if (paramsObj.title.length < 1) { errObj.TitleLength = 0; }
            if (errObj.AlertListCount < 1 || errObj.TitleLength === 0) { return errObj; }
        },

        callAlertsOverlay: function (personalAlertsObj) {
            var publishAlertsOverlayTemplate = this.templates.publishAlertsOverlay(personalAlertsObj);
            var overlayDiv = $("#alertsCreateOverlay");
            if (overlayDiv.length == 0)
                overlayDiv = $("#alertsCreateOverlay", this.$element);
            //show overlay
            $(".modalContent", overlayDiv).html(publishAlertsOverlayTemplate);
            overlayDiv.show().overlay({ closeOnEsc: true });

        },

        saveProperties: function (callback) {
            this.createAlertsOverlayFn = callback;
            if (this.getCanvas().options.isPublished) {
                var data = this.buildProperties();
                var privateAlertsObj = this._checkForPrivateAlerts(data);
                if (privateAlertsObj && privateAlertsObj.PrivateAlerts.length > 0) {
                    this.callAlertsOverlay(privateAlertsObj);
                }
                else { this.createAlerts(callback); }
            }
            else {
                this.createAlerts(callback);
            }
        },

        createAlerts: function (callback, data) {
            if (!data) {
                data = this.buildProperties()
            }
            var validationObj = this.ValidateBuildProperties(data);
            if (!validationObj) {
                $dj.proxy.invoke(
                {
                    url: this.options.createUpdateAlertModuleUrl,
                    data: data,
                    method: 'POST',
                    controlData: this.getCanvas().get_ControlData(),
                    preferences: this.getCanvas().get_Preferences(),
                    onSuccess: $dj.delegate(this, this._onCreateSuccess, callback),
                    onError: $dj.delegate(this, this._onCreateError, callback)
                });
            }
            else {
                this._onCreateError(callback, validationObj);
            }
        },

        fireOnEditCloseTrigger: function () {
            $(this.element).hideLoading();
            if (this.getModule()) {
                this.getModule().showContentArea();
            }
        },

        getAlerts: function () {
            var self = this;
            var alerts = [];
            $(".editable-item", this.$alertPrincipalList).each(function () {
                var alert = {};
                alert.alertId = $('span.label', $(this)).attr('data-id');
                alert.name = $('span.label', $(this)).html();
                alert.isPrivate = $('span.label', $(this)).attr('data-isPrivate');
                alerts[alerts.length] = alert;
            });
            return alerts;
        },

        /*
        * Private methods
        */

        _initializeElements: function (ctx) {
            this.$moduleName = $(this.selectors.moduleName, ctx);
            this.$description = $(this.selectors.description, ctx);
            this.$editListsEmptyMsg = $(this.selectors.editListsEmptyMsg, ctx);
            this.$editListAddControls = $(this.selectors.editListAddControls, ctx);
            this.$userAlertsList = $(this.selectors.userAlertsList, this.$editListAddControls);
            this.$addBtn = $(this.selectors.addBtn, this.$editListAddControls);
            this.$alertPrincipalList = $(this.selectors.alertPrincipalList, ctx);
        },

        _initializeFormFields: function () {
            this.$moduleName.val(this.options.moduleName);
            this.$description.val(this.options.moduleDescription);
        },

        _initializeEvents: function () {
            var self = this;
            this.$alertPrincipalList.sortable({ placeholder: "drop-placeholder", start: function () { self.alertNsQuitEditMode(); }, stop: function () { self.alertNsQuitEditMode(); $(".editable-item", this.$alertPrincipalList).removeClass('focus'); } });
            this.$alertPrincipalList.disableSelection();
            $(".editable-item", this.$alertPrincipalList).live('mouseover', function (e) {
                $(this).addClass('hover');
            });
            $(".editable-item", this.$alertPrincipalList).live('mouseout', function (e) {
                $(this).removeClass('hover');
            });
            this.$addBtn.bind('click', this._delegates.OnAddAlertClick);
            this.$description.bind('keypress', this._delegates.OnKeyPressDescription);
        },


        _onKeyPressDescription: function (e) {
            if (this.$description.val().length > 250) {
                var truncatedDesc = this.$description.val().substring(0, 250);
                this.$description.val(truncatedDesc);
                return false;
            }
        },

        _onAddAlertClick: function (e) {
            var newAlert = $('option:selected', this.$userAlertsList);
            if ($(newAlert).hasClass('default-option')) {
                return false;
            } else {
                if (!this._isAddedToPrincipalList(this.pricipalListArr, $(newAlert)[0].id)) {
                    this.$editListsEmptyMsg.addClass('hide');
                    this.$alertPrincipalList.append('<li class="sortable-item editable-item"><span class="reorder-icon"></span><span class="label" data-id="' + $(newAlert)[0].id + '" data-isPrivate="' + $(newAlert).attr('data-isPrivate') + '">' + $(newAlert).val() + '</span></li>');
                    this.pricipalListArr[this.pricipalListArr.length] = $(newAlert)[0].id;
                    $('option:selected', this.$userAlertsList).attr('disabled', 'disabled');
                    $('option.default-option', this.$userAlertsList).attr('selected', 'selected');
                    if (this.$alertPrincipalList.children().length >= 25) {
                        $(".list-message", this.$editListAddControls).removeClass('hide');
                        $(".add-new-list-item", this.$editListAddControls).addClass('hide');
                    }
                }
            }
            this.alertNSRefreshBinding();
            e.stopPropagation();
            return false;
        },

        _isAddedToPrincipalList: function (arr, obj) {
            return (arr.indexOf(obj) != -1);
        },

        _addAlertToPrincipalList: function (alert) {
            this.$editListsEmptyMsg.addClass('hide');
            if (alert.isActive) {
                this.$alertPrincipalList.append('<li class="sortable-item editable-item"><span class="reorder-icon"></span><span class="label" data-id="' + alert.id + '" data-isPrivate="' + ((alert.publishScope === 2) ? true : false) + '">' + alert.name + '</span></li>');
            }
            else {
                this.$alertPrincipalList.append('<li class="sortable-item editable-item"><span class="reorder-icon"></span><span class="label" data-id="' + alert.id + '" data-isPrivate="' + ((alert.publishScope === 2) ? true : false) + '">' + ((alert.name === null) ? "" : alert.name) + " (<%= Token("noLongerAvailable") %>)</span></li>");
            }
            this.pricipalListArr[this.pricipalListArr.length] = alert.id + "";
            if (this.$alertPrincipalList.children().length >= 25) {
                $(".list-message", this.$editListAddControls).removeClass('hide');
                $(".add-new-list-item", this.$editListAddControls).addClass('hide');
            }
            this.alertNSRefreshBinding();
        },

        _populateAlertsList: function () {
            var moduleId = "";
            var pageId = "";
            if (this.getModule()) {
                moduleId = this.getModule().options.moduleId;
                pageId = this.getCanvas().get_canvasId();
            }
            $(this.element).showLoading();
            $dj.proxy.invoke({
                url: this.options.alertListUrl,
                queryParams: {
                    "pageId": pageId,
                    "moduleId": moduleId
                },
                controlData: this.getCanvas().get_ControlData(),
                preferences: this.getCanvas().get_Preferences(),
                onSuccess: this._delegates.OnServiceCallSuccess,
                onError: this._delegates.OnServiceCallError
            });
        },

        _populateModuleAlerts: function (data) {
            var self = this;
            _.each(data.package.moduleAlerts, function (Alert) {
                self._addAlertToPrincipalList(Alert);
                $("option#" + Alert.id, self.$userAlertsList).attr('disabled', 'disabled');
            }, this);
        },

        _groupAlertsByAssetType: function (data) {
            var alertGroupObj = {};
            var Personal = [];
            var Grouped = [];
            _.each(data, function (alert) {
                switch (alert.isGroupFolder) {
                    case false:
                        var isPrivate = (alert.publishScope === 2) ? true : false;
                        Personal[Personal.length] = { "id": alert.id, "name": alert.name, "isPrivate": isPrivate };
                        break;
                    case true:
                        var isPrivate = (alert.publishScope === 2) ? true : false;
                        Grouped[Grouped.length] = { "id": alert.id, "name": alert.name, "isPrivate": isPrivate };
                        break;
                }

            }, this);
            alertGroupObj = { "Personal": Personal, "Grouped": Grouped };
            return alertGroupObj;
        },

        _bindOverlayButtons: function () {
            var self = this;

            //Bind close button
            var overlayClose = $('p.modalClose', this.alertsOverlay)[0];
            if (overlayClose) {
                $(overlayClose).click(function () { self._hideAlertsOverlay(self) });
            }

            //Bind proceed button
            var overlayProceed = $('a.dj_modal-proceed', this.alertsOverlay)[0];
            if (overlayProceed) {
                $(overlayProceed).click(function () {
                    //set make public flag to true for the selected private alerts
                    var data = self.buildProperties();
                    _.each(data.alerts, function (alertObj) {
                        if (alertObj.isPrivate === "true") {
                            alertObj.makePublic = "true";
                        }
                    }, this);
                    self.createAlerts(self.createAlertsOverlayFn, data);
                    self._hideAlertsOverlay(self);
                });
            }

            //Bind cancel button            
            var overlayCancel = $('a.dj_modal-close', this.alertsOverlay)[0];
            if (overlayCancel) {
                $(overlayCancel).click(function () { self._hideAlertsOverlay(self) });
            }
        },

        _hideAlertsOverlay: function (self) {
            $().overlay.hide("#alertsCreateOverlay");
            return false;
        },

        //Fuction to check for private alerts. If private show the overlay.
        _checkForPrivateAlerts: function (data) {
            var personalAlertsObj = {};
            personalAlertsObj.PrivateAlerts = [];
            _.each(data.alerts, function (alertObj) {
                if (alertObj.isPrivate === "true") {
                    personalAlertsObj.PrivateAlerts[personalAlertsObj.PrivateAlerts.length] = alertObj;
                }
            }, this);

            // personalAlertsObj.callback;
            return personalAlertsObj;
        },

        //Reset Alerts lists
        _resetAlertsLists: function () {
            this.$userAlertsList.empty();
            this.$alertPrincipalList.empty();
            this.$editListsEmptyMsg.removeClass('hide')
            this.$addBtn.unbind('click');
        },

        _onCreateSuccess: function (callback, result) {
            var returnObj = { status: 0, moduleId: result.moduleId };
            callback(returnObj);
        },

        _onCreateError: function (callback, result) {
            callback(result);
        },

        _onSuccess: function (data) {

            if (!data) {
                return;
            };

            if (data.returnCode != 0) {
                return;
            };

            if (data && data.package && data.package.alerts && data.package.alerts.length > 0) {
                var alertsPackage = this._groupAlertsByAssetType(data.package.alerts);

                //Reset the alert list
                this._resetAlertsLists();

                //Bind the alert list to the template
                this.$userAlertsList.append(this.templates.userAlertsList(alertsPackage));
                this._initializeEvents();
                if (this.getModule())
                    this._populateModuleAlerts(data);
            }

            //Hide loading once the alert list is populated
            $(this.element).hideLoading();
        },

        _onError: function (errorThrown, jqXHR, serverMessage) {
            var message = 'undefined error', errorCode;
            if (jqXHR.error && jqXHR.error.code) {
                message = jqXHR.error.message;
                errorCode = jqXHR.error.code;
            }
            else if (jqXHR.Message) {
                message = jqXHR.Message;
                errorCode = 'unknown';
            }

            if (this.getModule())
                this.getModule().showErrorMessage({ returnCode: errorThrown.code, statusMessage: serverMessage });
            //Hide loading once the alert list is populated
            $(this.element).hideLoading()
        },

        EOF: null

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_AlertsModuleEditor', DJ.UI.AlertsModuleEditor);


})(jQuery);