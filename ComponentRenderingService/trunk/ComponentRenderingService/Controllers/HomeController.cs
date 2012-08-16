using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DowJones.Web.ComponentRenderingService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			// Just return that we are alive
			return new HttpStatusCodeResult(200, "Working!");
        }

    }
}
