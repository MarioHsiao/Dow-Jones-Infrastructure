/*
* DJ.UI.TwitterExperts 
*/

DJ.UI.TwitterExperts = DJ.UI.Component.extend({

    // Default options
    defaults: {
    },

    events: {
        // jQuery events are namespaced as <event>.<namespace>
        showProfile: "showProfile.dj.TwitterExperts"

    },


    selectors: {
        profile: '.expert-profile-link',
        expertContentContainer: 'li.top_expert-data'
    },

    _initializeEventHandlers: function () {
        this._super();
        var me = this;
        this.$element.delegate(this.selectors.profile, 'click', function () {
            var container = $(this).parents(me.selectors.expertContentContainer);
            this._publish(me.events.showProfile, {
                userId: container.data('user-id'),
                screenName: container.data('screen-name')
            });
        });

    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "TwitterExperts" }, meta);
        this._super(element, $meta);

    },

    setData: function (data, template) {
        template = template || 'success';

        switch (template) {
            case 'success': this._bindOnSuccess(data); break;
            case 'error': this._bindOnError(data); break;
            default: this._bindOnNoData(); break;
        }
    },

    _bindOnSuccess: function (data) {
        var topexperts;

        try {
            this.$element.html("");
            if (data && data.length && data.length > 0) {
                // call to bind and append html to the div section 

                topexperts = this.templates.topexperts({ experts: data, options: this.options });
                this.$element.append(topexperts);
            }
            else {
                this.$element.append(this.templates.noData());
            }
        } catch (e) {
            $dj.error('bindOnSuccess:: Error while data binding:', e);
        }

    },

    _bindOnError: function (data) {
    },

    _bindOnNoData: function (data) {
    }

});


$.plugin('dj_TwitterExperts', DJ.UI.TwitterExperts);
