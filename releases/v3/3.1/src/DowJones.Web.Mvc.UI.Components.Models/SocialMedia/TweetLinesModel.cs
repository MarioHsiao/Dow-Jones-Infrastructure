using Newtonsoft.Json;
using System.Collections.Generic;
namespace DowJones.Web.Mvc.UI.Components.SocialMedia
{
    public class TweetLinesModel : ViewComponentModel
    {
        [ClientProperty("maxTweetsToShow")]
        public uint MaxTweetsToShow { get; set; }

        //[ClientData("tweets")]
        public List<Tweet> Tweets { get; set; }
    }
}