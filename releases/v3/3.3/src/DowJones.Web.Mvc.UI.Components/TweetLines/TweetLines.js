// !
// TweetLines
//

DJ.UI.TweetLines = DJ.UI.Component.extend({

	// Default options
	defaults: {

		maxTweetsToShow: 100,
		maxPagesInHistory: 0, // allow infinite history
        ronBars : false
	},

	events: {
		loadNewTweets: 'loadNewTweets.dj.tweetLines',
		loadOldTweets: 'loadOldTweets.dj.tweetLines'
	},

	selectors: {
		tweetItem: 'li.dj_tweet-item',
		tweetItemAnchor: '.dj_tweet-item a',
		tweetActions: '.dj_post-processing li',
		tweetMeta: '.dj_tweet-meta a',
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

        var lang = this._getInterfaceLanguage();
		this.options.webIntents = {
			follow: "https://twitter.com/intent/user?lang=" + lang + "&user_id=",
			reply: "https://twitter.com/intent/tweet?lang=" + lang + "&in_reply_to=",
			retweet: "https://twitter.com/intent/retweet?lang=" + lang + "&tweet_id=",
			favorite: "https://twitter.com/intent/favorite?lang=" + lang + "&tweet_id=",
			details: "https://twitter.com/#!/{screen-name}/status/"
		};



		if (this.data && this.data.tweets && this.data.tweets.length > 0) {
		    this.setData(this.data);
		}
	},


	dispose: function () {
		this._super();
		// will cause timeago to stop refresh on next interval
		this.$element.remove();
	},


	_initializeElements: function (ctx) {
	    // see if this is being added via DJ.Add.
        // if yes, render container markup via templates first
	    this.isPurelyClientSide = !ctx.find(this.selectors.recentItems).length;

	    if (this.isPurelyClientSide) {
	        this.$element.html(this.templates.container());

            // re-init the context
	        ctx = this.$element;
	    }

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

		this.$element.on("click", this.selectors.tweetMeta, function (e) {
			var $el = $(this),
				action = $el.data("action"),
                tweetItem = $el.closest(me.selectors.tweetItem),
				userId = tweetItem.attr("data-user-id");

			me._recordODSData({
				action: action,
				twitterHandle: tweetItem.data('screen-name'),
				twitterName: tweetItem.data('full-name'),
				twitterId: userId
			});

			e.stopPropagation();
		});

		this.$element.on("click", this.selectors.tweetActions, function (e) {
			var $el = $(this),
				action = $el.data("action"),
				id,
                tweetItem = $el.closest(me.selectors.tweetItem),
				tweetId = tweetItem.attr("data-tweet-id");

			// DO NOT use jQuery.data().
			// jQuery.data applies parseInt to it and the number gets rounded off
			// http://stackoverflow.com/questions/9297434/parseint-rounds-incorrectly/
			if (action === "follow") { id = tweetItem.attr("data-user-id"); }

			// fix the screen name (although twitter will take any junk screen name for now)
			if (action === "details") {
				me.options.webIntents.details = me.options.webIntents.details.replace("{screen-name}", tweetItem.data("screen-name"));
			}

			id = id || tweetId;
			me._openWebIntent(action, id);

			me._recordODSData({
				action: action,
				twitterHandle: tweetItem.data('screen-name'),
				twitterName: tweetItem.data('full-name'),
				twitterId: tweetId
			});

			e.stopPropagation();
		});

		this.$oldTweetsSpan.click(function () {
			me.publish(me.events.loadOldTweets);
			me.historyClicks++;
			me._recordODSData({
				action: 'more'
			});

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
                tweetItem = $this.closest(me.selectors.tweetItem),
				txt = $this.text(),
                startsWith = txt.charAt(0),
                action, expandedUrl, tagValue;


			if (startsWith === '#') {
				action = 'hashTagClick';
				tagValue = txt.substring(1);
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
				tagValue: tagValue,
				expandedUrl: expandedUrl
			});

			e.stopPropagation();
		});
	},


	timeago: function () {
		jQuery.timeago.settings.strings = {
			prefixAgo: "<%= Token('agoPre') %>",
			prefixFromNow: "<%= Token('fromNowPre') %>",
			suffixAgo: "<%= Token('agoPost') %>",
			suffixFromNow: "<%= Token('fromNowPost') %>",
			seconds: "%d <%= Token('seconds') %>",
			minute: "<%= Token('aboutAMinute') %>",
			minutes: "%d <%= Token('minutes') %>",
			hour: "<%= Token('aboutAnHour') %>",
			hours: "<%= Token('about') %> %d <%= Token('hours') %>",
			day: "<%= Token('aDay') %>",
			days: "%d <%= Token('daysLowercase') %>",
			month: "<%= Token('aboutAMonth') %>",
			months: "%d <%= Token('months') %>",
			year: "<%= Token('aboutAnYear') %>",
			years: "%d <%= Token('years') %>",
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
			var msg = (displayFinalMessage ? "<%= Token('about') %> " : '') + count + " <%= Token('newTweets') %>";
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
			tweetLines = this.templates.tweetlines(data.tweets);

			if (data.refresh) {
				this.$element.animate({ scrollTop: 0 }, 0);
				this.$recentItems.html(tweetLines);
				// reset history clicks
				this.historyClicks = 0;

				if (this.options.maxPagesInHistory >= 0) {
					this.$oldTweets.show().removeClass('hide');
					this.$toTop.hide();
				}
			}
			else if (data.append === true) {
				this.$recentItems.append(tweetLines);

				// if its not inifinite history and load more has been clicked max times, 
				// hide the button to disable paging in history
				if (this.options.maxPagesInHistory > 0 &&
					this.historyClicks >= this.options.maxPagesInHistory) {
					this.toggleHistory(false);
				}
			}
			else {
				this.$recentItems.prepend(tweetLines);
			}


			// update timestamp
			this.$recentItems.find(this.selectors.timeStamp).timeago(null, true);

			if (this.options.ronBars) {
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
			$dj.error(this.name, "::_getWebIntentUrl: action '", action, "' is not recognized.");
			return;
		}

		return this.options.webIntents[action] + id;
	},


	_getInterfaceLanguage: function () {
		var lang = 'en'; // default to english
		try {
			lang = $dj.globalHeaders.preferences.interfaceLanguage;
		}
		catch (ex) {
			$dj.warn(this.name, "::_getInterfaceLanguage: Failed to get interface langauge. Defaulting to english.\nDetails:", ex);
		}

		return lang;
	},


	_openWebIntent: function (action, id) {
		var options = 'scrollbars=yes, resizable=yes, toolbar=no, location=yes, width=550, height=420';
		return window.open(this._getWebIntentUrl(action, id), 'intent', options);
	},


	_recordODSData: function (ODSdata) {
		var fields = [
            { name: "FCS_OD_ModuleAction", value: ODSdata.action },
			{ name: "FCS_OD_ModuleSection", value: "RecentTweets" },
            { name: "FCS_OD_ODSTranName", value: "SocialMediaUsage" }
        ];

		if (ODSdata.twitterId) {
			fields.push({ name: "FCS_OD_TwitterName", value: ODSdata.twitterName });
			fields.push({ name: "FCS_OD_TwittererHandle", value: ODSdata.twitterHandle });
			fields.push({ name: "FCS_OD_TwittererId", value: ODSdata.twitterId });
		}

		if (ODSdata.expandedUrl) {
			fields.push({ name: "FCS_OD_ExpandedURL", value: ODSdata.expandedUrl });
		}

		if (ODSdata.tagValue) {
			fields.push({ name: "FCS_OD_TagValue", value: ODSdata.tagValue });
		}

		$dj.recordODSData(fields);
	}

});

// Declare this class as a jQuery plugin
$.plugin('dj_TweetLines', DJ.UI.TweetLines);
