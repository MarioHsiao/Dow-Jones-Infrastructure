using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core.PlotOptions
{
    [Serializable]
    public class PlotOptionsPie : PlotOptionsSeries
    {
        public string BorderColor { get; set; }
        public int? BorderWidth { get; set; }
        public object InnserSize { get; set; }
        public int? Size { get; set; }
        public int? SliceOffset { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore, 
                                                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                    });

            return !string.IsNullOrEmpty(ignored) ? string.Format("plotOptions: {{ pie: {0} }},", ignored) : string.Empty;
        }
    }
}