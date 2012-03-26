// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchManager.CodedNewsQuerySearch.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.CodedNews;
using Factiva.Gateway.Messages.CodedNews.CodedNewsQueries;
using Factiva.Gateway.Messages.CodedNews.Search;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Managers.Search
{
    /// <summary>
    /// Partial class for SearchManager
    /// </summary>
    public partial class SearchManager
    {
        /// <summary>
        /// Performs the coded structured search.
        /// </summary>
        /// <typeparam name="TIPerformContentSearchRequest">The type of the IPerformContentSearchRequest.</typeparam>
        /// <typeparam name="TIPerformContentSearchResponse">The type of the IPerformContentSearchResponse.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>
        /// An IPerformContentSearchResponse object.
        /// </returns>
        public IPerformContentSearchResponse PerformCodedStructuredSearch<TIPerformContentSearchRequest, TIPerformContentSearchResponse>(CodedStructuredSearchRequest request)
            where TIPerformContentSearchRequest : IPerformContentSearchRequest, new()
            where TIPerformContentSearchResponse : IPerformContentSearchResponse, new()
        {
            IPerformContentSearchResponse objPerformContentSearchResponse;
            if (request.IsValid())
            {
                var structuredQuery = StructuredQueryFactory.Create(request, ControlDataManager.Clone(ControlData, false));

                try
                {
                    var objPerformContentSearchRequest = GetPerformContentSearchRequest<TIPerformContentSearchRequest>(request, structuredQuery);

                    var temp = ControlDataManager.Clone(ControlData, true);
                    objPerformContentSearchResponse = GetPerformContentSearchResponse<TIPerformContentSearchResponse>(temp, objPerformContentSearchRequest);

                    var contentSearchResult = objPerformContentSearchResponse.ContentSearchResult;
                    if (contentSearchResult.HitCount < request.HitCountForExpandQuery)
                    {
                        if (structuredQuery.ExpandQuery != null)
                        {
                            objPerformContentSearchRequest.StructuredSearch.Query = structuredQuery.ExpandQuery; // Expand the query
                            objPerformContentSearchRequest.NavigationControl = null;
                            request.ReturnNavSet = false;
                            objPerformContentSearchResponse = GetPerformContentSearchResponse<TIPerformContentSearchResponse>(temp, objPerformContentSearchRequest);
                        }
                    }

                    return objPerformContentSearchResponse;
                }
                catch (DowJonesUtilitiesException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.SearchManagerUnableToFindCodedNewsQuery);
                }
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.SearchManagerInvalidDto);
        }

        public IPerformContentSearchRequest GetPerformContentSearchRequest<TIPerformContentSearchRequest>(CodedStructuredSearchRequest request, IStructuredQuery structuredQuery)
            where TIPerformContentSearchRequest : IPerformContentSearchRequest, new()
        {
            var objPerformContentSearchRequest = new TIPerformContentSearchRequest();
            var objStructuredSearch = new StructuredSearch
            {
                Query = structuredQuery.Query,
                Formatting = request.Formatting
            };
            objPerformContentSearchRequest.StructuredSearch = objStructuredSearch;
            objPerformContentSearchRequest.FirstResult = request.HeadlineStart;
            objPerformContentSearchRequest.MaxResults = request.HeadlineCount;
            objPerformContentSearchRequest.SearchContext = request.SearchContext;
            objPerformContentSearchRequest.DescriptorControl.Language = InterfaceLanguage;
            objPerformContentSearchRequest.DescriptorControl.Mode = DescriptorControlMode.All;

            if (request.KeywordBlackList != null)
            {
                objPerformContentSearchRequest.StructuredSearch.Formatting.KeywordFilter.BlackListTermCollection.AddRange(request.KeywordBlackList);
            }

            if (request.MetaDataController != null)
            {
                objPerformContentSearchRequest.NavigationControl = SearchUtility.GenerateNavigationControl(request.MetaDataController);
            }

            if (request.DateController != null)
            {
                objPerformContentSearchRequest.StructuredSearch.Query.Dates = SearchUtility.GetDates(request.DateController);
            }

            return objPerformContentSearchRequest;
        }
    }
}