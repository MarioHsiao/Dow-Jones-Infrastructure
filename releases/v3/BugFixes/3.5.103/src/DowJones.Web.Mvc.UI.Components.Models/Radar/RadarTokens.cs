using DowJones.Infrastructure;
using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.Radar
{
    public class RadarTokens : AbstractTokenBase
    {
        public RadarTokens()
        {
            Add("totalLabel", GetTokenByName("totalLabel"));
            Add("newsVolumePercentageChange", GetTokenByName("newsVolumePercentageChange"));
            Add("totalPercentageChange", GetTokenByName("totalPercentageChange"));
            Add("articlesLabel", GetTokenByName("articlesLabel"));
        }
    }
}
