@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;
#### Rendering the component via JavaScript

Add a reference to `common.js` in the `<head>` section of you page.

	<script type="text/javascript" src="http://<tbd>/common.js"></script>

Add a container to your page where you would like to display the component.

	<div id="videoPlayerContainer"></div>

Finally, add the component to the page.
If data is already available, you can pass it in as an argument to `DJ.add` function.

	<script type="text/javascript">
		DJ.add("VideoPlayer", {
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
            eventHandlers: {},
            data: {...}
		}); 
	</script>	  

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/VideoPlayer/data/js")

`data` can be specified inline as a JSON reprenstation of `ClipCollection`, or via a callback that returns a JSON reprenstation of `ClipCollection`. 
Click "View Sample Data" to see example of `ClipCollection` JSON.