using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Mapping;
using DTO = DowJones.Infrastructure.Models.SocialMedia;
using DowJones.Managers.SocialMedia.Twitter.Extensions;

namespace DowJones.Web.Mvc.UI.Components.SocialMedia
{
    public class Mapper : 
        ITypeMapper<DTO.Tweet, Tweet>, 
        ITypeMapper<DTO.TwitterUser, User> 
    {

        public Tweet Map(DTO.Tweet sourceTweet)
        {
            return new Tweet
                               {
                                   CreatedAt = sourceTweet.CreatedAt,
                                   Text = sourceTweet.Text,
                                   Html = sourceTweet.Html,
                                   User = Map(sourceTweet.User),
                                   Source = sourceTweet.Source,
                                   Id = sourceTweet.Id.ToString(),
                                   Favorited = sourceTweet.Favorited
                               };
        }

        public object Map(object source)
        {
            if (source is DTO.Tweet)
                return Map((DTO.Tweet)source);

            if (source is DTO.TwitterUser)
                return Map((DTO.TwitterUser)source);


            throw new NotSupportedException();
        }

        public User Map(DTO.TwitterUser twitterUser)
        {
            return new User()
            {
                FullName = twitterUser.Name,
                ScreenName = twitterUser.ScreenName,
                ProfileImageUrl = twitterUser.ProfileImageUrl,
                Id = twitterUser.Id,
                ProfileHashUrl = twitterUser.ProfileHasUrl,
                ProfileUrl = twitterUser.ProfileUrl
                
            };
        }
    }
}
