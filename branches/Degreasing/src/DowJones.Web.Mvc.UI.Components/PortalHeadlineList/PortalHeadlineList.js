/*
*  Portal Headline List Control
*/

    DJ.UI.PortalHeadlineList = DJ.UI.Component.extend({

        selectors: {
            source: 'span.source-clickable',
            author: 'span.article-clickable',
            noResultSpan: 'span.dj_noResults',
            headline: 'a.article-view-trigger',
            headlineEntry: 'li.dj_entry',
            headlineList: '.article-list'
        },
        
        defaults: {
            displayHeadlineTooltip: false,
            displayNoResultsToken: true,
            displaySnippets: 3,
            extension: "",
            layout: 0,
            maxNumHeadlinesToShow: 5,
            truncationType: 0,
            allowPagination: false,
            pageSize: 5,
            pageDirection: 'Vertical',
            pagePrevSelector: '.prev',
            pageNextSelector: '.next'
        },

        events: {
            headlineClick: "headlineClick.dj.PortalHeadlineList",
            sourceClick: "sourceClick.dj.PortalHeadlineList",
            authorClick: "authorClick.dj.PortalHeadlineList"
        },

        
        init: function (element, meta) {
            var $meta = $.extend({ name: "PortalHeadlineList" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // Initialize component if we got data from server
            this._setData(this.data);
        },


        _initializeHeadlineList: function (data) {
            var items = $(this.selectors.headlineEntry, this.$element);
            var me = this;
            _.each(_.first(data, items.length), function (headline, i) {
                var tLi = items.get(i);
                // Set the data to the li
                $(tLi).data("headline", headline);

                // Set the tooltip (snippets)
                // SnippetDisplayType = Hover
                if ((me.options.displaySnippets === 3) && headline.snippets && headline.snippets.length > 0) {
                    me._renderSnippets(headline, tLi);
                }

                else // SnippetDisplayType = HybridHover
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

            if (this.options.allowPagination) {
                this.$element
                    .find('.carouselContainer')
                    .jCarouselLite({
                        btnNext: this.options.pageNextSelector,
                        btnPrev: this.options.pagePrevSelector,
                        vertical: this.options.pageDirection === 'Vertical',
                        visible: this.options.maxNumHeadlinesToShow,
                        scroll: this.options.maxNumHeadlinesToShow,
                        circular: false
                    });

            }

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


        _initializeElements: function () {
        },


        _initializeEventHandlers: function () {
            var $parentContainer = this.$element
                , me = this;

            this.$element.delegate(this.selectors.headline, 'click', function () {
                me.publish(me.events.headlineClick, { headline: $(this).closest('li').data("headline") });
                // prevent browser from handling the click
                return false;
            });

            if (this.options.sourceClickable) {
                this.$element.delegate(this.selectors.source, 'click', function () {
                    me.publish(me.events.sourceClick, { sourceCode: $(this).attr("rel") });
                    return false;
                });
            }

            if (this.options.authorClickable) {
                this.$element.delegate(this.selectors.author, 'click', function () {
                    me.publish(me.events.authorClick, { authorCode: $(this).attr("rel") });
                    return false;
                });
            }

        },

        _setData: function (data) {
            if (data && data.resultSet)
                this.bindOnSuccess(data.resultSet);
            else
                this.bindOnSuccess({});
        },

        bindOnSuccess: function (data) {
            var headlineMarkup;

            try {
                this.$element.html("");
                if (data && data.count && data.count.value > 0) {
                    // call to bind and append html to ul in one shot

                    switch (this.options.layout) {
                        case 0: // normal headline view
                            headlineMarkup = this.templates.successHeadline({ headlines: data.headlines, options: this.options });
                            break;
                        case 1: // author headline view
                            headlineMarkup = this.templates.successAuthor({ headlines: data.headlines, options: this.options });
                            break;
                        case 2: // timeline headline view
                            headlineMarkup = this.templates.successTimeline({ headlines: data.headlines, options: this.options });
                            break;
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
                $dj.error('Error in PortalHeadlineList.bindOnSuccess:', e);
            }

        },


        bindOnError: function (data) {
            try {
                this.$element.html("");
                this.$element.append(this.templates.error(data));
            } catch (e) {
                $dj.error('Error in PortalHeadlineList.bindOnError:', e);
            }
        },


        getErrorTemplate: function () {
            return this.templates.error;
        },


        getNoDataTemplate: function () {
            return this.templates.noData;
        },


        showEditSection: function (show) {
            show = show || true;

            this.$element.html("");

            if (show) {
                this.$element.append(this.templates.addNewContent());
            }
        },


        setErrorTemplate: function (markup) {
            this.templates.error = _.template(markup);
        },


        setNoDataTemplate: function (markup) {
            this.templates.noData = _.template(markup);
        },


        EOF: null

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_PortalHeadlineList', DJ.UI.PortalHeadlineList);
    $dj.debug('Registered DJ.UI.PortalHeadlineList (extends DJ.UI.Component)');