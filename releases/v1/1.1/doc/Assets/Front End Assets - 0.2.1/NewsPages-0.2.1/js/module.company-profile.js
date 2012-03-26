/*

	-- -- -- -- -- -- --
	Description: Company Profile Module Plugin and Functions
	Version: 0.1
	Last Update: 01/10/2011
	Author: Ron Edgecomb II
	Company: Dow Jones & Company Inc.
	Copyright 2010 Dow Jones & Company Inc.
	
	Dependencies
	 - js/libs/jquery-1.4.3.min.js
	 - js/dashboard.js
	-- -- -- -- -- -- --
	
*/
(function($){

	if( !$.dj.module ){
		$.dj.module = new Object();
	};
	
	$.dj.module.companyProfile = function( el, options ){

		var module = this;
		
		// Access to jQuery and DOM versions of element
		module.$el = $(el);
		module.el = el;
		
		//window reference
		module.$messenger	= $(window);

		// Key module components
		module.$header		= $( '.module-header', module.$el ),
		module.$buttons		= $( '.dc_item', module.$header ),
		
		module.$edit		= $( '.module-edit-options', module.$el ),
		
		module.$core		= $( '.module-core', module.$el ),
		module.$footer		= $( '.module-footer', module.$core );
		
		// Add a reverse reference to the DOM object
		module.$el.data( "dj.module.companyProfile", module );
		
		module.init = function(){
			
			module.options = $.extend( {}, $.dj.module.companyProfile.defaultOptions, options);
			
			//DEV-NOTE: proof of concept to show the loading of all module data
			module.$el.addAction( 'module/initialize', function( $module, $header, $edit, $core ) {
				
				//DEV-NOTE: these options should be configured in production version
				var options = $module.data( "dj.module" ).options;
				options.disablePagination = true;
				
				module.loadModuleData( true );
				
			} );
			
		};
	
		module.loadModuleData = function( init ) {

			//DEV-NOTE: begin - proof of concept to show the loading of chart data			
			var	$moduleCol		= $('.module-col', module.$core),
				$moduleColWrap	= $('.module-col-wrap', $moduleCol),
				i = 0;;
			
			if( !init )
				module.$core.block( );
			
			$moduleColWrap.empty();
			
			$moduleCol.block( {
				
				message: '<div class="dj-loading"></div>',
				
				overlayCSS:  { 
					backgroundColor: '#fff'
				}
				
			} );			
			
			(function loadCol() {

				var $currentCol = $moduleCol.eq(i++),
					$currentColWrap = $( '.module-col-wrap', $currentCol );
				
				if( $currentCol.length > 0 ){
					
					switch( i ) {
					
						case 1: //main chart
						
							$currentColWrap.getSampleContent( 'public-company-chart' );
						
							var $currentRangeControl =  $( module.options.chartRangeSelector + ' li.active', module.$el );

							if( !$currentRangeControl.length )
								$currentRangeControl = $( module.options.chartRangeSelector + ' li:first', module.$el );

							var dateRangeData = module.getDateRangeData( $currentRangeControl.attr('title') );

							module.$el.data( {
								chartDateRangeData: 	dateRangeData,
								snapshotDateRangeData:	dateRangeData
							} );
						
							module.init.chart( dateRangeData, $currentRangeControl );
							module.init.chartRangeControls();							
						break;
						case 2: //company snapshot
							module.loadCompanySnapshot();							
						break;
						case 3: //trending topic
							module.loadTrendingTopics();
						break;
						case 4: //recent articles
							module.loadRecentArticles();
						break;						
						
					}
					$currentCol.unblock();					
					setTimeout( loadCol, 500 );
					
				} else {
					
					if( !init )
						module.$core.unblock( );
									
				}
				
			})();
			//DEV-NOTE: end - proof of concept to show the loading of chart data
			
		};		
		
		module.init.chart = function( dateRangeData, $currentRangeControl ) {
			
			if( !dateRangeData ) {
				//kill tha machine, this aint working!
				return false;
			}
			
			//just in case....
			$currentRangeControl.addClass('active');
			module.updateChartTitles( dateRangeData.startDate, dateRangeData.endDate );
			
			var	seriesConfig = module.fetchSeriesConfig( dateRangeData.startDate, dateRangeData.endDate, dateRangeData.rangeDays ),
				chartOptions = $.dj.module.companyProfile.chartOptions;

			//add the series data to the dateRangeData
			dateRangeData.seriesData = [];
			
			for( var i = 0; i < seriesConfig.length; i++ ) {
				dateRangeData.seriesData[i] = seriesConfig[i].data;
			}
			
			//cache initial data
			$currentRangeControl.data( {
				
				dateRangeData	:	dateRangeData,
				cachedChartData	:	true
				
			} );
				
			//if private company, remove the stock price series config 
			if( seriesConfig.length == 1 )
				chartOptions.yAxis.splice( 1, 1 ); 
			
			//if the series is greater than 31 values, let highcharts calculate interval
			if( dateRangeData.seriesData[0].length > 31 ) {
				chartOptions.xAxis.tickInterval = null;
				chartOptions.xAxis.labels.step = null;
			}			
			
			module.chart = new Highcharts.Chart( $.extend( true, {}, chartOptions, {

				chart: {
					renderTo: $( module.options.chartWrapSelector, module.$el )[0],
					events: {
						click: function( event ) {

							var	selectedDate	=	module.$el.data('tooltipX'),
								dateRangeData	=	module.getDateRangeData( Highcharts.dateFormat('%B %e, %Y', selectedDate ) + ' - ' + Highcharts.dateFormat('%B %e, %Y', selectedDate ) );
							
							module.$el.data( 'snapshotDateRangeData', dateRangeData );
							
							module.loadSnapshotColumns();
							
							//snapshot title text below chart
							$('h3.module-row-title span.snapshot-date-range', module.$el).text( Highcharts.dateFormat('%b. %e', dateRangeData.startDate ) + ' to ' + Highcharts.dateFormat('%b. %e %Y', dateRangeData.endDate ) );
							

			            }
					}
				},
				
				plotOptions: {
			        series: {
			            events: {
			                click: function(event) {
				
								var	selectedDate	=	module.$el.data('tooltipX'),
									dateRangeData	=	module.getDateRangeData( Highcharts.dateFormat('%B %e, %Y', selectedDate ) + ' - ' + Highcharts.dateFormat('%B %e, %Y', selectedDate ) );

								module.$el.data( 'snapshotDateRangeData', dateRangeData );

								module.loadSnapshotColumns();

								//snapshot title text below chart
								$('h3.module-row-title span.snapshot-date-range', module.$el).text( Highcharts.dateFormat('%b. %e', dateRangeData.startDate ) + ' to ' + Highcharts.dateFormat('%b. %e %Y', dateRangeData.endDate ) );
			
			                }
			            }
			        }
			    },

				tooltip: {
					formatter: function() {

						// caputre the x tooltip value (aka, tooltip highlighted date)
						module.$el.data( 'tooltipX', this.x );

						var s = Highcharts.dateFormat('%B %e, %Y', this.x )+'<br/>';
						
						s += 'News Volume: <b>' + this.points[0].y + ' Articles</b>';
						
						if( this.points[1] )
							s += '<br/>Price: <b>$' + Highcharts.numberFormat( this.points[1].y ) + '</b>';
						
						return s;

					}
				},

				series: seriesConfig

			} ) );
			
		}; //end - chartInit
		
		module.init.chartRangeControls = function() {
		
			$( module.options.chartRangeSelector, module.$el ).delegate('li', 'click', function() {

				var $module = module.$el,
					$rangeControls = $( module.options.chartRangeSelector + ' li', module.$el ),
					$currentRangeControl = $(this);

				//show loading
				module.chart.showLoading();
				
				//snapshot columns
				module.loadSnapshotColumns();

				//grab the chached data if available
				if( $currentRangeControl.data('cachedChartData') ) {

					var dateRangeData	=	$currentRangeControl.data( 'dateRangeData' );

				} else {

					var dateRangeData	=	module.getDateRangeData( $currentRangeControl.attr('title') ),
						seriesData		=	module.fetchSeriesData( dateRangeData.startDate, dateRangeData.endDate, dateRangeData.rangeDays );
					dateRangeData.seriesData = seriesData;
					
					$currentRangeControl.data( {
						dateRangeData:		dateRangeData,
						cachedChartData:	true
					} );

				}
				
				$module.data( 'chartDateRangeData', dateRangeData );

				//remove active class from previous range and add to current
				$rangeControls.removeClass( 'active' );
				$currentRangeControl.addClass( 'active' );
				
				for( var i = 0; i < dateRangeData.seriesData.length; i++ ) {

					module.chart.series[i].options.pointStart = dateRangeData.startDate;
					module.chart.series[i].setData( dateRangeData.seriesData[i], false );

				}

				//Chart will only properly display 15 labels and 31 ticks along the x-axis, must update appropriately
				module.chart.xAxis[0].options.tickInterval	=	( dateRangeData.rangeDays <= 31 )?( 24 * 60 * 60 * 1000 ):null; // 1 day or calculated by highcharts
				module.chart.xAxis[0].options.labels.step	=	( dateRangeData.rangeDays <= 31 )?2:null; // 2 or calculated by highcharts

				module.updateChartTitles( dateRangeData.startDate, dateRangeData.endDate );

				//redraw the chart with the new data
				module.chart.redraw();

				//hide the loading text
				module.chart.hideLoading();

				$currentRangeControl.blur();

				return false;

			} );		
			
		};//end - chartRangeControlsInit
		
		module.updateChartTitles = function( startDate, endDate ) {
		
			//update title text above chart
			$('h3.module-row-title span.chart-date-range', module.$el).text( Highcharts.dateFormat('%B %Y', startDate ) );

			//snapshot title text below chart
			$('h3.module-row-title span.snapshot-date-range', module.$el).text( Highcharts.dateFormat('%b. %e', startDate ) + ' to ' + Highcharts.dateFormat('%b. %e %Y', endDate ) );		
			
		};
		
		module.getDateRangeData = function( range ) {

			var dateRange	=	range.split('-'),
				startDate	=	function() {

					//parse returns local time, convert to UTC
					var tempDate = new Date( Date.parse( $.trim( dateRange[0] ) ) );
					return Date.UTC( tempDate.getFullYear(), tempDate.getMonth(), tempDate.getDate() );

				}(),
				endDate		=	function() {

					//parse returns local time, convert to UTC
					var tempDate = new Date( Date.parse( $.trim( dateRange[1] ) ) ); 
					return Date.UTC( tempDate.getFullYear(), tempDate.getMonth(), tempDate.getDate() );

				}(),
				rangeDays	=	Math.round( ( endDate - startDate ) / ( 24 * 60 * 60 * 1000 ) ) + 1;
		
			return {

				dateRange:	dateRange,
				startDate:	startDate,
				endDate:	endDate,
				rangeDays:	rangeDays
				
			};
			
		};
		
		module.fetchSeriesData = function( startDate, endDate, rangeDays ) {
		
			//DEV-TODO: fetch series data, may need to add additional function param for series type (news volume or stock price)
		
			//temporary for proof of concept...
			return [
				$.dj.randomChartSeries( 75, 200, rangeDays ), //data for news volume
				$.dj.randomChartSeries( 25, 150, rangeDays ) //data for stock price
			];
		
		};
		
		module.fetchSeriesConfig = function( startDate, endDate, rangeDays ) {
			
			//DEV-TODO: fetch series config data, may need to add additional function param for series type (news volume or stock price)
		
			//temporary for proof of concept...
			var tempDate = new Date( startDate ),
				tempData = module.fetchSeriesData ( startDate, endDate, rangeDays );
			
			return [ {
				name: 'News Volume',
				pointStart: Date.UTC( tempDate.getFullYear(), tempDate.getMonth(), tempDate.getDate() ),
				pointInterval: 24 * 60 * 60 * 1000, // 1 day,
				data: tempData[0]
			}, {
				name: 'Stock Price',
				type: 'spline',
				yAxis: 1,
				pointStart: Date.UTC( tempDate.getFullYear(), tempDate.getMonth(), tempDate.getDate() ),
				pointInterval: 24 * 60 * 60 * 1000, // 1 day
				data: tempData[1]
			} ];
			
		};
		
		module.loadCompanySnapshot = function() {

			var $col = $( module.options.snapshotRowSelector + ' .company-snapshot-col', module.$el ),
				$colWrap = $('.module-col-wrap', $col),
				dateRangeData = module.$el.data( 'snapshotDateRangeData' );
						
			//DEV-TODO: fetch company snapshot	
			$colWrap.getSampleContent( 'public-company-snapshot' );
			
			module.setupSnapshotTabs();

			
		};
		
		module.loadTrendingTopics = function(){
			
			var $col = $( module.options.snapshotRowSelector + ' .trending-chart-col', module.$el ),
				$colWrap = $('.module-col-wrap', $col),
				dateRangeData = module.$el.data( 'snapshotDateRangeData' );
			
			//DEV-TODO: fetch base html	
			$colWrap.getSampleContent( 'trending-chart-content' );
			
			//DEV-TODO: fetch trending chart data and put in bubble chart
			$('.trending-chart', $colWrap ).bubbleChart( {
				
				items:[ {
						id: "A",
						name: "Tech Stocks",
						radius: 50,
						posX: 50,
						posY: 50
				}, {
						id: "B",
						name: "Book Publishers",
						radius: 40,
						posX: 130,
						posY: 200
				}, {
						id: "C",
						name: "Publishers Associations",
						radius: 60,
						posX: 140,
						posY: 90
				}, {
						id: "D",
						name: "New York",
						radius: 35,
						posX: 200,
						posY: 250
				}, {
						id: "E",
						name: "Wall Street",
						radius: 25,
						posX: 180,
						posY: 30
				}, {
						id: "F",
						name: "App Store",
						radius: 35,
						posX: 50,
						posY: 172
				}, {
						id: "G",
						name: "Computer Companies",
						radius: 40,
						posX: 40,
						posY: 250
				}, {
						id: "H",
						name: "Apple",
						radius: 20,
						posX: 140,
						posY: 260
				}, {
						id: "I",
						name: "Dow Jones",
						radius: 40,
						posX: 250,
						posY: 180
				}, {
						id: "J",
						name: "News Corp",
						radius: 25,
						posX: 200,
						posY: 130
				}, {
						id: "K",
						name: "Fox News",
						radius: 40,
						posX: 250,
						posY: 50
				} ]
				
			} );
			
			//DEV-NOTE: the delay is just for a temporary effect
			$colWrap.animate({ opacity:1 }, 'slow', function() {
				
                $col.removeClass('module-col-loading');
				$('.prev-content', $colWrap).remove();

            });			
			
		};//end - loadTrendingTopics

		module.loadRecentArticles = function(){
		
			var $col = $( module.options.snapshotRowSelector + ' .article-group-col', module.$el ),
				$colWrap = $('.module-col-wrap', $col),
				dateRangeData = module.$el.data( 'snapshotDateRangeData' );
		
			//DEV-TODO: fetch base article group	
			$colWrap.getSampleContent( 'article-group-content' );
			
		};//end - loadRecentArticles
		
		module.setupSnapshotTabs = function(){
			
			var $col = $( module.options.snapshotRowSelector + ' .company-snapshot-col', module.$el )
			 	$tabs = $( ".snapshot-tabs", $col ).children(),
				$tabContent = $( ".tab-content-wrap", $col ).children();

			$tabContent.filter(":first").css( 'display', "block" );
			
			$tabs.bind( 'click', function () {
				
				var $targetTab = $(this),
					$siblingTabs = $targetTab.siblings(),
					targetLocation;

				if( $targetTab.hasClass("active") ) {
					
					return false;
					
				} else {

					fullTargetLocation = $targetTab.attr("href").split('#');

					targetLocation = fullTargetLocation[1];

					$siblingTabs.removeClass("active");
					$targetTab.addClass("active");

					$tabContent.each( function() {

						var $this = $(this);
						if( $(this).hasClass( targetLocation ) ) {

							$this.css({
								display: "block"	
							});

						} else {
							$this.css({
								display: "none"	
							});
						}

					} );

					return false;
				}

			}).filter( ':first' ).addClass( 'active' );
						
		};//end - setupSnapshotTabs
		
		// Run initializer
		module.init();
		
	};
	
	$.dj.module.companyProfile.defaultOptions = {
		
		chartWrapSelector:		".company-module-chart",
		chartRangeSelector:		".company-chart-date-range",
		snapshotRowSelector: 	".company-snapshot-row" 

	};
	
	$.dj.module.companyProfile.chartOptions = {

		chart: {
			defaultSeriesType: 'line',
			height: 300,
			zoomType: 'x',
			spacingRight: 0,
			spacingLeft: 0
		},

		loading: {
	        hideDuration: 250,
	        showDuration: 250
	    },

		colors: [
			'#ff9300',
			'#41a4ce'
		],

		credits: false,

		title: {
			text: null
		},

		subtitle: {
			text: null
		},

		xAxis: {
			type: 'datetime',
			tickInterval: 24 * 60 * 60 * 1000, // one day
			labels: {
				formatter: function() {
					return Highcharts.dateFormat('%b %d', this.value);
				},
				step: 2,
				style: {
					color: '#999999',
					fontSize: '6pt'
				}
			},
			gridLineDashStyle: 'shortdash',
			gridLineWidth: 1
		},

		yAxis: [{
			title: {
				text: null
			},
			labels: {
				style: {
					color: '#999999',
					fontSize: '7pt'
				}
			},
			gridLineWidth: 0
		},{
			title: {
				text: '$USD',
				style: {
					color: '#999999',
					fontSize: '7pt'
				}
			},
			alternateGridColor: '#f1f3f8',
			gridLineWidth: 0,
			labels: {
				style: {
					color: '#999999',
					fontSize: '7pt'

				}
			},
			opposite: true
		}],

		tooltip: {
			backgroundColor: 'rgba(100, 100, 100, .85)',
			borderColor: '#333333',
			style: {
				color: '#ffffff'
			},
			shared: true,
			crosshairs: {
				color: '#000000'
			}
		},

		legend: {
			enabled: false
		},

		plotOptions: {
			series: {
				shadow: false,
				lineWidth: 3,
				marker: {
					enabled: false,
	                states: {
                    	hover: {
							enabled: true
	                    }
	                }
				}
			}
		}

	};
	
	$.fn.dj_module_companyProfile = function(options){
		return this.each(function(){
			(new $.dj.module.companyProfile(this, options));
		});
	};
	
	// This function breaks the chain, but returns
	// the dj.module.companyProfile if it has been attached to the object.
	$.fn.getdj_module_companyProfile = function(){
		this.data("dj.module.companyProfile");
	};
	
})(jQuery);

$(function() {

	$('.company-profile-module', $('#dashboard')).dj_module_companyProfile();

} );