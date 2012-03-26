(function ($) {

    DJ.UI.SearchFiltersManager = DJ.UI.Component.extend({

        filters: [],


        init: function (el, meta) {
            this._super(el, $.extend(meta, { name: "SearchFiltersManager" }));
        },


        add: function (filters) {
            this._debug('Adding filters: ' + JSON.stringify(filters));

            // Make a copy of the existing filters
            var newFilters = $.merge([], this.filters);

            if (!$.isArray(filters)) {
                filters = [filters];
            }

            for (var i = 0; i < filters.length; i++) {
                var item = filters[i];
                if (!this._isMultipleAllowed(item)) {
                    newFilters = this._removePreviousItems(newFilters, item);
                }
                newFilters.push(item);
            }

            this._setFilters(newFilters);
        },

        clear: function () {
            this._debug('Clearing filters...');
            this._setFilters([]);
        },

        remove: function (codes) {
            this._debug('Removing filters: ' + JSON.stringify(filters));

            var codesIsArray = $.isArray(codes);

            var filter = _.select(this.filters, function (x) {
                var code = x.code;

                var isMatch = function (y) { return y == x.code; };

                return (codesIsArray)
                    ? _.any(codes, isMatch)
                    : isMatch(codes);
            });

            var filters = _.without(this.filters, filter[0]);

            this._setFilters(filters);
        },


        _initializeElements: function () {
            this.formField = this.$element;

            if (this.formField.val())
                this.filters = JSON.parse(this.formField.val());
        },

        _initializeEventHandlers: function () { },

        _setFilters: function (filters) {
            this.filters = filters;
            this._syncFormField();
        },

        _syncFormField: function () {
            this.formField.val(JSON.stringify(this.filters));
            this._debug('Updated Filters Value: ' + this.formField.val());
        },

        _isMultipleAllowed: function (filter) {
            var cat = filter && filter.category;
            var codeType = filter && filter.codeType;
            return !(cat === "Source" && codeType === "ProductDefineCode");
        },

        _removePreviousItems: function (filters, item) {
            //var cat = item.category;
            var codeType = item.codeType;
            var temp = $.merge([], filters);
            var newArray = _.reject(temp, function (item) {
                return (/*item.category === cat && */ item.codeType === codeType);
            });
            return newArray;
        },

        EOF: true

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_SearchFiltersManager', DJ.UI.SearchFiltersManager);

})(jQuery);