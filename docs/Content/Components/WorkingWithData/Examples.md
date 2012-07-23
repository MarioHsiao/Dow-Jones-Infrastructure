**ASP.NET MVC**

**Passing data to model which uses Assemblers**

In this example, `StockKiosk` Component uses GetData() method to pass the data to the `StockKioskModel`.


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

**Passing data to model which does not use Assemblers**

In this example, `VideoPlayer` Component gets data as shown below and uses it in the `VideoPlayerModel`.


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

**Javascript**

**Passing data using DJ.Add()**

In this example, data can be set to the Sample Component using `DJ.Add()` in the following ways:

**1. During Instantiation**

	<script type="text/javascript">
		DJ.add("SampleComponent", {
					container: "sampleComponentContainer",
					options: {
						..,
						..
					},
					data: getData(), //data can be a json data object of the component
					eventHandlers: {}
				});
	</script>


**2. After Instantiation**

As shown in the below example when a component has been successfully created, `done()` eventhandler is executed
and you can bind data inside the handler.     

	<script type="text/javascript">
    DJ.add( "sampleComponent" , [parameters])
      .done(function(instance){   // 'instance' is the newly-created component instance
          //alert('Component loaded: ' + instance.name);
		  instance.bindOnSuccess(data); //data can be a json data object of the component
      })
      .fail(function(err){
          alert('Error occurred while loading component'); // err contains an array of errors
      }); 
	</script>

