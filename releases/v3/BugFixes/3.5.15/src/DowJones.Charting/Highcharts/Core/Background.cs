using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    public class Background
    {
        public int[] LinearGradient { get; set; }
        public Collection<object[]> Stops { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
                                                                                     {
                                                                                         NullValueHandling = NullValueHandling.Ignore,
                                                                                         ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                     });
            return string.Format("{0},", ignored);
        }
    }
}