using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class PlotBands : List<PlotBand>
    {
        public override string ToString()
        {
            return string.Join(",", this.Select(serie => JsonConvert.SerializeObject(serie, Formatting.None, new JsonSerializerSettings
                                                                                                                 {
                                                                                                                     NullValueHandling = NullValueHandling.Ignore,
                                                                                                                     ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                                                                 })).ToArray());
        }
    }
}