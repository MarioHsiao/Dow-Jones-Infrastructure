$(document).ready(function(){
	
	$(document).on("click", hidePanels);
	
	// Display Options
	$('.display-options > a').on("click", changeDisplayOptions);
	
	//$(window).resize(resizeContentPanes);
	//$(window).resize();
	
	$('.singlecheck > li > a').on("click", singleCheckFunction);
	
	// Add Section Button
	$('.org-tools > .btn').on("click", orgBtnClick);
	
	// Prevent clicks in add panels from closing that panel
	$('#add-panels').on("click", function(e){
		e.stopPropagation();
	});
	
	// Add Section Panel
	$('#new-section-form').submit(addNewNode);

	$('#new-section-panel').on("click", ".btn-secondary", function(){
//		$('#new-section-panel').addClass("hide");
		$('#new-section-panel').fadeOut("fast");
		$('#new-section-btn').removeClass("active");
	});
	
	// Section Toolbar
	$('#selections-tree').on("click",".section-tools > .btn", sectionBtnClick);
	$('#selections-tree').on("focusout keydown",".section > .sort-container > .section-title-edit-wrap > input", sectionNameEdit);

	
	// Tree Toggle
	$('#selections-tree').on("click",".tree-toggle", function(e){
		e.preventDefault();
		$(this).closest('li').toggleClass('fnb-collapsed').toggleClass('fnb-expanded');
	});
	
	// Star Favorite
	$('.content-picker').on({
		mouseenter: starToggle,
		mouseleave: starToggle,
		click:			starToggle
		},'.dropdown-menu > li > a > [class^="icon"]');
		
	$('.content-picker').on('mouseenter', '> .dropdown-menu > li.dropdown-submenu', checkFavorites);
	
	// Load Sample Article JSON
	$.getJSON('sample-articles.js', renderArticles);
	
	$('#selections-tree').nestedSortable({
		isTree:true,
		listType : "ul",
		items : "li",
		handle : ".sort-container",
		toleranceElement : ".sort-container",
		tolerance : "poitner",
		maxLevels:4,
		placeholder : "dropzone",
		distance : 15,
		//forcePlaceholderSize : true,
		//appendTo: "body",
		/*over : function(e,ui){
			if ($(ui.sender).attr("id") == "content-results") {
				var $parentLI = $(ui.placeholder).parents("li");
				if ($parentLI.hasClass("article") && $parentLI.hasClass("fnb-branch")){
					$(ui.sender).nestedSortable("cancel");
				}
			}
		},*/
		receive : receiveNode,
		stop : sortStop,
		tabSize : 60,
		//isAllowed : sortIsAllowed,
    //cursorAt: {left:5, top:0},
		// Tree classes
		branchClass: 'fnb-branch',
		collapsedClass: 'fnb-collapsed',
		disableNestingClass: 'related-article',
		errorClass: 'fnb-error',
		expandedClass: 'fnb-expanded',
		hoveringClass: 'fnb-hovering',
		leafClass: 'fnb-leaf'
	});
	
	
	$("ul, li").disableSelection();
	
	last_child();
	
});


// Force IE7 & 8 to play nice with :last-child
function last_child() {
  if ($('html').hasClass("lt-ie9")) {
    $('*:last-child').addClass('last-child');
  }
}

function receiveNode(e,ui){
	//console.log(ui.placeholder);
	// Determine what item is being dragged
	// Then make a clone of it and add the clone back to the list
	// in the spot where it came from
	var fromIndex = $(ui.sender).attr("data-index"),
			$clone = $(ui.item).clone(true).addClass("selected"),
			$parentEl = $(ui.item).parent();
	if (fromIndex == 0){
		$(ui.sender).prepend($clone);
	} else {
		$(ui.sender).children('li:nth-child('+fromIndex+')').after($clone);
	}
	
	//Remove the inline style
	$(ui.item).removeAttr("style");
	// Add the dropdown toggle buttons and the related icon
	$(ui.item).find('.sort-container').prepend('<a href="#" class="tree-toggle"><i class="icon-caret-right icon-large"></i><i class="icon-caret-down icon-large"></i></a><i class="icon-share-alt"></i>');
	// Change the drag handle icon
	$(ui.item).find('.drag-handle').html('<i class="icon-reorder icon-large"></i>');
	
	if ($parentEl.attr("id") != "selections-tree" && $parentEl.parent().hasClass("article")) {
		$(ui.item).addClass("related-article");
	}
	
	
	$('#selections-tree').nestedSortable("refresh");
	//console.log($('#selections-tree').data("nestedSortable").items.length);
	//$(ui.item).addClass("adam");			
}

