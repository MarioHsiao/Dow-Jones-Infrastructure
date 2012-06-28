using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace DowJones.Documentation.Website.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString CodeSnippet<TModel>(this HtmlHelper<TModel> helper, string mode, Func<TModel, HelperResult> template)
        {
            var tag = new TagBuilder("pre");
            tag.AddCssClass("prettyprint linenums");
            tag.SetInnerText(template(helper.ViewData.Model).ToHtmlString());
            return new HtmlString(tag.ToString());
        }

        public static IHtmlString DemoFrame(this HtmlHelper helper, string url)
        {
			// the loading div...
			var builder = new TagBuilder("div");
			builder.AddCssClass("showcase");
			builder.SetInnerText("Please wait while the demo loads...");

			// seed for iframe
			var liveDemoUrl = helper.Hidden("liveDemoUrl", url);

			return new HtmlString(builder.ToString() + liveDemoUrl.ToString());
        }

		public static IHtmlString DataViewer(this HtmlHelper helper, string url)
		{
			return new HtmlString(
				string.Format("<p><button class=\"dataViewer btn btn-mini btn-info\" data-url='{0}'>View Sample Data</button></p>"
							  , url)
			);
		}

    }
}