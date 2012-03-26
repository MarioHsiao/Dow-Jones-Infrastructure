// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.GetRecentLists.cs" company="Dow Jones">
//      2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The social media client.
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
    /// The social media client.
    /// </summary>
    public partial class SocialMediaClient
    {
        #region Public Methods

        /// <summary>
        /// Searches for list.
        /// </summary>
        /// <param name="resultsPerPage">
        /// The num of lists per request.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// A <see cref="BaseListsResponse"/> object.
        /// </returns>
        public GetRecentListsResponse GetRecentLists(int resultsPerPage = 10, int page = 1)
        {
            Contract.Requires<ArgumentOutOfRangeException>(page > 0 && page < 100);
            Contract.Requires<ArgumentOutOfRangeException>(resultsPerPage > 0);
            
            var request = this.GetRecentListsRequest(resultsPerPage, page);
            return TryGetSuliaResponseImplementation<GetRecentListsResponse>(request);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get search request.
        /// </summary>
        /// <param name="resultsPerPage">
        /// The results per page.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// A <see cref="RestResponse"/> object.
        /// </returns>
        private RestRequest GetRecentListsRequest(int resultsPerPage, int page)
        {
            var request = new RestRequest { Path = "lists/recent.json", Method = WebMethod.Get, Serializer = Serializer };

            request.AddParameter("rpp", resultsPerPage.ToString());
            request.AddParameter("page", page.ToString());

            this.SetSocialMediaMeta(request);
            return request;
        }

        #endregion
    }
}