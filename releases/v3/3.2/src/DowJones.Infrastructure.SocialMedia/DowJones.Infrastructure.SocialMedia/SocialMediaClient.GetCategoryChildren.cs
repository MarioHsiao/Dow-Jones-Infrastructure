// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.GetCategoryChildren.cs" company="Dow Jones">
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
    /// </summary>
    public enum CategoryChildrenSortOrder
    {
        /// <summary>
        /// </summary>
        Top,

        /// <summary>
        /// </summary>
        New,
    }

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
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="page">The page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="curator">The curator.</param>
        /// <returns>
        /// A <see cref="GetCategoryChildrenResponse"/> object.
        /// </returns>
        public GetCategoryChildrenResponse GetCategoryChildren(string categoryPath, int resultsPerPage = 10, int page = 1, CategoryChildrenSortOrder sortOrder = CategoryChildrenSortOrder.Top, string curator = null)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(categoryPath));
            Contract.Requires<ArgumentOutOfRangeException>(page > 0 && page < 100);
            Contract.Requires<ArgumentOutOfRangeException>(resultsPerPage > 0);

            var request = this.GetCategoryChildrenRequest(categoryPath, resultsPerPage, page, sortOrder, curator);
            return TryGetSuliaResponseImplementation<GetCategoryChildrenResponse>(request);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the category users request.
        /// </summary>
        /// <param name="categoryPath">The category path.</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="page">The page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="curator">The curator.</param>
        /// <returns>
        /// A <see cref="RestRequest"/> object.
        /// </returns>
        private RestRequest GetCategoryChildrenRequest(string categoryPath, int resultsPerPage, int page, CategoryChildrenSortOrder sortOrder, string curator)
        {
           var request = new RestRequest
                {
                    Path = string.Format("categories/{0}/children.json", categoryPath), 
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