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
			},

			onLoad: init			/* function to wire up data on load */
		}); 
	</script>
		  
`init` can be any function that calls a service to get data and bind it to the component. Here is a sample implementation:

	<script type="text/javascript">
		// bootstrapper function to bind data to the component
		function init() {
			var $container = $('#newsRadarContainer'),
			component = $container.findComponent(DJ.UI.NewsRadar);

			if(component) {
				// get data via a service call (stub service shown here for reference purposes only)
				$.ajax({
					url: 'http://someService/NewsRadar/GetJsonData',
					success: function(data) {
								// on receiving data, call the bind success method of the component
								component.setData(data);
							},
					error:  function(jqXHR, textStatus, errorThrown) {
								// on error, call the bind error method of the component
								component.setData();
							}
				});
			}
		}
	</script>

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/NewsRadar90DayAvg/data/js")