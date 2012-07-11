@using DowJones.Documentation.Website.Extensions;
@using System.Configuration;

Populate the `DiscoveryGraph` model:

	using DowJones.Web.Mvc.UI.Components.DiscoveryGraph;

	var model = new DiscoveryGraphModel();
	
@Html.DataViewer(ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/DiscoveryGraph/data/cs")

Render the model in your view which will render the component in the browser:

	<!-- Use the default stylesheet or supply your own -->
	<link href="Content/css/discoveryGraph.css" rel="stylesheet" />
	
	<!-- Render the component -->
	@@model DowJones.Web.Mvc.UI.Components.DiscoveryGraph.DiscoveryGraphModel

	@@Html.DJ().Render(Model)	