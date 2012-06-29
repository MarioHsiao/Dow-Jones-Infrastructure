#### Rendering the component from a Razor View

Populate the StockKiosk model (either in your controller or model):
	
	var symsList = new List<string>(new[] { "ibm", "msft", "goog", "aapl", "znga", "fb", "amzn", "ma" });
	var symbolType = SymbolType.FCode;
	var frequency = Frequency.FifteenMinutes;
	var pageSize = 10;

	var stockKioskModel = new StockKioskModel();
    var response = MarketDataChartingManager.GetMarketChartData(syms.ToArray(), symbolType, TimePeriod.OneDay, frequency);
    if (response.PartResults == null || response.PartResults.Count() <= 0)
    {
        return null;
    }
    var data = _assembler.Convert(response.PartResults);
    stockKioskModel.Data = data;
    stockKioskModel.PageSize = pageSize;
    if (stockKioskModel.PageSize > 8) stockKioskModel.PageSize = 8; //min as per the design to fit in
    

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@Html.DJ().Render( stockKioskModel, new { id = "djStockKiosk"} )