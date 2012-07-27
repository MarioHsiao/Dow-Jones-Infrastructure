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

