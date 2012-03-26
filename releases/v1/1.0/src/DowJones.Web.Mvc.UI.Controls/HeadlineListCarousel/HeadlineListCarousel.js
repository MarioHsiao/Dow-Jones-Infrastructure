/*!
* Headline List Carousel Control
*/

(function ($) {

    DJ.UI.HeadlineListCarousel = DJ.UI.Component.extend({

        /*
        * Properties
        */
        defaults: {
            numberOfHeadlinesToScrollBy: 3
            , displaySnippets: "none"
            , displayTime: true
            , extension: ""
            , displayMode: "vertical" // horizontal
        },

        tokens: {
            noResultsTkn: '${noResults}'
        },


        /*
        * Initialization
        */
        init: function (element, meta) {
            this._super(element, meta);

            this.data = new DJ.UI.HeadlineListModel(this.data, this.options);

            this._carousel = $('ul.dj_carousel', this.element).get(0);

            $(this._carousel).jcarousel({
                scroll: this.options.visibleItems || 3,
                vertical: (this.options.orientation !== "horizontal"),
                auto: this.options.autoScrollSpeed
            });


            // Create the No Results container
            this._noResultsMessageContainer =
                $("<div class='dj_noResults' />")
                    .html(this.tokens.noResultsTkn)
                    .hide()
                    .prependTo(this.element)
                    .get(0);

            // set property values
            this.setDisplaySnippets(this.options.displaySnippets);
            this.setDisplayTime(this.options.displayTime);
            this.setNumberOfHeadlinesToScrollBy(this.options.numberOfHeadlinesToScrollBy);

            this.paint();
        },


        /*
        * Private methods
        */


        _onHeadlineImageClick: function (event) {
            event.type = 'dj_headlineListCarousel.HeadlineImageClick';
            $(event.data.hdlc.element).trigger(event, { headline: event.data.headline });

            event.stopPropagation();
            return false;
        },

        _onHeadlineImageHoverOver: function (event) {
            event.type = 'dj_headlineListCarousel.HeadlineImageHoverOver';
            
            $(event.data.hdlc)
                .addClass("image-hover")
                .trigger(event, { headline: event.data.headline });

            event.stopPropagation();
            return false;
        },

        _onHeadlineImageHoverOut: function (event) {
            event.type = 'dj_headlineListCarousel.HeadlineImageHoverOut';
            $(event.data.hdlc)
                .removeClass("image-hover")
                .trigger(event, { headline: event.data.headline });

            event.stopPropagation();
            return false;
        },

        _onHeadlineSnippetHoverOver: function (event) {
            $(event.data.hdlc).addClass("snippet-hover");
        },

        _onHeadlineSnippetHoverOut: function (event) {
            $(event.data.hdlc).removeClass("snippet-hover");
        },

        _onHeadlineTimeHoverOver: function (event) {
            $(event.data.hdlc).addClass("time-hover");
        },

        _onHeadlineTimeHoverOut: function (event) {
            $(event.data.hdlc).removeClass("time-hover");
        },

        _onExtensionItemClick: function (event) {
            event.type = 'dj_headlineListCarousel.ExtensionItemClick';
            $(event.data.hdlc.element).trigger(event, { headline: event.data.headline });
        },

        _onHeadlineClick: function (event) {
            event.type = 'dj_headlineListCarousel.HeadlineClick';
            $(event.data.hdlc.element).trigger(event, { headline: event.data.headline });

            event.stopPropagation();
            return false;
        },

        _onExtensionItemHoverOver: function (event) {
            $(event.data.hdlc).addClass("extension-hover");
        },

        _onExtensionItemHoverOut: function (event) {
            $(event.data.hdlc).removeClass("extension-hover");
        },

        _onHeadlineHoverOver: function (event) {
            event.type = 'dj_headlineListCarousel.HeadlineHoverOver';
            $(event.data.hdlc)
                .addClass("link-hover")
                .trigger(event, { headline: event.data.headline });

            event.stopPropagation();
            return false;
        },

        _onHeadlineHoverOut: function (event) {
            event.type = 'dj_headlineListCarousel.HeadlineHoverOut';
            $(event.data.hdlc)
                .removeClass("link-hover")
                .trigger(event, { headline: event.data.headline });

            event.stopPropagation();
            return false;
        },

        /*
        * Public methods
        */
        setData: function (value) {
            this._clear();

            this.data = new DJ.UI.HeadlineListModel(value, this.options);

            this.paint();
        },

        appendData: function (data) {
            var model = new DJ.UI.HeadlineListModel(data, this.options);

            if (!model || !model.HasHeadlines)
                return;

            // get the headlines to be appended
            var newData = this.data;

            for (i = 0; i < model.Headlines.length; i++) {
                newData[newData.length] = model.Headlines[i];
            }


        },

        getDisplayTime: function (value) {
            return this.options.displayTime;
        },

        setDisplayTime: function (value) {
            this.options.displayTime = value;

            if (value === true) {
                $("div.headline-time-container-vertical", this.element).show();
            }
            else {
                $("div.headline-time-container-vertical", this.element).hide();
            }
        },

        getExtension: function (value) {
            return this.options.displayTime;
        },

        setExtension: function (value) {
            this.options.extension = value;

            $("span", $("div.headline-extension-container-vertical", this.element)).empty();
            $("span", $("div.headline-extension-container-vertical", this.element)).append(value);

            if (this._getDisplayExtension()) {
                $("div.headline-extension-container-vertical", this.element).show();
            }
            else {
                $("div.headline-extension-container-vertical", this.element).hide();
            }
        },

        getNumberOfHeadlinesToScrollBy: function (value) {
            return this.options.numberOfHeadlinesToScrollBy;
        },

        setNumberOfHeadlinesToScrollBy: function (value) {
            this.options.numberOfHeadlinesToScrollBy = value;
            $.jcarousel(this._carousel).options.scroll = value;
        },

        getDisplaySnippets: function (value) {
            return this.options.displaySnippets;
        },

        setDisplaySnippets: function (value) {
            this.options.displaySnippets = value;

            var hoverSnippetContainer = $('div.headline-snippet-container-vertical-hover');

            if (this.options.displaySnippets === "hover") {
                var $anchor = $('a.link', $(this));
                $anchor.dj_simpleTooltip(hoverSnippetContainer);
            }
            else {
                $(hoverSnippetContainer).hide();
            }
        },

        addHeadline: function (headline, carousel, size) {
            carousel = carousel || $.jcarousel(this._carousel);
            size = size || carousel.size();

            var newItem = $(this.templats.itemTemplate(headline));
            
            newItem.data("headline", headline);

            // title
            $('a.link', newItem)
                .bind("click", { "hdlc": this, "headline": headline }, this._onHeadlineClick)
                .bind("mouseover", { "hdlc": this, "headline": headline }, this._onHeadlineHoverOver)
                .bind("mouseout", { "hdlc": this, "headline": headline }, this._onHeadlineHoverOut);

            // thumbnail
            $('img.image', newItem)
                .bind("click", { "hdlc": this, "headline": headline }, this._onHeadlineImageClick)
                .bind("mouseover", { "hdlc": this, "headline": headline }, this._onHeadlineImageHoverOver)
                .bind("mouseout", { "hdlc": this, "headline": headline }, this._onHeadlineImageHoverOut);

            // snippets
            $('p.snippet', newItem)
                .bind("mouseover", { "hdlc": this, "headline": headline }, this._onHeadlineSnippetHoverOver)
                .bind("mouseout", { "hdlc": this, "headline": headline }, this._onHeadlineSnippetHoverOut);

            // time
            $('span.time', newItem)
                .bind("mouseover", { "hdlc": this, "headline": headline }, this._onHeadlineTimeHoverOver)
                .bind("mouseout", { "hdlc": this, "headline": headline }, this._onHeadlineTimeHoverOut);

            // extensions
            $('span.extension', newItem)
                .bind("click", { "hdlc": this, "headline": headline }, this._onExtensionItemClick)
                .bind("mouseover", { "hdlc": this, "headline": headline }, this._onExtensionItemHoverOver)
                .bind("mouseout", { "hdlc": this, "headline": headline }, this._onExtensionItemHoverOut);

            carousel.add(size + 1, newItem);
            carousel.size(size + 1);
        },

        addHeadlines: function (headlines) {
            var carousel = $.jcarousel(this._carousel);
            var size = carousel.size();
            
            var me = this;
            $.each(headlines, function (i, headline) {
                me.addHeadline(headline, carousel, size + i);
            });
        },

        _clear: function () {
            var carousel = $.jcarousel(this._carousel);

            var size = carousel.size();

            carousel.scroll(1);

            for (i = 1; i <= size; i++)
                carousel.remove(i);

            carousel.size(0);
        },

        _paint: function () {
            var model = this.data;
            var $carousel = $(this._carousel);

            $carousel.hide();
            $(this._noResultsMessageContainer).toggle(!model.HasHeadlines);

            if (!model.HasHeadlines)
                return;

            this.addHeadlines(model.Headlines)

            $carousel.show();
        },

        EOF: null
    });

    DJ.UI.HeadlineListModel = function (data, options) {
        this.Headlines = [];
        this.HasHeadlines = (data && data.resultSet && data.resultSet && data.resultSet.headlines && data.resultSet.count && data.resultSet.count.isPositive);

        if (!this.HasHeadlines)
            return this;

        var headlinesSource = data.resultSet.headlines;
        for (var index = 0; index < headlinesSource.length; index++) {

            var source = headlinesSource[index];

            var headline = {};

            headline.DisplayExtensions = options.extension
                                          && options.extension.length
                                          && options.extension.length > 0;

            headline.DisplaySnippets = options.displaySnippets;

            headline.DisplayTime = options.displayTime;

            headline.Extension = options.extension;

            headline.ExternalUri = (source.reference && source.reference.externalUri) ? source.reference.externalUri : "javascript:void(0)";

            headline.ThumbnailHref = (source.thumbnailImage && source.thumbnailImage.uri) ? source.thumbnailImage.uri : "javascript:void(0)";

            headline.ThumbnailSourceUri = (source.thumbnailImage && source.thumbnailImage.src) ? source.thumbnailImage.src : "";

            headline.Time = source.time;

            headline.Snippets = [];
            if (source.snippet) {
                for (var i = 0; i < source.snippet.length; i++) {
                    if (source.snippet[i].items) {

                        var snippetItems = [];

                        for (var j = 0; j < source.snippet[i].items.length; j++) {
                            snippetItems[snippetItems.length] =
                                    source.snippet[i].items[j].value;
                        }

                        headline.Snippets[headline.Snippets.length] = snippetItems;
                    }
                }
            }

            headline.TitleItems = [];
            if (source.title) {
                for (var i = 0; i < source.title.length; i++) {
                    if (source.title[i].items) {
                        for (var j = 0; j < source.title[i].items.length; j++) {
                            headline.TitleItems[headline.TitleItems.length] =
                                    source.title[i].items[j].value;
                        }
                    }
                }
            }


            this.Headlines[this.Headlines.length] = headline;
        }

        return this;
    };



    $.plugin('dj_headlineListCarousel', DJ.UI.HeadlineListCarousel);

})(jQuery);