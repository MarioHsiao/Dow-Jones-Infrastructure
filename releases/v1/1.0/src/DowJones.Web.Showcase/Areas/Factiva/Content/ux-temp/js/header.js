$(function(){	
	animateHeader = function(action){
		$("#header-search").fadeOut(200,function(){});	
		$("#product-logo").fadeOut(200,function(){});	

		if(action == 'expand'){
			// Expand Animation
			$("#header-content").addClass('expanded',200, function(){
				$("#header-toggle").attr('title','Collapse');
				$("#header-toggle")
					.find('.fi')
					.removeClass('fi_expand')
					.addClass('fi_collapse');	
				$.cookie('headerState', 'expanded');	
				$("#header-search").fadeIn(200,function(){});
				$("#product-logo").fadeIn(200,function(){});	
			});
		}
		else{
			// Collapse Animation
			$("#header-content").removeClass('expanded',200, function(){
				$("#header-toggle").attr('title','Expand');
				$("#header-toggle")
					.find('.fi')
					.removeClass('fi_collapse')
					.addClass('fi_expand');	
				$.cookie('headerState', 'collapsed');
				$("#header-search").fadeIn(200,function(){});
				$("#product-logo").fadeIn(200,function(){});	

			});	
				
		}	
	}
	
	
});