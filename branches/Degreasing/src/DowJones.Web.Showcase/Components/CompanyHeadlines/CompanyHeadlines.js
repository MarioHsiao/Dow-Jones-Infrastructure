
    DJ.UI.CompanyHeadlines = DJ.UI.CompositeComponent.extend({

        eventNames: {
            CompanyFilterChanged: 'companyFilterChanged.dj.CompanyHeadlines',
            FilterUpdated: 'filterUpdated.dj.CompanyHeadlines'
        },

        _classNames: {
            filtered: 'filtered',
            selected: 'selected'
        },

        _filter: {},

        init: function (element, meta) {
            this._filtersContainer = $('.filters', element);
            this._companiesList = $('.dj_headlineListContainer', element);

            this._super(element, meta);
        },

        getData: function () {
            this._super();
            $.ajax({
                url: this.options.dataServiceUrl,
                contentType: 'application/json',
                data: JSON.stringify(this._filter),
                type: 'POST',
                success: $dj.delegate(this, this.setData)
            });
        },

        getFilter: function () {
            return this._filter;
        },

        setData: function (data) {
            var selectedClass = this._classNames.selected;

            $('li', this._filtersContainer)
                .removeClass(selectedClass)
                .each(function (i, filter) {
                    $.each(data.Companies, function (x, company) {
                        if ($(filter).data("item") == company) {
                            $(filter).addClass(selectedClass);
                        }
                    });
                });

            this._companiesList.findComponent(DJ.UI.PortalHeadlineList).bindOnSuccess(data.Headlines.Result.resultSet);
        },

        setFilter: function (filter) {
            this._filter = filter;
            this.publish(this.eventNames.FilterUpdated, this._filter);
        },

        _initializeDelegates: function () {
            $.extend(this._delegates, {
                OnFilterClick: $dj.delegate(this, this._OnFilterClick)
            });
        },

        _initializeEventHandlers: function () {
            $('li', this._filtersContainer).click(this._delegates.OnFilterClick);

            this.subscribe(this.eventNames.FilterUpdated, $dj.delegate(this, this.getData));

            // NOTE: explicitly take ownership of child controls
            // NOTE: this will be redundant after the next update to framework as the parent will ensure ownership 
            var self = this;
            this.$element.find('.ui-component').each(function () {
                var $comp = $(this).findComponent(DJ.UI.Component);
                $comp.setOwner(self);
            });


            this.subscribe('headlineClick.dj.PortalHeadlineList', $dj.delegate(this, this._onHeadlineClick));


        },

        _OnFilterClick: function (evt) {
            $(evt.target).toggleClass(this._classNames.selected);

            var selectedFilters = $('li.selected', this._filtersContainer);
            var filter = { companies: selectedFilters.map(function () { return $(this).data('item'); }).get() };

            filter = $.extend(this._filter, filter);

            this.publish(this.eventNames.CompanyFilterChanged, filter);
        },

        _onHeadlineClick: function (args) {
            $dj.debug('Received Headline Click. Headlines:', args);
        },

        EOF: null
    });

    $.plugin('dj_CompanyHeadlines', DJ.UI.CompanyHeadlines);

    $dj.debug('Registered DJ.UI.CompanyHeadlines as dj_CompanyHeadlines');
