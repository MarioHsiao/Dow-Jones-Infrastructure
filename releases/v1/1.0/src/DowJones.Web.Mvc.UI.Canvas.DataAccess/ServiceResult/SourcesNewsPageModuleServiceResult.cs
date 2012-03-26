// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourcesNewsPageModuleServiceResult.cs" company="Dow Jones">
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
using System.Xml.Serialization;
using DowJones.Session;
using DowJones.Tools.Ajax.Converters;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Sources;
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
using Factiva.Gateway.Messages.Sources.V1_0;
using Newtonsoft.Json;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using FreePerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreePerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;
using SourceCollection = DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult.SourceCollection;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    /// <summary>
    /// Sources News page "Service Result"
    /// </summary>
    [DataContract(Name = "sourcesNewsPageModuleServiceResult", Namespace = "")]
    public class SourcesNewsPageModuleServiceResult : Generic.AbstractServiceResult<SourcesNewsPageServicePartResult<SourcePackage>, SourcePackage>, IPopulate<NewsPageHeadlineModuleDataRequest> ////, IUpdateDefinitionAndPopulate<,NewsPageHeadlineModuleDataRequest>
    {
        private HeadlineListConversionManager conversionManager;
        private SourceCollection sourceList = new SourceCollection();
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration &&
                                                   Settings.Default.CacheSourcesNewsPageModuleService;

        [DataMember(Name = "sources")]
        [XmlElement(ElementName = "sources")]
        public SourceCollection SourceList
        {
            get { return sourceList; }
            set { sourceList = value; }
        }

        public void Populate(ControlData controlData, NewsPageHeadlineModuleDataRequest request, IPreferences preferences)
        {
            UpdateDefinitionAndPopulate(controlData, null, request, preferences);
        }

        protected internal static SourceCollection GetSourceListByContentLanguages(SourceListCollection sourcesCollection, IPreferences preferences)
        {
            var temp = new SourceCollection();

            // Handle the all condition
            if (preferences.ContentLanguages.Count == 0)
            {
                foreach (var sourceList in sourcesCollection)
                {
                    temp.AddRange(sourceList.SourceCodes.SourceCodeCollection);
                }

                return temp;
            }

            foreach (var sourcelist in sourcesCollection.Where(sourcelist => ValidateSource(sourcelist.SourceListId, preferences)))
            {
                temp.AddRange(sourcelist.SourceCodes.SourceCodeCollection);
            }

            return temp;
        }

        protected internal static bool ValidateSource(string sourceLanguage, IPreferences preferences)
        {
            return preferences.ContentLanguages.Any(contentLanguage => contentLanguage.ToLowerInvariant() == sourceLanguage.ToLowerInvariant());
        }

        /// <summary>
        /// Creates the search request.
        /// </summary>
        /// <typeparam name="TSearch">The type of the search.</typeparam>
        /// <param name="sourceCode">The source code.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResults">The max results.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A <see cref="IPerformContentSearchRequest"/> object.
        /// </returns>
        protected internal static IPerformContentSearchRequest CreateSearchRequest<TSearch>(string sourceCode, int firstResult, int maxResults, IPreferences preferences)
           where TSearch : IPerformContentSearchRequest, new()
        {
            // GET ROOT QUERY
            var performContentSearchRequest = ModuleSearchUtility.GetSearchRequest<TSearch>(
                string.Concat("sc=", sourceCode),
                firstResult,
                maxResults,
                ResultSortOrder.PublicationDateReverseChronological,
                new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites, },
                preferences, 
                DeduplicationMode.Off, 
                TimeFrame.LastSixMonths);

            // APPLY SPECIFIC MODIFICATION/CHANGES
            performContentSearchRequest.DescriptorControl.Mode = DescriptorControlMode.All;
            performContentSearchRequest.DescriptorControl.Language = preferences.InterfaceLanguage;
            performContentSearchRequest.NavigationControl.CodeNavigatorControl.CustomCollection.Add(new NavigatorControl
            {
                Id = "sc",
                MaxBuckets = 1,
                MinBucketValue = 1,
            });
            return performContentSearchRequest;
        }

        protected internal IPerformContentSearchResponse PerformContentSearch(string sourceCode, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var performContentSearchRequest = CreateSearchRequest<FreePerformContentSearchRequest>(sourceCode, request.FirstResultToReturn, request.MaxResultsToReturn, preferences);
            
            if (hasCacheBeenEnabled &&
                request.FirstResultToReturn == 0)
            {
                var generator = new SourcesCacheKeyGenerator(request.ModuleId, sourceCode, request.MaxResultsToReturn, preferences.InterfaceLanguage)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };
                controlData = generator.GetCacheControlData(controlData);
            }

            IPerformContentSearchResponse response = null;
            RecordTransaction(
                typeof(FreePerformContentSearchResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<FreePerformContentSearchResponse>(performContentSearchRequest).ObjectResponse;
                }, 
                new ModuleDataRetrievalManager(controlData, preferences));
            return response;
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
                        Process(getRequest, controlData, preferences);
                    },
                    preferences);
        }

        protected internal void Process(NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule(request, controlData, preferences);

            SourceList = GetSourceListByContentLanguages(module.SourcesListCollection, preferences);
            MaxPartsAvailable = SourceList.Count;
            PartResults = GetSourceSearches(GetPage(SourceList, request), request, controlData, preferences);
            return;
        }

        protected internal SourcesNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<SourcesNewspageModule>(request, controlData, preferences);
            if (module.SourcesListCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptySourcesListCollection);
            }

            return module;
        }

        protected internal SourcesNewsPageServicePartResult<SourcePackage> ProcessSource(string id, string sourceCode, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var temp = ProcessSource(sourceCode, request, controlData, preferences);
            temp.Identifier = id;
            return temp;
        }

        protected internal SourcesNewsPageServicePartResult<SourcePackage> ProcessSource(string sourceCode, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var partResult = new SourcesNewsPageServicePartResult<SourcePackage>();
            ProcessServicePartResult<SourcePackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    partResult.Package = new SourcePackage
                    {
                        SourceCode = sourceCode,
                        ViewAllSearchContextRef = GenerateSearchContextRef(request, sourceCode)
                    };

                    var searchResponse = PerformContentSearch(sourceCode, request, controlData, preferences);
                    partResult.Package.Result = conversionManager.Process(searchResponse).Convert(request.TruncationType);
                    partResult.Package.SourceName = GetSourceName(searchResponse);
                    partResult.Package.SourceLogoUrl = string.Concat(
                       Settings.Default.BaseSourceLogoRepositoryUrl,
                       partResult.Package.SourceCode,
                       Settings.Default.BaseSourceLogoRepositoryExtention);
                    if (partResult.Package.SourceName.IsNullOrEmpty())
                    {
                        partResult.Package.SourceName = GetSourceName(sourceCode, controlData, preferences);
                    }
                },
                preferences);
            return partResult;
        }

        protected internal List<SourcesNewsPageServicePartResult<SourcePackage>> GetSourceSearches(IEnumerable<string> sourceCodes, NewsPageHeadlineModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            if (sourceCodes.Count() == 1)
            {
                return new List<SourcesNewsPageServicePartResult<SourcePackage>> { ProcessSource(request.FirstPartToReturn.ToString(), sourceCodes.First(), request, controlData, preferences) };
            }

            var index = request.FirstPartToReturn - 1;
            var tasks = (from sourceCode in sourceCodes
                         let identifier = Interlocked.Increment(ref index).ToString()
                         select TaskFactory.StartNew(
                            () => ProcessSource(identifier, sourceCode, request, controlData, preferences), TaskCreationOptions.None)).ToList();

            Task.WaitAll(tasks.ToArray());
            var orderedTasks = tasks.OrderBy(task => Int32.Parse(task.Result.Identifier));
            return orderedTasks.Select(task => task.Result).ToList();
        }

        private static string GetSourceName(IPerformContentSearchResponse performContentSearchResponse)
        {
            //Guard.IsNotNull(performContentSearchResponse, "performContentSearchResponse");

            if (performContentSearchResponse != null &&
                performContentSearchResponse.ContentSearchResult != null &&
                performContentSearchResponse.ContentSearchResult.CodeNavigatorSet != null &&
                performContentSearchResponse.ContentSearchResult.CodeNavigatorSet.Count == 1)
            {
                var navigator = performContentSearchResponse.ContentSearchResult.CodeNavigatorSet.NavigatorCollection[0];
                if (navigator.Id.ToLower() == "sc" && navigator.__bucketCollection.Count > 0)
                {
                    return navigator.BucketCollection[0].Value;
                }
            }
            return null;
        }

        private static string GenerateSearchContextRef(IModuleRequest request, string sourceCode)
        {
            var searchContext = new SourcesViewAllSearchContext
            {
                SourceCode = sourceCode
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(SourcesViewAllSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        private static string GetSourceName(string code, ControlData controlData, IPreferences preferences)
        {
            var request = new GetSourcesByCodeRequest
                                           {
                                               responseType = ResponseType.Brief, 
                                               responseTypeSpecified = true, 
                                               sourceCodes = new[] { code }, 
                                           };

            try
            {
                var dataRetrievalManager = new ModuleDataRetrievalManager(controlData, preferences);
                var sourcesResponse = dataRetrievalManager.Invoke<GetSourcesByCodeResponse>(request).ObjectResponse;
                if (sourcesResponse.sourcesByCodeResponse.sourcesResultSet.Count == 1)
                {
                    var result = sourcesResponse.sourcesByCodeResponse.sourcesResultSet[0] as SourceDoc;
                    if (result != null)
                    {
                        return result.listName;
                        //// return result.baseLanguage.ToLower() == InterfaceLanguage.ToLower() ? result.sourceNameNationalLanguage : result.sourceName;
                    }
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, -1);
            }

            return null;
        }
    }
}