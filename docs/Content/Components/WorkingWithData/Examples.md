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

**Setting data using `DJ.Add()`**

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

**Binding data to single component** 

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

**Binding data to multiple components**

Once the components were succesfully added on the page you can find the N components and bind the results as shown below:

	<script type="text/javascript">
		var $ = DJ.jQuery;	
		//Find the components;
		var component1Instance = $('#component1Container').findComponent(DJ.UI.Component1Name);
		var component2Instance = $('#component2Container').findComponent(DJ.UI.Component2Name);
		var component3Instance = $('#component3Container').findComponent(DJ.UI.Component3Name);
		:
		:
		var componentNInstance = $('#componentNContainer').findComponent(DJ.UI.ComponentNName);

		//Bind the data
		$(component1Instance).data(dataObj1);
		$(component2Instance).data(dataObj2);
		$(component3Instance).data(dataObj3);
		:
		:
		$(componentNInstance).data(dataObjN);
	</script>



