/*

	-- -- -- -- -- -- --
	Description: Global javascript functions
	Version: 0.1
	Last Update: 12/23/2010
	Author: Ron Edgecomb II
	Company: Dow Jones & Company Inc.
	Copyright 2010 Dow Jones & Company Inc.
	
	Dependencies
	 - js/libs/jquery-1.4.3.min.js
	-- -- -- -- -- -- --
	
*/
	
(function($) {

    // <%= Token("w00t") %>
    
	if(!$.dj){
		$.dj = new Object();
	};

	/**
	 * Hooks a function on to a specific action at a global level.
	 *
	 * Actions launch at specific points during script execution/page load. functions hooked to the action will execute by the order of specified priority or the order in which they were added to the action. the concept is identical to wordpress actions
	 *
	 * @param string tag The name of the action(s) to which the functionToAdd is hooked. Multiple actions can be passed in separated by a space.
	 * @param callback functionToAdd The name of the function you wish to execute.
	 * @param int priority optional. Used to specify the order in which the callback associated with a particular action are executed (default: Lower numbers correspond with earlier execution, and functions with the same priority are executed in the order in which they were added to the action. )
	 * @param mixed arguments optional. Argument(s) to be passed to the callback function.
	*/
	$.dj.addAction = function( tag, functionToAdd, priority, args ) {
		
		if( !$.dj.djAction ){
			$.dj.djAction = new Array();
		};
	
		priority = priority || 10;
		args = args || [];
		
		tag = tag.split( ' ' );
		
		for( var i in tag ) {
		
			if( !$.isArray( $.dj.djAction[ tag[i] ] ) )
				$.dj.djAction[ tag[i] ] = new Array();
		
			$.dj.djAction[ tag[i] ].push( { 
				method:		functionToAdd, 
				args:		args,
				priority: 	priority
			} );

		}

	};

	/**
	 * Executs function(s) associated to a custom action at a global level.
	 *
	 * @param string tag The name of the action to trigger.
	 * @param mixed arguments optional. Argument(s) to be passed to the callback function.
	 * @return bool false if custom event tag does not exist, otherwise true
	*/
	$.dj.doAction = function( tag, args ) {
	
		if( !$.dj.djAction || !$.isArray( $.dj.djAction[ tag ] ) )
			return false;

		var extraArgs = ( arguments.length > 2 )?arguments.shift().shift():[];

		if ( $.isArray( args ) )
			args = $.merge( args, extraArgs );
		else
			args = $.merge( new Array( args ), extraArgs );

		$.dj.djAction[ tag ].sort( function( a, b ) {
			
			if( a.priority < b.priority ) return -1;
			else if( a.priority == b.priority ) return 0;
			else return 1;	
		
		} );
		
	  	for( var i in $.dj.djAction[ tag ] ) {
		
			var actionArgs = args;
		
			if( 'undefined' != $.dj.djAction[ tag ][ i ].args ) {
				$.dj.djAction[ tag ][i].args = ( !$.isArray( $.dj.djAction[ tag ][i].args ) )?new Array( $.dj.djAction[ tag ][i].args ):$.dj.djAction[ tag ][i].args;
				$.dj.djAction[ tag ][i].actionArgs = $.merge( $.dj.djAction[ tag ][i].args, args )
			}
			
			if( $.isFunction( $.dj.djAction[ tag ][i].method ) )
				$.dj.djAction[ tag ][i].method.apply( null, $.dj.djAction[ tag ][i].actionArgs );

			
		}

	};
	
	/**
	 * Hooks a function on to a specific action at the element level.
	 *
	 * Actions launch at specific points during script execution/page load. functions hooked to the action will execute by the order of specified priority or the order in which they were added to the action. the concept is identical to wordpress actions
	 *
	 * @param string tag The name of the action(s) to which the functionToAdd is hooked. Multiple actions can be passed in separated by a space.
	 * @param callback functionToAdd The name of the function you wish to execute.
	 * @param int priority optional. Used to specify the order in which the callback associated with a particular action are executed (default: Lower numbers correspond with earlier execution, and functions with the same priority are executed in the order in which they were added to the action. )
	 * @param mixed arguments optional. Argument(s) to be passed to the callback function.
	*/
	$.fn.addAction = function( tag, functionToAdd, priority, args ) {
		
		tag = tag.split( ' ' );		
		
		return this.each( function( ){
			
			if( !this.djAction ){
				this.djAction = new Array();
			};

			this.priority = priority || 10;
			this.args = args || [];

			for( var i in tag ) {

				if( !$.isArray( this.djAction[ tag[i] ] ) )
					this.djAction[ tag[i] ] = new Array();

				this.djAction[ tag[i] ].push( { 
					method:		functionToAdd, 
					args:		this.args,
					priority: 	this.priority
				} );

			}
			
		} );	
	
	};
	
	/**
	 * Executs function(s) associated to a custom action at the element level.
	 *
	 * @param string tag The name of the action to trigger.
	 * @param mixed arguments optional. Argument(s) to be passed to the callback function.
	 * @param bool trigger global. Whether or not to trigger the global acation, true by default.
	 * @return bool false if custom event tag does not exist, otherwise true
	*/
	$.fn.doAction = function( tag, args, triggerGlobal ) {

		var extraArgs = ( arguments.length > 3 )?arguments.shift().shift().shift():[];

		if ( $.isArray( args ) )
			args = $.merge( args, extraArgs );
		else
			args = $.merge( new Array( args ), extraArgs );
		
		if( triggerGlobal )
			$.dj.doAction( tag, args );
		
		return this.each( function( ){
		
			if( !this.djAction || !$.isArray( this.djAction[ tag ] ) )
				return false;

			this.djAction[ tag ].sort( function( a, b ) {
			
				if( a.priority < b.priority ) return -1;
				else if( a.priority == b.priority ) return 0;
				else return 1;	
		
			} );

		  	for( var i in this.djAction[ tag ] ) {
		
				var actionArgs = args;
		
				if( 'undefined' != this.djAction[ tag ][ i ].args ) {
					this.djAction[ tag ][i].args = ( !$.isArray( this.djAction[ tag ][i].args ) )?new Array( this.djAction[ tag ][i].args ):this.djAction[ tag ][i].args;
					this.djAction[ tag ][i].actionArgs = $.merge( this.djAction[ tag ][i].args, args )
				}
				
				if( $.isFunction( this.djAction[ tag ][i].method ) )
					this.djAction[ tag ][i].method.apply( null, this.djAction[ tag ][i].actionArgs );
			
			}

		} );
		
	};

	$.fn.equalHeights = function(minHeight, maxHeight) {
	
		tallest = (minHeight) ? minHeight : 0;
		this.each(function() {
			
			if( $.support.minHeight ) {
				$(this).css( 'minHeight', 'auto' );
			} else {
				$(this).height( 'auto' );
			}
			
			if($(this).height() > tallest) {
				tallest = $(this).height();
			}
		});
		
		if((maxHeight) && tallest > maxHeight) tallest = maxHeight;
		return this.each(function() {
			if( $.support.minHeight ) {
				$(this).css( 'minHeight', tallest );
			} else {
				$(this).height(tallest);
			}
		});
	}

    $.fn.fadeIn = function(speed, callback) {
        return this.animate({opacity: 'show'}, speed, function() {
                if ( $.browser.msie )
                {
                       this.style.removeAttribute('filter');
                }
                if ( $.isFunction(callback) )
                {
		callback.call(this);
                }
        });
    };

    $.fn.fadeOut = function(speed, callback) {
        return this.animate({opacity: 'hide'}, speed, function() {
                if ( $.browser.msie )
                {
                      this.style.removeAttribute('filter');
                }
                if ( $.isFunction(callback) )
                {
		callback.call(this);
                }
        });
    };

    $.fn.fadeTo = function(speed, to, callback) {
        return this.animate({opacity: to}, speed, function() {
                if ( to == 1 && $.browser.msie )
                {
                     this.style.removeAttribute('filter');
                }
                if ( $.isFunction(callback) )
                {
	         callback.call(this);
	     }
        });
    };

})(jQuery);