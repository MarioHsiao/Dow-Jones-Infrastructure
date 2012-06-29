@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

#### Rendering the component from a Razor View

Populate the Article model (either in your controller or model):

    var articleModel = new ArticleModel
    {
        ArticleDataSet = articleDataSet,
        ShowPostProcessing = true,
        PostProcessingOptions = new[]
    	{
    		PostProcessingOptions.Print,
    		PostProcessingOptions.Save,
    		PostProcessingOptions.PressClips,
    		PostProcessingOptions.Email, 
    		PostProcessingOptions.Listen,
    		PostProcessingOptions.Translate,
    		PostProcessingOptions.Share
    	},
        ShowSocialButtons = true,
        SocialButtons = new SocialButtonsModel
        {
            Url = "some url",
            Description = "",
            Target = "_blank",
            ImageSize = ImageSize.Small,
            Title = ProcessHeadlineRenderItems(articleDataSet.Headline),
            SocialNetworks = new[]
            {
                SocialNetworks.LinkedIn,
                SocialNetworks.Twitter,
                SocialNetworks.Facebook
            },
            Keywords = "",
            ID = "socialButtons",
            ShowCustomTooltip = false,
        },
        ShowSourceLinks = true
    };
	
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/Article/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@Html.DJ().Render(articleModel)