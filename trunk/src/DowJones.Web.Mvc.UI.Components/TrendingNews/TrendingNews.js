/*
 * TrendingNews
 */

(function ($) {

    DJ.UI.TrendingNews = DJ.UI.Component.extend({

        defaults: {
            debug: false,
            cssClass: 'TrendingNews',
            packageType: 'TrendingTopEntitiesPackage'
        },

        selectors: {
            industryitemtitle: ".industry-item-title a"
        },

        eventNames: {
            trendingEntityClick: "trendingEntityClick.dj.trendingnews"
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "TrendingNews" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            this.data ? this.setData(this.data) : this._bindOnNoData();
        },

        _initializeDelegates: function () {
            $.extend(this._delegates, {
                OnTrendingEntityClick: $dj.delegate(this, this._onTrendingEntityClick)
            });
        },

        _initializeEventHandlers: function () {
            this.$element.on("click", this.selectors.industryitemtitle, this._delegates.OnTrendingEntityClick);
        },

        _onTrendingEntityClick: function (event) {
            var $target = $(event.target);
            var currentVolumeCount = "undefined";

            //check the type...before passng.
            if (this.packageType != "TrendingTopEntitiesPackage") {
                var $currentVolume = $("span.news-volume-current", $target.parent().siblings());
                if ($currentVolume && $currentVolume.length > 0) {
                    currentVolumeCount = parseInt($currentVolume.html(), 10);
                }
            }

            var dataforsubscribing = {
                "searchContext": $target.data("searchcontext"),
                "title": $target.html(),
                "target": event.target,
                "modulePart": $target.closest("div").find("h3").text() + ' - ' + $target.html()
            };

            if (currentVolumeCount != undefined ) dataforsubscribing["headlineCount"] = currentVolumeCount; //no check for 0
            $dj.publish("publish " + this.eventNames.trendingEntityClick);
            this.publish(this.eventNames.trendingEntityClick, dataforsubscribing);
        },

        setData: function (data) {
            if (!data || data.length === 0) {
                this._bindOnNoData();
                return;
            }

            this._bindOnSuccess({ trendingEntitiesdata: data });
        },

        _bindOnSuccess: function (data) {
            try {
                this.packageType = this.options.packageType;
                var newstrends;
                switch (this.packageType) {
                    case "trendingDownPackage":
                    case "trendingUpPackage":
                    case "TrendingDownPackage":
                    case "TrendingUpPackage":
                        newstrends = this.templates.trendingnewspercentagesuccess({ data: data.trendingEntitiesdata, options: this.options }); //can multiply options in the future/
                        break;
                    case "trendingTopEntitiesPackage":
                    case "TrendingTopEntitiesPackage":
                    default:
                        newstrends = this.templates.trendingnewstop5success({ data: data.trendingEntitiesdata, options: this.options });
                        break;
                }

                this.$element.html(newstrends);
            }
            catch (exception) {
                //$dj.debug("error while calling the template on binding");
                $dj.error("failed to initialize the component, error on binding: " + exception.message);
            }
        },

        _bindOnNoData: function () {
            this.templates.nodata({ code: "8888", message: "Tokenised no error" }); //TODO
            return;
        },

        dispose: function () {
            this._super();
            this.$element = null;
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_TrendingNews', DJ.UI.TrendingNews);

})(jQuery);