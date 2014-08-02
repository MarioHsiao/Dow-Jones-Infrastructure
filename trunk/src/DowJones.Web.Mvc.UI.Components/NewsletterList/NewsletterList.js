/*!
 * NewsletterList
 */

DJ.UI.NewsletterList = DJ.UI.Component.extend({
    selectors: {
        newsletterTable: '#editionTable',
        noResultSpan: 'span.dj_noResults',
        addBtn: 'a.add-to-newsletter-btn',
        addSectionBtn: "a.add-to-section",
        clearBtn: 'a.clear-newsletter-btn',
        gotoBtn: 'a.open-newsletter-btn',
        editBtn: 'a.edit-workspace-btn',
        alertCloseBtn: 'div.alert-box-close',
        createWorkspaceBtn: 'button.btn-save',
        createNewsletterBtn: 'button.btn_create-newsletter',
        getMoreInfoBtn: 'button.btn_get-moreInfo'
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
        noDataMessage: "<%=Token('newsletterIntroVerbeage'%>",
        filterText: "<%=Token('filter')%>",
        filteredText: "<%=Token('filtered')%>",
        emptyRecordsText: "<%=Token('emptyRecordsTitle')%>",
        showingText: "<%=Token('showing')%>",
        toText: "<%=Token('to')%>",
        fromText: "<%=Token('from')%>",
        totalText: "<%=Token('totalLabel')%>",
        ofText: "<%=Token('of')%>",
        entriesText: "<%=Token('entries')%>",
        createWorkspaceDesc: "<%=Token('createWorkspaceDesc')%>",
        briefcaseName: "<%=Token('briefcaseName')%>",
        save: "<%=Token('save')%>",
        saving: "<%=Token('saving')%>",
        createNewNewsletter: "<%=Token('createNewNewsletter')%>",
        getMoreInfo: "<%=Token('getMoreInfo')%>",
        newsletterIntroVerbeage: "<%=Token('newsletterIntroVerbeage')%>",
        processing: "<%=Token('processing')%>"
    },

    events: {
        addClick: "addClick.dj.NewsletterList",
        addSectionClick: "addSectionClick.dj.NewsletterList",
        clearClick: "clearClick.dj.NewsletterList",
        gotoNewsletterClick: "gotoNewsletterClick.dj.NewsletterList",
        newsletterEntryClick: "newsletterEntryClick.dj.NewsletterList",
        createWorkspaceClick: "createWorkspaceClick.dj.NewsletterList",
        editWorkspaceClick: "editWorkspaceClick.dj.NewsletterList",
        createNewsletterClick: "createNewsletterClick.dj.NewsletterList",
        getMoreInfoClick: "getMoreInfoClick.dj.NewsletterList"
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
        var self = this,
            newsletterTable = this.$element.find(this.selectors.newsletterTable);
        var sInfoText = self.tokens.showingText + " _START_ "
            + self.tokens.toText + " _END_ "
            + self.tokens.ofText + " _TOTAL_ "
            + self.tokens.entriesText;
        var sInfoFilteredText = " (" + self.tokens.filteredText + " "
                                + self.tokens.fromText.toLowerCase() + " _MAX_ "
                                + self.tokens.totalText.toLowerCase() + " "
                                + self.tokens.entriesText + ")";
        var table = $(newsletterTable).dataTable({
            "bFilter": self.options.allowFilter,
            "bPaginate": false,
            "sScrollY": self.options.allowFilter ? "260px" : "300px",
            "bDeferRender": true,
            "oLanguage": {
                "sSearch": "",
                "sInfo": sInfoText,
                "sZeroRecords": self.tokens.emptyRecordsText,
                "sInfoFiltered": sInfoFilteredText
            },
            'aoColumnDefs': [{
                'bSortable': false,
                'aTargets': [-1] /* 1st one, start by the right */
            }]
        });
        $('div.dataTables_filter input', this.$element).attr('placeholder', self.tokens.filterText + '...');

        //Row Highlighting
        $('tbody', newsletterTable)
        .on('mouseover', 'td', function () {
                if ($(this).parent().attr('id') != "sections") {
                    $('tbody td', table).removeClass('highlight');
                    $(this).addClass('highlight');
                    $(this).siblings().addClass('highlight');
                }
            })
        .on('mouseleave', function () {
            $('tbody td', table).removeClass('highlight');
        });

        //Add processing template
        self.$element.find("#editionTable_wrapper").prepend(self.templates.processing());
    },

    _showLoading: function (show, message) {
        var self = this;
        if (show) {
            self.$element.find('.alert-box').remove();
            this.$element.find('.dj_processing').removeClass('dj_hide');
        } else {
            this.$element.find('.dj_processing').addClass('dj_hide');
        }
    },

    _initializeNewsletter: function () {
        var self = this;
        self._initializeSortable();

        self.$element.on('click', self.selectors.addBtn, function (e) {
            $dj.publish(self.events.addClick, { nid: $(this).data('nlid') });
            return false;
        });

        self.$element.on('click', self.selectors.addSectionBtn, function (e) {
            var nlId = $(this).closest('tr#sections').prev('tr').data('nlid');
            $dj.publish(self.events.addSectionClick, { nlid: nlId, ind: $(this).data('index'), positionIndicator: $(this).data('pi') });
            return false;
        });

        self.$element.on('click', self.selectors.createWorkspaceBtn, function (e) {
            $(this).attr('disabled', 'disabled').addClass('disabled').text(self.tokens.saving + "...");
            $dj.publish(self.events.createWorkspaceClick, $(this).siblings('.text-createWorkspace').val());
            return false;
        });


        self.$element.on('click', self.selectors.createNewsletterBtn, function (e) {
            $dj.publish(self.events.createNewsletterClick);
            return false;
        });

        self.$element.on('click', self.selectors.getMoreInfoBtn, function (e) {
            $dj.publish(self.events.getMoreInfoClick);
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

        self.$element.on('click', self.selectors.alertCloseBtn, function (e) {
            var alertBox = $(this).parent();
            $(alertBox).fadeOut(500, function () { $(alertBox).remove(); });
        });
    },

    _initializeEventHandlers: function () { },

    _setData: function (data, type) {
        this.data = data;
        if (data)
            this.bindOnSuccess(data, type);
        else
            this.bindOnSuccess({});
    },

    _setSectionsData: function (newsletterId, sectionsHtml) {
        var nlRow = $('table#editionTable', this.$element).find("tr[data-nlid='" + newsletterId + "']");
        $('table#editionTable', this.$element).find('tr#sections').remove();
        $("<tr id='sections'><td colspan='3'>" + sectionsHtml + "</td></tr>").insertAfter(nlRow);
    },

    bindOnSuccess: function (data, type) {
        var self = this;
        try {
            self.$element.html("");
            if (data && data.result && data.result.resultSet && data.result.resultSet.count.value > 0) {
                // call to bind and append html to ul in one shot
                data.result.resultSet.options = { type: self.options.type };
                self.$element.append(self.templates.successNewsletters(data.result.resultSet)).removeClass('add-workspace');

            }
            else {
                if (type && type.toLowerCase() === "workspace") {
                    self.$element.append(self.templates.createWorkspace()).addClass('add-workspace');
                } else {
                    self.$element.append(self.templates.createNewsletter()).addClass('add-newsletter');
                }
            }
            // bind events and perform other wiring up
            self._initializeNewsletter();
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

    updateStatus: function (data) {
        var self = this;
        self.$element.find('.alert-box').remove();
        $('.dataTables_wrapper', self.$element).prepend(self.templates.notification(data));
        setTimeout(function () {
            var alertBox = self.$element.find('.alert-box');
            $(alertBox).fadeOut(500, function () { $(alertBox).remove(); });
        }, 3000);
    },

    EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
});


// Declare this class as a jQuery plugin
$.plugin('dj_NewsletterList', DJ.UI.NewsletterList);
