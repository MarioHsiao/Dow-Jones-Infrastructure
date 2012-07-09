@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;
#### Rendering the component via JavaScript

Add a reference to `common.js` with a valid `sessionId` anywhere in the `<head>` section of you page.

	<script type="text/javascript" 
	    src="http://<tbd>/common.js?sessionId=27137ZzZKJAUQT2CAAAGUAIAAAAANFOUAAAAAABSGAYTEMBWGI2TCNBQGYZTKNZS"></script>

Add a container `<div>` to your page where you would like to display the component.

	<div id="stockKioskContainer"></div>

Finally, add the component to the page:

	<script type="text/javascript">
		DJ.add(DJ.UI.StockKiosk, {
			container	: "stockKioskContainer",
	        options		: {
							pagesize	: 8,
							timePeriod	: 24,
							frequency   : 15 
						  },
			//Set data 
			data		:data
		}); 
	</script>	  

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/StockKiosk/data/js")