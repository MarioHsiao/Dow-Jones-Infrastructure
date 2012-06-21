using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsXmlDocParser.Tests
{
	[TestClass]
	public class TextReaderExtensionsTest
	{
		[TestMethod]
		public void ReturnsSingleFunctionBlock()
		{
			const string block = @"function jQuery(selector, context) {
											///	<summary>
											///     1: $(expression, context) - This function accepts a string containing a CSS selector which is then used to match a set of elements.
											///     2: $(html) - Create DOM elements on-the-fly from the provided String of raw HTML.
											///     3: $(elements) - Wrap jQuery functionality around a single or multiple DOM Element(s).
											///     4: $(callback) - A shorthand for $(document).ready().
											///     5: $() - As of jQuery 1.4, if you pass no arguments in to the jQuery() method, an empty jQuery set will be returned.
											///	</summary>
											///	<param name=""selector"" type=""String"">
											///     1: expression - An expression to search with.
											///     2: html - A string of HTML to create on the fly.
											///     3: elements - DOM element(s) to be encapsulated by a jQuery object.
											///     4: callback - The function to execute when the DOM is ready.
											///	</param>
											///	<param name=""context"" type=""jQuery"">
											///     1: context - A DOM Element, Document or jQuery to use as context.
											///	</param>
											///	<returns type=""jQuery"" />

											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";
			var reader = new StringReader(block);
            var results = JsParser.ReadFunctionBlocks(reader);
			// count all lines except empty lines
			Assert.AreEqual(21, results[0].Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[TestMethod]
		public void ReturnsNestedFunctionBlocks()
		{
			const string block = @"   var jQuery = (function () {

											// Define a local copy of jQuery
											var jQuery = function (selector, context) {
												///	<summary>
												///     1: $(expression, context) - This function accepts a string containing a CSS selector which is then used to match a set of elements.
												///     &#10;2: $(html) - Create DOM elements on-the-fly from the provided String of raw HTML.
												///     &#10;3: $(elements) - Wrap jQuery functionality around a single or multiple DOM Element(s).
												///     &#10;4: $(callback) - A shorthand for $(document).ready().
												///     &#10;5: $() - As of jQuery 1.4, if you pass no arguments in to the jQuery() method, an empty jQuery set will be returned.
												///	</summary>
												///	<param name=""selector"" type=""String"">
												///     1: expression - An expression to search with.
												///     &#10;2: html - A string of HTML to create on the fly.
												///     &#10;3: elements - DOM element(s) to be encapsulated by a jQuery object.
												///     &#10;4: callback - The function to execute when the DOM is ready.
												///	</param>
												///	<param name=""context"" type=""jQuery"">
												///     1: context - A DOM Element, Document or jQuery to use as context.
												///	</param>
												///	<returns type=""jQuery"" />

												// The jQuery object is actually just the init constructor 'enhanced'
												return new jQuery.fn.init(selector, context);
											},

										})();";
			var reader = new StringReader(block);
            var results = JsParser.ReadFunctionBlocks(reader);
			Assert.AreEqual(2, results.Count);
		}

		[TestMethod]
		public void ReturnsMultipleFunctionBlocks()
		{
			#region JS String

			const string block = @"    var jQuery = (function () {

											// Define a local copy of jQuery
											var jQuery = function (selector, context) {
												///	<summary>
												///     1: $(expression, context) - This function accepts a string containing a CSS selector which is then used to match a set of elements.
												///     &#10;2: $(html) - Create DOM elements on-the-fly from the provided String of raw HTML.
												///     &#10;3: $(elements) - Wrap jQuery functionality around a single or multiple DOM Element(s).
												///     &#10;4: $(callback) - A shorthand for $(document).ready().
												///     &#10;5: $() - As of jQuery 1.4, if you pass no arguments in to the jQuery() method, an empty jQuery set will be returned.
												///	</summary>
												///	<param name=""selector"" type=""String"">
												///     1: expression - An expression to search with.
												///     &#10;2: html - A string of HTML to create on the fly.
												///     &#10;3: elements - DOM element(s) to be encapsulated by a jQuery object.
												///     &#10;4: callback - The function to execute when the DOM is ready.
												///	</param>
												///	<param name=""context"" type=""jQuery"">
												///     1: context - A DOM Element, Document or jQuery to use as context.
												///	</param>
												///	<returns type=""jQuery"" />

												// The jQuery object is actually just the init constructor 'enhanced'
												return new jQuery.fn.init(selector, context);
											},

											// Map over jQuery in case of overwrite
										_jQuery = window.jQuery,

											// Map over the $ in case of overwrite
										_$ = window.$,

											// A central reference to the root jQuery(document)
										rootjQuery,

											// A simple way to check for HTML strings or ID strings
											// (both of which we optimize for)
										quickExpr = /^(?:[^<]*(<[\w\W]+>)[^>]*$|#([\w\-]+)$)/,

											// Is it a simple selector
										isSimple = /^.[^:#\[\.,]*$/,

											// Check if a string has a non-whitespace character in it
										rnotwhite = /\S/,
										rwhite = /\s/,

											// Used for trimming whitespace
										trimLeft = /^\s+/,
										trimRight = /\s+$/,

											// Check for non-word characters
										rnonword = /\W/,

											// Check for digits
										rdigit = /\d/,

											// Match a standalone tag
										rsingleTag = /^<(\w+)\s*\/?>(?:<\/\1>)?$/,

											// JSON RegExp
										rvalidchars = /^[\],:{}\s]*$/,
										rvalidescape = /\\(?:[""\\\/bfnrt]|u[0-9a-fA-F]{4})/g,
										rvalidtokens = /""[^""\\\n\r]*""|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,
										rvalidbraces = /(?:^|:|,)(?:\s*\[)+/g,

											// Useragent RegExp
										rwebkit = /(webkit)[ \/]([\w.]+)/,
										ropera = /(opera)(?:.*version)?[ \/]([\w.]+)/,
										rmsie = /(msie) ([\w.]+)/,
										rmozilla = /(mozilla)(?:.*? rv:([\w.]+))?/,

											// Keep a UserAgent string for use with jQuery.browser
										userAgent = navigator.userAgent,

											// For matching the engine and version of the browser
										browserMatch,

											// Has the ready events already been bound?
										readyBound = false,

											// The functions to execute on DOM ready
										readyList = [],

											// The ready event handler
										DOMContentLoaded,

											// Save a reference to some core methods
										toString = Object.prototype.toString,
										hasOwn = Object.prototype.hasOwnProperty,
										push = Array.prototype.push,
										slice = Array.prototype.slice,
										trim = String.prototype.trim,
										indexOf = Array.prototype.indexOf,

											// [[Class]] -> type pairs
										class2type = {};

											jQuery.fn = jQuery.prototype = {
												init: function (selector, context) {
													var match, elem, ret, doc;

													// Handle $(""""), $(null), or $(undefined)
													if (!selector) {
														return this;
													}

													// Handle $(DOMElement)
													if (selector.nodeType) {
														this.context = this[0] = selector;
														this.length = 1;
														return this;
													}

													// The body element only exists once, optimize finding it
													if (selector === ""body"" && !context && document.body) {
														this.context = document;
														this[0] = document.body;
														this.selector = ""body"";
														this.length = 1;
														return this;
													}

													// Handle HTML strings
													if (typeof selector === ""string"") {
														// Are we dealing with HTML string or an ID?
														match = quickExpr.exec(selector);

														// Verify a match, and that no context was specified for #id
														if (match && (match[1] || !context)) {

															// HANDLE: $(html) -> $(array)
															if (match[1]) {
																doc = (context ? context.ownerDocument || context : document);

																// If a single string is passed in and it's a single tag
																// just do a createElement and skip the rest
																ret = rsingleTag.exec(selector);

																if (ret) {
																	if (jQuery.isPlainObject(context)) {
																		selector = [document.createElement(ret[1])];
																		jQuery.fn.attr.call(selector, context, true);

																	} else {
																		selector = [doc.createElement(ret[1])];
																	}

																} else {
																	ret = jQuery.buildFragment([match[1]], [doc]);
																	selector = (ret.cacheable ? ret.fragment.cloneNode(true) : ret.fragment).childNodes;
																}

																return jQuery.merge(this, selector);

																// HANDLE: $(""#id"")
															} else {
																elem = document.getElementById(match[2]);

																// Check parentNode to catch when Blackberry 4.6 returns
																// nodes that are no longer in the document #6963
																if (elem && elem.parentNode) {
																	// Handle the case where IE and Opera return items
																	// by name instead of ID
																	if (elem.id !== match[2]) {
																		return rootjQuery.find(selector);
																	}

																	// Otherwise, we inject the element directly into the jQuery object
																	this.length = 1;
																	this[0] = elem;
																}

																this.context = document;
																this.selector = selector;
																return this;
															}

															// HANDLE: $(""TAG"")
														} else if (!context && !rnonword.test(selector)) {
															this.selector = selector;
															this.context = document;
															selector = document.getElementsByTagName(selector);
															return jQuery.merge(this, selector);

															// HANDLE: $(expr, $(...))
														} else if (!context || context.jquery) {
															return (context || rootjQuery).find(selector);

															// HANDLE: $(expr, context)
															// (which is just equivalent to: $(context).find(expr)
														} else {
															return jQuery(context).find(selector);
														}

														// HANDLE: $(function)
														// Shortcut for document ready
													} else if (jQuery.isFunction(selector)) {
														return rootjQuery.ready(selector);
													}

													if (selector.selector !== undefined) {
														this.selector = selector.selector;
														this.context = selector.context;
													}

													return jQuery.makeArray(selector, this);
												},

												// Start with an empty selector
												selector: """",

												// The current version of jQuery being used
												jquery: ""1.4.4"",

												// The default length of a jQuery object is 0
												length: 0,

												// The number of elements contained in the matched element set
												size: function () {
													///	<summary>
													///     &#10;The number of elements currently matched.
													///     &#10;Part of Core
													///	</summary>
													///	<returns type=""Number"" />

													return this.length;
												},

												toArray: function () {
													///	<summary>
													///     &#10;Retrieve all the DOM elements contained in the jQuery set, as an array.
													///	</summary>
													///	<returns type=""Array"" />
													return slice.call(this, 0);
												},

												// Get the Nth element in the matched element set OR
												// Get the whole matched element set as a clean array
												get: function (num) {
													///	<summary>
													///     &#10;Access a single matched element. num is used to access the
													///     &#10;Nth element matched.
													///     &#10;Part of Core
													///	</summary>
													///	<returns type=""Element"" />
													///	<param name=""num"" type=""Number"">
													///     &#10;Access the element in the Nth position.
													///	</param>

													return num == null ?

													// Return a 'clean' array
												this.toArray() :

													// Return just the object
												(num < 0 ? this.slice(num)[0] : this[num]);
												},

												// Take an array of elements and push it onto the stack
												// (returning the new matched element set)
												pushStack: function (elems, name, selector) {
													///	<summary>
													///     &#10;Set the jQuery object to an array of elements, while maintaining
													///     &#10;the stack.
													///     &#10;Part of Core
													///	</summary>
													///	<returns type=""jQuery"" />
													///	<param name=""elems"" type=""Elements"">
													///     &#10;An array of elements
													///	</param>

													// Build a new jQuery matched element set
													var ret = jQuery();

													if (jQuery.isArray(elems)) {
														push.apply(ret, elems);

													} else {
														jQuery.merge(ret, elems);
													}

													// Add the old object onto the stack (as a reference)
													ret.prevObject = this;

													ret.context = this.context;

													if (name === ""find"") {
														ret.selector = this.selector + (this.selector ? "" "" : """") + selector;
													} else if (name) {
														ret.selector = this.selector + ""."" + name + ""("" + selector + "")"";
													}

													// Return the newly-formed element set
													return ret;
												},

												// Execute a callback for every element in the matched set.
												// (You can seed the arguments with an array of args, but this is
												// only used internally.)
												each: function (callback, args) {
													///	<summary>
													///     &#10;Execute a function within the context of every matched element.
													///     &#10;This means that every time the passed-in function is executed
													///     &#10;(which is once for every element matched) the 'this' keyword
													///     &#10;points to the specific element.
													///     &#10;Additionally, the function, when executed, is passed a single
													///     &#10;argument representing the position of the element in the matched
													///     &#10;set.
													///     &#10;Part of Core
													///	</summary>
													///	<returns type=""jQuery"" />
													///	<param name=""callback"" type=""Function"">
													///     &#10;A function to execute
													///	</param>

													return jQuery.each(this, callback, args);
												},

												ready: function (fn) {
													///	<summary>
													///     &#10;Binds a function to be executed whenever the DOM is ready to be traversed and manipulated.
													///	</summary>
													///	<param name=""fn"" type=""Function"">The function to be executed when the DOM is ready.</param>

													// Attach the listeners
													jQuery.bindReady();

													// If the DOM is already ready
													if (jQuery.isReady) {
														// Execute the function immediately
														fn.call(document, jQuery);

														// Otherwise, remember the function for later
													} else if (readyList) {
															// Add the function to the wait list
														readyList.push(fn);
													}

													return this;
												},

												eq: function (i) {
													///	<summary>
													///     &#10;Reduce the set of matched elements to a single element.
													///     &#10;The position of the element in the set of matched elements
													///     &#10;starts at 0 and goes to length - 1.
													///     &#10;Part of Core
													///	</summary>
													///	<returns type=""jQuery"" />
													///	<param name=""num"" type=""Number"">
													///     &#10;pos The index of the element that you wish to limit to.
													///	</param>

													return i === -1 ?
												this.slice(i) :
												this.slice(i, +i + 1);
												},

												first: function () {
													///	<summary>
													///     &#10;Reduce the set of matched elements to the first in the set.
													///	</summary>
													///	<returns type=""jQuery"" />

													return this.eq(0);
												},

												last: function () {
													///	<summary>
													///     &#10;Reduce the set of matched elements to the final one in the set.
													///	</summary>
													///	<returns type=""jQuery"" />

													return this.eq(-1);
												},

												slice: function () {
													///	<summary>
													///     &#10;Selects a subset of the matched elements.  Behaves exactly like the built-in Array slice method.
													///	</summary>
													///	<param name=""start"" type=""Number"" integer=""true"">Where to start the subset (0-based).</param>
													///	<param name=""end"" optional=""true"" type=""Number"" integer=""true"">Where to end the subset (not including the end element itself).
													///     &#10;If omitted, ends at the end of the selection</param>
													///	<returns type=""jQuery"">The sliced elements</returns>

													return this.pushStack(slice.apply(this, arguments),
												""slice"", slice.call(arguments).join("",""));
												},

												map: function (callback) {
													///	<summary>
													///     &#10;This member is internal.
													///	</summary>
													///	<private />
													///	<returns type=""jQuery"" />

													return this.pushStack(jQuery.map(this, function (elem, i) {
														return callback.call(elem, i, elem);
													}));
												},

												end: function () {
													///	<summary>
													///     &#10;End the most recent 'destructive' operation, reverting the list of matched elements
													///     &#10;back to its previous state. After an end operation, the list of matched elements will
													///     &#10;revert to the last state of matched elements.
													///     &#10;If there was no destructive operation before, an empty set is returned.
													///     &#10;Part of DOM/Traversing
													///	</summary>
													///	<returns type=""jQuery"" />

													return this.prevObject || jQuery(null);
												},

												// For internal use only.
												// Behaves like an Array's method, not like a jQuery method.
												push: push,
												sort: [].sort,
												splice: [].splice
											};


											// The DOM ready check for Internet Explorer
											function doScrollCheck() {
												if (jQuery.isReady) {
													return;
												}

												try {
													// If IE is used, use the trick by Diego Perini
													// http://javascript.nwbox.com/IEContentLoaded/
													document.documentElement.doScroll(""left"");
												} catch (e) {
													setTimeout(doScrollCheck, 1);
													return;
												}

												// and execute any waiting functions
												jQuery.ready();
											}

											// Expose jQuery to the global object
											return (window.jQuery = window.$ = jQuery);

										})();";
			#endregion

			var reader = new StringReader(block);
            var results = JsParser.ReadFunctionBlocks(reader);
			Assert.AreEqual(17, results.Count);
		}

	}
}
