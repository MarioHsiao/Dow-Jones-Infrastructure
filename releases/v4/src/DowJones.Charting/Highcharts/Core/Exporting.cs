using System;
using DowJones.Charting.Highcharts.Core.PlotOptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    public class Exporting
    {
        public Exporting()
        {
            Enabled = false;
        }

        public bool? Enabled { get; set; }
        public ChartOptions ChartOptions { get; set; }
        public string Filename { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public int? Width { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                 {
                                                                                     NullValueHandling = NullValueHandling.Ignore,
                                                                                     ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                 });
            return string.Format("exporting: {0},", ignored);
        }
    }
}