// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasControllerBase.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Managers.PAM;
using DowJones.Web.Mvc.UI.Canvas.Mapping;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using FactivaModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.Module;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class CanvasControllerBase : DowJonesControllerBase
    {
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPageAssetsManager PageAssetsManager { get; set; }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected FactivaModuleModelMapper ModuleMapper { get; set; }

        public virtual ActionResult AddModule(string id, string pageId, string callback)
        {
            var moduleIds = new[] { id };
            PageAssetsManager.AddModuleIdsToEndOfPage(pageId, moduleIds.ToList());
            return Module(id, pageId, callback);
        }

        public virtual ActionResult Module(string id, string pageId, string callback)
        {

            var factivaModule = PageAssetsManager.GetModuleById(pageId, id);


            var canvasModule = ModuleMapper.MapFactivaModule(factivaModule);

            if (canvasModule != null && string.IsNullOrWhiteSpace(canvasModule.CanvasId))
                canvasModule.CanvasId = pageId;

            var module = canvasModule as Module;
            if (module != null)
            {
                if (module.ControlData == null)
                    module.ControlData = ControlData;

                if (module.Preferences == null)
                    module.Preferences = Preferences;


            }

            ViewBag.Callback = callback;
            ViewData.Model = canvasModule;

            return new CanvasModuleViewResult
                       {
                           ViewData = ViewData,
                           TempData = TempData
                       };
        }


        protected CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvasModel, Page factivaPage)
            where TCanvasModel : Canvas
        {
            if (canvasModel.CanvasID.IsNullOrEmpty())
            {
                canvasModel.CanvasID = factivaPage.Id.ToString();
                canvasModel.Page = factivaPage;
            }
            return Canvas(canvasModel, factivaPage.ModuleCollection);
        }

        protected CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvasModel, IEnumerable<FactivaModule> factivaModules)
            where TCanvasModel : Canvas
        {
            var modules = ModuleMapper.MapFactivaModules(factivaModules);
            return Canvas(canvasModel, modules);
        }

        protected CanvasViewResult Canvas<TCanvasModel>(TCanvasModel canvasModel, IEnumerable<IModule> modules = null)
            where TCanvasModel : Canvas
        {
            if (canvasModel.ControlData == null)
                canvasModel.ControlData = ControlData;

            if (canvasModel.Preferences == null)
                canvasModel.Preferences = Preferences;

            if (string.IsNullOrWhiteSpace(canvasModel.LoadModuleUrl))
                canvasModel.LoadModuleUrl = Url.Action("Module");

            if (string.IsNullOrWhiteSpace(canvasModel.AddModuleUrl))
                canvasModel.AddModuleUrl = Url.Action("AddModule");

            canvasModel.AddChildren(modules ?? Enumerable.Empty<IModule>());

            ViewData.Model = canvasModel;

            var result = new CanvasViewResult(canvasModel)
            {
                ViewData = ViewData,
                TempData = TempData
            };

            return result;
        }
    }
}
