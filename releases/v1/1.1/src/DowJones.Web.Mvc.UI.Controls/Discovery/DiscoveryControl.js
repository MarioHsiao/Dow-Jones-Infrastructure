(function ($) {
    DJ.UI.Discovery = DJ.UI.Component.extend({
        init: function (element, m) {
            var _self = this
                , $control = $(element)
                , $content = $control.find('.sections').not('.template')
                , meta = $.extend({ name: 'Discovery' }, m);

            _self.id = element.id;

            function bindCustomHandlers() {
                _self._subscribe('search/changed', '_handleSearchChanged');
                _self._subscribe('filters/changed', '_handleSearchChanged');
                _self._subscribe('discovery/received', '_handleDiscoveryReceived');
            }

            function bindDomHandlers() {
                $control.delegate('.item', 'click', function () {
                    var $item = $(this)
                        , type = $item.attr('data-type')
                        , name = $item.attr('data-name')
                        , value = $item.attr('data-value');

                    if (!type || !value) { return false; }

                    var term = {
                        type: type,
                        name: name,
                        value: value
                    };

                    _self._publish('filter/clicked', term);

                    return false;
                });
            }

            _self._handleSearchChanged = function (ev, query) {
                $control.removeClass('module-loading');
                $content.html('');
            }
            _self._handleDiscoveryReceived = function (ev, data) {
                var templateSel = '#discovery-control-section-template'
                    , discovery = data || {};
                
                $control.removeClass('module-loading');
                $content.html(_self.templates.dsccontent({ dsccloud: _self.templates.dsccloud, dscdate: _self.templates.dscdate, dsclist: _self.templates.dsclist, discovery: discovery }));
            };

            // Call the base constructor
            _self._super(element, meta);
            bindCustomHandlers();
            bindDomHandlers();
        }
    });



    // Declare this class as a jQuery plugin
    $.plugin('dj_Discovery', DJ.UI.Discovery);

    $dj.debug('Registered DJ.UI.Discovery (extends DJ.UI.Component');
})(jQuery);
