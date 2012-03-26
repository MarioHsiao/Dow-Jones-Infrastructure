using Newtonsoft.Json;
using System.Collections.Generic;
namespace DowJones.Web.Mvc.UI.Components.Twitter
{
    public class TweetLinesModel 
    {
        [ClientProperty("maxTweetsToShow")]
        public uint MaxTweetsToShow { get; set; }

        [ClientData("tweets")]
        public List<Tweet> Tweets { get; set; }
    }
}