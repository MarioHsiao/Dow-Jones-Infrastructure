// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.GetCategoryUsers.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The social media client.
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
    /// The social media client.
    /// </summary>
    public partial class SocialMediaClient
    {
        #region Public Methods

        /// <summary>
        /// Gets the category users.
        /// </summary>
        /// <param name="categoryPath">The category path.</param>
        /// <param name="resultsPerPage">The num Of Users Per Request.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// A <see cref="GetCategoryUsersResponse"/> object.
        /// </returns>
        public GetCategoryUsersResponse GetCategoryUsers(string categoryPath, int resultsPerPage = 10, int page = 1)
        {   
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(categoryPath));
            Contract.Requires<ArgumentOutOfRangeException>(page > 0 && page < 100);
            Contract.Requires<ArgumentOutOfRangeException>(resultsPerPage > 0);

            var request = this.GetCategoryUsersRequest(categoryPath, resultsPerPage, page);
            return TryGetSuliaResponseImplementation<GetCategoryUsersResponse>(request);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the category users request.
        /// </summary>
        /// <param name="categoryPath">The category path.</param>
        /// <param name="resultsPerPage">The results Per Page.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// A <see cref="RestRequest"/> object.
        /// </returns>
        private RestRequest GetCategoryUsersRequest(string categoryPath, int resultsPerPage, int page)
        {
            var request = new RestRequest
                {
                    Path = string.Format("categories/{0}/users.json", categoryPath), 
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