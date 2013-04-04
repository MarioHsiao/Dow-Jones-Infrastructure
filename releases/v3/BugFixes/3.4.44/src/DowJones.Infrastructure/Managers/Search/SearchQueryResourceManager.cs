using System.Collections.Generic;
using System.Linq;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;

namespace DowJones.Managers.Search
{
    public interface ISearchQueryResourceManager
    {
        IEnumerable<string> PrimarySourceTypesByProductId(string productId);
        IEnumerable<string> PrimarySourceTypesByProductDefineCode(string productId, string sourceGoupId);
        IEnumerable<Group> SourceList(string id);
    }

    public class SearchQueryResourceManager : ISearchQueryResourceManager
    {
        private ProductSourceGroupConfigurationManager _productSourceGroup;
        private readonly IControlData _controlData;

        public SearchQueryResourceManager(IControlData controlData, ProductSourceGroupConfigurationManager productSourceGroup)
        {
            _controlData = controlData;
            _productSourceGroup = productSourceGroup;
        }

        public IEnumerable<string> PrimarySourceTypesByProductId(string productId)
        {
           return _productSourceGroup.PrimarySourceTypes(productId);
        }

        public IEnumerable<string> PrimarySourceTypesByProductDefineCode(string productId, string sourceGoupId)
        {
            return _productSourceGroup.PrimarySourceTypes(productId, sourceGoupId);
        }

        public IEnumerable<Group> SourceList(string id)
        {
            var byIDRequest = new GetQueryByIDRequest {Id = long.Parse(id)};
            var serviceResponse = QueryService.GetQueryByID(ControlDataManager.Convert(_controlData), byIDRequest);
            object objResponse;
            serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);
            var objRespone = objResponse as GetQueryByIDResponse;
            return objRespone != null && objRespone.Query != null ? objRespone.Query.Groups : Enumerable.Empty<Group>();
        }
    }
}