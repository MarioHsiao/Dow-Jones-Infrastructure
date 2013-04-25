using System;

namespace DowJones.Managers.Rss
{
    public class RssItem
    {
        public long FeedId { get; set; }
        public long SyndicationId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public ushort AggregationDays { get; set; }
    }
}
