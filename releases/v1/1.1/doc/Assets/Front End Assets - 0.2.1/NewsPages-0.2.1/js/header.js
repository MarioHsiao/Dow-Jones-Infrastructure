$(function(){
	var tabNum = 1;
	$("#header-toggle").bind("click", function(){
		if($("#header-content").hasClass('expanded')){
			animateHeader('collapse');
		}
		else{
			animateHeader('expand');
	
		}
		return false;
	}); 
	
	animateHeader = function(action){
		$("#header-search").fadeOut(200,function(){});	
		$("#product-logo").fadeOut(200,function(){});	

		if(action == 'expand'){
			// Expand Animation
			$("#header-content").addClass('expanded',200, function(){
				$("#header-toggle").attr('title','Collapse');
				$("#header-toggle")
					.children('.fi')
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
					.children('.fi')
					.removeClass('fi_collapse')
					.addClass('fi_expand');	
				$.cookie('headerState', 'collapsed');
				$("#header-search").fadeIn(200,function(){});
				$("#product-logo").fadeIn(200,function(){});	

			});	
				
		}	
	}
	
	adjustTabsSize = function(){
		if($("#dashboard-nav li.project:not(.dj-hidden)").length < 5){
			$("#dashboard-nav").find('.add-bt').css("float","left").css("margin-left","5px");
			$("#dashboard-nav li.project:not(.dj-hidden) .tab-text").css("width","130px");	
		}
		else{
			//console.log($("#dashboard-nav li.project:not(.dj-hidden) .tab-text").width());
			if($("#dashboard-nav").height()>45){
				$("#dashboard-nav").find('.add-bt').css("margin-left","0px").css("float","right");
				var tabs = $("#dashboard-nav li.project:not(.dj-hidden) .tab-text");
				
				while($("#dashboard-nav").height()>45){
					$(tabs).width(($(tabs).first().width()-1));
				}
			}
			
			var posEnd = $("#header-toggle").offset().left;
			var posLastTabEnd = $("#dashboard-nav .project.last").offset().left+$("#dashboard-nav .project.last").outerWidth();
			var diff = posEnd-posLastTabEnd;
			if(posEnd > posLastTabEnd){
				$("#dashboard-nav li.project.last .tab-text").css("width",($("#dashboard-nav li.project.last .tab-text").width()+diff)+"px");	
			}
		}
	}
	
	
	
	fixLastTabMargin = function(){
		$("#dashboard-nav .project").removeClass("last");
		$("#dashboard-nav .project:not(.dj-hidden)").last().addClass("last");
	}
	
	$(".add-bt").bind("click", function(){
		$("#dashboard-nav li.project .tab-text").css("width",'');
		$("#dashboard-nav li.project").css("display",'');
		var newMenuItem = $('<div class="dj-menuitem" tab="tab-'+tabNum+'"><div class="dj-label">Tab '+tabNum+'</div></div>');
		var newTab = $('<li class="project" title="Tab '+tabNum+'" id="tab-'+tabNum+'"><span class="tab sortable clearfix"><span class="action"><span class="fi fi_gear"></span></span><span class="tab-text">Tab '+tabNum+'</span></span></li>');
		
		
		$("#dashboard-nav li.project").removeClass("active-project");
		$(newTab).addClass("active-project");
		
		if($("#dashboard-nav li.project").length >= 8){
			var lastTab = $("#dashboard-nav li.project:not(.dj-hidden)").last();
			$(lastTab).addClass("dj-hidden"); 
			$("#header-hiddentabs-bt").css("display","block");
			$("#hiddentabs-menu .dj-menuitems.normal").append(newMenuItem);
			$("#hiddentabs-menu .dj-menuitem[tab='"+$(lastTab).attr("id")+"']").appendTo("#hiddentabs-menu .dj-menuitems.exceeding");
		}
		else{
			$("#hiddentabs-menu .dj-menuitems.normal").append(newMenuItem);	
		}
		
		$("#dashboard-nav li.project:not(.dj-hidden)").last().after(newTab);
		
		$("#hiddentabs-menu .dj-menuitem").removeClass('selected');
		$("#hiddentabs-menu .dj-menuitem[tab='tab-"+tabNum+"']").addClass('selected');
		
		tabNum++;
		fixLastTabMargin();
		adjustTabsSize();
		$("#hiddentabs-menu").css("display", "none");

	});
	
	$("#header-hiddentabs-bt").bind('click',function(event){
		
		$("#hiddentabs-menu .normal .dj-menuitem").unbind().bind("click",function(event){
			$("#dashboard-nav li.project").removeClass("active-project");
			$("#"+$(this).attr('tab')).addClass("active-project");
			
			$("#hiddentabs-menu .dj-menuitem").removeClass('selected');
			$(this).addClass('selected');
			$("#hiddentabs-menu").css("display", "none");
		});
		
		$("#hiddentabs-menu .exceeding .dj-menuitem").unbind().bind("click",function(event){
			$("#dashboard-nav li.project .tab-text").css("width",'');
			$("#dashboard-nav li.project").css("display",'');
			$("#dashboard-nav li.project").removeClass("active-project");
			var lastTab = $("#dashboard-nav li.project:not(.dj-hidden)").last();
			$(lastTab).addClass("dj-hidden"); 
			$(this).addClass('selected');
			$("#"+$(this).attr('tab')).removeClass("dj-hidden").addClass("active-project");
			$("#hiddentabs-menu .dj-menuitems.normal").append($(this));
			$("#hiddentabs-menu .dj-menuitem[tab='"+$(lastTab).attr("id")+"']").appendTo("#hiddentabs-menu .dj-menuitems.exceeding");
			
			$("#hiddentabs-menu .dj-menuitem").removeClass('selected');
			$("#hiddentabs-menu .dj-menuitem[tab='"+$(this).attr('tab')+"']").addClass('selected');
			
			fixLastTabMargin();
			adjustTabsSize();
			$("#hiddentabs-menu").css("display", "none");
		});
		
		
		$("#hiddentabs-menu").css("display", "inline");	
		$('body').one('click', function() {
			$("#hiddentabs-menu").css("display", "none");	
		});
		event.stopPropagation();
	});
	
	$("#hiddentabs-menu").bind('click',function(event){
		event.stopPropagation();	
	});
	
	
	$("#dashboard-nav li.project").live("click",function(event){
		$("#dashboard-nav li.project").removeClass("active-project");
		$(this).addClass("active-project");
		
		$("#hiddentabs-menu .dj-menuitem").removeClass('selected');
		$("#hiddentabs-menu .dj-menuitem[tab='"+$(this).attr('id')+"']").addClass('selected');

	});
	
	$("#dashboard-nav li").live("mouseenter",function(event){
		$(this).find(".fi_gear").css("display","inline-block");
		event.stopPropagation();	
	});
	
	$("#dashboard-nav li").live("mouseleave",function(event){
		$(this).find(".fi_gear").fadeOut(200);
		event.stopPropagation();	
	});
	
	$("#dashboard-nav").sortable({
		helper: "clone", 
		containment:"parent", 
		cancel: ".add-bt",
		start: function(event, ui) {
			$("#dashboard-nav").find('.add-bt').fadeOut(200);
		},
		stop: function(event, ui) {
			$("#dashboard-nav").find('.add-bt').appendTo("#dashboard-nav");
			$("#dashboard-nav").find('.add-bt').fadeIn(200);
			$("#dashboard-nav").find(".fi_gear").css("display","none");
			fixLastTabMargin();
			
			$("#dashboard-nav li.project").each(function(){
				if($(this).hasClass("dj-hidden")){
					$("#hiddentabs-menu .dj-menuitem[tab='"+$(this).attr('id')+"']").appendTo("#hiddentabs-menu .dj-menuitems.exceeding");

				}
				else{
					$("#hiddentabs-menu .dj-menuitem[tab='"+$(this).attr('id')+"']").appendTo("#hiddentabs-menu .dj-menuitems.normal");
				}	
			});
			
		},
		change: function(event, ui) {
			$("#dashboard-nav").find('.add-bt').css("display","none");
		}
	});
	$( "#dashboard-nav" ).disableSelection();
});

$(document).ready(function() {
	// Read the Header state from the cookie
	var headerState = $.cookie('headerState');
	// Apply the stored state
	if(headerState == 'expanded'){
		animateHeader('expand');
	}
});