function sortStop(e,ui){
	console.log(ui);
	var $item = $(ui.item),
			$parentEl = $item.parent(),
			itemType = "",
			dragToTrunk = false;
	console.log("Parent:");
	console.log($parentEl);
	
	// Are we dragging to the main trunk
	if ($parentEl.attr("id") == "selections-tree") dragToTrunk = true;
	
	// Are we dragging a section, sub-section, or article?
	if ($item.hasClass("section")) {
		itemType = "section";
	} else if ($item.hasClass("subsection")) {
		itemType = "subsection";
	} else if ($item.hasClass("article")) {
		itemType = "article";
	}
	
	// Determine if article is now nested below another article
	// If so, add icon for nested article
	if (!dragToTrunk && itemType == "section") {
		$item.removeClass("section").addClass("subsection");
	}
	
	if (dragToTrunk && itemType == "subsection") {
		$item.removeClass("subsection").addClass("section");
	}
	
	if (!dragToTrunk && itemType == "article") {
		if ($parentEl.parent().hasClass("article")){
			$item.addClass("related-article");
		} else {
			$item.removeClass("related-article");
		}
	} else if (dragToTrunk && itemType == "article") {
		$item.removeClass("related-article");
	}
	

}

function changeDisplayOptions(e){
	e.preventDefault();
	// Do nothing if the clicked button was the active one
	// TO DO: disable pointer cursor via css
	if ($(this).hasClass("active")) return;
	// Set up variables
	var view = $(this).attr("data-view"),
			$contentDivs = $('#content-selections > div'),
			$cPane = $('#content-selections > div:first-child'),
			$sPane = $('#content-selections > div + div'),
			$cDrop = $('.content-tools .btn-group'),
			$cSearch = $('.content-tools .form-search'),
			$rOpt = $('.results-options'),
			$rPag = $('.pagination'),
			currLeftSpan = $cPane.attr("class").slice(4,6),
			span = {},
			gridCols = 16;
	// Handle button classes
	$(this).closest("ul").find("li").removeClass("active");
	$(this).closest("li").toggleClass("active");
	// Set params for view
	switch (view) {
		case "split":
			span.l = gridCols/2, span.r = gridCols/2;
			break;
		
		case "split-left":
			span.l = gridCols*.625, span.r = gridCols*.375;
			break;
			
		case "split-right":
			span.l = gridCols*.375, span.r = gridCols*.625;
			break;
		
		case "full":
			span.l = currLeftSpan, span.r = gridCols;
			break;
			
		default:
			span.l = gridCols/2, span.r = gridCols/2;
			break;		
	}
	
	if (view == "full") {
		$cPane.hide();
		$sPane.addClass("offset"+currLeftSpan);
		$sPane.switchClass($sPane.attr("class"), "span"+span.r);
	} else if (view != "full" && $cPane.is(":hidden")) {
		$sPane.switchClass($sPane.attr("class"), "span"+span.r+" offset"+span.l, function(){
			$sPane.removeClass("offset"+span.l);
			$cPane.removeClass().addClass("span"+span.l).show();
		});
	} else {
		$cPane.switchClass($cPane.attr("class"), "span"+span.l);
		$sPane.switchClass($sPane.attr("class"), "span"+span.r);
	}
	
	// Change the view of the Content Tools when split-right
	if (view == "split-right") {
		$cDrop.removeClass("span10").addClass("span8");
		$cSearch.removeClass("span6").addClass("span8");
		$rOpt.removeClass("span10").addClass("span16");
		$rPag.removeClass("span6").addClass("span16");
	} else {
		$cDrop.removeClass("span8").addClass("span10");
		$cSearch.removeClass("span8").addClass("span6");
		$rOpt.removeClass("span16").addClass("span10");
		$rPag.removeClass("span16").addClass("span6");
	}
	
}

function resizeContentPanes(){
		var h =	$(window).height();
		//$('#content-pane').height(h-100);
		$('#selections-tree').height(h-100);
}

function singleCheckFunction(e){
	e.preventDefault();
	var $this = $(this),
			$parentDiv = $this.closest("div"),
			$parentLink = $parentDiv.children(".btn.dropdown-toggle"),
			myText = $(this).text();
	$this.closest('.singlecheck').find('li').removeClass("checked");
	$this.parent("li").addClass("checked");
	
	// Set text of button to selection
	myText = myText.replace(/^\s+|\s+$/g,''); //Strip the leading and trailing spaces
	$parentLink.contents().filter(function(){
		if (this.nodeType == 3) this.data = myText;
	});

	if ($parentDiv.attr("id") == "snippets-toggle"){
		if ($this.text() == " Snippets off") {
			$('#content-results').addClass("hide-snippets");
		} else {
			$('#content-results').removeClass("hide-snippets");
		}
	}
	
}

