
/* for Mozilla */
if (document.addEventListener) {
    document.addEventListener("DOMContentLoaded", window_OnLoad, false);
}
//! for Internet Explorer (using conditional comments)
/*@cc_on @*/
/*@if (@_win32)
	document.write("<script id=__ie_onload defer src=javascript:void(0)><\/script>");
	var script = document.getElementById("__ie_onload");
	script.onreadystatechange = function() {    
		if (this.readyState == "complete") {        
		window_OnLoad(); //! call the onload handler    
		}
	};
/*@end @*/
//! for Safari browsers
if (/WebKit/i.test(navigator.userAgent)) { //! sniff    
	var _timer = setInterval(function() {        
		if (/loaded|complete/.test(document.readyState)) {            
			clearInterval(_timer);            
			window_OnLoad(); //! call the onload handler        
		}    
	}, 10);
}

window.onload = window_OnLoad;