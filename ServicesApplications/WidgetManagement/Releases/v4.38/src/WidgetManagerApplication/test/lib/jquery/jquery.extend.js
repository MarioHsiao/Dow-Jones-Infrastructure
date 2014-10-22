
// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function(from, to) {
  var rest = this.slice((to || from) + 1 || this.length);
  this.length = from < 0 ? this.length + from : from;
  return this.push.apply(this, rest);
};

// Primitive template language
String.prototype.applyTemplate = function(d) {
    try {
        if (d === '') return this;
        return this.replace(/{([^{}]*)}/g,
            function(a, b) {
                var r;
                if (b.indexOf('.') !== -1) { // handle dot notation in {}, such as {Thumbnail.Url}
                    var ary = b.split('.');
                    var obj = d;
                    for (var i = 0; i < ary.length; i++)
                        obj = obj[ary[i]];
                    r = obj;
                }
                else
                    r = d[b];
                if (typeof r === 'string' || typeof r === 'number') return r; else throw (a);
            }
        );
    } catch (ex) {
        alert('Invalid JSON property ' + ex + ' found when trying to apply resultTemplate or paging.summaryTemplate.\nPlease check your spelling and try again.');
    }
};

//
// create closure
//
(function($) {
    $.dj = {
        emg : {
            
            debug : function (text) {
                if (window.console && window.console.log) {
                    window.console.log(text);
                }
                if (window.opera) {
                    window.opera.postError(text);
                }
                if (window.debugService) {
                    window.debugService.trace(text);
                }
            },
            
            def : function() {
              for(var i=0; i<arguments.length; ++i){if(typeof(arguments[i])=='undefined') return false;}
              return true;
            },

            getClientWidth : function () {
                var v=0,d=document,w=window;
                if((!d.compatMode || d.compatMode == 'CSS1Compat') && d.documentElement && d.documentElement.clientWidth)
                    {v=d.documentElement.clientWidth;}
                else if(d.body && d.body.clientWidth)
                    {v=d.body.clientWidth;}
                else if(this.def(w.innerWidth,w.innerHeight,d.height)) {
                    v=w.innerWidth;
                    if(d.height>w.innerHeight) v-=16;
                }
                return v;      
            },
            
            getClientHeight : function () {
                var v=0,d=document,w=window;
                if((!d.compatMode || d.compatMode == 'CSS1Compat') && d.documentElement && d.documentElement.clientHeight)
                    {v=d.documentElement.clientHeight;}
                else if(d.body && d.body.clientHeight)
                    {v=d.body.clientHeight;}
                else if(this.def(w.innerWidth,w.innerHeight,d.width)) {
                    v=w.innerHeight;
                    if(d.width>w.innerWidth) v-=16;
                }
                return v;        
            },
            
            getHorizontalScroll : function () {
                var d=document,w=window;
                var offset = 0;
                if (w.pageXOffset) { // All but IE
                    offset = w.pageXOffset;
                }
                else if (w.document.documentElement &&  // IE6 w/ doctype
                         w.document.documentElement.scrollLeft) {
                    offset = w.document.documentElement.scrollLeft;
                }
                else if (w.document.body.scrollLeft) { // IE4,5,6(w/o doctype)
                    offset = w.document.body.scrollLeft;
                }  
                if (this.isNum(offset)) {
                    return offset;
                }
                return 0;      
            },
            
            getVerticalScroll : function () {
                var d=document,w=window;
                var offset = 0;
                if (w.pageYOffset) { // All but IE
                    offset =  w.pageYOffset;
                }
                else if (w.document.documentElement &&  // IE6 w/ doctype
                        w.document.documentElement.scrollTop) {
                    offset =  w.document.documentElement.scrollTop;
                }
                else if (w.document.body.scrollTop) { // IE4,5,6(w/o doctype)
                    offset = w.document.body.scrollTop;
                }   
                 if (this.isNum(offset)) {
                    return offset;
                }
                return 0;     
            },
            
            trim : function(str, doNotRemoveSpecialChars) {
                if (!doNotRemoveSpecialChars) {
	                for(var i=0;i<str.length;i++){
		                if (str.charCodeAt(i) <= 32){
			                str = str.substring(0,i) + " " + str.substr(i+1);
		                }
	                }
	            }
	            str = str.replace(/^[\s]+/g,"");
	            str = str.replace(/[\s]+$/g,"");
	            return str;
            },
            
            cStr : function(vVariant) {
                try{
		            var str = new String(vVariant).toString();
		            if(str == "undefined" || str == "null")
			            str = "";
		            }
	            catch(e){str = "";}
	            return this.trim(str);
            },
            
            cInt : function(vVariant) {
                try{
		            var num = 0;
		            if (this.cStr(vVariant) != "" && !isNaN(vVariant))
			            num = parseInt(vVariant);
	            }
	            catch(e){num = 0;}
	            return num;
            },
            
            cFloat : function(vVariant) {
                try{
		            var num = 0;
		            if (this.cStr(vVariant) != "" && !isNaN(vVariant))
			            num = parseFloat(vVariant);
	            }
	            catch(e){num = 0;}
	            return num;
            },
            
            isNumeric : function(vVariant){
	            try {
		            var num = 0;
		            var numeric = false;
		            if (this.cStr(vVariant) != "" && !isNaN(vVariant)){
			            num = parseFloat(vVariant);
			            numeric = true;
		            }
	            }
	            catch(e){numeric = false;}
	            return numeric;
            },
            
            isNum : function() {
                for(var i=0; i<arguments.length; ++i){if(isNaN(arguments[i]) || typeof(arguments[i])!='number') return false;}
                return true;
            }                       
        }
    };
    
    // Core Plugins
    
    // Simple Tooltip plugin
    $.fn.dj_emg_simpleTooltip = function(className){
        return this.each(function() {
		    var sText = $(this).attr("title");
		    $(this).attr("title", "");
		    if(sText != undefined) {
			    $(this).hover(function(e){
		            $(this).attr("title", ""); 
		            var tipX = e.pageX + 12;
		            var tipY = e.pageY + 12;    			        
		            $("body").append("<div id='dj_emg_tooltip' class=\"" + className + "\" style='position: absolute; z-index: 100; display: none;'>" + sText + "</div>");
		            var $tObj = $("#dj_emg_tooltip");            
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
			    }, function(e){
				    $("#dj_emg_tooltip").remove();
				    $(this).attr("title", sText);
			    });
			    $(this).mousemove(function(e){
				    var $tObj = $("#dj_emg_tooltip");		            
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

})(jQuery);


// Extentions to the jQuery Library
jQuery.extend({
    createUploadIframe: function(id, uri)
	{
			//create frame
            var frameId = 'jUploadFrame' + id;
            
            if(window.ActiveXObject) {
                var io = document.createElement('<iframe id="' + frameId + '" name="' + frameId + '" />');
                if(typeof uri== 'boolean'){
                    io.src = 'javascript:false';
                }
                else if(typeof uri== 'string'){
                    io.src = uri;
                }
            }
            else {
                var io = document.createElement('iframe');
                io.id = frameId;
                io.name = frameId;
            }
            io.style.position = 'absolute';
            io.style.top = '-1000px';
            io.style.left = '-1000px';

            document.body.appendChild(io);

            return io			
    },
    createUploadForm: function(id, fileElementId)
	{
		//create form	
		var formId = 'jUploadForm' + id;
		var fileId = 'jUploadFile' + id;
		var form = $('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');	
		var oldElement = $('#' + fileElementId);
		var newElement = $(oldElement).clone();
		$(oldElement).attr('id', fileId);
		$(oldElement).before(newElement);
		$(oldElement).appendTo(form);
		//set attributes
		$(form).css('position', 'absolute');
		$(form).css('top', '-1200px');
		$(form).css('left', '-1200px');
		$(form).appendTo('body');		
		return form;
    },

    ajaxFileUpload: function(s) {
        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout		
        s = jQuery.extend({}, jQuery.ajaxSettings, s);
        var id = new Date().getTime();        
		var form = jQuery.createUploadForm(id, s.fileElementId);
		var io = jQuery.createUploadIframe(id, s.secureuri);
		var frameId = 'jUploadFrame' + id;
		var formId = 'jUploadForm' + id;		
        // Watch for a new set of requests
        if ( s.global && ! jQuery.active++ )
		{
			jQuery.event.trigger( "ajaxStart" );
		}            
        var requestDone = false;
        // Create the request object
        var xml = {}   
        if ( s.global )
            jQuery.event.trigger("ajaxSend", [xml, s]);
        // Wait for a response to come back
        var uploadCallback = function(isTimeout)
		{			
			var io = document.getElementById(frameId);
            try 
			{				
				if(io.contentWindow)
				{
					 xml.responseText = io.contentWindow.document.body?io.contentWindow.document.body.innerHTML:null;
                	 xml.responseXML = io.contentWindow.document.XMLDocument?io.contentWindow.document.XMLDocument:io.contentWindow.document;
					 
				}else if(io.contentDocument)
				{
					 xml.responseText = io.contentDocument.document.body?io.contentDocument.document.body.innerHTML:null;
                	xml.responseXML = io.contentDocument.document.XMLDocument?io.contentDocument.document.XMLDocument:io.contentDocument.document;
				}						
            }catch(e)
			{
				jQuery.handleError(s, xml, null, e);
			}
            if ( xml || isTimeout == "timeout") 
			{				
                requestDone = true;
                var status;
                try {
                    status = isTimeout != "timeout" ? "success" : "error";
                    // Make sure that the request was successful or notmodified
                    if ( status != "error" )
					{
                        // process the data (runs the xml through httpData regardless of callback)
                        var data = jQuery.uploadHttpData( xml, s.dataType );    
                        // If a local callback was specified, fire it and pass it the data
                        if ( s.success )
                            s.success( data, status );
    
                        // Fire the global callback
                        if( s.global )
                            jQuery.event.trigger( "ajaxSuccess", [xml, s] );
                    } else
                        jQuery.handleError(s, xml, status);
                } catch(e) 
				{
                    status = "error";
                    jQuery.handleError(s, xml, status, e);
                }

                // The request was completed
                if( s.global )
                    jQuery.event.trigger( "ajaxComplete", [xml, s] );

                // Handle the global AJAX counter
                if ( s.global && ! --jQuery.active )
                    jQuery.event.trigger( "ajaxStop" );

                // Process result
                if ( s.complete )
                    s.complete(xml, status);

                jQuery(io).unbind()

                setTimeout(function()
									{	try 
										{
											$(io).remove();
											$(form).remove();	
											
										} catch(e) 
										{
											jQuery.handleError(s, xml, null, e);
										}									

									}, 100)

                xml = null

            }
        }
        // Timeout checker
        if ( s.timeout > 0 ) 
		{
            setTimeout(function(){
                // Check to see if the request is still happening
                if( !requestDone ) uploadCallback( "timeout" );
            }, s.timeout);
        }
        try 
		{
           // var io = $('#' + frameId);
			var form = $('#' + formId);
			$(form).attr('action', s.url);
			$(form).attr('method', 'POST');
			$(form).attr('target', frameId);
            if(form.encoding)
			{
                form.encoding = 'multipart/form-data';				
            }
            else
			{				
                form.enctype = 'multipart/form-data';
            }			
            $(form).submit();

        } catch(e) 
		{			
            jQuery.handleError(s, xml, null, e);
        }
        if(window.attachEvent){
            document.getElementById(frameId).attachEvent('onload', uploadCallback);
        }
        else{
            document.getElementById(frameId).addEventListener('load', uploadCallback, false);
        } 		
        return {abort: function () {}};	

    },

    uploadHttpData: function( r, type ) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if ( type == "script" )
            jQuery.globalEval( data );
        // Get the JavaScript object, if JSON is used.
        if ( type == "json" )
            eval( "data = " + data );
        // evaluate scripts within html
        if ( type == "html" )
            jQuery("<div>").html(data).evalScripts();
			//alert($('param', data).each(function(){alert($(this).attr('value'));}));
        return data;
    }
})

