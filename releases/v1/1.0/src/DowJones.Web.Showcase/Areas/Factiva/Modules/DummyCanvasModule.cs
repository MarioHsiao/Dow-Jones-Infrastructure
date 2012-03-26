using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Canvas;

[assembly: WebResource(DowJones.Web.Showcase.Areas.Factiva.Modules.DummyCanvasModule.ScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Showcase.Areas.Factiva.Modules
{
    [ScriptResource("DummyCanvasModuleScript", ResourceName = ScriptFile, DeclaringType = typeof(DummyCanvasModule))]

    public class DummyCanvasModule : AbstractCanvasModule<Module>
    {
        internal const string BaseDirectory = "DowJones.Web.Showcase.Areas.Factiva.Modules";
        internal const string ScriptFile = BaseDirectory + ".DummyCanvasModule.js";


        public override string ClientPluginName
        {
            get { return "dj_DummyCanvasModule"; }
        }

        public DummyCanvasModule()
        {
            CssClass += " grid_9";
        }

        protected override void WriteEditArea(HtmlTextWriter writer)
        {
            writer.WriteLine("[[ TODO: EDIT AREA CONTENT ]]");
        }

        protected override void WriteContentArea(HtmlTextWriter writer)
        {
            writer.WriteLine("[[ TODO: MODULE CONTENT ]]");
        }
    }
}