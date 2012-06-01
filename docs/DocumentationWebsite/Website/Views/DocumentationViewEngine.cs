using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MarkdownRazor;

namespace DowJones.Documentation.Website.Views
{
    public class DocumentationViewEngine : MarkdownRazorViewEngine
    {
        public DocumentationViewEngine(string folder)
        {
            ViewLocationFormats = ReplacePageLocations(ViewLocationFormats, folder);
            PartialViewLocationFormats = ReplacePageLocations(PartialViewLocationFormats, folder);
        }

        private string[] ReplacePageLocations(IEnumerable<string> locations, string folder)
        {
            locations = locations.ToArray();

            // "~/Views/{1}/{0}.cshtml" -> "{folder}/{category}/{0}.cshtml"
            var pages = locations.Select(x => x.Replace("~/Views", folder).Replace("{1}", "[category]")).ToArray();

            // "{folder}/{category}/{0}.cshtml" -> "{folder}/{category}/{page}/{0}.cshtml"
            var sections = pages.Select(x => x.Replace("{0}", "[page]/{0}")).ToArray();

            return pages.Union(sections).ToArray();
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string category;
            if (TryGetRouteValue(controllerContext, "category", out category))
            {
                partialPath = RemapPath(controllerContext, partialPath);
                return base.CreatePartialView(controllerContext, partialPath);
            }

            return base.CreatePartialView(controllerContext, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string category;
            if (TryGetRouteValue(controllerContext, "category", out category))
            {
                viewPath = RemapPath(controllerContext, viewPath);
                return base.CreateView(controllerContext, viewPath, null);
            }

            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            virtualPath = RemapPath(controllerContext, virtualPath);

            return base.FileExists(controllerContext, virtualPath);
        }

        private static string RemapPath(ControllerContext controllerContext, string path)
        {
            string remappedPath = path;

            string category, page;

            if (TryGetRouteValue(controllerContext, "category", out category))
                remappedPath = remappedPath.Replace("[category]", category);

            if (TryGetRouteValue(controllerContext, "page", out page))
                remappedPath = remappedPath.Replace("[page]", page);

            return remappedPath;
        }

        private static bool TryGetRouteValue(ControllerContext controllerContext, string key, out string value)
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