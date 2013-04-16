using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hammock;

namespace DowJones.Managers.SocialMedia
{
    public interface ISocialMediaProvider
    {
        RestClient Client { get; }
        RestRequest GetSocialMediaRequest(string channel, RequestOptions requestOptions);

    }
}
