using System;
using System.Web.Mvc;
using DowJones.Extensions;

namespace DowJones.Utilities.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string WebResourceUrl<T>(this UrlHelper urlHelper, string resourceName)
        {
            return WebResourceUrl(urlHelper, typeof(T), resourceName);
        }

        public static string WebResourceUrl(this UrlHelper urlHelper, Type targetAssemblyType, string resourceName)
        {
            var resourceUrl = targetAssemblyType.Assembly.GetWebResourceUrl(resourceName);

            return resourceUrl;
        }
    }
}
