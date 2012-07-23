using DowJones.Infrastructure;
using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.HeadlineList
{
    public class HeadlineListTokens : AbstractTokenBase
    {
        #region << Accessors >>

        public string PreDupHeadline { get; set; }
        public string PostDupHeadline { get; set; }
        public string NoResults { get; set; }
        public string MoreLikeThis { get; set; }
        public string Delete { get; set; }
        public string Clip { get; set; }
        public string Analyst { get; set; }
        public string Blog { get; set; }
        public string Newspaper { get; set; }
        public string Audio { get; set; }
        public string Video { get; set; }
        public string Pdf { get; set; }
        public string Html { get; set; }
        public string Picture { get; set; }
        public string Article { get; set; }
        public string Rss { get; set; }
        public string Atom { get; set; }
        public string Multimedia { get; set; }
        public string Board { get; set; }
        public string Internal { get; set; }
        public string Summary { get; set; }
        public string Customerdoc { get; set; }
        public string File { get; set; }
        public string Webpage { get; set; }
        public string Graphic { get; set; }

        #endregion

        public HeadlineListTokens()
        {
            PreDupHeadline = GetTokenByName("preDupHeadline");
            PostDupHeadline = GetTokenByName("postDupHeadline");
            NoResults = GetTokenByName("noResults");
            MoreLikeThis = GetTokenByName("moreLikeThis");
            Delete = GetTokenByName("delete");
            Clip = GetTokenByName("clip");
            Analyst = GetTokenByName("analyst");
            Blog = GetTokenByName("blog");
            Newspaper = GetTokenByName("newspaper");
            Audio = GetTokenByName("audio");
            Video = GetTokenByName("video");
            Pdf = GetTokenByName("pdf");
            Html = GetTokenByName("html");
            Picture = GetTokenByName("picture");
            Article = GetTokenByName("article");
            Rss = GetTokenByName("rss");
            Atom = GetTokenByName("atom");
            Multimedia = GetTokenByName("multimedia");
            Board = GetTokenByName("board");
            Internal = GetTokenByName("internal");
            Summary = GetTokenByName("summary");
            Customerdoc = GetTokenByName("customerDoc");
            File = GetTokenByName("html");
            Webpage = GetTokenByName("webPage");
            Graphic = GetTokenByName("graphic");
        }
    }
}