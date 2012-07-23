
DJ.UI.Discovery = DJ.UI.Component.extend({
    selectors: {
        noResultSpan: 'span.dj_noResults',
        discoveryItem: '.item'
    },
    options: {
    },
    events: {
        itemClick: 'itemClick.dj.Discovery'
    }
    , init: function (element, m) {
        var meta = $.extend({ name: 'Discovery' }, m);

        // Call the base constructor
        this._super(element, meta);
    },
    // gets called during base.init()
    _initializeEventHandlers: function () {
        this._super();
        var $container = this.$element
                , self = this;

        $container.delegate('.discovery-type', 'click', function () {
            var $group = $(this).closest('.discovery-group')
                    , $items = $group.find('.discovery-items');

            $group.toggleClass('expanded');

            if ($group.hasClass('expanded')) {
                $items.hide().show(300, 'linear');
            }
            else {
                $items.show().hide(300, 'linear');
            }

            return false;
        });
        $container.delegate(self.selectors.discoveryItem, 'click', function () {
            var $this = $(this)
                    , type = $this.attr('data-type')
                    , name = $this.attr('data-name')
                    , value = $this.attr('data-value');

            var term = {
                type: type,
                name: name,
                value: value
            };

            $container.triggerHandler(self.events.itemClick, term);

            return false;
        });
    },
    _renderSuccess: function () {
        try {
            this.$element.empty();
            if (this.data && this.data.DiscoveryCollection && this.data.DiscoveryCollection.length > 0) {
                var html = this.templates.success(this.data);
                this.$element.append(html);
            }
            else {
                var html = this.templates.noData(this.data);
                this.$element.append(html);
            }
        } catch (e) {
            $dj.debug('Error in Discovery._renderSuccess');
            $dj.debug(e);
        }
    },
    _renderError: function () {
        try {
            this.$element.empty();
            this.$element.append(this.templates.error(data));
        } catch (e) {
            $dj.debug('Error in Discovery._renderError');
            $dj.debug(e);
        }
    },
    getSuccessTemplate: function () {
        return this.templates.success;
    },
    setSuccessTemplate: function (markup) {
        this.templates.success = _.template(markup);
    },
    getNoDataTemplate: function () {
        return this.templates.noData;
    },
    setNoDataTemplate: function (markup) {
        this.templates.noData = _.template(markup);
    },
    getErrorTemplate: function () {
        return this.templates.error;
    },
    setErrorTemplate: function (markup) {
        this.templates.error = _.template(markup);
    },
    setData: function (data) {
        this.data = data;

        if (data) {
            this._renderSuccess();
        }
        else {
            this._renderError();
        }
    }
});

$.plugin('dj_Discovery', DJ.UI.Discovery);
$dj.debug('Registered DJ.UI.Discovery (extends DJ.UI.Component');
