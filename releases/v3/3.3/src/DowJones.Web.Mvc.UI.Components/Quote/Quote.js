
    DJ.UI.Quote = DJ.UI.Component.extend({

        templates: {},

        events: {
            sourceClick: 'sourceClick.dj.Quote'
        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "Quote" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // call databind if we got data from server
            if (this.data)
                this.bindOnSuccess(this.data);
        },


        bindOnSuccess: function (data) {
            this.$element.html("");
            if (data) {
                this.$element.append(this.templates.success({ quote: data }));

                if (data.provider && data.provider.name) {
                    var me = this;
                    this.$element.find('span.market-data-source').click(function () {
                        me.$element.triggerHandler(me.events.sourceClick, { url: data.provider.externalUrl, source: data.provider.name });
                    });
                }
            }
            else {
                this.$element.append(this.templates.noData());
            }
        },

        bindOnError: function (data) {
            this.$element.html("");
            this.$element.append(this.templates.error(data));
        },

        setData: function (data) {
            this.data = data;
            this.bindOnSuccess(data);
        }
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_Quote', DJ.UI.Quote);


    $dj.debug('Registered DJ.UI.Quote (extends DJ.UI.Component)');
