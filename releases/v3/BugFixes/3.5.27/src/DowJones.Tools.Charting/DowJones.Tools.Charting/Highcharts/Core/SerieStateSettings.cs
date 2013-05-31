using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SerieStateSettings
    {
        [JsonProperty]
        public double? Brightness { get; set; }

        [JsonProperty]
        public bool? Enabled { get; set; }

        [JsonProperty]
        public int? LineWidth { get; set; }

        [JsonProperty]
        public Marker Marker { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                          {
                                                                              NullValueHandling = NullValueHandling.Include,
                                                                              ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                          });
        }
    }
}