using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core.PlotOptions
{
    public class PlotOptionsSpline : PlotOptionsSeries
    {
        public bool? step { get; set; }

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