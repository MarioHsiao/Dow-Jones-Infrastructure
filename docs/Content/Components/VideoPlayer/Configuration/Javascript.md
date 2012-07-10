@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;
#### Rendering the component via JavaScript

Add a reference to `common.js` with a valid `sessionId` anywhere in the `<head>` section of you page.

	<script type="text/javascript" 
	     src="http://<tbd>/common.js?sessionId=27137ZzZKJAUQT2CAAAGUAIAAAAANFOUAAAAAABSGAYTEMBWGI2TCNBQGYZTKNZS"></script>

Add a container `<div>` to your page where you would like to display the component.

	<div id="videoPlayerContainer"></div>

Finally, add the component to the page:

	<script type="text/javascript">
		DJ.add(DJ.UI.VideoPlayerControl, {
			container : "videoPlayerContainer",
			options: {
                width: 620,
                height: 348,
                autoPlay: false,
                controlBarPath: "[Your Asserts Folder Location]/flowplayer.controls-3.2.5.swf",
                playerPath: "[Your Asserts Folder Location]/flowplayer.unlimited-3.2.7.swf",
                rtmpPluginPath: "[Your Asserts Folder Location]/flowplayer.rtmp-3.2.3.swf",
                splashImagePath: "[Your Asserts Folder Location]/play_text_large.png",
                playerKey: "75a6c4404d9ffa80a63"
            },			
            onLoad: init			/* function to wire up data on load */
		}); 
	</script>	  

	
`init` can be any function that calls a service to get data and bind it to the component. Here is a sample implementation:

	<script type="text/javascript">
		// bootstrapper function to bind data to the component
		function init() {
			var $container = $('#videoPlayerContainer'),
			component = $container.findComponent(DJ.UI.VideoPlayerControl);

			if(component) {
				// get data via a service call (stub service shown here for reference purposes only)
				$.ajax({
					url: 'http://someService/VideoPlayer/GetJsonData',
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

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/VideoPlayer/data/js")