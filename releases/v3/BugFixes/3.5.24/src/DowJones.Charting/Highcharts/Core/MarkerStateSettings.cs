using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class MarkerStateSettings
    {
        public bool? Enabled { get; set; }
        public string FillColor { get; set; }
        public string LineColor { get; set; }
        public int? LineWidth { get; set; }
        public int? Radius { get; set; }

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