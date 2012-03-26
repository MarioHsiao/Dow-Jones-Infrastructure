/*

	-- -- -- -- -- -- --
	Description: Temporary helper javascript functions, not for production
	Version: 0.1
	Last Update: 01/15/2011
	Author: Ron Edgecomb II
	Company: Dow Jones & Company Inc.
	Copyright 2010 Dow Jones & Company Inc.
	-- -- -- -- -- -- --
	
*/
	
(function($) {

	if(!$.dj){
		$.dj = new Object();
	};
	
	/**
	 * Generates a random series of data to be used by charting.
	 *
	 * @param int min The minimum integer value in the range of random data. Defaults to 1.
	 * @param int max The maximum integer value in the range of random data. Defaults to 25.
	 * @param int total The total number of integer values to return. Defaults to 10.
	 * @return array data An array of all the integer values.
	*/	
	$.dj.randomChartSeries = function ( min, max, total ) {

		var data	=	[],
			min		=	min || 1,
			max		=	max || 25,
			total	=	total || 10,
			i;

		for( i=0; i<total; i++)
			data.push( $.dj.randomNumber( min, max) );

		return data;

	};

	/**
	 * Generates a random integer value.
	 *
	 * @param int min The minimum integer value for the random number. Defaults to 1.
	 * @param int max The maximum integer value for the random number. Defaults to 25.
	 * @return int The random integer value generated.
	*/
	$.dj.randomNumber = function( min, max ) {

		var min		=	min || 1,
			max		=	max || 25;

		return Math.floor( min + ( Math.random() * ( max - min ) ) ) ;

	};
	

	/**
	 * Loads sample module data if available
	 *
	 * @param mixed selector The id selector of the sample content.
	 * @return string The html from within the referenced id selector.
	*/
	$.fn.getSampleContent = function( selector, skipIfHasContent ) {
		
		var html = '';
		
		if( selector && selector.jquery ) {
			
			html = selector.html();
			
		} else if( !selector || 'string' === typeof( selector ) ) {
		
			selector = selector || 'sample-data';		
			selector = ( selector.indexOf( '#' ) == 1 )?selector:'#'+selector;
	
			html = $( selector ).html();
	
		}

		return this.each( function( ){
			
			$(this).html( $.trim( $(this).html() ) );
			
	        if( skipIfHasContent && !$(this).is(":empty") )
				return;
				
			$(this).html( html );

		} );

	};

})(jQuery);