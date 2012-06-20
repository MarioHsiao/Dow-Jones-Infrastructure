using DowJones.Ajax.Article;
using DowJones.Managers.Multimedia;

namespace DowJones.Articles
{
    public class MultimediaPlayerOptions
    {
        public int AvailableWidth { get; set; }
        public bool AutoPlay { get; set; }

        public string ControlBarPath { get; set; }

        public string PlayerPath { get; set; }

        public string SplashImagePath { get; set; }

        public string RTMPPluginPath { get; set; }
    }

    public class ArticleRendererRequest
    {
        public ArticleResultset ArticleResultset { get; set; }

        public MultimediaPackage MultimediaPackage { get; set; }

        public MultimediaPlayerOptions MultimediaPlayerOptions { get; set; }
    }
}


