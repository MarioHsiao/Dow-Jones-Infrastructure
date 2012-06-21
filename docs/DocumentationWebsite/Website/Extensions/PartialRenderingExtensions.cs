using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.Website.Extensions
{
    public static class PartialRenderingExtensions
    {
        public static IHtmlString DocumentationPage(this HtmlHelper<PageViewModel> helper)
        {
            var page = helper.ViewData.Model;

            if(page == null)
                return new HtmlString(string.Empty);

            return helper.Partial(page.Key, page);
        }

        public static IHtmlString DocumentationSection(this HtmlHelper<PageViewModel> helper, ContentSectionViewModel section)
        {
            return DocumentationSection(helper, section.Key);
        }
        public static IHtmlString DocumentationSection(this HtmlHelper<PageViewModel> helper, Name name)
        {
            return DocumentationSection(helper,name.Key);
        }
        public static IHtmlString DocumentationSection(this HtmlHelper<PageViewModel> helper, string section)
        {
            var page = helper.ViewData.Model;

            if(page == null)
                return new HtmlString(string.Empty);

            return helper.Partial(section, page);
        }
    }
}