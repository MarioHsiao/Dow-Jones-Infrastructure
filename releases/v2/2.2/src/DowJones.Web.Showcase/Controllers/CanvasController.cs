using System;
using System.Threading;
using System.Web.Mvc;
using DowJones.Web.Showcase.Models;
using DowJones.Web.Showcase.Modules.Empty;

namespace DowJones.Web.Showcase.Controllers
{
    public class CanvasController : CanvasControllerBase
    {

        public ActionResult Index( )
        {
            return Canvas(
                new EmptyModule { CanvasId = "CANVAS_1", ModuleId = 1234, Title = "Demo Module #1", ContentUrl = Url.Action("Content")},
                new EmptyModule { CanvasId = "CANVAS_1", ModuleId = 5678, Title = "Editable Module", CanEdit = true }
            );
        }

        public string Content(int sleep = 2)
        {
            Thread.Sleep(TimeSpan.FromSeconds(sleep));
            return string.Format("Current time: {0}", DateTime.Now);
        }

        public override ActionResult Module(string id, string pageId, string callback)
        {
            var module = new PortalHeadlineLists {CanvasId = "CANVAS_1", ModuleId = new Random().Next(99999), Title = "Portal Headlines", CanEdit = true};
            return Module(module, callback);
        }
    }
}