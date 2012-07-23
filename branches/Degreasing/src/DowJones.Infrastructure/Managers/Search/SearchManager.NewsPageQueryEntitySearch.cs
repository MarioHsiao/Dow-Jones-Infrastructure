// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchManager.NewsPageQueryEntitySearch.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DowJones.Ajax.HeadlineList;
using DowJones.Managers.Search.Base;
using DowJones.Search.Attributes;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using ResultSortOrder = Factiva.Gateway.Messages.Search.V2_0.ResultSortOrder;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Managers.Search
{
    /// <summary>
    /// Partial class for QueryEntity Searching in NewsPages
    /// </summary>
    public partial class SearchManager
    {
        /// <summary>
        /// The get query entity search request.
        /// </summary>
        /// <param name="queryEntity">The query entity.</param>
        /// <param name="firstResult">The first result.</param>
        /// <param name="maxResults">The max results.</param>
        /// <param name="entitiesToReturn">The entities to return.</param>
        /// <param name="timeNavigatorMode">The time navigator mode.</param>
        /// <param name="timeframe">The timeframe.</param>
        /// <returns>
        /// An IPerformContentSearchRequest object.
        /// </returns>
        public static IPerformContentSearchRequest GetQueryEntitySearchRequest(QueryEntity queryEntity, int firstResult, int maxResults, List<EntityType> entitiesToReturn = null, TimeNavigatorMode timeNavigatorMode = TimeNavigatorMode.None, TimeFrame timeframe = TimeFrame.ThreeMonths)
        {
            var request = new PerformContentSearchRequest();

            foreach (var searchstring in queryEntity.QueryCollection.Select(query => new SearchString
                                                                                         {
                                                                                             Id = "phrase",
                                                                                             Value = query.Text,
                                                                                             Type = SearchType.Free,
                                                                                             Mode = MapSearchMode(query.SearchMode),
                                                                                             Combine = false,
                                                                                             Filter = false,
                                                                                             Validate = true
                                                                                         }))
            {
                request.StructuredSearch.Query.SearchStringCollection.Add(searchstring);
            }

            request.StructuredSearch.Query.SearchCollectionCollection.AddRange(new[] { SearchCollection.Publications, SearchCollection.WebSites });
            if (entitiesToReturn != null && entitiesToReturn.Count > 0)
            {
                request.DescriptorControl.Mode = DescriptorControlMode.All;
                request.NavigationControl = new NavigationControl
                                                {
                                                    ReturnCollectionCounts = false,
                                                    ReturnHeadlineCoding = false,
                                                    CodeNavigatorControl = new CodeNavigatorControl
                                                                               {
                                                                                   Mode = CodeNavigatorMode.None,
                                                                                   MinBucketValue = 0,
                                                                                   MaxBuckets = 0,
                                                                               }, TimeNavigatorMode = timeNavigatorMode,
                                                };
            }
            else
            {
                request.NavigationControl = new NavigationControl { TimeNavigatorMode = timeNavigatorMode };
            }

            request.FirstResult = firstResult;
            request.MaxResults = maxResults;

            if (queryEntity.DeduplicationType != DeduplicationType.NotApplicable)
            {
                request.StructuredSearch.Formatting.DeduplicationMode = (DeduplicationMode)Enum.Parse(typeof(DeduplicationMode), queryEntity.DeduplicationType.ToString(), true);
            }

            request.StructuredSearch.Formatting.ClusterMode = ClusterMode.Off;
            request.StructuredSearch.Formatting.FreshnessDate = DateTime.Now;
            
            if (queryEntity.ResultSortOrder == Factiva.Gateway.Messages.Assets.Pages.V1_0.ResultSortOrder.Unspecified)
            {
                request.StructuredSearch.Formatting.SortOrder = ResultSortOrder.PublicationDateReverseChronological;
            }
            else 
            {
                request.StructuredSearch.Formatting.SortOrder = (ResultSortOrder)Enum.Parse(typeof(ResultSortOrder), queryEntity.ResultSortOrder.ToString(), true);
            }

            request.StructuredSearch.Query.Dates.Format = DateFormat.DDMMCCYY;
            request.StructuredSearch.Query.Dates.After = GetDate(timeframe);

            return request;
        }

        /// <summary>
        /// The get date.
        /// </summary>
        /// <param name="timeframe">The timeframe.</param>
        /// <returns>
        /// The a string representing the date.
        /// </returns>
        public static string GetDate(TimeFrame timeframe)
        {
            return ((int)timeframe).ToString();
            //// var timeSlice = (TimeSlice)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod().GetParameters()[0], typeof(TimeSlice));
            // var timeSlice = (TimeSlice)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod().GetParameters().First(), typeof(TimeSlice));
            // return timeSlice != null ? timeSlice.Slice.ToString() : null;
        }

        /// <summary>
        /// The get date.
        /// </summary>
        /// <param name="timeframe">The timeframe.</param>
        /// <returns>
        /// The a string representing the date.
        /// </returns>
        protected internal static int GetTimeSlice(TimeFrame timeframe)
        {
            var timeSlice = (TimeSlice)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod().GetParameters().First(), typeof(TimeSlice));
            return timeSlice != null ? timeSlice.Slice : 0;
        }

        /// <summary>
        /// The map search mode.
        /// </summary>
        /// <param name="searchMode">The search mode.</param>
        /// <returns>
        /// A search mode object.
        /// </returns>
        public static SearchMode MapSearchMode(Factiva.Gateway.Messages.Assets.Pages.V1_0.SearchMode searchMode)
        {
            switch (searchMode)
            {
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.SearchMode.Simple:
                    return SearchMode.Simple;
                default:
                    return SearchMode.Traditional;
            }
        }
    }
}