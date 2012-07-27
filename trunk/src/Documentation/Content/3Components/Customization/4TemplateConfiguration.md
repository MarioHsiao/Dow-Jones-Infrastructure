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
