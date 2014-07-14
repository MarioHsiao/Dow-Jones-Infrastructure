/*!
 * NewsletterSectionList
 */

    DJ.UI.NewsletterSectionList = DJ.UI.Component.extend({
        selectors: {
            addBtn: 'a.add-to-section'
        },

        events: {
            addClick: "addClick.dj.NewsletterList"
        },

        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "NewsletterSectionList" }, meta));

            this._setData(this.data);
        },
        
        _initializeNewsletterSections: function () {
            var self = this;

            self.$element.on('click', self.selectors.addBtn, function () {
                var $this = $(this);
                $dj.publish(self.events.addClick, { nlid: self.data.nlid, ind: $this.data('index'), positionIndicator: $this.data('pi') });
            });
        },

        _setData: function (data) {

            if (data && data.newsletters)
                this.bindOnSuccess(data.newsletters);
            else
                this.bindOnSuccess({});
        },

        bindOnSuccess: function (data) {
            var self = this;
            try {
                self.$element.html("");
                if (data && data.length > 0) {
                    // call to bind and append html to ul in one shot
                    self.$element.append(this.templates.success(data));

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
