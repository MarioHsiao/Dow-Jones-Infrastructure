using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using Factiva.Gateway.Messages.Track.V1_0;
using Factiva.Gateway.Messages.CodedNews;
using DowJones.Web.Mvc.UI.Models.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using DowJones.Utilities.Search.Core;
using System.Collections.Generic;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public class SimpleAlertServiceRequest : IRequest
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
        
        public virtual bool IsValid()
        {
            //ToDo: Check for other conditions also
            return AlertName.IsNotEmpty();
        }
    }

    public class Source: SourceList  
    {
        
        public string Name { get; set; }
        
    }

    public class SelectedSources
    {
        public SearchSourceGroupPreferenceItem SourceList { get; set; }
        public List<Source> Source { get; set; }

    }
}
