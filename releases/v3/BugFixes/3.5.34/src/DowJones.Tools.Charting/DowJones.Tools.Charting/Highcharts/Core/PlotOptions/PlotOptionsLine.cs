using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core.PlotOptions
{
    [Serializable]
    public class PlotOptionsLine : PlotOptionsSeries
    {
        public bool? Step { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                    });

            return !string.IsNullOrEmpty(ignored) ? string.Format("plotOptions: {{ series: {0} }},", ignored) : string.Empty;
        }
    }


    public class PlotOptionsDateTimeLine : PlotOptionsSeries
    {
        public new DateTime? PointStart { get; set; }

        public override string ToString()
        {
            var ignored = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                                    {
                                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                        Converters = new List<JsonConverter>(new[] { new JavaScriptDateTimeConverter()})
                                                                                    });

            return !string.IsNullOrEmpty(ignored) ? string.Format("plotOptions: {{ series: {0} }},", ignored) : string.Empty;
        }
    }
}