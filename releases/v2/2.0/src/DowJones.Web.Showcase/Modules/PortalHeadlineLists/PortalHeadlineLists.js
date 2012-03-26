/*!  PortalHeadlineLists  */

(function ($) {

    DJ.UI.PortalHeadlineLists = DJ.UI.AbstractCanvasModule.extend({

        selectors: {
            newsFeedTitles: 'h3.module-col-title span.module-col-title-source-icon-text',
            viewAllBtns: 'ul.view-all-btn a.dashboard-control',
            viewAllErrDiv: 'div.dj_viewAllErr',
            newsFeedIcons: 'h3.module-col-title img.module-col-title-source-img',
            portalHeadlineLists: 'div.dj_headlineListContainer',
            footer: '.module-footer',
            feedArea: 'div.module-col'
        },

        events: {
            portalHeadlinesViewAllClick: 'viewAllClick.dj.PortalHeadlineLists',
            portalHeadlinesHeadlineClick: 'headlineClick.dj.PortalHeadlineLists',
            portalHeadlinesSourceClick: 'sourceClick.dj.PortalHeadlineLists'
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "PortalHeadlineLists" }, meta);
            this._super(element, meta);
        },

        _initializeElements: function () {
            this._super();

            this.newsFeedTitles = $(this.selectors.newsFeedTitles, this.element);
            this.portalHeadlineLists = $(this.selectors.portalHeadlineLists, this.element);
            this.viewAllBtns = $(this.selectors.viewAllBtns, this.element);
            this.viewAllErrDivs = $(this.selectors.viewAllErrDiv, this.element);
            this.newsFeedIcons = $(this.selectors.newsFeedIcons, this.element);
            this.feedAreas = $(this.selectors.feedArea, this.element);
            this.$footer = $(this.selectors.footer, this.element);
        },


        _initializeDelegates: function () {
            this._super();
        },


        _initializeEventHandlers: function () {
            this._super();
            _.each(this.viewAllBtns, function (viewAllBtn) {
                $(viewAllBtn).unbind('click').click(function (e) {
                    this._publish(this.events.portalHeadlinesViewAllClick, {
                        elem: e.targetElement
                    });

                    e.stopPropagation();
                    return false;
                });
            }, this);
        },

        _getData: function () {
            this.showContentArea();
        }

    });

    $.plugin('dj_PortalHeadlineLists', DJ.UI.PortalHeadlineLists);

    $dj.debug('Registered DJ.UI.PortalHeadlineLists as dj_PortalHeadlineLists');

} (jQuery)); 