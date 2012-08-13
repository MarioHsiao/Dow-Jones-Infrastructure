using System.Web.Mvc;
using DowJones.Pages;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers
{
    public class EmbeddedContentModuleController : ModuleController<EmbeddedContentModule, Modules.EmbeddedContentModule>
    {
        public EmbeddedContentModuleController(IPageManager pageManager)
            : base(pageManager)
        {
        }

        [Route("canvas/modules/EmbeddedContent/{version}/data/{method}")]
        [ValidateInput(false)]
        public override ActionResult Post(string pageId, int? moduleId, EmbeddedContentModule canvasModule)
        {
            return base.Post(pageId, moduleId, canvasModule);
        }

        protected override void MapModule(EmbeddedContentModule source, Modules.EmbeddedContentModule dest)
        {
            dest.Height = source.Height;
            dest.Width = source.Width;
            dest.Url = source.Url;
        }
    }
}