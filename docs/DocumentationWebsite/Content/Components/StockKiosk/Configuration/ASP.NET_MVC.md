@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;
#### Rendering the component from a Razor View

Populate the StockKiosk model (either in your controller or model):
	
	   var stockKioskModel = new StockKioskModel
                                 {
                                     PageSize = 8,
									 
									 //Set Data
									 Data = data 
                                 };    

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@Html.DJ().Render( stockKioskModel, new { id = "djStockKiosk"} )

@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/StockKiosk/data/cs")