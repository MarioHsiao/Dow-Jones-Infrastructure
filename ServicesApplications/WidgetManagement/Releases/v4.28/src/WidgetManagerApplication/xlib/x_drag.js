/* Compiled from X 4.06 with XC 1.0 on 27Nov06 */
//function xDisableDrag(id,last){if(!window._xDrgMgr)return;var ele=xGetElementById(id);ele.xDraggable=false;ele.xODS=null;ele.xOD=null;ele.xODE=null;xRemoveEventListener(ele,'mousedown',_xOMD,false);if(_xDrgMgr.mm&&last){_xDrgMgr.mm=false;xRemoveEventListener(document,'mousemove',_xOMM,false);}}var _xDrgMgr={ele:null,mm:false};function xEnableDrag(id,fS,fD,fE){var ele=xGetElementById(id);ele.xDraggable=true;ele.xODS=fS;ele.xOD=fD;ele.xODE=fE;xAddEventListener(ele,'mousedown',_xOMD,false);if(!_xDrgMgr.mm){_xDrgMgr.mm=true;xAddEventListener(document,'mousemove',_xOMM,false);}}function _xOMD(e){var evt=new xEvent(e);var ele=evt.target;while(ele&&!ele.xDraggable){ele=xParent(ele);}if(ele){xPreventDefault(e);ele.xDPX=evt.pageX;ele.xDPY=evt.pageY;_xDrgMgr.ele=ele;xAddEventListener(document,'mouseup',_xOMU,false);if(ele.xODS){ele.xODS(ele,evt.pageX,evt.pageY);}}}function _xOMM(e){var evt=new xEvent(e);if(_xDrgMgr.ele){xPreventDefault(e);var ele=_xDrgMgr.ele;var dx=evt.pageX-ele.xDPX;var dy=evt.pageY-ele.xDPY;ele.xDPX=evt.pageX;ele.xDPY=evt.pageY;if(ele.xOD){ele.xOD(ele,dx,dy);}else{xMoveTo(ele,xLeft(ele)+dx,xTop(ele)+dy);}}}function _xOMU(e){if(_xDrgMgr.ele){xPreventDefault(e);xRemoveEventListener(document,'mouseup',_xOMU,false);if(_xDrgMgr.ele.xODE){var evt=new xEvent(e);_xDrgMgr.ele.xODE(_xDrgMgr.ele,evt.pageX,evt.pageY);}_xDrgMgr.ele=null;}}xLibrary={version:'4.06',license:'GNU LGPL',url:'http://cross-browser.com/'};
var _xDrgMgr = {ele:null, mm:false};
function xDisableDrag(id, last){
	if (!window._xDrgMgr) return;
	var ele = xGetElementById(id);
	ele.xDraggable = false;ele.xODS = null;
	ele.xOD = null;
	ele.xODE = null;
	if (document.body.ondragstart) {
		xRemoveEventListener(ele, 'dragstart', _xOMD, false);
	}
	else {	
		xRemoveEventListener(ele, 'mousedown', _xOMD, false);
	}
	if (_xDrgMgr.mm && last) {
		_xDrgMgr.mm = false;
	    if (document.body.ondrag) {
			xRemoveEventListener(ele, 'drag', _xOMM, false);
		}
		else {
			xRemoveEventListener(ele, 'mousemove', _xOMM, false);
		}
	}
}

function xEnableDrag(id,fS,fD,fE,fireMouseMove) {
	var ele = xGetElementById(id);
	ele.xDraggable = true;
	ele.xODS = fS;
	ele.xOD = fD;
	ele.xODE = fE;
	if (document.body.ondragstart) {
		xAddEventListener(document, 'dragstart', _xOMD, false);
	}
	else
		xAddEventListener(document, 'mousedown', _xOMD, false);
	if (!_xDrgMgr.mm) {
		_xDrgMgr.mm = true;
		if (document.body.ondrag) {
			xAddEventListener(document, 'drag', _xOMM, false);
		}
		else {
			xAddEventListener(document, 'mousemove', _xOMM, false);
		}
	}
}

function _xOMD(e) {
	var evt = new xEvent(e);
	var ele = evt.target;
	while(ele && !ele.xDraggable) {
		ele = xParent(ele);
	}
	if (ele) {
		xPreventDefault(e);
		ele.xDPX = evt.pageX;
		ele.xDPY = evt.pageY;

		_xDrgMgr.ele = ele;
		if (document.body.ondragend) {
			xAddEventListener(document, 'dragend', _xOMU, false);
		}
		else  {
			xAddEventListener(document, 'mouseup', _xOMU, false);
		}
		if (ele.xODS) {
			ele.xODS(ele, evt.pageX, evt.pageY);
		}
	}
}
function _xOMM(e) {
	var evt = new xEvent(e);
	if (_xDrgMgr.ele) {
		xPreventDefault(e);
		var ele = _xDrgMgr.ele;
		var dx = evt.pageX - ele.xDPX;
		var dy = evt.pageY - ele.xDPY;
		ele.xDPX = evt.pageX;
		ele.xDPY = evt.pageY;
		if (ele.xOD) {
			ele.xOD(ele, dx, dy);
		}
		else {
			xMoveTo(ele, xLeft(ele) + dx, xTop(ele) + dy);
		}	
		_clear();	
	}  
}

function _xOMU(e) {
	if (_xDrgMgr.ele) {
		xPreventDefault(e);
		var ele = _xDrgMgr.ele;
		if (document.body.ondragend) {
			xRemoveEventListener(document, 'dragend', _xOMU, false);
		}
		else {
			xRemoveEventListener(document, 'mouseup', _xOMU, false);
		}
		if (_xDrgMgr.ele.xODE) {
			var evt = new xEvent(e);
			_xDrgMgr.ele.xODE(_xDrgMgr.ele, evt.pageX, evt.pageY);
		}
		_xDrgMgr.ele = null;
		_clear();
	}  
}


function _clear(){
    if( document.selection && document.selection.empty ) {
        document.selection.empty();
    } else if(window.getSelection) {
        var sel = window.getSelection();
        if(sel.removeAllRanges) {
          window.getSelection().removeAllRanges();          
        }
    }
}