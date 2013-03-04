using System.Net;
using System.Web;
using DowJones.Extensions;
using DowJones.Web.Handlers.Proxy;

namespace DowJones.Charting.Highcharts
{
    public class ExporterHttpHandler : StreamingProxyHandler
    {
        private const string DefinedUrlValue = "http://export.highcharts.com/";

        public ExporterHttpHandler()
        {
            DefinedUrl = DefinedUrlValue;
        }
    }
}
