using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Ajax.SocialMedia;
using DowJones.Infrastructure;
using DowJones.Infrastructure.Common;
using DowJones.Loggers;
using DowJones.Managers.SocialMedia.TweetRiver;
using DowJones.Web.Mvc.UI.Components.SocialMedia;
using DowJones.Web.Showcase.Models;
using DowJones.Managers.SocialMedia;
using SocialMediaMapper = DowJones.Ajax.SocialMedia.Mapper;
using DowJones.Managers.SocialMedia.Config;
using DowJones.Session;

namespace DowJones.Web.Showcase.Controllers
{
    public class SocialMediaController : DowJones.Web.Mvc.ControllerBase
    {
        public ActionResult Index(string i = "iacc")
        {
            Guard.IsNotNullOrEmpty(i, "i");

            var mapper = new SocialMediaMapper();

            IControlData controlData = new ControlData { UserID = "snap_proxy", UserPassword = "pa55w0rd", ProductID = "16" };
            Product product = new Product("test", "test", null, true);
            var response = new SocialMediaService(new TweetRiverProvider(), new PAMSocialMediaIndustryProvider(controlData, new BasicTransactionTimer()), controlData, product).GetTweetsByIndustry(i);

            var socialMediaViewModel = new SocialMediaViewModel
            {
                TweetLinesModel = new TweetLinesModel
                {
                    MaxTweetsToShow = 50,
                    Tweets = response.Select(mapper.Map).ToList()
                },
                TwitterExpertsModel = new ExpertsModel()
                {
                    Experts = GetMockExperts()

                }
            };

            return View(socialMediaViewModel);
        }

        private static List<User> GetMockExperts()
        {
            return new List<User>
                       {
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                           new User
                               {
                                   FullName = "Scott Hanselman",
                                   ScreenName = "shanselman",
                                   ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg",
                                   IsVerified = true,
                                   Id = 5676102
                               },
                       };
        }
    }
}
