using DowJones.Ajax.SocialMedia;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace DowJones.Web.Mvc.UI.Components.SocialMedia
{
    public class TweetLinesModel : ViewComponentModel
    {
        private const uint DefaultMaxTweets = 100;

        [ClientProperty("maxTweetsToShow")]
        public uint MaxTweetsToShow { get; set; }

        /// <summary>
        /// No. of times you can click 'load more' before it is disabled.
        /// </summary>
        /// <remarks>
        /// -1 or less means infinite, 0 disables it and any positive integer allows that many times.
        /// </remarks>
        [ClientProperty("maxPagesInHistory")]
        public int MaxPagesInHistory { get; set; }


        [ClientData("tweets")]
        public List<Tweet> Tweets { get; set; }

        public TweetLinesModel()
        {
            MaxTweetsToShow = DefaultMaxTweets;
            Tweets = new List<Tweet>();
        }
    }
}