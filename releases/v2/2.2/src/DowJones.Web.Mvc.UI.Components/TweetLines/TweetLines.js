// !
// TweetLines
//

DJ.UI.TweetLines = DJ.UI.Component.extend({

	// Default options
	defaults: {
		webIntents: {
			follow: "https://twitter.com/intent/user?user_id=",
			reply: "https://twitter.com/intent/tweet?in_reply_to=",
			retweet: "https://twitter.com/intent/retweet?tweet_id=",
			favorite: "https://twitter.com/intent/favorite?tweet_id=",
			details: "https://twitter.com/#!/{screen-name}/status/"
		},
		maxTweetsToShow: 100,
		maxPagesInHistory: -1 // allow infinite history
	},

	events: {
		loadNewTweets: 'loadNewTweets.dj.tweetLines',
		loadOldTweets: 'loadOldTweets.dj.tweetLines'
	},

	selectors: {
		tweetItem: 'li.dj_tweet-item',
		tweetItemAnchor: '.dj_tweet-item a',
		tweetActions: '.dj_post-processing li',
		newTweets: '.dj_new-tweets',
		oldTweets: '.dj_old-tweets',
		recentItems: '.dj_twitter-recent-tweets',
		toTop: '.dj_to-top-wrap',
		tweetHrefs: '.dj_tweet a',
		timeStamp: '.dj_time-stamp span:first-child'
	},

	classNames: {
		fullName: 'dj_full-name',
		screenName: 'dj_screen-name'
	},

	init: function (element, meta) {
		var $meta = $.extend({ name: "TweetLines" }, meta);

		// Call the base constructor
		this._super(element, $meta);
		//this.setData(this.data);

		this.timeago();

		// counter to track no. of time 'load old' is clicked
		this.historyClicks = 0;


	},


	dispose: function () {
		this._super();
		// will cause timeago to stop refresh on next interval
		this.$element.remove();
	},


	_initializeElements: function (ctx) {

		this.$newTweets = ctx.find(this.selectors.newTweets);
		this.$newTweetsSpan = this.$newTweets.find('span');

		this.$oldTweets = ctx.find(this.selectors.oldTweets);
		this.$oldTweetsSpan = this.$oldTweets.find('span');

		this.$recentItems = ctx.find(this.selectors.recentItems);

		this.$toTop = ctx.find(this.selectors.toTop);

	},

	_initializeEventHandlers: function () {
		this._super();
		var me = this;

		this.$element.on("click", this.selectors.tweetActions, function () {
			var $el = $(this),
				action = $el.data("action");
			var id,
                tweetItem = $el.closest(me.selectors.tweetItem);

			// DO NOT use jQuery.data().
			// jQuery.data applies parseInt to it and the number gets rounded off
			// http://stackoverflow.com/questions/9297434/parseint-rounds-incorrectly/
			if (action === "follow") { id = tweetItem.attr("data-user-id"); }
			else { id = tweetItem.attr("data-tweet-id"); }

			// fix the screen name (although twitter will take any junk screen name for now)
			if (action === "details") {
				me.options.webIntents.details = me.defaults.webIntents.details.replace("{screen-name}", tweetItem.data("screen-name"));
			}

			me._openWebIntent(action, id);

			me._recordODSData({
				action: action,
				twitterHandle: tweetItem.data('screen-name'),
				twitterName: tweetItem.data('full-name')
			});

			//return false;
		});

		this.$oldTweetsSpan.click(function () {
			// if its not inifinite history and load more has been clicked max times, 
			// hide the button to disable paging in history
			if (me.options.maxPagesInHistory > 0 && me.historyClicks >= me.options.maxPagesInHistory) {
				$(this).hide();
			}
			else {
				me.publish(me.events.loadOldTweets);
				me.historyClicks++;
				me._recordODSData({
					action: 'more'
				});
			}
			return false;
		});

		this.$newTweetsSpan.click(function () {
			me.publish(me.events.loadNewTweets);

			me._recordODSData({
				action: 'new'
			});

			return false;
		});

		this.$element.find('.dj_to-top').click(function () {
			me.$element.animate({ scrollTop: 0 }, 600);
		});


		this.$element.on("click", this.selectors.tweetHrefs, function (e) {
			var $this = $(this),
				txt = $this.text(),
                startsWith = txt.charAt(0),
                action, expandedUrl;


			if (startsWith === '#') {
				action = 'hashTagClick';
			}
			else if ($this.data('screen-name')) {
				action = 'userMentionClick';

			}
			else {
				action = 'urlClick';
				expandedUrl = $this.data('expanded-url');
			}

			me._recordODSData({
				action: action,
				twitterHandle: tweetItem.data('screen-name'),
				twitterName: tweetItem.data('full-name'),
				expandedUrl: expandedUrl
			});

			e.stopPropagation();
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
		this.$element.timeago(this.selectors.timeStamp);
	},


	setData: function (data, template) {
		template = template || 'success';

		switch (template) {
			case 'success': this.bindOnSuccess(data); break;
			case 'error': this.bindOnError(data); break;
			default: this.bindOnNoData(); break;
		}

		
	},


	refreshData: function () {

	},


	notifyNewTweets: function (count, displayFinalMessage) {
		count = count || 0;
		if (count > 0) {
			var msg = (displayFinalMessage ? '<%= Token("about") %> ' : '') + count + ' <%= Token("newTweets") %>';
			this.$newTweetsSpan.text(msg);
			this.$newTweets.removeClass("hide").show();
		}
		else {
			this.$newTweets.hide();
		}
	},


	// not browser history but navigating to historical tweets
	toggleHistory: function (enable) {
		if (enable === undefined) {
			this.$oldTweets.toggle();
			this.$toTop.toggle();
		}
		else if (enable === true) {
			this.$oldTweets.show().removeClass('hide');
			this.$toTop.hide();
		}
		else {
			this.$oldTweets.hide();
			this.$toTop.show().removeClass('hide');
		}

		if (this.scrollBar) {
			this.scrollBar.refresh();
		}
	},


	bindOnSuccess: function (data) {
		var tweetLines;

		try {
			if (!data || !data.tweets || data.tweets.length === 0) {
				this.bindOnNoData();
				return;
			}

			this.$newTweets.addClass("hide");

			// call to bind and append html to the div section 
			tweetLines = this.templates.tweetlines({ tweets: data.tweets, options: this.options });

			if (data.append === true) {
				this.$recentItems.append(tweetLines);
			}
			else {
				this.$recentItems.prepend(tweetLines);
			}

			// update timestamp
			this.$recentItems.find(this.selectors.timeStamp).timeago(null, true);


			// initialize the fancy scroll bar only once 
			// settimeout allows the DOM binding to happen before 
			// so that the plugin can get physical dimesnions on attach.
			var me = this;
			window.setTimeout(function () {
				if (!me.scrollBar) {
					me.scrollBar = me.$element.dj_ScrollBar();
				}
				else {
					me.scrollBar.refresh();
				}
			}, 0);

		}
		catch (e) {
			this.toggleHistory(false);
			$dj.error(this.name, '::bindOnSuccess: Error while data binding. Details: ', e);
		}

	},


	bindOnNoData: function () {
		this.$recentItems.html(this.templates.noData());
		this.$newTweets.addClass("hide");
		this.$oldTweets.hide();
		this.$toTop.hide();
	},


	getSinceId: function () {
		var latestTweetItem = this.$recentItems.find(this.selectors.tweetItem + ':first');
		if (latestTweetItem) {
			return latestTweetItem.attr("data-tweet-id");
		}
	},


	getStartId: function () {
		var oldestTweetItem = this.$recentItems.find(this.selectors.tweetItem + ':last');
		if (oldestTweetItem) {
			return oldestTweetItem.attr("data-tweet-id");
		}
	},


	_getWebIntentUrl: function (action, id) {
		if (!this.options.webIntents[action]) {
			$dj.error(this.name, "::getWebIntentUrl: action '", action, "' is not recognized.");
			return;
		}

		return this.options.webIntents[action] + id;
	},


	_openWebIntent: function (action, id) {
		var options = 'scrollbars=yes, resizable=yes, toolbar=no, location=yes, width=550, height=420';
		return window.open(this._getWebIntentUrl(action, id), 'intent', options);
	},


	_recordODSData: function (ODSdata) {
		var fields = [
			{ name: "FCS_OD_ModuleSection", value: "RecentTweets" },
            { name: "FCS_OD_ODSTranName", value: "SocialMediaUsage" },
			{ name: "FCS_OD_ModuleAction", value: ODSdata.action },
			{ name: "FCS_OD_TwittererHandle", value: ODSdata.twitterHandle },
            { name: "FCS_OD_TwitterName", value: ODSdata.twitterName }

        ];

		if (ODSdata.expandedUrl) {
			fields.push({ name: "FCS_OD_Value", value: ODSdata.expandedUrl });
		}

		$dj.recordODSData(fields);
	}

});

// Declare this class as a jQuery plugin
$.plugin('dj_TweetLines', DJ.UI.TweetLines);
