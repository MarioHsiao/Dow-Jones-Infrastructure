using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.HeadlineList
{
    public class HeadlineListTokens : AbstractTokenBase
    {
        #region << Accessors >>

        public string preDupHeadlineTkn { get; set; }
        public string postDupHeadlineTkn { get; set; }
        public string noResultsTkn { get; set; }
        public string moreLikeThisTkn { get; set; }
        public string deleteTkn { get; set; }
        public string clipTkn { get; set; }
        public string analystTkn { get; set; }
        public string blogTkn { get; set; }
        public string newspaperTkn { get; set; }
        public string audioTkn { get; set; }
        public string videoTkn { get; set; }
        public string pdfTkn { get; set; }
        public string htmlTkn { get; set; }
        public string pictureTkn { get; set; }
        public string articleTkn { get; set; }
        public string rssTkn { get; set; }
        public string atomTkn { get; set; }
        public string multimediaTkn { get; set; }
        public string boardTkn { get; set; }
        public string internalTkn { get; set; }
        public string summaryTkn { get; set; }
        public string customerdocTkn { get; set; }
        public string fileTkn { get; set; }
        public string webpageTkn { get; set; }
        public string graphicTkn { get; set; }

        #endregion

        public HeadlineListTokens()
        {
            preDupHeadlineTkn = GetTokenByName("preDupHeadline");
            postDupHeadlineTkn = GetTokenByName("postDupHeadline");
            noResultsTkn = GetTokenByName("noResults");
            moreLikeThisTkn = GetTokenByName("moreLikeThis");
            deleteTkn = GetTokenByName("delete");
            clipTkn = GetTokenByName("clip");
            analystTkn = GetTokenByName("analyst");
            blogTkn = GetTokenByName("blog");
            newspaperTkn = GetTokenByName("newspaper");
            audioTkn = GetTokenByName("audio");
            videoTkn = GetTokenByName("video");
            pdfTkn = GetTokenByName("pdf");
            htmlTkn = GetTokenByName("html");
            pictureTkn = GetTokenByName("picture");
            articleTkn = GetTokenByName("article");
            rssTkn = GetTokenByName("rss");
            atomTkn = GetTokenByName("atom");
            multimediaTkn = GetTokenByName("multimedia");
            boardTkn = GetTokenByName("board");
            internalTkn = GetTokenByName("internal");
            summaryTkn = GetTokenByName("summary");
            customerdocTkn = GetTokenByName("customerDoc");
            fileTkn = GetTokenByName("html");
            webpageTkn = GetTokenByName("webPage");
            graphicTkn = GetTokenByName("graphic");
        }
    }
}