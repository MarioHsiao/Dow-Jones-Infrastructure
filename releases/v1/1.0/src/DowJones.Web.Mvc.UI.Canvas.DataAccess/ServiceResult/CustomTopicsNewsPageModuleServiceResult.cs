// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomTopicsNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Tools.Ajax.Converters;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Alerts;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
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
using CustomTopicsNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.CustomTopicsNewspageModule;
using FreePerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreePerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "customTopicsNewsPageModuleServiceResult", Namespace = "")]
    public class CustomTopicsNewsPageModuleServiceResult : Generic.AbstractServiceResult<CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>, CustomTopicsPackage>, IPopulate<NewsPageHeadlineModuleDataRequest>
    {
        private HeadlineListConversionManager conversionManager;
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration &&
                                                    Settings.Default.CacheCustomTopicsNewsPageModuleService;


        public void Populate(ControlData controlData, NewsPageHeadlineModuleDataRequest request, IPreferences preferences)
        {
            UpdateDefinitionAndPopulate(controlData, null, request, preferences);
        }

        protected internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(CustomTopicsNewspageModule module, int customTopicIndex, string customTopicName, int firstResult, int maxResults, IPreferences preferences)
            where TSearch : IPerformContentSearchRequest, new()
        {
            // Get custom topic by index and validate it by name
            var customTopic = module.CustomTopicCollection[customTopicIndex];
            if (customTopic.Name != customTopicName)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidCustomTopic);
            }

            return ModuleSearchUtility.GetSearchRequest<TSearch>(customTopic.QueryEntity, firstResult, maxResults, preferences);
        }

        protected internal void UpdateDefinitionAndPopulate(ControlData controlData, IModuleUpdateRequest updateRequest, NewsPageHeadlineModuleDataRequest getRequest, IPreferences preferences)
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
        
        protected internal void GetModuleData(NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule(request, controlData, preferences);

            MaxPartsAvailable = module.CustomTopicCollection.Count;
            PartResults = GetQueryEntities(GetPage(module.CustomTopicCollection, request), request, controlData, preferences);
        }

        protected internal CustomTopicsNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<CustomTopicsNewspageModule>(request, controlData, preferences);
            if (module.CustomTopicCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyCustomTopicCollection);
            }

            return module;
        }

        protected internal CustomTopicsNewsPageServicePartResult<CustomTopicsPackage> ProcessQueryEntity(int id, CustomTopic entity, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var temp = ProcessQueryEntity(entity, id, request, controlData, preferences);
            temp.Identifier = id.ToString();
            return temp;
        }

        protected internal List<CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>> GetQueryEntities(IEnumerable<CustomTopic> customTopics, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            if (customTopics.Count() == 1)
            {
                return new List<CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>> { ProcessQueryEntity(request.FirstPartToReturn, customTopics.First(), request, controlData, preferences) };
            }

            var index = request.FirstPartToReturn - 1;

            var tasks = (from customTopic in customTopics
                         let identifier = Interlocked.Increment(ref index)
                         select TaskFactory.StartNew(
                            () => ProcessQueryEntity(identifier, customTopic, request, controlData, preferences), TaskCreationOptions.None)).ToList();

            Task.WaitAll(tasks.ToArray());
            var orderedTasks = tasks.OrderBy(task => Int32.Parse(task.Result.Identifier));
            return orderedTasks.Select(task => task.Result).ToList();
        }

        protected internal CustomTopicsNewsPageServicePartResult<CustomTopicsPackage> ProcessQueryEntity(CustomTopic topic, int topicIndex, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>();
            ProcessServicePartResult<CustomTopicsPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                    partResult.Package = new CustomTopicsPackage
                    {
                        Title = GetTitle(topic.QueryEntity.Title.DescriptorCollection, preferences, QueryEntityType.CustomTopic),
                        ViewAllSearchContextRef = GenerateSearchContextRef(request, topicIndex, topic.Name)
                    };
                    var searchResponse = PerformContentSearch(topic, request, topicIndex, partResult.Package.Title, controlData, preferences);
                    partResult.Package.Result = conversionManager.Process(searchResponse).Convert(request.TruncationType);
                },
                preferences);

            return partResult;
        }

        protected IPerformContentSearchResponse PerformContentSearch(CustomTopic customTopic, NewsPageHeadlineModuleDataRequest request, int topicIndex, string title, ControlData controlData, IPreferences preferences)
        {
            var performContentSearchRequest = ModuleSearchUtility.GetSearchRequest<FreePerformContentSearchRequest>(
                customTopic.QueryEntity.QueryCollection,
                request.FirstResultToReturn,
                request.MaxResultsToReturn,
                ModuleSearchUtility.MapSortOrder(customTopic.QueryEntity.ResultSortOrder),
                new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites },
                preferences,
                ModuleSearchUtility.MapDuplicationType(customTopic.QueryEntity.DeduplicationType));

            if (hasCacheBeenEnabled &&
                request.FirstResultToReturn == 0)
            {
                var generator = new CustomTopicsCacheKeyGenerator(request.ModuleId, topicIndex.ToString(), title, request.MaxResultsToReturn, preferences.ContentLanguages.ToArray())
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                controlData = generator.GetCacheControlData(controlData);
            }

            IPerformContentSearchResponse response = null;
            RecordTransaction(
                typeof(FreePerformContentSearchRequest).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<FreePerformContentSearchResponse>(performContentSearchRequest, controlData).ObjectResponse;
                },
                new ModuleDataRetrievalManager(controlData, preferences));
            return response;
        }
        
        private static string GenerateSearchContextRef(IModuleRequest request, int customTopicIndex, string customTopicName)
        {
            var searchContext = new CustomTopicsViewAllSearchContext
            {
                CustomTopicIndex = customTopicIndex,
                CustomTopicName = customTopicName
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(CustomTopicsViewAllSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        private static string GetTitle(IEnumerable<Descriptor> descriptors, IPreferences preferences, QueryEntityType entityType)
        {
            if (descriptors == null || descriptors.Count() == 0)
            {
                try
                {
                    return ResourceTextManager.Instance.GetString(entityType.ToString());
                }
                catch
                {
                    return string.Empty;
                }
            }

            foreach (var descriptor in descriptors.Where(descriptor => descriptor.LangCode.ToLowerInvariant() == preferences.InterfaceLanguage.ToLowerInvariant()))
            {
                return descriptor.Value;
            }

            // no match return first on
            return descriptors.First().Value;
        }
    }
}
