
DJ.UI.DashboardComponent = DJ.UI.CompositeComponent.extend({

    defaults: {
        cssClass: 'dj_dashboardComponent'
    },


    // Constructor
    init: function (element, meta) {
        var $meta = $.extend({ name: "DashboardComponent" }, meta);
        this._super(element, $meta);

        this.events = this.events || {};
        $.extend(this.events, this._baseEvents);
    },

    _initializeElements: function (element) {
    },

    EOF: null

});

