using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Session;
using DowJones.Token;
using DowJones.Web.Mvc.UI;
using log4net;

namespace DowJones.Web.Mvc
{
    public abstract class DowJonesControllerBase : Controller
    {
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IControlData ControlData
        {
            get { return ViewBag.ControlData; }
            set { ViewBag.ControlData = value; }
        }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPreferences Preferences
        {
            get { return ViewBag.Preferences; }
            set { ViewBag.Preferences = value; }
        }


        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected ILog Log
        {
            // Provide a "sane default" so this is never the cause of NRE's
            get { return log ?? LogManager.GetLogger(GetType()); }
            set { log = value; }
        }
        private ILog log;

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected ScriptRegistryBuilder ScriptRegistry { get; set; }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected StylesheetRegistryBuilder StylesheetRegistry { get; set; }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected ITokenRegistry TokenRegistry { get; set; }


        protected ActionResult Components(string viewName, params IViewComponentModel[] componentModels)
        {
            return Components(viewName, null, componentModels);
        }

        protected ActionResult Components(string viewName, string masterName, params IViewComponentModel[] componentModels)
        {
            var componentContainer = new ContentContainerModel(componentModels);
            return View(viewName, masterName, componentContainer);
        }

        protected ActionResult ViewComponent(object viewComponentModel, string callback = null)
        {
            ViewData.Model = viewComponentModel;
            ViewBag.Callback = callback;

            var result = new ViewComponentViewResult
                             {
                                 TempData = TempData,
                                 ViewData = ViewData,
                             };

            return result;
        }
    }
}
