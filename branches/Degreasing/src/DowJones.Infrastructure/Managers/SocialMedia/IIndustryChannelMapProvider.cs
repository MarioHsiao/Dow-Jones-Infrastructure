using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Managers.SocialMedia
{
    interface IIndustryChannelMapProvider
    {
        string GetChannelFromIndustryCode(string industryCode);

    }
}
