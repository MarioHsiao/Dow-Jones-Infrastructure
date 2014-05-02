using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure.Models.SocialMedia;
using DowJones.Mapping;
using DTO = DowJones.Infrastructure.Models.SocialMedia;


namespace DowJones.Ajax.SocialMedia
{
    public class Mapper :
        ITypeMapper<DTO.Tweet, Tweet>,
        ITypeMapper<DTO.TwitterUser, User>,
        ITypeMapper<IList<DTO.Tweet>, IList<Tweet>>,
        ITypeMapper<IList<DTO.TwitterUser>, IList<User>>
    {

        public Tweet Map(DowJones.Infrastructure.Models.SocialMedia.Tweet sourceTweet)
        {
            return new Tweet
            {
                CreatedDateTime = sourceTweet.CreatedAt,
                Text = sourceTweet.Text,
                User = Map(sourceTweet.User),
                Source = sourceTweet.Source,
                Id = sourceTweet.Id.ToString(),
				Entities =  sourceTweet.Entities
            };
        }

        public IList<Tweet> Map(IList<DowJones.Infrastructure.Models.SocialMedia.Tweet> sourceTweets)
        {
            List<Tweet> tweets = new List<Tweet>();

            foreach (DowJones.Infrastructure.Models.SocialMedia.Tweet sourceTweet in sourceTweets)
            {
                tweets.Add(this.Map(sourceTweet));
            }
            return tweets;
        }

        public object Map(object source)
        {
            if (source is DTO.Tweet)
                return Map((DTO.Tweet)source);

            if (source is DTO.TwitterUser)
                return Map((DTO.TwitterUser)source);


            throw new NotSupportedException();
        }

        public User Map(TwitterUser twitterUser)
        {
            return new User()
            {
                FullName = twitterUser.Name,
                ScreenName = twitterUser.ScreenName,
                ProfileImageUrl = twitterUser.ProfileImageUrl,
                Id = twitterUser.Id,
                ProfileHashUrl = twitterUser.ProfileHashUrl,
                ProfileUrl = twitterUser.ProfileUrl
            };
        }

        public IList<User> Map(IList<TwitterUser> twitterUsers)
        {
            List<User> users = new List<User>();

            foreach (DowJones.Infrastructure.Models.SocialMedia.TwitterUser twitterUser in twitterUsers)
            {
                users.Add(this.Map(twitterUser));
            }
            return users;
        }
    }
}
