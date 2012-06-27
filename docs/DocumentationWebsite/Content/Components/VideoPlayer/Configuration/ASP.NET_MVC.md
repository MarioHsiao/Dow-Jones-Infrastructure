#### Rendering the component from a Razor View

Populate the VideoPlayer model (either in your controller or model):

	var mediaContent = new Clip
            {
                Url = "[Your Media File Location]",
                Medium = Medium.Video,
                Duration = "188",
                Type =  "video/mp4",
                BitRate = "1500",
                FrameRate = "29.97",
                Width = "450",
                Height = "170"
            };
	var videoPlayerModel = new VideoPlayerModel
            {
                AutoPlay = false,
                Width = 430,
                Height = 158,
                PlayList = new ClipCollection(new[] { mediaContent }),
                ControlBarPath = "[Your swf Files Folder]/flowplayer.controls-3.2.5.swf",
                PlayerPath = "[Your swf Files Folder]/flowplayer.unlimited-3.2.7.swf",
                RTMPPluginPath = "[Your swf Files Folder]/flowplayer.rtmp-3.2.3.swf",
                SplashImagePath = "[Your FlowPlayer Images Folder]/play_text_large.png",
                PlayerKey = "75a6c4404d9ffa80a63",
            };

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@Html.DJ().Render(videoPlayerModel)