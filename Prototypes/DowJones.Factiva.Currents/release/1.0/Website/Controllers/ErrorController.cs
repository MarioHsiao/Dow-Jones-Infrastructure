using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DowJones.Factiva.Currents.Website.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
			Response.StatusCode = (int)HttpStatusCode.NotFound; ;
			Response.TrySkipIisCustomErrors = true;
            return View();
        }

		public ActionResult GenericError()
		{
			Response.TrySkipIisCustomErrors = true;
			Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			return View();
		}

    }
}
