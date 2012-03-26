jQuery(function($) {

	//DEV-NOTE: begin - proof of concept for basic edit options
    $.dj.addAction( 'module/initialize', function( event, $module, $header, $edit, $content ) {
		
        var $buttons = $( '.dc_item a', $header );
        
        $module.delegate( 'form', 'submit', function() {
	
            $( '.settings', $buttons ).click();
            $('.reload', $buttons).click();
            
            return false;

        });

    } );
	//DEV-NOTE: end - proof of concept for basic edit options

});