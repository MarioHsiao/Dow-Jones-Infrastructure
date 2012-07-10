@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

#### Rendering the component from a Razor View

Populate the model with options and data:

	// Instantiate and fill data
	var data = new Collection<EntityModel>
	{
		// Click on "View Sample Data" to see sample data
	}

    var model = new NewsRadarModel
    {
		// Set data
		Data = data
    };
	
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/NewsRadar90DayAvg/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Render the component using the model -->
	@@Html.DJ().Render(model)