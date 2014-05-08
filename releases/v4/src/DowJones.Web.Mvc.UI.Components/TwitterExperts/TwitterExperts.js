/*
* DJ.UI.TwitterExperts 
*/

DJ.UI.TwitterExperts = DJ.UI.Component.extend({

    // Default options
    defaults: {
        webIntents: {
            follow: 'https://twitter.com/intent/user?user_id='
        },
        webIntentWindowOptions: {
            scrollbars: 'yes',
            resizable: 'yes',
            toolbar: 'no',
            location: 'yes',
            width: 550,
            height: 420
        }
    },

    events: {
        // jQuery events are namespaced as <event>.<namespace>
        showProfile: "showProfile.dj.TwitterExperts"

    },


    selectors: {
        profile: '.expert-profile-link',
        expertContentContainer: 'li.top_expert-data',
        topExperts: '.dj_twitter-top-experts',
        topExpertsItem: '.dj_experts-item',
        topExpertsAnchor: '.dj_experts-item a',
        follow: '.follow'
    },

    classnames: {
        fullname: 'dj_full-name',
        screenname: 'dj_screen-name'
    },

    _initializeElements: function (ctx) {
        //this.$topExperts = ctx.find(this.selectors.topExperts);
    },


    _initializeEventHandlers: function () {
        this._super();
        var me = this;

        //this.$topExperts not ready yet;
        this.$element.on("click", this.selectors.topExpertsAnchor, function (e) {
            var $el = $(this);
            var id,
                $expertItem = $(this).closest(me.selectors.topExpertsItem);

            if ($el.hasClass(me.classnames.fullname))
                action = "profileName";
            else if ($el.hasClass(me.classnames.screenname))
                action = "profileHandle";

            me._recordODSData({
                action: action,
                twitterHandle: $expertItem.data('screen-name'),
                twitterName: $expertItem.data('full-name'),
                twitterId: $expertItem.attr("data-user-id")
            });
        });


        this.$element.on("click", this.selectors.follow, function (e) {
            var $expertItem = $(this).closest(me.selectors.topExpertsItem);

            me._openWebIntent("follow", $expertItem.attr("data-user-id"));
            me._recordODSData({
                action: "follow",
                twitterHandle: $expertItem.data('screen-name'),
                twitterName: $expertItem.data('full-name'),
                twitterId: $expertItem.attr("data-user-id")
            });
        });
    },

    init: function (element, meta) {
        var $meta = $.extend({ name: "TwitterExperts" }, meta);
        this._super(element, $meta);

    },

    setData: function (data, template) {
        template = template || 'success';

        if (template === 'success') {
            this._bindOnSuccess(data);
        }
        else {
            this._bindOnNoData();
        }
    },

    _bindOnSuccess: function (data) {
        var expertsHtml;

        try {
            if (data && data.length && data.length > 0) {
                // call to bind and append html to the div section 
                expertsHtml = this.templates.experts(data);
                this.$element.html(expertsHtml);

                // initialize the fancy scroll bar only once
                var me = this;
                window.setTimeout(function () {
                    if (!me.scrollBar) {
                        me.scrollBar = me.$element.dj_ScrollBar();
                    }
                }, 0);
            }
            else {
                this._bindOnNoData();
            }
        } catch (e) {
            $dj.error(this.name, '::bindOnSuccess: Error while data binding:', e);
        }

    },

    _bindOnError: function (data) {
    },

    _bindOnNoData: function () {
        this.$element.html(this.templates.noData());
    },

    _getWebIntentUrl: function (action, id) {
        if (!this.options.webIntents[action]) {
            $dj.error(this.name, "::getWebIntentUrl: action '", action, "' is not recognized.");
            return;
        }

        return this.options.webIntents[action] + id;
    },


    _getWebIntentWindowOptions: function () {
        var features,
            options = this.options.webIntentWindowOptions,
            opt;
        for (opt in options) {
            if (options.hasOwnProperty(opt)) {
                features += opt + '=' + options[opt] + ',';
            }
        }

        return features.replace(/,$/g, '');
    },


    _openWebIntent: function (action, id) {
        return window.open(this._getWebIntentUrl(action, id), 'intent', this._getWebIntentWindowOptions());
    },


    _recordODSData: function (ODSdata) {
        $dj.recordODSData([{ "Name": "FCS_OD_ModuleAction", "Value": ODSdata.action},
                           { "Name": "FCS_OD_ModuleSection", "Value": "TopExperts" },
                           { "Name": "FCS_OD_TwittererId", "Value": ODSdata.twitterId },
                           { "Name": "FCS_OD_TwittererHandle", "Value": ODSdata.twitterHandle},
                           { "Name": "FCS_OD_TwitterName", "Value": ODSdata.twitterName },
                           { "Name": "FCS_OD_ODSTranName", "Value": "SocialMediaUsage" }
                     ]);
    }

});


$.plugin('dj_TwitterExperts', DJ.UI.TwitterExperts);
