// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.GetCategorySuperList.cs" company="">
//   
// </copyright>
// <summary>
//   The social media client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Infrastructure.SocialMedia
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using DowJones.SocialMedia.Responses;
    using Hammock;
    using Hammock.Web;

    /// <summary>
    /// The social media client.
    /// </summary>
    public partial class SocialMediaClient
    {
        #region Public Methods

        /// <summary>
        /// Gets the category super list.
        /// </summary>
        /// <param name="categoryPath">
        /// The category path.
        /// </param>
        /// <param name="showRelatedWords">
        /// if set to <c>true</c> [show related words].
        /// </param>
        /// <returns>
        /// A <see cref="GetCategorySuperListResponse"/>
        /// </returns>
        public GetCategorySuperListResponse GetCategorySuperList(string categoryPath, bool showRelatedWords = false)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(categoryPath));

            var request = this.GetCategorySuperListRequest(categoryPath, showRelatedWords);
            return TryGetSuliaResponseImplementation<GetCategorySuperListResponse>(request);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the category super list request.
        /// </summary>
        /// <param name="categoryPath">
        /// The category path.
        /// </param>
        /// <param name="showRelatedWords">
        /// if set to <c>true</c> [show related words].
        /// </param>
        /// <returns>
        /// A <see cref="RestResponse"/> object.
        /// </returns>
        private RestRequest GetCategorySuperListRequest(string categoryPath, bool showRelatedWords)
        {
            var request = new RestRequest { Path = string.Format("categories/{0}/superlist.json", categoryPath), Method = WebMethod.Get, Serializer = Serializer };
            request.AddParameter("show_related_words", showRelatedWords.ToString(CultureInfo.InvariantCulture).ToLower());

            this.SetSocialMediaMeta(request);
            return request;
        }



        #endregion
    }
}