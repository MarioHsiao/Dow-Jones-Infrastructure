@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the Discovery Graph model (either in your controller or model):

	var discoveryGraphModel = new DiscoveryGraphModel();
	
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/DiscoveryGraph/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Use the default stylesheet or supply your own -->
	@@{
		Html.DJ().StylesheetRegistry()
			.Include("~/Content/css/Components/DiscoveryGraph/discoveryGraph.css");
	}
	
	<!-- Render framework core files -->
	@@Html.DJ().ScriptRegistry().Render()
	@@Html.DJ().StylesheetRegistry().Render()

	<!-- Render the component -->
	@@Html.DJ().Render(discoveryGraphModel)	