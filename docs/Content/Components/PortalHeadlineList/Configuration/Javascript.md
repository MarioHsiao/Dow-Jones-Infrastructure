@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Add a reference to `core.js` in the `<head>` section of you page.

	<script type="text/javascript" src="http://<tbd>/core.js"></script>

Add a container `<div>` to your page where you would like to display the component.

	<div id="portalHeadlinesContainer"></div>

Finally, add the component to the page:

	<script type="text/javascript">
        DJ.add("PortalHeadlineList", {
            container : "portalHeadlinesContainer",
            options: {
	            maxNumHeadlinesToShow : 6,
	            showAuthor: true,
	            authorClickable: true,
	            displaySnippets: 1,
            },
            data: {...},
            eventHandlers: {...}
        }); 
	</script>

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/PortalHeadlineList/data/js")
		  
`data` can be specified inline as a JSON reprenstation of `PortalHeadlineListDataResult` (the model for the component), or via a callback that returns a JSON reprenstation of `PortalHeadlineListDataResult`. 
Click "View Sample Data" to see example of `PortalHeadlineListDataResult` JSON.