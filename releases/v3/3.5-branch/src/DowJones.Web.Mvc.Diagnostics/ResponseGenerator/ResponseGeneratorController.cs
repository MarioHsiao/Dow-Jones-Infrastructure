using System.Threading;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics.ResponseGenerator
{
    public class ResponseGeneratorController : DiagnosticsController
    {

        public ActionResult Index(int statusCode = 500, string body = null, string contentType = null, int? timeToProcess = null)
        {
            Response.TrySkipIisCustomErrors = true;
            ViewBag.StatusCode = Response.StatusCode = statusCode;
            ViewBag.TimeToProcess = timeToProcess.GetValueOrDefault();

            Thread.Sleep(ViewBag.TimeToProcess);

            if (!string.IsNullOrWhiteSpace(contentType))
                Response.ContentType = contentType;

            var hasBody = !string.IsNullOrWhiteSpace(body);
            if (hasBody)
                return Content(body);
            else
                return View<ResponseGeneratorView>(ViewBag);
        }

    }
}
