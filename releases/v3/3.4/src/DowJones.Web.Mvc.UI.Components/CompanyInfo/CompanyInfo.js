
DJ.UI.CompanyInfo = DJ.UI.Component.extend({

    templates: {},

    events: {
        companySnapshotClick: 'companySnapshotClick.dj.CompanyInfo',
        reportHeadlineClick: 'reportHeadlineClick.dj.CompanyInfo'
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
            this.$element.append(this.templates.success({ data: data, options: this.options }));

            //Render Report Headlines
            if (data.investorReportHeadlinesData) {
                me.renderReportHeadlines(data.investorReportHeadlinesData);
            }

            //Attach event handlers
            if (this.options.enableCompanySnapshotLink) {
                this.$element.find('.company-snapshot').click(function () {
                    me.publish(me.events.companySnapshotClick, data.companyCode);
                });
            }
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
    },

    renderReportHeadlines: function (data) {
        var self = this;
        DJ.add("PortalHeadlineList", {
            container: "investor-report-headlines",
            options: {
                displaySnippets: 3, // Hover
                maxNumHeadlinesToShow: 3,
                showAuthor: false,
                showSource: false,
                showPublicationDateTime: false,
                showTruncatedTitle: true
            }
        }).done(function (comp) {
            // Attach handler for headline click event
            comp.on("headlineClick.dj.PortalHeadlineList", function (data) {
                self.publish(self.events.reportHeadlineClick, data);
            });

            // Set data
            comp._setData({
                resultSet: data.resultSet
            });
        });
    }
});

// Declare this class as a jQuery plugin
$.plugin('dj_CompanyInfo', DJ.UI.CompanyInfo);


$dj.debug('Registered DJ.UI.CompanyInfo (extends DJ.UI.Component)');
