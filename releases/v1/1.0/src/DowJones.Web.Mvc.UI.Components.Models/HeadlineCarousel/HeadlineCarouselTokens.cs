using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.HeadlineCarousel
{
    public class HeadlineCarouselTokens: AbstractTokenBase
    {
        public HeadlineCarouselTokens()
        {
            Add("noResultsTkn", GetTokenByName("noResults"));
            Add("articlesLabelTkn", GetTokenByName("articlesLabel"));
        }
    }
}
