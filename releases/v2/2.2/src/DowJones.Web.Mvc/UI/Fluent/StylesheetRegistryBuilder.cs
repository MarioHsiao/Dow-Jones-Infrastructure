using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Resources;

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
            PageStyle pageStyle = new PageStyle { Selector = selector, Style = style };
            _stylesheetRegistry.AddPageStyle(pageStyle);
            return this;
        }

        public virtual StylesheetRegistryBuilder Include(ClientResource resource, ClientResourceDependencyLevel? dependencyLevel = null)
        {
            Guard.IsNotNull(resource, "resource");

            if (dependencyLevel != null)
                resource.DependencyLevel = dependencyLevel.Value;

            _stylesheetRegistry.Register(resource);

            return this;
        }

        public virtual StylesheetRegistryBuilder IncludeResource(Assembly targetAssembly, string resourceName, ClientResourceDependencyLevel? dependencyLevel = null, bool? performSubstitution = null)
        {
            Guard.IsNotNull(targetAssembly, "targetAssembly");
            Guard.IsNotNullOrEmpty(resourceName, "resourceName");

            var resolvedDependencyLevel = dependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Component);

            var resource = new EmbeddedClientResource(targetAssembly, resourceName, ClientResourceKind.Stylesheet, resolvedDependencyLevel, performSubstitution);

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

        [Obsolete("ToHtml() is no longer supported - use the explicit RenderHead() and Render() methods instead")]
        public IHtmlString ToHtml()
        {
            throw new NotSupportedException("ToHtml() is no longer supported - use the explicit RenderHead() and Render() methods instead");
        }
    }
}