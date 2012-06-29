@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Add a reference to `common.js` with a valid `sessionId` anywhere in the `<head>` section of you page.

	<script type="text/javascript" 
	        src="http://<tbd>/common.js?sessionId=27137ZzZKJAUQT2CAAAGUAIAAAAANFOUAAAAAABSGAYTEMBWGI2TCNBQGYZTKNZS"></script>

Add a container `<div>` to your page where you would like to display the component.

	<div id="portalHeadlinesContainer"></div>

Finally, add the component to the page:

	<script type="text/javascript">
		DJ.add(DJ.UI.PortalHeadlineList, {
			container : "portalHeadlinesContainer",
			maxNumHeadlinesToShow : 6,
			showAuthor: true,
			authorClickable: true,
			displaySnippets: 1,
			onLoad: init			/* function to wire up data on load */
		}); 
	</script>	  

`init` can be any function that calls a service to get data and bind it to the component. Here is a sample implementation:

	<script type="text/javascript">
		// bootstrapper function to bind data to the component
		function init() {
			var $container = $('#portalHeadlinesContainer'),
			component = $container.findComponent(DJ.UI.PortalHeadlineList);

			if(component) {
				// get data via a service call (stub service shown here for reference purposes only)
				$.ajax({
					url: 'http://someService/PortalHeadlineList/GetJsonData',
					success: function(data) {
								// on receiving data, call the bind success method of the component
								component.bindOnSuccess(data);
							},
					error:  function(jqXHR, textStatus, errorThrown) {
								// on error, call the bind error method of the component
								component.bindOnError(textStatus);
							}
				});
			}
		}
	</script>
	
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/PortalHeadlineList/data/js")