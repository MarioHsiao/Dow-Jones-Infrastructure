// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchManager.PerformMoreLikeThisSearch.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the SearchManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers.Search.Requests;
using Factiva.Gateway.Messages.Search;

namespace DowJones.Utilities.Managers.Search
{
    /// <summary>
    /// Partial class for Search manager accession number searching.
    /// </summary>
    public partial class SearchManager
    {
        /// <summary>
        /// Gets the "More Like This" search.
        /// </summary>
        /// <typeparam name="TIPerformContentSearchRequest">The type of the I perform content search request.</typeparam>
        /// <param name="dto">The Data Transfer Object.</param>
        /// <returns>
        /// An IPerformContentSearchResponse.
        /// </returns>
        public IPerformContentSearchResponse PerformMoreLikeThisSearch<TIPerformContentSearchRequest, TIPerformContentSearchResponse>(MoreLikeThisSearchRequestDTO dto) 
            where TIPerformContentSearchRequest : IPerformContentSearchRequest, new()
            where TIPerformContentSearchResponse : IPerformContentSearchResponse, new()
        {
            if (dto == null)
            {
                throw new ArgumentNullException("dto");
            }

            if (dto.IsValid())
            {
                ushort currentContentServerAddress = 0;
                if (!dto.HasDocumentVector)
                {
                    var accNoSearchRequestDto = new AccessionNumberSearchRequestDTO
                                                    {
                                                        AccessionNumbers = new[] { dto.AccessionNumber }
                                                    };
                    var accResponse = GetPerformContentSearchResponse<TIPerformContentSearchRequest, TIPerformContentSearchResponse>(accNoSearchRequestDto);
                    if (accResponse != null && 
                        accResponse.ContentSearchResult != null &&
                        accResponse.ContentSearchResult.ContentHeadlineResultSet != null &&
                        accResponse.ContentSearchResult.ContentHeadlineResultSet.Count == 1)
                    {
                        dto.DocumentVector = accResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection[0].DocumentVector;
                        currentContentServerAddress = ContentServerAddress;
                    }
                    else
                    {
                        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.SearchManagerUnableToFindDocumentVector);
                    }
                }

                // Clone the control data and pass this one into the request. This will put you on the same search 2.0 server.
                var temp = ControlDataManager.Clone(ControlData, true);
                temp.ContentServerAddress = currentContentServerAddress;

                return GetPerformContentSearchResponse<TIPerformContentSearchRequest,TIPerformContentSearchRequest>(temp, dto);
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.SearchManagerInvalidDto);
        }

        /// <summary>
        /// Gets the "More Like This" search.
        /// </summary>
        /// <typeparam name="TIPerformContentSearchRequest">The type of the I perform content search request.</typeparam>
        /// <typeparam name="TIPerformContentSearchResponse"></typeparam>
        /// <param name="dto">The Data Transfer Object.</param>
        /// <param name="documentVector">The document vector.</param>
        /// <returns>A IPerformContentSearchResponse</returns>
        public IPerformContentSearchResponse PerformMoreLikeThisSearch<TIPerformContentSearchRequest, TIPerformContentSearchResponse>(MoreLikeThisSearchRequestDTO dto, out string documentVector)
            where TIPerformContentSearchRequest : IPerformContentSearchRequest, new()
            where TIPerformContentSearchResponse : IPerformContentSearchResponse, new()
        {
            var response = PerformMoreLikeThisSearch<TIPerformContentSearchRequest, TIPerformContentSearchResponse>(dto);
            documentVector = dto.DocumentVector;
            return response;
        }
    }
}
