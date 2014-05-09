/* ***** BEGIN LICENSE BLOCK *****
 * Distributed under the BSD license:
 *
 * Copyright (c) 2010, Ajax.org B.V.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of Ajax.org B.V. nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL AJAX.ORG B.V. BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * ***** END LICENSE BLOCK ***** */

ace.define('ace/theme/factiva_light', ['require', 'exports', 'module' , 'ace/lib/dom'], function(require, exports, module) {

exports.isDark = false;
exports.cssClass = "ace-factiva-light";
exports.cssText = ".ace-factiva-light .ace_gutter {\
background: #fbf1d3;\
color: #333\
}\
.ace-factiva-light .ace_print-margin {\
width: 1px;\
background: #e8e8e8\
}\
.ace-factiva-light {\
background-color: #fff;\
color: #586E75\
}\
.ace-factiva-light .ace_cursor {\
color: #000000\
}\
.ace-factiva-light .ace_marker-layer .ace_selection {\
background: rgba(7, 54, 67, 0.09)\
}\
.ace-factiva-light.ace_multiselect .ace_selection.ace_start {\
box-shadow: 0 0 3px 0px #FDF6E3;\
border-radius: 2px\
}\
.ace-factiva-light .ace_marker-layer .ace_step {\
background: rgb(255, 255, 0)\
}\
.ace-factiva-light .ace_marker-layer .ace_bracket {\
margin: -1px 0 0 -1px;\
border: 1px solid rgba(147, 161, 161, 0.50)\
}\
.ace-factiva-light .ace_marker-layer .ace_active-line {\
background: #fff5e3\
}\
.ace-factiva-light .ace_gutter-active-line {\
background-color : #333\
}\
.ace-factiva-light .ace_marker-layer .ace_selected-word {\
border: 1px solid #ccc\
}\
.ace-factiva-light .ace_entity.ace_name.ace_tag,\
.ace-factiva-light .ace_keyword,\
.ace-factiva-light .ace_meta.ace_tag,\
.ace-factiva-light .ace_storage {\
color: #7766e7\
}\
.ace-factiva-light .ace_punctuation,\
.ace-factiva-light .ace_punctuation.ace_tag {\
color: #fff\
}\
.ace-factiva-light .ace_constant.ace_character,\
.ace-factiva-light .ace_constant.ace_language,\
.ace-factiva-light .ace_constant.ace_numeric,\
.ace-factiva-light .ace_constant.ace_other {\
color: #88CCEE\
}\
.ace-factiva-light .ace_invalid {\
color: #F8F8F0;\
background-color: #F92672\
}\
.ace-factiva-light .ace_invalid.ace_deprecated {\
color: #F8F8F0;\
background-color: #AE81FF\
}\
.ace-factiva-light .ace_support.ace_constant,\
.ace-factiva-light .ace_support.ace_function {\
color: #88ccee\
}\
.ace-factiva-light .ace_fold {\
background-color: #A6E22E;\
border-color: #F8F8F2\
}\
.ace-factiva-light .ace_storage.ace_type,\
.ace-factiva-light .ace_support.ace_class,\
.ace-factiva-light .ace_support.ace_type {\
color: #66D9EF\
}\
.ace-factiva-light .ace_entity.ace_name.ace_function,\
.ace-factiva-light .ace_entity.ace_other,\
.ace-factiva-light .ace_entity.ace_other.ace_attribute-name,\
.ace-factiva-light .ace_variable {\
color: #A6E22E\
}\
.ace-factiva-light .ace_variable.ace_parameter {\
color: #FD971F\
}\
.ace-factiva-light .ace_paren {\
color: #ddcc77\
}\
.ace-factiva-light .ace_string {\
color: #E6DB74\
}\
.ace-factiva-light .ace_operator {\
color: #aa4499\
}\
.ace-factiva-light .ace_comment {\
color: #75715E\
}\
.ace-factiva-light .ace_constant.ace_character.ace_asterisk{\
color: #117733\
}\.ace-factiva-light .ace_indent-guide {\
background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAACCAYAAACZgbYnAAAAEklEQVQImWNgYGBgYHjy8NJ/AAjgA5fzQUmBAAAAAElFTkSuQmCC) right repeat-y\
}";

var dom = require("../lib/dom");
dom.importCssString(exports.cssText, exports.cssClass);
});
