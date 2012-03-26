using System;
using System.Threading;
using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Showcase.ActionFilters;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    [LogActivity]
    public class FiltersController : ControllerBase
    {
        public ActionResult Index( )
        {
            return LoggedAction();
        }

        public ActionResult LoggedAction()
        {
            return View("LoggedAction");
        }

        #region Caching

        [TrackExecutionTime]
        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult OutputCached()
        {
            ViewData["CacheType"] = "Cached";

            DoSomeWorkThatTakesAWhile();

            return View("OutputCache");
        }

        [TrackExecutionTime]
        public ActionResult Uncached()
        {
            ViewData["CacheType"] = "Uncached";

            DoSomeWorkThatTakesAWhile();

            return View("OutputCache");
        }

        private void DoSomeWorkThatTakesAWhile()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        #endregion

    }
}
