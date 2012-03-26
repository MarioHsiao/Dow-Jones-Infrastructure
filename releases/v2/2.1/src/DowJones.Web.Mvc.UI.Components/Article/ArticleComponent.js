/*!
* ArticleControl
*/

(function ($) {

    DJ.UI.ArticleControl = DJ.UI.Component.extend({

        // Default options
        defaults: {
            debug: false,
            cssClass: 'ArticleControl'
            // ,name: value     // add more defaults here separated by comma
        },

        events: {
            entityClick: 'entityClick.dj.Article',
            sourceClick: 'sourceClick.dj.Article',
            authorClick: 'authorClick.dj.Article',
            anchorClick: 'anchorClick.dj.Article',
            postProcessingClick: 'postprocessing.dj.article',
            eLinkClick: 'eLinkClick.dj.Article',
            headlineLinkClick: 'headlineLinkClick.dj.article',
            smallPictureClick: 'smallPictureClick.dj.article'

        },

        selectors: {
            articleContainer: '.dj_article-container',
            postProcessing: '.dj_PostProcessing li.action',
            entityLinks: '.dj_article_entity',
            anchorLinks: '.dj_article_anchor',
            sourceLinks: '.dj_article_source',
            authorLinks: '.dj_article_author',
            eLinks: '.dj_article_elink',
            headline: 'h4.headline',
            headlineLink: '.dj_article_headline_link',
            smallPictureImg: 'img.smallImage'
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "ArticleComponent" }, meta);

            // Call the base constructor
            this._super(element, $meta);

        },

        _initializeElements: function () {
            this.$articleContainer = this.$element.find(this.selectors.articleContainer);
            $dj.debug("initialize -> $articleContainer:" + this.$articleContainer)
        },

        _initializeDelegates: function () {
            this._super();
        },

        _initializeEventHandlers: function () {
            var self = this;

            this.$element.delegate(this.selectors.entityLinks, 'click', function (e) {
                var data = $(this).data("entity");
                //Need element reference to show the callout pop
                //Adding it to data to keep the component backward compatible 
                data.element = this;
                self.publish(self.events.entityClick, data);
                return false;
            });

            this.$element.delegate(this.selectors.anchorLinks, 'click', function (e) {
                self.publish(self.events.anchorClick, { href: $(this).data("href") });
                return false;
            });

            this.$element.delegate(this.selectors.sourceLinks, 'click', function (e) {
                //self.publish(self.events.sourceClick, $(this).data("entity"));
                self.publish(self.events.sourceClick, { event: e, entityType: 'source', entityCode: $(this).data("entity").fcode });
                return false;
            });

            this.$element.delegate(this.selectors.authorLinks, 'click', function (e) {
                //self.publish(self.events.authorClick, $(this).data("entity"));
                self.publish(self.events.authorClick, { event: e, entityType: 'author', entityCode: $(this).data("entity").fcode });
                return false;
            });

            this.$element.delegate(this.selectors.eLinks, 'click', function (e) {
                self.publish(self.events.eLinkClick, { href: $(this).data("href") });
                return false;
            });

            this.$element.delegate(this.selectors.postProcessing, "click", function (e) {
                self.publish(self.events.postProcessingClick,
                             $.extend({ type: $(this).data("ref"), title: $(self.selectors.headline, self.$element).text() },
                                      self.$articleContainer.data("ref")));
                return false;
            });

            this.$element.delegate(this.selectors.headlineLink, "click", function (e) {
                self.publish(self.events.headlineLinkClick,
                             $.extend({ title: $(this).text() },
                                      self.$articleContainer.data("ref")));
                return false;
            });

            this.$element.delegate(this.selectors.smallPictureImg, "click", function (e) {
                self.publish(self.events.smallPictureClick, { largeImgSrc: $(this).data("ref") });
                return false;
            });
        },

        _onEntityClick: function (data) {
            if (this.postProcessingComp) {
                this.postProcessingComp.showEntityPopup(data);
            }
        },

        getArticleMetaData: function () {
            return self.$articleContainer.data("ref");
        },

        EOF: true
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_Article', DJ.UI.ArticleControl);


})(jQuery);