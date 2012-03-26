// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsstandNewsPageModuleServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Newsstand;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using DowJones.Web.Mvc.UI.Models.Common;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Newsstand.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Newtonsoft.Json;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using EntityType = DowJones.Tools.Ajax.HeadlineList.EntityType;
using NewsstandSection = DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.NewsstandSection;
using TruncationType = DowJones.Tools.Ajax.PortalHeadlineList.TruncationType;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    /// <summary>
    /// The newsstand news page module service result.
    /// </summary>
    [DataContract(Name = "newsstandNewsPageModuleServiceResult", Namespace = "")]
    public class NewsstandNewsPageModuleServiceResult : Generic.AbstractServiceResult<NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>, AbstractNewsstandPackage>, IPopulate<NewsstandNewsPageModuleDataRequest>
    {
        private const int PartResultTimeout = 2500;
        private const int MaxNumberofNewsstands = 12;
        private const int MaxNumberOfNewsstandsToDisplay = 3;
        private bool hasCacheBeenEnabled = Settings.Default.IncludeCacheKeyGeneration &&
                                                    Settings.Default.CacheNewsstandNewsPageModuleService;

        #region Implementation of IPopulate

        /// <summary>
        /// The populate.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="getRequest">The request.</param>
        /// <param name="preferences">The preferences.</param>
        public void Populate(ControlData controlData, NewsstandNewsPageModuleDataRequest getRequest, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        if (getRequest == null || !getRequest.IsValid())
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                        }

                        if (preferences == null)
                        {
                            preferences = GetPreferences(controlData);
                        }

                        hasCacheBeenEnabled = hasCacheBeenEnabled && getRequest.CacheState != CacheState.Off;
                        GetModuleData(controlData, getRequest, preferences);
                    },
                    preferences);
        }

        #endregion

        protected internal static List<Newsstand> GetNewsstandListByContentLanguages(NewsstandListCollection newsstandListCollection, IPreferences preferences)
        {
            var temp = new List<Newsstand>();

            // Handle the all condition
            if (preferences.ContentLanguages.Count == 0)
            {
                foreach (var sourceList in newsstandListCollection)
                {
                    temp.AddRange(sourceList.NewsstandCollection);
                }

                return temp;
            }

            foreach (var newsstandList in newsstandListCollection.Where(sourcelist => ValidateNewsstand(sourcelist.NewsstandQualifier, preferences)))
            {
                temp.AddRange(newsstandList.NewsstandCollection);
            }

            return temp;
        }

        protected internal static bool ValidateNewsstand(string newsstandLanguage, IPreferences preferences)
        {
            return preferences.ContentLanguages.Any(contentLanguage => contentLanguage.ToLowerInvariant() == newsstandLanguage.ToLowerInvariant());
        }

        /// <summary>
        /// Creates the get multiple newsstand section headlines request.
        /// </summary>
        /// <param name="sectionId">The section id.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResults">The max results.</param>
        /// <returns>A <see cref="GetMultipleNewsstandSectionHeadlinesRequest"/> object.</returns>
        protected internal static GetMultipleNewsstandSectionHeadlinesRequest CreateGetMultipleNewsstandSectionHeadlinesRequest(int sectionId, int firstResult, int maxResults)
        {
            return new GetMultipleNewsstandSectionHeadlinesRequest
            {
                CombineResults = false,
                SectionCollection = new NewsstandSectionCollection
                                               {
                                                   new Factiva.Gateway.Messages.Newsstand.V1_0.NewsstandSection
                                                       {
                                                           MaxResultsToReturn = maxResults,
                                                           SectionID = sectionId,
                                                           SearchRequest = GetSearchRequestForHeadlines(firstResult, maxResults)
                                                       }
                                               }
            };
        }

        /// <summary>
        /// Called from Discovered Entities search context
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="preferences">The preferences.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResults">The max results.</param>
        /// <returns>
        /// A <see cref="GetMultipleNewsstandSectionHeadlinesRequest"/> object.
        /// </returns>
        protected internal static GetMultipleNewsstandSectionHeadlinesRequest CreateGetMultipleNewsstandSectionHeadlinesRequest(NewsstandNewspageModule module, IPreferences preferences, int firstResult, int maxResults)
        {
            var newsstandList = GetNewsstandListByContentLanguages(module.NewsstandListCollection, preferences);
            return CreateGetMultipleNewsstandSectionHeadlinesRequest(preferences, newsstandList, firstResult, maxResults);
        }

        protected internal NewsstandNewspageModule GetModule(IModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var module = GetModule<NewsstandNewspageModule>(request, controlData, preferences);
            if (module.NewsstandListCollection.Count <= 0)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EmptyNewsstandListCollection);
            }

            return module;
        }

        protected internal void GetModuleData(ControlData controlData, NewsstandNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var module = GetModule(request, controlData, preferences);

            var newsstandList = GetNewsstandListByContentLanguages(module.NewsstandListCollection, preferences);
            
            if (newsstandList.Count <= 0)
            {
                return;
            }

            var newsstandCollectionForHeadlines = new NewsstandCollection();

            newsstandCollectionForHeadlines.AddRange(newsstandList.Take(MaxNumberOfNewsstandsToDisplay));

            var newsstandSectionsForCounts = new NewsstandCollection();
            newsstandSectionsForCounts.AddRange(newsstandList.Where(aNewsstand => (newsstandList.IndexOf(aNewsstand) > MaxNumberOfNewsstandsToDisplay - 1 && newsstandList.IndexOf(aNewsstand) < MaxNumberofNewsstands)));

            var uniqueParts = request.Parts.GetUniques();

            var tasks = new List<Task<NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>>>();

            foreach (var part in uniqueParts)
            {
                switch (part)
                {
                    case NewsstandPart.Headlines:
                        tasks.Add(TaskFactory.StartNew(() => GetNewsstandHeadlinesDataPartResult(newsstandCollectionForHeadlines, request, controlData, preferences), TaskCreationOptions.None));
                        break;
                    case NewsstandPart.Counts:
                        tasks.Add(TaskFactory.StartNew(() => GetTotalCountNewsstandHeadlinesPerNewsstandSection(newsstandSectionsForCounts, request, preferences), TaskCreationOptions.None));
                        break;
                    case NewsstandPart.DiscoveredEntities:
                        tasks.Add(TaskFactory.StartNew(() => GetNewsstandDiscoveryDataPartResult(newsstandList, request, preferences), TaskCreationOptions.None));
                        break;
                }
            }

            Task.WaitAll(tasks.ToArray(), PartResultTimeout);
            MaxPartsAvailable = 3;
            PartResults = tasks.Select(task => task.Result).ToList();
        }

        /// <summary>
        /// The get newsstand headlines data part result.
        /// </summary>
        /// <param name="newsstandList">The newsstand list.</param>
        /// <param name="request">The request.</param>
        /// <param name="controlData">The control data.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A newsstand part results.
        /// </returns>
        protected internal NewsstandNewsPageServicePartResult<AbstractNewsstandPackage> GetNewsstandHeadlinesDataPartResult(NewsstandCollection newsstandList, NewsstandNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            // newsstandPackage.NewsstandSections.Add(newsstandSectionHeadlines1);
            // for the first 6 do one each multi and get the headlines

            // get headlines
            var partResult = new NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>();
            ProcessServicePartResult<NewsstandHeadlinesPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    var sectionHeadlines = GetNewsstandHeadlines(newsstandList, request, controlData, preferences);
                    var newsstandPackage = new NewsstandHeadlinesPackage
                    {
                        NewsstandSections = sectionHeadlines
                    };
                    partResult.Package = newsstandPackage;
                },
                preferences);
            return partResult;
        }

        /// <summary>
        /// The get newsstand discovery data part result.
        /// </summary>
        /// <param name="newsstandList">The newsstand list.</param>
        /// <param name="request">The request.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A NewsstandNewsPageServicePartResult object.
        /// </returns>
        protected internal NewsstandNewsPageServicePartResult<AbstractNewsstandPackage> GetNewsstandDiscoveryDataPartResult(List<Newsstand> newsstandList, AbstractModuleGetRequest request, IPreferences preferences)
        {
            var partResult = new NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>();
            ProcessServicePartResult<NewsstandDiscoveredEntitiesPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    var multipleNewsstandSectionHeadlinesRequest = CreateGetMultipleNewsstandSectionHeadlinesRequest(preferences, newsstandList, 0, 0);
                    //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, Settings.Default.DataAccessProxyUser);
                    var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

                    if (hasCacheBeenEnabled)
                    {
                        var generator = new NewsstandDiscoveryCacheKeyGenerator(
                                            request.ModuleId,
                                            newsstandList,
                                            preferences.InterfaceLanguage)
                                            {
                                                CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                            };
                        lightweightProxy = generator.GetCacheControlData(lightweightProxy);
                    }

                    GetMultipleNewsstandSectionHeadlinesResponse response = null;
                    RecordTransaction(
                        typeof(GetMultipleNewsstandSectionHeadlinesResponse).FullName,
                        MethodBase.GetCurrentMethod().Name,
                        manager =>
                        {
                            response = manager.Invoke<GetMultipleNewsstandSectionHeadlinesResponse>(multipleNewsstandSectionHeadlinesRequest).ObjectResponse;
                        },
                        new ModuleDataRetrievalManager(lightweightProxy, preferences));

                    var newsstandPackage = new NewsstandDiscoveredEntitiesPackage
                                    {
                                        PortalHeadlinesAreAvailable = false,
                                        TopNewsVolumeEntities = new List<NewsEntity>()
                                    };

                    if (response != null &&
                        response.NewsstandSectionHeadlinesResultSet != null && 
                        response.NewsstandSectionHeadlinesResultSet.NewsstandSectionHeadlinesResultCollection != null)
                    {
                        foreach (var navigatorCollection in response.NewsstandSectionHeadlinesResultSet.NewsstandSectionHeadlinesResultCollection
                            .Where(newsstandSectionHeadlinesResult => (
                                newsstandSectionHeadlinesResult.SearchResponse != null && 
                                newsstandSectionHeadlinesResult.SearchResponse.ContentSearchResult != null) && 
                                newsstandSectionHeadlinesResult.SearchResponse.ContentSearchResult.CodeNavigatorSet != null)
                            .SelectMany(newsstandSectionHeadlinesResult => newsstandSectionHeadlinesResult.SearchResponse.ContentSearchResult.CodeNavigatorSet.NavigatorCollection))
                        {
                            EntityType entityType;
                            switch (navigatorCollection.Id.ToLower())
                            {
                                case "co":
                                    entityType = EntityType.Organization;
                                    break;
                                case "ns":
                                    entityType = EntityType.NewsSubject;
                                    break;
                                case "pe":
                                    entityType = EntityType.Person;
                                    break;
                                case "re":
                                    entityType = EntityType.Region;
                                    break;
                                case "in":
                                    entityType = EntityType.Industry;
                                    break;
                                default:
                                    continue;
                            }

                            foreach (var temp in navigatorCollection.BucketCollection.Select(entity => new NewsEntity
                                                                                                           {
                                                                                                               Code = entity.Id,
                                                                                                               Descriptor = entity.Value,
                                                                                                               CurrentTimeFrameNewsVolume = new WholeNumber(entity.HitCount),
                                                                                                               Type = entityType,
                                                                                                               SearchContextRef = GenerateSearchContextRef(request, entityType, entity.Id)
                                                                                                           }))
                            {
                                newsstandPackage.TopNewsVolumeEntities.Add(temp);
                            }
                        }
                    }

                    // Sort the end result
                    if (newsstandPackage.TopNewsVolumeEntities.Count > 0)
                    {
                        newsstandPackage.TopNewsVolumeEntities = newsstandPackage.TopNewsVolumeEntities.OrderBy(tempEntity => tempEntity.CurrentTimeFrameNewsVolume.Value).Reverse().ToList();
                    }

                    partResult.Package = newsstandPackage;
                },
                preferences);
            return partResult;
        }

        private static GetMultipleNewsstandSectionHeadlinesRequest CreateGetMultipleNewsstandSectionHeadlinesRequest(IPreferences preferences, IEnumerable<Newsstand> newsstandList, int firstResult, int maxResults)
        {
            var multipleNewsstandSectionHeadlinesRequest = new GetMultipleNewsstandSectionHeadlinesRequest
                                                               {
                                                                   CombineResults = true,
                                                                   SectionCollection = new NewsstandSectionCollection()
                                                               };

            foreach (var section in newsstandList)
            {
                multipleNewsstandSectionHeadlinesRequest.SectionCollection.Add(new Factiva.Gateway.Messages.Newsstand.V1_0.NewsstandSection
                                                                                   {
                                                                                       MaxResultsToReturn = maxResults,
                                                                                       SectionID = Convert.ToInt32(section.SectionID),
                                                                                       SearchRequest = GetSearchRequestForDiscovery(preferences, firstResult, maxResults)
                                                                                   });
            }

            return multipleNewsstandSectionHeadlinesRequest;
        }

        private static string GenerateSearchContextRef(IModuleRequest request, EntityType entityType, string code)
        {
            var searchContext = new NewsstandDiscoveredEntitiesSearchContext
            {
                EntityType = entityType,
                Code = code
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(NewsstandDiscoveredEntitiesSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        /// <summary>
        /// Gets the search request for headlines.
        /// </summary>
        /// <param name="firstToReturn">The first to return.</param>
        /// <param name="maxToReturn">The max to return.</param>
        /// <returns>A <see cref="PerformContentSearchRequest">PerformContentSearchRequest</see> object.</returns>
        private static PerformContentSearchRequest GetSearchRequestForHeadlines(int firstToReturn = 0, int maxToReturn = 0)
        {
            var searchRequest = new PerformContentSearchRequest();

            var query = new StructuredQuery
            {
                Dates = new Dates { All = true },
                SearchCollectionCollection = new SearchCollectionCollection
                                                                 {
                                                                     SearchCollection.Publications, SearchCollection.WebSites
                                                                 }
            };

            searchRequest.StructuredSearch = new StructuredSearch
            {
                Query = query
            };

            searchRequest.FirstResult = firstToReturn;
            searchRequest.MaxResults = maxToReturn;
            return searchRequest;
        }

        /// <summary>
        /// The get search request for discovery.
        /// </summary>
        /// <param name="preferences">The preferences.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResults">The max results.</param>
        /// <returns>
        /// A <see cref="PortalHeadlineListResultSet">PortalHeadlineListResultSet.</see>
        /// </returns>
        private static PerformContentSearchRequest GetSearchRequestForDiscovery(IPreferences preferences, int firstResult, int maxResults)
        {
            var searchRequest = new PerformContentSearchRequest();

            var query = new StructuredQuery
            {
                //// Dates = new Dates { All = true },
                SearchCollectionCollection = new SearchCollectionCollection
                                                 {
                                                     SearchCollection.Publications, 
                                                     SearchCollection.WebSites
                                                 }
            };

            searchRequest.StructuredSearch = new StructuredSearch
            {
                Query = query
            };

            searchRequest.DescriptorControl = new DescriptorControl
            {
                Language = preferences.InterfaceLanguage, // should be coming in from request
                Mode = DescriptorControlMode.All
            };

            searchRequest.NavigationControl = new NavigationControl
            {
                ReturnCollectionCounts = false,
                ReturnHeadlineCoding = false,
                CodeNavigatorControl = new CodeNavigatorControl(),
                TimeNavigatorMode = TimeNavigatorMode.None
            };

            searchRequest.NavigationControl.CodeNavigatorControl.CustomCollection.Add(new NavigatorControl
                                                                                            {
                                                                                            Id = "co",
                                                                                            MaxBuckets = 10,
                                                                                            MinBucketValue = 0,
                                                                                            });

            searchRequest.NavigationControl.CodeNavigatorControl.CustomCollection.Add(new NavigatorControl
                                                                                            {
                                                                                                Id = "ns",
                                                                                                MaxBuckets = 10,
                                                                                                MinBucketValue = 0,
                                                                                            }); 
            
            searchRequest.NavigationControl.CodeNavigatorControl.CustomCollection.Add(new NavigatorControl
                                                                                            {
                                                                                                Id = "in",
                                                                                                MaxBuckets = 10,
                                                                                                MinBucketValue = 0,
                                                                                            });

            searchRequest.FirstResult = firstResult;
            searchRequest.MaxResults = maxResults;
            return searchRequest;
        }

        /// <summary>
        /// The assemble headlines result set.
        /// </summary>
        /// <param name="performContentSearchResponse">The perform content search response.</param>
        /// <param name="truncationType">Type of the truncation.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        /// <returns>
        /// A <see cref="PortalHeadlineListResultSet">PortalHeadlineListResultSet.</see>
        /// </returns>
        private static PortalHeadlineListResultSet AssembleHeadlinesResultset(IPerformContentSearchResponse performContentSearchResponse, TruncationType truncationType, DateTimeFormatter dateTimeFormatter)
        {
            var conversionManager = new HeadlineListConversionManager(dateTimeFormatter);
            var headlineListDataResult = conversionManager.Process(performContentSearchResponse).Convert(truncationType);
            return headlineListDataResult.ResultSet;
        }

        private static NewsstandSection AssembleNewsstandHeadlines(IModuleRequest request, NewsstandSectionHeadlinesResult newsstandSectionsHeadlinesResult, Newsstand newsStandEntity, TruncationType truncationType, IPreferences preferences)
        {
            var newsstandSectionHeadlines = new NewsstandSection();
            var datetimeFormatter = new DateTimeFormatter(preferences);
            if (newsstandSectionsHeadlinesResult != null)
            {
                if (newsstandSectionsHeadlinesResult.Status == 0)
                {
                    var sectionId = newsstandSectionsHeadlinesResult.NewsstandSectionInfoCollection[0].SectionID;
                    newsstandSectionHeadlines.Status = 0;
                    newsstandSectionHeadlines.SectionId = sectionId.ToString();
                    newsstandSectionHeadlines.SectionTitle = newsstandSectionsHeadlinesResult.NewsstandSectionInfoCollection[0].SectionName;
                    newsstandSectionHeadlines.SourceCode = newsstandSectionsHeadlinesResult.NewsstandSectionInfoCollection[0].SourceCode;
                    newsstandSectionHeadlines.SourceTitle = newsstandSectionsHeadlinesResult.NewsstandSectionInfoCollection[0].SourceName;
                    newsstandSectionHeadlines.SourceLogoUrl = string.Concat(
                        Settings.Default.BaseSourceLogoRepositoryUrl,
                        newsstandSectionHeadlines.SourceCode,
                        Settings.Default.BaseSourceLogoRepositoryExtention);
                    newsstandSectionHeadlines.Result = new PortalHeadlineListDataResult();
                    if (newsstandSectionsHeadlinesResult.SearchResponse != null && newsstandSectionsHeadlinesResult.SearchResponse.ContentSearchResult != null)
                    {
                        if (newsstandSectionsHeadlinesResult.SearchResponse.ContentSearchResult.ContentHeadlineResultSet != null &&
                            newsstandSectionsHeadlinesResult.SearchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection != null &&
                            newsstandSectionsHeadlinesResult.SearchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection.Count > 0)
                        {
                            newsstandSectionHeadlines.SectionDate = newsstandSectionsHeadlinesResult.SearchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection.FirstOrDefault().PublicationDate;
                            newsstandSectionHeadlines.SectionDateDescriptor = datetimeFormatter.FormatLongDate((DateTime)newsstandSectionHeadlines.SectionDate);
                            newsstandSectionHeadlines.Result.HitCount = new WholeNumber(newsstandSectionsHeadlinesResult.SearchResponse.ContentSearchResult.HitCount);
                        }
                    }

                    newsstandSectionHeadlines.Result.ResultSet = AssembleHeadlinesResultset(newsstandSectionsHeadlinesResult.SearchResponse, truncationType, datetimeFormatter);
                    newsstandSectionHeadlines.ViewAllSearchContextRef = GenerateSearchContextRef(request, sectionId);
                }
                else
                {
                    newsstandSectionHeadlines.SectionId = newsStandEntity.SectionID;
                    newsstandSectionHeadlines.SourceCode = newsStandEntity.SourceID;
                    newsstandSectionHeadlines.Status = newsstandSectionsHeadlinesResult.Status;
                    newsstandSectionHeadlines.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(newsstandSectionHeadlines.Status.ToString());
                }
            }
            else
            {
                newsstandSectionHeadlines.Status = -1;
                newsstandSectionHeadlines.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(newsstandSectionHeadlines.Status.ToString());
            }

            return newsstandSectionHeadlines;
        }

        private static string GenerateSearchContextRef(IModuleRequest request, int sectionId)
        {
            var searchContext = new NewsstandSectionSearchContext
            {
                SectionId = sectionId
            };

            var searchContextWrapper = new SearchContextWrapper
            {
                PageId = request.PageId,
                ModuleId = request.ModuleId,
                SearchContextType = typeof(NewsstandSectionSearchContext).Name,
                Json = searchContext.ToString()
            };

            return JsonConvert.SerializeObject(searchContextWrapper);
        }

        /// <summary>
        /// Assembles the newsstand headlines hit count.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="newsstandHeadlinesResponse">The newsstand headlines response.</param>
        /// <returns>
        /// A list of NewsstandHeadlineHitCount objects
        /// </returns>
        private static List<NewsstandHeadlineHitCount> AssembleNewsstandHeadlinesHitCount(IModuleRequest request, GetMultipleNewsstandSectionHeadlinesResponse newsstandHeadlinesResponse)
        {
            var newsstandHeadlineHitCountsList = new List<NewsstandHeadlineHitCount>();
            if (newsstandHeadlinesResponse != null && newsstandHeadlinesResponse.NewsstandSectionHeadlinesResultSet != null && newsstandHeadlinesResponse.NewsstandSectionHeadlinesResultSet.Count > 0)
            {
                var newsstandSectionHeadlinesResultCollection = newsstandHeadlinesResponse.NewsstandSectionHeadlinesResultSet.NewsstandSectionHeadlinesResultCollection;
                foreach (var result in newsstandSectionHeadlinesResultCollection)
                {
                    var newsstandHeadlineHitCounts = new NewsstandHeadlineHitCount();
                    if (result.Status == 0)
                    {
                        if (result.NewsstandSectionInfoCollection.Count > 0)
                        {
                            var newsstandSectionInfo = result.NewsstandSectionInfoCollection[0];
                            newsstandHeadlineHitCounts.SectionId = newsstandSectionInfo.SectionID.ToString();
                            newsstandHeadlineHitCounts.SectionTitle = newsstandSectionInfo.SectionName;
                            newsstandHeadlineHitCounts.SourceCode = newsstandSectionInfo.SourceCode;
                            newsstandHeadlineHitCounts.SourceTitle = newsstandSectionInfo.SourceName;
                            newsstandHeadlineHitCounts.SearchContextRef = GenerateSearchContextRef(request, newsstandSectionInfo.SectionID);
                            if (result.SearchResponse != null &&
                                result.SearchResponse.ContentSearchResult != null &&
                                result.SearchResponse.ContentSearchResult.HitCount > 0)
                            {
                                newsstandHeadlineHitCounts.HitCount = result.SearchResponse.ContentSearchResult.HitCount;
                            }

                            newsstandHeadlineHitCountsList.Add(newsstandHeadlineHitCounts);
                        }
                    }
                    else if (result.NewsstandSectionInfoCollection.Count > 0)
                    {
                        var newsstandSectionInfo = result.NewsstandSectionInfoCollection[0];
                        newsstandHeadlineHitCounts.Status = result.Status;
                        newsstandHeadlineHitCounts.SectionId = newsstandSectionInfo.SectionID.ToString();
                    }
                }
            }

            return newsstandHeadlineHitCountsList;
        }

        private NewsstandSection GetNewsstandHeadlinesPerNewsstandSection(Newsstand newsstandEntity, NewsstandNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var sectionHeadlinesRequest =
                   CreateGetMultipleNewsstandSectionHeadlinesRequest(
                        Convert.ToInt32(newsstandEntity.SectionID), 
                        request.FirstResultToReturn, 
                        request.MaxResultsToReturn);

            GetMultipleNewsstandSectionHeadlinesResponse response = null;

            if (hasCacheBeenEnabled &&
                request.FirstResultToReturn == 0)
            {
                var generator = new NewsstandHeadlinesCacheKeyGenerator(
                                    request.ModuleId,
                                    newsstandEntity.SourceID,
                                    newsstandEntity.SectionID,
                                    request.MaxResultsToReturn)
                                    {
                                        CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                    };

                controlData = generator.GetCacheControlData(controlData);
            }
            
            RecordTransaction(
                typeof(GetMultipleNewsstandSectionHeadlinesResponse).FullName,
                MethodBase.GetCurrentMethod().Name,
                manager =>
                {
                    response = manager.Invoke<GetMultipleNewsstandSectionHeadlinesResponse>(sectionHeadlinesRequest).ObjectResponse;
                },
                new ModuleDataRetrievalManager(controlData, preferences));

            return AssembleNewsstandHeadlines(request, response.NewsstandSectionHeadlinesResultSet.NewsstandSectionHeadlinesResultCollection[0], newsstandEntity, request.TruncationType, preferences);
        }

        private List<NewsstandSection> GetNewsstandHeadlines(IEnumerable<Newsstand> newsstandCollectionForHeadlines, NewsstandNewsPageModuleDataRequest request, ControlData controlData, IPreferences preferences)
        {
            var tasks = newsstandCollectionForHeadlines.Select(newsstandEntity => TaskFactory.StartNew(() => GetNewsstandHeadlinesPerNewsstandSection(newsstandEntity, request, controlData, preferences), TaskCreationOptions.None)).ToList();

            Task.WaitAll(tasks.ToArray());
            tasks.OrderBy(task => task.Result.SectionId);

            return tasks.Select(task => task.Result).ToList();
        }

        /// <summary>
        /// The get total count newsstand headlines per newsstand section.
        /// </summary>
        /// <param name="newsstandList">The newsstand entities.</param>
        /// <param name="request">The request.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A Package part.
        /// </returns>
        private NewsstandNewsPageServicePartResult<AbstractNewsstandPackage> GetTotalCountNewsstandHeadlinesPerNewsstandSection(IEnumerable<Newsstand> newsstandList, NewsstandNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var partResult = new NewsstandNewsPageServicePartResult<AbstractNewsstandPackage>();
            ProcessServicePartResult<NewsstandHeadlineHitCountsPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                {
                    var newsstandHeadlineHitCountsPackage = new NewsstandHeadlineHitCountsPackage();
                    if (newsstandList != null && newsstandList.Count() > 0)
                    {
                        var multipleNewsstandSectionHeadlinesRequest = new GetMultipleNewsstandSectionHeadlinesRequest
                                          {
                                              CombineResults = false, 
                                              SectionCollection = new NewsstandSectionCollection()
                                          };
                        foreach (var section in newsstandList)
                        {
                            multipleNewsstandSectionHeadlinesRequest.SectionCollection.Add(new Factiva.Gateway.Messages.Newsstand.V1_0.NewsstandSection
                                                              {
                                                                  MaxResultsToReturn = 0, 
                                                                  SectionID = Convert.ToInt32(section.SectionID), 
                                                                  SearchRequest = GetSearchRequestForHeadlines()
                                                              });
                        }

                        //// var proxyControlData = ControlDataManager.UpdateProxyCredentials(dataRetrievalManager.ControlData, Settings.Default.DataAccessProxyUser);
                        var lightweightProxy = ControlDataManager.GetLightWeightUserControlData(ProxyUser);

                        if (hasCacheBeenEnabled)
                        {
                            var generator = new NewsstandCountsCacheKeyGenerator(
                                                request.ModuleId,
                                                newsstandList, 
                                                preferences.InterfaceLanguage)
                                                {
                                                    CacheForceCacheRefresh = request.CacheState == CacheState.ForceRefresh
                                                };
                            lightweightProxy = generator.GetCacheControlData(lightweightProxy);
                        }

                        GetMultipleNewsstandSectionHeadlinesResponse response = null;

                        RecordTransaction(
                            typeof(GetMultipleNewsstandSectionHeadlinesResponse).FullName,
                            MethodBase.GetCurrentMethod().Name,
                            manager =>
                            {
                                response = manager.Invoke<GetMultipleNewsstandSectionHeadlinesResponse>(multipleNewsstandSectionHeadlinesRequest).ObjectResponse;
                            },
                            new ModuleDataRetrievalManager(lightweightProxy, preferences));

                        if (response != null)
                        {
                            newsstandHeadlineHitCountsPackage.NewsstandHeadlineHitCounts = new List<NewsstandHeadlineHitCount>();
                            newsstandHeadlineHitCountsPackage.NewsstandHeadlineHitCounts = AssembleNewsstandHeadlinesHitCount(request, response);
                        }
                    }
                    
                    partResult.Package = newsstandHeadlineHitCountsPackage;
                },
                preferences);

            return partResult;
        }
        
/*
        /// <summary>
        /// The assemble newsstand headlines.
        /// </summary>
        /// <param name="newsstandHeadlinesResponse">
        /// The newsstand headlines response.
        /// </param>
        private List<NewsstandSection> AssembleNewsstandHeadlinesList(GetMultipleNewsstandSectionHeadlinesResponse newsstandHeadlinesResponse)
        {
            var newsstandSectionHeadlineslist = new List<NewsstandSection>();
            if (newsstandHeadlinesResponse != null && newsstandHeadlinesResponse.NewsstandSectionHeadlinesResultSet != null && newsstandHeadlinesResponse.NewsstandSectionHeadlinesResultSet.Count > 0)
            {
                var newsstandSectionHeadlinesResultCollection = newsstandHeadlinesResponse.NewsstandSectionHeadlinesResultSet.NewsstandSectionHeadlinesResultCollection;
                newsstandSectionHeadlineslist.AddRange(newsstandSectionHeadlinesResultCollection.Select(AssembleNewsstandHeadlines));
            }

            return newsstandSectionHeadlineslist;
        }*/
    }
}
