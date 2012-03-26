using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Web.Mvc.UI.Components.SocialMedia;

namespace DowJones.Web.Showcase.Models
{
    public class SocialMediaViewModel : Mvc.UI.Canvas.Module
    {
        public TweetLinesModel TweetLinesModel { get; set; }

        public ExpertsModel Experts { get; set; }

        public SocialMediaViewModel()
        {
            //TweetLinesModel = GetMockTweets();
        }

        private TweetLinesModel GetMockTweets()
        {
            var TweetLines = new TweetLinesModel
            {
                MaxTweetsToShow = 100,
                Tweets = new List<Tweet>() { 
                                                new Tweet{ 
                                                    Text  = "Thisa stack:", 
                                                    Id = "88524551671132160",
                                                    ReTweetId = "88524551671132160",
                                                    User = new User {FullName = "Mamta" , ScreenName = "Mamta Aithani" , Id = 5676102, ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg" }
                                                } , 
                                                new Tweet{ 
                                                    Text  = "Month 2 at this", 
                                                    Id = "88518261217570820",
                                                    ReTweetId = "88518261217570820",
                                                    User = new User {FullName = "Hrushi", ScreenName = "Hrushi Panda" , Id = 5637652, ProfileImageUrl = "http://a1.twimg.com/profile_images/58911850/coding-horror-gravatar-128_normal.png" }
                                                } , 
                                                new Tweet{ 
                                                    Text  = "This month's non-Kindle Reading List, as a stack:  ", 
                                                    Id = "88524551671132160",
                                                    ReTweetId = "88524551671132160",
                                                    User = new User {FullName = "Dave" , ScreenName = "Dave" , Id = 5676102, ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg" }
                                                } , 
                                                new Tweet{ 
                                                    Text  = "There is no problem animated GIFs cannot solve.", 
                                                    Id = "88518916447547400",
                                                    ReTweetId = "88518916447547400",
                                                    User = new User {FullName = "Jess" , ScreenName = "Jess" , Id = 5676102, ProfileImageUrl = "http://a2.twimg.com/profile_images/1425619283/image_normal.jpg" }
                                                } 
                                         }
            };

            return TweetLines; 
        }




        public ExpertsModel ExpertsModel { get; set; }
    }
}