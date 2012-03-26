/*!
 * CompanyOverviewModuleEditor
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

    DJ.UI.CompanyOverviewModuleEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

        selectors: {
            moduleName: 'input#txtModuleName',
            description: 'textarea#txtDescription',
            search: 'input#txtCompanyEditSearch',
            company: 'div.txtCompany',
            addCompanyWrap: 'div.add-company-wrap',
            companyList: 'ul#dj_company_list',
            companyLi: 'li.editable-item',
            controls: 'div.controls'
        },

        // Default options
        defaults: {
            debug: false,
            cssClass: 'CompanyOverviewModuleEditor'
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "CompanyOverviewModuleEditor" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            $.extend(this._delegates, {
                OnItemSelect: $dj.delegate(this, this._onItemSelect),
                ToggleCompanyInputs: $dj.delegate(this, this._onToggle),
                OnKeyPressDescription: $dj.delegate(this, this._onKeyPressDescription)
            });

            this.autoSuggestInitialized = false;
            this.controlsAdded = false;
            this.fCode = null;
            this.txtCompanyEditSearch = $('input#txtCompanyEditSearch', this.$element).get(0);
            this.txtCompany = $('div.txtCompany', this.$element).get(0);
            this.addCompanyWrap = $(this.selectors.addCompanyWrap, this.$element);
            this.companyUl = $(this.selectors.companyList, this.$element);
            this.companyLi = $(this.selectors.companyLi, this.companyUl);
            this.controls = $(this.selectors.controls, this.companyUl);

            //Assign Dynamic Id to Company Search Textbox (txtCompanyEditSearch-moduleId)
            this.txtCompanyEditSearch.id = this.txtCompanyEditSearch.id + '-' + this.options.moduleId;


        },

        _initializeEvents: function () {
            this.$description.bind('keypress', this._delegates.OnKeyPressDescription);
        },

        _onKeyPressDescription: function (e) {
            if (this.$description.val().length > 250) {
                var truncatedDesc = this.$description.val().substring(0, 250);
                this.$description.val(truncatedDesc);
                return false;
            }
        },

        onShow: function () {
            this._initializeFormFields();
            this._initializeAutoSuggest();
            this._initializeEvents();
        },

        _initializeElements: function (ctx) {
            this._super();
            this.$moduleName = $(this.selectors.moduleName, ctx);
            this.$description = $(this.selectors.description, ctx);
            this.$search = $(this.selectors.search, ctx);
            this.$company = $(this.selectors.company, ctx);
        },

        _initializeFormFields: function () {
            //Get Company Object
            this.$moduleName.val(this.options.moduleName);
            this.$description.val(this.options.moduleDescription);
            if (this.getModule()) {
                var compObj = this.getModule()._companyObj;
                var x = { "value": compObj.companyName, "code": compObj.fcode }
                this._onInitializeCompany(x);
                this.isFirstTimeEdit = false;
            }
        },

        buildProperties: function () {
            var fCodesArr = [];
            if (this.fCode) { fCodesArr[fCodesArr.length] = this.fCode; }

            return {
                "title": this.$moduleName.val(),
                "description": this.$description.val(),
                "fcodes": fCodesArr,
                "moduleId": this.options.moduleId,
                "pageId": this._canvas.get_canvasId()
            };
        },

        validateBuildProperties: function (paramsObj) {
            errObj = {};
            if (paramsObj.fcodes.length < 1) { errObj.CompanyCount = 0; }
            if (paramsObj.title.length < 1) { errObj.TitleLength = 0; }
            if (errObj.CompanyCount < 1 || errObj.TitleLength === 0) { return errObj; }
        },

        saveProperties: function (callback) {
            var validationObj = this.validateBuildProperties(this.buildProperties());
            if (!validationObj) {
                var requestParms = this.buildProperties();
                this._reset();
                $dj.proxy.invoke(
                {
                    url: this.options.createUpdateCompanyOverviewModuleUrl,
                    data: requestParms,
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

        _onCreateSuccess: function (callback, result) {
            var returnObj = { status: 0, moduleId: result.moduleId };
            callback(returnObj);
        },

        _onCreateError: function (callback, result) {
            //var returnObj = { status: -1, msg: result.error };
            callback(result);
        },


        _initializeAutoSuggest: function () {
            if (!this.autoSuggestInitialized) {
                if (this.getSessionId()) {
                    var companySuggest = {
                        url: this.options.suggestServiceUrl,
                        controlId: this.txtCompanyEditSearch.id,
                        controlClassName: "djAutoCompleteSource",
                        autocompletionType: "Company",
                        selectFirst: false,
                        options: { maxResults: '10', dataSet: 'newsCodedAbt', filterADR: false },
                        useSessionId: this.getSessionId(),
                        onItemSelect: this._delegates.OnItemSelect
                    }

                    //Create an instance of AutoSuggest
                    if (np && np.web && np.web.widgets && np.web.widgets.autocomplete) {
                        np.web.widgets.autocomplete(companySuggest);
                        this.autoSuggestInitialized = true;
                    }
                }
            }
        },

        _reset: function () {
            this.$moduleName.val('');
            this.$description.val('');
            this.$search.val('');
            $(this.addCompanyWrap).show();
            $(this.txtCompanyEditSearch).show();
            $(this.companyUl).html('');
        },

        _onItemSelect: function (data) {
            var self = this;
            if (data) {
                $(this.companyUl).html('');
                $(this.companyUl).append('<li class=\"editable-item sortable-item\"><span class=\"label\">' + data.value + '</span></li>');

                //Hook the events
                $(this.selectors.companyLi, this.companyUl).hover(function (e) { $(this).toggleClass('hover'); })
                                                           .click(function (e) { if (self.controlsAdded) { return; } self._appendControls(); });

                //Set the fCode
                this.fCode = data.code;

                //Hide the suggest textbox

                //Hide the suggest textbox
                setTimeout(function () {
                    $(self.txtCompanyEditSearch).hide();
                    $(self.addCompanyWrap).toggle();
                    $(self.addCompanyWrap).hide();
                    $(self.txtCompany).show();
                }, 5);
            }
        },

        _onInitializeCompany: function (data) {
            var self = this;
            if (data) {
                $(this.companyUl).html('');
                $(this.companyUl).append('<li class=\"editable-item sortable-item\"><span class=\"label\">' + data.value + '</span></li>');

                //Hook the events
                $(this.selectors.companyLi, this.companyUl).hover(function (e) { $(this).toggleClass('hover'); })
                                                           .click(function (e) { if (self.controlsAdded) { return; } self._appendControls(); });

                //Set the fCode
                this.fCode = data.code;

                //Hide the suggest textbox

                $(this.txtCompanyEditSearch).hide();
                $(this.addCompanyWrap).toggle();
                $(this.addCompanyWrap).hide();
                $(this.txtCompany).show();

            }
        },

        _onToggle: function () {
            if ($(this.addCompanyWrap).is(":visible")) {
                $(this.addCompanyWrap).hide();
                $(this.txtCompany).show();
            }
            else {
                $(this.addCompanyWrap).show();
                $(this.txtCompany).hide();
            }
        },

        _onCompanyRemove: function (event) {
            $(event.data.selectors.companyLi, event.data.companyUl).remove();
            $(event.data.selectors.controls, event.data.companyUl).remove();
            event.data.fCode = null;
            event.data.$search.val('');
            $(event.data.addCompanyWrap).show();
            $(event.data.txtCompanyEditSearch).show();
            $(event.data.txtCompanyEditSearch).focus();
            event.data.controlsAdded = false;
            event.stopPropagation();
        },

        _onCompanyCancel: function (event) {
            $(event.data.selectors.controls, event.data.companyUl).remove();
            event.data.controlsAdded = false;
            event.stopPropagation();
        },

        _appendControls: function () {
            var self = this;
            var controlsHTML = "<div class=\"controls\"><ul class=\"dc_list\"><li class=\"dc_item\"><a class=\"dashboard-control dc_btn dc_btn-2 dc_btn-remove\"><%= Token("moduleMenuRemove") %></a></li><li class=\"dc_item\"><a class=\"dashboard-control dc_btn dc_btn-3 dc_btn-cancel\"><%= Token("cancel") %></a></li></ul></div>";
            $(self.selectors.companyLi, self.companyUl).append(controlsHTML);
            self.controlsAdded = true;

            //Attach EventHandlers
            $('a.dc_btn-remove', $(self.selectors.controls, self.companyUl)).bind("click", this, (self._onCompanyRemove));
            $('a.dc_btn-cancel', $(self.selectors.controls, self.companyUl)).bind("click", this, (self._onCompanyCancel));
        },

        // DEMO: Overriding the base _paint method:
        _paint: function () {

            // "this._super()" is available in all overridden methods
            // and refers to the base method.
            this._super();

        }
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_CompanyOverviewModuleEditor', DJ.UI.CompanyOverviewModuleEditor);


})(jQuery);