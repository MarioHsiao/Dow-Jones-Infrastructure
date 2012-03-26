using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using TweetSharp;

using TwitterTestApplication.ActionFilters;

namespace TwitterTestApplication.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// </summary>
        private readonly TwitterService service = new TwitterService(
                "ihQUiOm8C90eI7xmFqZkg",
                "i9p7iXAaRpwdsMMocJ5bUoxSipvby8FS5g3zezZuAa0",
                "32752715-5kZramQQrlZ41obO79jOMPUIpCJSp78zFNLksy0P5",
                "sOxjXiR4MLhTAxSUwlsY8SQExNNebuKEhyWIdNJP7Ic");

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Retweets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>An Action Result</returns>
        [AutoRefresh(5)]
        public ActionResult Retweet(long id = 80757375564390400)
        {     
            // http://api.twitter.com/1/statuses/80757375564390400/retweeted_by.json?count=10

            TwitterRateLimitStatus ratelimit = null;
            var retweets = service.ListUsersWhoRetweeted(id, 10);    
            if (service.Response.StatusCode == HttpStatusCode.OK) // <-- Should be 401 - Unauthorized
            {
                ratelimit = service.Response.RateLimitStatus;
            }
            else
            {
                
            }

            this.service.ListFavoriteTweets(1, 200);
                     
            this.ViewBag.ResetTime = this.GetDate(ratelimit.ResetTime); 
            return View(ratelimit);
        }

        private string GetDate(DateTime dt)
        {
            const string EasternZoneId = "Eastern Standard Time";
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById(EasternZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(dt, easternZone).ToString();
        }
    }        
}
