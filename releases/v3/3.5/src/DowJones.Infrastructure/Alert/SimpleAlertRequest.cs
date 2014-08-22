using System.Collections.Generic;
using DowJones.Managers.Search.CodedNewsQueries;
using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;

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
        public List<DeliveryContentType> DeliveryContentType { get; set; }
        public bool AllowMMContent { get; set; }
        public ResultsDisplayFormat ResultDisplayFormat { get; set; }
        public bool EnabledEmailHighlight { get; set; }
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

}