function renderArticles(data){
	$.each(data.articles, function(){
		// pull out different sections, wrap them in the proper html, then write to DOM
		var $entry = $('<li class="article ' + this.contentCategoryDescriptor + '"><div class="sort-container clearfix"><div class="article-wrap" /></div></li>');
		var $entryDiv = $entry.find('.article-wrap');
		$entryDiv.append('<i class="fi-two fi_f-article"></i>');
		$entryDiv.append('<h5><a href="#">' + this.title + '</a></h5>');
		$entryDiv.append('<p class="article-meta">' + this.sourceDescriptor + ', ' + this.publicationDateDescriptor + ', ' + this.wordCountDescriptor + ', ' + this.baseLanguageDescriptor + '</p>');
		var snip = "";
		for (i=0; i<this.snippets.length; i++) {
			snip += this.snippets[i];
		}
		$entryDiv.append('<p class="article-snip">' + snip + '</p>');
		$entry.find('.sort-container').append('<div class="drag-handle"><i class="icon-signout icon-2x"></i></div>');
		$entry.appendTo($('#content-results'));
	});
	
	$("#content-results").nestedSortable({
		items : "li",
		handle : ".drag-handle",
		toleranceElement:".sort-container",
		listType:"ul",
		//helper: dragHelper,
		helper : "clone",
    connectWith: "#selections-tree",
    //start:dragStart,
    cursorAt: {left:5, top:0},
    //appendTo:"body",
    placeholder:"dropzone",
    tolerance:"pointer",
    //forceHelperSize:true,
    //disableNesting:"no-nest",
    maxLevels:-1,
    errorClass:"fnb-error",
    start:startCopy,
    cancel:".selected",
    isTree:true,
		// Tree classes
		branchClass: 'fnb-branch',
		collapsedClass: 'fnb-collapsed',
		disableNestingClass: 'fnb-no-nesting',
		errorClass: 'fnb-error',
		expandedClass: 'fnb-expanded',
		hoveringClass: 'fnb-hovering',
		leafClass: 'fnb-leaf'
	});

}

function startCopy(e,ui){
	var $item = $(ui.item),
		i = $item.index(),
		$par = $item.parent();
		$item.css("display","list-item"); //keeps element visible
		$par.attr("data-index",i);
}

function addNewNode(){
	var newSectionName = $(this).find("input").val(),
			$newSection = $('<li class="section fnb-leaf"><div class="sort-container clearfix" /></li>'),
			$newSectionDiv = $newSection.find(".sort-container");
	if (newSectionName != "") {
		$newSectionDiv.append('<a href="#" class="tree-toggle"><i class="icon-caret-right icon-large"></i><i class="icon-caret-down icon-large"></i></a>');
		$newSectionDiv.append('<h3>' + newSectionName + '</h3>');
		$newSectionDiv.append('<div class="section-tools pull-right"><button class="btn btn-inverse" id="edit-section-title" title="Edit Section Name"><i class="icon-pencil"></i></button><button class="btn btn-inverse" id="add-subsection" title="Add Subsection"><i class="icon-plus"></i></button><button class="btn btn-inverse" id="delete-section" title="Delete Section"><i class="icon-trash"></i></button></div>');
		$newSection.appendTo('#selections-tree');
		// Scroll to bottom of selections tree to make sure we can see the new section
		$('#selections-tree').prop({scrollTop:$('#selections-tree').prop("scrollHeight")});
		// Clear the field, then dismiss panel
		$(this).find("input").val("");
	//	$('#new-section-panel').addClass("hide");
		$('#new-section-panel').fadeOut("fast");
		$('#new-section-btn').removeClass("active");
	}
	//Prevent refresh
	return false;
}
			
function dragStart(e, ui){
	//console.log("started drag");
}

function dragHelper(e,dragItem){
	var headline = $(dragItem).find("h5").text();
	return '<div class="drag-helper"><i class="icon-list-alt icon-2x pull-left"></i> ' + headline + '</div>';
//	return '<div class="drag-helper"><i class="icon-list-alt icon-2x pull-left"></i></div>';
}

