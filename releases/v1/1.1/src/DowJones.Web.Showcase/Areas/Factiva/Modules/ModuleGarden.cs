using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas;

using DowJones.Web.Showcase.Areas.Factiva.Models;
using DowJones.Web.Showcase.Areas.Factiva.Modules;

[assembly: WebResource(ModuleGardenCanvas.ScriptFile, KnownMimeTypes.JavaScript)]
namespace DowJones.Web.Showcase.Areas.Factiva.Modules

{
    [ScriptResource("ModuleGardenCanvas", ResourceName = ScriptFile)]
    public class ModuleGardenCanvas : AbstractCanvas<ModuleGardenModel>
    {
        internal const string BaseDirectory = "DowJones.Web.Showcase.Areas.Factiva.Modules";
        internal const string ScriptFile = BaseDirectory + ".ModuleGardenCanvas.js";

        public override string ClientPluginName
        {
            get { return "dj_moduleGardenCanvas"; }
        }
    }
}