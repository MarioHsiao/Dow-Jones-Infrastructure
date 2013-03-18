
    DJ.UI.QueryBuilder = DJ.UI.Component.extend({

        getQuery: function () {
            this._debug('getQuery not implemented!');
            throw 'getQuery not implemented!';
        },

        getSearchRequest: function () {
            this._debug('Getting initial search builder context.');
            //var context = $("#SearchBuilderContext").val();
            var context = $("input[name='request']").val();
            if (context) {
                return $.parseJSON(context);
            }
        },

        getRawSearchRequest: function () {
            this._debug('Getting initial search builder context.');
            return $("input[name='request']").val();
        },

        EOF: null

    });

    $.plugin('dj_QueryBuilder', DJ.UI.QueryBuilder);

