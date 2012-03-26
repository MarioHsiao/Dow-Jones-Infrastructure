/// <reference name="jquery.js" assembly="DowJones.Web.Mvc" />

/*!
 * RadarCanvasModule
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

    DJ.UI.RadarCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        selectors: {
            radarContainer: ".dj_Radar"
        },

        events: {
            radarNodeClick: 'radarNode.dj.RadarCanvasModule'
        },

        // add client side properties here

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "RadarCanvasModule" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            var me = this;

            me.scopeSelectors = $(".scope .dj_btn", me.$element);

            /*initialize timePeriod*/
            me.scopeSelectors
                .addClass("no-bg")
                .each(function () {
                    var scopeItem = this,
                        scopeItemID = $(scopeItem).attr('id');

                    scopeItemID = scopeItemID.slice(-1); //extract the number out of the ID ('radarTP-#' - 'radarTP-')
                    if (scopeItemID == me.options.timePeriod) {
                        $(scopeItem).removeClass("no-bg");
                    }
                })
                .live('click', function () {
                    var scopeItemID = $(this).attr('id');

                    if ($(this).hasClass('no-bg')) {
                        scopeItemID = scopeItemID.slice(-1); //extract the number out of the ID ('radarTP-#' - 'radarTP-')
                        me._onTimePeriod(this, scopeItemID);
                    }

                    return false;
                });

            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this.showErrorMessage),
                OnRadarNodeClick: $dj.delegate(this, this._onRadarNodeClick)
            });

            this.radarContainer = $(this.selectors.radarContainer, this.$element);
        },

        /*
        * Public methods
        */

        getData: function () {
            this._super();
            $dj.proxy.invoke({
                url: this.options.dataServiceUrl,
                queryParams: {
                    "pageid": this._canvas.get_canvasId(),
                    "moduleid": this.get_moduleId(),
                    "timeframe": this._getTimeFrame(this.options.timePeriod)
                },
                controlData: this._canvas.get_ControlData(),
                preferences: this._canvas.get_Preferences(),
                onSuccess: this._delegates.OnServiceCallSuccess,
                onError: this._delegates.OnServiceCallError
            });
        },

        fireOnSaveAndCloseEditArea: function (e) {
            var editorProps = this._editor.buildProperties();
            this._publish('swap', editorProps);
        },

        /*
        * Private methods
        */

        _onTimePeriod: function (e, id) {
            this.options.timePeriod = id;
            $(".radar-TimePeriod .dj_btn", this.$element).addClass("no-bg");
            $(e).removeClass("no-bg");
            this.getData();
        },

        _getTimeFrame: function (timePeriod) {
            var timeFrame;
            switch (timePeriod) {
                case "0":
                    timeFrame = 'lastweek';
                    break;
                case "1":
                    timeFrame = 'lastmonth';
                    break;
                case "2":
                    timeFrame = 'threemonths';
                    break;
                default:
                    timeFrame = 'lastweek';
                    break;
            }
            return timeFrame

        },

        SetRadarData: function (radarData) {
            $(".dj_Radar", this.$element).findComponent('dj_Radar').setData(radarData);
        },

        _onSuccess: function (data) {
            var me = this;
            if (!data) {
                // todo: display error
                this.showErrorMessage({ returnCode: 'unknown', statusMessage: 'data is undefined or null' });
                return;
            };

            if (data.returnCode != 0) {
                this.showErrorMessage(data);
                return;
            };

            if (data && data.partResults && data.partResults.length > 0) {
                var radar = this.radarContainer.findComponent(DJ.UI.Radar);
                for (var i = 0; i < data.partResults.length; i++) {
                    var packageType = data.partResults[i].package && data.partResults[i].packageType;
                    if (packageType && (packageType.toLowerCase() === "radarpackage")) {
                        if (data && data.partResults[i] && data.partResults[i].package.parentNewsEntities.length > 0) {
                            var newsEntities = data.partResults[i].package;
                            setTimeout(function () { me.SetRadarData(newsEntities) }, 100);
                        }
                        else {
                            //Show the error messaage
                        }
                    }

                }
                $(radar.element).unbind(radar.events.nodeClick).bind(radar.events.nodeClick, this._delegates.OnRadarNodeClick);
            }
            this.showContentArea();
        },

        _onRadarNodeClick: function (sender, data) {
            this._publish(this.events.radarNodeClick, { "searchContext": data.newsEntitiesSearchContextRef,
                "title": data.title, "target": data.target, "modulePart": data.title, "headlineCount": data.headlineCount
            });
            $dj.debug('Published ' + this.events.radarNodeClick);
        },

        clearTrailNodes: function () {
            try {
                this.radarContainer.findComponent(DJ.UI.Radar).clearTrailNodes();
            } catch (e) { }
        },

        _onError: function (jqXHR, textStatus, errorThrown) {
            this.showContentArea();
        },

        EOF: null

    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_RadarCanvasModule', DJ.UI.RadarCanvasModule);

    $dj.debug('Registered DJ.UI.RadarCanvasModule as dj_RadarCanvasModule');

})(jQuery);