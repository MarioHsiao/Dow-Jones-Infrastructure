/*

	-- -- -- -- -- -- --
	Description: Regional Map Module Functions
	Version: 0.1
	Last Update: 01/17/2011
	Author: Philippe Arcand
	Company: Dow Jones & Company Inc.
	Copyright 2010 Dow Jones & Company Inc.
	
	Requires
	 - js/libs/jquery-1.4.3.min.js
	 - js/dashboard.js
	 - js/module.js
	-- -- -- -- -- -- --
	
*/
$(function() {

	$('.regional-map-module', '#dashboard')
	
		//DEV-NOTE: begin - proof of concept for inital module load
		.addAction( 'module/initialize', function( $module, $header, $edit, $core ) {

			//DEV-NOTE: these options should be configured in production version
			module = $module.data( "dj.module" );			
			module.options.disablePagination = true;
			module.options.disableAdjustHeight = true;

			var $regionalMap = $('.regional-map', $core);
			
			$core.block( {
			
				message: '<div class="dj-loading"></div>',
			
				overlayCSS:  { 
					backgroundColor: '#fff'
				}
			
			} );
			
			$regionalMap.regionalMap( {
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
				},
				filters:[
					{
						articleVolume: 72,
						name: 'Aerospace'
					},{
						articleVolume: 90,
						name: 'Automotive'
					},{
						articleVolume: 123,
						name: 'Energy'
					},{
						articleVolume: 68,
						name: 'Internet'
					},{
						articleVolume: 17,
						name: 'Marketing'
					},{
						articleVolume: 49,
						name: 'Retail'
					},{
						articleVolume: 24,
						name: 'Telecomunications'
					},	
				],
				onRegionSelect: function(e){},
				onFilterChange: function(e){
					// EXAMPLE: Change values dynamically
					var o = $regionalMap.regionalMap("option");
					
					// Get changed filter's properties (in this example, we retrieve the article volume)
					var articleVolume = $(e.target).attr("articleVolume");
					
					// Update the regions object with random values
					for (var i in o.regions){
						o.regions[i].articleVolume = Math.floor(Math.random()*articleVolume);
					}
					
					// Refresh the map
					$regionalMap.regionalMap("renderMap");	
				}
			});
			
			setTimeout( function() {
				$core.unblock( );
			}, 1000 );
		
		}, 1);
		//DEV-NOTE: end - proof of concept for inital module load
	
});