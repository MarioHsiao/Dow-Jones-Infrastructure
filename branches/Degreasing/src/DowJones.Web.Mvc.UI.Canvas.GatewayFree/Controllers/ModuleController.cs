using System.Web.Mvc;
using DowJones.Pages;
using DowJones.Web.Mvc.UI.Canvas.Controllers;

namespace DowJones.Web.Mvc.UI.Canvas.GatewayFree.Controllers
{
    using CanvasModule = DowJones.Web.Mvc.UI.Canvas.Module;
    using Module = DowJones.Pages.Modules.Module;

    public abstract class ModuleController<TCanvasModule, TModule> : DashboardControllerBase
        where TCanvasModule : CanvasModule 
        where TModule : Module, new()
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
        public virtual ActionResult Post(string pageId, int? moduleId, [Bind(Prefix = "")]TCanvasModule canvasModule)
        {
            if (moduleId.GetValueOrDefault() == default(int))
                return Create(pageId, canvasModule);
            else
                return Update(pageId, moduleId.Value, canvasModule);
        }


        [ValidateInput(false)]
        public ActionResult Create(string pageId, [Bind(Prefix = "")]TCanvasModule canvasModule)
        {
            var module = new TModule();

            module.Title = canvasModule.Title;

            MapModule(canvasModule, module);

            var moduleId = PageManager.AddModuleToPage(pageId, module);

            return Json(new { moduleId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult Update(string pageId, int moduleId, [Bind(Prefix = "")]TCanvasModule canvasModule)
        {
            var module = (TModule)PageManager.GetModuleById(moduleId);

            module.Title = canvasModule.Title;

            MapModule(canvasModule, module);

            PageManager.UpdateModule(module);

            return new HttpStatusCodeResult(200);
        }
        
        protected abstract void MapModule(TCanvasModule source, TModule dest);
    }
}