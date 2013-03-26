using System.Reflection;
using System.Web;
using System.Web.UI;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI
{
    public class StylesheetRegistryBuilder : ComponentBuilderBase<IStylesheetRegistry, StylesheetRegistryBuilder>
    {
        private readonly IStylesheetRegistry _stylesheetRegistry;

        public StylesheetRegistryBuilder(IStylesheetRegistry stylesheetRegistry)
            : base(stylesheetRegistry)
        {
            _stylesheetRegistry = stylesheetRegistry;
        }

        public virtual StylesheetRegistryBuilder Add(string selector, string style)
        {
            var pageStyle = new PageStyle { Selector = selector, Style = style };
            _stylesheetRegistry.AddPageStyle(pageStyle);
            return this;
        }

        public virtual StylesheetRegistryBuilder Include(string url, ClientResourceDependencyLevel? dependencyLevel = null)
        {
            Guard.IsNotNullOrEmpty(url, "url");
            return Include((ClientResource) url, dependencyLevel);
        }

        public virtual StylesheetRegistryBuilder Include(ClientResource resource, ClientResourceDependencyLevel? dependencyLevel = null)
        {
            Guard.IsNotNull(resource, "resource");

            if (dependencyLevel != null)
                resource.DependencyLevel = dependencyLevel.Value;

            _stylesheetRegistry.Register(resource);

            return this;
        }

        public virtual StylesheetRegistryBuilder IncludeResource(Assembly targetAssembly, string resourceName, ClientResourceDependencyLevel? dependencyLevel = null)
        {
            Guard.IsNotNull(targetAssembly, "targetAssembly");
            Guard.IsNotNullOrEmpty(resourceName, "resourceName");

            var resolvedDependencyLevel = dependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Component);

            var resource = new EmbeddedClientResource(targetAssembly, resourceName, ClientResourceKind.Stylesheet, resolvedDependencyLevel);

            _stylesheetRegistry.Register(resource);

            return this;
        }


        public void Render(HtmlTextWriter writer)
        {
            _stylesheetRegistry.Render(writer);
        }

        public IHtmlString Render()
        {
            return _stylesheetRegistry;
        }

        public IHtmlString RenderHead()
        {
            return Render();
        }
    }
}