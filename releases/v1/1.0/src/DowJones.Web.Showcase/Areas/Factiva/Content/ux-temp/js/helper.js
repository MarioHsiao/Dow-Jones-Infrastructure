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

	/**
	 * Loads sample module data if available
	 *
	 * @param mixed options File name, array of file names or JSON object of options.
	 * @return bool Returns true, just because thats some strict JS, yo. Or false if options is not defined.
	*/
	$.dj.include = function( options ) {
		
		var el = this;
		
		var defaultOptions = {
		
			basePath		: $.dj.basePath || '',
			useProjectPath	: false,
			files			: new Array(),
			target			: 'body',
			replaceTarget	: false,
			callback		: null
			
		};
		
		if( !options )
			return false;
		
		if( 'string' === typeof ( options ) ) {
			//just a file name....

			defaultOptions.files.push( options );
			options = defaultOptions;
			
		} else if ( $.isArray( options ) ) {
			//an array of file names....

			defaultOptions.files = options;			
			options = defaultOptions;
			
		} else if( $.isPlainObject( options) ) {
			//an object containg file names and options....
			
			options = $.extend( {}, defaultOptions, options );
			
		} 

		for( var i in options.files ) {

			var file = ( ( options.useProjectPath )?$.dj.projectPath:options.basePath ) + options.files[i];
			
			$.dj.fileExists( file );
				
			switch( $.dj.getFileType( file ) ) {
				
				case 'js':
					$.dj.getScript( file, options );
				break;

				case 'css':
					$.dj.getCSS( file, options );
				break;

				default: //html, text, etc.
					$.dj.getHTML( file, options );
				break;
				
			}
		
		}
		
	};
	
	/**
	 * Returns the file type of the file passed in
	 *
	 * @param string file File name.
	 * @return string fileType Currently supports css, js, html and text.
	*/
	$.dj.getFileType = function( fileName ) {
		
		var fileType = 'text',
			filePieces = fileName.split('.');
		
		switch( filePieces[ filePieces.length - 1 ] ) {
			
			case 'css':
			case 'less':
				fileType = 'css';
			break;
			case 'js':
				fileType = 'js';
			break;
			case 'html':
				fileType = 'html';
			break;
			default:
				fileType = 'text';
			break;				
		}
		
		return fileType;			
		
	};
	
	/**
	 * Loads javascript file.
	 *
	 * @param string file File name.
	 * @param object options Options from include function.
	 * @return bool Returns true, all day every day.
	*/
	$.dj.getScript = function( file, options ){
		
		$.getScript( file );
		
		return true;
				
	};

	/**
	 * Loads css file.
	 *
	 * @param string file File name.
	 * @param object options Options from include function.
	 * @return bool Returns true, all day every day.
	*/
	$.dj.getCSS = function( file, options ){
		
		$("head").append( $("<link>").attr( {
			type	: 'text/css',
			rel		: 'stylesheet',
			href	: file
		} ) );
		
		return true;
		
	};

	/**
	 * Loads html file.
	 *
	 * @param string file File name.
	 * @param object options Options from include function.
	 * @return bool Returns true, all day every day.
	*/
	$.dj.getHTML = function( file, options ){
	
		$.ajax({
			type		: 'GET',
			url			: file,
			success		: function( html, status, request ) {

				if( options.replaceTarget ) 
					$(options.target).html( html );
				else
					$(options.target).append( html );
				
			},
			error		: function( request, status, error ) {
				var msg = '<p class="notice error">Sorry, there was an error loading <strong>' + file + '</strong>:<br/><i>';
				$(options.target).append( msg + error + '</i></p>' );
			},
			dataType	: "text"
		});
		
		return true;		
	
	};

	/**
	 * Checks to see if a file exisits.
	 *
	 * @param string file File name.
	 * @return bool Returns true if file exisits.
	*/
	$.dj.fileExists = function( file ){
		
		var fileExists = false;
	
		$.ajax( {
			url		: file,
			type	: 'HEAD',
			success : function(){
				fileExists = true;
			},
			cache	: false,
			async	: false
		} );
		
		return fileExists;
		
	};

	/**
	 * Locates a template file by first checking to see if the file exisits at a 
	 * project level before getting the root based equivalent.
	 *
	 * @param string file File name.
	 * @return string Returns the appropriate file path.
	*/
	$.dj.locateTemplate = function( file, projectPath ){

		projectPath = projectPath || $.dj.projectPath;
				
		var projectFile = projectPath + file;
		return ( ( $.dj.fileExists( projectFile ) )?projectFile:file );
		
	};

})(jQuery);