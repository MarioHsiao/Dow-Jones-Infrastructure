using DowJones.Infrastructure;
using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.NewsStandTicker
{
    public class NewsStandTickerTokens : AbstractTokenBase
    {
        public NewsStandTickerTokens()
        {
            Add("noResultsTkn", GetTokenByName("noResults"));
            Add("articlesTkn", GetTokenByName("articles"));
        }
    }
}
