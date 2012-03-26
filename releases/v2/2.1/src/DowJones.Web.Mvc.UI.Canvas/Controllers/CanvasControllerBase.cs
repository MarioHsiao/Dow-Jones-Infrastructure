using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Pages;
using DowJones.Web.Mvc.ActionFilters;
using DowJones.Web.Mvc.Extensions;
using Page = Factiva.Gateway.Messages.Assets.Pages.V1_0.Page;

namespace DowJones.Web.Mvc.UI.Canvas.Controllers
{
    public class CanvasControllerBase : ControllerBase
    {
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPageAssetsManager PageAssetsManager { get; set; }

        protected PageCollection PageCollection
        {
            get { return ViewData.SingleOrDefault<PageCollection>(); }
        }

        [RequireAuthentication]
        public virtual ActionResult AddModule(string id, string pageId, string callback)
        {
            var moduleIds = new[] { id };
            PageAssetsManager.AddModuleIdsToEndOfPage(pageId, moduleIds.ToList());
            return Module(id, pageId, callback);
        }

//        [RequireAuthentication]
        public virtual ActionResult Module(string id, string pageId, string callback)
        {
            var factivaModule = PageAssetsManager.GetModuleById(pageId, id);

            var pagesModule = Mapper.Map<DowJones.Pages.Modules.Module>(factivaModule);
            var canvasModule = Mapper.Map<IModule>(pagesModule);

            return Module(canvasModule, callback);
        }

        protected virtual ActionResult Module(IModule canvasModule, string callback)
        {
            var module = canvasModule as Module;
            if (module != null)
            {
                if (module.ControlData == null)
                    module.ControlData = ControlData;

                if (module.Preferences == null)
                    module.Preferences = Preferences;
            }

            ViewData.Model = canvasModule;

            return new CanvasModuleViewResult
                       {
                           Callback = callback,
                           ViewData = ViewData,
                           TempData = TempData
                       };
        }

        [RequireAuthentication]
        public virtual ActionResult Page(string id)
        {
            Page page = null;

            if(!string.IsNullOrWhiteSpace(id))
                page = PageAssetsManager.GetPage(id);

            if (page == null)
                return PageNotFound(id);

            return Canvas(new Canvas(), page);
        }

        public virtual ActionResult PageNotFound(string id)
        {
            var result = View("PageNotFound");
            result.ViewData.Model = id;
            return result;
        }

        [RequireAuthentication]
        public virtual ActionResult Subscribe(string id, string position)
        {
            int positionNumber;
            if (!int.TryParse(position ?? string.Empty, out positionNumber))
                positionNumber = 1;

            id = PageAssetsManager.SubscribeToPage(id, positionNumber);

            return Page(id);
        }

        [Obsolete("Avoid using Gateway objects directly within the UI layer")]
        protected CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvas, Page page)
            where TCanvasModel : Canvas
        {
            Guard.IsNotNull(canvas, "canvas");
            Guard.IsNotNull(page, "page");

            if (canvas.CanvasID.IsNullOrEmpty())
                canvas.CanvasID = page.Id.ToString();

            return Canvas(canvas, page.ModuleCollection);
        }

        [Obsolete("Avoid using Gateway objects directly within the UI layer")]
        protected CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvas, IEnumerable<Factiva.Gateway.Messages.Assets.Pages.V1_0.Module> modules)
            where TCanvasModel : Canvas
        {
            Guard.IsNotNull(canvas, "canvas");
            Guard.IsNotNull(modules, "modules");

            var canvasModules = modules.Select(Mapper.Map<IModule>);
            return Canvas(canvas, canvasModules);
        }

        protected CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvas, IEnumerable<IModule> modules = null)
            where TCanvasModel : Canvas
        {
            Guard.IsNotNull(canvas, "canvas");
            Guard.IsNotNull(modules, "modules");

            if (canvas.ControlData == null)
                canvas.ControlData = ControlData;

            if (canvas.Preferences == null)
                canvas.Preferences = Preferences;

            if (string.IsNullOrWhiteSpace(canvas.LoadModuleUrl))
                canvas.LoadModuleUrl = Url.Action("Module");

            if (string.IsNullOrWhiteSpace(canvas.AddModuleUrl))
                canvas.AddModuleUrl = Url.Action("AddModule");

            canvas.AddChildren(modules ?? Enumerable.Empty<IModule>());

            ViewData.Model = canvas;

            var result = new CanvasViewResult(canvas)
            {
                ViewData = ViewData,
                TempData = TempData
            };

            return result;
        }
    }
}
