using System;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI
{
    /// <summary>
    /// Registry to keep track of all stylesheets required
    /// for an individual request
    /// </summary>
    public class ScriptRegistry : ViewComponentClientResourceRegistry, IScriptRegistry
    {
        /// <summary>
        /// Gets the on document ready actions.
        /// </summary>
        /// <value>The on page load actions.</value>
        public virtual IList<Action> OnDocumentReadyActions
        {
            get;
            private set;
        }

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
        /// Gets the on window unload actions.
        /// </summary>
        /// <value>The on page unload actions.</value>
        public virtual IList<Action> OnWindowUnloadActions
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


        public ScriptRegistry(IClientResourceManager globalResourceManager, IViewComponentRegistry componentRegistry)
            : base(globalResourceManager, componentRegistry)
        {
            OnDocumentReadyActions = new List<Action>();
            OnDocumentReadyStatements = new List<string>();
            OnWindowUnloadActions = new List<Action>();
            OnWindowUnloadStatements = new List<string>();
        }


        public IEnumerable<ClientResource> GetScripts()
        {
            var scripts = base.GetResources(x => x.ResourceKind == ClientResourceKind.Script);
            return scripts;
        }

        public IEnumerable<ClientResource> GetClientTemplates()
        {
            var clientTemplates = base.GetResources(x => x.ResourceKind == ClientResourceKind.ClientTemplate);
            return clientTemplates;
        }

        protected override void OnRegistered(ClientResource resource)
        {
            resource.ResourceKind = ClientResourceKind.Script;
        }
    }
}
