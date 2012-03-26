(function ($) {

    DJ.UI.TwitterExperts = DJ.UI.Component.extend({

        templates: {},
            
        // Default options
        defaults: {
            debug: false,
            cssClass: 'TwitterExperts'

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

            this.$element.delegate(this.selectors.profile, 'click', function () {
                this._publish(this.events.showProfile, { userId: $(this).parents(expertContentContainer).data('user-id'), screenName: $(this).parents(expertContentContainer).data('screen-name')});
            });


        },

        init: function (element, meta) {
            var $meta = $.extend({ name: "TwitterExperts" }, meta);
            this._super(element, $meta);
        },

        setData: function (data) {
           // this.$element.html("");
            if (data && data.topExperts && data.topExperts.length > 0) {
                this.$element.append(this.templates.success({ experts: data.topExperts, options: { viewAll: data.viewAll} }));
            }
        }
    });


    $.plugin('dj_TwitterExperts', DJ.UI.TwitterExperts);


})(jQuery);