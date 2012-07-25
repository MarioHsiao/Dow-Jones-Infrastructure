@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;
#### Rendering the component via JavaScript

Add a reference to `common.js` in the `<head>` section of you page.

	<script type="text/javascript" src="http://<tbd>/common.js"></script>

Add a container to your page where you would like to display the component.

	<div id="stockKioskContainer"></div>

Finally, add the component to the page.
If data is already available, you can pass it in as an argument to `DJ.add` function.

	<script type="text/javascript">
		DJ.add("StockKiosk", {
			container	: "stockKioskContainer",
	        options		: {
							pagesize	: 8,
							timePeriod	: 24,
							frequency   : 15 
						  },
			eventHandlers: {},
            data: {...}
		}); 
	</script>	

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/StockKiosk/data/js")

`data` can be specified inline as a JSON reprenstation of `MarketDataInstrumentIntradayResultSet`, or via a callback that returns a JSON reprenstation of `MarketDataInstrumentIntradayResultSet`. 
Click "View Sample Data" to see example of `MarketDataInstrumentIntradayResultSet` JSON.
