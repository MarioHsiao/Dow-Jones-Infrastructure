/*

	-- -- -- -- -- -- --
	Description: Global Module Framework Setup
	Version: 0.1
	Last Update: 01/13/2011
	Author: Ron Edgecomb II, Blago Dachev
	Company: Dow Jones & Company Inc.
	Copyright 2010 Dow Jones & Company Inc.
	
	Dependencies
	 - js/libs/jquery-1.4.3.min.js
	 - js/dashboard.js
	-- -- -- -- -- -- --
	
*/
(function($){
	
	if(!$.dj){
		$.dj = new Object();
	};
	
	$.dj.module = function(el, options){
	
		var module = this;

		module.$el = $(el);
		module.el = el;
		
		// Add a reverse reference to the DOM object
		module.$el.data( "dj.module", module );

		//window reference
		module.$messenger	= $(window);

		// Key module components
		module.$header		= $( '.module-header', module.$el ),
		module.$buttons		= $( '.dc_item', module.$header ),
		
		module.$edit		= $( '.module-edit-options', module.$el ),
		
		module.$core		= $( '.module-core', module.$el ),
		module.$footer		= $( '.module-footer', module.$core );		
		
		module.init = function(){
	
			// get the module options
			module.options = $.extend( {},$.dj.module.defaultOptions, options );

			// initialize the module header buttons
			module.init.headerControls();
			
			// initialize the edit draw for the module
			module.init.actions();

			// trigger action module/initialize, to be hooked into by specific module types
			module.$el.doAction( 'module/initialize', [ module.$el, module.$header, module.$edit, module.$core ] );

		};
		
		module.init.headerControls = function() {
		
			// the settings button, aka, edit draw control
			$( '.settings', module.$buttons ).bind( 'click', function( event ) {

				//DEV-NOTE: doAction is similar to custom events. documented function can be found in global.js
				if ( module.$edit.hasClass('module-edit-options-open') == false )
			        module.$el.doAction( 'module/edit/open', [ event, module.$el, module.$header, module.$edit, module.$core ] );
				else
					module.$el.doAction( 'module/edit/close', [ event, module.$el, module.$header, module.$edit, module.$core ] );

			    return false;
			} );


			// the refresh button, aka, edit draw control			
			$( '.reload', module.$buttons ).bind( 'click', function( event ) {
				
				module.$el.doAction( 'module/reload', [ event, module.$el, module.$header, module.$edit, module.$core ] );
			
			    return false;
						
			} );
			
		};// end - init.headerControls
		
		module.init.actions = function() {
			
			//setup action for module initialization
			module.$el.addAction( 'module/initialize', function( $module, $header, $edit, $core ) {
			
				// equalize the height of columns within each row
				module.adjustHeight();
			
				// setup pagination for module content
				module.init.pagination();
				
			} );			
			
			//setup action for module edit draw open
			module.$el.addAction( 'module/edit/open', function( event, $module, $header, $edit, $core ) {
				
	            $edit.addClass( 'module-edit-options-open' );
	            $core.block( { message: null } );
	
			}, 1 );

			//setup action for module edit draw open
			module.$el.addAction( 'module/edit/close', function( event, $module, $header, $edit, $core ) {
				
	            $edit.removeClass( 'module-edit-options-open' );
	            $core.unblock();
	
			} );
			
		};// end - init.actions
		
		module.init.pagination = function() {
			
			if( module.options.disablePagination )
				return;
			
			var $module = module.$el;
			
			//pagination goodness using jquery cycle
			if( $( '.module-content-page', $module ).size() > 1 ) {

				var $moduleContentPages = $( '.module-content-pages', $module ),
					activePageClass = 'pager-active';

				module.$footer.empty().append( $('<div class="module-pager"></div>') );
				
				//kill any previous cycle instances and inline styles
				$moduleContentPages.cycle('destroy').removeAttr('style').children().removeAttr('style');
				
				$moduleContentPages.cycle( {
					
					timeout 				: 0,
					fx						: 'scrollHorz',
					pager					: $('.module-pager', $module),
					activePagerClass		: activePageClass,
					
					updateActivePagerLink	: function( pager, currSlideIndex ) { 

						$( 'a', pager ).removeClass( activePageClass ).filter( 'a:eq('+currSlideIndex+')' ).addClass( activePageClass );

					},

					pagerClick				: function( idx, slide ) {

					},

					pagerAnchorBuilder		:function( idx, slide ) {
						return '<a class="pager-link pager-'+(idx+1)+'"></a>';
					}
					
				} );

			}
			
		};// end - init.pagination
		
		module.adjustHeight = function() {
			
			if( module.options.disableAdjustHeight )
				return;
				
			//TODO: we need to come up with a better way to qualize heights....			
		    $('.module-row:odd .module-col-wrap', module.$el ).equalHeights();
            $('.module-row:even .module-col-wrap', module.$el ).equalHeights();
			
		};// end - adjustHeight
		
		// Run initializer
		module.init();
	};
	
	$.dj.module.defaultOptions = {
		
		disablePagination	: false, 
		disableAdjustHeight	: false

	};
	
	$.fn.dj_module = function( options ){
		
		options = options || {};
		
		return this.each( function(){
			( new $.dj.module( this, options ) );
		} );
		
	};
	
	// This function breaks the chain, but returns
	// the dj.module if it has been attached to the object.
	$.fn.getdj_module = function(){
		this.data("dj.module");
	};	
	
})(jQuery);

$(function() {
	
	// block ui configuration
	$.blockUI.defaults.css = {
		padding			: 0,
		margin			: 0,
		width			: '30%',
		top				: '40%',
		left			: '35%',
		textAlign		: 'center',
		color			: '#000',
		border			: 'none',
		backgroundColor	: 'transparent',
		cursor			: 'default'
	};
	
	$.blockUI.defaults.overlayCSS = {
		zIndex								: '2000',
		backgroundColor						: '#000',
		opacity								: 0.6,
		cursor								: 'default',
		'border-bottom-left-radius'			: '5px',
		'border-bottom-right-radius'		: '5px',
		'-moz-border-radius-bottomleft'		: '5px',
		'-moz-border-radius-bottomright'	: '5px',
		'-webkit-border-bottom-left-radius'	: '5px',
		'-webkit-border-bottom-right-radius': '5px'
	};
	
	//DEV-NOTE: required, add action to execute at dashboard initialization
	$('#dashboard').addAction( 'dashboard/initialize', function() {

		$('.module').dj_module();
		
		$('#dashboard').sortable({
			items: '.module',
			handle: '.module-header',
			containment: '#dashboard'
		});
		
	}, 1);
	
});