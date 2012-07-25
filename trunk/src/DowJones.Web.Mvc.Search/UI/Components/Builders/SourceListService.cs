// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceListService.cs" company="Dow Jones & Company">
//  Enterprise Media Group.
// </copyright>
// <summary>
//   The source list service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
                                       
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Managers.Search;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Components.Common;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Services.V1_0;
using DowJones.Web.Mvc.UI.Components.Search;

namespace DowJones.Web.Mvc.Search.UI.Components.Builders
{
    public class SourceCollection
    {
        public List<KeyValuePair<string, string>> TopLevelSourceGrouping { get; set; }
        public List<KeyValuePair<string, string>> SourceList { get; set; }
    }

    public interface ISourceListService
    {
        List<KeyValuePair<string, string>> GetTopLevelSourceGroupings(string assetId);

        List<KeyValuePair<string, string>> GetSourceLists();

        SourceCollection GetAllSources(string assetId);
    }

    /// <summary>
    /// The source list service.
    /// </summary>
    public class SourceListService : ISourceListService
    {
        /// <summary>
        /// The _configuration manager.
        /// </summary>
        private readonly ProductSourceGroupConfigurationManager _configurationManager;

        /// <summary>
        /// The _control data.
        /// </summary>
        private readonly IControlData _controlData;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceListService"/> class.
        /// </summary>
        /// <param name="controlData">
        /// The control data.
        /// </param>
        /// <param name="configurationManager">
        /// The configuration manager.
        /// </param>
        public SourceListService(IControlData controlData, ProductSourceGroupConfigurationManager configurationManager)
        {
            this._controlData = controlData;
            this._configurationManager = configurationManager;
        }      

        /// <summary>
        /// The get source list.
        /// </summary>
        /// <returns>
        /// A list of Code Desc
        /// </returns>
        public List<CodeDesc> GetSourceList()
        {
            var list = new List<CodeDesc>();
            var myList = this.GetSourceListsResponse();

            if (myList != null && myList.QueryDetailsItems != null)
            {
                list.AddRange(
                    from queryDetailsItem in myList.QueryDetailsItems
                    let a = queryDetailsItem.Query as SourceListQuery
                    select new CodeDesc
                               {
                                   Code = queryDetailsItem.Id.ToString(), 
                                   Desc = a.Properties.Name
                               });
            }

            return list;
        }

        /// <summary>
        /// The get top level source grouping.
        /// </summary>
        /// <param name="assetId">
        /// The asset id.
        /// </param>
        /// <returns>
        /// A list of 
        /// </returns>
        public List<CodeDesc> GetTopLevelSourceGrouping(string assetId)
        {
            var sourceGroups = this._configurationManager.SourceGroups(assetId);
            return sourceGroups.Select(sourceGroup =>
                                       new CodeDesc
                                           {
                                               Code = sourceGroup.PdfCode, 
                                               Desc = sourceGroup.Descriptor
                                           }).ToList();
        }

        public List<KeyValuePair<string, string>> GetTopLevelSourceGroupings(string assetId)
        {
            var sourceGroups = this._configurationManager.SourceGroups(assetId);
            return sourceGroups.Select(sourceGroup => new KeyValuePair<string, string>(sourceGroup.PdfCode, sourceGroup.Descriptor)).ToList();
        }

        public List<KeyValuePair<string, string>> GetSourceLists()
        {
            var list = new List<KeyValuePair<string, string>>();
            var myList = this.GetSourceListsResponse();

            if (myList != null && myList.QueryDetailsItems != null)
            {
                list.AddRange(
                    from queryDetailsItem in myList.QueryDetailsItems
                    let a = queryDetailsItem.Query as SourceListQuery
                    select new KeyValuePair<string, string>(queryDetailsItem.Id.ToString(), a.Properties.Name));
            }

            return list;                                        
        }

        private GetQueriesDetailsListResponse GetSourceListsResponse()
        {
            var listRequest = new GetQueriesDetailsListRequest
            {
                QueryTypes = new List<QueryType>
                                {
                                    QueryType.SourceListQuery
                                }
            };
            var response = QueryService.GetQueriesDetailsList(ControlDataManager.Convert(this._controlData), listRequest);
            return response.GetObject<GetQueriesDetailsListResponse>();
        }
        
        public SourceCollection GetAllSources(string assetId)
        {

            var sources = new SourceCollection();
            sources.TopLevelSourceGrouping = GetTopLevelSourceGroupings(assetId);
            sources.SourceList = GetSourceLists();
            
            return sources;
        }
    }
}