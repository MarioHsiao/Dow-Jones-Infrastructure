using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.Website.Extensions
{
    public static class LinkExtensions
    {
        public static IHtmlString DocumentationCategoryLink(this HtmlHelper helper, string categoryName)
        {
            var category = MvcApplication.DocumentationPages.Category(categoryName);
            return DocumentationCategoryLink(helper, category);
        }

        public static IHtmlString DocumentationCategoryLink(this HtmlHelper helper, DocumentationCategory category)
        {
            return helper.RouteLink(
                category.DisplayName, "DocumentationBrowser",
                new {category = category.Name, page = (string) null}, null
            );
        }

        public static IHtmlString DocumentationPageLink(this HtmlHelper helper, DocumentationPage page)
        {
            return helper.RouteLink(
                page.DisplayName, "DocumentationBrowser",
                new {category = page.Category.Name, page = page.Name}, null
                );
        }
    }
}