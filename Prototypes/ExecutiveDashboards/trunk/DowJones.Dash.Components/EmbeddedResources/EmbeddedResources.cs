using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using DowJones.Dash.Components.EmbeddedResources;
using DowJones.Web;

#region Scripts

[assembly: WebResource(EmbeddedResources.Js.Counter, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighChartsMap, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighChartsUsMapShapes, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighChartsGermanyMapShapes, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighChartsWorldMapShapes, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighChartsSouthKoreaMapShapes, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.HighChartsIndonesiaMapShapes, KnownMimeTypes.JavaScript)]
[assembly: WebResource(EmbeddedResources.Js.QuickFlip, KnownMimeTypes.JavaScript)]

#endregion

namespace DowJones.Dash.Components.EmbeddedResources
{
	public static class EmbeddedResources
	{
		[ScriptResource(ResourceName = Counter, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "jquery-counter", DependsOn = new[] { "jquery" })]
		[ScriptResource(ResourceName = HighChartsMap, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "highcharts-map", DependsOn = new[] { "highcharts" })]
		[ScriptResource(ResourceName = HighChartsUsMapShapes, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "us-map-shapes", DependsOn = new[] { "highcharts-map" })]
		[ScriptResource(ResourceName = HighChartsGermanyMapShapes, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "germany-map-shapes", DependsOn = new[] { "highcharts-map" })]
		[ScriptResource(ResourceName = HighChartsWorldMapShapes, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "world-map-shapes", DependsOn = new[] { "highcharts-map" })]
		[ScriptResource(ResourceName = HighChartsSouthKoreaMapShapes, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "south-korea-map-shapes", DependsOn = new[] { "highcharts-map" })]
		[ScriptResource(ResourceName = HighChartsIndonesiaMapShapes, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "indonesia-map-shapes", DependsOn = new[] { "highcharts-map" })]
		[ScriptResource(ResourceName = QuickFlip, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "quickflip", DependsOn = new[] { "jquery" })]

		public static class Js
		{
			public const string BasePath = "DowJones.Dash.Components.EmbeddedResources.js.";
			public const string Counter = BasePath + "jquery.counter.js";

			#region Highcharts map
			public const string HighchartsMapBasePath = "DowJones.Dash.Components.EmbeddedResources.js.highchartsMap.";
			public const string HighChartsMap = HighchartsMapBasePath + "highchartsMap.js";
			public const string HighChartsUsMapShapes = HighchartsMapBasePath + "us.map.shapes.js";
			public const string HighChartsGermanyMapShapes = HighchartsMapBasePath + "germany.map.shapes.js";
			public const string HighChartsWorldMapShapes = HighchartsMapBasePath + "world.map.shapes.js";
			public const string HighChartsSouthKoreaMapShapes = HighchartsMapBasePath + "south.korea.map.shapes.js";
			public const string HighChartsIndonesiaMapShapes = HighchartsMapBasePath + "indonesia.map.shapes.js";

			#endregion

			public const string QuickFlip = BasePath + "quickflip.jquery.quickflip.js";
		}
	}
}
