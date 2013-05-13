using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Charting.Highcharts.Core
{
    [Serializable]
    public class YAxis : List<YAxisItem>
    {
        public YAxis(){}

        public YAxis(IEnumerable<YAxisItem> items) : base(items) {}

        public override string ToString()
        {
            var serialize = string.Join(",", this.Select(axis => JsonConvert.SerializeObject(axis, Formatting.None, new JsonSerializerSettings
                                                                                                                           {
                                                                                                                               NullValueHandling = NullValueHandling.Ignore,
                                                                                                                               ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                                                                           })).ToArray());
            return string.IsNullOrEmpty(serialize) ? serialize : string.Format("yAxis: [{0}],", serialize);
        }
    }
}