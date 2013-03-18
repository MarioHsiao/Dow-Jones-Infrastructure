using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Managers.SocialMedia.Config
{
    public interface ISocialMediaIndustryProvider
    {
        string GetChannelFromIndustryCode(string code);             
    }
}
