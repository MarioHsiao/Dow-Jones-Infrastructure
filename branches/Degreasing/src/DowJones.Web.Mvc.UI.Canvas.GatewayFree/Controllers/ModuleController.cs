using System.Web.Mvc;
using DowJones.Pages;
using DowJones.Web.Mvc.UI.Canvas.Controllers;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers
{
    using CanvasModule = DowJones.Web.Mvc.UI.Canvas.Module;

    public abstract class ModuleController<TCanvasModule> : PageControllerBase
        where TCanvasModule : CanvasModule
    {
        private readonly IPageManager _pageManager;

        protected IPageManager PageManager
        {
            get { return _pageManager; }
        }

        protected ModuleController(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }


        [ValidateInput(false)]
        public ActionResult Create(string pageId, [Bind(Prefix = "")]TCanvasModule canvasModule)
        {
            var module = new Modules.HtmlModule();

            MapModule(canvasModule, module);

            PageManager.AddModuleToPage(pageId, module);

            return new HttpStatusCodeResult(200);
        }

        [ValidateInput(false)]
        public ActionResult Update(string pageId, int moduleId, [Bind(Prefix = "")]TCanvasModule canvasModule)
        {
            var module = (Modules.HtmlModule)PageManager.GetModuleById(moduleId);

            MapModule(canvasModule, module);

            PageManager.UpdateModule(module);

            return new HttpStatusCodeResult(200);
        }
        
        protected abstract void MapModule(TCanvasModule source, Pages.Modules.Module dest);
    }
}