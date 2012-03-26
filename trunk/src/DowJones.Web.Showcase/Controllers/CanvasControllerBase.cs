using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Showcase.Models;

namespace DowJones.Web.Showcase.Controllers
{
    public abstract class CanvasControllerBase : DowJones.Web.Mvc.UI.Canvas.Controllers.CanvasControllerBase
    {
        protected CanvasViewResult Canvas(params IModule[] modules)
        {
            return Canvas(new ShowcaseCanvasModel(), modules);
        }
    }
}