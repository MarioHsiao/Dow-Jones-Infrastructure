using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.UI.Generators
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class LocalizationScriptGenerator : IScriptGenerator
    {
        public string DecimalPoint;
        public string DownloadJpeg;
        public string DownloadPdf;
        public string DownloadPng;
        public string DownloadSvg;
        public string ExportButtonTitle;
        public string Loading;

        public string[] Months;

        public string PrintButtonTitle;
        public string ResetZoom;
        public string ResetZoomTitle;
        public string[] ShortMonths;
        public string ThousandsSep;
        public string[] Weekdays;

        public void LoadDefaults()
        {
            DecimalPoint = ".";
            DownloadPng = "Download PNG image";
            DownloadJpeg = "Download JPEG image";
            DownloadPdf = "Download PDF document";
            DownloadSvg = "Download SVG vector image";
            ExportButtonTitle = "Export to raster or vector image";
            Loading = "Loading...";

            Months = new[]
                         {
                             "January", "February", "March", "April", "May", "June", "July",
                             "August", "September", "October", "November", "December"
                         };

            ShortMonths = new[]
                              {
                                  "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                                  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                              };

            PrintButtonTitle = "Print the chart";
            ResetZoom = "Reset zoom";
            ResetZoomTitle = "Reset zoom level 1:1";
            ThousandsSep = ",";
            Weekdays = new[] {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
        }

        public string RenderScript()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                                    });
            return @"Highcharts.setOptions({
                        lang: " + ignored +
                   "});";
        }
    }
}