/*!
 * NewsletterList
 */

    DJ.UI.NewsletterList = DJ.UI.Component.extend({
        selectors: {
            newsletterTable: '#editionTable',
            noResultSpan: 'span.dj_noResults',
            addBtn: 'a.add-to-newsletter',
            clearBtn: 'a.clear-newsletter',
            gotoBtn: 'a.goto-newsletter'
        },

        events: {
            addClick: "addClick.dj.NewsletterList",
            clearClick: "clearClick.dj.NewsletterList",
            gotoNewsletterClick: "gotoNewsletterClick.dj.NewsletterList",
            newsletterEntryClick: "newsletterEntryClick.dj.NewsletterList"
        },


        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "NewsletterList" }, meta));

            // Initialize component if we got data from server
            this._setData(this.data);
        },
        
        _initializeSortable: function () {
            this.$element.find(this.selectors.newsletterTable).tablesorter({
                cssHeader: "header",
                cssAsc: "headerSortUp",
                cssDesc: "headerSortDown",
                sortList: [[2, 1]],
                headers: { 0: { sorter: false }, 3: { sorter: false } },
                widgets: ['zebra'],
                dateFormat: 'd,MM,yy'
            });
        },

        _initializeNewsletter: function () {
            var self = this;
            self._initializeSortable();

            self.$element.on('click', self.selectors.addBtn, function () {
                $dj.publish(self.events.addClick, { nid: $(this).attr('id') });
            });

            self.$element.on('click', self.selectors.clearBtn, function () {
                $dj.publish(self.events.clearClick, { nid: $(this).attr('id') });
            });

            self.$element.on('click', self.selectors.gotoBtn, function () {
                $dj.publish(self.events.gotoNewsletterClick, { nid: $(this).attr('id') });
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
                    self.$element.append(this.templates.successNewsletters(data));

                    // bind events and perform other wiring up
                    this._initializeNewsletter();
                }
                else {
                    // display no data
                    this.$element.append(this.templates.noData());
                }
            } catch (e) {
                $dj.error('Error in NewsletterList.bindOnSuccess:', e);
            }
        },

        bindOnError: function (data) {
            try {
                this.$element.html("");
                this.$element.append(this.templates.error(data));
            } catch (e) {
                $dj.error('Error in NewsletterList.bindOnError:', e);
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
    $.plugin('dj_NewsletterList', DJ.UI.NewsletterList);
