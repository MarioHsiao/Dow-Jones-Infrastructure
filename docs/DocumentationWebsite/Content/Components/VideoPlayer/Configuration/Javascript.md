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
            data: [{
                url: "[Your Media File Location]/demo.mp4",
                medium: "video",
                duration: "188",
                type: "video/mp4",
                bitRate: "1500",
                frameRate: "29.97",
                width: "640",
                height: "360"
            }]
		}); 
	</script>	  
