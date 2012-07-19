using DowJones.Search;

namespace DowJones.Web.Mvc.Search.UI.Components.Builders.Alert
{
    public class AlertSearchBuilder : SearchBuilder
    {
       
        public int? SelectedAlertId { get; set; }
        public string SelectedAlertText { get; set; }
        public AlertHeadlineViewType AlertHeadlineViewType { get; set; }
        public string ServiceUrl { get; set; }
        public string PostBackUrl { get; set; }

        public bool HideAlertList { get; set; }
        public string Sessionmark { get; set; }
        public string Bookmark { get; set; }
    }
}
