
    DJ.UI.SearchNavigator = DJ.UI.Component.extend({
        data: {
            primaryGroupId: null,
            secondaryGroupId: null,
            filter: null
        },

        events: {
            sourceFilterChanged: 'sourceFilterChanged.dj.SearchNavigator',
            toggleView: 'toggleView.dj.SearchNavigator'
        },


        init: function (el, meta) {
            this._super(el, $.extend(meta, { name: "SearchNavigator" }));
        },

        // added by RE (UX) to dynamically update the filters container height
        // this method will be removed if a pure CSS solution can be adopted
        updateFiltersContainerHeight: function () {

            this._filtersContainer.parent('div').height( this._filtersContainer.parents('.column').height() - this._sourceGroupsContainer.height() );

        },

        setData: function (data) {
            $.extend(true, this.data, data);

            this._primaryGroupInput.val(this.data.primaryGroupId);
            this._secondaryGroupInput.val(this.data.secondaryGroupId);

            this._triggerFilterChanged();
        },

        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                OnSourceGroupClick: $dj.delegate(this, this._onSourceGroupClick),
                OnFilterClick: $dj.delegate(this, this._onFilterClick),
                OnToggleViewClick: $dj.delegate(this, this._onToggleViewClick),
                ToggleFiltersContainer: $dj.delegate(this, this._toggleFiltersContainer)
            });
        },

        _initializeElements: function () {
            $.extend(this, {
                _sourceGroupsContainer: $('.source-list', this.$element),
                _filtersContainer: $('.filters', this.$element),
                _primaryGroupInput: $('.primaryGroupInput', this.$element),
                _secondaryGroupInput: $('.secondaryGroupInput', this.$element),
                _toggleView: $('.toggle-view', this.$element),
                _layout: $('input[name=layout]', this.$element)
            });

            this.updateFiltersContainerHeight();

        },

        _initializeEventHandlers: function () {
            $('.source', this._sourceGroupsContainer).click(this._delegates.OnSourceGroupClick);
            $('.item a', this._filtersContainer).click(this._delegates.OnFilterClick);
            $('.trigger', this._filtersContainer).click(this._delegates.ToggleFiltersContainer);
            this._toggleView.click(this._delegates.OnToggleViewClick);
        },

        _onSourceGroupClick: function (e) {
            var group = $(e.currentTarget).data('group');
            this.setData({ primaryGroupId: group, secondaryGroupId: null });
            e.stopPropagation();
            return false;
        },

        _onFilterClick: function (e) {
            var target = $(e.currentTarget).parent('.item');

            var group = target.data('group');
            var code = target.data('code');
            var name = target.children('.name').text();

            var filter = { category: group, code: code, name: name };
            var data = { filter: filter };

            if (target.data('secondary-group')) {
                data.secondaryGroupId = group;
                filter.category = 'Source';
                filter.code = group;
                filter.codeType = 'ProductDefineCode';
            }

            this.setData(data);

            e.stopPropagation();
            return false;
        },

        _onToggleViewClick: function (e) {
            var v = this._layout.val();
            var t = (v.toLowerCase() === "full") ? "split" : "full";
            this._layout.val(t);
            $dj.publish(this.events.toggleView, t);
        },

        _toggleFiltersContainer: function (e) {
            $(e.target).parent('.branch').toggleClass('expanded');
        },

        _triggerFilterChanged: function () {
            $dj.publish(this.events.sourceFilterChanged, this.data);
        },

        EOF: true
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_SearchNavigator', DJ.UI.SearchNavigator);
