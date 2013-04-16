using System;
using DowJones.Charting.Highcharts.Core.Appearance;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    public class Credits
    {
        public bool? Enabled { get; set; }
        public string Href { get; set; }
        public string Text { get; set; }
        public CSSObject Style { get; set; }
        public Position Position { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                                        DefaultValueHandling = DefaultValueHandling.Include,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                    });
            return string.Format("credits: {0},", ignored);

        }
    }

    [Serializable]
    public class Position
    {
        [JsonConverter(typeof (StringEnumConverter))]
        public Align? Align { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        public VerticalAlign? VerticalAlign { get; set; }

        public int? X { get; set; }
        public int? Y { get; set; }
    }
}