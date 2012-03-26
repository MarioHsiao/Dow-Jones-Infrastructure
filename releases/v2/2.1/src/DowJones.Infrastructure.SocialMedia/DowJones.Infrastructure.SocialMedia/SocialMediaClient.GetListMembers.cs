// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.GetListMembers.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.Contracts;
using Hammock;
using Hammock.Web;

namespace DowJones.Infrastructure.SocialMedia
{
    using System;
    using DowJones.SocialMedia.Responses;

    /// <summary>
    /// </summary>
    public partial class SocialMediaClient
    {
        #region Public Methods

        /// <summary>
        /// Searches for list.
        /// </summary>
        /// <param name="screenName">The screen Name.</param>
        /// <param name="slug">The slug.</param>
        /// <param name="resultsPerPage">The num of users per request.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// A <see cref="BaseListsResponse"/> object.
        /// </returns>
        public GetListMemebersResponse GetListMembers(string screenName, string slug, int resultsPerPage = 10, int page = 1)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(screenName));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(slug));
            Contract.Requires<ArgumentOutOfRangeException>(page > 0 && page < 100);
            Contract.Requires<ArgumentOutOfRangeException>(resultsPerPage > 0);

            var request = this.GetListMembersRequest(screenName, slug, resultsPerPage, page);
            return TryGetSuliaResponseImplementation<GetListMemebersResponse>(request);
        }

        /// <summary>
        /// The get search request.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="slug">The slug.</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// A <see cref="RestResponse"/> object.
        /// </returns>
        private RestRequest GetListMembersRequest(string screenName, string slug, int resultsPerPage, int page)
        {
            var request = new RestRequest
            {
                Path = string.Format("{0}/{1}/members.json", screenName, slug),
                Method = WebMethod.Get,
                Serializer = Serializer
            };

            request.AddParameter("rpp", resultsPerPage.ToString());
            request.AddParameter("page", page.ToString());

            this.SetSocialMediaMeta(request);
            return request;
        }

        #endregion    
    }
}