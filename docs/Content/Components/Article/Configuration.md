@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the `Article` model:

	// Instantiate and fill data
	var data = new ArticleResultSet
	{
		// Click on "View Sample Data" to see sample data
	}

	var model = new ArticleModel
	{
		// Set options
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
		ShowSourceLinks = true,

		// Set data
		ArticleDataSet = GetData() //Something that returns ArticleResultSet
	};
	
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/Article/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Render the component using the model -->
	@@model DowJones.Web.Mvc.UI.Components.Models.Article.ArticleModel

	@@Html.DJ().Render(Model)
	
