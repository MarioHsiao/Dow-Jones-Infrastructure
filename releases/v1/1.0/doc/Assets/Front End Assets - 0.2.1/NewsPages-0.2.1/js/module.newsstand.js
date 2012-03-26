/*

	-- -- -- -- -- -- --
	Description: NewsStand Profile Module Plugin and Functions
	Version: 0.2
	Last Update: 01/17/2011
	Authors: Ron Edgecomb II
			 Philippe Arcand
			 Ryan Schoch
	Company: Dow Jones & Company Inc.
	Copyright 2010 Dow Jones & Company Inc.
	
	Dependencies
	 - js/libs/jquery-1.4.3.min.js
	 - js/dashboard.js
	-- -- -- -- -- -- --
	
*/
var popupVisible = false;

(function($){

	if( !$.dj.module ){
		$.dj.module = new Object();
	};
	
	$.dj.module.newsStand = function( el, options ){
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
		module.$el.data("dj.module.newsStand", module );
		
		module.init = function(){
			
			module.options = $.extend( {}, $.dj.module.newsStand.defaultOptions, options);
			
			module.$el.delegate( '.news-ticker-item, .source-carousel-item', 'click', function() {
				
				var $popupLink = $(this);

				$popupLink.popup({
					
					width  : 300,
					height : 220,					
					title  : $popupLink.find("h3").text(),
					body   : '',
					
					open   : function( panel ) {
						
						popupVisible = true;
						console.log();
						$popupLink.parents('.scroll').smoothDivScroll("stopAutoScroll");
						
						panel.update({state:'dj-loading'});

						setTimeout( function() {
							
							panel.update({
								body  : $("#sample-popup-data").html(),
								state : 'dj-loaded'
							});
							
						}, 1000 );
						
					},
					
					close  : function( panel ) {
						
						popupVisible = false;
						if( $popupLink.parents('.scroll').hasClass("news-stand-ticker") ){
							$popupLink.parents('.scroll').smoothDivScroll("startAutoScroll");
						}
												
					}
				});
				
				return false;
				
			});

			//news ticker carousel js
			$( '.news-stand-ticker', module.$el ).smoothDivScroll( {
				scrollableArea		: ".news-ticker",
				scrollWrapper		: ".news-stand-ticker-wrap",
				autoScroll			: "always",
				autoScrollDirection	: "endlessloopright",
				autoScrollStep		: 1,
				autoScrollInterval	: 15
			} );
					
			module.$el.delegate( '.news-stand-ticker', 'mouseover', function(){
				
				$( '.news-stand-ticker', module.$el ).smoothDivScroll( "stopAutoScroll" );
				
			} );
			
			module.$el.delegate( '.news-stand-ticker', 'mouseout', function(){
				
				if( popupVisible == false ){
					
					$( '.news-stand-ticker', module.$el ).smoothDivScroll("startAutoScroll");
										
				}
				
			});
			
			//news source carousel js
			$(function() {
				
				$( '.news-stand-source-carousel', module.$el ).smoothDivScroll( {
					scrollableArea			: ".source-carousel",
					scrollWrapper			: ".source-carousel-wrap",
					scrollingHotSpotLeft	: ".scroll-source-left",
					scrollingHotSpotRight	: ".scroll-source-right",
					visibleHotSpots			: "always",
					mouseDownSpeedBooster	: 1
				} ).smoothDivScroll("stopAutoScroll");				
			
			});
			
		};
		
		module.init();
	};
	
	$.fn.dj_module_newsStand = function(options){
		return this.each(function(){
			(new $.dj.module.newsStand(this, options));
		});
	};
	
	// This function breaks the chain, but returns
	// the dj.module.newsStand if it has been attached to the object.
	$.fn.getdj_module_newsStand = function(){
		this.data("dj.module.newsStand");
	};
	
})(jQuery);

$(function() {

	$('.news-stand-module', $('#dashboard'))
	
		.dj_module_newsStand()
	
		//DEV-NOTE: begin - proof of concept for inital module load
		.addAction( 'module/initialize', function( $module, $header, $edit, $core ) {
			
			//DEV-NOTE: these options should be configured in production version
			module = $module.data( "dj.module" );			
			module.options.disablePagination = true;
			module.options.disableAdjustHeight = true;			

			var $moduleCol		= $('.module-col', $core),
				$moduleColWrap	= $('.module-col-wrap', $moduleCol),
				i = 0;
			
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
				
					$currentColWrap.getSampleContent( 'sample-data', true );
					$currentCol.unblock();					
					setTimeout( loadCol, 500 );
				
				}
			
			})();
		
		}, 1)
		//DEV-NOTE: end - proof of concept for inital module load
		
		//DEV-NOTE: begin - proof of concept for module reload
		.addAction( 'module/reload', function( event, $module, $header, $edit, $core ) {

			var $moduleCol		= $('.module-col', $core),
				$moduleColWrap	= $('.module-col-wrap', $moduleCol),
				i = 0;
			
			$core.block( );
			
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
					
					$currentColWrap.getSampleContent();
					$currentCol.unblock();					
					setTimeout( loadCol, 500 );
					
				} else {
					
					$core.unblock( );
					module.adjustHeight();
					module.init.pagination();
					
				}
				
			})();
	
        });
		//DEV-NOTE: end - proof of concept for module reload	

} );