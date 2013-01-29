using DowJones.Web.Mvc.Search.Controllers;
using DowJones.Web.Mvc.UI.Components.SearchBuilder;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Web.Showcase.Controllers
{
    public class SearchBuilderController : SearchBuilderControllerBase
    {
        protected override Query GetTopicDetails(string topicId)
        {
            return new CommunicatorTopicQuery {Properties = new CommunicatorTopicQueryProperties()};
        }

        protected override void PopulateFromPreferences(SearchBuilderData sbData)
        {
        }
    }
}