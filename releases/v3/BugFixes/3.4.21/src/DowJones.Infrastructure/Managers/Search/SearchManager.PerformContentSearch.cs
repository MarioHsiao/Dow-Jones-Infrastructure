using System;
using DowJones.Exceptions;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.V1_0;

namespace DowJones.Managers.Search
{
    public partial class SearchManager
    {
        /// <summary>
        /// Performs the more like this search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <returns>
        /// A Perform Content Search Response
        /// </returns>
        public IPerformContentSearchResponse PerformContentSearch<T>(IPerformContentSearchRequest request) where T : IPerformContentSearchResponse, new()
        {
            try
            {
                var sr = Invoke<T>(request, ControlData);
                if (sr != null)
                {
                    return sr.ObjectResponse;
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }

            return null;
        }

        public ServiceResponse<PerformContentSearchResponse> PerformContentSearchAndReturnServiceResponse(PerformContentSearchRequest request)
        {
            try
            {
                var sr = Invoke<PerformContentSearchResponse>(request);
                if (sr != null)
                {
                    return sr;
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }

            return null;
        }
    }
}
