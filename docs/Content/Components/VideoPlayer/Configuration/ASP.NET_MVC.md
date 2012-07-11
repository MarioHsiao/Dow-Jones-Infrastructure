@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the `VideoPlayer` model:

	var videoPlayerModel = new VideoPlayerModel
            {
                AutoPlay = false,
                Width = 430,
                Height = 158,                
                ControlBarPath = "[Your swf Files Folder]/flowplayer.controls-3.2.5.swf",
                PlayerPath = "[Your swf Files Folder]/flowplayer.unlimited-3.2.7.swf",
                RTMPPluginPath = "[Your swf Files Folder]/flowplayer.rtmp-3.2.3.swf",
                SplashImagePath = "[Your FlowPlayer Images Folder]/play_text_large.png",
                PlayerKey = "75a6c4404d9ffa80a63",
				
				//Set data
				PlayList = data
            };

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@model videoPlayerModel

	@@Html.DJ().Render(videoPlayerModel) 

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/VideoPlayer/data/cs")