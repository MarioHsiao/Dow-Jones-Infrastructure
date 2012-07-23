using System;
using System.Web;
using System.Web.Mvc;

namespace DowJones.Web.Showcase.Extensions
{
    public static class HtmlHelperExtensions
    {

        public static IHtmlString ExecutionTime<T>(this HtmlHelper<T> htmlHelper)
        {
            var executionTime = DateTime.Now - htmlHelper.ViewContext.HttpContext.Timestamp;
            return new HtmlString(executionTime.ToString());
        }
    }
}