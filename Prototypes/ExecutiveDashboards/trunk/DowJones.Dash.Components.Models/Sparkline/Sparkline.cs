using System.Collections.Generic;
using DowJones.Web.Mvc.UI;
using Newtonsoft.Json;

namespace DowJones.Dash.Components.Models.Sparkline
{
    public class SparklineResult
    {
        [JsonProperty("max")]
        public double Max { get; set; }

        [JsonProperty("min")]
        public double Min { get; set; }

        [JsonProperty("values")]
        public List<double> Values { get; set; }
    } 

    public class SparklineModel : ViewComponentModel
    {
        public SparklineModel()
        {
           
        }
    }
}