function toggleTree(e){
	alert("hs");
	var $branch = $(this).parent('li');
	$branch.removeClass("fnb-expanded").addClass("fnb-collapsed");
}


function hidePanels(){
//	$('#add-panels').find('.inline-modal').addClass("hide");
	$('#add-panels').find('.inline-modal').fadeOut('fast');
	$('.org-tools > .btn').removeClass("active");
}

function orgBtnClick(e){
	e.preventDefault();
	e.stopPropagation();
	hidePanels();
	$('.org-tools > .btn').removeClass("active");
	
	if ($(this).attr("id") == "new-section-btn") {
		$(this).addClass("active");
//		$('#new-section-panel').removeClass("hide").find("input").focus();
		$('#new-section-panel').fadeIn('fast').find("input").focus();
	}
	
	if ($(this).attr("id") == "new-link-btn")
		$(this).addClass("active");
}

function sectionBtnClick(){
	console.log(this);
	//e.preventDefault();
	var theBtn = $(this).attr("id"),
			$theTools = $(this).closest("div"),
			$theSection = $(this).closest(".sort-container"),
			theTitle = $theSection.find("h3").text();
	
	if (theBtn == "edit-section-title" && !$theSection.hasClass("editing")) {
		$theSection.addClass("editing").find("h3").addClass("hide"); // Hide existing title
		$theTools.before('<span class="section-title-edit-wrap"><input type="text" value="'+theTitle+'" /></span>'); // Create input prepopulated with current title, show it
		$theSection.find("input").focus();
	}	
}

function sectionNameEdit(e){
	if (e.type == "focusout" || (e.type == "keydown" && e.keyCode == 13)){
		var newSectionName = $(this).val(),
				$sectionNameEl = $(this).closest(".sort-container").find("h3"),
				$myWrapper = $(this).parent("span");
		if (newSectionName != ""){ // New section name can't be blank
			$sectionNameEl.text(newSectionName).removeClass("hide");
			$sectionNameEl.closest(".sort-container").removeClass("editing");
			$myWrapper.remove();
		} else {
			$(this).addClass("error");
		}
	}
}

function sortIsAllowed(placeholder, placeholderparent, currentitem){
	if ( ($(currentitem).hasClass("fnb-branch") && $(placeholderparent).hasClass("article")) || $(placeholderparent).hasClass("related-article") ) {
		return false;
	} else {
		return true;
	}
}

function starToggle(e) {
	var isFavorite = $(this).closest("li").hasClass("favorite");
	if (e.type === "mouseenter" && !isFavorite) {
		$(this).removeClass("icon-star-empty").addClass("icon-star");
	}
	if (e.type === "mouseleave" && !isFavorite) {
		$(this).removeClass("icon-star").addClass("icon-star-empty");
	}
	if (e.type === "click") {
		e.preventDefault();
		e.stopPropagation();
		$(this).closest("li").toggleClass("favorite");
		var favCount = $(this).closest(".dropdown-menu").children(".favorite").length;
		favCount == 0 ? $(this).closest(".dropdown-submenu").removeClass("hasFavorites") : $(this).closest(".dropdown-submenu").addClass("hasFavorites");
		//console.log(favCount);
	}
}

function checkFavorites(){
	var $submenu = $(this),
			$subList = $submenu.children(".dropdown-menu"),
			$favs = $subList.find(".favorite"),
			favCount = $favs.length,
			hasFavs = $submenu.hasClass("hasFavorites"),
			$viewAllBase = '<li class="divider"></li><li class="dropdown-submenu"><a tabindex="-1" href="#">All Alerts</a><ul class="dropdown-menu" /></li>';
			
			$subList.children(".dropdown-submenu").length > 0 ? favListCreated = true : favListCreated = false;
	
	//console.log("hasFavs:"+hasFavs+", favListCreated:"+favListCreated);
	// If no favorites, show all
	// else show favorites and create new submenu for all
	if (hasFavs && !favListCreated) {
		$subList.append($viewAllBase);
		var $allList = $subList.children(".dropdown-submenu").children(".dropdown-menu");
		$subList.children("li").not(".divider").not(".dropdown-submenu").clone().appendTo($allList);
		$subList.children("li").not(".favorite").not(".divider").not(".dropdown-submenu").hide();
	}
	
	if (!hasFavs && favListCreated) {
		// put all items back then remove the all list
		$subList.children("li").not(".favorite").not(".divider").not(".dropdown-submenu").show();
		$subList.children(".divider").remove();
		$subList.children(".dropdown-submenu").remove();
	}
}