using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DowJones.Newsletters.Website.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return InternalServerError();
        }

        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("NotFound");
        }

        public ActionResult InternalServerError()
        {
            var exception = Server.GetLastError();

            var model = new HandleErrorInfo(Server.GetLastError(), "Error", "InternalServerError");
            // Log the exception.
            Response.Clear();

            // Clear the error on server.
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;

            var httpException = exception as HttpException;

            if (httpException == null)
            {
                
            }
            else //It's an Http Exception, Let's handle it.
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return NotFound();
                    case 500:
                        // Server error.
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return View("InternalServerError", model);

                    // Here you can handle Views to other error codes.
                    // I choose a General error template  
                    default:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return View("InternalServerError", model);
                }
            }
            
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("InternalServerError", model);
        }

    }
}
