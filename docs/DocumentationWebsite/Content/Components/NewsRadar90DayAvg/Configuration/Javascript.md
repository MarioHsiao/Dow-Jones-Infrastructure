@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

#### Rendering the component via JavaScript

Add a reference to `common.js` with a valid `sessionId` anywhere in the `<head>` section of you page.

	<script type="text/javascript" 
	     src="http://<tbd>/common.js?sessionId=27137ZzZKJAUQT2CAAAGUAIAAAAANFOUAAAAAABSGAYTEMBWGI2TCNBQGYZTKNZS" />

Add a container `<div>` to your page where you would like to display the component.

	<div id="newsRadarContainer"></div>

Finally, add the component to the page.
If data is already available, you can pass it in as an argument to DJ.add function.

	<script type="text/javascript">
		DJ.add(DJ.UI.NewsRadar, {
			// Set DOM element id which will hold the component. Required.
			container : "newsRadarContainer", 
			
			// Set data, if already available at this point. Optional.
            data: {
				// Click on "View Sample Data" to see sample data
			},

			// Set component specific options. Optional.
			options: {
				displayTicker: false,
				hitcolor: "999",
				hitfont: "Verdana",
				hitsize: "8",
				windowSize: 6,
				scrollSize: 5
            },
			
			// Set custom client templates. Optional.
			templates: {
			},

			// Set event handlers ("onLoad", "onDataBind", etc.). Optional
			callBacks: {
			}
		}); 
	</script>
		  
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/NewsRadar90DayAvg/data/js")

Here is an example of how to get and set data after the component is added.

	<script type="text/javascript">
		var component = $('#newsRadarContainer').findComponent(DJ.UI.NewsRadar);

		if(component) {
			// get data via a service call (stub shown here for reference purposes only)
			$.ajax({
				url: 'http://someDomain/GetData/GetJsonData',
				success: function(data) {
					// on receiving data, call the bindOnSuccess method of the component
					component.bindOnSuccess(data);
				},
				error: function(jqXHR, textStatus, errorThrown) {
					// on error, call the renderError method of the component
					component.renderError();
				}
			});
		}
	</script>