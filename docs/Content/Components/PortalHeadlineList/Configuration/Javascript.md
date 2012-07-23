@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Add a reference to `common.js` in the `<head>` section of you page.

	<script type="text/javascript" src="http://<tbd>/common.js"></script>

Add a container to your page where you would like to display the component.

	<div id="portalHeadlinesContainer"></div>

Finally, add the component to the page.
If data is already available, you can pass it in as an argument to `DJ.add` function.

	<script type="text/javascript">
        DJ.add("PortalHeadlineList", {
            container: "portalHeadlinesContainer",
            options: {
                maxNumHeadlinesToShow: 6,
                showAuthor: true,
                authorClickable: true,
                displaySnippets: 1
            },
            eventHandlers: {},
            data: {...}
        });
	</script>

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/PortalHeadlineList/data/js")

`data` can be specified inline as a JSON reprenstation of `PortalHeadlineListDataResult`, or via a callback that returns a JSON reprenstation of `PortalHeadlineListDataResult`. 
Click "View Sample Data" to see example of `PortalHeadlineListDataResult` JSON.