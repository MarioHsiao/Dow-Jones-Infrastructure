The Example below is based on templating engines provided by `DoT.js`. 

(A (web) templating engine is a software that is designed to process `web templates` and content information to produce output `web documents`.)
We can use any style templating engine like mustache, jquery-tmpl, handlebars, closure templates to generate the template function.
(We will see shortly what is the template function)

**Steps to create the HTML template for the component**

**1. Create the template that describes the overall structure of the component to be rendered.**

In this example, We will use the Portal Headline List Component to demonstrate the example. 

	<script type="text/x-dot-template" id="successTemplate">  
	<ul class="realtime-news">
		<%  var x, title, headlines = self.headlines, options = self.options;
			for (var i = 0, len = Math.min(headlines.length, options.maxNumHeadlinesToShow); i < len; i++) { 
				x = headlines[i]; 
				title = (options.showTruncatedTitle && $.trim(x.truncatedTitle)) || x.title;
		%>
		<li class="dj_entry">
			<h4 class="headline">
				<a class="article-view-trigger ellipsis" href="#">
					<% if (x.modificationTimeDescriptor) { %>
						<span class="time-stamp"><%= x.modificationTimeDescriptor %></span>
					<% } %>
					<span class="headline-text"><%= title %></span>
				 </a>
			</h4>
		</li>
		<% } %> 
	</ul>
	</script>
The example creates the templates inside the closing and opening `<script>` tag. The script tag is given the template id, 'successTemplate'.
The Template functions can interpolate variables, using delimiters <%= … %>, as well as execute arbitrary JavaScript code, with delimiters <% … %>. 
If you wish to interpolate a value, and have it be HTML-escaped, use <%= … %> or any demiliter.

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

**3. Override the existing template by passing this newly created template function into the data object.**
	
	<script type="text/javascript">
		DJ.add("PortalHeadlineList", {
			container: "portalHeadlinesContainer",
			options: {
				maxNumHeadlinesToShow: 6,
				showAuthor: true,
				authorClickable: true,
				displaySnippets: 1, 
				layout: 3
			},
			templates: {
				  success: doTCompiledTemplateFunction 
			} 
		});
	</script>
