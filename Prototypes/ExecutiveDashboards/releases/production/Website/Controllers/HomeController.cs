using System.Web.Mvc;

namespace DowJones.Dash.Website.Controllers
{
    public class HomeController : DowJones.Web.Mvc.ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Data()
        {
            return View();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Initialize SignalR
            ScriptRegistry.OnDocumentReady(@"
                $.connection.dashboard.publish = DJ.publish;
                $.connection.hub.start();");
        }
    }
}
