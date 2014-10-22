/* Compiled from X 4.06 with XC 1.0 on 27Nov06 */
function xDeleteCookie(name, path){if (xGetCookie(name)) {document.cookie = name + "=" +"; path=" + ((!path) ? "/" : path) +"; expires=" + new Date(0).toGMTString();}}
function xGetCookie(name){var value=null, search=name+"=";if (document.cookie.length > 0) {var offset = document.cookie.indexOf(search);if (offset != -1) {offset += search.length;var end = document.cookie.indexOf(";", offset);if (end == -1) end = document.cookie.length;value = unescape(document.cookie.substring(offset, end));}}return value;}
function xSetCookie(name, value, expires, path, domain, secure) {
    document.cookie= name + "=" + escape(value) +
        ((expires) ? "; expires=" + expires.toGMTString() : "") +
        ((path) ? "; path=" + path : "; path=/") +
        ((domain) ? "; domain=" + domain : "") +
        ((secure) ? "; secure" : "");
}