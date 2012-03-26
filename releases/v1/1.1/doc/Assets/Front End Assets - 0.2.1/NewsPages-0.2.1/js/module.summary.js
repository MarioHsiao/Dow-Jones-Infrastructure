/*

	-- -- -- -- -- -- --
	Description: Summary Module Plugin and Functions
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
	
	$.dj.module.summary = function( el, options ){

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
		module.$el.data( "dj.module.summary", module );
		
		module.init = function(){
			
			module.options = $.extend( {}, $.dj.module.summary.defaultOptions, options);

			//DEV-NOTE: proof of concept to show the loading of all module data
			module.$el.addAction( 'module/initialize', function( $module, $header, $edit, $core ) {
				
				//DEV-NOTE: these options should be configured in production version
				var options = $module.data( "dj.module" ).options;
				options.disablePagination = true;
				
				module.loadModuleData( true );
				
			} );
			
			//DEV-NOTE: proof of concept to show the reloading of all module data
			module.$el.addAction( 'module/reload', function( $module, $header, $edit, $core ) {
				
				module.loadModuleData( );
				
			} );
						
			//DEV-NOTE: proof of concept for generic popup link
			/* currently not working....
			module.$el.delegate('.popup-trigger', 'click', function() {
	            $(this).popup({
	                title  : '',
	                body   : '',
	                open   : function( panel ) {
		
			            panel.update({state:'dj-loading'});

			            setTimeout(function() {
			                panel.update({
			                    body  : 'content body returned by the server',
			                    title : 'One very, very, very long popup title that should overlow the available space',
			                    state : 'dj-loaded'
			                });
			            }, 1000);
					
					},
	                close  : function() {  },
	                width  : 300,
	                height : 200
	            });
	        });
			*/

		};
		
		module.loadModuleData = function( init ) {
			
			//DEV-NOTE: begin - proof of concept to show the loading of chart data
			var dateRangeData = module.getDateRangeData( module.options.chartDateRange ),
				$moduleCol		= $('.module-col', module.$core),
				$moduleColWrap	= $('.module-col-wrap', $moduleCol),
				i = 0;

			module.$el.data( {
				chartDateRangeData: 	dateRangeData
			} );
			
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
					
						case 1: //realtime news headlines
							$currentColWrap.getSampleContent( 'sample-realtime-news-data' );
						break;
						case 2: //trending people
							$currentColWrap.getSampleContent( 'sample-trending-people-data' );
						break;
						case 3: //mini chart
							$currentColWrap.getSampleContent( 'sample-chart-data' );
							module.loadChart( dateRangeData );
						break;
						case 4: //video carousel
							$currentColWrap.getSampleContent( 'sample-video-data' );
							module.setupVideoCarousel( dateRangeData );
						break;						
						case 5: //trending companies
							$currentColWrap.getSampleContent( 'sample-trending-companies-data' );
						break;
						case 6: //mini regional map
							$currentColWrap.getSampleContent( 'sample-map-data' );
							module.loadRegionalMap( );
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
		
		module.loadChart = function( dateRangeData ) {
			
			if( !dateRangeData ) {
				//kill tha machine, this aint working!
				return false;
			}
			
			var	seriesConfig = module.fetchSeriesConfig( dateRangeData.startDate, dateRangeData.endDate, dateRangeData.rangeDays ),
				chartOptions = $.dj.module.summary.chartOptions;

			//add the series data to the dateRangeData
			dateRangeData.seriesData = [];
			
			for( var i = 0; i < seriesConfig.length; i++ ) {
				dateRangeData.seriesData[i] = seriesConfig[i].data;
			}
				
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
							
							module.$el.data( 'selectedDateRangeData', dateRangeData );
							

			            }
					}
				},
				
				plotOptions: {
			        series: {
			            events: {
			                click: function(event) {
				
								var	selectedDate	=	module.$el.data('tooltipX'),
									dateRangeData	=	module.getDateRangeData( Highcharts.dateFormat('%B %e, %Y', selectedDate ) + ' - ' + Highcharts.dateFormat('%B %e, %Y', selectedDate ) );

								module.$el.data( 'selectedDateRangeData', dateRangeData );
			
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

		module.setupVideoCarousel = function( ) {
			
			var $pageWrap	= $('.carousel-pages', module.$el ),
				$page		= $pageWrap.children(),
				$pageCount	= $page.size(),
				$pageWidth	= $page.outerWidth();
			
			$pageWrap.css( {
					width: ( $pageWidth * $pageCount ) + "px"
			} );
				
			$page.css( {
				display: 'block'
			} );
				
			$pageWrap.cycle( {
				fx:     'scrollHorz', 
				prev:   $('.carousel-prev', module.$el), 
				next:   $('.carousel-next', module.$el),
				after:   function ( curr, next, opts) {

					var index = opts.currSlide;

					if (index == 0) {
						$('.carousel-prev', module.$el).hide();
						$('.disabled-btn', module.$el).addClass('disabled-btn-prev');
					} else {
						$('.carousel-prev', module.$el).show();
						$('.disabled-btn', module.$el).removeClass('disabled-btn-prev');
					}

					if (index == opts.slideCount - 1) {
						$('.carousel-next', module.$el).hide();
						$('.disabled-btn', module.$el).addClass('disabled-btn-next');
					} else {
						$('.carousel-next', module.$el).show();
						$('.disabled-btn', module.$el).removeClass('disabled-btn-next');
					}

				},
				timeout: 0 
			});
			
		};

		module.loadRegionalMap = function( ) {
	
			//DEV-NOTE: proof of concept for loading the regional mp
			$( ".mini-regional-map", module.$core ).regionalMap({
				width: 300,
				height: 161,
				showSidebar:false,
				showTextLabels:false,
				circleMaxRadius: 20,
				circleMinRadius: 2,
				totalArticleVolume:1312,
				regions:{
					na:{
						articleVolume: 235,
						variationPercentage: 5
					},
					ca:{
						articleVolume: 160,
						variationPercentage: 6
					},
					sa:{
						articleVolume: 208,
						variationPercentage: 4
					},
					eur:{
						articleVolume: 167,
						variationPercentage: 8
					},
					me:{
						articleVolume: 140,
						variationPercentage: 2
					},
					asi:{
						articleVolume: 260,
						variationPercentage: 1
					},	
					aus:{
						articleVolume: 142,
						variationPercentage: 7
					}					
				}
			});
		
			
		};
		
		// Run initializer
		module.init();
		
	};
	
	$.dj.module.summary.defaultOptions = {
		
		chartWrapSelector:		".summary-mini-chart",
		//DEV-TODO: Update this default date range option option
		chartDateRange: 		"December 1, 2010 - December 31, 2010"

	};
	
	$.dj.module.summary.chartOptions = {

		chart: {
			defaultSeriesType: 'line',
			height: 150,
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
			tickInterval: 48 * 60 * 60 * 1000, // two days
			labels: {
				enabled: false
			},
			gridLineDashStyle: 'shortdash',
			gridLineWidth: 1
		},

		yAxis: [{
			title: {
				text: null
			},
			labels: {
				enabled: false
			},
			gridLineWidth: 0
		},{
			title: {
				text: null
			},
			alternateGridColor: '#f1f3f8',
			gridLineWidth: 0,
			labels: {
				enabled: false
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
				lineWidth: 2,
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
	
	$.fn.dj_module_summary = function(options){
		return this.each(function(){
			(new $.dj.module.summary(this, options));
		});
	};
	
	// This function breaks the chain, but returns
	// the dj.module.companyProfile if it has been attached to the object.
	$.fn.getdj_module_summary = function(){
		this.data("dj.module.summary");
	};
	
})(jQuery);

$(function() {

	$('.summary-module', $('#dashboard')).dj_module_summary();

} );