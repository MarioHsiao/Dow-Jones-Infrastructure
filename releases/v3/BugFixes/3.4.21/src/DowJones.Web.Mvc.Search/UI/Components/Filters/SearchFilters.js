
    DJ.UI.SearchFilters = DJ.UI.Component.extend({

        filtersManager: null,

        selectors: {
            filter: '.dj_pill',
            filterGroup: '.filter-group'
        },


        init: function (el, meta) {
            this._super(el, $.extend(meta, { name: "SearchFilters" }));
        },


        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                OnAddFilters: $dj.delegate(this, this._onAddFilters),
                OnClear: $dj.delegate(this, this._onClear),
                OnRemoveFilters: $dj.delegate(this, this._onRemoveFilters)
            });
        },

        _initializeElements: function () {
            this.clearButton = $('.clear-filters', this.$element);
            this.filtersContainer = $('.dj_pill-list', this.$element);

            this.filtersManager = this.$element.find('input.filters').dj_SearchFiltersManager();
        },

        _initializeEventHandlers: function () {
            this.clearButton.click(this._delegates.OnClear);
            this.filtersContainer.find('.remove').live('click', this._delegates.OnRemoveFilters);

            $dj.subscribe('addFilters.dj.SearchFilters', this._delegates.OnAddFilters);
        },

        _onAddFilters: function (filters) {
            this.filtersManager.add(filters);
            this._onFiltersChanged();
        },

        _onClear: function (e) {
            this.filtersManager.clear();
            this._onFiltersChanged();
        },

        _onFiltersChanged: function () {
            $dj.publish('filtersChanged.dj.SearchFilters');
        },

        _onRemoveFilters: function (e) {
            var filter = $(e.target).parent(this.selectors.filter);
            var code = filter.data('code');

            filter.remove();

            if (code) {
                this.filtersManager.remove(code);
                this._onFiltersChanged();
            }
        },

        EOF: null
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_SearchFilters', DJ.UI.SearchFilters);
