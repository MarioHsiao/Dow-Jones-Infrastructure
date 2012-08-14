using System;
using DowJones.Pages;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Showcase.Models;
using Module = DowJones.Pages.Modules.Module;

namespace DowJones.Web.Showcase.Controllers
{
    public class CanvasControllerBase : DowJones.Web.Mvc.UI.Canvas.Controllers.DashboardControllerBase
    {
        protected CanvasViewResult Canvas(params IModule[] modules)
        {
            return Canvas(new ShowcaseCanvasModel(), modules);
        }

        protected override CanvasModuleViewResult AddModuleInternal(string id, string pageId, string callback)
        {
            throw new NotImplementedException();
        }

        protected override Module GetModule(string id, string pageId)
        {
            throw new NotImplementedException();
        }

        protected override Page GetPage(string id)
        {
            throw new NotImplementedException();
        }

        protected override string SubscribeToPage(string id, int positionNumber)
        {
            return string.Empty;
        }
    }
}