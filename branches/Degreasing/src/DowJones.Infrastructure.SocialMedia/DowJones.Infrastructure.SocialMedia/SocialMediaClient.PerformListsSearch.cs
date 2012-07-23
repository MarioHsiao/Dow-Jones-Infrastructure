// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.PerformListsSearch.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
      
using System;
using System.Diagnostics.Contracts;
using DowJones.SocialMedia.Responses;
using Hammock;
using Hammock.Web;

namespace DowJones.Infrastructure.SocialMedia
{
    /// <summary>
    /// </summary>
    public partial class SocialMediaClient
    {
        #region Public Methods

        /// <summary>
        /// Searches for list.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="resultsPerPage">The num of lists per request.</param>
        /// <param name="page">The page.</param>
        /// <param name="curator">The curator.</param>
        /// <returns>A <see cref="BaseListsResponse"/> object.</returns>
        public PerformListsSearchResponse PerformListsSearch(string query, int resultsPerPage = 10, int page = 1, string curator = null)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(query));
            Contract.Requires<ArgumentOutOfRangeException>(page > 0 && page <= 100);
            Contract.Requires<ArgumentOutOfRangeException>(resultsPerPage > 0);

            var request = this.GetListsSearchRequest(query, resultsPerPage, page, curator);
            return TryGetSuliaResponseImplementation<PerformListsSearchResponse>(request);
        }

        /// <summary>
        /// The get search request.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="resultsPerPage">The num of lists per request.</param>
        /// <param name="page">The page.</param>
        /// <param name="curator">The curator.</param>
        /// <returns>
        /// A <see cref="RestResponse"/> object.
        /// </returns>
        private RestRequest GetListsSearchRequest(string query, int resultsPerPage, int page, string curator)
        {
            var request = new RestRequest
            {
                Path = "lists/search.json",
                Method = WebMethod.Get,
                Serializer = Serializer
            };

            request.AddParameter("q", query);
            request.AddParameter("rpp", resultsPerPage.ToString());
            request.AddParameter("page", page.ToString());

            if (!string.IsNullOrWhiteSpace(curator))
            {
                request.AddParameter("curator", curator);
            }

            this.SetSocialMediaMeta(request);
            return request;
        }

        #endregion    
    }
}