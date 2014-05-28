using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas.Components.Layout.Grid;

[assembly: WebResource(GridLayoutComponent.ScriptFile, KnownMimeTypes.JavaScript)]
[assembly: WebResource(GridLayoutComponent.Gridster, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Canvas.Components.Layout.Grid
{
    [ScriptResource("grid-layout", ResourceName = ScriptFile, DependencyLevel = ClientResourceDependencyLevel.Global, DependsOn = new [] { "gridster" })]
    [ScriptResource("gridster", ResourceName = Gridster, DependencyLevel = ClientResourceDependencyLevel.Global)]
    public static class GridLayoutComponent
    {
        public const string Namespace = "DowJones.Web.Mvc.UI.Canvas.Components.Layout.Grid";
        public const string ScriptFile = Namespace + ".GridLayout.js";
        public const string Gridster = Namespace + ".jquery.gridster.min.js";
    }
}
