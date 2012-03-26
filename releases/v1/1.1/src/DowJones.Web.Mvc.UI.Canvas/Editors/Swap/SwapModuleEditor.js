/// <reference name="jquery.js" assembly="DowJones.Web.Mvc" />
/// <reference name="common.js" assembly="DowJones.Web.Mvc" />
/// <reference name="ServiceProxy.js" assembly="DowJones.Web.Mvc" />

//! SwapModuleEditor

(function ($) {

    DJ.UI.SwapModuleEditor = DJ.UI.AbstractCanvasModuleEditor.extend({

        selectors: {
            industry: 'select.dj_Lens_Industry',
            region: 'select.dj_Lens_Region'
        },

        swapOptions: {
            All: 0,
            Industry: 1,
            Region: 2
        },

        init: function (element, meta) {

            this._super(element, meta);
            this._selectedModuleId = this.options.moduleId;

        },

        innerTemplates: {
            options: _.template([
                                  '<% _.each(items, function(item) { %>'
                                , '<option value="<%= item.id %>"><%= item.name %></option>'
                                , '<% }); %>'
                                ].join(''))

        },

        _initializeElements: function (ctx) {
            this.$industry = $(this.selectors.industry, ctx);
            this.$region = $(this.selectors.region, ctx);
        },

        onShow: function () {
            this._reset();
            this._selectedModuleId = 0;
            this.getData();
        },

        getData: function () {
            this.$element.showLoading();

            $dj.proxy.invoke({
                url: this.options.webServiceUrl,
                queryParams: this._getQueryParams(),
                controlData: this.getCanvas().get_ControlData(),
                preferences: this.getCanvas().get_Preferences(),
                onSuccess: this._delegates.OnServiceCallSuccess,
                onError: this._delegates.OnServiceCallError,
                onComplete: this._delegates.OnServiceCallComplete
            });
        },


        setData: function (data) {
            if (!data || data.returnCode !== 0 || !data.package) {
                $dj.debug('Error while loading data. Data is either null or return code is not 0. Dump begins here:', data);
                this._onError(data);
                return;
            }

            this._setLens(data.package);

        },

        _reset: function () {
            $(':radio', this.$element).each(function (i, radio) {
                $(radio).attr('checked', false);
            });

            $('option', this.$industry).not('.doNotRemove').remove();
            $('option', this.$region).not('.doNotRemove').remove();
            this.$industry.val(0);
            this.$region.val(0);
        },

        _setLens: function (data) {
            this.$industry.append(this.innerTemplates.options({ items: data.industries }));
            this.$region.append(this.innerTemplates.options({ items: data.regions }));

            this._setCurrentModuleSelected(data);

        },


        _setCurrentModuleSelected: function (data) {

            var containsCurrentModule = false,
                moduleId = this.options.moduleId.toString();


            containsCurrentModule = _.detect(data.industries, function (industry) {
                return industry.id === moduleId;
            });

            if (containsCurrentModule) {
                this.$industry
                    .val(moduleId)
                    .siblings('input[name=lens]:radio').attr('checked', true);

                return;
            }

            containsCurrentModule = _.detect(data.regions, function (region) {
                return region.id === moduleId;
            });

            if (containsCurrentModule) {
                this.$region
                    .val(moduleId)
                    .siblings('input[name=lens]:radio').attr('checked', true);
                return;
            }


            // no match found. set it to default
            this.$industry.val(0);
            this.$region.val(0);
        },


        _initializeEventHandlers: function () {
            this.$industry.change(this._delegates.OnLensChange);
            this.$region.change(this._delegates.OnLensChange);
        },


        _onLensChange: function (e) {
            var $select = $(e.target);

            // check the associated radio button
            $select.siblings('input[name=lens]:radio').attr('checked', true);

            this._selectedModuleId = ($select.val() === 0) ? undefined : $select.val();
        },

        _onSuccess: function (data) {
            this.setData(data);
        },


        _onError: function (error) {
            $dj.debug(error);

            // no requirements on how to display/handle error
        },


        _onComplete: function () {
            this.$element.hideLoading();
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this._onError),
                OnLensChange: $dj.delegate(this, this._onLensChange),
                OnServiceCallComplete: $dj.delegate(this, this._onComplete)
            });

        },


        buildProperties: function () {
            return {
                "moduleIdToRemove": this.options.moduleId,
                "moduleIdToAdd": this._selectedModuleId
            };
        },


        _getQueryParams: function () {
            return {
                moduleType: this.options.moduleType
            };
        },

        getSwapOptions: function () {
            if (this.$element.hasClass('region')) {
                return this.swapOptions.Region;
            }
            else if (this.$element.hasClass('industry')) {
                return this.swapOptions.Industry;
            }
            else {
                return this.swapOptions.All;
            }
        },

        setSwapOptions: function (option) {
            switch (option) {
                case this.swapOptions.Industry:
                    this.$element.removeClass('region').addClass('industry');
                    break;
                case this.swapOptions.Region:
                    this.$element.removeClass('industry').addClass('region');
                    break;
                case this.swapOptions.All:
                default:
                    this.$element.removeClass('industry').removeClass('region');
            }
        }
    });

    $.plugin('dj_SwapModuleEditor', DJ.UI.SwapModuleEditor);

    $dj.debug('Registered DJ.UI.SwapModuleEditor as dj_SwapModuleEditor');


})(jQuery);
