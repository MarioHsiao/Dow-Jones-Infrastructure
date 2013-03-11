/* Compiled from X 4.06 with XC 1.0 on 27Nov06 */
function xAddEventListener(e,eT,eL,cap){if(!(e=xGetElementById(e)))return;eT=eT.toLowerCase();if(e==window&&!e.opera&&!document.all){if(eT=='resize'){e.xPCW=xClientWidth();e.xPCH=xClientHeight();e.xREL=eL;xResizeEvent();return;}if(eT=='scroll'){e.xPSL=xScrollLeft();e.xPST=xScrollTop();e.xSEL=eL;xScrollEvent();return;}}if(e.addEventListener)e.addEventListener(eT,eL,cap);else if(e.attachEvent)e.attachEvent('on'+eT,eL);else e['on'+eT]=eL;}function xResizeEvent(){if(window.xREL)setTimeout('xResizeEvent()',250);var w=window,cw=xClientWidth(),ch=xClientHeight();if(w.xPCW!=cw||w.xPCH!=ch){w.xPCW=cw;w.xPCH=ch;if(w.xREL)w.xREL();}}function xScrollEvent(){if(window.xSEL)setTimeout('xScrollEvent()',250);var w=window,sl=xScrollLeft(),st=xScrollTop();if(w.xPSL!=sl||w.xPST!=st){w.xPSL=sl;w.xPST=st;if(w.xSEL)w.xSEL();}}function xEvent(evt){var e=evt||window.event;if(!e)return;if(e.type)this.type=e.type;if(e.target)this.target=e.target;else if(e.srcElement)this.target=e.srcElement;if(e.relatedTarget)this.relatedTarget=e.relatedTarget;else if(e.type=='mouseover'&&e.fromElement)this.relatedTarget=e.fromElement;else if(e.type=='mouseout')this.relatedTarget=e.toElement;if(xDef(e.pageX,e.pageY)){this.pageX=e.pageX;this.pageY=e.pageY;}else if(xDef(e.clientX,e.clientY)){this.pageX=e.clientX+xScrollLeft();this.pageY=e.clientY+xScrollTop();}if(xDef(e.offsetX,e.offsetY)){this.offsetX=e.offsetX;this.offsetY=e.offsetY;}else if(xDef(e.layerX,e.layerY)){this.offsetX=e.layerX;this.offsetY=e.layerY;}else{this.offsetX=this.pageX-xPageX(this.target);this.offsetY=this.pageY-xPageY(this.target);}this.keyCode=e.keyCode||e.which||0;this.shiftKey=e.shiftKey;this.ctrlKey=e.ctrlKey;this.altKey=e.altKey;}xLibrary={version:'4.06',license:'GNU LGPL',url:'http://cross-browser.com/'};function xPreventDefault(e){if(e&&e.preventDefault)e.preventDefault();else if(window.event)window.event.returnValue=false;}function xRemoveEventListener(e,eT,eL,cap){if(!(e=xGetElementById(e)))return;eT=eT.toLowerCase();if(e==window){if(eT=='resize'&&e.xREL){e.xREL=null;return;}if(eT=='scroll'&&e.xSEL){e.xSEL=null;return;}}if(e.removeEventListener)e.removeEventListener(eT,eL,cap);else if(e.detachEvent)e.detachEvent('on'+eT,eL);else e['on'+eT]=null;}function xStopPropagation(evt){if(evt&&evt.stopPropagation)evt.stopPropagation();else if(window.event)window.event.cancelBubble=true;}
function xAddEventListener(e,eT,eL,cap)
{
  if(!(e=xGetElementById(e)))return;
  eT=eT.toLowerCase();
  if(e.addEventListener)e.addEventListener(eT,eL,cap||false);
  else if(e.attachEvent)e.attachEvent('on'+eT,eL);
  else e['on'+eT]=eL;
}
function xDispatchMouseEvent(e,eT,evt,screenX,screenY,clientX,clientY,fireDrag) 
{
  if(!(e=xGetElementById(e)))return;
  if (document.createEvent) 
  { 
    var evObj = document.createEvent('MouseEvents');
   // evObj.initEvent( eT, true, false )
    evObj.initMouseEvent( eT, true, false, window, 1, screenX, screenY, clientX, clientY, false, false, false, false, 0, null );
    e.dispatchEvent(evObj); 
  } 
  else if (document.createEventObject) 
  { 
    if (fireDrag)
        e.fireEvent('ondrag'); 
    else
        e.fireEvent('on'+eT);
  }
}
function xRemoveEventListener(e,eT,eL,cap)
{
  if(!(e=xGetElementById(e)))return;
  eT=eT.toLowerCase();
  if(e.removeEventListener)e.removeEventListener(eT,eL,cap||false);
  else if(e.detachEvent)e.detachEvent('on'+eT,eL);
  else e['on'+eT]=null;
}
function xRemoveEventListener2(e,eT,eL,cap)
{
  if(!(e=xGetElementById(e))) return;
  eT=eT.toLowerCase();
  if(e==window) {
    if(eT=='resize' && e.xREL) {e.xREL=null; return;}
    if(eT=='scroll' && e.xSEL) {e.xSEL=null; return;}
  }
  if(e.removeEventListener) e.removeEventListener(eT,eL,cap||false);
  else if(e.detachEvent) e.detachEvent('on'+eT,eL);
  else e['on'+eT]=null;
}
// original implementation
function xAddEventListener2(e,eT,eL,cap) 
{
  if(!(e=xGetElementById(e))) return;
  eT=eT.toLowerCase();
  if (e==window && !e.opera && !document.all) { 
    // simulate resize and scroll events for all except Opera and IE
    if(eT=='resize') { e.xPCW=xClientWidth(); e.xPCH=xClientHeight(); e.xREL=eL; xResizeEvent(); return; }
    if(eT=='scroll') { e.xPSL=xScrollLeft(); e.xPST=xScrollTop(); e.xSEL=eL; xScrollEvent(); return; }
  }
  if(e.addEventListener) e.addEventListener(eT,eL,cap||false);
  else if(e.attachEvent) e.attachEvent('on'+eT,eL);
  else e['on'+eT]=eL;
}
// called only from the above
function xResizeEvent()
{
  if (window.xREL) setTimeout('xResizeEvent()', 250);
  var w=window, cw=xClientWidth(), ch=xClientHeight();
  if (w.xPCW != cw || w.xPCH != ch) { w.xPCW = cw; w.xPCH = ch; if (w.xREL) w.xREL(); }
}
function xScrollEvent()
{
  if (window.xSEL) setTimeout('xScrollEvent()', 250);
  var w=window, sl=xScrollLeft(), st=xScrollTop();
  if (w.xPSL != sl || w.xPST != st) { w.xPSL = sl; w.xPST = st; if (w.xSEL) w.xSEL(); }
}
function xAddEventListener3(e,eT,eL,cap)
{
  if(!(e=xGetElementById(e))) return;
  eT=eT.toLowerCase();
  if (e==window && !e.opera && !document.all) { 
  // simulate resize and scroll events for all except Opera and IE
    if(eT=='resize') {
      e.xPCW=xClientWidth(); e.xPCH=xClientHeight();
      var pREL = e.xREL ;
      e.xREL= pREL ? function() { eL(); pREL(); } : eL;
      xResizeEvent(); return;
    }
    if(eT=='scroll') {
      e.xPSL=xScrollLeft(); e.xPST=xScrollTop();
      var pSEL = e.xSEL ;
      e.xSEL=pSEL ? function() { eL(); pSEL(); } : eL;
      xScrollEvent(); return; }
  }
  if(e.addEventListener) e.addEventListener(eT,eL,cap);
  else if(e.attachEvent) e.attachEvent('on'+eT,eL);
  else {
    var pev = e['on'+eT] ;
    e['on'+eT]= pev ? function() { eL(); typeof(pev) == 'string' ? eval(pev) : pev(); } : eL ;
  }
}