// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.GetListDetail.cs" company="Dow Jones">
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
        /// <returns>
        /// A <see cref="BaseListsResponse"/> object.
        /// </returns>
        public GetListDetailResponse GetListDetail(string screenName, string slug)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(screenName));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(slug));

            var request = this.GetListDetailRequest(screenName, slug); 
            return TryGetSuliaResponseImplementation<GetListDetailResponse>(request);
        }

        /// <summary>
        /// The get search request.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="slug">The slug.</param>
        /// <returns>
        /// A <see cref="RestResponse"/> object.
        /// </returns>
        private RestRequest GetListDetailRequest(string screenName, string slug)
        {
            var request = new RestRequest
            {
                Path = string.Format("{0}/{1}.json", screenName, slug),
                Method = WebMethod.Get,
                Serializer = Serializer
            };

            this.SetSocialMediaMeta(request);
            return request;
        }

        #endregion    
    }
}