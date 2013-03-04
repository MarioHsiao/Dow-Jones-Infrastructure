using System.Net;
using System.Web;
using DowJones.Extensions;
using DowJones.Web.Handlers.Proxy;

namespace DowJones.Charting.Highcharts
{
    public class ExporterHttpHandler : StreamingProxyHandler
    {
        private const string UrlParam = "url";
        private const string UrlValue = "http://export.highcharts.com/";

        public override void ProcessRequest(HttpContext context)
        {
               
            if (IsValid(context)) throw new HttpException((int)HttpStatusCode.BadRequest, "Invalid Parameters");
            base.ProcessRequest(context);
        }

        private static bool IsValid(HttpContext context)
        {
            var url = context.Request[UrlParam] ?? string.Empty;
            if (url.IsEmpty())
            {
                if (context.Request.Form.AllKeys.Contains(UrlParam, true))
                {
                    context.Request.Form[UrlParam] = UrlValue;
                }
                else
                {
                    context.Request.Form.Add(UrlParam, UrlValue);
                }

                return true;
            }
            return false;
        }
    }
}
