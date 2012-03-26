/*
 * jQuery labeledSlider
 * 
 * Author: Philippe Arcand
 * Copyright 2011 Dow Jones & Company Inc.
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 */
(function( $, undefined ) {
    
	$.widget("ui.labeledSlider", {
		options: {
			scrollPane:'',
			scrollContent:''
		},
		
		/**
		 * Plugin Initialization. (Executed automatically)
		 */
		_init: function(){
			var self = this,
			el = self.element;
			o = self.options;

			var scrollPane = $(o.scrollPane),
			scrollContent = $(o.scrollContent);
			
			var contentwidth = 0;
			$(scrollContent).find(".scroll-content-group").each(function(){
				contentwidth+= $(this).outerWidth();
			});
			
			$(scrollContent).width(contentwidth);
			//build slider
			var scrollbar = $(self.element).slider({
				create: function(event, ui) {
					$(o.scrollPane).find('.scroll-content-group').each(function(){
						groupX = ($(this).children().first().position().left);
						contentWidth = $(this).parent().width();
						scrollBarWidth = $(self.element).parent().width();
						var marginVal = (Math.ceil((groupX*scrollBarWidth)/contentWidth));
						var label = ('<span class="scrollbar-label" style="margin-left:'+marginVal+'px">'+$(this).attr('groupname')+'</span>');
						$('.scrollbar-labels').append(label);
					});
					
					//$(this).find('.scrollbar-labels').prependTo($(this).parent())	
				},
				slide: function( event, ui ) {
					if ( scrollContent.width() > scrollPane.width() ) {
						scrollContent.css( "margin-left", Math.round(
							ui.value / 100 * ( scrollPane.width() - scrollContent.width() )
						) + "px" );
					} else {
						scrollContent.css( "margin-left", 0 );
					}
				}
			});
			
			//append icon to handle
			var handleHelper = scrollbar.find( ".ui-slider-handle" )
			.mousedown(function() {
				scrollbar.width( handleHelper.width() );
				$('.scrollbar-labels').css("left",((handleHelper.width()-$(scrollbar).parent().width())/2)+'px');
				
			})
			.mouseup(function() {
				scrollbar.width( "100%" );
				$('.scrollbar-labels').css("left",'0px');
	
			})
			.append( "<span class='ui-icon ui-icon-grip-dotted-vertical'></span>" )
			.wrap( "<div class='ui-handle-helper-parent'></div>" ).parent();
			
			//change overflow to hidden now that slider handles the scrolling
			scrollPane.css( "overflow", "hidden" );
			
			//size scrollbar and handle proportionally to scroll distance
			function sizeScrollbar() {
				var remainder = scrollContent.width() - scrollPane.width();
				var proportion = remainder / scrollContent.width();
				var handleSize = scrollPane.width() - ( proportion * scrollPane.width() );
				scrollbar.find( ".ui-slider-handle" ).css({
					width: handleSize,
					"margin-left": -handleSize / 2
				});
				handleHelper.width("").width( scrollbar.width() - handleSize );
			}
			
			//reset slider value based on scroll content position
			function resetValue() {
				var remainder = scrollPane.width() - scrollContent.width();
				var leftVal = scrollContent.css( "margin-left" ) === "auto" ? 0 :
					parseInt( scrollContent.css( "margin-left" ) );
				var percentage = Math.round( leftVal / remainder * 100 );
				scrollbar.slider( "value", percentage );
			}
			
			//if the slider is 100% and window gets larger, reveal content
			function reflowContent() {
					var showing = scrollContent.width() + parseInt( scrollContent.css( "margin-left" ), 10 );
					var gap = scrollPane.width() - showing;
					if ( gap > 0 ) {
						scrollContent.css( "margin-left", parseInt( scrollContent.css( "margin-left" ), 10 ) + gap );
					}
			}
			
			//change handle position on window resize
			$( window ).resize(function() {
				/*resetValue();
				sizeScrollbar();
				reflowContent();*/
			});
			//init scrollbar size
			//sizeScrollbar();
			setTimeout( sizeScrollbar, 10 );//safari wants a timeout	
		}
		
	});
})( jQuery );

/**
 *	Prevent Event Propagation (aka Event Bubbling)
 *
 *	@param	e	Event Object
 */
function preventEventPropagation(e){
	if (e && e.stopPropagation) //if stopPropagation method supported
		e.stopPropagation()
	else
		event.cancelBubble=true
}					