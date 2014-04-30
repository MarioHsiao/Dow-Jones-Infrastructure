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

ace.define('ace/theme/factiva_dark', ['require', 'exports', 'module' , 'ace/lib/dom'], function(require, exports, module) {

exports.isDark = true;
exports.cssClass = "ace-factiva-dark";
exports.cssText = ".ace-factiva-dark .ace_gutter {\
background: #2F3129;\
color: #8F908A\
}\
.ace-factiva-dark .ace_print-margin {\
width: 1px;\
background: #555651\
}\
.ace-factiva-dark {\
background-color: #272822;\
color: #F8F8F2\
}\
.ace-factiva-dark .ace_cursor {\
color: #F8F8F0\
}\
.ace-factiva-dark .ace_marker-layer .ace_selection {\
background: #49483E\
}\
.ace-factiva-dark.ace_multiselect .ace_selection.ace_start {\
box-shadow: 0 0 3px 0px #272822;\
border-radius: 2px\
}\
.ace-factiva-dark .ace_marker-layer .ace_step {\
background: rgb(102, 82, 0)\
}\
.ace-factiva-dark .ace_marker-layer .ace_bracket {\
margin: -1px 0 0 -1px;\
border: 1px solid #49483E\
}\
.ace-factiva-dark .ace_marker-layer .ace_active-line {\
background: #202020\
}\
.ace-factiva-dark .ace_gutter-active-line {\
background-color: #272727\
}\
.ace-factiva-dark .ace_marker-layer .ace_selected-word {\
border: 1px solid #49483E\
}\
.ace-factiva-dark .ace_invisible {\
color: #52524d\
}\
.ace-factiva-dark .ace_entity.ace_name.ace_tag,\
.ace-factiva-dark .ace_keyword,\
.ace-factiva-dark .ace_meta.ace_tag,\
.ace-factiva-dark .ace_storage {\
color: #F92672\
}\
.ace-factiva-dark .ace_punctuation,\
.ace-factiva-dark .ace_punctuation.ace_tag {\
color: #fff\
}\
.ace-factiva-dark .ace_constant.ace_character,\
.ace-factiva-dark .ace_constant.ace_language,\
.ace-factiva-dark .ace_constant.ace_numeric,\
.ace-factiva-dark .ace_constant.ace_other {\
color: #AE81FF\
}\
.ace-factiva-dark .ace_invalid {\
color: #F8F8F0;\
background-color: #F92672\
}\
.ace-factiva-dark .ace_invalid.ace_deprecated {\
color: #F8F8F0;\
background-color: #AE81FF\
}\
.ace-factiva-dark .ace_support.ace_constant,\
.ace-factiva-dark .ace_support.ace_function {\
color: #66D9EF\
}\
.ace-factiva-dark .ace_fold {\
background-color: #A6E22E;\
border-color: #F8F8F2\
}\
.ace-factiva-dark .ace_storage.ace_type,\
.ace-factiva-dark .ace_support.ace_class,\
.ace-factiva-dark .ace_support.ace_type {\
font-style: italic;\
color: #66D9EF\
}\
.ace-factiva-dark .ace_entity.ace_name.ace_function,\
.ace-factiva-dark .ace_entity.ace_other,\
.ace-factiva-dark .ace_entity.ace_other.ace_attribute-name,\
.ace-factiva-dark .ace_variable {\
color: #A6E22E\
}\
.ace-factiva-dark .ace_variable.ace_parameter {\
font-style: italic;\
color: #FD971F\
}\
.ace-factiva-dark .ace_string {\
color: #E6DB74\
}\
.ace-factiva-dark .ace_comment {\
color: #75715E\
}\
.ace-factiva-dark .ace_indent-guide {\
background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAACCAYAAACZgbYnAAAAEklEQVQImWPQ0FD0ZXBzd/wPAAjVAoxeSgNeAAAAAElFTkSuQmCC) right repeat-y\
}";

var dom = require("../lib/dom");
dom.importCssString(exports.cssText, exports.cssClass);
});
