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
					url: 'http://someService/Search/GetJsonData',
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

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/PortalHeadlineList/data/js")
		  