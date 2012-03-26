/*!
* RealtimeHeadlineList
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

/// <reference name="MicrosoftAjaxTimer.debug.js" />
/// <reference name="MicrosoftAjaxWebForms.debug.js" />
/// <reference name="AjaxControlToolkit.ExtenderBase.BaseScripts.js" assembly="AjaxControlToolkit" />

(function ($) {
	DJ.UI.RealtimeHeadlineList = DJ.UI.Component.extend({

		/*
		* Properties
		*/

		// Default options
		defaults: {
			debug: false,
			cssClass: 'RealtimeHeadlineList'
			// ,name: value     // add more defaults here separated by comma
		},

		// Localization/Templating tokens
		tokens: {
			controlTitleTkn: '{controlTitleTkn}',
			queueTkn: '{queueTkn}',
			viewAllTkn: '{viewAllTkn}'
		},


		/*
		* Initialization (constructor)
		*/
		init: function (element, meta) {
			var $meta = $.extend({ name: "RealtimeHeadlineList" }, meta);

			// Call the base constructor
			this._super(element, $meta);

			// TODO: Add custom initialization code like the following:
			// this._testButton = $('.testButton', element).get(0);
			//properties
			this._settings = {};
			this._result = {};
			this._alertContext = null;
			this._maxHeadlinesToReturn = 10;
			this._dateTimeFormatingPreference = "";
			this._clockType = null;
			this._rtheadlineListServiceUrl = null;
			this._interfaceLanguage = "en";
			this._productPrefix = null;
			this._accessPointCode = null;
			this.rthlReqObj = {
				"AlertContext": null,
				"MaxHeadlinesToReturn": 10,
				"DateTimeFormatingPreference": "",
				"ClockType": ""
			};
			this.retry = 0;
			this.maxRetry = 5;
			this.dataTimer = null;
			this.displayTimer = null;
			this.displayQueue = [];
			this.dataQueue = [];
			this._element = element;

			//Events
			this.fireOnViewAllLinkClick = $dj.delegate(this, this.fire_OnViewAllLinkClick);

			//Delegates
			this.$delegate$getProcessDataOnSuccess = $dj.delegate(this, this.getProcessDataOnSuccess);
			this.$delegate$getProcessDataOnFailure = $dj.delegate(this, this.getProcessDataOnFailure);
			this.$delegate$getData = $dj.delegate(this, this.getRealtimeHeadlineData);
			this.$delegate$startDisplayTimer = $dj.delegate(this, this.startDisplayTimer);
			this.$delegate$stopDisplayTimer = $dj.delegate(this, this.stopDisplayTimer);
			this.$delegate$refreshDisplayTimer = $dj.delegate(this, this.refreshDisplayTimer);
			this.$delegate$listInitCallback = $dj.delegate(this, this.listInitCallback);
			this.$delegate$getHeadlineObjectByreference = $dj.delegate(this, this.getHeadlineObjectByreference);
			//Initialize the control
			this.initialize();
		},


		/*
		* Public methods
		*/
		initialize: function () {
			if (this.data !== null) {
				this._result = this.data.resultSet;
				this._alertContext = this.options.alertContext;
				this._maxHeadlinesToReturn = this.options.maxHeadlinesToReturn;
				this._dateTimeFormatingPreference = this.options.dateTimeFormatingPreference;
				this._clockType = this.options.clockType;
				var timeStampArr = $(this._element).find('div.headline-timestamp');
				//Apply the jquery timeago plugin                
				//                $.each(timeStampArr, function () {
				//                    var timeStamp = this.innerHTML;
				//                    this.innerHTML = $.timeago(timeStamp);
				//                });

				//Initialize Realtime Headline Here
				this.initializeRealtimeHeadline(this._alertContext, this._maxHeadlinesToReturn, this._dateTimeFormatingPreference, this._clockType);
				this.dataTimer = $dj.timer(10000, this.$delegate$getData);
				this.displayTimer = $dj.timer(10000 / 4, this.$delegate$getData);

				//Push the first set of headlines into the queue
				for (var i = this.data.resultSet.count.Value; i > 0; i--) {
					this.dataQueue.push(this.data.resultSet.headlines[i - 1]);
				}

				//                var listContainer = $(this._element).find('#nContainer');
				//                $(listContainer).jcarousel({
				//                    vertical: true,
				//                    initCallback: this.$delegate$listInitCallback,
				//                    buttonNextHTML: null,
				//                    buttonPrevHTML: null
				//                });

				var me = this;
				var $container = $(this._element);
				$('.dj_emg_rtheadline_entry').each(function () {
					$(this).click(function (e) {
						var dataObj = me.getHeadlineObjectByreference($(this).attr('ref'));
						$container.triggerHandler('dj.RealtimeHeadlineList.HeadlineClick', { sender: this, data: dataObj });
						e.stopPropagation();
						return false;
					});
				});
			}
		},

		getHeadlineObjectByreference: function (reference) {
			for (var i = 0, len = this.dataQueue.length; i < len; i++) {
				if (this.dataQueue[i].reference.guid === reference) {
					return this.dataQueue[i];
				}
			}
		},

		listInitCallback: function (carousel) {
			var jcarousel_next = $(this._element).find('.next').get(0);
			var jcarousel_prev = $(this._element).find('.previous').get(0);
			$(jcarousel_next).bind('click', function () {
				carousel.next();
				return false;
			});

			$(jcarousel_prev).bind('click', function () {
				carousel.prev();
				return false;
			});
		},

		getRealtimeHeadlineData: function () {
			var requestDelegate = {
				'requestDelegate': this.rthlReqObj,
				'interfaceLanguage': this._interfaceLanguage,
				'productPrefix': this._productPrefix,
				'accessPointCode': this._accessPointCode
			};
			$.ajax({
				type: "POST",
				url: this.options.RealtimeHeadlineListServiceUrl + "/ProcessRealtimeHeadline",
				data: JSON.stringify(requestDelegate),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: this.$delegate$getProcessDataOnSuccess,
				error: this.$delegate$getProcessDataOnFailure
			});
		},

		updateDisplay: function () {
			if (this.displayTimer._isStopped) {
				return;
			}
			// process items of of the data queue
			if (this.displayQueue.length > 0) {
				this.displayTimer.start();
			}
		},

		//update queue status
		updateFooterInfo: function () {
			var queueCount = $(this._element).find(".dj_emg_rt_queueCount").get(0);
			queueCount.innerHTML = this.displayQueue.length;
		},

		//Start timer to 
		startDisplayTimer: function () {
			var me = this;
			var $container = $(this._element);
			var rthl_play = $(this._element).find(".play_blue").get(0);
			var rthl_pause = $(this._element).find(".pause").get(0);
			var rthl_stop_status = $(this._element).find(".stop-status").get(0);
			$(rthl_play).removeClass("play_blue")
                    .addClass("play");
			$(rthl_pause).removeClass("pause")
                     .addClass("pause_blue");
			$(rthl_stop_status).removeClass("stop-status")
                     .addClass("start-status");
			var rtheadlineUl = $(this._element).find(".listContainer");
			if (this.displayQueue.length > 0) {
				for (var i = 0; i < this.displayQueue.length; i++) {
					var rtheadLine = this.displayQueue[i];
					var tLi = $("<li class=\"" + rtheadLine.reference.guid + " dj_emg_rtheadline_entry\" ref=\"" + rtheadLine.reference.guid + "\"><div class=\"headline-container\"><div class=\"headline-timestamp\">" + rtheadLine.publicationDateTimeDescriptor + "</div><div class=\"headline\"><a class=\"dj_emg_rt_headlineTitle\" href=\"javascript:void(0)\"><div class=\"text\"><span class=\"ellipsis_text\">" + rtheadLine.title[0].items[0].value + "</div></span><span class=\"dj_emg_space\"> </span></a></div></div></li>");

					//Prepend the headline to the Ul
					if (this.displayQueue.length === 1) {
						$(tLi).prependTo(rtheadlineUl).slideDown("slow");
					}
					else {
						$(tLi).prependTo(rtheadlineUl);
					}
					//Clip the long text and show the tooltip
					this.showTooltip(false, tLi);
					$(tLi).click(function (e) {
						var dataObj = me.getHeadlineObjectByreference($(this).attr('ref'));
						$container.triggerHandler('dj.RealtimeHeadlineList.HeadlineClick', { sender: this, data: dataObj });
						e.stopPropagation();
						return false;
					});
				}
			}

			this.displayQueue.length = 0;
			//set the isStopped flag
			this.displayTimer._isStopped = false;

			//Update footer Info
			this.updateFooterInfo();
		},

		//Stop timer to move the headlines into queue
		stopDisplayTimer: function () {
			var rthl_play = $(this._element).find(".play").get(0);
			var rthl_pause = $(this._element).find(".pause_blue").get(0);
			var rthl_start_status = $(this._element).find(".start-status").get(0);
			$(rthl_pause).removeClass("pause_blue")
                     .addClass("pause");
			$(rthl_play).removeClass("play")
                    .addClass("play_blue");
			$(rthl_start_status).removeClass("start-status")
                     .addClass("stop-status");
			//set the isStopped flag
			this.displayTimer._isStopped = true;

			//Update footer Info
			this.updateFooterInfo();
		},

		//Refreah headlines to display which are in queue
		refreshDisplayTimer: function () {
			var me = this;
			var $container = $(this._element);
			var rtheadlineUl = $(this._element).find(".listContainer");
			if (this.displayQueue.length > 0) {
				for (var i = 0; i < this.displayQueue.length; i++) {
					var rtheadLine = this.displayQueue[i];
					var tLi = $("<li class=\"" + rtheadLine.reference.guid + " dj_emg_rtheadline_entry\" ref=\"" + rtheadLine.reference.guid + "\"><div class=\"headline-container\"><div class=\"headline-timestamp\">" + rtheadLine.publicationDateTimeDescriptor + "</div><div class=\"headline\"><a class=\"dj_emg_rt_headlineTitle\" href=\"javascript:void(0)\"><div class=\"text\"><span class=\"ellipsis_text\">" + rtheadLine.title[0].items[0].value + "</div></span><span class=\"dj_emg_space\"> </span></a></div></div></li>");
					//Prepend the headline to the Ul
					if (this.displayQueue.length === 1) {
						$(tLi).prependTo(rtheadlineUl).slideDown("slow");
					}
					else {
						$(tLi).prependTo(rtheadlineUl);
					}
					//Clip the long text and show the tooltip
					this.showTooltip(false, tLi);
					$(tLi).click(function (e) {
						var dataObj = me.getHeadlineObjectByreference($(this).attr('ref'));
						$container.triggerHandler('dj.RealtimeHeadlineList.HeadlineClick', { sender: this, data: dataObj });
						e.stopPropagation();
						return false;
					});
				}
			}
			this.displayQueue.length = 0;
			//Update footer Info
			this.updateFooterInfo();
		},

		//Show tooltip
		showTooltip: function (isFirstSet, rthl) {
			// Find available li.dj_emg_rtheadline_entry , control
			if (isFirstSet === true) {
				var rthlContainer = $(this._element).find("div.dj_emg_rt_headlines").get(0);
				var tLis = $("li.dj_emg_rtheadline_entry", rthlContainer);
				for (var i = 0; i < tLis.length; i++) {
					var headlineTextSpan = $("div.text", tLis[i]).get(0);
					var headlineTitleSpan = $("span.ellipsis_text", tLis[i]).get(0);

					// Apply the ellipse plugin
					$(headlineTextSpan).ThreeDots({ max_rows: 1 });
					if ($(headlineTextSpan).data("threedots") !== $(headlineTitleSpan).text()) {
						// Appply the simple tooltip plugin
						$(headlineTextSpan).attr("title", "<div>" + $(headlineTextSpan).data("threedots") + "</div>");
						//TODO: Tooltip
						$(headlineTextSpan).dj_simpleTooltip("tooltip");
					}
				}
			}
			else {
				var headlineTextSpan = $("div.text", rthl).get(0);
				var headlineTitleSpan = $("span.ellipsis_text", rthl).get(0);

				// Apply the ellipse plugin
				$(headlineTextSpan).ThreeDots({ max_rows: 1 });

				if ($(headlineTextSpan).data("threedots") !== $(headlineTitleSpan).text()) {
					// Appply the simple tooltip plugin
					$(headlineTextSpan).attr("title", "<div>" + $(headlineTextSpan).data("threedots") + "</div>");
					//TODO: Tooltip
					$(headlineTextSpan).dj_simpleTooltip("tooltip");
				}

				//New headline fadeout effect
				//$(rthl).effect('highlight', { color: "#F7B835" }, 5000);
			}
		},

		//Remove an array element by value
		removeArrayElementByValue: function (arr, arrElement) {
			for (var i = 0; i < arr.length; i++) {
				if (arr[i].reference.guid === arrElement)
					arr.splice(i, 1);
			}
		},

		getProcessDataOnSuccess: function (data, userContext, methodName) {
			var me = this;
			var $container = $(this._element);
			var result = data.d;
			if (result.ReturnCode === 0) {
				var rtheadlineUl = $(this._element).find(".listContainer");
				var rtheadLinesCount = result.headlineListDataResult.resultSet.count.Value;
				this.retry = 0;
				if (rtheadLinesCount > 0) {
					this.updateDisplay();
					//for (var i = 0; i < rtheadLinesCount; i++) {
					for (var i = rtheadLinesCount; i > 0; i--) {
						var rtheadLine = result.headlineListDataResult.resultSet.headlines[i - 1];
						if (!this.displayTimer._isStopped) {
							var tLi = $("<li class=\"" + rtheadLine.reference.guid + " dj_emg_rtheadline_entry\" ref=\"" + rtheadLine.reference.guid + "\"><div class=\"headline-container\"><div class=\"headline-timestamp\">" + rtheadLine.publicationDateTimeDescriptor + "</div><div class=\"headline\"><a class=\"dj_emg_rt_headlineTitle\" href=\"javascript:void(0)\"><div class=\"text\"><span class=\"ellipsis_text\">" + rtheadLine.title[0].items[0].value + "</span></div><span class=\"dj_emg_space\"> </span></a></div></div></li>");
							//Prepend the headline to the Ul
							if (rtheadLinesCount === 1) {
								$(tLi).prependTo(rtheadlineUl).slideDown("slow");
							}
							else {
								$(tLi).prependTo(rtheadlineUl);
							}

							//Clip the long text and show the tooltip
							this.showTooltip(false, tLi);
							$(tLi).click(function (e) {
								var dataObj = me.getHeadlineObjectByreference($(this).attr('ref'));
								$container.triggerHandler('dj.RealtimeHeadlineList.HeadlineClick', { sender: this, data: dataObj });
								e.stopPropagation();
								return false;
							});
						}
						else {
							this.displayQueue.push(rtheadLine);
							this.updateFooterInfo();
						}
						this.dataQueue.push(rtheadLine);
					}

					//Check if length of the queue > 100
					//If > 100 delete the older headline list items
					var maxheadlinesOnPage = 100;
					var headlinesToRemove = this.dataQueue.length - maxheadlinesOnPage;
					if (this.dataQueue.length > maxheadlinesOnPage) {
						for (var d = 0; d < headlinesToRemove; d++) {
							var liClass = this.dataQueue[0].reference.guid;
							var dirtyli = $(rtheadlineUl).find('li.' + liClass).get(0);
							$(dirtyli).remove();
							this.removeArrayElementByValue(this.dataQueue, liClass);
						}
					}
				}

				//start the timer (webservice call)
				this.dataTimer.start();
			}
			else {
				if (this.retry < this.maxRetry) {
					this.dataTimer.start();
					this.retry++;
					return;
				}
				// alert eror
			}
		},

		getProcessDataOnFailure: function (result, userContext, methodName) {
			//TODO: Error handling
		},

		initializeRealtimeHeadline: function (alertContext, maxHeadlinesToReturn, dateTimeFormatingPreference, clockType) {
			this.rthlReqObj.AlertContext = alertContext;
			this.rthlReqObj.MaxHeadlinesToReturn = maxHeadlinesToReturn;
			this.rthlReqObj.DateTimeFormatingPreference = dateTimeFormatingPreference;
			this.rthlReqObj.ClockType = clockType;

			//Clip the long text and show the tooltip
			this.showTooltip(true, null);

			//call the service GetShareAlert Service
			this.getRealtimeHeadlineData();

			//Initialize scrolling
			//this.initScroll();

			//click events
			var rthl_play = $(this._element).find(".play").get(0);
			var rthl_pause = $(this._element).find(".pause_blue").get(0);
			var rt_refresh = $(this._element).find(".refresh").get(0);

			//play click function
			$(rthl_play).click($dj.delegate(this, this.$delegate$startDisplayTimer));

			//pause click function
			$(rthl_pause).click($dj.delegate(this, this.$delegate$stopDisplayTimer));

			//refresh click function
			$(rt_refresh).click($dj.delegate(this, this.$delegate$refreshDisplayTimer));
		},

		//Dispose all the variables here
		dispose: function () {
			this._super();
			// properties
			this._settings = null;
			this._scroller = null;
			this._rtheadlineListServiceUrl = null;

			// events
			$clearHandlers(this.get_element());
			$RealtimeHeadlineList.callBaseMethod(this, 'dispose');
		},

		// TODO: Public Methods here


		/*
		* Private methods
		*/

		// DEMO: Overriding the base _paint method:
		_paint: function () {
			//this.initialize();
			// "this._super()" is available in all overridden methods
			// and refers to the base method.
			this._super();

			//alert('TODO: implement RealtimeHeadlineList._paint!');
		}
	});


	// Declare this class as a jQuery plugin
	$.plugin('dj_RealtimeHeadlineList', DJ.UI.RealtimeHeadlineList);

})(jQuery);
