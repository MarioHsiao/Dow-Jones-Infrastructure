/*!
 * TweetLines
 *   e.g. , "this._imageSize" is generated automatically.
 *
 *   
 *  Getters and Setters are generated automatically for every Client Property during init;
 *   e.g. if you have a Client Property called "imageSize" on server side code
 *        get_imageSize() and set_imageSize() will be generated during init.
 *  
 *  These can be overriden by defining your own implementation in the script. 
 *  You'd normally override the base implementation if you have extra logic in your getter/setter 
 *  such as calling another function or validating some params.
 *
 */

DJ.UI.TweetLines = DJ.UI.Component.extend({

    // Default options
    defaults: {
        webIntents: {
            follow: 'https://twitter.com/intent/user?user_id=',
            reply: "https://twitter.com/intent/tweet?in_reply_to=",
            retweet: "https://twitter.com/intent/retweet?tweet_id=",
            favorite: "https://twitter.com/intent/favorite?tweet_id="
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
        loadNewTweets: 'loadNewTweets.dj.tweetLines',
        loadOldTweets: 'loadOldTweets.dj.tweetLines'
    },

    selectors: {
        tweetItem: '.dj_tweet-item',
        tweetActions: '.dj_post-processing',
        newTweets: '.dj_new-tweets',
        oldTweets: '.dj_old-tweets',
        recentItems: '.dj_twitter-recent-tweets'
    },


    /*
    * Initialization (constructor)
    */
    init: function (element, meta) {
        var $meta = $.extend({ name: "TweetLines" }, meta);

        // Call the base constructor
        this._super(element, $meta);
        //this.setData(this.data);

        this.timeago();


    },


    _initializeElements: function (ctx) {
        this.$tweetActions = ctx.find(this.selectors.tweetActions);

        this.$newTweets = ctx.find(this.selectors.newTweets);
        this.$newTweetsSpan = this.$newTweets.find('span');

        this.$oldTweets = ctx.find(this.selectors.oldTweets);
        this.$oldTweetsSpan = this.$oldTweets.find('span');

        this.$recentItems = ctx.find(this.selectors.recentItems);

    },


    _initializeEventHandlers: function () {
        this._super();
        var me = this;

        this.$tweetActions.on("click", "li", function () {
            var action = $(this).data("action");
            var id;

            // DO NOT use jQuery.data().
            // jQuery.data applies parseInt to it and the number gets rounded off
            // http://stackoverflow.com/questions/9297434/parseint-rounds-incorrectly/
            if (action === "follow") {
                id = $(this).closest(me.selectors.tweetItem).attr("data-user-id");
            }
            else {
                id = $(this).closest(me.selectors.tweetItem).attr("data-tweet-id");
            }
            me._openWebIntent(action, id);
        });

        this.$oldTweetsSpan.click(function () {
            me.publish(me.events.loadOldTweets);
            return false;
        });

        this.$newTweetsSpan.click(function () {
            me.publish(me.events.loadNewTweets);
            return false;
        });

    },


    timeago: function () {
        jQuery.timeago.settings.strings = {
            prefixAgo: null,
            prefixFromNow: null,
            suffixAgo: '<%= Token("ago") %>',
            suffixFromNow: '<%= Token("fromNow") %>',
            seconds: '%d <%= Token("seconds") %>',
            minute: '<%= Token("aboutAMinute") %>',
            minutes: '%d <%= Token("minutes") %>',
            hour: '<%= Token("aboutAnHour") %>',
            hours: '<%= Token("about") %> %d <%= Token("hours") %>',
            day: '<%= Token("aDay") %>',
            days: '%d <%= Token("days") %>',
            month: '<%= Token("aboutAMonth") %>',
            months: '%d <%= Token("months") %>',
            year: '<%= Token("aboutAnYear") %>',
            years: '%d <%= Token("years") %>',
            numbers: []
        };
        this.$element.timeago('.dj_time-stamp abbr');
    },

    setData: function (data, template) {
        template = template || 'success';

        switch (template) {
            case 'success': this.bindOnSuccess(data); break;
            case 'error': this.bindOnError(data); break;
            default: this.bindOnNoData(); break;
        }

        this.$newTweets.addClass("hide");
    },

    refreshData: function () {

    },

    notifyNewTweets: function (count) {
        count = count || 0;
        if (count > 0) {
            this.$newTweetsSpan.text(count + ' <%= Token("newTweets") %>');
            this.$newTweets.removeClass("hide");
        }
        else {
            this.$newTweets.addClass("hide");
        }
    },

    bindOnSuccess: function (data) {
        var tweetLines;

        try {
            if (data && data.tweets && data.tweets.length > 0) {
                // call to bind and append html to the div section 

                tweetLines = this.templates.tweetlines({ tweets: data.tweets, options: this.options });
                if (data.append === true) {
                    this.$recentItems.append(tweetLines);
                }
                else {
                    this.$recentItems.prepend(tweetLines);
                }

                $.fn.timeago.refresh();
            }
        } catch (e) {
            $dj.error('DJ.UI.TweetLines::bindOnSuccess: Error while data binding. Details: ', e);
        }

    },


    _getWebIntentUrl: function (action, id) {
        if (!this.options.webIntents[action]) {
            $dj.error("DJ.UI.TweetLines::getWebIntentUrl: action '", action, "' is not recognized.");
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
    }

});

// Declare this class as a jQuery plugin
$.plugin('dj_TweetLines', DJ.UI.TweetLines);