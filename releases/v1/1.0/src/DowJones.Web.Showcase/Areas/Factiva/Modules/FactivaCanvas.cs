using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Showcase.Areas.Factiva.Modules;

[assembly: WebResource(FactivaCanvas.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Showcase.Areas.Factiva.Modules
{
    [ScriptResource("FactivaCanvas", ResourceName = ScriptFile)]
    public class FactivaCanvas : AbstractCanvas<FactivaCanvasModel>
    {
        internal const string BaseDirectory = "DowJones.Web.Showcase.Areas.Factiva.Modules";
        internal const string ScriptFile = BaseDirectory + ".FactivaCanvas.js";

        public override string ClientPluginName
        {
            get { return "dj_factivaCanvas"; }
        }
    }
}