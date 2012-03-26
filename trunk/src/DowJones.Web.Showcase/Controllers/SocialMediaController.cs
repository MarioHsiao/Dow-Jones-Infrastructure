using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Managers.SocialMedia.TweetRiver;
using DowJones.Web.Mvc.UI.Components.SocialMedia;
using DowJones.Web.Showcase.Models;
using DowJones.Managers.SocialMedia;
using SocialMediaMapper = DowJones.Web.Mvc.UI.Components.SocialMedia.Mapper;

namespace DowJones.Web.Showcase.Controllers
{
    public class SocialMediaController : CanvasControllerBase
    {
        //
        // GET: /SocialMedia/

        public ActionResult Index(string i = "iacc")
        {
            Guard.IsNotNullOrEmpty(i, "i");

            var mapper = new SocialMediaMapper();

            var response = new SocialMediaService(new TweetRiverProvider()).GetTweetsByIndustry(i);

            var socialMediaViewModel = new SocialMediaViewModel
                                     {
                                         TweetLinesModel = new TweetLinesModel
                                         {
                                             MaxTweetsToShow = 50,
                                             Tweets = response.Select(mapper.Map).ToList()
                                         },
                                         ExpertsModel = new ExpertsModel()
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
