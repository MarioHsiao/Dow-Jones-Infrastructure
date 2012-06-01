using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.Website.Extensions
{
    public static class PartialRenderingExtensions
    {
        public static IHtmlString DocumentationPage(this HtmlHelper<DocumentationPage> helper)
        {
            var page = helper.ViewData.Model;

            if(page == null)
                return new HtmlString(string.Empty);

            return helper.Partial(page.Name, page);
        }

        public static IHtmlString DocumentationSection(this HtmlHelper<DocumentationPage> helper, string section)
        {
            var page = helper.ViewData.Model;

            if(page == null)
                return new HtmlString(string.Empty);

            return helper.Partial(section, page);
        }
    }
}