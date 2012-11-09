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

DJ.UI.DiscoveryFilters = DJ.UI.Component.extend({

	/*
	* Properties
	*/

	//Selectors
	selectors: {
		discoveryFiltersList: 'ul.dj_discoveryFilters-list',
		chartContainer: 'div.dj_hc-container',
		export: 'span.dj_df-export a.export',
		expandBtn: 'a.dj_df-expand',
		collapseBtn: 'a.dj_df-collapse',
		expandDiv: 'div.cd_div_expand',
		collapseDiv: 'div.cd_div_collapse'
	},

	// Default options
	defaults: {
		debug: false,
		cssClass: 'DiscoveryFilters',
		sortable: true
		// ,name: value     // add more defaults here separated by comma
	},

	//Events
	events: {
		itemClick: 'itemClick.dj.DiscoveryFilters',
		exportClick: 'exportClick.dj.DiscoveryFilters'
	},

	// Localization/Templating tokens
	tokens: {
		// name: value     // add more defaults here separated by comma
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
		this.bindOnSuccess(this.data.discovery);

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
		$(this.selectors.collapseBtn).bind('click', function () {
			self._expandCollapse(this, true); return false;
		});
	},

	//Expand/Collapse function
	_expandCollapse: function (el, expand) {
		var self = this;
		var ContainerLi = $(el).closest('li');
		if (expand) {
			$(el).removeClass('dj_df-collapse').addClass('dj_df-expand').unbind('click').bind('click', function () {
				self._expandCollapse(this, false); return false;
			});
			$(this.selectors.collapseDiv, ContainerLi).removeClass('cd_div_collapse').addClass('cd_div_expand');
			$(this.selectors.export, ContainerLi).removeClass('hide').addClass('show');
		} else {
			$(el).removeClass('dj_df-expand').addClass('dj_df-collapse').unbind('click').bind('click', function () {
				self._expandCollapse(this, true); return false;
			});
			$(this.selectors.expandDiv, ContainerLi).removeClass('cd_div_expand').addClass('cd_div_collapse');
			$(this.selectors.export, ContainerLi).removeClass('show').addClass('hide');
		}
	},

	//Check if sortable is enabled
	_isSortable: function () {
		return this.options.sortable;
	},

	//Initialize Delegates
	_initializeDelegates: function () {
		$.extend(this._delegates, {
			OnItemClicked: $dj.delegate(this, this._onItemClicked),
			OnExportClicked: $dj.delegate(this.selectors.export, 'click', this._onExportClicked)
		});
	},

	//Initialize Discovery Filters
	_initializeDiscoveryFilters: function (discoveryEntityObj) {
		var $this = this,
		index = 0,
		discoveryData = {};
		//Build the Discovery Entity Lists
		_.each(discoveryEntityObj, function (entitiesObj) {
			var categoryArr = [],
				seriesDataArr = [],
				discoveryEntityObjArr = entitiesObj.newsEntities;

			//Expand Collapse the div based on the isExpanded property
			if (entitiesObj.isExpanded) {
				var container = $this.$element.find('.dj_discoveryFilters-listItem-' + index);
				var collapseBtn = $($this.selectors.collapseBtn, container);
				$this._expandCollapse(collapseBtn, true);
			}

			//Set the height for each hc container based on no. of entities
			var discoveryLi = $this.$element.find('.dj_discoveryFilters-listItem-' + index);

			//$($this.selectors.chartContainer, discoveryLi).css('height', 27 * (discoveryEntityObjArr.length) + '');

			//Construct the graph for the discovery entity objects
			var idx = 0;
			_.each(discoveryEntityObjArr, function (entity) {
				var dataObj = {};
				dataObj.y = entity.currentTimeFrameNewsVolume.value;
				dataObj.jsonObj = entity;
				seriesDataArr[seriesDataArr.length] = dataObj;
				if (_.indexOf([15, 16, 17, 18], entitiesObj.type) >= 0) {   //Date Navigator
					/*if (idx==0) {
						categoryArr[categoryArr.length] = entity.startDateFormattedString;  //Start
					} else if (idx == discoveryEntityObjArr.length - 1){
						categoryArr[categoryArr.length] = entity.endDateFormattedString;    //End
					} else {
						categoryArr[categoryArr.length] = "";   //skip the dates in the middle
					}*/
					categoryArr[categoryArr.length] = [entity.startDateFormattedString,categoryArr[categoryArr.length] = entity.endDateFormattedString];
				}
				else{
					categoryArr[categoryArr.length] = entity.descriptor;
				}
				idx++;
			});
			//discoveryData.title = entitiesObj.title;
			discoveryData.categories = categoryArr;
			discoveryData.seriesData = seriesDataArr;
			//EntityType at Infrastructure
			//DateYearly = 15
			//DateMonthly = 16
			//DateWeekly = 17
			//DateDaily = 18
			if (_.indexOf([15, 16, 17, 18], entitiesObj.type) >= 0) {   // Date Navigator
				$($this.selectors.chartContainer, discoveryLi).css('height', 160 + ''); //Fix height for column chart
				//var chartTitle = '${distribution}: '
				var chartTitle;
				switch (entitiesObj.type) {
					case 15:
						chartTitle = 'yearly';
						break;
					case 16:
						chartTitle = 'monthly';
						break;
					case 17:
						chartTitle = 'weekly';
						break;
					case 18:
						chartTitle = 'daily';
						break;
				}
				discoveryData.chartTitle = chartTitle;  //entitiesObj.newsEntities[0].typeDescriptor;
				$this._renderDiscoveryDateFilters(discoveryData, index);
			}else{
				$($this.selectors.chartContainer, discoveryLi).css('height', 27 * (discoveryEntityObjArr.length) + '');  //calucate the height for bar chart, height is depended on the num of items
				$this._renderDiscoveryFilters(discoveryData, index);
			}
			index++;
		});
	},

	//On Discovery Item Click Event Handler
	_onItemClicked: function (evt) {
		var self = this;
		$dj.info(self.events.itemClick + " Event clicked");
		self.publish(self.events.itemClick, { "data": evt.point.options.jsonObj });
	},

	//On Discovery Filters Export Click
	_onExportClicked: function (evt) {
		var self = this;
		$dj.info(self.events.exportClick + " Event clicked");
		self.publish(self.events.exportClick, { "data": "test export" });
	},

	//Get discovery Filter Config
	_getDiscoveryFiltersConfig: function () {
		//BEGIN: Discovery Filters Configuration
		return {
			chart: { type: 'bar' },
			title: { text: null },
			subtitle: { text: null },
			xAxis: {
				lineWidth: 0,
				labels: { align: 'left', x: 0, y: 7 },
				tickWidth: 0,
				title: { text: null }
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
					cursor: 'pointer'
				},
				bar: {
					dataLabels: { enabled: true },
					pointWidth: 5,
					groupPadding: 0,
					borderWidth: 0,
					pointPadding: 0,
					shadow: false
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
		return {
			chart: {
				defaultSeriesType: "column",
				height:160,
				width:200
			},
			credits: {enabled: false},
			title: {
				text: null
			},
			subtitle: {
				text:null
			},
			xAxis: {
				gridLineColor: "#ffffff",
				labels: {
					style: {
						color: "#003366",
						fontFamily: "Arial, sans-serif",
						fontSize: "9px",
						fontWeight: "normal",
						width: "230px"
					},
					y: 20,
					formatter: function() {
						//if(this.isFirst || this.isLast){
						//    return this.value;
						//}
						if(this.isFirst)
							return this.value[0];
						if(this.isLast)
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
					width: 1}],
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
					cursor: 'pointer'
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

	_extractSeriesData: function (seriesData) {
		var actualDataArr = [], tweakedDataArr = [];
		_.each(seriesData, function (obj) {
			//Construct original series array
			actualDataArr[actualDataArr.length] = {
				dataLabels: {
					style: {
						display: 'none'
					}
				},
				y: obj.y
			};

			//Contruct tweaked series data
			tweakedDataArr[tweakedDataArr.length] = {
				y: seriesData[0].y,
				color: 'transparent',
				dataLabels: { x: -25, y: 1, formatter: function () { return obj.y; } }
			}
		});
		return { "actual": actualDataArr, "tweaked": tweakedDataArr };
	},

	_extractDateSeriesData: function (seriesData) {
		var DateDataArr = [];
		_.each(seriesData, function (obj) {
			//Construct original series array
			var displayVal = obj.jsonObj.currentTimeFrameNewsVolume.displayText.value;
			DateDataArr[DateDataArr.length] = [displayVal, obj.y];
		});
		return DateDataArr;
	},

	//Render Discovery Filters Chart
	_renderDiscoveryFilters: function (discoveryData, idx) {
		var chartContainer = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.dj_hc-container');
		var seriesData = this._extractSeriesData(discoveryData.seriesData);
		return new Highcharts.Chart($.extend(true, {}, this.discoveryFitlersConfig, {
			chart: {
				renderTo: chartContainer[0],
				width: $(chartContainer[0]).width()
			},

			xAxis: {
				categories: discoveryData.categories
			},
			tooltip: {
				enabled: false
			},
			series: [{
				name: null,
				data: seriesData.tweaked
			}, {
				name: null,
				data: seriesData.actual
			}]
		}));
	},

	//Render Discovery Filters Date Chart
	_renderDiscoveryDateFilters: function (discoveryData, idx){
		var chartContainer = this.$element.find('.dj_discoveryFilters-listItem-' + idx).find('.dj_hc-container');
		var seriesData = this._extractDateSeriesData(discoveryData.seriesData);
		return new Highcharts.Chart($.extend(true, {}, this.discoveryFitlersDateChartConfig, {
			chart: {
				renderTo: chartContainer[0]
				//width: $(chartContainer[0]).width()
			},
			xAxis: {
				categories: discoveryData.categories,
				title: {
					"text": '${distribution}: ' + '${' + discoveryData.chartTitle + '}'
				}
			},
			tooltip: {
				formatter: function() {
					var tooltipStr;
					if (discoveryData.chartTitle == 'daily') {
						tooltipStr = '${date}: '+ this.x[0]
					} else {
						tooltipStr = '${startDate}: '+ this.x[0] + '<br/>${endDate}: '+ this.x[1]
					}
					return tooltipStr + '<br/>${numHits}: '+ Highcharts.numberFormat(this.y, 0);
					
					//return /*this.series.name + */'${startDate}: <b>'+ this.x[0] + '${endDate}: <b>'+ this.x[1]
					//	 +'</b><br/>${numHits}: '+ Highcharts.numberFormat(this.y, 0);
					
				},
				style: {
					color: '#333333',
					fontSize: '7pt',
					padding: '2px'
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