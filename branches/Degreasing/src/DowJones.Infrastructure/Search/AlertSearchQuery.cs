using DowJones.Attributes;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using Factiva.Gateway.Messages.Assets.WebWidgets.V1_0_OLD;

namespace DowJones.Search
{
    public class AlertSearchQuery : AbstractBaseSearchQuery
    {
        public int AlertId { get; set; }

        //Search within alert reslut
        public string Keywords { get; set; }

        public AlertHeadlineViewType ViewType { get; set; }

        public SortOrder Sort { get; set; }

        public string Sessionmark { get; set; }

        public string Bookmark { get; set; }

        public bool ResetSessionmark { get; set; }

        public override bool IsValid()
        {
            return (AlertId > 0);
        }
    }

    public enum AlertHeadlineViewType
    {
        New,
        All,
        Session,
    }

//    public enum AlertResultSortBy
//    {
//        Unspecified,
//        
//        [AssignedToken("publicationsMostRecentRefresh")] 
//        PublicationDateMostRecentFirst,
//        
//       
//        [AssignedToken("publicationsOldestFirstRefresh")] 
//        PublicationDateOldestFirst,
//        
//        [AssignedToken("publicationsRelevanceRefresh")] 
//        Relevance,
//
//        [AssignedToken("arrivalTime")] 
//        ArrivalTime,
//        
//    }
}