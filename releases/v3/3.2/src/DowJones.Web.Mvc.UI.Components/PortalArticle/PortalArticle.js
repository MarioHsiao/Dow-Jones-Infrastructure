/*!
* Portal Article Component
*/

DJ.UI.PortalArticle = DJ.UI.Component.extend({

    // Default options
    defaults: {
        debug: false,
        instrument: false
    },

    events: {
        entityClick: 'entityClick.dj.PortalArticle',
        accessionNumClick: 'accessionNumClick.dj.PortalArticle',
        sourceClick: 'sourceClick.dj.PortalArticle',
        authorClick: 'authorClick.dj.PortalArticle',
        anchorClick: 'anchorClick.dj.PortalArticle',
        postProcessingClick: 'postprocessing.dj.PortalArticle',
        eLinkClick: 'eLinkClick.dj.PortalArticle',
        headlineLinkClick: 'headlineLinkClick.dj.PortalArticle',
        smallPictureClick: 'smallPictureClick.dj.PortalArticle',
        enlargeImageLinkClick: 'enlargeImageLinkClick.dj.PortalArticle'
    },

    selectors: {
        articleContainer: '.dj_article-container',
        postProcessing: '.dj_PostProcessing li.action',
        entityLinks: '.dj_article_entity',
        anchorLinks: '.dj_article_anchor',
        sourceLinks: '.dj_article_source',
        authorLinks: '.dj_article_author',
        eLinks: '.dj_article_elink',
        headline: '.headline',
        headlineLink: '.dj_article_headline_link',
        smallPictureImg: 'img.smallImage',
        accessionNum: '.dj_article_accessionNum',
        enlargeImageLink: '.dj_article_enlargeImg_link'
    },

    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "PortalArticleComponent" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        if (this.data) {
            this.setData(this.data);
        }

    },

    _initializeElements: function () {
    },

    _initializeDelegates: function () {
        this._super();
    },

    _initializeEventHandlers: function () {
        var self = this;

        this.$element.on('click', this.selectors.entityLinks, function (e) {
            var data = $(this).data("entity");
            //Need element reference to show the callout pop
            //Adding it to data to keep the component backward compatible 
            data.element = this;
            self.publish(self.events.entityClick, data);
            return false;
        });

        this.$element.on('click', this.selectors.anchorLinks, function (e) {
            self.publish(self.events.anchorClick, { href: $(this).data("href") });
            return false;
        });

        this.$element.on('click', this.selectors.accessionNum, function (e) {
            self.publish(self.events.accessionNumClick,
                {
                    event: e,
                    entityType: 'accessionNum',
                    entityCode: $(this).data("accessionNum")
                });
        });

        this.$element.on('click', this.selectors.sourceLinks, function (e) {
            self.publish(self.events.sourceClick,
                {
                    event: e,
                    entityType: 'source',
                    entityCode: $(this).data("entity").fcode
                });
            return false;
        });

        this.$element.on('click', this.selectors.authorLinks, function (e) {
            self.publish(self.events.authorClick,
                {
                    event: e,
                    entityType: 'author',
                    entityCode: $(this).data("entity").fcode
                });
            return false;
        });

        this.$element.on('click', this.selectors.eLinks, function (e) {
            self.publish(self.events.eLinkClick, { href: $(this).data("href") });
            return false;
        });

        this.$element.on("click", this.selectors.postProcessing, function (e) {
            self.publish(self.events.postProcessingClick,
                             $.extend({
                                 type: $(this).data("ref"),
                                 title: $(self.selectors.headline, self.$element).text()
                             }, self.$element.data("ref")));
            return false;
        });

        this.$element.on("click", this.selectors.headlineLink, function (e) {
            self.publish(self.events.headlineLinkClick,
                             $.extend({ title: $(this).text() },
                                      self.$element.data("ref")));
            return false;
        });

        this.$element.on("click", this.selectors.smallPictureImg, function (e) {
            self.publish(self.events.smallPictureClick, { largeImgSrc: $(this).data("ref") });
            return false;
        });

        this.$element.on("click", this.selectors.enlargeImageLink, function (e) {
            self.publish(self.events.enlargeImageLinkClick, { LargeImgSrc: $(this).data("href") });
            return false;
        });
    },

    setData: function (data) {
        if (!data) {
            this.bindOnNoData(data);
            return;
        }

        if (data.status !== 0) {
            this.bindOnError(data);
            return;
        }

        this.bindOnSuccess(data);
    },

    bindOnSuccess: function (data) {
        var timerName = 'Article Rendered In'
        if (this.options.instrument === true && (console && console.time)) {
            console.time(timerName);
        }

        var articleMarkup = this.templates.success(data);
        this.$element.append(articleMarkup);

        if (this.options.instrument === true && (console && console.timeEnd)) {
            console.timeEnd(timerName);
        }

    },

    bindOnError: function (data) {
        this.templates.error(data);
    },

    bindOnNoData: function () {
        this.templates.noData();
    }

});

// Declare this class as a jQuery plugin
$.plugin('dj_PortalArticle', DJ.UI.PortalArticle);
