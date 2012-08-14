using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Pages;
using Page = Factiva.Gateway.Messages.Assets.Pages.V1_0.Page;

namespace DowJones.Web.Mvc.UI.Canvas.Controllers
{
    public class CanvasControllerBase : DashboardControllerBase
    {
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPageAssetsManager PageAssetsManager { get; set; }

        protected CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvas, Page page)
            where TCanvasModel : Canvas
        {
            Guard.IsNotNull(canvas, "canvas");
            Guard.IsNotNull(page, "page");

            if (canvas.CanvasID.IsNullOrEmpty())
                canvas.CanvasID = page.Id.ToString();

            return Canvas(canvas, page.ModuleCollection);
        }

        protected CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvas, IEnumerable<Factiva.Gateway.Messages.Assets.Pages.V1_0.Module> modules)
            where TCanvasModel : Canvas
        {
            Guard.IsNotNull(canvas, "canvas");
            Guard.IsNotNull(modules, "modules");

            var canvasModules = modules.Select(Mapper.Map<IModule>);
            return Canvas(canvas, canvasModules);
        }

        protected override Pages.Modules.Module GetModule(string id, string pageId)
        {
            var factivaModule = PageAssetsManager.GetModuleById(pageId, id);
            var pagesModule = Mapper.Map<DowJones.Pages.Modules.Module>(factivaModule);
            return pagesModule;
        }

        protected override Pages.Page GetPage(string id)
        {
            var page = PageAssetsManager.GetPage(id);
            return Mapper.Map<Pages.Page>(page);
        }

        protected override CanvasModuleViewResult AddModuleInternal(string id, string pageId, string callback)
        {
            var moduleIds = new[] { id };
            PageAssetsManager.AddModuleIdsToEndOfPage(pageId, moduleIds.ToList());
            return ModuleInternal(id, pageId, callback);
        }

        protected override string SubscribeToPage(string id, int positionNumber)
        {
            return PageAssetsManager.SubscribeToPage(id, positionNumber);
        }
    }
}
