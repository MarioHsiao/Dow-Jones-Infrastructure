
DJ.UI.DashboardComponent = DJ.UI.CompositeComponent.extend({

    defaults: {
        cssClass: 'dj_dashboard'
    },

    coreSelectors: {
        noDataContainer: '.noData',
        content: '.content'
    },
    
    coreTemplates: {
        container: function() { return "<div class=\"kpiComponent\"><div class=\"content\"></div><div class=\"noData\">Coming Soon...</div></div>"; }
    },

    // Constructor
    init: function (element, meta) {
        var $meta = $.extend({ name: "Dashboard" }, meta);
        this._super(element, $meta);

        this.events = this.events || {};
        $.extend(this.events, this._baseEvents);
    },

    _initializeElements: function (ctx) {
        this._super(ctx);
        ctx.html(this.coreTemplates.container());
        this.$container = this.$element.find(this.coreSelectors.content);
        this.showContent();
    },
    
    updateContent: function(cHtml) {
        this.$element.find(this.coreSelectors.content).html(cHtml);
    },

    hideContent: function () {
        this.$element.find(this.coreSelectors.content).hide('fast');
        this.$element.find(this.coreSelectors.noDataContainer).show('fast');
    },

    showContent: function () {
        this.$element.find(this.coreSelectors.content).show('fast');
        this.$element.find(this.coreSelectors.noDataContainer).hide('fast');
    },

    EOF: null

});

// Declare this class as a jQuery plugin
$.plugin('dj_Dashboard', DJ.UI.Dashboard);
