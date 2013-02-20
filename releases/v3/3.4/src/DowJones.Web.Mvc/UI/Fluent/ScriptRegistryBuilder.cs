using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Resources;

namespace DowJones.Web.Mvc.UI
{
    public class ScriptRegistryBuilder : ComponentBuilderBase<IScriptRegistry, ScriptRegistryBuilder>
    {
        private readonly IScriptRegistry _scriptRegistry;
        private readonly IScriptRegistryWriter _scriptRegistryWriter;

        internal HtmlHelper HtmlHelper { get; set; }

        public ScriptRegistryBuilder(IScriptRegistry scriptRegistry, IScriptRegistryWriter scriptRegistryWriter)
            : base(scriptRegistry)
        {
            _scriptRegistry = scriptRegistry;
            _scriptRegistryWriter = scriptRegistryWriter;
        }

        public virtual ScriptRegistryBuilder Include(string url, ClientResourceDependencyLevel? dependencyLevel = null)
        {
            Guard.IsNotNullOrEmpty(url, "url");

            return Include((ClientResource)url, dependencyLevel);
        }

        public virtual ScriptRegistryBuilder Include(ClientResource resource, ClientResourceDependencyLevel? dependencyLevel = null)
        {
            Guard.IsNotNull(resource, "resource");

            resource.DependencyLevel = dependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Independent);
            
            _scriptRegistry.Register(resource);

            return this;
        }

        public virtual ScriptRegistryBuilder IncludeResource(Assembly targetAssembly, string resourceName, ClientResourceDependencyLevel? dependencyLevel = null)
        {
            var resourceDependencyLevel = dependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Independent);

            EmbeddedClientResource clientResource = new EmbeddedClientResource(targetAssembly, resourceName, ClientResourceKind.Script, resourceDependencyLevel);

            return Include(clientResource);
        }


        /// <summary>
        /// Defines the inline handler executed when the DOM document is ready (using the $(document).ready jQuery event)
        /// </summary>
        /// <param name="onDocumentReadyAction">The action defining the inline handler</param>
        /// <example>
        /// <code lang="CS">
        /// @DJ.ScriptRegistry().OnDocumentReady(@&lt;text&gt;
        ///             function() {
        ///                 alert("Document is ready");
        ///             }
        ///           &lt;/text&gt;);
        /// </code>
        /// </example>
        public virtual ScriptRegistryBuilder OnDocumentReady(Func<object, object> onDocumentReadyAction)
        {
            var model = (HtmlHelper.ViewData != null) ? HtmlHelper.ViewData.Model : null;
            var result = onDocumentReadyAction(model);
            return OnDocumentReady((result ?? string.Empty).ToString());
        }

        /// <summary>
        /// Appends the specified statement in $(document).ready jQuery event.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <returns></returns>
        public virtual ScriptRegistryBuilder OnDocumentReady(string statements)
        {
            _scriptRegistry.OnDocumentReadyStatements.Add(statements);
            return this;
        }

        public virtual ScriptRegistryBuilder With<TComponent>()
            where TComponent : class, IViewComponent
        {
            return With(typeof(TComponent));
        }

        public virtual ScriptRegistryBuilder With(Type componentType)
        {
            Guard.Against(
                !typeof(IViewComponent).IsAssignableFrom(componentType), 
                "componentType must be of type IViewComponent");

            var clientResources =
                componentType
                    .GetClientResourceAttributes()
                    .Select(x => x.ToClientResource(componentType))
                    .Where(x => x.ResourceKind == ClientResourceKind.Script).ToArray();

            foreach (var resource in clientResources)
                _scriptRegistry.Register(resource);

            return this;
        }
        
        public virtual ScriptRegistryBuilder WithCrossDomain(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.CrossDomain, ClientResourceDependencyLevel.Core, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryUIEffects(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUICore, ClientResourceDependencyLevel.Global, enabled);
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUIEffects, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryUIInteractions(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUICore, ClientResourceDependencyLevel.Global, enabled);
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUIInteractions, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryUIWidgets(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUICore, ClientResourceDependencyLevel.Global, enabled);
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUIWidgets, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithHighCharts(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.Highcharts, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithHighChartsMore(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.HighchartsMore, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithServiceProxy(bool enabled = true)
        {
            if (enabled)
            {
                WithErrorManager();
            }

            RegisterFrameworkWebResource(EmbeddedResources.Js.ServiceProxy, ClientResourceDependencyLevel.Global, enabled: enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithErrorManager(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.ErrorManager, ClientResourceDependencyLevel.Global, enabled: enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryCarousel(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryCarousel, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryCycle(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryCycle, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        [Obsolete("Moved to common.js - explicit inclue no longer required")]
        public virtual ScriptRegistryBuilder WithDateFormat(bool enabled = true)
        {
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryTouch(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryTouchMin, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }


        public virtual ScriptRegistryBuilder WithColorPicker(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.ColorPicker, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryTimeAgo(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryTimeAgo, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryTools(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryTools, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJScrollPane(bool enabled = true)
        {
            RegisterFrameworkWebResource( EmbeddedResources.Js.JQueryJScrollPane, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithPopupBalloon (bool enabled = true)
        {
            RegisterFrameworkWebResource( EmbeddedResources.Js.JQueryPopupBalloon, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

		public virtual ScriptRegistryBuilder WithODSManager(bool enabled = true)
		{
			WithServiceProxy();
			RegisterFrameworkWebResource(EmbeddedResources.Js.ODSManager, ClientResourceDependencyLevel.MidLevel, enabled);
			return this;
		}

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal ClientResource RegisterFrameworkWebResource(string resourceName, ClientResourceDependencyLevel? dependencyLevel, bool enabled)
        {
            var resourceDependencyLevel = dependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Global);

            var clientResource = new EmbeddedClientResource(GetType().Assembly, resourceName, ClientResourceKind.Script, resourceDependencyLevel);

            if (enabled)
                _scriptRegistry.Register(clientResource);
            else
                _scriptRegistry.Unregister(clientResource);

            return clientResource;
        }

        public IHtmlString Render()
        {
            return new HtmlString(_scriptRegistryWriter.Render(_scriptRegistry));
        }

        public void Render(HtmlTextWriter writer)
        {
            _scriptRegistryWriter.Render(_scriptRegistry, writer);
        }

        public IHtmlString RenderHead()
        {
            return new HtmlString(_scriptRegistryWriter.RenderHead(_scriptRegistry));
        }

        public IScriptRegistry ToScriptRegistry()
        {
            return _scriptRegistry;
        }
    }
}
