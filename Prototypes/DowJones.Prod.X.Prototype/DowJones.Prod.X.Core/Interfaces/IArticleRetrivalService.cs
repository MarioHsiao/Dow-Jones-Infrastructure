using DowJones.Ajax.PortalArticle;
using DowJones.Managers.Abstract;
using DowJones.Prod.X.Core.Services.Archive;
using DowJones.Web.Mvc.UI.Components.Article;
using Factiva.Gateway.Messages.Archive.V2_0;

namespace DowJones.Prod.X.Core.Interfaces
{
    public interface IArticleRetrivalService : IService
    {
        PortalArticleResultSet GetPortalArticleResultSet(Article article, ConvertionOptions options);

        PortalArticleResultSet GetPortalArticleResultSet(string accessionNumber, ConvertionOptions options, string canonicalSearchString = null);

        ArticleModel GetArticleModel(Article article, ConvertionOptions options);

        ArticleModel GetArticleModel(string accessionNumber, ConvertionOptions options, string canonicalSearchString = null);

        string GetWebArticleUrl(string accessionNumber);

        string GetMultiMediaArticleUrl(string accessionNumber);

        GetBinaryResponse GetBinary(string accessionNumber, string reference, string imageType, string mimeType); 
    }
}