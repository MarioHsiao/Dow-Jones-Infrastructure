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

            string category, page, section;

            if (TryGetRouteValue(controllerContext, "category", out category))
                remappedPath = remappedPath.Replace("[category]", category);

            if (TryGetRouteValue(controllerContext, "page", out page))
                remappedPath = remappedPath.Replace("[page]", page);

            if (TryGetRouteValue(controllerContext, "section", out section))
                remappedPath = remappedPath.Replace("[section]", section);

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

            // "{folder}/{category}/{0}.cshtml" -> "{folder}/{category}/{page}/{section}/{0}.cshtml"
            var sectionViews = sections.Select(x => x.Replace("{0}", "[section]/{0}")).ToArray();

            return sectionViews.Union(sections).Union(pages).ToArray();
        }


        internal static bool TryGetRouteValue(this ControllerContext controllerContext, string key, out string value)
        {
            object objValue;

            if (!controllerContext.RouteData.Values.TryGetValue(key, out objValue))
            {
                ViewDataDictionary viewData;

                var viewContext = controllerContext as ViewContext;
                if (viewContext != null)
                    viewData = viewContext.ViewData;
                else
                    viewData = controllerContext.Controller.ViewData;

                viewData.TryGetValue(key, out objValue);
            }

            value = objValue as string;
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}