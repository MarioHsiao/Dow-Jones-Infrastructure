using System.Collections.Generic;
using DowJones.Managers.Search.CodedNewsQueries;
using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Newtonsoft.Json;
using FilterItem = DowJones.Utilities.Search.Core.FilterItem;

namespace DowJones.Infrastructure.Alert
{
    public class SimpleAlertRequest
    {
        public string AlertName { get; set; }
        public string SearchText { get; set; }
        public string EmailAddress { get; set; }
        public DocumentFormat EmailFormat { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public DeliveryTimes DeliveryTime { get; set; }
        public RemoveDuplicate RemoveDuplicate { get; set; }
        public List<NewsFilterCollection> NewsFilter { get; set; }
        public SelectedSources SelectedSources { get; set; }
        public bool AdjustToDaylightSavingsTime { get; set; }
        public string TimeZoneOffset { get; set; }
    }

    public class Source : SourceList
    {
        public string Name { get; set; }
    }

    public class SelectedSources
    {
        public SearchSourceGroupPreferenceItem SourceList { get; set; }
        public List<Source> Source { get; set; }
    }

    public class AlertNewsFilters
    {
        [JsonProperty("author")]
        public FilterList<FilterItem> Author { get; set; }

        [JsonProperty("executive")]
        public FilterList<FilterItem> Executive { get; set; }

        [JsonProperty("industry")]
        public FilterList<FilterItem> Industry { get; set; }

        [JsonProperty("newssubject")]
        public FilterList<FilterItem> Subject { get; set; }

        [JsonProperty("region")]
        public FilterList<FilterItem> Region { get; set; }

        [JsonProperty("company")]
        public FilterList<FilterItem> Company { get; set; }

        [JsonProperty("companyoccur")]
        public FilterList<FilterItem> CompanyOccur { get; set; }

        [JsonProperty("source")]
        public FilterList<FilterItem> Source { get; set; }
    }
}
