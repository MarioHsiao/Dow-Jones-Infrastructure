using System.Net;
using System.Web.Mvc;

namespace $rootnamespace$.Controllers
{
    public class ErrorController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Response.TrySkipIisCustomErrors = true;
        }

        public ActionResult NotFound404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("GenericError", new { ErrorCode = "210904" });
        }

        public ActionResult InternalServer500()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("GenericError", new { ErrorCode = "210905" });
        }

        public ActionResult Generic()
        {
            Response.StatusCode = 418; // Assume HTCPCP: http://tools.ietf.org/html/rfc2324
            return View("GenericError", new { ErrorCode = "-1" });
        }
    }
}