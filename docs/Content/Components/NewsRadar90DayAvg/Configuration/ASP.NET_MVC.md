@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the `NewsRadar` model:

	// Instantiate and fill data
	var data = new Collection<EntityModel>
	{
		// Click on "View Sample Data" to see sample data
	}

    var newsRadarModel = new NewsRadarModel
    {
		// Set data
		Data = data
    };
	
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/NewsRadar90DayAvg/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Render the component using the model -->
	@@model newsRadarModel

	@@Html.DJ().Render(newsRadarModel)