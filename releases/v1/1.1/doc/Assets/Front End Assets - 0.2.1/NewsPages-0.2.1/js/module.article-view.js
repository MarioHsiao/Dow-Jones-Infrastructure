/*
 * DEV-NOTE: This is only an example of how you can trigger the article view.	
*/
 
$(document).ready(function() {
	
	$(".article-view-trigger").fancybox({
		'type'				: 'inline',
		'transitionIn'		: 'fade',
		'transitionOut'		: 'fade',
		'autoScale'			: 'false',
		'autoDimensions'	: 'false',
		'padding'			: '0',
		'margin'			: '0',
		'overlayColor'		: '#000',
		'overlayOpacity'	: '0.6'
	});
	
});