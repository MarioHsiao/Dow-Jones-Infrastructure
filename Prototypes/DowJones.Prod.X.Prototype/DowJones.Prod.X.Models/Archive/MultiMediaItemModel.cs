using DowJones.Web.Mvc.UI.Components.VideoPlayer;

namespace DowJones.Prod.X.Models.Archive
{
    public class MultiMediaItemModel
    {
        public bool MustPlayFromSource { get; set; }
        public string ExternalUrl { get; set; }
        public VideoPlayerModel MultiMediaPlayerModel { get; set; }
    }
}
