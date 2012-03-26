using Factiva.Gateway.Messages.Archive.V2_0;

namespace DowJones.Articles
{
    public interface IArticleService
    {
        // NOTE: This should not reference the Gateway directly, 
        //       but the object is so complex that we are making an exception
        Article GetArticle(string accessionNumber, string canonicalSearchString = null);

        string GetWebArticleUrl(string accessionNumber);

        string GetMultiMediaArticleUrl(string accessionNumber);

        ArticleResponseSet GetArticles(GetArticleRequest request);

        ArticleResponseSet GetArticles(MixedContentArticleRequest request);
    }
}