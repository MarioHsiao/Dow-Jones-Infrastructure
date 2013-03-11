//
// create closure
//
(function($) {
  //
  // plugin definition
  //
    $.fn.dj_emg_headlineList = function(options, data, tokens) {
        $.dj_emg_headlineList(this, options, data, tokens);
        return this;
    };

    $.dj_emg_headlineList = function (container, options, data, tokens) {
        var container = $(container).get(0);
        if (container != null)
            return container.headlineList || (container.headlineList = new $._dj_emg_headlineList(container, options, data, tokens));
        return null;
    };

    $._dj_emg_headlineList = function (container, options, data, tokens) {
        // setup a variable on the container
        var hds = this;
        
        hds.options = $.extend({}, $.fn.dj_emg_headlineList.defaults, options);
        hds.tokens = $.extend({}, $.fn.dj_emg_headlineList.tokens, tokens);
        hds.data = $.extend(true,{}, data);
        hds.container = container;        
        
        hds.appendHeadlines = function(data){
            var $container = $(hds.container);            
            var tData = $container.data("data");
            if (data.hitCount != null && data.hitCount.value > 0 && data.resultSet.count.value > 0) {
                
                var tUl = $('ul.dj_emg_headlineList', container).get(0);
                var $tUl = $(tUl);                
                var count = data.resultSet.count.value + tData.resultSet.count.value;
                var i = data.resultSet.count.value;
                
                // update the data object
                //{"exp":0,"isPositive":true,"rawText":{"Value":"15"},"text":{"Value":"15"},"value":15}
                tData.resultSet.count.value = data.resultSet.count.value + tData.resultSet.count.value;
                tData.resultSet.count.rawText.Value = $.formatNumber(tData.resultSet.count.value);
                tData.resultSet.count.text.Value = $.formatNumber(tData.resultSet.count.value);
                tData.hitCount = data.hitCount;
                tData.resultSet.headlines = $.merge(tData.resultSet.headlines,data.resultSet.headlines);
                $container.data("data",tData);  
                                
                $.each(data.resultSet.headlines, function () { 
                    if (this.duplicateHeadlines && this.duplicateHeadlines.length > 0) {
                        count = count - this.duplicateHeadlines.length;
                    }
                    hds._paintHeadline(tUl,this,i++,count);
                });
                
                // add handlers to checkboxes to 
                var checkboxes = $(":checkbox", tUl);
                $.each(checkboxes, function () { 
                    $(this).click(function (e) {                       
                        e.stopPropagation();
                    });
                });
                
                hds._setDraggable(tUl);
                hds._setDroppable(tUl);     
                
                hds.setMaxNumHeadlinesToShow(hds.options.maxNumHeadlinesToShow);       
            }
        };
               
        hds.paint = function() {
                   
            var $container = $(hds.container);
            if (hds.options.baseClassName != null && 
                hds.options.baseClassName.length > 0 && 
                !$.isArray(hds.options.baseClassName)) {
                $container.addClass(hds.options.baseClassName);  
            }            
            
            $container.addClass("dj_emg_headlineListContainer");     
            $container.data("options", hds.options);
            $container.data("tokens", hds.tokens);
            $container.data("data", hds.data);   
            
            if (hds.data.hitCount != null && hds.data.hitCount.value > 0) {
                $container.html("<ul class=\"dj_emg_headlineList\"></ul>");
                var tUl = $('ul.dj_emg_headlineList', container).get(0);
                var $tUl = $(tUl);                
                var count = hds.data.resultSet.count.value;
                var i = 0;
                
                $.each(hds.data.resultSet.headlines, function () { 
                    if (this.duplicateHeadlines && this.duplicateHeadlines.length > 0) {
                        count = count - this.duplicateHeadlines.length;
                    }
                    hds._paintHeadline(tUl,this,i++,count);
                });
                
                // add handlers to checkboxes to 
                var checkboxes = $(":checkbox", tUl);
                $.each(checkboxes, function () { 
                    $(this).click(function (e) {                       
                        e.stopPropagation();
                    });
                });
                
                
                hds._setDraggable(tUl);
                hds._setDroppable(tUl);        
                hds.setMaxNumHeadlinesToShow(hds.options.maxNumHeadlinesToShow);  
            }
            else {
                $container.html("<span class=\"dj_emg_noResults\">" + hds.tokens.noResults + "</span>");
                if (!hds.options.displayNoResultsToken) {
                    var no_results = $("span.dj_emg_noResults", $container).get(0);
                    if (no_results) {
                        $(no_results).hide();
                    }
                }
            }   
           // $container.hide("fast",hds._animation);
        };
        
        hds._animation = function() {
             var $container = $(hds.container);
             $container.fadeIn("slow");
        };
        
        hds._paintHeadline = function(container, headline, index, count) {
            var $tUl = $(container);
            var tLi = $("<li class=\"dj_emg_entry\"><div class=\"dj_emg_entry_handle\"></div><div class=\"dj_emg_headline\"><div class=\"dj_emg_headline_body\"></div><div class=\"dj_emg_headline_gutter\"></div><div class=\"dj_emg_headline_action\"></div></div></li>"); 
            var tDivMain = $('div.dj_emg_headline', tLi).get(0);  
            var tDivAction = $('div.dj_emg_headline_action', tLi).get(0);  
            var tDivBody = $('div.dj_emg_headline_body', tLi).get(0);  
            var tDivGutter = $('div.dj_emg_headline_gutter', tLi).get(0);  
            
            $(tLi).data("headline", headline);    
                                  
            // paint action span         
            $.fn.dj_emg_headlineList.addAction(tDivAction, hds.options, hds.tokens);
                                   
            // paint title div
            $.fn.dj_emg_headlineList.paintParas(tDivBody, hds.options, headline.title , hds.tokens, "dj_emg_title", headline); 
                   
                       
            // add MetaData
            var tMeta = $("<div class=\"dj_emg_meta\"></div>")
            // Add details
            $.fn.dj_emg_headlineList.addDetails(tMeta, hds.options, headline , hds.tokens);
            
            // Add byline                    
            $.fn.dj_emg_headlineList.paintParas(tMeta, hds.options, headline.byline , hds.tokens, "dj_emg_byline");
            // Add a meta wrapper to the following
            $(tDivBody).append(tMeta);
            
            // Add snippet                    
            $.fn.dj_emg_headlineList.paintParas(tDivBody, hds.options, headline.snippet , hds.tokens, "dj_emg_snippet", headline );
                        
            // paint accession numbers
            if (headline.reference.type == "accessionNo") {                
                
                // process Display of accession
                var accessNo = $("<p class=\"dj_emg_accessionNo\">(Document " + headline.reference.guid + ")</p>");
                var tMoreLikeThis = $("<p class=\"dj_emg_moreLikeThis\"><span>" + hds.tokens.moreLikeThis + "</span></p>");  
                if (!hds.options.displayAccessionNumbers) {
                    $(accessNo).hide();
                } 
                
                // process MoreLikeThis
                $(tMoreLikeThis).hover(function(e) {
                    $(this).addClass('dj_emg_mouseover'); 
                },function(e) {                  
                    $(this).removeClass('dj_emg_mouseover'); 
                }).click(function(e){        
                    var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
                    var parentLiTag = $(this).closest("li").get(0);
                    if (parentContainer && parentLiTag) {
                        $(parentContainer).triggerHandler("dj.emg.headlineList.moreLikeThisClick", {headline:$(parentLiTag).data("headline")})
                    }  
                    e.stopPropagation();
                    return false;                             
                });
                if (!hds.options.displayMoreLikeThis) {
                    $(tMoreLikeThis).hide();
                }
                
                $(tDivBody).append(accessNo);                 
                $(tDivBody).append(tMoreLikeThis);
            }           
                
            $(tLi).hover(function(e) {                        
                 $(this).addClass('dj_emg_mouseover'); 
                 var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);   
                 if (parentContainer){
                    $(parentContainer).triggerHandler("dj.emg.headlineList.headlineHoverOver", {headline:$(this).data("headline")});  
                 }
            },function(e) {                  
                $(this).removeClass('dj_emg_mouseover'); 
                var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);   
                if (parentContainer){ 
                    $(parentContainer).triggerHandler("dj.emg.headlineList.headlineHoverOut", {headline:$(this).data("headline")}); 
                }
            }).click(function(e){ 
                var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);   
                if (parentContainer){
                    $(parentContainer).triggerHandler("dj.emg.headlineList.headlineClick", {headline:$(this).data("headline")});
                }     
                e.stopPropagation();
                return false;
            });
                             
            $.fn.dj_emg_headlineList.addGutter(tDivGutter, headline,  hds.options, hds.tokens);             
            
            if (hds.options.displayIconography && (!hds.options.displayCheckboxes && !hds.options.displayNumbering)) {
                $(tDivBody).addClass("dj_emg_headline_body_correction");
                $(tDivGutter).addClass("dj_emg_headline_gutter_correction");                
                $("span.dj_emg_iconography",tDivBody).hide();
            }        
            else {
                $("span.dj_emg_iconography",tDivGutter).hide(); 
            }
            
            if (!hds.options.displayIconography && !hds.options.displayCheckboxes && !hds.options.displayNumbering) {
                $(tDivBody).addClass("dj_emg_headline_body_noLeftMargin");
                $(tDivGutter).addClass("dj_emg_headline_gutter_noLeftMargin");   
                $("span.dj_emg_iconography",tDivBody).hide();
                $("span.dj_emg_iconography",tDivGutter).hide();             
            } 
            
            if (headline.duplicateHeadlines && headline.duplicateHeadlines.length > 0) {
                $(tDivBody).append("<div class=\"dj_emg_duplicatesContainer\"><div class=\"dj_emg_duplicatesContainerTitle\"><span class=\"dj_emg_expand dj_emg_plus\"><span>+</span></span> <span class=\"dj_emg_dupText\">" + hds.tokens.preDupHeadline + " " + headline.duplicateHeadlines.length +  " " + hds.tokens.preDupHeadline +  "</span></div><div class=\"dj_emg_duplicatesContainerBody\"><ul id=\"h_" + headline.reference.guid + "\" class=\"dj_emg_dupHeadlineList\"></ul></div></div>");
                //var dupContainerBody = $('div.dj_emg_duplicatesContainerBody', tDivBody).get(0);
                var dupExpand = $('span.dj_emg_expand', tDivBody).get(0);
                var dUl = $("ul#h_" + headline.reference.guid, tDivBody).get(0);
                var i=0;
                $.each(headline.duplicateHeadlines, function () { 
                    hds._paintDupHeadline(dUl ,this,i++,headline.duplicateHeadlines.length);
                });
                
                if (dupExpand) {
                    $(dupExpand).click(function(e) {
                        var $el = $(this);
                        var par = $el.closest("div.dj_emg_duplicatesContainerTitle").get(0);
                        if ($el.hasClass("dj_emg_plus")) {
                            $el.removeClass("dj_emg_plus").addClass("dj_emg_minus").html("<span>-</span>");
                            if (par) {
                                $(par).next().show();   
                            }
                        } 
                        else {
                            $el.removeClass("dj_emg_minus").addClass("dj_emg_plus").html("<span>+</span>");
                            if (par) {
                                $(par).next().hide();  
                            }
                        }
                        e.stopPropagation();
                        return false;
                    }); 
                    if (!hds.options.displayDuplicates){
                        var dContainer = $("div.dj_emg_duplicatesContainer",tDivBody).get(0);
                        if (dContainer)
                            $(dContainer).hide(); 
                    }    
                }                           
            }   
            $tUl.append(tLi);
        };
        
        hds._destroyDroppable = function() {
            $('#' + $.dj.emg.cStr(hds.options.droppableId)).droppable("destroy");
        };
        
        hds._setDroppable = function(){
            if ($.dj.emg.cStr(hds.options.droppableId).length > 0) {
                $('#' + $.dj.emg.cStr(hds.options.droppableId)).droppable({
                  tolerance : "pointer",
                  accept: '.dj_emg_entry',
                  drop: function(e,ui) {  
                    try{
                        ui.draggable.data("dropSuccess",true);                        
                    }
                    catch(e){}
                  },
                  activate: function(e,ui) {
                      $.each(ui.draggable.children(), function () { 
                        $(this).css("visibility", "hidden");  
                      });                 
                  },
                  deactivate: function(e,ui) {
                    $.each(ui.draggable.children(), function () { 
                        $(this).css("visibility", "visible"); 
                    });
                  }                  
                }); 
            }
        };
        
        
        hds._destroyDraggable = function(container){
             $("li.dj_emg_entry",container).draggable('destroy');
        }
        
        hds._setDraggable = function(container) {
            $("li.dj_emg_entry",container).draggable({
              //handler : 'div.dj_emg_entry_handle',
              appendTo: 'body',
              helper : function(e){
                var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
                var t = $(this).clone()
                    .css("width", $(this).outerWidth() +"px")
                    .css("height", $(this).outerHeight() +"px")
                    .addClass("dj_emg_clone");
                return t;
              },  
              handle: "div.dj_emg_entry_handle",
              ghosting: true,
              opacity: 0.9,
              revert: "invalid",
              zIndex: 350,
              stop : function(e,ui) {
                var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
                var parentObj = $.dj_emg_headlineList(parentContainer);
                var prevSiblings =  $(this).prevAll();
                var ind = prevSiblings.length;
                var tHeadline =  $(this).data("headline");
                var success = $(this).data("dropSuccess");
                if (success) {
                    $(parentContainer).triggerHandler("dj.emg.headlineList.headlineDrop", {headlineList: parentObj, index: ind, headline:tHeadline})        
                    if (parentObj.options.removeHeadlineAfterDrop) {
                        parentObj.removeHeadline(ind);
                        return;
                    }
                }
                $(this).data("dropSuccess",false);
              }
            });
            if (!hds.options.enableHeadlineDraging) {
                //$("li.dj_emg_entry",container).draggable('disable'); 
                $("div.dj_emg_entry_handle", container).hide();
            }
        }
        
        hds._setSortable = function(container) {
            $(container).sortable({
                connectWith : '.dj_emg_headlineList',
                helper : function(e,ui){
                    var parentContainer = $(ui).closest(".dj_emg_headlineListContainer").get(0);
                    var t = $(ui).clone()
                        .css("width", $(ui).outerWidth() +"px")
                        .css("backgroundColor", "#FFF");
                    return t;
                }, 
                appendTo: 'body',
                opacity: 0.9,
                revert: "invalid",
                zIndex: 350,
                tolerance : "pointer",
                items : ".dj_emg_entry"
            });
        };
        
        hds._paintDupHeadline = function(container, headline, index, count) {
            var $tUl = $(container);
            var tLi = $("<li class=\"dj_emg_dupEntry\"><div class=\"dj_emg_dupHeadline\"><div class=\"dj_emg_dupHeadline_body\"></div><div class=\"dj_emg_dupHeadline_gutter\"></div><div class=\"dj_emg_dupHeadline_action\"></div></div></li>");   
            var tDivMain = $('div.dj_emg_dupHeadline', tLi).get(0);  
            var tDivAction = $('div.dj_emg_dupHeadline_action', tLi).get(0);  
            var tDivBody = $('div.dj_emg_dupHeadline_body', tLi).get(0);  
            var tDivGutter = $('div.dj_emg_dupHeadline_gutter', tLi).get(0);  
            $(tLi).data("headline", headline);    
            
            $(tLi).data("headline", headline);    
            
            // paint clip span         
            $.fn.dj_emg_headlineList.addClipSpan(tDivAction, hds.options, hds.tokens);
            
            // paint delete span
            //$.fn.dj_emg_headlineList.addDeleteSpan(tDivAction, hds.options,hds.tokens);
                       
            // paint title div
            $.fn.dj_emg_headlineList.paintParas(tDivBody, hds.options, headline.title , hds.tokens, "dj_emg_title", headline); 
            
             // Add a meta wrapper to the following
            $(tDivBody).append("<div class=\"dj_emg_meta\"></div>");
            var tMeta = $('div.dj_emg_meta', tDivBody).get(0);            
            // Add details
            $.fn.dj_emg_headlineList.addDetails(tMeta, hds.options, headline , hds.tokens);
            
            // Add byline                    
            $.fn.dj_emg_headlineList.paintParas(tMeta, hds.options, headline.byline , hds.tokens, "dj_emg_byline");
            
            // Add snippet                    
            $.fn.dj_emg_headlineList.paintParas( tDivBody, hds.options, headline.snippet , hds.tokens, "dj_emg_snippet", headline );
            
            
            var tSnippet = $( ".dj_emg_snippet" , tDivBody).get(0);
            if (!hds.options.displaySnippets || hds.options.displaySnippets === "none" || hds.options.displaySnippets === "hover") {
                $(tSnippet).hide();
            }
            
            // clean up the snippet, done for rss/atom results
            var linksInSnippets = $("a",$(tSnippet))
            $.each(linksInSnippets, function () { 
                $(this).click(function(e) {
                    var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
                    if (parentContainer)  {
                        $(parentContainer).triggerHandler("dj.emg.headlineList.internalSnippetLink", {href:this.href})
                    }
                    e.stopPropagation();
                    return false;
                });
            });
            
            // paint accession numbers
            if (headline.reference.type == "accessionNo") {
                $(tDivBody).append("<div class=\"dj_emg_accessionNo\">Document " + headline.reference.guid + "</div>");  
            }
            
            // paint displayMoreLikeThisOption 
            if (headline.reference.type == "accessionNo") {
                $(tDivBody).append("<div class=\"dj_emg_moreLikeThis\"><span>" + hds.tokens.moreLikeThis + "</span></div>");
                var tMoreLikeThis = $( "div.dj_emg_moreLikeThis" , tDivBody).get(0);  
                $(tMoreLikeThis).hover(function(e) {
                    $(this).addClass('dj_emg_mouseover'); 
                },function(e) {                  
                    $(this).removeClass('dj_emg_mouseover'); 
                }).click(function(e){        
                    var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
                    var parentLiTag = $(this).closest("li").get(0);
                    if (parentContainer && parentLiTag) {
                        $(parentContainer).triggerHandler("dj.emg.headlineList.moreLikeThisClick", {headline:$(parentLiTag).data("headline")})
                    }                               
                });
                if (!hds.options.displayMoreLikeThis) {
                    $(tMoreLikeThis).hide();
                }
            }           
                
            $(tLi).hover(function(e) {                        
                 $(this).addClass('dj_emg_mouseover'); 
            },function(e) {                  
                $(this).removeClass('dj_emg_mouseover'); 
            }).click(function(e){ 
                var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);   
                if (parentContainer){
                    $(parentContainer).triggerHandler("dj.emg.headlineList.headlineClick", {headline:$(this).data("headline")})  
                }   
                e.stopPropagation();   
                return false;  
            });
           
            if (index == count-1) {
                $(tLi).addClass("dj_emg_last");
            }
            
             // Add chechbox
            $.fn.dj_emg_headlineList.addCheckBox(tDivGutter, headline,  hds.options,hds.tokens);     
            // Add index
            $.fn.dj_emg_headlineList.addIndex(tDivGutter, headline,  hds.options,hds.tokens);   
            // paint the iconography span
            $.fn.dj_emg_headlineList.addIconographySpan(tDivGutter,headline,hds.options,hds.tokens);
            
            if (hds.options.displayIconography && (!hds.options.displayCheckboxes && !hds.options.displayNumbering)) {
                 $(tDivBody).addClass("dj_emg_headline_body_correction");
                $(tDivGutter).addClass("dj_emg_headline_gutter_correction");               
                $("span.dj_emg_iconography",tDivBody).hide();
            }        
            else {
                $("span.dj_emg_iconography",tDivGutter).hide(); 
            }
            
            if (!hds.options.displayIconography && !hds.options.displayCheckboxes && !hds.options.displayNumbering) {
                $(tDivBody).addClass("dj_emg_headline_gutter_correction");
                $(tDivGutter).addClass("dj_emg_headline_gutter_correction");  
                $("span.dj_emg_iconography",tDivBody).hide();
                $("span.dj_emg_iconography",tDivGutter).hide();             
            } 
            $tUl.append(tLi); 
        };
                       
    
                  
        hds._clear = function() {
            $(hds.container).empty();
        };
        
        hds.getContainer = function() {
            return $(hds.container);
        };
        
        hds.showHeadline = function() {
            var tLast = $( "li.dj_emg_last" , hds.container);
        };
        
        hds.hideHeadline = function() {
            var tLast = $( "li.dj_emg_last" , hds.container);
        };
        
        // Start Dispaly Numbering
         hds.setDisplayNumbering = function(bool) {
            var $tSnippet = $( "span.dj_emg_index" , hds.container);
            if (bool === true) {   
                 hds.options.displayNumbering = true;         
                 $.each($tSnippet, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displayNumbering = false;
                $.each($tSnippet, function () {
                    $(this).hide(); 
                 });
            }
            return;
        };
        
        hds.getDispalyNumbering = function() {
            return hds.options.displayNumbering;
        };
        // End Display Numbering
        
        // Start Display Snippets
        hds.setDisplaySnippets = function(bool) {
            var $tSnippet = $( "div.dj_emg_snippet" , hds.container);
            if (bool === true) {   
                 hds.options.displaySnippets = true;         
                 $.each($tSnippet, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displaySnippets = false;
                $.each($tSnippet, function () {
                    $(this).hide(); 
                 });
            }
            return;
        };
        
        hds.getDispalySnippets = function() {
            return hds.options.displaySnippets;
        };
        // End Display Snippets
        
        hds.setDisplayAccessionNumbers = function(bool) {
            var $tAcsNo = $( "p.dj_emg_accessionNo" , hds.container);
            if (bool === true) {   
                 hds.options.displayAccessionNumbers = true;         
                 $.each($tAcsNo, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displayAccessionNumbers = false;
                $.each($tAcsNo, function () {
                    $(this).hide(); 
                 });
            }
            return;
        };
        
        hds.getDisplayAccessionNumbers = function() {
            return hds.options.displayAccessionNumbers;
        };
        
        hds.setDisplayDeleteTrigger = function(bool) {
            var $tDel = $( "span.dj_emg_iconography_delete" , hds.container);
            if (bool === true) {   
                 hds.options.displayDeleteTrigger = true;         
                 $.each($tDel, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displayDeleteTrigger = false;
                $.each($tDel, function () {
                    $(this).hide(); 
                 });
            }
            return;
        };
        
        hds.getDisplayIconography = function() {
            return hds.options.displayIconography;
        };
        
        hds.setDisplayIconography = function() {
            return hds.options.displayIconography;
        };
        
        hds.setDisplayDeleteTrigger = function(bool) {
            var $tDel = $( "span.dj_emg_iconography_delete" , hds.container);
            if (bool === true) {   
                 hds.options.displayDeleteTrigger = true;         
                 $.each($tDel, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displayDeleteTrigger = false;
                $.each($tDel, function () {
                    $(this).hide(); 
                 });
            }
            return;
        };
        
        hds.getDisplayDeleteTrigger = function() {
            return hds.options.displayDeleteTrigger;
        };
        
        hds.setDisplayClipTrigger = function(bool) {
            var $tDel = $( "span.dj_emg_iconography_clip" , hds.container);
            if (bool === true) {   
                 hds.options.displayClipTrigger = true;         
                 $.each($tDel, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displayClipTrigger = false;
                $.each($tDel, function () {
                    $(this).hide(); 
                 });
            }
            return;
        };
        
        hds.getDisplayMoreLikeThis = function() {
            return hds.options.displayMoreLikeThis;
        };
        
        hds.setDisplayMoreLikeThis = function(bool) {
            var $tDel = $( "div.dj_emg_moreLikeThis" , hds.container);
            if (bool === true) {   
                 hds.options.displayMoreLikeThis = true;         
                 $.each($tDel, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displayMoreLikeThis = false;
                $.each($tDel, function () {
                    $(this).hide(); 
                 });
            }
            return;
        };
        
        hds.getDisplayClipTrigger = function() {
            return hds.options.displayClipTrigger;
        };
        
        hds.setDisplayDuplicates = function(bool) {
            var $tObj = $("div.dj_emg_duplicatesContainer" , hds.container);
            if (bool === true) {   
                 hds.options.displayDuplicates = true;         
                 $.each($tObj, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displayDuplicates = false;
                $.each($tObj, function () {
                    $(this).hide(); 
                 });
            }
        };
        
        hds.getDisplayDuplicates = function() {
             return hds.options.displayDuplicates;
        };
        
        hds.setDisplayNoResultsToken = function(bool) {
            var $tObj = $( "span.dj_emg_noResults" , hds.container);
            if (bool === true) {   
                 hds.options.displayNoResultsToken = true;         
                 $.each($tObj, function () {
                    $(this).show(); 
                 });
            } 
            else {
                hds.options.displayNoResultsToken = false;
                $.each($tObj, function () {
                    $(this).hide(); 
                 });
            }
        };
        
        hds.removeHeadline = function(index){
            var tEntry = $( "li.dj_emg_entry" , hds.container).get(index);
            if (tEntry) {  
                var prevSiblings =  $(tEntry).prevAll();
                var index = prevSiblings.length;
                if ($(tEntry).hasClass("dj_emg_last")) {                            
                    if (prevSiblings.length > 0) {
                        $(prevSiblings.get(0)).addClass("dj_emg_last");
                    }
                }
                var headlineResult = hds.getContainer().data("data");
                headlineResult.hitCount.value--;
                headlineResult.resultSet.headlines.remove(index);
                $(tEntry).remove();
            }
        };
        
        hds.getDroppableId = function() {
            return hds.options.droppableId ;
        }
        
        hds.setMaxNumHeadlinesToShow = function(num) {
            var tEntries = $( "li.dj_emg_entry" , hds.container);
            var tLast = $( "li.dj_emg_last" , hds.container);
            $.each(tEntries, function () {
                $(this).removeClass("dj_emg_last");
            });
            if (num > 0 && tEntries.length > 0) {   
                 var i = 0;
                 hds.options.maxNumHeadlinesToShow = num;         
                 $.each(tEntries, function () {
                    if (i < num || num === -1){
                        $(this).show(); 
                    }
                    else {
                         $(this).hide(); 
                    }
                    i++;
                 });
            } 
            else {
                hds.options.maxNumHeadlinesToShow = -1;
                $.each(tEntries, function () {
                    $(this).show(); 
                 });
            }
            // update the last entry
             if (num === -1 || tEntries.length <= num) {
                $(tEntries.get(tEntries.length - 1)).addClass("dj_emg_last");
             }
             else {
                $(tEntries.get(num-1)).addClass("dj_emg_last");
             }
            return;
        };        
        
        hds.getMaxNumHeadlinesToShow = function() {
            return hds.options.maxNumHeadlinesToShow;
        };
        
        hds.setData = function(value) {
            hds.data = value;
            hds._clear();
            hds.paint(); 
        };
        
        hds.getData = function() {
            return hds.data;
        };
        
        hds.appendData = function(value){
            var startTime = new Date();        
            hds.appendHeadlines(value); 
            $.dj.emg.debug("appendData:" + (new Date().getTime() - startTime.getTime()));
              
        };
        
        // initialize here
        //hds._clear();
        var startTime = new Date();        
        hds.paint();      
        $.dj.emg.debug("paint:" + (new Date().getTime() - startTime.getTime()));
    };
        
    $.fn.dj_emg_headlineList.paintParas = function(container, options, paras, tokens, className, headline, /*optional*/ titleStr ) {
        
        if (paras == null)
            return;
        var $container = $(container);
        var tCont = null;
        var $tCont = null       
        
        if (className === "dj_emg_title" && headline) {
            var href = (headline.reference.externalUri != null) ? headline.reference.externalUri : "javascript:void(0)";
            
            $container.append("<h3><a href=\"" + href + "\" class=\"" + className + "\"></a></h3>");
            tCont = $( "a." + className , $container).get(0);
            $tCont = $(tCont);
            
            // addIconographySpan
            $.fn.dj_emg_headlineList.addIconographySpan(tCont,headline,options,tokens);
            // paint the importance flag
            
            $.fn.dj_emg_headlineList.addImportanceSpan(tCont,headline,options,tokens);
            // update the title tag;
            
            $tCont.attr("title", $.fn.dj_emg_headlineList.getSnippetParas( options, headline.snippet, tokens, headline.thumbnailImage));
            $tCont.dj_emg_snippetTooltip("dj_emg_tooltip"); 
            $tCont.click(function(e){
                 var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0); 
                 var parentLiTag = $(this).closest("li").get(0);  
                 if (parentContainer){
                    $(parentContainer).triggerHandler("dj.emg.headlineList.headlineClick", {headline: $(parentLiTag).data("headline")});
                 }
                 return false;
            });
                         
        }
        else {
            $container.append("<p class=\"" + className + "\"></p>");
            tCont = $( "p." + className , $container).get(0);               
            $tCont = $(tCont);
        }
        
        if (className === "dj_emg_snippet" && headline != null && headline.thumbnailImage !== null && headline.thumbnailImage.src){
            if ( headline.thumbnailImage.uri) {
                $tCont.append("<a href=\"" +  headline.thumbnailImage.uri + "\" class=\"dj_emg_image\"><img src=\"" +  headline.thumbnailImage.src + "\"/></a>");
            }
            else { 
                $tCont.append("<span class=\"dj_emg_image\"><img src=\"" + headline.thumbnailImage.src + "\"/></span>");
            }
        }
              
        
        var tClassName = "text";
        $.each(paras, function () { 
            $.each(this.items, function () { 
                if (options.displayEntities) {
                    switch(this.type){
                        default:
                        case "Textual":
                            tClassName = "dj_emg_text";                   
                            break;
                        case "Highlight":
                            tClassName = "dj_emg_highlight";
                            break;
                        case "Company":
                            tClassName = "dj_emg_company";
                            break;                
                        case "Person":
                            tClassName = "dj_emg_person";
                            break;                    
                    }

                    var tSpan = ("<span class=\"" + tClassName + "\">" + this.value + "</span>" + "<span class=\"dj_emg_space\"> </span>"); 
                    if ((this.type == "Company" || this.type == "Person")) {
                        var $tSpan = $(tSpan);
                        $tSpan.data("entity", this);
                        $tSpan.hover(function(e) {
                            $(this).addClass('dj_emg_mouseover'); 
                        }, function(e) {                  
                            $(this).removeClass('dj_emg_mouseover');                                       
                        }).click(function(e){
                            var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);   
                            var parentLiTag = $(this).closest("li").get(0);
                            if (parentContainer && parentLiTag) {
                                if ($(parentContainer).data("options").displayEntities){
                                    $(parentContainer).triggerHandler("dj.emg.headlineList.entityClick", {entity: $(this).data("entity"), headline: $(parentLiTag).data("headline")})  
                                }                            
                            }
                            e.stopPropagation();
                            return false;
                        });                    
                    }
                    $tCont.append(tSpan); 
                }
                else {
                    $tCont.append("<span class=\"" + tClassName + "\">" + this.value + "</span>" + "<span class=\"dj_emg_space\"> </span>"); 
                }
            }); 
        });
        
        if (className === "dj_emg_snippet"){
            
            if (!options.displaySnippets || options.displaySnippets === "none" || options.displaySnippets === "hover") {
                if (tCont){$tCont.hide();}
            }
            
            // clean up the snippet, done for rss/atom results
            var linksInSnippets = $("a",$tCont)
            $.each(linksInSnippets, function () { 
                $(this).click(function(e) {
                    var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
                    if (parentContainer)  {
                        $(parentContainer).triggerHandler("dj.emg.headlineList.internalSnippetLink", {href:this.href})
                    }
                    e.stopPropagation();
                    return false;
                });
            });
        } 
    };
    
    $.fn.dj_emg_headlineList.addGutter = function(container, headline, options,tokens) {
        var $container = $(container);
        var t = [];
        t[t.length] = "<span class=\"dj_emg_headline_cbContainer\"><input name=\"dj_emg_headline_cb\" type=\"checkbox\" class=\"dj_emg_headline_cb\" /></span>"; 
        t[t.length] = "<span class=\"dj_emg_index\">" + headline.index.text.Value + ".</span>";
        t[t.length] = "<span class=\"dj_emg_iconography dj_emg_iconography_" + headline.contentSubCategoryDescriptor + "\" ><span>" + tokens[headline.contentSubCategoryDescriptor + "Tkn"] + "</span></span>";     
        $container.html(t.join(""));
        
        var checkboxContainerSpan = $("span.dj_emg_headline_cbContainer", container).get(0); 
        if (checkboxContainerSpan && !options.displayCheckboxes) {
            $(checkboxContainerSpan).hide();
        } 
        
        var indexSpan = $("span.dj_emg_index", container).get(0);          
        if (indexSpan && !options.displayNumbering) {
            $(indexSpan).hide();
        } 
        
        var iconographySpan = $("span.dj_emg_iconography_" + headline.contentSubCategoryDescriptor , container).get(0); 
        $(iconographySpan).attr("title", tokens[headline.contentSubCategoryDescriptor + "Tkn"]);
        $(iconographySpan).dj_emg_simpleTooltip("dj_emg_iconTip"); 
        if (iconographySpan && !options.displayIconography) {
            $(iconographySpan).hide()
        }  
        
    };
    
    $.fn.dj_emg_headlineList.addCheckBox = function(container, headline, options){
        var checkboxContainerSpan = $("<span class=\"dj_emg_headline_cbContainer\"><input name=\"dj_emg_headline_cb\" type=\"checkbox\" class=\"dj_emg_headline_cb\" /></span>"); 
        if (checkboxContainerSpan && !options.displayCheckboxes) {
            $(checkboxContainerSpan).hide();
        } 
        $(container).append(checkboxContainerSpan);
    };
    
    $.fn.dj_emg_headlineList.addIndex = function(container, headline, options) {
        var indexSpan = $("<span class=\"dj_emg_index\">" + headline.index.text.Value + ".</span>");          
        if (indexSpan && !options.displayNumbering) {
            $(indexSpan).hide();
        } 
        $(container).append(indexSpan);     
    };
    
    $.fn.dj_emg_headlineList.addIconographySpan = function(container, headline, options,tokens) {
        var iconographySpan = $("<span class=\"dj_emg_iconography dj_emg_iconography_" + headline.contentSubCategoryDescriptor + "\" ><span>" + tokens[headline.contentSubCategoryDescriptor + "Tkn"] + "</span></span>"); 
        $(iconographySpan).attr("title", tokens[headline.contentSubCategoryDescriptor + "Tkn"]);
        $(iconographySpan).dj_emg_simpleTooltip("dj_emg_iconTip"); 
        if (iconographySpan && !options.displayIconography) {
            $(iconographySpan).hide();
        }   
        $(container).append(iconographySpan);       
    };
    
    $.fn.dj_emg_headlineList.addImportanceSpan = function(container, headline, options,tokens) {
        switch(headline.importance) {
            default:
            case "Normal":
                break;
            case "Breaking_Hot":
            case "New":
            case "MustRead":    
                var importanceSpan = $("<span class=\"dj_emg_importance dj_emg_importance" + headline.importance + "\" ><span>" + headline.importanceDescriptor + "</span></span>");  
                if (importanceSpan && !options.displayImportance) {               
                        $(importanceSpan).hide();
                }
                $(container).append(importanceSpan);
                break;
        }               
    };
    
    $.fn.dj_emg_headlineList.addAction = function(container,options,tokens) {
        var $container = $(container);
        var t = [];
        t[t.length] = "<span class=\"dj_emg_iconography dj_emg_iconography_clip\" title=\"" + tokens.clipTkn +  "\"><span>" + tokens.clipTkn + "</span></span>"; 
        t[t.length] = "<span class=\"dj_emg_iconography dj_emg_iconography_delete\" title=\"" + tokens.deleteTkn + "\" ><span>" + tokens.deleteTkn + " X</span></span>";             
        
        if (options.extension && options.extension.length && options.extension.length>0) {
             t[t.length] = "<span class=\"dj_emg_extension\">" + options.extension + "</span>";
        }
        
        $container.html(t.join(""));
        
        var clipSpan = $("span.dj_emg_iconography_clip", container).get(0);  
        $(clipSpan).dj_emg_simpleTooltip("dj_emg_iconTip"); 
        $(clipSpan).hover(function(e) {
            $(this).addClass('dj_emg_mouseover'); 
        },function(e) {                  
            $(this).removeClass('dj_emg_mouseover'); 
        }).click(function(e){        
            var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
            var parentLiTag = $(this).closest("li").get(0);
            if (parentContainer && parentLiTag) {
                $(parentContainer).triggerHandler("dj.emg.headlineList.headlineClipClick", {headline:$(parentLiTag).data("headline")});
            }   
            e.stopPropagation();   
            return false;                                                
        });
        if (!options.displayClipTrigger) {
            $(clipSpan).hide();
        } 
        
        var deleteSpan = $("span.dj_emg_iconography_delete", container).get(0);  
        $(deleteSpan).dj_emg_simpleTooltip("dj_emg_iconTip"); 
        $(deleteSpan).hover(function(e) {
            $(this).addClass('dj_emg_mouseover'); 
        },function(e) {                  
            $(this).removeClass('dj_emg_mouseover'); 
        }).click(function(e){        
            var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
            var parentLiTag = $(this).closest("li").get(0);
            if (parentContainer && parentLiTag) {
                $(parentContainer).triggerHandler("dj.emg.headlineList.headlineDeleteClick", {headline:$(parentLiTag).data("headline")});
                var prevSiblings =  $(parentLiTag).prevAll();
                var index = prevSiblings.length;
                if ($(parentLiTag).hasClass("dj_emg_last")) {                            
                    if (prevSiblings.length > 0) {
                        $(prevSiblings.get(0)).addClass("dj_emg_last");
                    }
                }
                var headlineResult = $(parentContainer).data("data");
                headlineResult.hitCount.value--;
                headlineResult.resultSet.headlines.remove(index);
				$("#dj_emg_tooltip").remove();                
                $(parentLiTag).remove();
            }   
            e.stopPropagation();      
            return false;                                             
        });
        if (!options.displayDeleteTrigger) {
            $(deleteSpan).hide();
        }
        
        var extensionSpan = $("span.dj_emg_extension", container).get(0); 
        if (extensionSpan) {
            $(extensionSpan).hover(function(e) {
                $(this).addClass('dj_emg_mouseover'); 
            },function(e) {                  
                $(this).removeClass('dj_emg_mouseover'); 
            }).click(function(e){        
                var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
                var parentLiTag = $(this).closest("li").get(0);
                if (parentContainer && parentLiTag) {
                    $(parentContainer).triggerHandler("dj.emg.headlineList.extensionClick", {sender: this, item:parentLiTag, headline:$(parentLiTag).data("headline")});
                }   
                e.stopPropagation();   
                return false;                                           
            });
        }
    };
    
    $.fn.dj_emg_headlineList.addClipSpan = function(container,options,tokens) {
        var clipSpan = $("<span class=\"dj_emg_iconography dj_emg_iconography_clip\" title=\"" + tokens.clipTkn +  "\"><span>" + tokens.clipTkn + "</span></span>");  
        $(clipSpan).dj_emg_simpleTooltip("dj_emg_iconTip"); 
        $(clipSpan).hover(function(e) {
            $(this).addClass('dj_emg_mouseover'); 
        },function(e) {                  
            $(this).removeClass('dj_emg_mouseover'); 
        }).click(function(e){        
            var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
            var parentLiTag = $(this).closest("li").get(0);
            if (parentContainer && parentLiTag) {
                $(parentContainer).triggerHandler("dj.emg.headlineList.headlineClipClick", {headline:$(parentLiTag).data("headline")});
            }   
            e.stopPropagation();     
            return false;                                              
        });
        if (!options.displayClipTrigger) {
            $(clipSpan).hide();
        }       
        $(container).append(clipSpan);        
    };
    
    $.fn.dj_emg_headlineList.addDeleteSpan = function(container,options,tokens) {
        var deleteSpan = $("<span class=\"dj_emg_iconography dj_emg_iconography_delete\" title=\"" + tokens.deleteTkn + "\" ><span>" + tokens.deleteTkn + " X</span></span>");  
        $(deleteSpan).dj_emg_simpleTooltip("dj_emg_iconTip"); 
        $(deleteSpan).hover(function(e) {
            $(this).addClass('dj_emg_mouseover'); 
        },function(e) {                  
            $(this).removeClass('dj_emg_mouseover'); 
        }).click(function(e){        
            var parentContainer = $(this).closest(".dj_emg_headlineListContainer").get(0);
            var parentLiTag = $(this).closest("li").get(0);
            if (parentContainer && parentLiTag) {
                $(parentContainer).triggerHandler("dj.emg.headlineList.headlineDeleteClick", {headline:$(parentLiTag).data("headline")});
                var prevSiblings =  $(parentLiTag).prevAll();
                var index = prevSiblings.length;
                if ($(parentLiTag).hasClass("dj_emg_last")) {                            
                    if (prevSiblings.length > 0) {
                        $(prevSiblings.get(0)).addClass("dj_emg_last");
                    }
                }
                var headlineResult = $(parentContainer).data("data");
                headlineResult.hitCount.value--;
                headlineResult.resultSet.headlines.remove(index);
				$("#dj_emg_tooltip").remove();                
                $(parentLiTag).remove();
            }   
            e.stopPropagation();    
            return false;                                               
        });
        if (!options.displayDeleteTrigger) {
            $(deleteSpan).hide();
        }
        $(container).append(deleteSpan);
    };
    
    $.fn.dj_emg_headlineList.addDetails = function(container, options, headline, tokens) {
        if (headline == null)
            return;
        var $container = $(container);
        var tDiv = $("<p class=\"dj_emg_details\"></p>");
        var t = [];
        var addedContent = false;
        
        if (headline.sourceDescriptor) { 
            t[t.length] = "<span class=\"dj_emg_soruce\">" + headline.sourceDescriptor + "</span>";
            addedContent = true;
        }
        
        if (headline.publicationDateTimeDescriptor) { 
            if(addedContent) t[t.length] = "<span class=\"dj_emg_space\">, </span>";
            t[t.length] = "<span class=\"dj_emg_publicationDate\">" + headline.publicationDateTimeDescriptor + "</span>";
            addedContent = true;
        }
        
        if (headline.wordCountDescriptor) { 
            if(addedContent) t[t.length] = "<span class=\"dj_emg_space\">, </span>";
            t[t.length] = "<span class=\"dj_emg_words\">" + headline.wordCountDescriptor + "</span>";
            addedContent = true;
        }
        
         if (headline.baseLanaguageDescriptor) { 
            if(addedContent) t[t.length] = "<span class=\"dj_emg_space\">, </span>";
            t[t.length] = "<span class=\"dj_emg_language\">" + headline.baseLanaguageDescriptor + "</span>";
            addedContent = true;
        }
        $(tDiv).html(t.join(""));
        $(container).append(tDiv);
    };
    
    
    $.fn.dj_emg_headlineList.getSnippetParas = function(options, paras, tokens, thumbnailImage) {
        var t = [];
        if (paras == null)
            return;
            
        if (thumbnailImage && thumbnailImage.src){
            t[t.length] = "<span class=\"dj_emg_image\"><img src=\"" + thumbnailImage.src + "\"/></span>";
        }
            
        var tClassName;
        $.each(paras, function () { 
            $.each(this.items, function () { 
                switch(this.type){
                    default:
                    case "Textual":
                        tClassName = "dj_emg_text";                   
                        break;
                    case "Highlight":
                        tClassName = "dj_emg_highlight";
                        break;
                    case "Company":
                        tClassName = "dj_emg_company";
                        break;                
                    case "Person":
                        tClassName = "dj_emg_person";
                        break;
                    
                }
                t[t.length] = "<span class=\"" + tClassName + "\">" + this.value + "</span>" + "<span class=\"dj_emg_space\"> </span>"; 
            }); 
        });
        return t.join("");
    };
    
    
    $.fn.dj_emg_headlineList.defaults = {
        debug : false,
        maxNumHeadlinesToShow : -1,
        displaySnippets : "inline" ,//"none,inline,hover",
        displayMoreLikeThis : false,
        displayIconography : true,
        displayDeleteTrigger : false,
        displayClipTrigger : false,
        displayNumbering : false,
        displayCheckboxes : false,
        displayDuplicates : true,
        displayAccessionNumbers : true,
        displayEntities : true,
        displayImportance : true,
        displayNoResultsToken : true,
        enableHeadlineDraging : true,
        removeHeadlineAfterDrop : true,
        droppableId : "",        
        baseClassName : "",
        extension : ""        
    };
    
    $.fn.dj_emg_headlineList.tokens = {
        of : '${of}',
        preDupHeadline : '${preDupHeadline}',
        postDupHeadline : '${postDupHeadline}',
        publication : '${publication}' ,
        website : '${website}' ,
        picture : '${picture}' ,
        noResults: '${noResults}',
        moreLikeThis: '${moreLikeThis}',
        deleteTkn: '${delete}',
        clipTkn: '${clip}',  
        articleTkn: "${article}", 
        summaryTkn: "${summary}", 
        audioTkn: "${audio}", 
        pictureTkn: "${picture}",   
        htmlTkn: "${html}",   
        multimediaTkn: "${multimedia}",   
        pdfTkn: "${pdf}",   
        videoTkn: "${video}",   
        rssTkn: "${rss}",   
        atomTkn: "${atom}"
    };
    
    $.fn.dj_emg_snippetTooltip = function(className){
    
	    return this.each(function() {
		    var text = $(this).attr("title");
		    var wHeight = Math.max($(window))
		    $(this).attr("title", "");
		    if(text != undefined) {
			    $(this).hover(function(e){			        
		         var parentTag = $(this).closest(".dj_emg_headlineListContainer").get(0);
		            $(this).attr("title", ""); 
			        if (parentTag && $(parentTag).data("options").displaySnippets === "hover") {		        
			            $("body").append("<div id='dj_emg_snippetTooltip' class=\"" + className + "\" style='position: absolute; z-index: 100; display: none;'>" + text + "</div>");
			            var $tObj = $("#dj_emg_snippetTooltip");	            
			            var cRight = e.pageX + 12 + $tObj.outerWidth() - $.dj.emg.getHorizontalScroll();
	                    var cBottom = e.pageY + 12 + $tObj.outerHeight() - $.dj.emg.getVerticalScroll();
	                    var tipX = e.pageX + 12;
	                    var tipY = e.pageY + 12;
	                    if (cRight >= $.dj.emg.getClientWidth()) {	                    
                            tipX = e.pageX - 12 - $tObj.outerWidth();
                        }
                        if (cBottom >= $.dj.emg.getClientHeight()) {    	                    
                            tipY = e.pageY - 12 - $tObj.outerHeight();
                        }
	                    $tObj.css("left", tipX).css("top", tipY).fadeIn("fast");		            
			        }
			        		        			        
			    }, function(e){
				    $("#dj_emg_snippetTooltip").remove();
			        $(this).attr("title", text);
			    });
			    $(this).mousemove(function(e){
			        var $tObj = $("#dj_emg_snippetTooltip");	            
		            var cRight = e.pageX + 12 + $tObj.outerWidth() - $.dj.emg.getHorizontalScroll();
	                var cBottom = e.pageY + 12 + $tObj.outerHeight() - $.dj.emg.getVerticalScroll();
	                var tipX = e.pageX + 12;
	                var tipY = e.pageY + 12;
	                if (cRight >= $.dj.emg.getClientWidth()) {	                    
                        tipX = e.pageX - 12 - $tObj.outerWidth();
                    }
                    if (cBottom >= $.dj.emg.getClientHeight()) {    	                    
                        tipY = e.pageY - 12 - $tObj.outerHeight();
                    }
	                $tObj.css("left", tipX).css("top", tipY).fadeIn("fast");	
		        });
		    };
	    });
	};
	
	//
    // end of closure
    //*/
})(jQuery);