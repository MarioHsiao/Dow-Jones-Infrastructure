using Factiva.Gateway.Messages.Archive.V2_0;

namespace DowJones.Articles
{
    public interface IArticleService
    {
        Article GetArticle(string accessionNumber, string canonicalSearchString = null);

        Article GetDocument(string accessionNumber);

        string GetWebArticleUrl(string accessionNumber);

        string GetMultiMediaArticleUrl(string accessionNumber);

        ArticleResponseSet GetArticles(GetArticleRequest request);

        ArticleResponseSet GetArticles(MixedContentArticleRequest request);

        GetBinaryResponse GetBinary(GetBinaryRequest request) ;
    }
}