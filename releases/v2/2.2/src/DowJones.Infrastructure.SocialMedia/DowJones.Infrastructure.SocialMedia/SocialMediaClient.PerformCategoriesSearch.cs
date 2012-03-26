// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.PerformCategoriesSearch.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.Contracts;
using DowJones.SocialMedia.Responses;
using Hammock;
using Hammock.Web;

namespace DowJones.Infrastructure.SocialMedia
{
    using System;

    /// <summary>
    /// </summary>
    public partial class SocialMediaClient
    {
        #region Public Methods

        /// <summary>
        /// Searches for list.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="resultsPerPage">The num of categories per request.</param>
        /// <param name="page">The page.</param>
        /// <returns>A <see cref="BaseListsResponse"/> object.</returns>
        public PerformCategoriesSearchResponse PerformCategoriesSearch(string query, int resultsPerPage = 10, int page = 1)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(query));
            Contract.Requires<ArgumentOutOfRangeException>(page > 0 && page < 100);
            Contract.Requires<ArgumentOutOfRangeException>(resultsPerPage > 0);

            var request = this.GetCategoriesSearchRequest(query, resultsPerPage, page);
            
            var restResponse = this.client.Request(request);
            return TryGetSuliaResponseImplementation<PerformCategoriesSearchResponse>(request);
        }

        /// <summary>
        /// The get search request.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// A <see cref="RestRequest"/> object.
        /// </returns>
        private RestRequest GetCategoriesSearchRequest(string query, int resultsPerPage, int page)
        {    
            var request = new RestRequest
            {
                Path = "categories/search.json",
                Method = WebMethod.Get,
                Serializer = Serializer
            };

            request.AddParameter("q", query);
            request.AddParameter("rpp", resultsPerPage.ToString());
            request.AddParameter("page", page.ToString());

            this.SetSocialMediaMeta(request);
            return request;
        }

        #endregion                        
    }
}