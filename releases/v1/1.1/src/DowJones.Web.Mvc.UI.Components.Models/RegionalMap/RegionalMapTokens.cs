using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.RegionalMap
{
    public class RegionalMapTokens : AbstractTokenBase
    {
        public RegionalMapTokens()
        {
            // TODO: add these 4 tokens into translate DB
            Add("regionNAMZ", GetTokenByName("northAmerica"));
            Add("regionCAMZ", GetTokenByName("centralAmerica"));
            Add("regionSAMZ", GetTokenByName("southAmerica"));
            Add("change", GetTokenByName("changeLowerCase"));

            Add("regionEURZ", GetTokenByName("europe"));
            Add("regionMEASTZ", GetTokenByName("middleEast"));
            Add("regionAPACZ", GetTokenByName("asia"));
            Add("regionAUSTR", GetTokenByName("countryName9Aus"));
            Add("regionRUSS", GetTokenByName("s2regionRussia"));
            Add("regionAFRICAZ", GetTokenByName("africa"));
            Add("regionINDIA", GetTokenByName("countryName9Ind"));
            Add("articles", GetTokenByName("articles"));
        }
    }
}
