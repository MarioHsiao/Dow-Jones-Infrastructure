using System;
using System.Web;
using System.Web.Mvc;
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
            var tag = new TagBuilder("iframe");
            tag.AddCssClass("showcase");
            tag.Attributes.Add("src", url);
            return new HtmlString(tag.ToString());
        }
    }
}