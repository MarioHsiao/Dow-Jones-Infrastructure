(function($){
	
	if(!$.dj){
		$.dj = new Object();
	};
	
	$.dj.dashboard = function( el, options ){

		var dashboard = this;

		// Access to jQuery and DOM versions of element		
		dashboard.$el = $(el);
		dashboard.el = el;
		
		// Add a reverse reference to the DOM object
		dashboard.$el.data("dj.dashboard", dashboard);
		
		// Key module components
		dashboard.$header	= $( '#header' ),
		dashboard.$footer	= $( '#footer' );
		
		dashboard.init = function(){
			
			dashboard.options = $.extend( {}, $.dj.dashboard.defaultOptions, options );
			
			dashboard.$el.doAction( 'dashboard/initialize' );
			
			dashboard.searchInit();

		};
		
		dashboard.searchInit = function() {
		
			//TODO: ugh, searchbox need an id....
			
			//search field
			$('#s').focus( function() {

				var $searchBox = $(this);

				if( !$searchBox.data( 'default' ) || $searchBox.data( 'default' ) == $searchBox.val() ) {
					$searchBox.data( 'default', $searchBox.val() );
					$searchBox.val( '' );
				}

				$searchBox.addClass( 'focus' );

				dashboard.$el.doAction( 'header/search/focus' );

			} ).blur( function() {

				var $searchBox = $( this );

				if( !$searchBox.val() ) {

					$searchBox.removeClass( 'focus' );		
					$searchBox.val( $searchBox.data( 'default' ) );

				}

				dashboard.$el.doAction( 'header/search/blur' );

			} );			
			
		};
		
		// Run initializer
		dashboard.init();
		
	};
	
	$.dj.dashboard.defaultOptions = {

	};
	
	$.fn.dj_dashboard = function( options ){
		
		options = options || {};
		
		return this.each( function(){
			( new $.dj.dashboard( this, options ) );
		} );
		
	};

})(jQuery);

$(function() {
	
	$.dj.addAction( 'page/init', function() {

		$('#dashboard').dj_dashboard();
		
	}, 1);
	
});
