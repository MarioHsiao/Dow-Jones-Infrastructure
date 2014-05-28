using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core.PlotOptions
{
    [Serializable]
    public class DataLabels : Labels
    {
        public DataLabels()
        {
            Enabled = true;
        }

        public string Color { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public VerticalAlign? VerticalAlign { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                          {
                                                                              NullValueHandling = NullValueHandling.Ignore,
                                                                              DefaultValueHandling = DefaultValueHandling.Include,
                                                                              ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                          });
        }
    }
}