using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ChartItemProperties")]
    public class ChartItemProperties : ItemProperties
    {
        public ChartItemProperties()
        {
            ChartFilter = new List<ChartFilters>();
            ChartDates = new ChartDates();
            QueryReference = new List<QueryReference>();
        }

        [DataMember(Name = "graphType")]
        public GraphType GraphType { get; set; }

        [DataMember(Name = "metricType")]
        public MetricType? MetricType { get; set; }

        [DataMember(Name = "queryReference")]
        public List<QueryReference> QueryReference { get; set; }

        [DataMember(Name = "chartDates")]
        public ChartDates ChartDates { get; set; }

        [DataMember(Name = "chartFilter")]
        public List<ChartFilters> ChartFilter { get; set; }

        [DataMember(Name = "maxResults")]
        public int? MaxResults { get; set; }
    }
}