using System;
using System.ComponentModel;
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

        public virtual ScriptRegistryBuilder Include(ClientResource resource, ClientResourceDependencyLevel? dependencyLevel = null, bool? performSubstitution = null)
        {
            Guard.IsNotNull(resource, "resource");

            resource.DependencyLevel = dependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Independent);
            resource.PerformSubstitution = performSubstitution.GetValueOrDefault(resource.PerformSubstitution);
            
            _scriptRegistry.Register(resource);

            return this;
        }

        public virtual ScriptRegistryBuilder IncludeResource(Assembly targetAssembly, string resourceName, ClientResourceDependencyLevel? dependencyLevel = null, bool? performSubstitution = null)
        {
            var resourceDependencyLevel = dependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Independent);

            EmbeddedClientResource clientResource = new EmbeddedClientResource(targetAssembly, resourceName, ClientResourceKind.Script, resourceDependencyLevel, performSubstitution);

            return Include(clientResource);
        }


        /// <summary>
        /// Defines the inline handler executed when the DOM document is ready (using the $(document).ready jQuery event)
        /// </summary>
        /// <param name="onDocumentReadyAction">The action defining the inline handler</param>
        /// <example>
        /// <code lang="CS">
        /// &lt;% Html.DJ().ScriptRegistry()
        ///           .OnDocumentReady(() =>
        ///           {
        ///             %&gt;
        ///             function() {
        ///                 alert("Document is ready");
        ///             }
        ///             &lt;%
        ///           });
        /// %&gt;
        /// </code>
        /// </example>
        public virtual ScriptRegistryBuilder OnDocumentReady(Action onDocumentReadyAction)
        {
            _scriptRegistry.OnDocumentReadyActions.Add(onDocumentReadyAction);
            return this;
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

        public virtual ScriptRegistryBuilder WithUnderscoreJS(bool enabled = true)
        {
            if (enabled)
                WithjQuery();

            RegisterFrameworkWebResource(EmbeddedResources.Js.Underscore, ClientResourceDependencyLevel.Global, enabled);

            return this;
        }

        public virtual ScriptRegistryBuilder WithUnobtrusiveJavaScript(bool enabled = true)
        {
            HtmlHelper.EnableUnobtrusiveJavaScript(enabled);

            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUnobtrusiveAjax, ClientResourceDependencyLevel.MidLevel, enabled);

            return this;
        }

        public virtual ScriptRegistryBuilder WithClientSideValidation(bool enabled = true)
        {
            HtmlHelper.EnableClientValidation(enabled);

            WithUnobtrusiveJavaScript(enabled);
            
            if(enabled)
                WithjQuery();

            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryValidate, ClientResourceDependencyLevel.MidLevel, enabled);
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryValidateUnobtrusive, ClientResourceDependencyLevel.MidLevel, enabled);

            return this;
        }

        public virtual ScriptRegistryBuilder WithjQuery(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQuery, ClientResourceDependencyLevel.Global, enabled);
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryJson, ClientResourceDependencyLevel.Global, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryUIEffects(bool enabled = true)
        {
            if (enabled)
                WithjQuery();

            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUICore, ClientResourceDependencyLevel.Global, enabled);
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUIEffects, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryUIInteractions(bool enabled = true)
        {
            if (enabled)
                WithjQuery();

            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUICore, ClientResourceDependencyLevel.Global, enabled);
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUIInteractions, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryUIWidgets(bool enabled = true)
        {
            if (enabled)
                WithjQuery();

            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUICore, ClientResourceDependencyLevel.Global, enabled);
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryUIWidgets, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithModernizer(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.Modernizer, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithHighCharts(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.Highcharts, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithDowJonesCommon(bool enabled = true)
        {
            if (enabled)
            {
                WithjQuery();
                WithUnderscoreJS();
                
            }

            RegisterFrameworkWebResource(EmbeddedResources.Js.DowJonesCommon, ClientResourceDependencyLevel.Global, enabled);

            // this should be registered after common
            if(enabled)
            {
                WithPubSub();
            }

            return this;
        }

        public virtual ScriptRegistryBuilder WithPubSub(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.PubSubManager, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithServiceProxy(bool enabled = true)
        {
            if (enabled)
            {
                WithUnderscoreJS();
                WithJson2();
                WithErrorManager();
            }

            RegisterFrameworkWebResource(EmbeddedResources.Js.ServiceProxy, ClientResourceDependencyLevel.Global, enabled: enabled, performSubstitution: true);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJson2(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.Json2, ClientResourceDependencyLevel.Global, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithErrorManager(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.ErrorManager, ClientResourceDependencyLevel.Global, enabled: enabled, performSubstitution: true);
            return this;
        }

        public virtual ScriptRegistryBuilder WithPubSubManager(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.PubSubManager, ClientResourceDependencyLevel.Global, enabled: enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryCycle(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryCycle, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithDateFormat(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.DateFormat, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }

        public virtual ScriptRegistryBuilder WithJQueryTouch(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.JQueryTouch, ClientResourceDependencyLevel.MidLevel, enabled);
            return this;
        }


        public virtual ScriptRegistryBuilder WithColorPicker(bool enabled = true)
        {
            RegisterFrameworkWebResource(EmbeddedResources.Js.ColorPicker, ClientResourceDependencyLevel.MidLevel, enabled);
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

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal ClientResource RegisterFrameworkWebResource(string resourceName, ClientResourceDependencyLevel? dependencyLevel, bool enabled, bool? performSubstitution = null)
        {
            var resourceDependencyLevel = dependencyLevel.GetValueOrDefault(ClientResourceDependencyLevel.Global);

            var clientResource = new EmbeddedClientResource(GetType().Assembly, resourceName, ClientResourceKind.Script, resourceDependencyLevel, performSubstitution);

            if (enabled)
                _scriptRegistry.Register(clientResource);
            else
                _scriptRegistry.Unregister(clientResource);

            return clientResource;
        }

        public IHtmlString Render()
        {
            return new HtmlString(_scriptRegistryWriter.Render(this));
        }

        public void Render(HtmlTextWriter writer)
        {
            _scriptRegistryWriter.Render(this, writer);
        }

        public IHtmlString RenderHead()
        {
            return new HtmlString(_scriptRegistryWriter.RenderHead(this));
        }


        [Obsolete("ToHtml() is no longer supported - use the explicit RenderHead() and Render() methods instead")]
        public IHtmlString ToHtml()
        {
            throw new NotSupportedException("ToHtml() is no longer supported - use the explicit RenderHead() and Render() methods instead");
        }

        public static implicit operator ScriptRegistry(ScriptRegistryBuilder builder)
        {
            return builder._scriptRegistry as ScriptRegistry;
        }
    }
}
