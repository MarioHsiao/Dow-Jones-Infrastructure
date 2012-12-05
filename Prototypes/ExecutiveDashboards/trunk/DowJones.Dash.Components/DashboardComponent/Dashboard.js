
DJ.UI.Dashboard = DJ.UI.CompositeComponent.extend({

    defaults: {
        cssClass: 'dj_dashboard'
    },

    selectors: {
        noDataContainer: '.noData',
    },

    // Constructor
    init: function (element, meta) {
        var $meta = $.extend({ name: "Dashboard" }, meta);
        this._super(element, $meta);

        this.events = this.events || {};
        $.extend(this.events, this._baseEvents);
    },

    _initializeElements: function (ctx) {
        ctx.html(this.templates.container());
    },

    showNoData: function () {
        this.$element.find(this.selectors.noDataContainer).show('fast');
    },

    hideNoData:function(){
        this.$element.find(this.selectors.noDataContainer).hide('fast');
    },

    EOF: null

});

// Declare this class as a jQuery plugin
$.plugin('dj_Dashboard', DJ.UI.Dashboard);
