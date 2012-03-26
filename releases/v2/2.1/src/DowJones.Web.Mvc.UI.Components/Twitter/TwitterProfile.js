(function ($) {

    DJ.UI.TwitterProfile = DJ.UI.Component.extend({

        templates: {},

        // Default options
        defaults: {
            debug: false,
            cssClass: 'TwitterProfile'

        },

        events: {
            // jQuery events are namespaced as <event>.<namespace>
            followClick: "followClick.dj.TwitterProfile",
            titleClick: "titleClick.dj.TwitterProfile"
        },

        selectors: {
            follow: '.follow-action',
            profileDetials: '.profile-details',
            profileTitle: '.profile-title-action',
            lastTweet: '.last-tweet-date'
        },

        _initializeEventHandlers: function () {
            this._super();

            this.$element.delegate(this.selectors.follow, 'click', function () {
                this._publish(this.events.followClick, { userId: $(this).parents(profileDetials).data('user-id'), screenName: $(this).parents(profileDetials).data('screen-name') });
            });

            this.$element.delegate(this.selectors.profileTitle, 'click', function () {
                this._publish(this.events.titleClick, { userId: $(this).parents(profileDetials).data('user-id'), screenName: $(this).parents(profileDetials).data('screen-name') });
            });

        
            
        },

        _initializeElements: function ($ctx) {
            this.$followLink = $ctx.find(this.selectors.follow);
            this.$profileDetailsContainer = $ctx.find(this.selectors.profileDetials);
            this.$profileTitleContainer = $ctx.find(this.selectors.profileTitle);
            this.$lastTweetContainer = $ctx.find(this.selectors.lastTweet);

        },


        init: function (element, meta) {
            var $meta = $.extend({ name: "TwitterProfile" }, meta);
            this._super(element, $meta);
        },

        setData: function (data) {
            this.$element.html("");
            if (data) {
                this.$element.append(this.templates.success({ profile: data.profile, options: { splitScreen: data.splitScreen} }));

            }
        }
        
   
    });


    $.plugin('dj_TwitterProfile', DJ.UI.TwitterProfile);


})(jQuery);