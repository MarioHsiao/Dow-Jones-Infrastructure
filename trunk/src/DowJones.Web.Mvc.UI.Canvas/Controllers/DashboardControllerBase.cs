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
    public abstract class DashboardControllerBase : ControllerBase
    {
        protected PageCollection PageCollection
        {
            get { return ViewData.SingleOrDefault<PageCollection>(); }
        }

        [RequireAuthentication]
        public virtual ActionResult AddModule(string id, string pageId, string callback)
        {
            return AddModuleInternal(id, pageId, callback);
        }

        protected abstract CanvasModuleViewResult AddModuleInternal(string id, string pageId, string callback);

        [RequireAuthentication]
        public virtual ActionResult Module(string id, string pageId, string callback)
        {
            return ModuleInternal(id, pageId, callback);
        }

        protected virtual CanvasModuleViewResult ModuleInternal(string id, string pageId, string callback)
        {
            var module = GetModule(id, pageId);
            var canvasModule = Mapper.Map<IModule>(module);

            return Module(canvasModule, callback);
        }

        protected abstract Pages.Modules.Module GetModule(string id, string pageId);

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
                page = GetPage(id);

            if (page == null)
                return PageNotFound(id);

            return Canvas(new Canvas(), page);
        }

        protected abstract Page GetPage(string id);

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

            id = SubscribeToPage(id, positionNumber);

            return Page(id);
        }

        protected abstract string SubscribeToPage(string id, int positionNumber);

        protected virtual CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvas, Page page) 
            where TCanvasModel : Canvas
        {
            return Canvas(canvas, page.ModuleCollection.Select(Mapper.Map<IModule>));
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