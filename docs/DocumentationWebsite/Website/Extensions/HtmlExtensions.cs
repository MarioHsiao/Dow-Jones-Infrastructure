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
			return helper.Partial("_demoFrame", new ViewDataDictionary{{ "Url", url }});
        }

		public static IHtmlString DataViewer(this HtmlHelper helper, string url)
		{
			return new HtmlString(
				string.Format("<p><button class=\"dataViewer btn btn-mini btn-info\" data-url='{0}'>View Sample Data</button></p>"
							  , url)
			);
		}

		public static IHtmlString Note(this HtmlHelper helper, string text)
		{
			return new HtmlString(
				string.Format("<div class=\"note\"><img src=\"{1}\" alt=\"note\" /><span>{0}</span></div>"
							  , text, VirtualPathUtility.ToAbsolute("~/Styles/themes/spacelab/img/note.png"))
			);
		}

    }
}