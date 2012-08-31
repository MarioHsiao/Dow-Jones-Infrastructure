using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas.Components.Layout;

[assembly: WebResource(ZoneLayout.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Canvas.Components.Layout
{
    [ScriptResource(ResourceName = ScriptFile, DependencyLevel = ClientResourceDependencyLevel.MidLevel, Name = "zone-canvas-layout")]
    public class ZoneLayout
    {
        public const string ScriptFile = "DowJones.Web.Mvc.UI.Canvas.Components.Layout.ZoneLayout.js";
    }
}
