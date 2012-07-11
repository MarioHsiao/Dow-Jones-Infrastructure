@using DowJones.Documentation.Website.Extensions
@using System.Configuration

Populate the `StockKiosk` model:
	
	var stockKioskModel = new StockKioskModel
		{
			PageSize = 8,		//min as per the design to fit in
			Data = GetData()	// Something that returns an instance of MarketDataInstrumentIntradayResultSet
		};

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/StockKiosk/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@model stockKioskModel

	@@Html.DJ().Render(stockKioskModel) 