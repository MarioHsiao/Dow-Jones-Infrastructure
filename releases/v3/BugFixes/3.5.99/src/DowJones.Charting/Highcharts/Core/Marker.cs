using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class Marker : MarkerStateSettings
    {
        public MarkerStates States;
        public string Symbol;

        public Marker()
        {
        }

        public Marker(string symbol)
        {
            Symbol = symbol;
        }

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