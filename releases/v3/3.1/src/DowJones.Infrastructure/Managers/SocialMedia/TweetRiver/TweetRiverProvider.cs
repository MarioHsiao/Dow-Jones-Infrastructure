using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hammock;
using Hammock.Web;
using DowJones.Infrastructure;

namespace DowJones.Managers.SocialMedia.TweetRiver
{
    public class TweetRiverProvider : ISocialMediaProvider
    {
        #region Member Variables

        private const string DefaultServerToken = "643e1eae1f6405f2145fb1a0ca4bf838";
        private const string DefaultApiAuthority = "http://tweetriver.com/dowjonesmaster/";
        
        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public TweetRiverProvider(string serverToken = DefaultServerToken)
        {
            ServerToken = serverToken;
            Client = new RestClient { Authority = DefaultApiAuthority };
        }

        #endregion

        #region Properties

        #endregion

        #region ISocialMediaProvider Members

        public RestRequest GetSocialMediaRequest(string channel, RequestOptions requestOptions)
        {
            Guard.IsNotNullOrEmpty(channel, "channel");
            Guard.IsNotNull(requestOptions, "requestOptions");

            var request = new RestRequest
            {
                Path = string.Format("{0}.{1}", channel.TrimStart('/'), requestOptions.StreamFormat.ToString().ToLower()),
                Method = WebMethod.Get
            };

            request.AddParameter("limit", requestOptions.Limit.ToString());

            if (!string.IsNullOrEmpty(requestOptions.SinceId))
            {
                request.AddParameter("since_id", requestOptions.SinceId);
            }

            if (!string.IsNullOrEmpty(requestOptions.StartId))
            {
                request.AddParameter("start", requestOptions.StartId);
            }

            if (requestOptions.PageIndex.HasValue)
            {
                request.AddParameter("page", requestOptions.PageIndex.Value.ToString());
            }

            if (requestOptions.QueryType == QueryType.Experts)
            {
                request.AddParameter("source", "1");
            }

            SetSocialMediaMeta(request);
            return request;
        }

        /// <summary>
        /// The Client.
        /// </summary>
        public RestClient Client { get; private set; }


        /// <summary>
        /// Gets the API key associated with the Mass Relevance (TweetRiver) Service.
        /// </summary>
        /// <value>The server token.</value>
        public string ServerToken { get; private set; }

        /// <summary>
        ///   Gets or sets the REST API endpoint by specifying your own address, if necessary.
        /// </summary>
        public string Authority
        {
            get
            {
                return Client.Authority;
            }

            set
            {
                Client.Authority = value;
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// The set social media meta.
        /// </summary>
        /// <param name="request">The request.</param>
        private void SetSocialMediaMeta(RestBase request)
        {
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("User-Agent", "Dow Jones Factiva");
            request.AddParameter("app_key", ServerToken);
        }

        #endregion
    }
}
