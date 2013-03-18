using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Web.Mvc.Search.Requests.Modules
{
    public class ModuleSearchRequest : SearchRequest
    {
        public string PageId { get; set; }

        public string PageName { get; set; }

        public string ModuleId { get; set; }

        public ModuleType ModuleType { get; set; }

        public string ModulePart { get; set; }

        public string ChartId { get; set; }

        public string SearchContext { get; set; }
    }

//    public enum ModuleType
//    {
//        CommunicatorAuthorActivitiesSummaryModule,
//
//        CommunicatorAuthorSummaryModule,
//
//        CommunicatorChartsModule,
//
//        CommunicatorNewAuthorAlertsModule,
//
//        CommunicatorNewsModule,
//
//        CommunicatorOutletSummaryModule,
//    }
}