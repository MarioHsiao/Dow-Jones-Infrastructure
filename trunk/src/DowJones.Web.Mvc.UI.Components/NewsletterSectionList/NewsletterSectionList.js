/*!
 * NewsletterSectionList
 */

    DJ.UI.NewsletterSectionList = DJ.UI.Component.extend({
        selectors: {},

        tokens: {
            placeSelectedItems: '<%=Token("placeSelectedItems")%>',
            topOfEdition: '<%=Token("topOfEdition")%>',
            bottomOfEdition: '<%=Token("bottomOfEdition")%>',
            addActionTooltip: "<%=Token('add')%>",
            noDataMessage: ""
        },

        events: {},

        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "NewsletterSectionList" }, meta));
            
            if (this.data) {
                this._setData(this.data);
            }
        },
        
        _initializeNewsletterSections: function (){},

        _setData: function (data) {
            this.data = data;

            if (data) {
                this.bindOnSuccess(data);
            }
            else
                this.bindOnSuccess({});
        },

        bindOnSuccess: function (data) {
            var self = this;
            try {
                self.$element.html("");
                if (data && data.result && data.result.resultSet && data.result.resultSet.count.value > 0) {
                    // call to bind and append html to ul in one shot
                    self.$element.append(this.templates.success(data.result.resultSet));

                    // bind events and perform other wiring up
                    this._initializeNewsletterSections();
                }
                else {
                    // display no data
                    this.$element.append(this.templates.noData());
                }
            } catch (e) {
                $dj.error('Error in NewsletterSectionList.bindOnSuccess:', e);
            }
        },

        bindOnError: function (data) {
            try {
                this.$element.html("");
                this.$element.append(this.templates.error(data));
            } catch (e) {
                $dj.error('Error in NewsletterSectionList.bindOnError:', e);
            }
        },

        setErrorTemplate: function (markup) {
            this.templates.error = _.template(markup);
        },

        setNoDataTemplate: function (markup) {
            this.templates.noData = _.template(markup);
        },

        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_NewsletterSectionList', DJ.UI.NewsletterSectionList);
