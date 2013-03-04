using System;
using System.Net;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Infrastructure.Common;
using DowJones.Loggers;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using DowJones.Token;
using DowJones.Web.Configuration;
using DowJones.Web.Mvc.ActionResults;
using DowJones.Web.Mvc.UI;
using log4net;

namespace DowJones.Web.Mvc
{
    public abstract class ControllerBase : Controller
    {
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IControlData ControlData
        {
            get { return ViewBag.ControlData; }
            set { ViewBag.ControlData = value; }
        }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected ITransactionTimer TransactionTimer
        {
            get { return ViewBag.ITransactionTimer; }
            set { ViewBag.ITransactionTimer = value; }
        }
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected ILog Log
        {
            // Provide a "sane default" so this is never the cause of NRE's
            get { return log ?? LogManager.GetLogger(GetType()); }
            set { log = value; }
        }
        private ILog log;

        [Obsolete("Use ClientConfiguration instead")]
        protected ClientConfiguration GlobalHeaders
        {
            get { return ClientConfiguration; }
        }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected ClientConfiguration ClientConfiguration
        {
            get { return ViewBag.ClientConfiguration; }
            set { ViewBag.ClientConfiguration = value; }
        }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IUserContext UserContext
        {
            get { return ViewBag.UserContext; }
            set { ViewBag.UserContext = value; }
        }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPreferences Preferences
        {
            get { return ViewBag.Preferences; }
            set { ViewBag.Preferences = value; }
        }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected IPrinciple Principle
        {
            get { return ViewBag.Principle; }
            set { ViewBag.Principle = value; }
        }

        [Inject("Avoiding constructor injection in abstract class")]
        protected Product Product { get; set; }

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

            var result = ServiceLocator.Resolve<ViewComponentViewResult>();
            result.Callback = callback ?? RouteData.Values["callback"] as string;
            result.TempData = TempData;
            result.ViewData = ViewData;

            return result;
        }

        protected virtual ActionResult Unauthorized()
        {
            return new HttpStatusCodeResult((int)HttpStatusCode.Unauthorized);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NewtonsoftJsonResult
                       {
                           ContentEncoding = contentEncoding,
                           ContentType = contentType,
                           Data = data,
                           JsonRequestBehavior = behavior,
                       };
        }
    }
}
