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
		roundHitCount: true,
		truncationLength: 0
		// ,name: value     // add more defaults here separated by comma
	},

	//Events
	events: {
		itemClick: 'itemClick.dj.DiscoveryFilters',
		exportClick: 'exportClick.dj.DiscoveryFilters'
	},

	//Selectors
	selectors: {
		discoveryFiltersList: 'ul.dj_discoveryFilters-list',
		chartContainer: 'div.dj_hc-container',
		exportLink: 'span.dj_df-export a.export',
		expandBtn: 'a.dj_df-expand',
		collapseBtn: 'a.dj_df-collapse',
		expandDiv: 'div.cd_div_expand',
		collapseDiv: 'div.cd_div_collapse'
	},

	// Localization/Templating tokens
	tokens: {
		expandTkn: '<%= Token("expand") %>',
		collapseTkn: '<%= Token("collapse") %>',
		exportTkn: '<%= Token("export") %>',
		distributionTkn: '<%= Token("distribution") %>',
		dateTkn: '<%= Token("date") %>',
		startDateTkn: '<%= Token("startDate") %>',
		endDateTkn: '<%= Token("endDate") %>',
		yearlyTkn: '<%= Token("yearly") %>',
		monthlyTkn: '<%= Token("monthly") %>',
		weeklyTkn: '<%= Token("weekly") %>',
		dailyTkn: '<%= Token("daily") %>',
		numHitsTkn: '<%= Token("numHits") %>',
		noResultsTkn: '<%= Token("noResults") %>'
	},

	/*
	* Initialization (constructor)
	*/
	init: function (element, meta) {
		var $meta = $.extend({ name: "DiscoveryFilters" }, meta);

		// Call the base constructor
		this._super(element, $meta);

		this.discoveryFitlersConfig = this._getDiscoveryFiltersConfig();
		this.discoveryFitlersDateChartConfig = this._getDiscoveryFiltersDateChartConfig();
		if (this.data && this.data.discovery) {
			this.bindOnSuccess(this.data.discovery);
		}

		//Initialize Sortable
		this._initializeSortable();

		//Initialize Expand/Collapse
		this._initializeExpandCollapse();
	},

	/*
	* Public methods
	*/

	// Bind the data to the component on Success
	bindOnSuccess: function (data) {
		if (!data) {
			$dj.warn("bindOnSuccess:: called with empty data object");
			return;
		}

		var discoveryFiltersMarkUp = this.templates.success(data);
		$(this.$element).html(discoveryFiltersMarkUp);

		// bind events and perform other wiring up
		this._initializeDiscoveryFilters(data);
	},

	/*
	* Private methods
	*/

	_initializeElements: function (ctx) {

	},

	_initializeEventHandlers: function () {

	},

	//Initialize Sortable
	_initializeSortable: function () {
		if (this._isSortable) {
			$(this.selectors.discoveryFiltersList).sortable({
				placeholder: "ui-state-highlight"
			}).disableSelection();
		}
	},

	//Initialize Expand/Collapse Discovery
	_initializeExpandCollapse: function () {
		var self = this;
		$(this.selectors.collapseBtn).on('click', function () {
			self._expandCollapse(this, true); /*alert('Expanding!');*/return false;
		});
	},

	//Expand/Collapse function
	_expandCollapse: function (el, expand) {
		var self = this;
		var containerLi = $(el).closest('li');
		if (expand) {
			$(el).removeClass('dj_df-collapse').addClass('dj_df-expand').unbind('click').bind('click', function () {
				self._expandCollapse(this, false); return false;
			}).attr('title', self.tokens.collapseTkn);
			$(this.selectors.collapseDiv, containerLi).removeClass('cd_div_collapse').addClass('cd_div_expand');
			$(this.selectors.exportLink, containerLi).removeClass('hide').addClass('show');
		} else {
			$(el).removeClass('dj_df-expand').addClass('dj_df-collapse').unbind('click').bind('click', function () {
				self._expandCollapse(this, true); return false;
			}).attr('title', self.tokens.expandTkn); ;
			$(this.selectors.expandDiv, containerLi).removeClass('cd_div_expand').addClass('cd_div_collapse');
			$(this.selectors.exportLink, containerLi).removeClass('show').addClass('hide');
		}
	},

	//Check if sortable is enabled
	_isSortable: function () {
		return this.options.sortable;
	},

	//Check if hit count has to be rounded
	_isRoundHitCount: function () {
		return this.options.roundHitCount;
	},

	//Check the truncation length
	_truncationLength: function () {
		return this.options.truncationLength;
	},

	//Initialize Delegates
	_initializeDelegates: function () {
		$.extend(this._delegates, {
			OnItemClicked: $dj.delegate(this, this._onItemClicked),
			OnExportClicked: $dj.delegate(this.selectors.exportLink, 'click', this._onExportClicked)
		});
	},

	//Initialize Discovery Filters
	_initializeDiscoveryFilters: function (discoveryEntityObj) {
		var $this = this,
			discoveryDataArr = [],
			discoveryLi;

		//Build the Discovery Entity Lists
		_.each(discoveryEntityObj, function (entitiesObj) {
			var discoveryData = {};
			if (entitiesObj != null) {
				var categoryArr = [],
				seriesDataArr = [],
				discoveryEntityObjArr = entitiesObj.newsEntities;
				//Expand Collapse the div based on the isExpanded property
				/*
				if (entitiesObj.isExpanded) {
				var container = $this.$element.find('.dj_discoveryFilters-listItem-' + index);
				var collapseBtn = $($this.selectors.collapseBtn, container);
				$this._expandCollapse(collapseBtn, true);
				}
				*/

				//Set the height for each hc container based on no. of entities
				//var discoveryLi = $this.$element.find('.dj_discoveryFilters-listItem-' + index);
				//discoveryLi = $this.$element.find('.dj_discoveryFilters-listItem-' + index);

				//$($this.selectors.chartContainer, discoveryLi).css('height', 27 * (discoveryEntityObjArr.length) + '');

				if (discoveryEntityObjArr.length > 0) {
					//Construct the graph for the discovery entity objects
					//var idx = 0;
					_.each(discoveryEntityObjArr, function (entity) {
						var dataObj = {
							y: entity.currentTimeFrameNewsVolume.value,
							jsonObj: entity
						};
						seriesDataArr.push(dataObj);

						if (_.indexOf([15, 16, 17, 18], entitiesObj.type) >= 0) {   //Date Navigator
							categoryArr.push([entity.startDateFormattedString, entity.endDateFormattedString]);
						}
						else {
							if ($this.options.truncationLength > 0) {
								categoryArr.push([entity.descriptor.substring(0, $this.options.truncationLength - 3) + "...", entity.descriptor]);
							} else {
								categoryArr.push([entity.descriptor, entity.descriptor]);
							}
						}
						//idx++;
					});
				}
				discoveryData.title = entitiesObj.title;
				discoveryData.position = entitiesObj.position;
				discoveryData.categories = categoryArr;
				discoveryData.seriesData = seriesDataArr;
				discoveryData.entityType = entitiesObj.type;
				discoveryData.isExpanded = entitiesObj.isExpanded;

				//discoveryData.DiscoveryLi = discoveryLi;
				//EntityType at Infrastructure
				//DateYearly = 15
				//DateMonthly = 16
				//DateWeekly = 17
				//DateDaily = 18


				if (_.indexOf([15, 16, 17, 18], entitiesObj.type) >= 0) {   // Date Navigator
					//$($this.selectors.chartContainer, discoveryLi).css('height', 160 + ''); //Fix height for column chart
					//var chartTitle = '${distribution}: '
					var chartTitle = "";
					switch (entitiesObj.type) {
						case 15:
							chartTitle = $this.tokens.yearlyTkn;
							break;
						case 16:
							chartTitle = $this.tokens.monthlyTkn;
							break;
						case 17:
							chartTitle = $this.tokens.weeklyTkn;
							break;
						case 18:
							chartTitle = $this.tokens.dailyTkn;
							break;
					}
					discoveryData.chartTitle = chartTitle;  //entitiesObj.newsEntities[0].typeDescriptor;
					//$this._renderDiscoveryDateFilters(discoveryData, index);
				} else {
					//$($this.selectors.chartContainer, discoveryLi).css('height', 27 * (discoveryEntityObjArr.length) + '');  //calucate the height for bar chart, height is depended on the num of items
					//$this._renderDiscoveryFilters(discoveryData, index);
				}

				discoveryDataArr.push(discoveryData);
			}
		});



		//Sort by position
		discoveryDataArr.sort(function (a, b) {
			return a.position > b.position;
		});

		//idx = 0;
		//Render Discovery Filters after sorting
		_.each(discoveryDataArr, function (dd) {
			//Expand Collapse the div based on the isExpanded property
			if (dd.isExpanded) {
				var container = $this.$element.find('.dj_discoveryFilters-listItem-' + dd.position);
				var collapseBtn = $($this.selectors.collapseBtn, container);
				$this._expandCollapse(collapseBtn, true);
			}
			discoveryLi = $this.$element.find('.dj_discoveryFilters-listItem-' + dd.position);


			if (_.indexOf([15, 16, 17, 18], dd.entityType) >= 0) {   // Date Navigator
				$($this.selectors.chartContainer, discoveryLi).css('height', 160 + ''); //Fix height for column chart
				$this._renderDiscoveryDateFilters(dd, dd.position);
			} else {
				if (dd.seriesData.length == 0) { //no result for the navigator
					$($this.selectors.chartContainer, discoveryLi).css('height', 20 + '');  //The height for chat is fix if no result for the navigator
				} else {
					$($this.selectors.chartContainer, discoveryLi).css('height', 27 * (dd.seriesData.length) + 20 + '');  //calucate the height for bar chart, height is depended on the num of items
				}
				$this._renderDiscoveryFilters(dd, dd.position);
			}

			//idx++;
		});

	},

	//On Discovery Item Click Event Handler
	_onItemClicked: function (evt) {
		var self = this;
		self.publish(self.events.itemClick, { "data": evt.point.options.jsonObj });
	},

	//On Discovery Filters Export Click
	_onExportClicked: function (evt) {
		var self = this;
		self.publish(self.events.exportClick, { "data": "test export" });
	},

	//Get discovery Filter Config
	_getDiscoveryFiltersConfig: function () {
		var self = this;
		//BEGIN: Discovery Filters Configuration
		return {
			chart: {
				defaultSeriesType: 'bar'
				//margin: [0, 0, 0, 0]
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
						return this.value[0];
					},
					style: {
						width: "100px"
					}
				},
				tickWidth: 0,
				title: { text: null },
				//maxPadding: 0.15,
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
					color: '#5BB4E5',
					cursor: 'pointer',
					pointWidth: 5
				},
				bar: {
					point: { events: { click: self._delegates.OnItemClicked} },
					stacking: "percent",
					shadow: false,
					minPointLength: 2
				}
			},
			legend: { enabled: false },
			credits: { enabled: false },
			exporting: { enabled: false }
		};
		//END: Discovery Filters Configuration
	},

	//Get discovery Filter Date Chart Config
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
						fontFamily: "Arial, sans-serif",
						fontSize: "9px",
						fontWeight: "normal",
						width: "180px"
					},
					y: 20,
					formatter: function () {
						//if(this.isFirst || this.isLast){
						//    return this.value;
						//}
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
						fontFamily: "Arial, sans-serif",
						fontSize: "9px",
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
						fontFamily: "Arial, sans-serif",
						fontSize: "9px",
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
			/*tooltip: {
			enabled: false
			},*/
			plotOptions: {
				column: {
					color: "#5bb4e5",
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

	/*_extractSeriesData: function (seriesData) {
	var actualDataArr = [], tweakedDataArr = [];
	_.each(seriesData, function (obj) {
	//Construct original series array
	actualDataArr.push({
	dataLabels: {
	style: {
	display: 'none'
	}
	},
	jsonObj: obj.jsonObj,
	y: obj.y
	});

	//Contruct tweaked series data
	tweakedDataArr.push({
	y: seriesData[0].y,
	color: 'white',
	jsonObj: obj.jsonObj,
	dataLabels: { x: -25, y: 1, formatter: function () { return obj.y; } }
	});
	});
	return { "actual": actualDataArr, "tweaked": tweakedDataArr };
	},
	*/
	_extractDateSeriesData: function (seriesData) {
		var dateDataArr = [];
		_.each(seriesData, function (obj) {
			//Construct original series array
			var displayVal = obj.jsonObj.currentTimeFrameNewsVolume.displayText.value;
			dateDataArr.push({
				name: displayVal,
				y: obj.y,
				jsonObj: obj.jsonObj
			});
		});
		return dateDataArr;
	},

	_extractSeriesData: function (seriesData) {
		var returnData = [[], []];
		if (typeof (seriesData) !== "undefined") {
			for (var i = 0; i < seriesData.length; i++) {
				returnData[0][i] = { y: (seriesData[0].y - seriesData[i].y), name: seriesData[i].y };
				returnData[1][i] = { y: seriesData[i].y, jsonObj: seriesData[i].jsonObj };
			}
		}
		return returnData;
	},

	//Render Discovery Filters Chart
	_renderDiscoveryFilters: function (discoveryData, idx) {
		var self = this;
		var chartContainer = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.dj_hc-container');
		var chartTitle = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.dj_cd-title');
		chartTitle.html(discoveryData.title);
		var seriesData = this._extractSeriesData(discoveryData.seriesData);
		if (seriesData[0].length == 0) {
			chartContainer.html(self.tokens.noResultsTkn);
			return;
		}
		return new Highcharts.Chart($.extend(true, {}, this.discoveryFitlersConfig, {
			chart: {
				renderTo: chartContainer[0],
				width: $(chartContainer[0]).width()
			},
			xAxis: {
				categories: discoveryData.categories
			},
			tooltip: {
				formatter: function () {
					return this.x[1];
				},
				style: {
					color: '#333333',
					fontSize: '7pt'
					//padding: '2px'
				}
			},
			series: [{
				name: "faux",
				color: "#ffffff",
				pointWidth: 5,
				dataLabels: {
					enabled: true,
					align: "right",
					y: 12,
					color: "#a6a6a6",
					formatter: function () {
						return this.point.name;
					}
					/*style: {
					fontWeight: "normal",
					fontFamily: "Arial",
					fontSize: "11px"
					}*/
				},
				data: seriesData[0]
			}, {
				name: "data",
				dataLabels: {
					"enabled": false
				},
				pointWidth: 5,
				color: "#4da7cc",
				data: seriesData[1]
			}]
		}));
	},

	//Render Discovery Filters Date Chart
	_renderDiscoveryDateFilters: function (discoveryData, idx) {
		var self = this;
		var chartContainer = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.dj_hc-container');
		var chartTitle = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.dj_cd-title');
		chartTitle.html(discoveryData.title);

		var seriesData = this._extractDateSeriesData(discoveryData.seriesData);
		return new Highcharts.Chart($.extend(true, {}, this.discoveryFitlersDateChartConfig, {
			chart: {
				renderTo: chartContainer[0],
				width: $(chartContainer[0]).width()
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
					if (discoveryData.chartTitle == 'daily') {
						tooltipStr = self.tokens.dateTkn + ': ' + this.x[0];
					} else {
						tooltipStr = self.tokens.startDateTkn + ': ' + this.x[0] + '<br/>' + self.tokens.endDateTkn + ': ' + this.x[1];
					}
					return tooltipStr + '<br/>' + self.tokens.numHitsTkn + ': ' + Highcharts.numberFormat(this.y, 0);

				},
				style: {
					color: '#333333',
					fontSize: '7pt'
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