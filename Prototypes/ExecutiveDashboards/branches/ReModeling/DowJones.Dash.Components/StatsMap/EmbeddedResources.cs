using System.Web.UI;
using DowJones.Dash.Components.StatsMap;
using DowJones.Web;

[assembly: WebResource(EmbeddedResources.MarkerRed, KnownMimeTypes.PngImage)]
[assembly: WebResource(EmbeddedResources.MarkerGreen, KnownMimeTypes.PngImage)]
[assembly: WebResource(EmbeddedResources.MarkerYellow, KnownMimeTypes.PngImage)]
namespace DowJones.Dash.Components.StatsMap
{
	public static class EmbeddedResources
	{
		public const string BasePath = "DowJones.Dash.Components.StatsMap.";
		public const string MarkerRed = BasePath + "marker_rounded_red.png";
		public const string MarkerYellow = BasePath + "marker_rounded_yellow_orange.png";
		public const string MarkerGreen = BasePath + "marker_rounded_yellow_green.png";
	}
}
