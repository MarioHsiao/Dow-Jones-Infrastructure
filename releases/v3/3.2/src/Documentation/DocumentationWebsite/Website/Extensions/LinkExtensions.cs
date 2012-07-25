using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.Website.Extensions
{
    public static class LinkExtensions
    {
        public class DocumentationBrowserRouteData : RouteValueDictionary
        {
            public string category
            {
                get { return this["category"] as string; }
                set { this["category"] = value; }
            }

            public string page
            {
                get { return this["page"] as string; }
                set { this["page"] = value; }
            }

            public string section
            {
                get { return this["section"] as string; }
                set { this["section"] = value; }
            }

            public string DisplayName { get; set; }

            public DocumentationBrowserRouteData()
            {
                category = null;
                page = null;
                section = null;
            }

            public DocumentationBrowserRouteData(CategoryViewModel category)
                : this()
            {
                if (category == null) return;

				this.category = category.DisplayKey;
                DisplayName = category.DisplayName;
            }

            public DocumentationBrowserRouteData(PageViewModel page)
                : this(page.Category)
            {
				this.page = page.DisplayKey;
                DisplayName = page.DisplayName;
            }
        }


        public static IHtmlString DocumentationCategoryLink(this HtmlHelper helper, CategoryViewModel category)
        {
            return DocumentationPageLink(helper, new DocumentationBrowserRouteData(category));
        }

        public static IHtmlString DocumentationPageLink(this HtmlHelper helper, PageViewModel page)
        {
            return DocumentationPageLink(helper, new DocumentationBrowserRouteData(page));
        }

        static IHtmlString DocumentationPageLink(this HtmlHelper helper, DocumentationBrowserRouteData routeData)
        {
            // HACK: Need to build this manually because apparently using RouteLink works locally 
            //       but not when deployed to Montana.

			var relativeUrl = string.Format("~/{0}/{1}", routeData.category, routeData.page);

            var link = new TagBuilder("a");
            link.Attributes["href"] = VirtualPathUtility.ToAbsolute(relativeUrl);
            link.InnerHtml = routeData.DisplayName;

            return new HtmlString(link.ToString());
        }
    }
}