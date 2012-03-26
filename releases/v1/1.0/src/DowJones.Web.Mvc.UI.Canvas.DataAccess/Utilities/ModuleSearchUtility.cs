// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleSearchUtility.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Session;
using DowJones.Utilities.Managers.Search;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using FreePerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities
{
    /// <summary>
    /// Build all NewsPage modules search request
    /// </summary>
    internal class ModuleSearchUtility
    {
        private ModuleSearchUtility()
        {
        }

        /// <summary>
        /// Gets the search request.
        /// </summary>
        /// <typeparam name="TSearch">The type of the search.</typeparam>
        /// <param name="queryEntity">The query entity.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResults">The max results.</param>
        /// <param name="preferences">The preferences.</param>
        /// <param name="timeFrame">The time frame.</param>
        /// <returns>
        /// A Search Request.
        /// </returns>
        public static IPerformContentSearchRequest GetSearchRequest<TSearch>(QueryEntity queryEntity, int firstResult, int maxResults, IPreferences preferences = null, TimeFrame timeFrame = TimeFrame.LastMonth) where TSearch : IPerformContentSearchRequest, new()
        {
            var searchCollection = new SearchCollectionCollection
                                       {
                                           SearchCollection.Publications,
                                           SearchCollection.WebSites
                                       };

            var request = GetSearchRequest<TSearch>(
                queryEntity.QueryCollection, 
                firstResult, 
                maxResults, 
                MapSortOrder(queryEntity.ResultSortOrder),
                searchCollection,
                preferences,
                DeduplicationMode.NearExact, 
                timeFrame);
            request.StructuredSearch.Formatting.DeduplicationMode = MapDuplicationType(queryEntity.DeduplicationType);
            return request;
        }

        /// <summary>
        /// Gets the search request.
        /// </summary>
        /// <typeparam name="TSearch">The type of the search.</typeparam>
        /// <param name="queryCollection">The query collection.</param>
        /// <param name="firstResult">The first Result.</param>
        /// <param name="maxResult">The max result.</param>
        /// <param name="resultSortOrder">The result sort order.</param>
        /// <param name="searchCollectionCollection">The search collection collection.</param>
        /// <param name="preferences">The preferences.</param>
        /// <param name="deduplicationMode">The de-duplication mode.</param>
        /// <param name="timeFrame">The time frame.</param>
        /// <returns>
        /// A IPerformContentSearchRequest Object.
        /// </returns>
        public static IPerformContentSearchRequest GetSearchRequest<TSearch>(
            QueryCollection queryCollection,
            int firstResult, 
            int maxResult, 
            ResultSortOrder resultSortOrder, 
            SearchCollectionCollection searchCollectionCollection, 
            IPreferences preferences = null,
            DeduplicationMode deduplicationMode = DeduplicationMode.Similar, 
            TimeFrame timeFrame = TimeFrame.LastMonth) 
            where TSearch : IPerformContentSearchRequest, new()
        {
            return GetSearchRequest<TSearch>(MapSearchString(queryCollection), firstResult, maxResult,  resultSortOrder, searchCollectionCollection, deduplicationMode, timeFrame, preferences);
        }

        /// <summary>
        /// Gets the search request.
        /// </summary>
        /// <typeparam name="TSearch">The type of the search.</typeparam>
        /// <param name="traditionalQuery">The traditional query.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResult">The max result.</param>
        /// <param name="resultSortOrder">The result sort order.</param>
        /// <param name="searchCollectionCollection">The search collection collection.</param>
        /// <param name="preferences">The preferences.</param>
        /// <param name="deduplicationMode">The de-duplication mode.</param>
        /// <param name="timeframe">The timeframe.</param>
        /// <returns>
        /// A IPerformContentSearchRequest Object.
        /// </returns>
        public static IPerformContentSearchRequest GetSearchRequest<TSearch>(
            string traditionalQuery, 
            int firstResult, 
            int maxResult, 
            ResultSortOrder resultSortOrder, 
            SearchCollectionCollection searchCollectionCollection = null, 
            IPreferences preferences = null,
            DeduplicationMode deduplicationMode = DeduplicationMode.Similar,
            TimeFrame timeframe = TimeFrame.LastMonth) 
            where TSearch : IPerformContentSearchRequest, new()
        {
            var qs = new SearchStringCollection
                         {
                             new SearchString
                                 {
                                     Id = "traditionalQuery",
                                     Value = traditionalQuery,
                                     Type = SearchType.Free,
                                     Mode = SearchMode.Traditional,
                                     Combine = false,
                                     Filter = false,
                                     Validate = true
                                 }
                         };

            return GetSearchRequest<TSearch>(qs, firstResult, maxResult, resultSortOrder, searchCollectionCollection, deduplicationMode, timeframe, preferences);
        }

        /// <summary>
        /// Gets the search request.
        /// </summary>
        /// <typeparam name="TSearch">The type of the search.</typeparam>
        /// <param name="searchStringCollection">The search string collection.</param>
        /// <param name="firstResult">The start result.</param>
        /// <param name="maxResult">The max result.</param>
        /// <param name="resultSortOrder">The result sort order.</param>
        /// <param name="searchCollectionCollection">The search collection collection.</param>
        /// <param name="deduplicationMode">The de-duplication mode.</param>
        /// <param name="timeframe">The timeframe.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A Search Request.
        /// </returns>
        public static IPerformContentSearchRequest GetSearchRequest<TSearch>(
            SearchStringCollection searchStringCollection,
            int firstResult, 
            int maxResult, 
            ResultSortOrder resultSortOrder, 
            SearchCollectionCollection searchCollectionCollection, 
            DeduplicationMode deduplicationMode = DeduplicationMode.Similar,
            TimeFrame timeframe = TimeFrame.LastMonth,
            IPreferences preferences = null) 
            where TSearch : IPerformContentSearchRequest, new()
        {
            // Validate request

            // All Modules request should not have date range more than one year!
            switch (timeframe)
            {
                // case TimeFrame.LastDay:
                case TimeFrame.LastWeek:
                case TimeFrame.LastMonth:
                case TimeFrame.LastSixMonths:
                case TimeFrame.ThreeMonths:
                case TimeFrame.LastYear:
                    break;
                default:
                    timeframe = TimeFrame.LastYear;
                    break;
            }

            // Ensure max result
            if (maxResult < 0)
            {
                maxResult = 0;
            }

            // Ensure start result
            if (firstResult < 0)
            {
                firstResult = 0;
            }

            // Default content categories if it is not supplied
            if (searchCollectionCollection == null || searchCollectionCollection.Count == 0)
            {
                searchCollectionCollection = new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites };
            }
            
            var freeSearchRequest = new TSearch
                                        {
                                            FirstResult = firstResult,
                                            MaxResults = maxResult,
                                            DescriptorControl = new DescriptorControl
                                                                    {
                                                                        Mode = DescriptorControlMode.None
                                                                    },
                                            NavigationControl = new NavigationControl
                                                                    {
                                                                        ReturnCollectionCounts = false,
                                                                        ReturnHeadlineCoding = false,
                                                                        CodeNavigatorControl = new CodeNavigatorControl
                                                                                                   {
                                                                                                       Mode = CodeNavigatorMode.None
                                                                                                   },
                                                                        TimeNavigatorMode = TimeNavigatorMode.None
                                                                    },
                                            StructuredSearch = new StructuredSearch
                                                                   {
                                                                       Formatting = new ResultFormatting
                                                                                        {
                                                                                            ClusterMode = ClusterMode.Off,
                                                                                            DeduplicationMode = deduplicationMode,
                                                                                            FreshnessDate = DateTime.Now,
                                                                                            SortOrder = resultSortOrder
                                                                                        },
                                                                       Query = new StructuredQuery
                                                                                   {
                                                                                       SearchCollectionCollection = searchCollectionCollection,
                                                                                       SearchStringCollection = searchStringCollection,
                                                                                       Dates = new Dates
                                                                                                   {
                                                                                                       Format = DateFormat.MMDDCCYY,
                                                                                                       After = SearchManager.GetDate(timeframe)
                                                                                                   }
                                                                                   },
                                                                       Linguistics = new Linguistics
                                                                                         {
                                                                                             LemmatizationOn = false,
                                                                                             SpellCheckMode = LinguisticsMode.Off,
                                                                                             SymbolRecognitionMode = LinguisticsMode.Off
                                                                                         }
                                                                   }
                                        };
            if (preferences != null)
            {
                freeSearchRequest.StructuredSearch.Query.SearchStringCollection.Add(new SearchString
                {
                    Id = "la",
                    Scope = "la",
                    Type = SearchType.Controlled,
                    Filter = true,
                    Mode = SearchMode.Any,
                    Value = string.Join(" ", preferences.ContentLanguages)
                });
            }

            return freeSearchRequest;
        }

        #region <<<   PRIVATE STATIC METHODS   >>>

        protected internal static ResultSortOrder MapSortOrder(Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder resultSortOrder)
        {
            switch (resultSortOrder)
            {
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.ArrivalTime:
                    return ResultSortOrder.ArrivalTime;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.FIFO:
                    return ResultSortOrder.FIFO;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.FreshnessDateChronological:
                    return ResultSortOrder.FreshnessDateChronological;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.FreshnessDateReverseChronological:
                    return ResultSortOrder.FreshnessDateReverseChronological;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.LIFO:
                    return ResultSortOrder.LIFO;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.PublicationDateChronological:
                    return ResultSortOrder.PublicationDateChronological;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.PublicationDateReverseChronological:
                    return ResultSortOrder.PublicationDateReverseChronological;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.Relevance:
                    return ResultSortOrder.Relevance;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.RelevanceHighFreshness:
                    return ResultSortOrder.RelevanceHighFreshness;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.RelevanceMediumFreshness:
                    return ResultSortOrder.RelevanceMediumFreshness;
                default:
                    return ResultSortOrder.PublicationDateReverseChronological;
            }
        }

        protected internal static SearchString MapSearchString(Query q)
        {
            return new SearchString
            {
                Id = "Query_FreeType",
                Value = q.Text,
                Type = SearchType.Free,
                Mode = MapSearchMode(q.SearchMode),
                Combine = false,
                Filter = false,
                Validate = true
            };
        }

        protected internal static SearchMode MapSearchMode(Factiva.Gateway.Messages.Assets.Pages.V1_0.SearchMode searchMode)
        {
            switch (searchMode)
            {
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.SearchMode.Simple:
                    return SearchMode.Simple;
                default:
                    return SearchMode.Traditional;
            }
        }

        protected internal static DeduplicationMode MapDuplicationType(DeduplicationType deduplicationType)
        {
            switch (deduplicationType)
            {
                case DeduplicationType.Similar:
                    return DeduplicationMode.Similar;
                case DeduplicationType.VirtuallyIdentical:
                    return DeduplicationMode.NearExact;
                default:
                    return DeduplicationMode.Off;
            }
        }

        private static SearchStringCollection MapSearchString(IEnumerable<Query> queryCollection)
        {
            var stringCollection = new SearchStringCollection();
            foreach (var query in queryCollection)
            {
                stringCollection.Add(MapSearchString(query));
            }

            return stringCollection;
        }

        #endregion
    }
}