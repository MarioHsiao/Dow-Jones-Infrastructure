using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Showcase.Models;
using DowJones.Web.Showcase.Modules;

[assembly: WebResource(ShowcaseCanvas.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Showcase.Modules
{
    [ScriptResource("ShowcaseCanvas", ResourceName = ScriptFile, DependsOn = new [] { "AbstractCanvas" })]
    public class ShowcaseCanvas : AbstractCanvas<ShowcaseCanvasModel>
    {
        internal const string BaseDirectory = "DowJones.Web.Showcase.Modules";
        internal const string ScriptFile = BaseDirectory + ".ShowcaseCanvas.js";

        public override string ClientPluginName
        {
            get { return "dj_ShowcaseCanvas"; }
        }
    }
}