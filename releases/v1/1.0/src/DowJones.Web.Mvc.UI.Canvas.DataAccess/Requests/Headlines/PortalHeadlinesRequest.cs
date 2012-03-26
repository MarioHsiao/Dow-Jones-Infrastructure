using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Headlines
{
    public class HeadlinesRequest : IHeadlinesRequest
    {
        public HeadlinesRequest()
        {
            TruncationType = AbstractServiceResult.DefaultTruncationType;
        }

        public TruncationType TruncationType { get; set; }

        public string SearchContextRef { get; set; }

        public int FirstResultToReturn { get; set; }

        public int MaxResultsToReturn { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(SearchContextRef) &&
                   FirstResultToReturn >= 0 &&
                   MaxResultsToReturn >= 0;
        }
    }
}
