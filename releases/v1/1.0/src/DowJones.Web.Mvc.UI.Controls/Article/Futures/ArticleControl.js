(function ($) {
    DJ.UI.Article = DJ.UI.Component.extend({
        init: function (element, m) {
            var _self = this
                , $control = $(element)
                , $content = $control.find('.wrap').not('.template')
                , request = null
                , meta = $.extend({ name: 'Article' }, m);

            _self.id = element.id;

            function bindCustomHandlers() {
                _self._subscribe('article/received', '_handleArticleReceived');
                _self._subscribe('search/changed', '_handleSearchChanged');
                _self._subscribe('filters/changed', '_handleSearchChanged');
            }

            function bindDomHandlers() {
                $control.delegate('dj_article_entity', 'click', function () {
                    var $item = $(this)
                        , ref = $item.attr('rel');
                    _self._publish('entity/clicked', ref);
                });
                $control.delegate('.dj_article_elink', 'click', function () {
                    var $item = $(this)
                        , ref = $item.attr('rel');
                    _self._publish('elink/clicked', ref);
                });
                $control.delegate(".dj_article_entity", "mouseover", function () {
                    var $item = $(this)
                    , ref = $item.attr('rel');
                    _self._publish('entity/hover', ref);
                });
            }

            _self._handleArticleReceived = function (ev, data) {
                $content.html('');
                $control.removeClass('module-loading');
                var templateSel = '#article-item-template'
                    , article = data || {};
                
                $content.html(_self.templates.atccontent({ article: article }));
            };
            _self._handleSearchChanged = function (ev, query) {
                if (request) {
                    request.abort();
                    request = null;
                }

                $control.removeClass('module-loading');
                $content.html('');
            };

            // Call the base constructor
            _self._super(element, meta);
            bindCustomHandlers();
            bindDomHandlers();
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_Article', DJ.UI.Article);

    $dj.debug('Registered DJ.UI.Article (extends DJ.UI.Component');
})(jQuery);
