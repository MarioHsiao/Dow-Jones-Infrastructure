using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Extensions;
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
            var account = Properties.SocialMedia.Default.Account;
            var endpoint = Properties.SocialMedia.Default.ApiAuthority ?? DefaultApiAuthority;
            Client = new RestClient
                         {
                             Authority = "{0}/{1}".FormatWith(endpoint, account)
                         };
        }

        #endregion

        #region Properties

        #endregion

        #region ISocialMediaProvider Members

        public RestRequest GetSocialMediaRequest(string channel, RequestOptions requestOptions)
        {
            Guard.IsNotNullOrEmpty(channel, "channel");
            Guard.IsNotNull(requestOptions, "requestOptions");
           

            // for tweets, would look like http://massrelevance.com/mr_dowjones/accounting-consulting.json?<query params>
            // for experts, would be http://massrelevance.com/mr_dowjones/accounting-consulting/meta.json?sources=1
            var stream = requestOptions.QueryType == QueryType.Experts ? "{0}/meta".FormatWith(channel) : channel;

            var request = new RestRequest
            {
                Path = "{0}.{1}".FormatWith(stream, requestOptions.StreamFormat.ToString().ToLower()),
                Method = WebMethod.Get
            };

            if (requestOptions.QueryType == QueryType.Tweets)
            {
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

            }
            else if (requestOptions.QueryType == QueryType.Experts)
            {
                request.AddParameter("sources", "1");
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
