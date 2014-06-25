using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Extensions;
using DowJones.Web.Mvc.ModelBinders;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class MtaDemoController : ControllerBase
    {
        public ActionResult ATest([ModelBinder(typeof(CommaStringSplitModelBinder))]string[] syms)
        {
            var items = new List<string>(syms ?? new[] { "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" });
            return View("ApartmentState", new { ActionName = "Test2", Items = items }.ToExpando());
        }
        public ActionResult Index()
        {
            return View("ApartmentState", new { ActionName = "Index", Items = new List<string>() }.ToExpando());
        }

        public ActionResult Test1()
        {
            return View("ApartmentState", new { ActionName = "Test1", Items = new List<string>() }.ToExpando());
        }

        public ActionResult Test2([ModelBinder(typeof(CommaStringSplitModelBinder))]string[] syms)
        {
            var items = new List<string>(syms ?? new[] { "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" });
            return View("ApartmentState", new { ActionName = "Test2", Items = items }.ToExpando());
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewData["ApartmentState"] = System.Threading.Thread.CurrentThread.GetApartmentState();
        }
         
    }
}