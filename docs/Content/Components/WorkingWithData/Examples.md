**ASP.NET MVC**

**Passing data to model which require Assemblers**

In this example, StockKiosk Component uses GetData() method to pass the data to the StockKioskModel.

<pre>
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
</pre>

**Passing data to model which does not require Assemblers**

In this example, VideoPlayer Component gets data as shown below and uses it in the VideoPlayerModel.

<pre>
var data = new ClipCollection(new[]
	{
		new Clip
			{
				Url = appPath + "/styles/views/videoplayer/media/demo.mp4",
				Medium = Medium.Video,
				Duration = "188",
				Type = "video/mp4",
				BitRate = "1500",
				FrameRate = "29.97",
				Width = "670",
				Height = "300"
			}
	});
</pre>

**Javascript**


