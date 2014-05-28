using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core.PlotOptions
{
    [Serializable]
    public class PlotOptionsBar : PlotOptionsSeries
    {
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
        public int? BorderRadius { get; set; }
        public int? BorderWidth { get; set; }
        public bool? ColorByPoint { get; set; }
        public int? GroupPadding { get; set; }
        public int? MinPointLength { get; set; }
        public int? PointPadding { get; set; }
        public int? PointWidth { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                 {
                                                                                     NullValueHandling = NullValueHandling.Ignore, 
                                                                                     DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                     ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                 });

            return !string.IsNullOrEmpty(ignored) ? string.Format("plotOptions: {{ series: {0} }},", ignored) : string.Empty;
        }
    }
}