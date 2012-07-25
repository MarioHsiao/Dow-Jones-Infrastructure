using System.Diagnostics.Contracts;
using System.Web.Mvc;

namespace DowJones.Documentation.Website.ViewEngines
{
    public class VSDocViewEngine : XmlViewEngine.XmlViewEngine
    {
        public string Layout { get; set; }

        public VSDocViewEngine(string folder)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(folder));

            Layout = "~/Views/Shared/VSDoc.xsl";
            ViewLocationFormats = ViewLocationFormats.ReplacePageLocations(folder);
            PartialViewLocationFormats = ViewLocationFormats;
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string category;
            if (controllerContext.TryGetRouteValue("category", out category))
            {
                partialPath = partialPath.GetRemapDocumentationPath(controllerContext);
                return base.CreatePartialView(controllerContext, partialPath);
            }

            return base.CreatePartialView(controllerContext, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string category;
            if (controllerContext.TryGetRouteValue("category", out category))
            {
                viewPath = viewPath.GetRemapDocumentationPath(controllerContext);
                return base.CreateView(controllerContext, viewPath, Layout);
            }

            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            virtualPath = virtualPath.GetRemapDocumentationPath(controllerContext);
            return base.FileExists(controllerContext, virtualPath);
        }
    }
}