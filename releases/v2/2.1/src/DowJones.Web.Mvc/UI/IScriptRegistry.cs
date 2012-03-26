using System;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI
{
    /// <summary>
    /// Registry to keep track of all stylesheets required
    /// for an individual request
    /// </summary>
    public interface IScriptRegistry : IClientResourceRegistry
    {
        /// <summary>
        /// Gets the on document ready actions.
        /// </summary>
        /// <value>The on page load actions.</value>
        IList<Action> OnDocumentReadyActions { get; }

        /// <summary>
        /// Gets the on document ready statements that is used in <code>RenderAction</code>.
        /// </summary>
        /// <value>The on page load actions.</value>
        IList<string> OnDocumentReadyStatements { get; }

        /// <summary>
        /// Gets the on window unload actions.
        /// </summary>
        /// <value>The on page unload actions.</value>
        IList<Action> OnWindowUnloadActions { get; }

        /// <summary>
        /// Gets the on window unload statements.that is used in <code>RenderAction</code>.
        /// </summary>
        /// <value>The on page load actions.</value>
        IList<string> OnWindowUnloadStatements { get; }

        /// <summary>
        /// Get the registered script resources
        /// </summary>
        IEnumerable<ClientResource> GetScripts();

        /// <summary>
        /// Get the registered client templates
        /// </summary>
        IEnumerable<ClientResource> GetClientTemplates();
    }
}