using System.Collections.Generic;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Articles
{
    public class MixedContentArticleResponse
    {
        public IList<Article> Articles { get; set; }

        public IList<ContentHeadline> ContentHeadlines { get; set; }
    }
}