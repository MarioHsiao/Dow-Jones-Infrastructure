using DowJones.Ajax.HeadlineList;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.Common
{
    public class ArticleSource
    {
        public string DisplayName { get; set; }

        public string Reference { get; set; }

        public ArticleSource()
        {
        }

        public ArticleSource(HeadlineInfo headline)
        {
            Guard.IsNotNull(headline, "headline");
            DisplayName = headline.sourceDescriptor;
            Reference = headline.sourceReference;
        }
    }
}