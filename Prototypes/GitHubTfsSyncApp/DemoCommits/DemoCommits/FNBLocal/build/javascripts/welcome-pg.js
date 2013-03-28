jQuery(function($) {
    
    // call placeholder.js
    $('input, textarea').placeholder();
    
    // Choose Existing
    $('.build-option').on("click", ".dropdown-menu > li > a", function(){
    	var myText = $(this).text(),
    			$myBox = $(this).parents(".btn-group").find(".dropdown-toggle");
	    $myBox.children("span").text(myText);
	    $myBox.blur();
    });
    
});
