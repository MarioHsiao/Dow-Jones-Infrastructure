function getBForm() {
	return document.forms['PageBaseForm'];
}
function getTForm() {
	return document.forms['SearchBarForm'];
}
function getRedirectionForm() {
    return document.forms['RedirectionForm'];
}

function postBar(action){
	var form = getTForm();
	if ( form ) {
		form.action = action;
		form.submit();
	}
	else window.location.href = action;
}

function validateEmail(str) {
	str = trim(str);
	if(str.length == 0 || str.length > 80) {
		return false;
	} else if(str.indexOf("@")==-1 || (str.indexOf("@")!=str.lastIndexOf("@")) || str.indexOf(" ")!=-1) {
		return false;
	} else if(str.indexOf(".")==-1) {
		return false
	} else {
		return true;
	}
}

function hasIllegalChar(str){
	if (str.match(/[<>&#\\%+|]/)){
		return true;
	} else {
		return false;
	}
}

function trim(str, doNotRemoveSpecialChars){ 

	/* this code replicated in login.asp, make changes in both places. */
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
}

function cStr(vVariant){
	try{
		var str = new String(vVariant).toString();
		if(str == "undefined" || str == "null")
			str = "";
		}
	catch(e){str = "";}
	return str;
}

function cInt(vVariant){
	try{
		var num = 0;
		if (cStr(vVariant) != "" && !isNaN(vVariant))
			num = parseInt(vVariant);
	}
	catch(e){num = 0;}
	return num;
}

function cFloat(vVariant){
	try{
		var num = 0;
		if (cStr(vVariant) != "" && !isNaN(vVariant))
			num = parseFloat(vVariant);
	}
	catch(e){num = 0;}
	return num;
}

function isNumeric(vVariant){
	try {
		var num = 0;
		var numeric = false;
		if (cStr(vVariant) != "" && !isNaN(vVariant)){
			num = parseFloat(vVariant);
			numeric = true;
		}
	}
	catch(e){numeric = false;}
	return numeric;
}

/* PROTOTYPES ADDED TO ARRAY OBJECTS */
function deleteIndex(index){
	if (index == -1)
	return this;
	else if (index == 0)
		return this.slice(1);
	else if (index == (this.length - 1))
		return this.slice(0,(this.length - 1));
	else{
		tempArr1 = this.slice(0,index);
    	tempArr2 = this.slice(index + 1);
		returnArr = tempArr1.concat(tempArr2);
	}
	return returnArr;
}
Array.prototype.deleteIndex = deleteIndex;
	
function findIndex(value){
	for(i=0; i < this.length; i++)
		if(this[i] == value)
			return (i);
	return (-1);
}
Array.prototype.findIndex = findIndex;
	
function findObjIndex(param,value){
	for(i=0; i < this.length; i++)
		if( this[i][param] == value )
			return (i);
	return (-1);
}
Array.prototype.findObjIndex = findObjIndex;
	
function getNode(obj){
	while ( obj.nodeType != Node.ELEMENT_NODE ){
		obj = obj.parentNode;
	}			
	return obj;
}
	
function findChildNode(nodeObj,tn,cn) {
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

function findParentElement(node,tagName,className){
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


function findChildElement(node,tagName,className,index){
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
			else if(childNode.nodeType == 1 && childNode.nodeName.toLowerCase() == tagName.toLowerCase())
					return childNode;		
		}
	}
	return null;
}

function getInnerText(nodeObj){
	/* Not needed but it is faster for IE browsers */
	if (xIE4Up) return nodeObj.innerText;	
		
	var str = "";
	for (var i=0; i<nodeObj.childNodes.length; i++){
		switch (nodeObj.childNodes.item(i).nodeType){
			case 1: 
				/* ELEMENT_NODE */
				str += getInnerText(nodeObj.childNodes[i]);
				break;
			case 3:	
				/* TEXT_NODE */
				str += nodeObj.childNodes[i].nodeValue;
				break;
		}		
	}	
	return str;
}

function findPreviousSibling(node,tagName,className){
	try{
		if (node.previousSibling){
			for (node = node.previousSibling; node != null; node = node.previousSibling)
			if (node.nodeType == 1 && node.nodeName.toUpperCase() == tagName.toUpperCase() && (node.className == className || node.id == className))
				return node;
		}
	}
	catch(e) {return null;}
	return null;
}

function findNextSibling(node,tagName,className){
	try{
		if (node.nextSibling){
			for (node = node.nextSibling ;node != null; node = node.nextSibling) 
				if (node.nodeType == 1 && node.nodeName.toUpperCase() == tagName.toUpperCase() && (node.className == className || node.id == className))
					return node;
		}	
	}
	catch(e) {return null;}
	return null;
}

var _windowLoaded = false;
function window_OnLoad() {
	/*! Quit if this function has already been called */
    if (arguments.callee.done) return;
    
    /*! Writedebug message when dbg >0*/
    try{writedebug(Date()+":::Start Window_onload");}catch(e){}

    /*! Flag this function so we don't do the same thing twice */
    arguments.callee.done = true;
    
	/*! Hook that any page can take advantage of */
	if (typeof(windowOnLoad) != "undefined" && typeof(windowOnLoad) == "function") {
	    window.setTimeout("windowOnLoad()",1);
    }
    	
	/*! Connects the pages List event for ListPageControl.. */
	if (typeof(pageListLoad) != "undefined" && typeof(pageListLoad) == "function") {
	    window.setTimeout("pageListLoad()",1);
    }
    /*! Fires Wizard Control is function is there*/
    if (typeof(addNewsPageWizard)  != "undefined" && typeof(addNewsPageWizard) == "function") {
        window.setTimeout("addNewsPageWizard()",1);
    }
    
    if (typeof(tempWidgetIds) != "undefined" && typeof(tempWidgetIds) == "object") {
        window.setTimeout("getWidgetData()",1);
    }
    
    /*! Hook that is formally defined in clippings.js */
	if (typeof(clippingsOnLoad) != "undefined" && typeof(clippingsOnLoad) == "function") {
	    window.setTimeout("clippingsOnLoad()",1);
	}
	_windowLoaded = true;
    
}


// memoize: a general-purpose function to enable a function to use memoization
//   func: the function to be memoized
//   context: the context for the memoized function to execute within
//   Note: the function must use explicit, string-serializable parameters
function memoize (func, context) {
    function memoizeArg (argPos) {
        var cache = {};
        return function () {
            if (argPos == 0) {
                if (!(arguments[argPos] in cache)) {
                    cache[arguments[argPos]] = func.apply(context, arguments);
                }
                return cache[arguments[argPos]];
            }
            else {
                if (!(arguments[argPos] in cache)) {
                    cache[arguments[argPos]] = memoizeArg(argPos - 1);
                }
                return cache[arguments[argPos]].apply(this, arguments);
            }
        }
    }
    // JScript doesn't grok the arity property, but uses length instead
    var arity = func.arity || func.length;
    return memoizeArg(arity - 1);
}

