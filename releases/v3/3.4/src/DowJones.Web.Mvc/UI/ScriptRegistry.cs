using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Mvc.UI
{
    /// <summary>
    /// Registry to keep track of all stylesheets required
    /// for an individual request
    /// </summary>
    public class ScriptRegistry : ViewComponentClientResourceRegistry, IScriptRegistry
    {
        /// <summary>
        /// Gets the on document ready statements that is used in <code>RenderAction</code>.
        /// </summary>
        /// <value>The on page load actions.</value>
        public virtual IList<string> OnDocumentReadyStatements
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the on window unload statements.that is used in <code>RenderAction</code>.
        /// </summary>
        /// <value>The on page load actions.</value>
        public virtual IList<string> OnWindowUnloadStatements
        {
            get;
            private set;
        }

        protected IEnumerable<IViewComponent> RegisteredComponents
        {
            get
            {
                var registeredComponents = ComponentRegistry.GetRegisteredComponents().Reverse();
                return registeredComponents;
            }
        }


        public ScriptRegistry(IClientResourceManager globalResourceManager, IViewComponentRegistry componentRegistry)
            : base(globalResourceManager, componentRegistry)
        {
            OnDocumentReadyStatements = new List<string>();
            OnWindowUnloadStatements = new List<string>();
        }


        public IEnumerable<string> GetCleanupStatements()
        {
            foreach (var component in RegisteredComponents)
            {
                var buffer = new System.IO.StringWriter();
                component.WriteCleanupScript(buffer);
                yield return buffer.GetStringBuilder().ToString();
            }
        }

        public IEnumerable<ClientResource> GetClientTemplates()
        {
            var clientTemplates = base.GetResources(x => x.ResourceKind == ClientResourceKind.ClientTemplate);
            return clientTemplates;
        }

        public IEnumerable<string> GetInitializationStatements()
        {
            foreach (var component in RegisteredComponents)
            {
                var buffer = new System.IO.StringWriter();
                component.WriteInitializationScript(buffer);
                yield return buffer.GetStringBuilder().ToString();
            }
        }

        public IEnumerable<ClientResource> GetScripts()
        {
            var scripts = base.GetResources(x => x.ResourceKind == ClientResourceKind.Script);
            return scripts;
        }

        protected override void OnRegistered(ClientResource resource)
        {
            resource.ResourceKind = ClientResourceKind.Script;
        }
    }
}
