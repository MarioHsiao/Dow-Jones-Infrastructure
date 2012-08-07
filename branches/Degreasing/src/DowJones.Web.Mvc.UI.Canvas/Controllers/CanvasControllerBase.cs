using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using DowJones.Pages;
using DowJones.Web.Mvc.ActionFilters;
using DowJones.Web.Mvc.Extensions;

namespace DowJones.Web.Mvc.UI.Canvas.Controllers
{
    [Obsolete("Use PageControllerBase instead")]
    public class CanvasControllerBase : PageControllerBase
    {
        [Obsolete]
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPageAssetsManager PageAssetsManager { get; set; }
    }

    public class PageControllerBase : ControllerBase
    {
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPageManager PageManager { get; set; }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPageSubscriptionManager PageSubscriptionManager { get; set; }

        protected PageCollection PageCollection
        {
            get { return ViewData.SingleOrDefault<PageCollection>(); }
        }

        [RequireAuthentication]
        public virtual ActionResult AddModule(string id, string pageId, string callback)
        {
            return AddModuleInternal(id, pageId, callback);
        }

        protected virtual CanvasModuleViewResult AddModuleInternal(string id, string pageId, string callback)
        {
            PageManager.AddModuleToPage(pageId, id);
            return ModuleInternal(id, pageId, callback);
        }

        [RequireAuthentication]
        public virtual ActionResult Module(string id, string pageId, string callback)
        {
            return ModuleInternal(id, pageId, callback);
        }

        protected virtual CanvasModuleViewResult ModuleInternal(string id, string pageId, string callback)
        {
            var pagesModule = PageManager.GetModuleById(pageId, id);
            var canvasModule = Mapper.Map<IModule>(pagesModule);

            return Module(canvasModule, callback);
        }

        protected virtual CanvasModuleViewResult Module(IModule canvasModule, string callback)
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
            
            var result = ServiceLocator.Resolve<CanvasModuleViewResult>();
            result.Callback = callback;
            result.ViewData = ViewData;
            result.TempData = TempData;

            return result;
        }

        [RequireAuthentication]
        public virtual ActionResult Page(string id)
        {
            Page page = null;

            if(!string.IsNullOrWhiteSpace(id))
                page = PageManager.GetPage(id);

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

        public virtual ActionResult Subscribe(string id, string position)
        {
            int positionNumber;
            if (!int.TryParse(position ?? string.Empty, out positionNumber))
                positionNumber = 1;

            id = PageSubscriptionManager.SubscribeToPage(id, positionNumber);

            return Page(id);
        }


        protected virtual CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvas, Page page)
            where TCanvasModel : Canvas
        {
            var modules = page.ModuleCollection.Select(Mapper.Map<IModule>).ToArray();
            
            var viewResult = Canvas(canvas, modules);
            var viewModel = ((TCanvasModel) viewResult.Model);

            if (string.IsNullOrWhiteSpace(viewModel.CanvasID))
                viewModel.CanvasID = page.ID;

            if (string.IsNullOrWhiteSpace(viewModel.Title))
                viewModel.Title = page.Title;
            
            return viewResult;
        }

        protected virtual CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvas, IEnumerable<IModule> modules = null)
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

            canvas.AddChildren((modules ?? Enumerable.Empty<IModule>()).OrderBy(x => x.Position));

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
