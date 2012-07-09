@using DowJones.Documentation.Website.Extensions
@using System.Configuration

Populate the `StockKiosk` model:
	
	using DowJones.Web.Mvc.UI.Components.StockKiosk;

	var stockKioskModel = new StockKioskModel
		{
			PageSize = 8,		//min as per the design to fit in
			Data = GetData()	// Something that returns an instance of MarketDataInstrumentIntradayResultSet
		};

Render the model in your view which will render the component in the browser:

	<!-- include default styles or supply your own  -->
	<link href="Content/css/StockKiosk.css" rel="stylesheet" />

	<!-- Render the component -->
	@@Html.DJ().Render(Model) 

	
`GetData()` can be any method or service call that returns a valid `MarketDataInstrumentIntradayResultSet`.
A sample implementation of `GetData()` using Factiva Gateway is shown below:
	
	using System.Linq;
	using DowJones.Assemblers.Charting.MarketData;
	using DowJones.Managers.Charting.MarketData;
	using DowJones.Models.Charting.MarketData;
	using DowJones.Web.Mvc.UI.Components.StockKiosk;

	private MarketDataInstrumentIntradayResultSet GetData()
	{
		// arbitrary list of FCodes
		var fCodes = new[] { "ibm", "mcrost", "goog", "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" };
		var response = MarketDataChartingManager.GetMarketChartData(fCodes);

		// if we do not have a reponse, do not process further.
		if (response.PartResults == null || !response.PartResults.Any())
			return null;

		// map response to DTO
		var assembler = new MarketDataInstrumentIntradayResultSetAssembler(new Preferences.Preferences("en"));
		var data = assembler.Convert(response.PartResults);
		return data;
	}

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/StockKiosk/data/cs")