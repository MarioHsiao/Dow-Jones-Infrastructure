
    DJ.UI.DidYouMean = DJ.UI.Component.extend({

        init: function (element, meta) {
            this._super(element, $.extend({ name: "DidYouMean" }, meta));
        },

        getSelectedEntities: function () {
            var selectedEntities = this.entities.filter(':checked');

            var filters = $.map(selectedEntities, function (el, i) {
                return {
                    category: $(el).closest('.recognized-entity-group').data('category'),
                    code: $(el).data('code'),
                    name: $(el).data('name'),
                    searchtext: $(el).data('searchtext')
                };
            });

            return filters;
        },


        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                OnOkClicked: $dj.delegate(this, this._onOkClicked)
            });
        },

        _initializeElements: function () {
            this.entities = $('.recognized-entity', this.$element);
            this.okButton = $('.dj_OkButton', this.$element);
            this.listOfItems = $('.list-items', this.$element);
            this.showHide = $('.showHide', this.$element);
            this.dymContext = $('#Context', this.$element);
        },

        _initializeEventHandlers: function () {
            var me = this;
            this.okButton.click(this._delegates.OnOkClicked);

            this.showHide.click(function () {
                var $this = $(this);
                var $icon = $this.find('.dj_icon');
                var $label = $this.find('.label');
                if ($icon.hasClass('dj_icon-less')) {
                    $icon.removeClass('dj_icon-less').addClass('dj_icon-more');
                    $label.html("<%= Token('show') %>");
                    me.okButton.hide();
                    me.listOfItems.hide();
                    $dj.publish('entitiesToggle.dj.DidYouMean', { visible: true });
                }
                else {
                    $icon.removeClass('dj_icon-more').addClass('dj_icon-less');
                    $label.html("<%= Token('hide') %>");
                    me.okButton.show();
                    me.listOfItems.show();
                    $dj.publish('entitiesToggle.dj.DidYouMean', { visible: false });
                }
            });

        },

        _onOkClicked: function (e) {
            var entities = this.getSelectedEntities();
            var context = jQuery.parseJSON(this.dymContext.val());

            $dj.publish('entitiesRecognized.dj.DidYouMean', { entities: entities, context: context });
            e.stopPropagation();
            return false;
        },

        EOF: null
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_DidYouMean', DJ.UI.DidYouMean);
