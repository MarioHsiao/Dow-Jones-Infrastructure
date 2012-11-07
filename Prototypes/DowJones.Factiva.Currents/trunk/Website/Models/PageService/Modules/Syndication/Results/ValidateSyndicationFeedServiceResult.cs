using DowJones.Pages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Syndication.Results
{
    public class ValidateSyndicationFeedServiceResult : AbstractServiceResult
    {
        public string FeedTitle { get; set; }
        public FeedType FeedType { get; set; }
    }
}
