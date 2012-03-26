/*!
 * RegionalMapCanvasModule
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

    DJ.UI.RegionalMapCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        /*
        * Initialization (constructor)
        */
        defaults: {
            tf: 'lastweek'
        },

        events: {
            regionalMapRegionClick: "regionClick.dj.RegionalMapCanvasModule"
        },

        selectors: {
            regionalMap: "div.dj_RegionalMap",
            regionalMapContainer: "div.dj_RegionalMap-container",
            timePeriodBtnsContainer: "div.regionalmap-TimePeriod",
            timePeriodBtns: "span.dj_btn",
            noResults: "div.no-results"
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "RegionalMapCanvasModule" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this._setDefaultProperties();
        },

        _setDefaultProperties: function () {
            var me = this;
            this.refresh = false;
            this.scopeSelectors.find("span").addClass("no-bg");
            this.options.regtimeframe = this.scopeSelectors.find("span:first").removeClass("no-bg").attr("days");
        },

        _initializeElements: function (ctx) {
            var me = this;
            this._super();
            this.scopeSelectors = $(this.selectors.timePeriodBtnsContainer, ctx);
            this.options.regtimeframe = this.scopeSelectors.find("span:first").attr("days");

            /*initialize timePeriod*/
            this.scopeSelectors.delegate(this.selectors.timePeriodBtns, 'click', function () {
                if ($(this).hasClass('no-bg')) {
                    me._onTimePeriod(this, $(this).attr("days"));
                }
                return false;
            });
        },

        _initializeDelegates: function () {
            this._super();
            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this._onError),
                OnTimeFrameCall: $dj.delegate(this, this._onTimeFrame)
            });
        },

        _onTimePeriod: function (e, days) {
            this.options.regtimeframe = days;
            $(this.selectors.timePeriodBtnsContainer + " " + this.selectors.timePeriodBtns, this.$element).addClass("no-bg");
            $(e).removeClass("no-bg");
            this.refresh = true;
            this.getData();
        },


        fireOnSaveAndCloseEditArea: function (e) {
            var editorProps = this._editor.buildProperties();
            this._setDefaultProperties();
            this._publish('swap', editorProps);
        },

        getData: function () {

            if (!this.refresh) {
                this._super();
            }
            else {
                $(this.selectors.regionalMapContainer, this.$element).showLoading();
            }

            $dj.proxy.invoke({
                url: this.options.dataServiceUrl,
                queryParams: {
                    "pageid": this._canvas.get_canvasId(),
                    "moduleid": this.get_moduleId(),
                    "timeframe": this.options.regtimeframe
                },
                controlData: this._canvas.get_ControlData(),
                preferences: this._canvas.get_Preferences(),
                onSuccess: this._delegates.OnServiceCallSuccess,
                onError: this._delegates.OnServiceCallError
            });
        },

        _onSuccess: function (data) {

            var errors = $dj.getError(data);
            if (errors) {
                this.showErrorMessage(errors);
            }
            else {


                if (!this.refresh || this.hasMessages) {
                    this.showContentArea();
                }
                else {
                    $(this.selectors.regionalMapContainer, this.$element).hideLoading();
                }

                if (data && data.partResults && data.partResults.length > 0) {
                    if (data.partResults[0].package.regionNewsVolume) {
                        $(this.selectors.regionalMapContainer, this.$element).show();
                        $(this.selectors.noResults, this.$element).hide();
                        var rmCtrl = $(this.selectors.regionalMap, this.$element).findComponent(DJ.UI.RegionalMap);
                        rmCtrl.setData(data.partResults[0].package);
                        $(rmCtrl.element).unbind(rmCtrl.events.regionClick).bind(rmCtrl.events.regionClick, $dj.delegate(this, this._regionClick));
                    }
                    else {
                        $(this.selectors.regionalMapContainer, this.$element).hide();
                        $(this.selectors.noResults, this.$element).show();
                    }
                }
                else {
                    errors = $dj.getError(data);
                    this.showErrorMessage(errors);
                }

            }
        },

        _onError: function (response) {
            if (this.refresh && !this.hasMessages) {
                $(this.selectors.regionalMapContainer, this.$element).hideLoading();
            }

            if (response)
                this.showErrorMessage($dj.getError(response));
            else
                this.showErrorMessage($dj.getError(response));

        },

        _regionClick: function (sender, data) {
            this._publish(this.events.regionalMapRegionClick, { "searchContext": data.searchContext,
                "title": data.title, "target": data.element, "regionCode": data.regionCode,
                "modulePart": data.title,
                "positionX": data.positionX,
                "positionY": data.positionY,
                "offset": data.offset
            });
            $dj.debug('Published ' + this.events.regionalMapRegionClick);
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_RegionalMapCanvasModule', DJ.UI.RegionalMapCanvasModule);

    $dj.debug('Registered DJ.UI.RegionalMapCanvasModule as dj_RegionalMapCanvasModule');

})(jQuery);
