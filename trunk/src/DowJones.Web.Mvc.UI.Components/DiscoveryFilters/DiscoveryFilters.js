/*!
* DiscoveryFilters
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

"use strict";

DJ.UI.DiscoveryFilters = DJ.UI.Component.extend({

	// Default options
	defaults: {
		debug: false,
		cssClass: 'DiscoveryFilters',
		sortable: true,
		fontFamily: 'Arial, sans-serif',
		fontSize: '9px',
		roundHitCount: true,
		truncationLength: 0,
		isColumnChartPointWidthSpecified: false,
		columnChartPointWidth: 0,
		barChartPointWidth: 5,
		barChartDataLabelWidth: '100px',
		barColor: '#5BB4E5',
		columnColor: '#5BB4E5',
		enableExcludeItems: true,
		useHTML: false
		// ,name: value     // add more defaults here separated by comma
	},

	//Events
	events: {
		itemClick: 'itemClick.dj.DiscoveryFilters',
		exportClick: 'exportClick.dj.DiscoveryFilters',
		expandCollapseClick: 'expandCollapseClick.dj.DiscoveryFilters',
		itemExcludeClick: 'itemExcludeClick.dj.DiscoveryFilters'
	},

	//Selectors
	selectors: {
		discoveryFiltersList: '.dj_discoveryFilters-list',
		chartContainer: '.dj_hc-container',
		exportLink: '.dj_df-header .fi_export',
		expandCollapseBtn: '.dj_df-header .side-section-title',
		headerContainer: '.dj_df-header',
		dataContainer: '.dj_df-chart',
		excludeItem: '#excludeItem',
		mainContainer: '.dj_df-div'
	},

	// Localization/Templating tokens
	tokens: {
		expandTkn: "<%= Token('expand') %>",
		collapseTkn: "<%= Token('collapse') %>",
		exportTkn: "<%= Token('export') %>",
		distributionTkn: "<%= Token('distribution') %>",
		dateTkn: "<%= Token('date') %>",
		startDateTkn: "<%= Token('startDate') %>",
		endDateTkn: "<%= Token('endDate') %>",
		yearlyTkn: "<%= Token('yearly') %>",
		monthlyTkn: "<%= Token('monthly') %>",
		weeklyTkn: "<%= Token('weekly') %>",
		dailyTkn: "<%= Token('daily') %>",
		numHitsTkn: "<%= Token('numHits') %>",
		noResultsTkn: "<%= Token('noResults') %>",
		moduleDragTkn: "<%= Token('moduleDrag') %>"
	},

	periodType: {
		DateYearly: 15,
		DateMonthly: 16,
		DateWeekly: 17,
		DateDaily: 18
	},

	/*
	* Initialization (constructor)
	*/
	init: function (element, meta) {
		var $meta = $.extend({ name: "DiscoveryFilters" }, meta);

		// Call the base constructor
		this._super(element, $meta);

		this._setData(this.data);

		//Initialize Sortable
		this._initializeSortable();
	},

	/*
	* Public methods
	*/

	_setData: function (data) {
		if (data && data.discovery) {
			this._bindOnSuccess(data.discovery);
			return;
		}
		this._bindOnNoData();
	},

	_bindOnNoData: function () {
		this.templates.nodata(); //TODO
		return;
	},

	// Bind the data to the component on Success
	_bindOnSuccess: function (data) {
		//this.discoveryFiltersConfig = this._getDiscoveryFiltersConfig();
		//this.discoveryFiltersDateChartConfig = this._getDiscoveryFiltersDateChartConfig();

		var discoveryDataArr = this._sortDiscoveryFilters(data);

		var discoveryFiltersMarkUp = this.templates.success(data);
		$(this.$element).html(discoveryFiltersMarkUp);

		// bind events and perform other wiring up
		this._initializeDiscoveryFilters(discoveryDataArr);
	},

	/*
	* Private methods
	*/

	_initializeEventHandlers: function () {
		this.$element.on('click', this.selectors.exportLink, this._delegates.OnExportClicked);
		this.$element.on('click', this.selectors.expandCollapseBtn, this._delegates.OnExpandCollapseClicked);
		//		this.$element.on('click', this.selectors.expandCollapseBtn, function () {
		//			$(this).parent().next('.dj_df-chart').slideToggle('fast');
		//			$(this).parent().toggleClass('open');
		//		});
		var self = this;
		$("." + this.options.cssClass).on("mouseleave", function (event) {
			$(self.selectors.excludeItem).hide();
			$(self.selectors.excludeItem).data("entityData", "");
		});
		this.$element.on('click', this.selectors.excludeItem, this._delegates.OnItemExcludeClicked);
	},

	//Initialize Sortable
	_initializeSortable: function () {
		if (this.options.sortable) {
			$(this.selectors.discoveryFiltersList).sortable({
				placeholder: "ui-state-highlight"
			}).disableSelection();
		}
	},

	//Initialize Delegates
	_initializeDelegates: function () {
		$.extend(this._delegates, {
			OnItemClicked: $dj.delegate(this, this._onItemClicked),
			OnExportClicked: $dj.delegate(this, this._onExportClicked),
			OnItemExcludeClicked: $dj.delegate(this, this._onItemExcludeClicked),
			OnExpandCollapseClicked: $dj.delegate(this, this._onExpandCollapseClicked)
		});
	},

	_sortDiscoveryFilters: function (discoveryEntityObj) {
		var self = this,
			discoveryDataArr = [];
		//Build the Discovery Entity Lists
		_.each(discoveryEntityObj, function (entitiesObj) {
			var discoveryData = {};
			if (entitiesObj != null) {
				var categoryArr = [],
				seriesDataArr = [],
				discoveryEntityObjArr = entitiesObj.newsEntities;

				if (discoveryEntityObjArr.length > 0) {
					//Construct the graph for the discovery entity objects
					_.each(discoveryEntityObjArr, function (entity) {
						var dataObj = {
							y: entity.currentTimeFrameNewsVolume.value,
							entityData: entity
						};
						seriesDataArr.push(dataObj);
						//Date chart, daily, weekly, monthly and yearly
						if (_.indexOf([self.periodType.DateDaily, self.periodType.DateWeekly, self.periodType.DateMonthly, self.periodType.DateYearly], entitiesObj.type) >= 0) {   //Date Navigator
							categoryArr.push([entity.startDateFormattedString, entity.endDateFormattedString]);
						} else {
							categoryArr.push(entity.descriptor);
						}

					});
				}
				discoveryData.title = entitiesObj.title;
				discoveryData.position = entitiesObj.position;
				discoveryData.categories = categoryArr;
				discoveryData.seriesData = seriesDataArr;
				discoveryData.entityType = entitiesObj.type;
				discoveryData.isExpanded = entitiesObj.isExpanded;

				var chartTitle = "";
				switch (entitiesObj.type) {
					case self.periodType.DateYearly:
						chartTitle = self.tokens.yearlyTkn;
						break;
					case self.periodType.DateMonthly:
						chartTitle = self.tokens.monthlyTkn;
						break;
					case self.periodType.DateWeekly:
						chartTitle = self.tokens.weeklyTkn;
						break;
					case self.periodType.DateDaily:
						chartTitle = self.tokens.dailyTkn;
						break;
				}
				if (chartTitle.length > 0) {
					discoveryData.chartTitle = chartTitle;
				}
				discoveryDataArr.push(discoveryData);
			}
		});

		//Sort by position
		//TBD: _sort
		discoveryDataArr.sort(function (a, b) {
			return a.position > b.position;
		});
		return discoveryDataArr;
	},

	//Initialize Discovery Filters
	_initializeDiscoveryFilters: function (discoveryDataArr) {
		var self = this,
			discoveryLi;

		//Render Discovery Filters after sorting
		_.each(discoveryDataArr, function (dd) {
			//Expand Collapse the div based on the isExpanded property
			var container = self.$element.find('.dj_discoveryFilters-listItem-' + dd.position);
			if (!dd.isExpanded) {
				var header = $(self.selectors.headerContainer, container);
				header.addClass('open');
				var dataContainer = $(self.selectors.dataContainer, container);
				dataContainer.hide();
			}
			discoveryLi = self.$element.find('.dj_discoveryFilters-listItem-' + dd.position);

			if (_.indexOf([self.periodType.DateDaily, self.periodType.DateWeekly, self.periodType.DateMonthly, self.periodType.DateYearly], dd.entityType) >= 0) {   // Date Navigator
				$(self.selectors.chartContainer, discoveryLi).css('height', 160 + ''); //Fix height for column chart
				self._renderDiscoveryDateFilters(dd, dd.position);
			} else {
				if (dd.seriesData.length == 0) { //no result for the navigator
					$(self.selectors.chartContainer, discoveryLi).css('height', 20 + '');  //The height for chat is fix if no result for the navigator
				} else {
					$(self.selectors.chartContainer, discoveryLi).css('height', 27 * (dd.seriesData.length) + 20 + '');  //calucate the height for bar chart, height is depended on the num of items
				}
				self._renderDiscoveryFilters(dd, dd.position);
			}
		});

	},

	//On Discovery Item Click Event Handler
	_onItemClicked: function (evt) {
		var self = this;
		self.publish(self.events.itemClick, { "data": evt.point.options.entityData });
	},

	//On Discovery Item Click Event Handler
	_onItemExcludeClicked: function (evt) {
		var self = this;
		var excludeItem = $(self.selectors.excludeItem);
		self.publish(self.events.itemExcludeClick, { "data": excludeItem.data("entityData") });
	},

	//On Discovery Filters Export Click
	_onExportClicked: function (evt) {
		var self = this;
		self.publish(self.events.exportClick, { "data": "test export" });
	},

	_onExpandCollapseClicked: function (evt) {
		var self = this;
		$(evt.target).parent().next('.dj_df-chart').slideToggle('fast');
		$(evt.target).parent().toggleClass('open');
		//Find the entity
		var type = $(evt.target).closest("li").data("type");
		var expandCollapseData = { type: type, expand: ($(evt.target).parent().hasClass("open")) ? false : true };
		self.publish(self.events.expandCollapseClick, { "data": expandCollapseData });
	},

	//Get Discovery Filter Config
	_getDiscoveryFiltersConfig: function () {
		var self = this;
		//BEGIN: Discovery Filters Configuration
		return {
			chart: {
				defaultSeriesType: 'bar'
			},
			title: { text: null },
			subtitle: { text: null },
			xAxis: {
				lineWidth: 0,
				labels: {
					align: 'left',
					x: 1,
					y: 12,
					formatter: function () {
//						if (self.options.truncationLength > 0 && this.value.length > self.options.truncationLength) {
//							return this.value.substring(0, self.options.truncationLength - 3) + "..."
//						} else {
//							return this.value
//						}
                        var truncatedLabel = this.value;
                        if (self.options.truncationLength > 0 && this.value.length > self.options.truncationLength) {
							truncatedLabel = this.value.substring(0, self.options.truncationLength - 3) + "...";
						}
						return "<div title='" + this.value + "'>" + truncatedLabel + "</div>";
					},
                    useHTML: true,
					style: {
						width: self.options.barChartDataLabelWidth
					}
				},
				tickWidth: 0,
				title: { text: null },
				gridLineColor: "#ffffff"
			},
			yAxis: {
				labels: { enabled: false },
				gridLineWidth: 0,
				min: 0,
				title: { text: null }
			},
			plotOptions: {
				series: {
					pointWidth: self.options.barChartPointWidth
				},
				bar: {
					shadow: false,
					cursor: 'pointer',
					point: { events: { click: self._delegates.OnItemClicked} },
					stacking: "percent",
					minPointLength: 2
				}
			},
			legend: { enabled: false },
			credits: { enabled: false },
			exporting: { enabled: false }
		};
		//END: Discovery Filters Configuration
	},

	//Get Discovery Filter Date Chart Config
	_getDiscoveryFiltersDateChartConfig: function () {
		//BEGIN: Discovery Filters Configuration
		var self = this;
		return {
			chart: {
				defaultSeriesType: "column",
				height: 160,
				marginRight: 27
			},
			credits: { enabled: false },
			title: {
				text: null
			},
			subtitle: {
				text: null
			},
			xAxis: {
				gridLineColor: "#ffffff",
				labels: {
					style: {
						color: "#003366",
						fontFamily: self.options.fontFamily,
						fontSize: self.options.fontSize,
						fontWeight: "normal",
						width: "180px"
					},
					y: 20,
					formatter: function () {
						if (this.isFirst)
							return this.value[0];
						if (this.isLast)
							return this.value[1];
					}
				},
				lineWidth: 0,
				maxPadding: 0,
				minPadding: 0,
				startOnTick: false,
				tickWidth: 0,
				title: {
					style: {
						fontFamily: self.options.fontFamily,
						fontSize: self.options.fontSize,
						fontWeight: "normal"
					},
					margin: 20
				}
			},
			yAxis: {
				gridLineWidth: 1,
				labels: {
					style: {
						color: "#003366",
						fontFamily: self.options.fontFamily,
						fontSize: self.options.fontSize,
						fontWeight: "normal"
					}
				},
				lineWidth: 2,
				min: 0,
				plotLines: [{
					color: "#808080",
					value: 0,
					width: 1
				}],
				startOnTick: false,
				tickPixelInterval: 65,
				title: {
					text: ""
				}
			},
			legend: {
				enabled: false
			},
			plotOptions: {
				column: {
					color: self.options.columnColor,
					shadow: false,
					cursor: 'pointer',
					point: { events: { click: self._delegates.OnItemClicked} }
				},
				series: {
					borderWidth: 1,
					groupPadding: 0,
					minPointLength: 3,
					pointPadding: 0
				}
			}
		};
		//END: Discovery Filters Date Chart Configuration
	},

	_extractDateSeriesData: function (seriesData) {
		var dateDataArr = [];
		_.each(seriesData, function (obj) {
			//Construct original series array
			var displayVal = obj.entityData.currentTimeFrameNewsVolume.displayText.value;
			dateDataArr.push({
				name: displayVal,
				y: obj.y,
				entityData: obj.entityData
			});
		});
		return dateDataArr;
	},

	_extractSeriesData: function (seriesData) {
		var returnData = [[], []];
		if (typeof (seriesData) !== 'undefined') {
			for (var i = 0; i < seriesData.length; i++) {

				returnData[0][i] = { y: (seriesData[0].y - seriesData[i].y), name: ((this.options.roundHitCount) ? seriesData[i].entityData.currentTimeFrameNewsVolume.displayText.value : seriesData[i].y) };
				returnData[1][i] = { y: seriesData[i].y, entityData: seriesData[i].entityData };
			}
		}
		return returnData;
	},


	//Render Discovery Filters Chart
	_renderDiscoveryFilters: function (discoveryData, idx) {
		var self = this;
		var chartContainer = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.dj_hc-container');
		var mainContainer = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find(this.selectors.mainContainer);
		var chartTitle = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.side-section-title');
		chartTitle.append(discoveryData.title);
		this.discoveryFiltersConfig = this._getDiscoveryFiltersConfig();
		var seriesData = this._extractSeriesData(discoveryData.seriesData);
		if (seriesData[0].length == 0) {
			chartContainer.html(self.tokens.noResultsTkn);
			return;
		}
		if (self.options.enableExcludeItems) {
			this.discoveryFiltersConfig.chart.marginLeft = 15;
			this.discoveryFiltersConfig.plotOptions.series.point = {
				events:
				{
					mouseOver: function (event) {
						var excludeItem = $(self.selectors.excludeItem);
						excludeItem.data("entityData", event.target.entityData);
						excludeItem.css({
							'top': chartContainer[0].offsetTop + event.target.clientX + 14
						});
						excludeItem.css({
							'left': '5px'
						});
						excludeItem.show();
					}
				}
			}

		}
		return new Highcharts.Chart($.extend(true, {}, this.discoveryFiltersConfig, {
			chart: {
				renderTo: chartContainer[0],
				width: $(mainContainer[0]).width()
			},
			xAxis: {
				categories: discoveryData.categories
			},

			tooltip: {
			    //enable: false
                
				formatter: function () {
					//return this.x;
					return "<div style='white-space:normal;max-width:140px;'>" + this.x + "</div>";
				},

				style: {
					color: '#333333',
					fontSize: self.options.fontSize,
					padding: '2px'
				},
	            borderWidth: 2,
                shadow: true,
				
				useHTML: true
				//headerFormat: '<table><tr><td style="white-space:normal;max-width:160px;">{point.key}<td><tr></table>',
				//pointFormat: '',
				//footerFormat: ''
			},
			
			series: [{
				name: "faux",
				color: "#ffffff",
				pointWidth: self.options.barChartPointWidth,
				dataLabels: {
					enabled: true,
					align: "right",
					y: 12,
					color: "#a6a6a6",
					formatter: function () {
						return this.point.name;
					},
					padding: 6
				},
				data: seriesData[0]
			}, {
				name: "data",
				dataLabels: {
					"enabled": false
				},
				pointWidth: self.options.barChartPointWidth,
				color: self.options.barColor,
				data: seriesData[1]
			}]
		}));
	},

	//Render Discovery Filters Date Chart
	_renderDiscoveryDateFilters: function (discoveryData, idx) {
		var self = this;
		var chartContainer = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.dj_hc-container');
		var mainContainer = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find(this.selectors.mainContainer);
		var chartTitle = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.side-section-title');
		chartTitle.append(discoveryData.title);
		this.discoveryFiltersDateChartConfig = this._getDiscoveryFiltersDateChartConfig();

		var seriesData = this._extractDateSeriesData(discoveryData.seriesData);
		if (self.options.isColumnChartPointWidthSpecified) {
			this.discoveryFiltersDateChartConfig.plotOptions.series.pointWidth = self.options.columnChartPointWidth; //This is to ensure that the columns are not too wide when there are less columns
		}
		return new Highcharts.Chart($.extend(true, {}, this.discoveryFiltersDateChartConfig, {
			chart: {
				renderTo: chartContainer[0],
				width: $(mainContainer[0]).width()
			},
			xAxis: {
				categories: discoveryData.categories,
				title: {
					"text": self.tokens.distributionTkn + ': ' + discoveryData.chartTitle
				}
			},
			tooltip: {
				formatter: function () {
					var tooltipStr;
					if (discoveryData.chartTitle == self.tokens.dailyTkn) {
						tooltipStr = self.tokens.dateTkn + ': ' + this.x[0];
					} else {
						tooltipStr = self.tokens.startDateTkn + ': ' + this.x[0] + '<br/>' + self.tokens.endDateTkn + ': ' + this.x[1];
					}

					return tooltipStr + '<br/>' + self.tokens.numHitsTkn + ': ' + ((self.options.roundHitCount) ? this.point.name : Highcharts.numberFormat(this.y, 0));

				},
				style: {
					color: '#333333',
					fontSize: self.options.fontSize
					//padding: '2px'
				}
			},
			series: [{
				data: seriesData
			}]
		}));
	}

});


// Declare this class as a jQuery plugin
$.plugin('dj_DiscoveryFilters', DJ.UI.DiscoveryFilters);

