
DJ.UI.CompanyInfo = DJ.UI.Component.extend({

    templates: {},

    events: {
        companySnapshotClick: 'companySnapshotClick.dj.CompanyInfo'
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "CompanyInfo" }, meta);

        // Call the base constructor
        this._super(element, $meta);

        // call databind if we got data from server
        if (this.data)
            this.bindOnSuccess(this.data);
    },


    bindOnSuccess: function (data) {
        this.$element.html("");
        if (data) {
            var me = this;
            this.$element.append(this.templates.success(data));
            //Attach event handler
            this.$element.find('.company-snapshot').click(function () {
                me.publish(me.events.companySnapshotClick);
            });
        }
        else {
            this.$element.append(this.templates.noData());
        }
    },

    bindOnError: function (data) {
        this.$element.html("");
        this.$element.append(this.templates.error(data));
    },

    setData: function (data) {
        this.data = data;
        this.bindOnSuccess(data);
    }
});

// Declare this class as a jQuery plugin
$.plugin('dj_CompanyInfo', DJ.UI.CompanyInfo);


$dj.debug('Registered DJ.UI.CompanyInfo (extends DJ.UI.Component)');
