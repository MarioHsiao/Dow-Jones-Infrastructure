using DowJones.Ajax.PortalHeadlineList;
using DowJones.Managers.Abstract;
using DowJones.Prod.X.Core.DataTransferObjects;
using DowJones.Prod.X.Models.Search;
using Factiva.Gateway.Messages.Search;

namespace DowJones.Prod.X.Core.Interfaces
{
    public interface ISearchService
    {
        PortalHeadlineListDataResult GetPortalHeadlineListDataResult<TRequest, TResponse>(IPerformContentSearchResponse response, int startIndex = 0)
            where TRequest : IPerformContentSearchRequest, new()
            where TResponse : IPerformContentSearchResponse, new();

        PortalHeadlineListDataResult GetPortalHeadlineListDataResult<TRequest, TResponse>(string[] accessionNumbers, SortOrder order)
            where TRequest : IPerformContentSearchRequest, new()
            where TResponse : IPerformContentSearchResponse, new();

        TRequest BuildPerformContentSearchRequest<TRequest>(SearchServiceDTO searchDTO)
            where TRequest : IPerformContentSearchRequest, new();

        IPerformContentSearchResponse PerformSearch<TRequest, TResponse>(TRequest request)
            where TRequest : IPerformContentSearchRequest, new()
            where TResponse : IPerformContentSearchResponse, new(); 
    }
}