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

            string category, page, mode;

            if (TryGetRouteValue(controllerContext, "category", out category))
                remappedPath = remappedPath.Replace("[category]", category);

            if (TryGetRouteValue(controllerContext, "page", out page))
                remappedPath = remappedPath.Replace("[page]", page);

            if (TryGetRouteValue(controllerContext, "mode", out mode))
                remappedPath = remappedPath.Replace("[mode]", mode);

            return remappedPath;
        }

        internal static string[] ReplacePageLocations(this IEnumerable<string> locations, string folder)
        {
            locations = locations.ToArray();

            // "~/Views/{1}/{0}.cshtml" -> "{folder}/{category}/{0}.cshtml"
            var pages = locations.Select(x => x.Replace("~/Views", folder).Replace("{1}", "[category]")).ToArray();

            // "{folder}/{category}/{0}.cshtml" -> "{folder}/{category}/{page}/{0}.cshtml"
            var sections = pages.Where(x => x.IndexOf("/Shared/") == -1)
                .Select(x => x.Replace("{0}", "[page]/{0}")).ToArray();

            // "{folder}/{category}/{0}.cshtml" -> "{folder}/{category}/{page}/{mode}/{0}.cshtml"
            var sectionViews = sections.Select(x => x.Replace("{0}", "[mode]/{0}")).ToArray();

            return sectionViews.Union(sections).Union(pages).ToArray();
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