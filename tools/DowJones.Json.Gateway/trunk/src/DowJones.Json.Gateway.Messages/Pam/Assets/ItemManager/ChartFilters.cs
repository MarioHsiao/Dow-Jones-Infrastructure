using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ChartFilters")]
    public class ChartFilters
    {
        public ChartFilters()
        {
            ChartFilter = new List<ChartFilter>();
        }

        [DataMember(Name = "chartFilterType")]
        public ChartFilterType ChartFilterType { get; set; }

        [DataMember(Name = "chartFilter")]
        public List<ChartFilter> ChartFilter { get; set; }

        [DataMember(Name = "chartFilterMode")]
        public ChartFilterMode ChartFilterMode { get; set; }
    }
}