using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Showcase.Models;

namespace DowJones.Web.Showcase.Controllers
{
    public class CanvasControllerBase : DowJones.Web.Mvc.UI.Canvas.Controllers.PagesControllerBase
    {
        protected CanvasViewResult Canvas(params IModule[] modules)
        {
            return Canvas(new ShowcaseCanvasModel(), modules);
        }
    }
}