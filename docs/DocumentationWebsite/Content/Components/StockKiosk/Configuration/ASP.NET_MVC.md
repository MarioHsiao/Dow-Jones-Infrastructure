#### Rendering the component from a Razor View

Populate the StockKiosk model (either in your controller or model):

	public ActionResult Index([ModelBinder(typeof(StringSplitModelBinder))]string[] syms, SymbolType symbolType = SymbolType.FCode, Frequency frequency = Frequency.FifteenMinutes, int pageSize = 10)
    {
         var symsList = new List<string>(new[] { "ibm", "msft", "goog", "aapl", "znga", "fb", "amzn", "ma" });
         var model = GetStockKioskModel(symsList, symbolType, frequency, pageSize);
         return View("Index", model);
    }

	private StockKioskModel GetStockKioskModel(IEnumerable<string> syms, SymbolType symbolType = SymbolType.Ticker, Frequency frequency = Frequency.FifteenMinutes, int pageSize = 10)
    {
        var kioskModel = new StockKioskModel();
        var response = MarketDataChartingManager.GetMarketChartData(syms.ToArray(), symbolType, TimePeriod.OneDay, frequency);
        if (response.PartResults == null || response.PartResults.Count() <= 0)
        {
            return null;
        }
        var data = _assembler.Convert(response.PartResults);
        kioskModel.Data = data;
        kioskModel.PageSize = pageSize;
        if (kioskModel.PageSize > 8) kioskModel.PageSize = 8; //min as per the design to fit in
        return kioskModel;
    }

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@Html.DJ().Render( Model, new { id = "djStockKiosk"} )