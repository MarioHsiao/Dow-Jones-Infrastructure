using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO = DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.Web.Mvc.UI.Components.Twitter
{
    public class ModelMapper
    {
        public Tweet MapTweet(DTO.Tweet socialMediaTweet)
        {
            return new Tweet
                       {
                           IsoTimestamp = socialMediaTweet.CreatedDate,
                           Text = socialMediaTweet.Text,
                           User = MapUser(socialMediaTweet.User)
                       };
        }

        public User MapUser(DTO.TwitterUser twitterUser)
        {
            return new User()
                       {
                           FullName = twitterUser.Name,
                           ScreenName = twitterUser.ScreenName,
                           ProfileImageUrl = twitterUser.ProfileImageUrl
                       };
        }
    }
}
