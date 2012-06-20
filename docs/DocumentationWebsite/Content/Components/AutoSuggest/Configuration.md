#### Rendering the component from a Razor View

Add the following lines to render the AutoSuggest component from a Razor View:
	
	<!-- Render framework core files -->
	@@Html.DJ().ScriptRegistry().Render()
	@@Html.DJ().StylesheetRegistry().Render()

	<!-- Render the component -->
	@@Html.DJ().Render(Model, new { id = Model.ID })