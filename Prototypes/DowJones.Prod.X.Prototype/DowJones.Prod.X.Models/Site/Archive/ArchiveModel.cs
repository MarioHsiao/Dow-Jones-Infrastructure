using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.VideoPlayer;

namespace DowJones.Prod.X.Models.Site.Archive
{
    public class ArchiveModel
    {
        public ArticleModel ArticleModel { get; set; }
        public string ExternalItemUri { get; set; }
        public VideoPlayerModel MultiMediaPlayerModel { get; set; }
        public PortalHeadlineInfo Headline { get; set; }
    }
}
