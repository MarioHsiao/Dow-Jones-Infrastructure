using System;
using DowJones.Tools.Charting.Highcharts.Core.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class PlotLine
    {
        public string Color { get; set; }
        public DashStyle? DashStyle { get; set; }
        public PlotEvents Events { get; set; }
        public string Id { get; set; }
        public PlotLabel Label { get; set; }
        public double? Value { get; set; }
        public int? Width { get; set; }

        [JsonProperty("zIndex")]
        public int? zIndex { get; set; }

        public override string ToString()
        {
            string ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                                    });
            return ignored;
        }
    }
}