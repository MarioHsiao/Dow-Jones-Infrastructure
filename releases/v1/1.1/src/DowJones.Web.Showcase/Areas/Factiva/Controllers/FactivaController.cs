using System.Diagnostics;
using System.Web.Mvc;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Showcase.Areas.Factiva.Modules;
using DowJones.Web.Showcase.Mocks;
using DowJones.Web.Showcase.Models;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DowJones.Web.Showcase.Areas.Factiva.Models;


namespace DowJones.Web.Showcase.Areas.Factiva.Controllers
{
    [HandleError(ExceptionType = typeof(DowJonesUtilitiesException), View = "UtilitiesException")]
    public class FactivaController : CanvasControllerBase
    {
        public ActionResult Index(string id = "12713")
        {
            Page page = PageAssetsManager.GetPage(id);

            return Canvas(new FactivaCanvasModel(), page);
        }


        public ActionResult Sandbox(string id)
        {
            Page page;

            if (string.IsNullOrWhiteSpace(id))
                page = new MockPageAssetsManager().GetPage(1.ToString());
            else
                page = PageAssetsManager.GetPage(id);

            var canvasResult = Canvas(new FactivaCanvasModel(), page);
            canvasResult.ViewName = "Sandbox";

            return canvasResult;
        }

        public override ActionResult AddModule(string id, string pageId, string callback)
        {
            Debug.WriteLine("Adding module {0} to Page {1} with callback {2}", id, pageId, callback);
            return Module(id, pageId, callback);
        }

        public ActionResult API( )
        {
            var controlData = new ShowcaseControlData(ControlData);
            controlData.Password = Request["password"];

            return View("API", controlData);
        }

        public ActionResult ModuleGarden(int id = 1)
        {
            Page page = PageAssetsManager.GetPage(id.ToString());

            var canvasResult = Canvas(new ModuleGardenModel(), page);
            canvasResult.ViewName = "ModuleGarden";
            return canvasResult;

        }
    }
}
