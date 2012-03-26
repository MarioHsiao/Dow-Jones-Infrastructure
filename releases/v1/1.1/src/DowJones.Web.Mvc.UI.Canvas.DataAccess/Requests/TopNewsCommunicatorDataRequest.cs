using System.Collections.Generic;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests
{
    public enum TopNewsCommunicatorPart
    {
        BreakingNews,
        RecentHeadlines,
        Trending,
    }

    public class TopNewsCommunicatorDataRequest : AbstractModuleGetRequest
    {
        public List<TopNewsCommunicatorPart> Parts { get; set; }
        
        /// <summary>
        /// Gets or sets the type of the truncation.
        /// </summary>
        public TruncationType TruncationType { get; set; }
       
        /// <summary>
        /// Max number of headlines to return - for recent articles part
        /// </summary>
        public int MaxResultsToReturn { get; set; }

        /// <summary>
        /// Start index of the headlines to return - for recent articles part
        /// This is zero based.
        /// </summary>
        public int FirstResultToReturn { get; set; }

        /// <summary>
        /// Max  entities to return - for trending part
        /// </summary>
        public int MaxEntitiesToReturn { get; set; }

        public TopNewsCommunicatorDataRequest()
        {
            FirstResultToReturn = 0;
            MaxResultsToReturn = 5;
            MaxEntitiesToReturn = 10;
            TruncationType = AbstractServiceResult.DefaultTruncationType;
            Parts = new List<TopNewsCommunicatorPart>
                        {
                            TopNewsCommunicatorPart.RecentHeadlines,
                            TopNewsCommunicatorPart.BreakingNews,
                            TopNewsCommunicatorPart.Trending
                        };
        }

        public override bool IsValid()
        {
            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && Parts.Count > 0 && FirstResultToReturn >= 0 && MaxResultsToReturn >= 0 && MaxEntitiesToReturn > 0;
        }
    }
}
