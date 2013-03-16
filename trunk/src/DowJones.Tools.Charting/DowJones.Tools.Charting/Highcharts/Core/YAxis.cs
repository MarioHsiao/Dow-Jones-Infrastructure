using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class YAxis : List<YAxisItem>
    {
        public override string ToString()
        {
            string serialize = string.Join(",", this.Select(axis => JsonConvert.SerializeObject(axis, Formatting.None, new JsonSerializerSettings
                                                                                                                           {
                                                                                                                               NullValueHandling = NullValueHandling.Ignore,
                                                                                                                               ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                                                                           })).ToArray());
            return string.Format("yAxis: [{0}],", serialize);
            //return string.Format("yAxis: [{0}],", ignored);
        }
    }
}