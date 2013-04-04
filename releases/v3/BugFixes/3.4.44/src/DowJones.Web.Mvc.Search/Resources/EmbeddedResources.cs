using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc.Search.Resources;
using DowJones.Web.Mvc.UI;

[assembly: WebResource(EmbeddedResources.Js.SearchResultsPage, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.Search.Resources
{
    public static class EmbeddedResources
    {
        private const string BaseName = "DowJones.Web.Mvc.Search.Resources";

        [ScriptResource(ResourceName = SearchResultsPage, DependencyLevel = ClientResourceDependencyLevel.Independent)]

        public static class Js
        {
            public const string SearchResultsPage = BaseName + ".SearchResultsPage.js";
        }
    }

    public static class EmbeddedResourcesExtensions
    {
        public static ScriptRegistryBuilder WithSearchResultsPage(this ScriptRegistryBuilder builder)
        {
            return builder.IncludeResource(
                typeof (EmbeddedResources).Assembly, 
                EmbeddedResources.Js.SearchResultsPage);
        }
    }
}
