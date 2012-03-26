/*
*  Portal Headline List Control
*/

(function ($) {

    DJ.UI.PortalHeadlineList = DJ.UI.Component.extend({

        selectors: {
            source: 'a.article-source',
            noResultSpan: 'span.dj_noResults',
            headline: 'a.article-view-trigger',
            headlineEntry: 'li.dj_entry'
        },

        options: {
            maxNumHeadlinesToShow: 5,
            displayHeadlineTooltip: false,
            truncationType: 0,
            extension: ""
        },

        events: {
            // jQuery events are namespaced as <event>.<namespace>
            headlineClick: "headlineClick.dj.PortalHeadlineList",
            sourceClick: "sourceClick.dj.PortalHeadlineList"
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "PortalHeadlineList" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // call databind if we got data from server
            if (this.data && this.data.resultSet)
                this.bindOnSuccess(this.data.resultSet);
        },


        _initializeHeadlineList: function (data) {
            var items = $(this.selectors.headlineEntry, this.$element);
            var me = this;
            _.each(data, function (headline, i) {
                var tLi = items.get(i);
                // Set the data to the li
                $(tLi).data("headline", headline);

                //Set the tooltip (snippets)
                //displaySnippetType = Hover
                if ((me.options.displaySnippets === 3) && headline.snippets && headline.snippets.length > 0) {
                    me._renderSnippets(headline, tLi);
                }

                else //displaySnippetType = Hybrid- Hover
                    if ((me.options.displaySnippets === 4) && headline.snippets && headline.snippets.length > 0) {
                        if (i != 0) {
                            me._renderSnippets(headline, tLi);
                        }
                        else {
                            var snippetStr = "";
                            _.each(headline.snippets, function (snippet, s) {
                                snippetStr += snippet;
                            });
                            var inlineSnippetHtml = '<p class="article-snip">' + snippetStr + '</p>';
                            $('div.article-wrap', tLi).append(inlineSnippetHtml);
                        }
                    }
            }, this);

        },

        //Render snippets
        _renderSnippets: function (headline, tLi) {
            var snippetStr = "";
            _.each(headline.snippets, function (snippet, s) {
                snippetStr += "<div>" + snippet + "</div>";
            });
            $(this.selectors.headline, tLi).attr("title", snippetStr);
            $(this.selectors.headline, tLi).dj_simpleTooltip("tooltip");
        },


        // gets called during base.init()
        _initializeEventHandlers: function () {
            this._super();
            var $parentContainer = this.$element
                , me = this;

            this.$element.delegate(this.selectors.headline, 'click', function () {
                $parentContainer.triggerHandler(me.events.headlineClick,
                                                    { headline: $(this).closest('li').data("headline") });

                // prevent browser from handling the click
                return false;
            });



            if (this.options.sourceClickable) {
                this.$element.delegate(this.selectors.source, 'click', function () {
                    $parentContainer.triggerHandler(me.events.sourceClick, { sourceCode: $(this).attr("rel") });
                    return false;
                });
            }

        },

        bindOnSuccess: function (data) {
            var headlineMarkup;

            try {
                this.$element.html("");
                if (data && data.count && data.count.value > 0) {
                    // call to bind and append html to ul in one shot
                    if (this.options.useTimeLineLayout) {
                        headlineMarkup = this.templates.timeline({ headlines: data.headlines, options: this.options });
                    }
                    else {
                        headlineMarkup = this.templates.success({ headlines: data.headlines, options: this.options });
                    }


                    this.$element.append(headlineMarkup);

                    // bind events and perform other wiring up
                    this._initializeHeadlineList(data.headlines);
                }
                else {
                    // bind the template
                    this.$element.append(this.templates.noData());

                    if (!this.options.displayNoResultsToken) {
                        var no_results = $(this.selectors.noResultSpan, this.$element).get(0);
                        if (no_results) {
                            $(no_results).hide();
                        }
                    }
                }
            } catch (e) {
                $dj.debug('Error in PortalHeadlineList.bindOnSuccess');
                $dj.debug(e);
            }

        },


        bindOnError: function (data) {
            try {
                this.$element.html("");
                this.$element.append(this.templates.error(data));
            } catch (e) {
                $dj.debug('Error in PortalHeadlineList.bindOnError');
                $dj.debug(e);
            }
        },


        showEditSection: function (show) {
            show = show || true;

            this.$element.html("");

            if (show) {
                this.$element.append(this.templates.addNewContent());
            }
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


        EOF: null

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_PortalHeadlineList', DJ.UI.PortalHeadlineList);


    $dj.debug('Registered DJ.UI.PortalHeadlineList (extends DJ.UI.Component)');

})(jQuery);
