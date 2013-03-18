using System.Collections.Generic;
using DowJones.Articles;
using DowJones.Web.Mvc.UI.Components.Article;
using Factiva.Gateway.Messages.Archive.V2_0;

namespace DowJones.Web.Mvc.Search.Requests.Article
{
    public class ArticlesRequest
    {
        public ArticlesRequest()
        {
            Options = DisplayOptions.Full;
            PictureSize = PictureSize.Small;
        }

        public string[] Ids { get; set; }

        public string[] Type { get; set; }

        public string Highlight { get; set; }

        public DisplayOptions Options { get; set; }

        public string Usage { get; set; }

        public PictureSize PictureSize { get; set; }


        public bool IsValid
        {
            get { return (Ids != null && Ids.Length > 0); }
        }

        public MixedContentArticleRequest MapToMixedContentArticleRequest()
        {
            var mixedContentArticleRequest = new MixedContentArticleRequest
                                                 {
                                                     ArticleReferences = new List<ArticleReference>(),
                                                     CanonicalSearchString = Highlight,
                                                     ResponseDataSet = new ResponseDataSet {articleFormat = Map(Options)},
                                                     UsageAggregator = Usage
                                                 };
            if (IsValid)
            {   
                for (var i = 0; i < Ids.Length; i++)
                {
                    mixedContentArticleRequest.ArticleReferences.Add(new ArticleReference
                                                                         {
                                                                             AccessionNumber = Ids[i],
                                                                             ContentType = Type[i]
                                                                         });
                }
            }
            return mixedContentArticleRequest;
        }

        private static ArticleFormatType Map(DisplayOptions options)
        {
            switch (options)
            {
                case DisplayOptions.Headline:
                    return ArticleFormatType.HLPI;
                case DisplayOptions.Indexing:
                    return ArticleFormatType.FULR;
                case DisplayOptions.Keywords:
                    return ArticleFormatType.KWIC;
                default:
                    return ArticleFormatType.FULL;
            }
        }
    }
}