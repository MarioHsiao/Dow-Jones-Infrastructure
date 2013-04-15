using System;
using DowJones.Charting.Properties;
using DowJones.Web.Handlers.Proxy;

namespace DowJones.Charting.Highcharts
{
    public class ExporterHttpHandler : StreamingProxyHandler
    {
        private readonly Lazy<string> _definedTargetUrl = new Lazy<string>(()=> Settings.Default.HighChartsExportService);

        public ExporterHttpHandler()
        {
            DefinedTargetUrl = _definedTargetUrl.Value;
        }
    }
}
