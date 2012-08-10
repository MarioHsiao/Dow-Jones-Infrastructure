using System.Web.Mvc;
using DowJones.Pages;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Canvas.GatewayFree.CanvasModules.EmbeddedContent;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers
{
    public class EmbeddedContentModuleController : ModuleController<EmbeddedContentModule>
    {
        public EmbeddedContentModuleController(IPageManager pageManager)
            : base(pageManager)
        {
        }

        [Route("canvas/modules/EmbeddedContent/{version}/data/{method}")]
        [ValidateInput(false)]
        public ActionResult Post(string pageId, int? moduleId, [Bind(Prefix = "")]EmbeddedContentModule canvasModule)
        {
            if (moduleId.GetValueOrDefault() == default(int))
                return Create(pageId, canvasModule);
            else
                return Update(pageId, moduleId.Value, canvasModule);
        }

        protected override void MapModule(EmbeddedContentModule source, Pages.Modules.Module dest)
        {
            MapEmbeddedContentModule(source, dest as Modules.EmbeddedContentModule);
        }

        private void MapEmbeddedContentModule(EmbeddedContentModule source, Modules.EmbeddedContentModule dest)
        {
            dest.Height = source.Height;
            dest.Width = source.Width;
            dest.Url = source.Url;
        }
    }
}