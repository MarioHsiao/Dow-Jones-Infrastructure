using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Components.NewsRadar
{
    [DataContract(Name="newsVolume")]
    public class NewsVolumeModel
    {
        [DataMember(Name = "timeFrame")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TimeFrame TimeFrame { get; set; }

        [DataMember(Name = "hitCount")]
        public int HitCount { get; set; }
    }
}
