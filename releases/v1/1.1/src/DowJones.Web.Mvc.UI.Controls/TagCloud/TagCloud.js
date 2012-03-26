(function ($) {
    DJ.UI.TagCloud = DJ.UI.Component.extend({

        templates: {
            success: _.template([
                '<ul class="dj_tag_cloud">',
                    '<% for (var index = 0, len = data.result.length; index < len; index++) {',
                        'h = data.result[index]; %>',
                        '<li class="<%=options.tagCloudCssPrefix %><%=h.distributionIndex%>">',
                            '<a href="javascript:void())">',
                                '<span rel="<%=index %>"><%=h.text %></span>',
                            '</a>',
                        '</li>',
                     '<% } %>',
                 '</ul>', ].join('')),
            error: _.template('<span class="dj_error"><%= code %>: <%= message %></span>'),
            noData: _.template("<span class='dj_noResults'><%= Token("noResults") %></span>")
        },
        defaults: {
            debug: false,
            cssClass: 'TagCloud'
        },

        // Localization/Templating tokens
        tokens: {
        //name: value add more defaults here separated by comma
    },

    events: {
        // jQuery events are namespaced as <event>.<namespace>
        tagItemClick: "tagItemClick.dj.TagCloud"
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "TagCloud" }, meta);
        this._super(element, $meta);

        this.container = element;
        this.$container = $(element);
    },

    selectors: {
        tagItem: 'ul li>a>span'
    },

    // gets called during base.init()
    _initializeEventHandlers: function () {
        this._super();
        var $parentContainer = this.$element
                , me = this;

        if (this.options.EnableEventFiring) {
            this.$element
                .delegate(this.selectors.tagItem, 'click', function (e) {
                    $parentContainer.triggerHandler(me.events.tagItemClick, { data: me.data.result[$(this).attr("rel")], element: this });
                    return false;
                });
        }
    },

    initialize: function () {
        var me = this;
        //$(me)[0].$container.find("ul li>a", this.$container).dj_simpleTooltip("tooltip");
    },

    bindOnSuccess: function () {
        if (this.data) {
            var data = this.data;
            this.$container.html("");
            if (data && data.result && data.result.length > 0) {
                // call to bind and append html to ul in one shot
                this.$container.append(this.templates.success({
                    data: data,
                    options: this.options

                }));
                // bind events and perform other wiring up
                this.initialize();
            }
            else {
                this.$container.append(this.templates.noData());
            }
        }
    },

    setData: function (Data) {
        this.data = Data;
        this.bindOnSuccess();
    },

    bindOnError: function (data) {
            try {
                this.$container.html("");
                this.$container.append(this.templates.error(data));
            } catch (e) {
                $dj.debug('Error in Tagcloud.bindOnError');
                $dj.debug(e);
            }
        }

});

// Declare this class as a jQuery plugin
$.plugin('dj_TagCloud', DJ.UI.TagCloud);

})(jQuery);
