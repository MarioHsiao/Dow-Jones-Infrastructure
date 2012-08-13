using System.Web.Mvc;
using DowJones.Pages;
using DowJones.Web.Mvc.Routing;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers
{
    public class HtmlModuleController : ModuleController<CanvasModules.HtmlModule.HtmlModule, Modules.HtmlModule>
    {
        public HtmlModuleController(IPageManager pageManager)
            : base(pageManager)
        {
        }

        [Route("canvas/modules/Html/{version}/data/{method}")]
        [ValidateInput(false)]
        public override ActionResult Post(string pageId, int? moduleId, CanvasModules.HtmlModule.HtmlModule canvasModule)
        {
            return base.Post(pageId, moduleId, canvasModule);
        }

        protected override void MapModule(CanvasModules.HtmlModule.HtmlModule source, Modules.HtmlModule dest)
        {
            dest.Html = source.Html;
            dest.Script = source.Script;
        }
    }
}