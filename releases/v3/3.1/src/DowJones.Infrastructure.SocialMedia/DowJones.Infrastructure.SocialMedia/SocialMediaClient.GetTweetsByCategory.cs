// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.GetTweetsByCategory.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>           
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        /// Gets the tweets.
        /// </summary>
        /// <param name="slug">The slug or channel listing eg. [all/sports/baseball].</param>
        /// <param name="count">The count.</param>
        /// <param name="sinceId">The since id.</param>
        /// <param name="maxId">The max_id.</param>
        /// <param name="ignoreTweeters">The ignore tweeters.</param>
        /// <returns>
        /// A <see cref="GetTweetsByCategoryResponse"/> object.
        /// </returns>
        public GetTweetsByCategoryResponse GetTweetsByCategory(string slug, int count = 200, long? sinceId = null, long? maxId = null, List<long> ignoreTweeters = null)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(slug));
            Contract.Requires<ArgumentOutOfRangeException>(count > 0);

            var request = this.GetTweetsByCategoryRequest(slug, count, sinceId, maxId, ignoreTweeters);
            var response = TryGetSuliaResponseImplementation<GetTweetsByCategoryResponse>(request);
            return response;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get tweets by channel request.
        /// </summary>
        /// <param name="slug">The slug or channel name.</param>
        /// <param name="count">The count.</param>
        /// <param name="sinceId">The since id.</param>
        /// <param name="maxId">The max id.</param>
        /// <param name="ignoreTweeters">The ignore tweeters.</param>
        /// <returns>
        /// A <see cref="RestResponse"/> object.
        /// </returns>
        private RestRequest GetTweetsByCategoryRequest(string slug, int count = 200, long? sinceId = null, long? maxId = null, List<long> ignoreTweeters = null)
        {
            var request = new RestRequest
            {
                    Path = string.Format("categories/{0}/tweets.json", slug), 
                    Method = WebMethod.Get, 
                    Serializer = Serializer
                };

            if (count != 200)
            {
                request.AddParameter("count", count.ToString());
            }

            if (sinceId != null && sinceId != 0)
            {
                request.AddParameter("since_id", sinceId.ToString());
            }

            if (maxId != null && maxId != 0)
            {
                request.AddParameter("max_id", maxId.ToString());
            }

            if (ignoreTweeters != null && ignoreTweeters.Count > 0)
            {
                var temp = ignoreTweeters.ConvertAll(i => i.ToString());
                request.AddParameter("ignore_tweeters", string.Join(",", temp));
            }

            this.SetSocialMediaMeta(request);
            return request;
        }

        #endregion
    }
}