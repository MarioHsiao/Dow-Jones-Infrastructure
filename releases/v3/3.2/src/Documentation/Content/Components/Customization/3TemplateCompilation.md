**2. Generate a template function for the above template using doT.template()**
	
**2a. Provide the template settings.**
	
	doT.templateSettings = { 
						evaluate : /\<\%([\s\S]+?)\%\>/g, 
						interpolate : /\<\%=([\s\S]+?)\%\>/g, 
						varname : 'self', 
						strip: true 
			}; 

You can customize doT by changing compilation settings. Here are the default settings:

	doT.templateSettings = {
	  evaluate:    /\{\{([\s\S]+?)\}\}/g,
	  interpolate: /\{\{=([\s\S]+?)\}\}/g,
	  encode:      /\{\{!([\s\S]+?)\}\}/g,
	  use:         /\{\{#([\s\S]+?)\}\}/g,
	  define:      /\{\{##\s*([\w\.$]+)\s*(\:|=)([\s\S]+?)#\}\}/g,
	  conditional: /\{\{\?(\?)?\s*([\s\S]*?)\s*\}\}/g,
	  iterate:     /\{\{~\s*(?:\}\}|([\s\S]+?)\s*\:\s*([\w$]+)\s*(?:\:\s*([\w$]+))?\s*\}\})/g,
	  varname: 'it',
	  strip: true,
	  append: true,
	  selfcontained: false
	};
If you want to use your own delimiters, you can modify RegEx in doT.templateSettings to your liking.
 
**2b. Compile to template function.**

	var doTCompiledTemplateFunction = doT.template( $('#successTemplate').html() );

DoT compiles Javascript templates into Javascript functions that can be evaluated for rendering. 

