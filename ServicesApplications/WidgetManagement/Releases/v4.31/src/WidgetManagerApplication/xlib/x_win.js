/* Compiled from X 4.06 with XC 1.0 on 27Nov06 */
xLibrary={version:'4.06',license:'GNU LGPL',url:'http://cross-browser.com/'};
function xWinClass(clsName,winName,w,h,x,y,loc,men,res,scr,sta,too){var thisObj=this;var e='',c=',',xf='left=',yf='top=';this.n=name;if(document.layers){xf='screenX=';yf='screenY=';}this.f=(w?'width='+w+c:e)+(h?'height='+h+c:e)+(x>=0?xf+x+c:e)+(y>=0?yf+y+c:e)+'location='+loc+',menubar='+men+',resizable='+res+',scrollbars='+scr+',status='+sta+',toolbar='+too;this.opened=function(){return this.w&&!this.w.closed;};this.close=function(){if(this.opened())this.w.close();};this.focus=function(){if(this.opened())this.w.focus();};this.load=function(sUrl){if(this.opened())this.w.location.href=sUrl;else this.w=window.open(sUrl,this.n,this.f);this.focus();return false;};
function onClick(){return thisObj.load(this.href);}
xGetElementsByClassName(clsName,document,'*',bindOnClick);
function bindOnClick(e){e.onclick=onClick;}}
function xWindow(name,w,h,x,y,loc,men,res,scr,sta,too){var e='',c=',',xf='left=',yf='top=';this.n=name;if(document.layers){xf='screenX=';yf='screenY=';}this.f=(w?'width='+w+c:e)+(h?'height='+h+c:e)+(x>=0?xf+x+c:e)+(y>=0?yf+y+c:e)+'location='+loc+',menubar='+men+',resizable='+res+',scrollbars='+scr+',status='+sta+',toolbar='+too;this.opened=function(){return this.w&&!this.w.closed;};this.close=function(){if(this.opened())this.w.close();};this.focus=function(){if(this.opened())this.w.focus();};this.load=function(sUrl){if(this.opened())this.w.location.href=sUrl;else this.w=window.open(sUrl,this.n,this.f);this.focus();return false;};}var xChildWindow=null;function xWinOpen(sUrl){var features="left=0,top=0,width=600,height=500,location=0,menubar=0,"+"resizable=1,scrollbars=1,status=0,toolbar=0";if(xChildWindow&&!xChildWindow.closed){xChildWindow.location.href=sUrl;}else{xChildWindow=window.open(sUrl,"myWinName",features);}xChildWindow.focus();return false;}var xWinScrollWin=null;function xWinScrollTo(win,x,y,uTime){var e=win;if(!e.timeout)e.timeout=25;var st=xScrollTop(e,1);var sl=xScrollLeft(e,1);e.xTarget=x;e.yTarget=y;e.slideTime=uTime;e.stop=false;e.yA=e.yTarget-st;e.xA=e.xTarget-sl;if(e.slideLinear)e.B=1/e.slideTime;else e.B=Math.PI/(2*e.slideTime);e.yD=st;e.xD=sl;var d=new Date();e.C=d.getTime();if(!e.moving){xWinScrollWin=e;_xWinScrollTo();}}function _xWinScrollTo(){var e=xWinScrollWin||window;var now,s,t,newY,newX;now=new Date();t=now.getTime()-e.C;if(e.stop){e.moving=false;}else if(t<e.slideTime){setTimeout("_xWinScrollTo()",e.timeout);s=e.B*t;if(!e.slideLinear)s=Math.sin(s);newX=Math.round(e.xA*s+e.xD);newY=Math.round(e.yA*s+e.yD);e.scrollTo(newX,newY);e.moving=true;}else{e.scrollTo(e.xTarget,e.yTarget);xWinScrollWin=null;e.moving=false;if(e.onslideend)e.onslideend();}}
// xWinOpen()
// A simple alternative to xWindow.
var xChildWindow = {};
function xWinOpen(sUrl,w,h,winName)
{
 var winWidth = 800;
 var winHeight = 500;
 if (w && w!=null) winWidth = w;
 if (h && h!=null) winHeight = h; 
 if (!winName) {
	winName = "_factivaWindow"; 
 }
 var features = "left=0,top=0,width=" + winWidth + ",height=" + winHeight + ",location=0,menubar=1," +
	"resizable=1,scrollbars=1,status=1,toolbar=0";
  if (xChildWindow[winName] && !xChildWindow[winName].closed) { xChildWindow[winName].location.href = sUrl; try {xChildWindow[winName].resizeTo(winWidth,winHeight);} catch(e){} }
  else { xChildWindow[winName] = window.open(sUrl, winName, features); try {xChildWindow[winName].resizeTo(winWidth,winHeight);} catch(e){} }
  try {xChildWindow[winName].focus();} catch(e){}
  return false;
}
function xWinOpenFullWindow(sUrl,winName) {
  if (!winName) {
	winName = "_factivaFullWindow"; 
  }
  if (xChildWindow[winName] && !xChildWindow[winName].closed) { xChildWindow[winName].location.href = sUrl; } 
  else { xChildWindow[winName] = window.open(sUrl, winName); }
  try {xChildWindow[winName].focus();} catch(e){}
  return false;
}

// xPostOpenWin
var tempForm = null;
var tempFormAction = null;
var tempFormTarget = null;
function xPostOpenWin(action) {
	// open the window
	var form = getBForm();
	if ( form ) {
		_xPostOpenWin(form,action,"_factivaFullWindow");
	}
}
function _xPostOpenWin(form,action,winName){
    if ( form ) {
		xWinOpenFullWindow("about:blank", winName);
		if (action && typeof(action)== "string") form.action = action;
		if (winName && typeof(winName)== "string") form.target = winName;
		form.submit();
	}
}

function xPostReadSpeakerWin(contentId,lang) {
    var cText = "";
    var rsform = document.rs_form;
    if (rsform) {
        if(document.all) {
            cText=document.getElementById(contentId).innerText;
        } 
        else {
            cText= document.getElementById(contentId).textContent;
        }   

        /*
        if (window.location.href) { 
            rsform.url.value = window.location.href; 
        } 
        else if (document.location.href) { 
            rsform.url.value = document.location.href; 
        }*/
        rsform.rstext.value = cText;
        rsform.lang.value = lang;
        _xPostOpenWin(rsform,null,"rs");
    }    
}

function post(action){
	xPost(action);
}
function xPost(action) {
	var form = getBForm();
	if ( form ) {
		form.action = action;
		form.target = "";
		form.submit();
	}
	else window.location.href = action;
}

function resetFormObj() {
	if ( tempForm && tempFormObj != null) {
		tempFormObj.target = tempFormTarget;
		tempFormObj.action = tempFormAction;
		tempFormObj = null;
	}
}

function xPostSelectOption( selectObj, baseUrl, parameterName ) {
    baseUrl = baseUrl.replace("&apos;","'");
    if (baseUrl.indexOf("?") == -1) {
	    baseUrl += "?";
	}
	baseUrl += "&" + parameterName + "=" + selectObj.options[selectObj.selectedIndex].value;
	xPost(baseUrl);	
    return false;
}