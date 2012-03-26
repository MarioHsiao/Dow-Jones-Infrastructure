/*

	-- -- -- -- -- -- --
	Description: Global javascript functions
	Version: 0.1
	Last Update: 12/23/2010
	Author: Ron Edgecomb II
	Company: Dow Jones & Company Inc.
	Copyright 2010 Dow Jones & Company Inc.
	
	Dependencies
	 - js/libs/jquery-1.4.4.min.js
	 - js/libs/jquery.simplemodal.1.4.1.min.js
	-- -- -- -- -- -- --
	
*/
	
(function($) {

	if(!$.dj){
		$.dj = new Object();
	};

	/**
	 * Opens a modal box
	 *
	 * @param	mixed		The name of the action to trigger.
	 * @return	bool		true if modal box sucessfully opens
	*/
	$.dj.modal = function( options ) {
		
		if(!$.modal)
			return false;

		var modal = false;

		switch( options.type ){
			
			case 'article':
				//FED-TODO: Dyanmically assemble the article view modal html and place in content....
				modal =	$.modal( options.content, options.options );
			break;
			
			default:
				//FED-TODO: Dyanmically assemble the default modal html and place in content....
				modal =	$.modal( options.content, options.options );
			break;			
		}

		return modal;

	};
	
	$.modal.defaults = {
		appendTo: 'body',
		focus: true,
		opacity: 50,
		overlayId: 'dj_modal-overlay',
		overlayCss: {},
		containerId: 'dj_modal-container',
		containerCss: {},
		dataId: 'dj_modal-data',
		dataCss: {},
		minHeight: null,
		minWidth: null,
		maxHeight: null,
		maxWidth: null,
		autoResize: false,
		autoPosition: true,
		zIndex: 1000,
		close: true,
		closeHTML: '',
		closeClass: 'dj_modal-close',
		escClose: true,
		overlayClose: true,
		position: null,
		persist: false,
		modal: true,
		onOpen: null,
		onShow: null,
		onClose: null
	};	
})(jQuery);
$(function() {

	//DEV-NOTE: proof of concept for article view modal
	$( document ).delegate(".article-view-trigger", "click", function( event ){
		
		$.dj.modal( {
			
			type	: 'article',
			content	: function() {
			
				var $modal = $('#dj_modal-article').clone()
				
				//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
				// 162px  = 62px modal gray box border and padding + 40px modal header + 40px modal content margin + 20px extra window padding
				$('#dj_modal-article-content', $modal).height( ( $(window).height() - 162 ) );
				
				return $modal;
				
			}(),
			options	: {
				onShow: function( modal ) {
			
					$('#dj_modal-article-content', '#dj_modal-container').jScrollPane( { 
						showArrows				: true, 
						autoReinitialise		: true,
						verticalDragMaxHeight	: 52,
						verticalGutter			: 10,
						horizontalGutter		: 20
					} );
			
				}
			}
			
		} );
	
		return false;
	});
	
	//DEV-NOTE: proof of concept for delete page modal
	$( document ).delegate(".delete-page-trigger", "click", function( event ){
				
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-delete-page-modal').text(),
					modalContent	= $('.modal-content', '#sample-delete-page-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );

					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					$('#dj_modal-content', $modal).height( 'auto' );
					
					return $modal;
					
			}()
			
		} );
	
		return false;
	});

	//DEV-NOTE: proof of concept for unpublish page modal
	$( document ).delegate(".unpublish-page-trigger", "click", function( event ){
				
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-unpublish-page-modal').text(),
					modalContent	= $('.modal-content', '#sample-unpublish-page-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );

					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					$('#dj_modal-content', $modal).height( 'auto' );
					
					return $modal;
					
			}()
			
		} );
	
		return false;
	});


	//DEV-NOTE: proof of concept for publish page modal
	$( document ).delegate(".publish-page-trigger", "click", function( event ){
	
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-publish-page-modal').text(),
					modalContent	= $('.modal-content', '#sample-publish-page-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );
					
					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					// 140px  = 40px modal header + 40px modal content margin + 30px extra window padding
					$('#dj_modal-content', $modal).height( ( $(window).height() - 110 ) );
					
					return $modal;
					
			}(),
			options	: {
				onShow: function( modal ) {
			
					$('#dj_modal-content', '#dj_modal-container').jScrollPane( { 
						showArrows				: true, 
						autoReinitialise		: true,
						verticalDragMaxHeight	: 52,
						verticalGutter			: 10,
						horizontalGutter		: 20
					} );
			
				}
			}
			
		} );
	
		return false;
	});
	
	//DEV-NOTE: proof of concept for publish page alert modal
	$( document ).delegate(".publish-page-alert-trigger", "click", function( event ){
	
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-publish-page-alert-modal').text(),
					modalContent	= $('.modal-content', '#sample-publish-page-alert-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );
					
					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					// 140px  = 40px modal header + 40px modal content margin + 30px extra window padding
					$('#dj_modal-content', $modal).height( 'auto' );
					
					return $modal;
					
			}()
			
		} );
	
		return false;
	});

	//DEV-NOTE: proof of concept for share page modal
	$( document ).delegate(".share-page-trigger", "click", function( event ){
	
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-share-page-modal').text(),
					modalContent	= $('.modal-content', '#sample-share-page-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );
					
					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					// 140px  = 40px modal header + 40px modal content margin + 30px extra window padding
					$('#dj_modal-content', $modal).height( ( $(window).height() - 110 ) );
					
					return $modal;
					
			}(),
			options	: {
				onShow: function( modal ) {
			
					$('#dj_modal-content', '#dj_modal-container').jScrollPane( { 
						showArrows				: true, 
						autoReinitialise		: true,
						verticalDragMaxHeight	: 52,
						verticalGutter			: 10,
						horizontalGutter		: 20
					} );
			
				}
			}
			
		} );
	
		return false;
	});
	
	//DEV-NOTE: proof of concept for share page modal
	$( document ).delegate(".admin-add-page-trigger", "click", function( event ){
	
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-admin-add-page-modal').text(),
					modalContent	= $('.modal-content', '#sample-admin-add-page-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );
					
					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					// 140px  = 40px modal header + 40px modal content margin + 30px extra window padding
					$('#dj_modal-content', $modal).height( 'auto' );
					
					return $modal;
					
			}()
			
		} );
	
		return false;
	});
	
	//DEV-NOTE: proof of concept for delete page modal
	$( document ).delegate(".admin-delete-page-trigger", "click", function( event ){
				
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-admin-delete-page-modal').text(),
					modalContent	= $('.modal-content', '#sample-admin-delete-page-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );

					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					$('#dj_modal-content', $modal).height( 'auto' );
					
					return $modal;
					
			}()
			
		} );
	
		return false;
	});
	
	//DEV-NOTE: proof of concept for publish page modal
	$( document ).delegate(".admin-publish-page-trigger", "click", function( event ){
	
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-admin-publish-page-modal').text(),
					modalContent	= $('.modal-content', '#sample-admin-publish-page-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );
					
					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					// 140px  = 40px modal header + 40px modal content margin + 30px extra window padding
					$('#dj_modal-content', $modal).height( ( $(window).height() - 110 ) );
					
					return $modal;
					
			}(),
			options	: {
				onShow: function( modal ) {
			
					$('#dj_modal-content', '#dj_modal-container').jScrollPane( { 
						showArrows				: true, 
						autoReinitialise		: true,
						verticalDragMaxHeight	: 52,
						verticalGutter			: 10,
						horizontalGutter		: 20
					} );
			
				}
			}
			
		} );
	
		return false;
	});
	
	//DEV-NOTE: proof of concept for publish page alert modal
	$( document ).delegate(".admin-publish-page-alert-trigger", "click", function( event ){
	
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-admin-publish-page-alert-modal').text(),
					modalContent	= $('.modal-content', '#sample-admin-publish-page-alert-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );
					
					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					// 140px  = 40px modal header + 40px modal content margin + 30px extra window padding
					$('#dj_modal-content', $modal).height( 'auto' );
					
					return $modal;
					
			}()
			
		} );
	
		return false;
	});
	
	//DEV-NOTE: proof of concept for unpublish page modal
	$( document ).delegate(".admin-unpublish-page-trigger", "click", function( event ){
				
		$.dj.modal( {
			
			content	: function() {
				
				var $modal 			= $('#dj_modal').clone(),
					modalTitle		= $('.modal-title', '#sample-unpublish-page-modal').text(),
					modalContent	= $('.modal-content', '#sample-unpublish-page-modal').html();
		
					$('#dj_modal-title', $modal).html( modalTitle );
					$('#dj_modal-content-wrap', $modal).html( modalContent );

					//DEV-NOTE: recommened using in production, adjust height of article content area based on viewable area
					$('#dj_modal-content', $modal).height( 'auto' );
					
					return $modal;
					
			}()
			
		} );
	
		return false;
	});
	
});
