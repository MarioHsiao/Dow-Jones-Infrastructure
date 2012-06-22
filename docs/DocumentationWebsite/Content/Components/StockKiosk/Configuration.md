#### Rendering the component from a Razor View

Add the following lines to render the StockKiosk component from a Razor View:
		
	<!-- Use the default stylesheet or supply your own -->
	@@{
		Html.DJ().StylesheetRegistry()
			.Include("~/Content/css/Components/StockKiosk/stockKiosk.css");
	}
	
	<!-- Render framework core files -->
	@@Html.DJ().ScriptRegistry().Render()
	@@Html.DJ().StylesheetRegistry().Render()

	<!-- Render the component -->
	@@Html.DJ().RenderComponent("StockKiosk", new StockKioskhModel())