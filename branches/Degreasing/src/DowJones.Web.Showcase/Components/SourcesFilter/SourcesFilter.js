
    DJ.UI.SourcesFilter = DJ.UI.CompositeComponent.extend({

        eventNames: {
            SourcesFilterChanged: 'sourcesFilterChanged.dj.SourcesFilter'
        },

        _classNames: {
            selected: 'selected'
        },

        init: function (element, meta) {
            this._filtersContainer = $('.filters', element);

            this._super(element, meta);
        },

        _initializeDelegates: function () {
            $.extend(this._delegates, {
                OnFilterClick: $dj.delegate(this, this._OnFilterClick)
            });

            $('li', this._filtersContainer).click(this._delegates.OnFilterClick);
        },

        _OnFilterClick: function (evt) {
            $(evt.target).toggleClass(this._classNames.selected);

            var selectedFilters = $('li.selected', this._filtersContainer);
            var filter = { sources: selectedFilters.map(function () { return $(this).data('item'); }).get() };

            this.publish(this.eventNames.SourcesFilterChanged, filter);
        },

        EOF: null
    });

    $.plugin('dj_SourcesFilter', DJ.UI.SourcesFilter);

    $dj.debug('Registered DJ.UI.SourcesFilter as dj_SourcesFilter');
