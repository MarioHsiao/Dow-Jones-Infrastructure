using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core.PlotOptions
{
    [Serializable]
    public class PlotOptionsAreaSpline : PlotOptionsSeries
    {
        public string FillColor { get; set; }
        public double? FillOpacity { get; set; }
        public string LineColor { get; set; }
        public int? Threshold { get; set; }

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