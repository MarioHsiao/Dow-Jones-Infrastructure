using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class Exporting
    {
        public Exporting()
        {
            Enabled = false;
        }

        public bool? Enabled { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                 {
                                                                                     NullValueHandling = NullValueHandling.Ignore,
                                                                                     ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                 });
            return string.Format("exporting: {0},", ignored);
        }
    }
}