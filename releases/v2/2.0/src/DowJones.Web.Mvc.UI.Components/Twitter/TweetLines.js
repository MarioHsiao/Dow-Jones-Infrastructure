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

(function ($) {

    DJ.UI.TweetLines = DJ.UI.Component.extend({

        /*
        * Properties
        */

        // Default options
        defaults: {
            debug: false,
            cssClass: 'TweetLines'
            // ,name: value     // add more defaults here separated by comma
        },

        
        events: {
            retweet: 'retweet.dj.tweetLines'
            , markFavorite: 'markFavorite.dj.tweetLines'
            , reply: 'reply.dj.tweetLines'
            , showMore: 'showMore.dj.tweetLines'
        },

        selectors: {
            retweet: 'a.retweet-action'
            , favorite: 'a.favorite-action'
            , reply: 'a.favorite-action'
            , more: 'a.more-action',
            , tweetActions: 'span.tweet-actions'
        },


        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "TweetLines" }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // TODO: Add custom initialization code like the following:
            // this._testButton = $('.testButton', element).get(0);
        },


        /*
        * Public methods
        */

        // TODO: Public Methods here


        /*
        * Private methods
        */

        
        _initializeElements: function ($ctx) {
            this.$retweet = $ctx.find(this.selectors.retweet);
            this.$reply = $ctx.find(this.selectors.reply);
            this.$favorite = $ctx.find(this.selectors.favorite);
            this.$tweetActions = $ctx.find(this.selectors.tweetActions);
        },


        _initializeEventHandlers: function () {
            this._super();
            var me = this;

            this.$element.delegate(this.selectors.retweet, 'click', function () { 
                this._publish(this.events.retweet, { tweetId: $(this).parents(this.selectors.tweetActions).data('tweet-id') });
            });
            this.$element.delegate(this.selectors.reply, 'click', function () { 
                this._publish(this.events.reply, { tweetId: $(this).parents(this.selectors.tweetActions).data('tweet-id') });
            });
            this.$element.delegate(this.selectors.favorite, 'click', function () { 
                this._publish(this.events.favorite, { tweetId: $(this).parents(this.selectors.tweetActions).data('tweet-id') });
            });
        }



    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_tweetLines', DJ.UI.TweetLines);


})(jQuery);