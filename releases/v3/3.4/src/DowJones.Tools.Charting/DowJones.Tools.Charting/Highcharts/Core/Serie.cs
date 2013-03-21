using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core.Data.Chart
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class Serie
    {
        public object[] Center;
        public object[] Data;
        public string Id;
        public int? InnerSize;
        public string Name;
        public int? Size;
        
        // Added stacking, but it isn't valid for all serie types. Should it be here?
        public object Stack;
        
        [JsonConverter(typeof (StringEnumConverter))] 
        public RenderType? Type;

        [JsonProperty("xAxis")]
        public int? xAxis;

        [JsonProperty("yAxis")]
        public int? yAxis;

        public int? Level { get; set; }
        public string Color { get; set; }
        public bool? ShowInLegend { get; set; }
        public bool? Selected { get; set; }
        public bool? Visible { get; set; }

        public override string ToString()
        {
            if (Center.Length < 2)
                Center = null;
            else if (Center.Length > 2)
                Center = new[] {Center.GetValue(0), Center.GetValue(1)};

            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                          {
                                                                              NullValueHandling = NullValueHandling.Ignore,
                                                                              ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                              Converters = new List<JsonConverter>( new[] {new JavaScriptDateTimeConverter()} ),
                                                                          });
        }
    }
}