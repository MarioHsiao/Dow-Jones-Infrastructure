using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Managers.SocialMedia
{
    public class RequestOptions
    {
        #region Member Variables

        private const int DefaultNumberOfTweets = 100;
        private const int DefaultPageIndex = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public RequestOptions()
        {
            StreamFormat = StreamFormat.Json;
            Limit = DefaultNumberOfTweets;
            QueryType = QueryType.Tweets;
        }

        #endregion

        #region Properties

        public string Channel { get; set; }
        public int Limit { get; set; }
        public StreamFormat StreamFormat { get; set; }
        public string SinceId { get; set; }
        public string StartId { get; set; }
        public int? PageIndex { get; set; }
        public QueryType QueryType { get; set; }
        #endregion
    }

    public enum QueryType
    {
        Tweets,
        Experts
    }
}
