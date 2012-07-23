using System.Collections.Generic;
using Factiva.Gateway.Messages.Archive.V2_0;
using Newtonsoft.Json;

namespace DowJones.Articles
{
    public class MixedContentArticleRequest
    {
        public IList<ArticleReference> ArticleReferences { get; set; }

        public string CanonicalSearchString { get; set; }

        public string UsageAggregator { get; set; }

        public ResponseDataSet ResponseDataSet { get; set; }

        public DisplayFormat DisplayFormat { get; set; }
    }

    public enum DisplayFormat
    {
        Article,
        Headline
    }

    [JsonObject("articleReference")]
    public class ArticleReference
    {
        [JsonProperty("accessionNumber")]
        public string AccessionNumber { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }
}