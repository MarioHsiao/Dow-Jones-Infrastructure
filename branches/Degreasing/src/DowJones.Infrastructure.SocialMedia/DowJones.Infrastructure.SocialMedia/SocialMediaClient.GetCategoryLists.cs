// -----------------------------------------------------------------------
// <copyright file="GetCategoryLists.cs" company="Dow Jones & Co.">
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.Contracts;                    
using DowJones.SocialMedia.Responses;     
using Hammock;
using Hammock.Web;

namespace DowJones.Infrastructure.SocialMedia
{
    /// <summary>
    /// </summary>
    public enum ListSortOrder
    {
        /// <summary>
        /// Top Lists
        /// </summary>
        Top,

        /// <summary>
        ///  New Lists
        /// </summary>
        New,
    }

    /// <summary>
    /// </summary>
    public partial class SocialMediaClient
    {     
        #region Public Methods

        /// <summary>
        /// Gets the category lists.
        /// </summary>
        /// <param name="categoryPath">The category path.</param>
        /// <param name="resultsPerPage">The results Per Page.</param>
        /// <param name="page">The page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="curator">The curator.</param>
        /// <returns>
        /// A <see cref="GetCategorySuperListResponse"/>
        /// </returns>
        public GetCategoryListsResponse GetCategoryLists(string categoryPath, int resultsPerPage = 10, int page = 1, ListSortOrder sortOrder = ListSortOrder.Top, string curator = null)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(categoryPath));
            Contract.Requires<ArgumentOutOfRangeException>(page > 0);
            Contract.Requires<ArgumentOutOfRangeException>(resultsPerPage > 0);

            var request = this.GetCategoryListsRequest(categoryPath, resultsPerPage, page, sortOrder, curator);
            return TryGetSuliaResponseImplementation<GetCategoryListsResponse>(request);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the category super list request.
        /// </summary>
        /// <param name="categoryPath">The category path.</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="page">The page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="curator">The curator.</param>
        /// <returns>
        /// A <see cref="RestResponse"/> object.
        /// </returns>
        private RestRequest GetCategoryListsRequest(
            string categoryPath,
            int resultsPerPage = 10,
            int page = 1,
            ListSortOrder sortOrder = ListSortOrder.Top,
            string curator = null)
        {
            var request = new RestRequest
                {                                                                                     
                    Path = string.Format("categories/{0}/lists.json", categoryPath),
                    Method = WebMethod.Get,
                    Serializer = Serializer
                };

            request.AddParameter("rpp", resultsPerPage.ToString());
            request.AddParameter("page", page.ToString());
            request.AddParameter("sort", sortOrder.ToString().ToLower());

            if (!string.IsNullOrEmpty(curator))
            {
                request.AddParameter("curator", curator);
            }
            
            this.SetSocialMediaMeta(request);
            return request;
        }         

        #endregion
    }
}
