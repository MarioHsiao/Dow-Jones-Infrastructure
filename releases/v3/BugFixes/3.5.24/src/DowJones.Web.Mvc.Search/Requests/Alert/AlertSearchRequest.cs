using DowJones.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.Search.Requests.Alert
{
    public class AlertSearchRequest : SearchRequest
    {
        public int AlertId { get; set; }

        public AlertHeadlineViewType ViewType { get; set; }

        public string Sessionmark { get; set; }

        public string Bookmark { get; set; }

        public bool HideAlertList { get; set; }
    }
}