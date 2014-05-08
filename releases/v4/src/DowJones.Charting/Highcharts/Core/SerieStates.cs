using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class SerieStates
    {
        public SerieStateSettings Hover;

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