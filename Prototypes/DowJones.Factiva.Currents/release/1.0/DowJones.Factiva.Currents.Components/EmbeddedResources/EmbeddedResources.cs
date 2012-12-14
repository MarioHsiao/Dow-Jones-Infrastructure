using System.Web.UI;
using DowJones.Factiva.Currents.Components.EmbeddedResources;
using DowJones.Web;

#region Scripts

[assembly: WebResource(EmbeddedResources.Js.HighChartsMap, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighChartsWorldMapShapes, KnownMimeTypes.JavaScript)]

#endregion

namespace DowJones.Factiva.Currents.Components.EmbeddedResources
{
	public static class EmbeddedResources
	{
		[ScriptResource(ResourceName = HighChartsMap, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "highcharts-map", DependsOn = new[] { "highcharts" })]
		[ScriptResource(ResourceName = HighChartsWorldMapShapes, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "world-map-shapes", DependsOn = new[] { "highcharts-map" })]

		public static class Js
		{
			public const string BasePath = "DowJones.Factiva.Currents.Components.EmbeddedResources.js.";

			public const string HighchartsMapBasePath = "DowJones.Factiva.Currents.Components.EmbeddedResources.js.highchartsMap.";
			public const string HighChartsMap = HighchartsMapBasePath + "highchartsMap.js";
			public const string HighChartsWorldMapShapes = HighchartsMapBasePath + "world.map.shapes.js";
		}
	}
}
