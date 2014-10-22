//
// create closure
//
(function($) {
  //
  // plugin definition
  //
    $.fn.headlines = function(options, data, literals) {
        $.headlines(this, options, data, literals);
        return this;
    };

    $.headlines = function (container, options, data, literals) {
        var container = $(container).get(0);
        return container.headlines || (container.headlines = new $._headlines(container, options, data, literals));
    };

    $._headlines = function (container, options, data, literals) {

        // setup a variable on the container
        var hds = this;
        hds.options = $.extend({}, $.fn.headlines.defaults, options);
        hds.literals = $.extend({}, $.fn.headlines.literals, literals);
        hds.data = $.extend(true,{}, data);
        hds.container = container;
       
        
        hds.paint = function() {
                   
            var $container = $(hds.container);
            if (hds.options.baseClassName != null && 
                hds.options.baseClassName.length > 0 && 
                !$.isArray(hds.options.baseClassName)) {
                $container.addClass(hds.options.baseClassName);         
            }
            
            if (hds.data.hitCount != null && hds.data.hitCount.value > 0) {
                $container.html("<ul class=\"headlineContainer\"></ul>");
                var tUl = $('ul.headlineContainer', container).get(0);
                var $tUl = $(tUl);
                tUl.options = hds.options;
                var i = 0;
                var count = hds.data.resultSet.count.value;
                
                $.each(hds.data.resultSet.headlines, function () { 
                    
                    $tUl.append("<li><div class=\"headline\"></div></li>"); 
                    var tLi = $('li',$tUl).get(i);   
                    // bind the headline data to the expando property: be carefull with memory leaks in IE
                    tLi.headline = this;
                                        
                    var tDiv = $('div.headline', tLi).get(0);  
                     
                    // start painting of headline 
                    // paint title
                    $.fn.headlines.paintMarkup(tDiv, hds.options, this.title , hds.literals, "title", this ); 
                    
                    // Add details sections
                    $.fn.headlines.addDetails(tDiv, hds.options, this , hds.literals);
                    
                    // paint snippet
                    if (hds.options.displaySnippets) {
                        $.fn.headlines.paintMarkup( tDiv, hds.options, this.snippet , hds.literals, "snippet" );
                    }           
                    else {                   
                        $(tLi).attr("title", $.fn.headlines.getSnippetMarkup( hds.options, this.snippet , hds.literals));
                        $(tLi).simpletooltip();                 
                    } 
                                       
                    // paint accession numbers
                    if (hds.options.displayAccessionNumbers && this.reference.type == "accessionNo" ) {
                        $(tDiv).append("<div class=\"accessionNo\">Document " + this.reference.guid + "</div>");  
                    }
                        
                    // Do duplication    
                          
                    $(tLi).addClass(this.contentSubCategoryDescriptor);                            
                                           
                    $(tLi).bind("mouseover", {}, function(event) {
                        var parentTag = $(this).parent("ul.headlineContainer").get(0);
                        if ($.isFunction(parentTag.options.headlineClickCallback)){
                            $(this).addClass('mouseover'); 
                        }
                    }).bind("mouseout", {}, function(event) {                  
                        var parentTag = $(this).parent("ul.headlineContainer").get(0);
                        if ($.isFunction(parentTag.options.headlineClickCallback)){
                            $(this).removeClass('mouseover'); 
                        }                       
                    }).bind("click", {}, function(event){                            
                        var parentTag = $(this).parent("ul.headlineContainer").get(0);
                        if ($.isFunction(parentTag.options.headlineClickCallback)){
                            parentTag.options.headlineClickCallback(this, {headline:this.headline});    
                        }     
                    });
                    
                    if (!(i < count-1 && i < hds.options.maxNumHeadlinesToShow-1)) {
                        $(tLi).addClass("last");
                    }
                    
                    if (i >= hds.options.maxNumHeadlinesToShow) {
                        $(tLi).hide();
                    }
                    
                    i++;
                    
                });
            }
            else {
                $container.html("<span>" + hds.literals.noResults + "</span>");
            }
           
        };
        
        hds._clear = function() {
            $(hds.container).empty();
        };
        
        hds.setHeadlineClickCallback = function(callback) {
            if ($.isFunction(callback)) {
                hds.options.headlineClickCallback = callback; 
            }   
            hds._clear();
            hds.paint();           
        };
        
         hds.setHeadlineEntityClickCallback = function(callback) {
            if ($.isFunction(callback)) {
                hds.options.headlineEntityClickCallback = callback; 
            }
             hds._clear();
            hds.paint();              
        };
        
        // initialize here
        hds._clear();
        hds.paint();      
    };
  
    $.fn.headlines.debug = function (msg) {
        if (window.console && window.console.log)
            window.console.log(msg);
    };
        
    $.fn.headlines.paintMarkup = function(container, options, markup, literals, className, data,/*optional*/ titleStr ) {
        
        if (markup == null)
            return;
        var $container = $(container);
        $container.append("<div class=\"" + className + "\"></div>");
        var $tDiv = $($( "div." + className , $container).get(0));
        var tClassName = "text";
        $.each(markup, function () { 
            if (options.displayEntities) {
                switch(this.type){
                    default:
                    case "Textual":
                        tClassName = "text";                   
                        break;
                    case "Highlight":
                        tClassName = "highlight";
                        break;
                    case "Company":
                        tClassName = "company";
                        break;                
                    case "Person":
                        tClassName = "person";
                        break;
                    
                }

                $tDiv.append("<span class=\"" + tClassName + "\">" + this.value + "</span>" + "<span class=\"space\"> </span>"); 
                if ((this.type == "Company" || this.type == "Person") &&
                    $.isFunction(options.headlineEntityClickCallback)) {
                    
                    var tSpan = $($( "span." + tClassName , $tDiv).get(0));
                    $(tSpan).bind("mouseover", {entity : this, headline: data, func: options.headlineEntityClickCallback}, function(event) {
                        if ($.isFunction(event.data.func)) 
                            $(this).addClass('mouseover'); 
                    }).bind("mouseout", {entity : this, headline: data, func: options.headlineEntityClickCallback}, function(event) {                  
                            //Add and remove class
                        if ($.isFunction(event.data.func))
                            $(this).removeClass('mouseover');                        
                    }).bind("click", { entity : this, headline: data, func: options.headlineEntityClickCallback },function(event){
                        if (event.data.func) {
                            event.stopPropagation();
                            event.data.func(event);
                        }         
                    });
                    
                }
            }
            else {
                $tDiv.append("<span class=\"" + tClassName + "\">" + this.value + "</span>" + "<span class=\"space\"> </span>"); 
            }
        }); 
        
        if ( titleStr != null && titleStr.length > 0 && !$.isArray(titleStr)) {
            $tDiv.attr("title", titleStr);
            $tDiv.simpletooltip();
        }
    };
    
    $.fn.headlines.addDetails = function(container, options, headline, literals) {
        if (headline == null)
            return;
        var $container = $(container);
        $container.append("<div class=\"details\"></div>");
        var $tDiv = $($( "div.details", $container).get(0));
        var addedContent = false;
        
        if (headline.sourceDescriptor) { 
            $tDiv.append("<span class=\"soruce\">" + headline.sourceDescriptor + "</span>");
            addedContent = true;
        }
        
        if (headline.publicationDateTimeDescriptor) { 
            if(addedContent) $tDiv.append("<span class=\"space\">, </span>");
            $tDiv.append("<span class=\"publicationDate\">" + headline.publicationDateTimeDescriptor + "</span>");
            addedContent = true;
        }
        
        if (headline.wordCountDescriptor) { 
            if(addedContent) $tDiv.append("<span class=\"space\">, </span>");
            $tDiv.append("<span class=\"words\">" + headline.wordCountDescriptor + "</span>");
            addedContent = true;
        }
        
         if (headline.baseLanaguageDescriptor) { 
            if(addedContent) $tDiv.append("<span class=\"space\">, </span>");
            $tDiv.append("<span class=\"language\">" + headline.baseLanaguageDescriptor + "</span>");
            addedContent = true;
        }
    }
    
    $.fn.headlines.getSnippetMarkup = function(options, markup, literals) {
        var t = new Array();
        if (markup == null)
            return;
            
        var tClassName;
        $.each(markup, function () { 
            switch(this.type){
                default:
                case "Textual":
                    tClassName = "text";                   
                    break;
                case "Highlight":
                    tClassName = "highlight";
                    break;
                case "Company":
                    tClassName = "company";
                    break;                
                case "Person":
                    tClassName = "person";
                    break;
                
            }
            t[t.length] = "<span class=\"" + tClassName + "\">" + this.value + "</span>" + "<span class=\"space\"> </span>"; 
        }); 
        return t.join("");
    };
    
    
    $.fn.headlines.defaults = {
        debug : false,
        maxNumHeadlinesToShow : 5,
        displaySnippets : true,
        displayAccessionNumbers : true,
        displayDeleteTrigger : false,
        displayClipTrigger : false,
        displayMoreLikeThisTrigger : true,
        displayByLines : true,
        displayInputCheckBoxes : false,
        displayDuplicates : true,
        displayAccessionNumbers : false,
        displayEntities : true,
        displayImportance : false,
        enableHeadlineDraging : false,
        dropZoneId : "",        
        baseClassName : "",
        headlineClickCallback : null,
        headlineEntityClickCallback : null,
        sourceClickCallback : null,
        moreLikeThisClickCallback : null
    };
    
    $.fn.headlines.literals = {
        of : '${of}',
        publication : '${publication}' ,
        website : '${website}' ,
        picture : '${picture}' ,
        noResults: '${noResults}'
        
    };
    
    //
    // end of closure
    //*/
})(jQuery);

/**
*
*	simpleTooltip jQuery plugin, by Marius ILIE
*	visit http://dev.mariusilie.net for details
*
**/
(function($){ $.fn.simpletooltip = function(className){
    
	return this.each(function() {
		var text = $(this).attr("title");
		$(this).attr("title", "");
		if(text != undefined) {
			$(this).hover(function(e){
				var tipX = e.pageX + 12;
				var tipY = e.pageY + 12;
				$(this).attr("title", ""); 
				$("body").append("<div id='simpleTooltip' class=\"" + className + "\" style='position: absolute; z-index: 100; display: none;'>" + text + "</div>");
				$("#simpleTooltip").css("left", tipX).css("top", tipY).fadeIn("fast");
			}, function(){
				$("#simpleTooltip").remove();
				$(this).attr("title", text);
			});
			$(this).mousemove(function(e){
				var tipX = e.pageX + 12;
				var tipY = e.pageY + 12;
				$("#simpleTooltip").css("left", tipX).css("top", tipY).fadeIn("fast");
			});
		}
	});
}})(jQuery);