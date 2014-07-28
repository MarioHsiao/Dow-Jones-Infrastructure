/*!
 * NewsletterList
 */

    DJ.UI.NewsletterList = DJ.UI.Component.extend({
        selectors: {
            newsletterTable: '#editionTable',
            noResultSpan: 'span.dj_noResults',
            addBtn: 'a.add-to-newsletter-btn',
            clearBtn: 'a.clear-newsletter-btn',
            gotoBtn: 'a.open-newsletter-btn',
            editBtn: 'a.edit-workspace-btn'
        },

        defaults: {
            showAddAction: true,
            showEditAction: false,
            showClearAction: true,
            showGotoAction: true,
            allowFilter: true
        },

        tokens: {
            nameColumnLabel: "<%=Token('name')%>",
            dateColumnLabel: "<%=Token('date')%>",
            actionsColumnLabel: "<%=Token('actions')%>",
            addLinkTooltip: "<%=Token('add')%>",
            clearLinkTooltip: "<%=Token('clear')%>",
            editLinkTooltip: "<%=Token('edit')%>",
            gotoLinkTooltip: "<%=Token('goToNewsletter')%>",
            noDataMessage: "<%=Token('newsletterIntroVerbeage'%>"
        },

        events: {
            addClick: "addClick.dj.NewsletterList",
            clearClick: "clearClick.dj.NewsletterList",
            gotoNewsletterClick: "gotoNewsletterClick.dj.NewsletterList",
            newsletterEntryClick: "newsletterEntryClick.dj.NewsletterList",
            editWorkspaceClick: "editWorkspaceClick.dj.NewsletterList"
        },

        init: function (element, meta) {
            // Call the base constructor
            this._super(element, $.extend({ name: "NewsletterList" }, meta));

            // Initialize component if we got data from server
            if (this.data) {
                this._setData(this.data);
            }
        },
        
        _initializeSortable: function () {
            var self = this;
            this.$element.find(this.selectors.newsletterTable).dataTable({
                "bFilter": self.options.allowFilter,
                "bPaginate": false,
                "sScrollY": self.options.allowFilter ? "260px" : "300px",
                "bDeferRender": true,
                "oLanguage": {
                    "sSearch": ""
                },
                'aoColumnDefs': [{
                    'bSortable': false,
                    'aTargets': [-1] /* 1st one, start by the right */
                }]
            });
            $('div.dataTables_filter input', this.$element).attr('placeholder', 'Filter...');
        },

        _initializeNewsletter: function () {
            var self = this;
            self._initializeSortable();
            
            self.$element.on('click', self.selectors.addBtn, function (e) {
                $dj.publish(self.events.addClick, { nid: $(this).data('nlid') });
                return false;
            });

            self.$element.on('click', self.selectors.clearBtn, function (e) {
                $dj.publish(self.events.clearClick, { nid: $(this).data('nlid') });
                return false;
            });

            self.$element.on('click', self.selectors.gotoBtn, function (e) {
                $dj.publish(self.events.gotoNewsletterClick, { nid: $(this).data('nlid') });
                return false;
            });

            self.$element.on('click', self.selectors.editBtn, function (e) {
                $dj.publish(self.events.editWorkspaceClick, { nid: $(this).data('nlid') });
                return false;
            });
        },

        _initializeEventHandlers: function(){

        },

        _setData: function (data) {
            this.data = data;
            if (data)
                this.bindOnSuccess(data);
            else
                this.bindOnSuccess({});
        },

        _setSectionsData: function(newsletterId, sectionsHtml) {
            var nlRow = $('table#editionTable', this.$element).find("tr[data-nlid='" + newsletterId + "']");
            //$('td.actions a.add-to-newsletter-btn', this.$element).show();
            //$('td.actions a.add-to-newsletter-btn', nlRow).hide();
            $('table#editionTable', this.$element).find('tr#sections').remove();
            $("<tr id='sections'><td colspan='3'>" + sectionsHtml + "</td></tr>").insertAfter(nlRow);
        },

        bindOnSuccess: function (data) {
            var self = this;
            try {
                self.$element.html("");
                if (data && data.result && data.result.resultSet && data.result.resultSet.count.value > 0) {
                    // call to bind and append html to ul in one shot
                    data.result.resultSet.options = { type: self.options.type };
                    self.$element.append(this.templates.successNewsletters(data.result.resultSet));

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
