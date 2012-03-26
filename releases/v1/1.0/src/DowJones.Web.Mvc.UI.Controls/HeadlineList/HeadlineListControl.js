(function ($) {
    DJ.UI.HeadlineList = DJ.UI.Component.extend({
        init: function (element, m) {
            var _self = this
                , $control = $(element)
                , $list = $control.find('.dj_headlineListContainer').not('.template')
                , $items = $list.find('.item')
                , meta = $.extend({ name: 'HeadlineList' }, m);

            _self.id = element.id;



            function bindCustomHandlers() {
                _self._subscribe('search/changed', '_handleSearchChanged');
                _self._subscribe('filters/changed', '_handleSearchChanged');
                _self._subscribe('headlines/received', '_handleHeadlinesReceived');
            }

            function bindDomHandlers() {
                $('.dj_headlineListContainer').data('options', { displaySnippets: 'hover' });
                $('a.title').dj_snippetTooltip('dj_tooltip', 'dj_headlineListContainer');

                //primary click
                $list.delegate('.item', 'click', function () {
                    var $item = $(this)
                        , djid = $item.attr('data-djid')
                        , uri = $item.attr('rel');

                    if (!djid) { return false; }

                    _self._publish('headline/clicked', djid, uri);
                    $items.removeClass('selected');
                    $item.addClass('selected');

                    return false;
                });
                //dup accordian
                $list.delegate('.trigger', 'click', function () {
                    var $this = $(this)
                        , $accent = $this.find('.accent');

                    if ($accent.text() == '+') {
                        $this.parent().find('.items').show();
                        $accent.text('-');
                    }
                    else {
                        $this.parent().find('.items').hide();
                        $accent.text('+');
                    }

                    return false;
                });
                //check headline
                $list.delegate('.chk-headline', 'click', function (ev) {
                    var $item = $(this)
                        , djid = $item.val()
                        , state = $item.attr('checked');
                    console.log($item);
                    if (!djid) { return false; }

                    if (state) {
                        _self._publish('headline/checked', djid);
                    }
                    else {
                        _self._publish('headline/unchecked', djid);

                    }
                    ev.stopPropagation();
                });
                //source over
                $list.delegate('.source', 'mouseover', function () {
                    var $item = $(this)
                        , djid = $item.attr('data-djid');

                    if (!djid) { return false; }

                    _self._publish('source/hover', djid);

                    return false;
                });
            }

            _self._handleSearchChanged = function (ev, query) {
                $control.removeClass('module-loading');
                $list.html('');
            };

            _self._handleHeadlinesReceived = function (ev, data) {
                var templateSel = '#headline-list-template'
                    , headlines = data || [];

                headlines.options = meta.options;
                $control.removeClass('module-loading');
                //$list.html(_self._template(templateSel, { headlines: headlines }));
                $list.html(_self.templates.hlcontent({ headlines: headlines }));

                $items = $list.find('.item');
                $items.eq(0).addClass('selected');
            };

            // Call the base constructor
            _self._super(element, meta);
            bindCustomHandlers();
            bindDomHandlers();
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_HeadlineList', DJ.UI.HeadlineList);

    $dj.debug('Registered DJ.UI.HeadlineList (extends DJ.UI.Component');
})(jQuery);
