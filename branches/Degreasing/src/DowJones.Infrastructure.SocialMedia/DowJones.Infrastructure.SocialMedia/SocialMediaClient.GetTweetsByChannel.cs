// -----------------------------------------------------------------------
// <copyright file="SocialMediaClient.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using DowJones.SocialMedia.Responses;
using Hammock;
using Hammock.Web;

namespace DowJones.Infrastructure.SocialMedia
{  
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class SocialMediaClient
    {
        /// <summary>
        /// Gets the tweets by category.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="count">The count.</param>
        /// <param name="sinceId">The since id.</param>
        /// <param name="maxId">The max id.</param>
        /// <param name="ignoreTweeters">The ignore tweeters.</param>
        /// <returns>
        /// A SocialMedia Response object.
        /// </returns>
        public GetTweetsByChannelResponse GetTweetsByChannel(string channel, int count = 200, long? sinceId = null, long? maxId = null, List<long> ignoreTweeters = null)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(channel));
            Contract.Requires<ArgumentOutOfRangeException>(count > 0);

            var request = this.GetTweetsByChannelRequest(channel, count, sinceId, maxId, ignoreTweeters);
            var response = TryGetSuliaResponseImplementation<GetTweetsByChannelResponse>(request);
            return response;
        }

        /// <summary>
        /// Gets the tweets by channel request.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="count">The count.</param>
        /// <param name="sinceId">The since id.</param>
        /// <param name="maxId">The max id.</param>
        /// <param name="ignoreTweeters">The ignore tweeters.</param>
        /// <returns>
        /// A Hammock RestRequest
        /// </returns>
        private RestRequest GetTweetsByChannelRequest(string channel, int count = 200, long? sinceId = null, long? maxId = null, List<long> ignoreTweeters = null)
        {
            if (channel.StartsWith("/"))
            {
                channel = channel.Substring(1);
            }

            var request = new RestRequest
                {
                    Path = string.Format("channels/{0}/tweets.json", channel),
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

            SetSocialMediaMeta(request);
            return request;
        }
    }
}
