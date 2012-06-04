using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DowJones.Documentation.Website.ViewEngines
{
    public static class DocumentationViewEngineHelpers
    {
        internal static string GetRemapDocumentationPath(this string path, ControllerContext controllerContext)
        {
            string remappedPath = path;

            string category, page;

            if (TryGetRouteValue(controllerContext, "category", out category))
                remappedPath = remappedPath.Replace("[category]", category);

            if (TryGetRouteValue(controllerContext, "page", out page))
                remappedPath = remappedPath.Replace("[page]", page);

            return remappedPath;
        }

        internal static string[] ReplacePageLocations(this IEnumerable<string> locations, string folder)
        {
            locations = locations.ToArray();

            // "~/Views/{1}/{0}.cshtml" -> "{folder}/{category}/{0}.cshtml"
            var pages = locations.Select(x => x.Replace("~/Views", folder).Replace("{1}", "[category]")).ToArray();

            // "{folder}/{category}/{0}.cshtml" -> "{folder}/{category}/{page}/{0}.cshtml"
            var sections = pages.Select(x => x.Replace("{0}", "[page]/{0}")).ToArray();

            return pages.Union(sections).ToArray();
        }


        internal static bool TryGetRouteValue(this ControllerContext controllerContext, string key, out string value)
        {
            object objValue;

            if (!controllerContext.RouteData.Values.TryGetValue(key, out objValue))
            {
                controllerContext.Controller.ViewData.TryGetValue(key, out objValue);
            }

            value = objValue as string;
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}