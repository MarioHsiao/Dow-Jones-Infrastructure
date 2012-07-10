@using DowJones.Documentation.Website.Extensions
@using System.Configuration

Populate the `StockKiosk` model:
	
	using DowJones.Web.Mvc.UI.Components.StockKiosk;

	var stockKioskModel = new StockKioskModel
		{
			PageSize = 8,		//min as per the design to fit in
			Data = GetData()	// Something that returns an instance of MarketDataInstrumentIntradayResultSet
		};

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/StockKiosk/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- include default styles or supply your own  -->
	<link href="Content/css/StockKiosk.css" rel="stylesheet" />

	<!-- Render the component -->
	@@Html.DJ().Render(Model) 