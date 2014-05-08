using System;
using DowJones.Charting.Highcharts.Core.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    public class PlotBand
    {
        public string Color { get; set; }
        public PlotEvents Events { get; set; }
        public double? From { get; set; }
        public string Id { get; set; }
        public PlotLabel Label { get; set; }
        public double? To { get; set; }
        [JsonProperty("zIndex")]
        public int? zIndex { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                          {
                                                                              NullValueHandling = NullValueHandling.Ignore,
                                                                              ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                          });
        }
    }
}