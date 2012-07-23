@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Add a reference to `common.js` in the `<head>` section of you page.

	<script type="text/javascript" src="http://<tbd>/common.js"></script>

Add a container to your page where you would like to display the component.

	<div id="discoveryContainer"></div>

Finally, add the component to the page.
If data is already available, you can pass it in as an argument to `DJ.add` function.

	<script type="text/javascript">
		DJ.add("DiscoveryGraph", {
			container : "discoveryContainer",
			eventHandlers: {},
			data: {...}			
		}); 
	</script>	  

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/DiscoveryGraph/data/js")

`data` can be specified inline as a JSON reprenstation of `DowJones.Web.Mvc.UI.Components.DiscoveryGraph.Entities`, or via a callback that returns a JSON reprenstation of `DowJones.Web.Mvc.UI.Components.DiscoveryGraph.Entities`. 
Click "View Sample Data" to see example of `DowJones.Web.Mvc.UI.Components.DiscoveryGraph.Entities` JSON.