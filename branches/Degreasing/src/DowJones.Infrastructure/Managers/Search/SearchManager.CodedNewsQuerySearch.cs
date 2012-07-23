// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchManager.CodedNewsQuerySearch.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Exceptions;
using DowJones.Managers.Search.CodedNewsQueries.Code;
using DowJones.Managers.Search.Requests;
using DowJones.Search.Core;
using DowJones.Session;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search
{
    /// <summary>
    /// Partial class for SearchManager
    /// </summary>
    public partial class SearchManager
    {
        public IPerformContentSearchResponse PerformCodedStructuredSearch<TIPerformContentSearchRequest, TIPerformContentSearchResponse>(CodedStructuredSearchRequest request)
            where TIPerformContentSearchRequest : IPerformContentSearchRequest, new()
            where TIPerformContentSearchResponse : IPerformContentSearchResponse, new()
        {
            if (request.IsValid())
            {
                var structuredQuery = new StructuredQueryFactory().Create(request);
                structuredQuery.ControlData = ControlDataManager.Clone(ControlData);

                try
                {
                    var objPerformContentSearchRequest = GetPerformContentSearchRequest<TIPerformContentSearchRequest>(request, structuredQuery);

                    var temp = ControlDataManager.Clone(ControlData, true);
                    var objPerformContentSearchResponse = GetPerformContentSearchResponse<TIPerformContentSearchResponse>(temp, objPerformContentSearchRequest);

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
            var objPerformContentSearchRequest = new TIPerformContentSearchRequest
            {
                StructuredSearch = new StructuredSearch
                                    {
                                        Query = structuredQuery.Query,
                                        Formatting = request.Formatting
                                    },
                FirstResult = request.HeadlineStart,
                MaxResults = request.HeadlineCount,
                SearchContext = request.SearchContext,
                DescriptorControl =
                    {
                        Language = _preferences.InterfaceLanguage,
                        Mode = DescriptorControlMode.All
                    }
            };

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