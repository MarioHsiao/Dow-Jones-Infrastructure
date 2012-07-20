@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;
#### Rendering the component via JavaScript

Add a reference to `common.js` with a valid `sessionId` anywhere in the `<head>` section of you page.

	<script type="text/javascript" src="http://<tbd>/common.js"></script>

Add a container to your page where you would like to display the component.

	<div id="autoSuggestContainer"></div>

Finally, add the component to the page:

	<script type="text/javascript">
		DJ.add("AutoSuggest", {
		container: "autoSuggestContainer",
		options: { suggestServiceUrl: "[Your Sugegst Service Url]",
				   autocompletionType: "Keyword",
				   controlId: "djKeywordAutoSuggest",
		           authTypeValue: "[Your Suggest Context]",
		           authType: "SuggestContext",
				   selectFirst: true,
				   fillInputOnKeyUpDown: true,
				   showHelp: false,
				   showViewAll: false }
		}); 
	</script>	  
