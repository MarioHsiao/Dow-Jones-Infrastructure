// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopNewsNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Alerts;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Newtonsoft.Json;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using FreePerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreePerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "topNewsNewsPageModuleServiceResult", Namespace = "")]
    public class TopNewsNewsPageModuleServiceResult : Generic.AbstractServiceResult<TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>, AbstractTopNewsPackage>, IPopulate<TopNewsNewsPageModuleDataRequest> // , IUpdateDefinitionAndPopulate
    {
        private HeadlineListConversionManager conversionManager;
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration && Settings.Default.CacheTopNewsNewsPageModuleService;
        
        public void Populate(ControlData controlData, TopNewsNewsPageModuleDataRequest request, IPreferences preferences)
        {
            UpdateDefinitionAndPopulate(controlData, null, request, preferences);
        }

        #region Implementation of IUpdateDefinitionAndPopulate

        protected internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(TopNewsNewspageModule module, TopNewsModulePart modulePart, int firstResult, int maxResults, IPreferences preferences)
            where TSearch : IPerformContentSearchRequest, new()
        {
            var queryEntity = FindQueryEntity(module.QueryEntityCollection, MapModulePartToQueryEntityType(modulePart));
            return ModuleSearchUtility.GetSearchRequest<TSearch>(
                queryEntity.QueryCollection,
                                                                 firstResult,
                                                                 maxResults,
                                                                 ResultSortOrder.PublicationDateReverseChronological,
                                                                 GetSearchCollection(modulePart),
                                                                 preferences);
        }

        protected internal void UpdateDefinitionAndPopulate(ControlData controlData, IModuleUpdateRequest updateRequest, TopNewsNewsPageModuleDataRequest getRequest, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        if (updateRequest != null && updateRequest.IsValid())
                        {
                            // fire some type of update transaction
                        }

                        if (getRequest == null || !getRequest.IsValid())
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                        }

                        if (preferences == null)
                        {
                            preferences = GetPreferences(controlData);
                        }

                        hasCacheBeenEnabled = hasCacheBeenEnabled && getRequest.CacheState != CacheState.Off;
                        conversionManager = new HeadlineListConversionManager(new DateTimeFormatter(preferences));
                        GetModuleData(getRequest, controlData, preferences);
                    },
                    preferences);
        }

        #endregion

        protected internal void GetModuleData(TopNewsNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule(request, controlData, preferences);

            request.Parts = (List<TopNewsModulePart>)request.Parts.GetUniques();

            MaxPartsAvailable = 3;
            PartResults = GetQueryEntityResult(module.QueryEntityCollection, request, controlData, preferences);

            return;
        }

        protected internal TopNewsNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<TopNewsNewspageModule>(request, controlData, preferences);
            if (module.QueryEntityCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyQueryEntityCollection);
            }

            return module;
        }

        protected internal List<TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>> GetQueryEntityResult(QueryEntityCollection collection, TopNewsNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            if (request.Parts.Count() == 1)
            {
                return new List<TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>>
                           {
                               GetQueryEntityResult(
                                   request,
                                   FindQueryEntity(collection, MapModulePartToQueryEntityType(request.Parts.First())),
                                   controlData,
                                   preferences)
                           };
            }

            var tasks = request.Parts.Select(part1 => TaskFactory.StartNew(() => GetQueryEntityResult(request, FindQueryEntity(collection, MapModulePartToQueryEntityType(part1)), controlData, preferences), TaskCreationOptions.None)).ToList();
            Task.WaitAll(tasks.ToArray());
            return tasks.Select(task => task.Result).ToList();
        }

        #region <<<   STATIC METHODS   >>>
        private static QueryEntityType MapModulePartToQueryEntityType(TopNewsModulePart modulePart)
        {
            switch (modulePart)
            {
                case TopNewsModulePart.EditorsChoice:
                    return QueryEntityType.TopNewsEditorsChoice;
                case TopNewsModulePart.VideoAndAudio:
                    return QueryEntityType.TopNewsAudioAndVideo;
                case TopNewsModulePart.OpinionAndAnalysis:
                    return QueryEntityType.TopNewsOpinionAndAnalysis;
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidTopNewsPart);
        }

        /// <summary>
        /// Gets the search collection.
        /// </summary>
        /// <param name="dto">The Data Transfer object [i.e. dto].</param>
        /// <returns>A collection of SearchCollections</returns>
        private static SearchCollectionCollection GetSearchCollection(TopNewsModulePart dto)
        {
            var list = new SearchCollectionCollection();
            switch (dto)
            {
                case TopNewsModulePart.EditorsChoice:
                    list.Add(SearchCollection.Publications);
                    list.Add(SearchCollection.WebSites);
                    break;
                case TopNewsModulePart.OpinionAndAnalysis:
                    list.Add(SearchCollection.Publications);
                    list.Add(SearchCollection.WebSites);
                    break;
                case TopNewsModulePart.VideoAndAudio:
                    list.Add(SearchCollection.Multimedia);
                    break;
            }

            return list;
        }

        /// <summary>
        /// Maps the specified type.
        /// </summary>
        /// <param name="type">The query entity type.</param>
        /// <returns>A TopNewsModulePart enumeration.</returns>
        private static TopNewsModulePart Map(QueryEntityType type)
        {
            switch (type)
            {
                case QueryEntityType.TopNewsAudioAndVideo:
                    return TopNewsModulePart.VideoAndAudio;
                case QueryEntityType.TopNewsEditorsChoice:
                    return TopNewsModulePart.EditorsChoice;
                default:
                    return TopNewsModulePart.OpinionAndAnalysis;
            }
        }

        #endregion

        /// <summary>
        /// Finds the query entity.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>A QueryEntity object.</returns>
        private static QueryEntity FindQueryEntity(IEnumerable<QueryEntity> collection, QueryEntityType entityType)
        {
            return collection.FirstOrDefault(queryEntity => queryEntity.QueryEntityType == entityType);
        }

        private static string GetTitle(IEnumerable<Descriptor> descriptors, IPreferences preferences, QueryEntityType entityType)
        {
            if (descriptors == null || descriptors.Count() == 0)
            {
                // throw new ArgumentException(@"Descriptor collection is null or empty", "descriptors");
                return ResourceTextManager.Instance.GetString(entityType.ToString());
            }

            foreach (var descriptor in descriptors
                .Where(descriptor => descriptor.LangCode.ToLowerInvariant() == preferences.InterfaceLanguage.ToLowerInvariant()))
            {
                return descriptor.Value;
            }

            // no match return first on
            return descriptors.First().Value;
        }

        private static string GenerateSearchContextRef(IModuleRequest request, TopNewsModulePart part)
        {
            var searchContext = new TopNewsViewAllSearchContext
            {
                TopNewsModulePart = part
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(TopNewsViewAllSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        private TopNewsNewsPageServicePartResult<AbstractTopNewsPackage> GetQueryEntityResult(TopNewsNewsPageModuleDataRequest request, QueryEntity item, ControlData controlData, IPreferences preferences)
        {
            var partResult = new TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>();
            
            //// var methodName = MethodBase.GetCurrentMethod().Name;
            var partType = Map(item.QueryEntityType);
            //// partResult.Identifier = partType.ToString();
            switch (partType)
            {
                case TopNewsModulePart.EditorsChoice:
                    ProcessServicePartResult<TopNewsEditorsChoicePackage>(
                        MethodBase.GetCurrentMethod(),
                        partResult,
                        () =>
                            {
                                var title = GetTitle(item.Title.DescriptorCollection, preferences, item.QueryEntityType);
                                var topNewsEditorsChoicePackage = new TopNewsEditorsChoicePackage
                                                                      {
                                                                          Title = title,
                                                                          ViewAllSearchContextRef = GenerateSearchContextRef(request, partType),
                                                                      };
                                partResult.Package = topNewsEditorsChoicePackage;
                                topNewsEditorsChoicePackage.Result = GetHeadlines(partType, item, request, controlData, preferences);
                            },
                            preferences);
                    break;

                case TopNewsModulePart.OpinionAndAnalysis:
                    ProcessServicePartResult<TopNewsOpinionAndAnalysisPackage>(
                        MethodBase.GetCurrentMethod(),
                        partResult,
                        () =>
                            {
                                var title = GetTitle(item.Title.DescriptorCollection, preferences, item.QueryEntityType);
                                var topNewsOpinionAndAnalysisPackage = new TopNewsOpinionAndAnalysisPackage
                                                                       {
                                                                           Title = title,
                                                                           ViewAllSearchContextRef = GenerateSearchContextRef(request, partType),
                                                                       };
                                partResult.Package = topNewsOpinionAndAnalysisPackage;
                                topNewsOpinionAndAnalysisPackage.Result = GetHeadlines(partType, item, request, controlData, preferences);
                            },
                            preferences);
                    break;

                case TopNewsModulePart.VideoAndAudio:
                    ProcessServicePartResult<TopNewsVideoAndAudioPackage>(
                        MethodBase.GetCurrentMethod(),
                        partResult,
                        () =>
                            {
                                var title = GetTitle(item.Title.DescriptorCollection, preferences, item.QueryEntityType);
                                var topNewsVideoAndAudioPackage = new TopNewsVideoAndAudioPackage
                                                         {
                                                              Title = title,
                                                              ViewAllSearchContextRef = GenerateSearchContextRef(request, partType),
                                                         };
                                partResult.Package = topNewsVideoAndAudioPackage;
                                topNewsVideoAndAudioPackage.Result = GetHeadlines(partType, item, request, controlData, preferences);
                            },
                            preferences);
                    break;
            }

            return partResult;
        }
        
        private PortalHeadlineListDataResult GetHeadlines(TopNewsModulePart partType, QueryEntity queryEntity, TopNewsNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var response = GetSearchResponse(partType, queryEntity, request, controlData, preferences);
            return conversionManager.Process(response).Convert(request.TruncationType);
        }

        private IPerformContentSearchResponse GetSearchResponse(TopNewsModulePart partType, QueryEntity queryEntity, TopNewsNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            FreePerformContentSearchResponse response = null; 
            var freeSearchRequest = ModuleSearchUtility.GetSearchRequest<FreePerformContentSearchRequest>(
                queryEntity.QueryCollection, 
                request.FirstResultToReturn, 
                request.MaxResultsToReturn, 
                ModuleSearchUtility.MapSortOrder(queryEntity.ResultSortOrder), 
                GetSearchCollection(partType), 
                preferences,
                ModuleSearchUtility.MapDuplicationType(queryEntity.DeduplicationType));
            

            if (hasCacheBeenEnabled &&
                request.FirstResultToReturn == 0)
            {
                var generator = new TopNewsCacheKeyGenerator(
                    request.ModuleId, 
                    partType, 
                    request.MaxResultsToReturn, 
                    preferences.ContentLanguages.ToArray(), 
                    preferences.InterfaceLanguage)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };

                controlData = generator.GetCacheControlData(controlData);
            }

            RecordTransaction(
                typeof(FreePerformContentSearchResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                    {
                        response = manager.Invoke<FreePerformContentSearchResponse>(freeSearchRequest).ObjectResponse;
                    },
                new ModuleDataRetrievalManager(controlData, preferences));
            return response;
        }
    }
}
