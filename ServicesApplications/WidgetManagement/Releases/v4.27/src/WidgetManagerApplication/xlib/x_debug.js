/* Compiled from X 4.06 with XC 1.0 on 27Nov06 */
function xEvalTextarea(){var f=document.createElement('FORM');f.onsubmit='return false';var t=document.createElement('TEXTAREA');t.id='xDebugTA';t.name='xDebugTA';t.rows='20';t.cols='60';var b=document.createElement('INPUT');b.type='button';b.value='Evaluate';b.onclick=function(){eval(this.form.xDebugTA.value);};f.appendChild(t);f.appendChild(b);document.body.appendChild(f);}function xGetElePropsArray(ele,eleName){var u='undefined';var i=0,a=new Array();nv('Element',eleName);nv('id',(xDef(ele.id)?ele.id:u));nv('tagName',(xDef(ele.tagName)?ele.tagName:u));nv('xWidth()',xWidth(ele));nv('style.width',(xDef(ele.style)&&xDef(ele.style.width)?ele.style.width:u));nv('offsetWidth',(xDef(ele.offsetWidth)?ele.offsetWidth:u));nv('scrollWidth',(xDef(ele.offsetWidth)?ele.offsetWidth:u));nv('clientWidth',(xDef(ele.clientWidth)?ele.clientWidth:u));nv('xHeight()',xHeight(ele));nv('style.height',(xDef(ele.style)&&xDef(ele.style.height)?ele.style.height:u));nv('offsetHeight',(xDef(ele.offsetHeight)?ele.offsetHeight:u));nv('scrollHeight',(xDef(ele.offsetHeight)?ele.offsetHeight:u));nv('clientHeight',(xDef(ele.clientHeight)?ele.clientHeight:u));nv('xLeft()',xLeft(ele));nv('style.left',(xDef(ele.style)&&xDef(ele.style.left)?ele.style.left:u));nv('offsetLeft',(xDef(ele.offsetLeft)?ele.offsetLeft:u));nv('style.pixelLeft',(xDef(ele.style)&&xDef(ele.style.pixelLeft)?ele.style.pixelLeft:u));nv('xTop()',xTop(ele));nv('style.top',(xDef(ele.style)&&xDef(ele.style.top)?ele.style.top:u));nv('offsetTop',(xDef(ele.offsetTop)?ele.offsetTop:u));nv('style.pixelTop',(xDef(ele.style)&&xDef(ele.style.pixelTop)?ele.style.pixelTop:u));nv('','');nv('xGetComputedStyle()','');nv('top');nv('right');nv('bottom');nv('left');nv('width');nv('height');nv('color');nv('background-color');nv('font-family');nv('font-size');nv('text-align');nv('line-height');nv('content');nv('float');nv('clear');nv('margin');nv('padding');nv('padding-top');nv('padding-right');nv('padding-bottom');nv('padding-left');nv('border-top-width');nv('border-right-width');nv('border-bottom-width');nv('border-left-width');nv('position');nv('overflow');nv('visibility');nv('display');nv('z-index');nv('clip');nv('cursor');return a;function nv(name,value){a[i]=new Object();a[i].name=name;a[i].value=typeof(value)=='undefined'?xGetComputedStyle(ele,name):value;++i;}}function xGetElePropsString(ele,eleName,newLine){var s='',a=xGetElePropsArray(ele,eleName);for(var i=0;i<a.length;++i){s+=a[i].name+' = '+a[i].value+(newLine||'\n');}return s;}xLibrary={version:'4.06',license:'GNU LGPL',url:'http://cross-browser.com/'};function xLoadScript(url){if(document.createElement&&document.getElementsByTagName){var s=document.createElement('script');var h=document.getElementsByTagName('head');if(s&&h.length){s.src=url;h[0].appendChild(s);}}}function xName(e){if(!e)return e;else if(e.id&&e.id!="")return e.id;else if(e.name&&e.name!="")return e.name;else if(e.nodeName&&e.nodeName!="")return e.nodeName;else if(e.tagName&&e.tagName!="")return e.tagName;else return e;}function xParentChain(e,delim,bNode){if(!(e=xGetElementById(e)))return;var lim=100,s="",d=delim||"\n";while(e){s+=xName(e)+', ofsL:'+e.offsetLeft+', ofsT:'+e.offsetTop+d;e=xParent(e,bNode);if(!lim--)break;}return s;}function xSetIETitle(){var ua=navigator.userAgent.toLowerCase();if(!window.opera&&navigator.vendor!='KDE'&&document.all&&ua.indexOf('msie')!=-1&&!document.layers){var i=ua.indexOf('msie')+1;var v=ua.substr(i+4,3);document.title='IE '+v+' - '+document.title;}}function xSmartLoadScript(url){var loadedBefore=false;if(typeof(xLoadedList)!="undefined"){for(i=0;i<xLoadedList.length;i++){if(xLoadedList[i]==url){loadedBefore=true;break;}}}if(document.createElement&&document.getElementsByTagName&&!loadedBefore){var s=document.createElement('script');var h=document.getElementsByTagName('head');if(s&&h.length){s.src=url;h[0].appendChild(s);if(typeof(xLoadedList)=="undefined")xLoadedList=new Array();xLoadedList.push(url);}}}