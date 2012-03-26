/*

	-- -- -- -- -- -- --
	Description: Top News Module Functions
	Version: 0.1
	Last Update: 01/17/2011
	Author: Ron Edgecomb II
	Company: Dow Jones & Company Inc.
	Copyright 2010 Dow Jones & Company Inc.
	
	Requires
	 - js/libs/jquery-1.4.3.min.js
	 - js/dashboard.js
	 - js/module.js
	-- -- -- -- -- -- --
	
*/
$(function() {

	$('.top-news-module', '#dashboard')
	
		//DEV-NOTE: begin - proof of concept for inital module load
		.addAction( 'module/initialize', function( $module, $header, $edit, $core ) {
			
			//DEV-NOTE: these options should be configured in production version
			module = $module.data( "dj.module" );			
			module.options.disablePagination = true;

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
				
					switch( i ) {
						case 1:
							$currentColWrap.getSampleContent( 'sample-first-col-data' );						
						break;
						case 2:
							$currentColWrap.getSampleContent( 'sample-second-col-data' );
						break;
						case 3:
							$currentColWrap.getSampleContent( 'sample-third-col-data' );
						break;						
					}
					
					$currentCol.unblock();					
					setTimeout( loadCol, 500 );
				
				}
			
			})();
		
		}, 1);
		//DEV-NOTE: end - proof of concept for inital module load
	
});