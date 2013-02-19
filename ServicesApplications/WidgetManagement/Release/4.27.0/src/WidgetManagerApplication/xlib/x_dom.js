/* Compiled from X 4.06 with XC 1.0 on 27Nov06 */
function xAppendChild(oParent,oChild){if(oParent.appendChild)return oParent.appendChild(oChild);else return null;}
function xCreateElement(sTag){if(document.createElement)return document.createElement(sTag);else return null;}
function xFirstChild(e,t){if(!(e=xGetElementById(e)))return;var c=e?e.firstChild:null;if(t)while(c&&c.nodeName.toLowerCase()!=t.toLowerCase()){c=c.nextSibling;}else while(c&&c.nodeType!=1){c=c.nextSibling;}return c;}
function xGetComputedStyle(oEle,sProp,bInt){var s,p='undefined';var dv=document.defaultView;if(dv&&dv.getComputedStyle){s=dv.getComputedStyle(oEle,'');if(s)p=s.getPropertyValue(sProp);}else if(oEle.currentStyle){var i,c,a=sProp.split('-');sProp=a[0];for(i=1;i<a.length;++i){c=a[i].charAt(0);sProp+=a[i].replace(c,c.toUpperCase());}p=oEle.currentStyle[sProp];}else return null;return bInt?(parseInt(p)||0):p;}
function xGetElementsByAttribute(sTag,sAtt,sRE,fn){var a,list,found=new Array(),re=new RegExp(sRE,'i');list=xGetElementsByTagName(sTag);for(var i=0;i<list.length;++i){a=list[i].getAttribute(sAtt);if(!a){a=list[i][sAtt];}if(typeof(a)=='string'&&a.search(re)!=-1){found[found.length]=list[i];if(fn)fn(list[i]);}}return found;}
function xGetElementsByClassName(c,p,t,f){var found=new Array();var re=new RegExp('\\b'+c+'\\b','i');var list=xGetElementsByTagName(t,p);for(var i=0;i<list.length;++i){if(list[i].className&&list[i].className.search(re)!=-1){found[found.length]=list[i];if(f)f(list[i]);}}return found;}
function xGetElementsByTagName(t,p){var list=null;t=t||'*';p=p||document;if(p.getElementsByTagName){list=p.getElementsByTagName(t);if(t=='*'&&(!list||!list.length))list=p.all;}else{if(t=='*')list=p.all;else if(p.all&&p.all.tags)list=p.all.tags(t);}return list||new Array();}
function xInnerHtml(e,h){if(!(e=xGetElementById(e))||!xStr(e.innerHTML))return null;var s=e.innerHTML;if(xStr(h)){e.innerHTML=h;}return s;}xLibrary={version:'4.06',license:'GNU LGPL',url:'http://cross-browser.com/'};
function xNextSib(e,t){if(!(e=xGetElementById(e)))return;var s=e?e.nextSibling:null;if(t)while(s&&s.nodeName.toLowerCase()!=t.toLowerCase()){s=s.nextSibling;}else while(s&&s.nodeType!=1){s=s.nextSibling;}return s;}
function xParentNode(ele,n){while(ele&&n--){ele=ele.parentNode;}return ele;}
function xPrevSib(e,t){if(!(e=xGetElementById(e)))return;var s=e?e.previousSibling:null;if(t)while(s&&s.nodeName.toLowerCase()!=t.toLowerCase()){s=s.previousSibling;}else while(s&&s.nodeType!=1){s=s.previousSibling;}return s;}
function xWalkEleTree(n,f,d,l,b){if(typeof l=='undefined')l=0;if(typeof b=='undefined')b=0;var v=f(n,l,b,d);if(!v)return 0;if(v==1){for(var c=n.firstChild;c;c=c.nextSibling){if(c.nodeType==1){if(!l)++b;if(!xWalkEleTree(c,f,d,l+1,b))return 0;}}}return 1;}
function xWalkTree(n,f){f(n);for(var c=n.firstChild;c;c=c.nextSibling){if(c.nodeType==1)xWalkTree(c,f);}}
// Function that have been added or augmented
function xGetElementsByName(name, parentEle, tagName, fn){var found = new Array(); var re = new RegExp('\\b'+name+'\\b', 'i'); var list = xGetElementsByTagName(tagName, parentEle); for (var i = 0; i < list.length; ++i) {if (list[i].name.search(re) != -1) {found[found.length] = list[i]; if (fn) fn(list[i]);}} return found; }
function xRemoveChild(oParent, oChild){if (oParent.removeChild) return oParent.removeChild(oChild);else return null;}
function xInsertBefore(oParent, oNode, oChild) { if (oParent.insertBefore) return oParent.insertBefore(oNode,oChild);}
function xCloneNode(oNode){if (oNode.cloneNode) return oNode.cloneNode(true);else return null;}
function xCloneLightNode(oNode){if (oNode.cloneNode) return oNode.cloneNode(false);else return null;}
function xRemoveChildren(oNode){ 
	if (typeof(oNode) != 'undefined' && oNode != null){return ;}  
	var len = oNode.childNodes.length;
	while (oNode.hasChildNodes()) {
		xRemoveChild(oNode, oNode.firstChild);
	}
}
function xParentElement(node,tagName,className){
	/* USE: TO FIND THE PARENT OF A NODE USING TAGNAME AND CLASSNAME AS THE SEARCH CRETERIA. */
	/* NOTE: ONLY RETURNS THE FIRST NODE FOUND */
	for(;node != null; node = node.parentNode){
		if (className && className.length > 0){
			if (node.nodeType == 1 && node.nodeName.toUpperCase() == tagName.toUpperCase() && (node.className == className || node.id == className)) 
				return node;
		}
		else if (node.nodeType == 1 && node.nodeName.toUpperCase() == tagName.toUpperCase())
			return node;
	}
	return null;
}
function xFindChildNode(nodeObj,tn,cn) {
	cn = (cn)? cStr(cn):"";
	for (var i=0; i<nodeObj.childNodes.length;i++) {
		switch (nodeObj.childNodes[i].nodeType) {
			case 1:
				tVal = null;
				if ( nodeObj.childNodes[i].tagName == tn.toUpperCase() && 
					(nodeObj.childNodes[i].className == cn || nodeObj.childNodes[i].id == cn) ) {
					return nodeObj.childNodes[i];
				}
				else {
					tVal = findChildNode(nodeObj.childNodes[i],tn,cn);
					if (tVal) return tVal;
				}
				break;
		}
	}
}
function xChildElement(node,tagName,className,index){
	/* USE: TO FIND THE CHILDELEMENTS OF A NODE USING TAGNAME AND CLASSNAME AS THE SEARCH RETERIA. */
	/* NOTE: ONLY RETURNS THE FIRST NODE FOUND */
	if ( node && node.hasChildNodes){
		var sInd = 1;
		for (var i=0;i < node.childNodes.length; i++) {
			childNode = node.childNodes[i];
			if (className && className.length > 0){
				if (childNode.nodeType == 1 && childNode.nodeName.toLowerCase() == tagName.toLowerCase() && (childNode.className == className || childNode.id == className)){
					if (cInt(index) != 0) {
						if (cInt(index) == sInd){
							return childNode;
						}
						sInd++;
					}else{
						return childNode;
					}
				} 
			}
			else if(childNode.nodeType == 1 && childNode.nodeName.toLowerCase() == tagName.toLowerCase()) {
					return childNode;		
		    }
		}
	}
	return null;
}
function xInnerText(oNode){
	/* Not needed but it is faster for IE browsers */
	if (oNode.innerText) return oNode.innerText;	
		
	var str = "";
	for (var i=0; i<oNode.childNodes.length; i++){
		switch (oNode.childNodes.item(i).nodeType){
			case 1: 
				/* ELEMENT_NODE */
				str += xInnerText(oNode.childNodes[i]);
				break;
			case 3:	
				/* TEXT_NODE */
				str += oNode.childNodes[i].nodeValue;
				break;
		}		
	}	
	return str;
}

