/*
 * jQuery popupBalloon
 * 
 * Author: Philippe Arcand
 * Copyright 2011 Dow Jones & Company Inc.
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 *
 */
 (function( $, undefined ) {
    
	$.widget("ui.popupBalloon", {
		options: {
			width: 400,
			height:130,
			title: '',
			content: '',
			onHide: function(){}
		},
		
		/**
		 * Plugin Creation. (Executed automatically)
		 */
		_create: function(){
			$.ui.popupBalloon.instances.push(this.element);
		 
		},
		
		/**
		 * Plugin Initialization. (Executed automatically)
		 */
		_init: function(){
			var self = this,
			el = self.element,
			o = self.options;	
			
			self.show();
		},
		
		/**
		 * Build the menu based on the options.items object
		 */
		show: function(){
			var self = this,
			el = self.element,
			o = self.options;	
			
			// Hide previously opened instances
			self.hide();
			
			// Create popup
			var popbox = $('<div class="popup-balloon"><div class="balloon-arrow"></div><div class="content"></div></div>');
			popbox.data("controller",el);
			
			// Add Title
			if(o.title){
				$(popbox).addClass('with-title');
				$(popbox).prepend('<div class="header"><div class="title ellipsis">'+o.title+'</div></div>');
			}
			
			// Add Content
			if(o.content){
				$(popbox).find('.content').html(o.content);
			}
			
			// Calculate Size
			var headerHeight = $(popbox).find('.header').height();
			var contentHeight = o.height-headerHeight;
			
			$(popbox).find('.content, .title').width(o.width);
			$(popbox).find('.content').height(contentHeight);
			
			$('body').append(popbox);
			
			// Position & Show Element			
			self.positionElement(popbox);

			// Bind Mouse Events
			$(popbox).bind('mousedown', function(e){
				e.stopPropagation();
			});
			
			window.setTimeout(function(){
				$("body").bind('mousedown', function(e){
					self.hide();
					e.stopPropagation();
				});
			},20);
			
		},
		
		/**
		*  Hide (remove) popbox elements from the page.
		*/
		hide: function(){
			var self = this,
			el = self.element,
			o = self.options;
			
			// Remove all menu instances
			$('.popup-balloon').fadeOut(300, function(){
				$(this).remove();
			});
			$("body").unbind('mousedown');
			
			// Execute callback function
			o.onHide();
		},
		
		/**
		*  Position the popbox element
		*
		*  @param	elem	Element
		*/
		positionElement: function(elem){
			var self = this,
			el = self.element,
			o = self.options;
			
			var arrowWidth = $(elem).find('.balloon-arrow').width();
			var arrowHeight = $(elem).find('.balloon-arrow').height();
			var elemHeight = $(elem).height();
						
			// Center arrow vertically
			$(elem).find('.balloon-arrow').css('top', (elemHeight/2)-(arrowHeight/2)+'px');
			
			// Center balloon vertically
			$(elem).css('top', (($(el).offset().top+($(el).height()/2))-(elemHeight/2))+'px');
			
			// Align element to the right
			$(elem).find('.balloon-arrow').addClass('arrow-left');
			$(elem).css('left', ($(el).offset().left+arrowWidth+$(el).width())+'px');
			
			$(elem).css({'display':'block',opacity:0});
			// If the element appears outside of the window, align it to the left
			if(($(elem).offset().left+$(elem).width())>$(window).width()){
				$(elem).find('.balloon-arrow').removeClass('arrow-left').addClass('arrow-right');
				$(elem).css('left', (($(el).offset().left)-$(elem).width()-arrowWidth)+'px');
				$(elem).animate({opacity: 1, 'left':'+=20'},300)
			}
			else{
				$(elem).animate({opacity: 1, 'left':'-=20'},300)
			}
			
			// Animate the element
			//$(elem).fadeIn(300);
		}
	})
	
	$.extend($.ui.popupBalloon, {
		instances: []
	});
})( jQuery );