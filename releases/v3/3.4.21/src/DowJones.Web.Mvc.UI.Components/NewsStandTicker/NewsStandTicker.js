/*!
* NewsStandTicker
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

var popupVisible = false;

    DJ.UI.NewsStandTicker = DJ.UI.Component.extend({

        templates: {
            success: _.template(['<div class="dj_news-stand-ticker scroll">',
            ' <div class="dj_news-stand-ticker-wrap">',
            '<ul class="dj_news-ticker" style="width: 1486px;">',
                                    '<% for (var index = 0, len = data.topNewsVolumeEntities.length; index < len; index++) {',
            'h = data.topNewsVolumeEntities[index]; %>',
                                        '<li class="dj_news-ticker-item">',
                                        '<h3><%= h.descriptor %></h3>',
                                        '<span><%= h.currentTimeFrameNewsVolume.displayText.Data %> <%= tokens.articlesTkn %></span>',
                                        '</li>',
                                         '<% } %>',
                                            '</ul>',
                                            '</div>',
                                            '</div>', ].join('')),
            error: _.template('<span class="dj_error"><%= errorText %></span>'),
            noData: _.template('<span class="dj_noResults"><%= noResultsTkn %></span>')
        },

        // Default options
        defaults: {
            debug: false,
            cssClass: 'NewsStandTicker'
        },

        options: {
            tickerSpeed: 15
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "NewsStandTicker" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            //this.bindOnSuccess();
        },

        bindOnSuccess: function () {
            if (this.data) {
                var data = this.data;
                this.$element.html("");
                if (data && data.topNewsVolumeEntities.length > 0) {
                    // call to bind and append html to ul in one shot
                    this.$element.append(this.templates.success({
                        data: data,
                        tokens: this.tokens
                    }));

                    // bind events and perform other wiring up
                    this._initializeNewsTicker(data.topNewsVolumeEntities);
                }
                else {
                    // bind the template
                    this.$element.append(this.templates.noData(this.tokens));
                    if (!this.options.displayNoResultsToken) {
                        var no_results = $("span.dj_noResults", this.$element).get(0);
                        if (no_results) {
                            $(no_results).hide();
                        }
                    }
                }
            }
        },

        bindOnError: function (data) {
            this.$element.html("");
            this.$element.append(this.templates.error(data));
        },

        getSuccessTemplate: function () {
            return this.templates.success;
        },

        setSuccessTemplate: function (markup) {
            this.templates.success = _.template(markup);
        },

        getNoDataTemplate: function () {
            return this.templates.noData;
        },

        setNoDataTemplate: function (markup) {
            this.templates.noData = _.template(markup);
        },

        getErrorTemplate: function () {
            return this.templates.error;
        },

        setErrorTemplate: function (markup) {
            this.templates.error = _.template(markup);
        },

        setData: function (tickerData) {
            this.data = tickerData;
            this.bindOnSuccess();            
        },
        
        startAutoScroll: function(){
            var module = this;
            module.$el = this.$element;
            $('.dj_news-stand-ticker', module.$el).smoothDivScroll("startAutoScroll");
        },

        stopAutoScroll: function(){
            var module = this;
            module.$el = this.$element;
            $('.dj_news-stand-ticker', module.$el).smoothDivScroll("startAutoScroll");
        },

        _initializeNewsTicker: function (data) {
            var items = $("li.dj_news-ticker-item", this.$element);
            var tickerDirection = (this.options.tickerDirection.toLowerCase()==="left")?"endlessloopright":"endlessloopleft";
            var tickerSpeed = parseFloat(this.options.tickerSpeed);
            var module = this;
            // Access to jQuery and DOM versions of element
            module.$el = this.$element;

            //Attach the jQuery scrolling plugin
            $('.dj_news-stand-ticker', module.$el).smoothDivScroll({
                scrollableArea: ".dj_news-ticker",
                scrollWrapper: ".dj_news-stand-ticker-wrap",
                autoScroll: "always",
                autoScrollDirection: tickerDirection,
                autoScrollStep: 1,
                autoScrollInterval: tickerSpeed
            });

            module.$el.delegate('.dj_news-stand-ticker', 'mouseover', function () {
                $('.dj_news-stand-ticker', module.$el).smoothDivScroll("stopAutoScroll");
            });

            module.$el.delegate('.dj_news-stand-ticker', 'mouseout', function () {
                //if( popupVisible == false ){
                $('.dj_news-stand-ticker', module.$el).smoothDivScroll("startAutoScroll");
                //}
            });

            $.each(data, function (i, headline) {
                module._initializeNewsTickerItem(items, headline, i);
            });
        },

        _initializeNewsTickerItem: function (items, headline, i) {
            var tLi = items.get(i);
            // Set the data to the li
            $(tLi).data("headline", headline);
            this._addNewsTickerItemEvents(tLi);
        },

        _addNewsTickerItemEvents: function (tLi) {
            var $parentContainer = this.$element;
            $(tLi).click(function (e) {
                $parentContainer.triggerHandler("dj.NewsStandTicker.sourceTitleClick", $(this).data("headline"));
                e.stopPropagation();
                return false;
            });
        }
        
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_NewsStandTicker', DJ.UI.NewsStandTicker);
