using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DowJones.Ajax.HeadlineList;
using DowJones.Models.Search;
using DowJones.Search;
using DowJones.Search.Navigation;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Managers.Search
{
    public interface IContentSearchService : ISearchService
    {
    }

    public class ContentSearchService : IContentSearchService
    {
        private readonly SearchManager _manager;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ContentSearchService));

        private readonly SearchQueryBuilder _queryBuilder;

        public ContentSearchService(SearchManager manager, SearchQueryBuilder queryBuilder)
        {
            _manager = manager;
            _queryBuilder = queryBuilder;
        }

        public SearchResponse PerformSearch(AbstractBaseSearchQuery query)
        {
            var searchQuery = query as AbstractSearchQuery;

            if (searchQuery == null)
            {
                return null;
            }

            SearchResponse response;

            if(searchQuery.IsValid())
            {
                var request = _queryBuilder.GetRequest<PerformContentSearchRequest>(searchQuery);
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("***** ContentSearchService::PerformContentSearchRequest *******");
                    Logger.Debug(GetFormattedXmlString(request));
                }

                response = GetResponse(request, searchQuery);
            }
            else
            {
                response = new SearchResponse() {Results = new HeadlineListDataResult()};
            }

            response.Query = searchQuery;

            return response;
        }

        private SearchResponse GetResponse(IPerformContentSearchRequest request, AbstractSearchQuery searchQuery)
        {
            var contentSearchResponse = _manager.PerformContentSearch<PerformContentSearchResponse>(request);

            var controlData = _manager.LastTransactionControlData;

            var contentSearchResult = contentSearchResponse.ContentSearchResult;

            
            var secondarySourceGroup = searchQuery.Filters.Source.Where(each => each.SourceType == SourceFilterType.ProductDefineCode).LastOrDefault();
            if (secondarySourceGroup != null)
            {
                searchQuery.SecondarySourceGroupId = secondarySourceGroup.SourceCode;
            }

            var productContentSearchResult = 
                new ProductContentSearchResult(searchQuery.ProductId, searchQuery.PrimarySourceGroupId, searchQuery.SecondarySourceGroupId)
                    {
                        ContentSearchResult = contentSearchResult,
                    };


            var histogram = Mapper.Map<Histogram>(contentSearchResult);

            var navigators = Mapper.Map<ResultNavigator>(productContentSearchResult);

            var recognizedEntities = Mapper.Map<RecognizedEntities>(contentSearchResult);

            var results = Mapper.Map<HeadlineListDataResult>(contentSearchResponse);


            return new SearchResponse
                       {
                           ContentServerAddress = controlData.ContentServerAddress,
                           ContextId = contentSearchResult.SearchContext,
                           Histogram = histogram,
                           Navigators = navigators,
                           RecognizedEntities = recognizedEntities,
                           Response = contentSearchResponse,
                           Results = results,
                       };
        }

        public static string GetFormattedXmlString(object anySerializable)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer ser = new XmlSerializer(anySerializable.GetType());
                XmlTextWriter w = new XmlTextWriter(sw);
                w.Formatting = Formatting.Indented;
                ser.Serialize(w, anySerializable);
                return sw.ToString();
            }
        }
    }
}