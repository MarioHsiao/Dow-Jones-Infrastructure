

(function ($) {
    var request = null;

    var onSearchChanged = {
        query: function (control, query) {
            if (request) {
                request.abort();
                request = null;
            }
            var term = {
                type: 'text',
                name: query,
                value: query
            };
            terms = [term];
            onFiltersChanged.query(this, terms);
        }


    };
    var onFiltersChanged = {
        query: function (control, terms) {
            if (request) {
                request.abort();
                request = null;
            }

            var query = generateQuery(terms)
                , url = SITE_ROOT + 'made/resultset';

            $.ajax({
                url: url,
                type: 'POST',
                data: query,
                contentType: 'application/json; charset=utf-8',
                traditional: true,
                success: function (response, status) {
                    request = null;
                    if (status == 'success') {
                        $dj.publish('headlines/received', this, response.HeadlineList.Headlines || {});
                        $dj.publish('discovery/received', this, response.Discovery || {});
                        $dj.publish('article/received', this, response.FirstArticle || {});
                        $dj.publish('content/received', this, response);
                    }
                    else {
                        $dj.publish('content/received', this, response.ErrorMessage || {});
                        document.write('error in response');
                    }
                }
            });
        }
    };
    var onHeadlineClicked = {
        query: function (control, accessionNo) {
            if (request) {
                request.abort();
                request = null;
            }
            var query = '{accessionNumber: \'' + accessionNo + '\'}'
                , url = SITE_ROOT + 'made/article?' + query;

            $.ajax({
                url: url,
                type: 'POST',
                data: query,
                contentType: 'application/json; charset=utf-8',
                traditional: true,
                success: function (response, status) {
                    request = null;

                    if (status == 'success') {
                        $dj.publish('article/received', this, response);
                    }
                    else {
                        $dj.publish('article/received', this, {});
                    }
                }
            });
        }
    }

    $dj.subscribe('filters/changed', onFiltersChanged, 'query');
    $dj.subscribe('headline/clicked', onHeadlineClicked, 'query');
    $dj.subscribe('search/changed', onSearchChanged, 'query');

    function generateQuery(terms) {
        var tokens = [];

        var connector = {
            type: 'connector',
            name: 'AND',
            value: 'AND'
        };

        $.each(terms, function (idx, term) {
            tokens.push(term);
            if (idx < terms.length - 1) {
                tokens.push(connector);
            }
        });

        return $.toJSON({ search: tokens });
    }

})(jQuery);
