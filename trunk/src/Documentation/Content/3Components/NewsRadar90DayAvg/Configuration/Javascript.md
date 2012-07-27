@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

#### Rendering the component via JavaScript

Add a reference to `common.js` in the `<head>` section of you page.

	<script type="text/javascript" src="http://<tbd>/common.js" />

Add a container to your page where you would like to display the component.

	<div id="newsRadarContainer"></div>

Finally, add the component to the page.
If data is already available, you can pass it in as an argument to `DJ.add` function.

	<script type="text/javascript">
		DJ.add("NewsRadar", {
			container : "newsRadarContainer", 
			options: {
				displayTicker: false,
				hitcolor: "999",
				hitfont: "Verdana",
				hitsize: "8",
				windowSize: 6,
				scrollSize: 5
            },
			eventHandlers: {},
            data: {...}
		}); 
	</script>
		  
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/NewsRadar90DayAvg/data/js")

`data` can be specified inline as a JSON reprenstation of `Collection<EntityModel>`, or via a callback that returns a JSON reprenstation of `Collection<EntityModel>`. 
Click "View Sample Data" to see example of `Collection<EntityModel>` JSON.