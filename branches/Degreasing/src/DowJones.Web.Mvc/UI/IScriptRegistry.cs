using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI
{
    /// <summary>
    /// Registry to keep track of all scripts required
    /// for an individual request
    /// </summary>
    public interface IScriptRegistry : IClientResourceRegistry
    {
        /// <summary>
        /// Gets the on document ready statements
        /// </summary>
        /// <value>The on page load actions.</value>
        IList<string> OnDocumentReadyStatements { get; }

        /// <summary>
        /// Gets the on window unload statements
        /// </summary>
        /// <value>The on page load actions.</value>
        IList<string> OnWindowUnloadStatements { get; }

        /// <summary>
        /// Gets the component cleanup statements
        /// </summary>
        IEnumerable<string> GetCleanupStatements();

        /// <summary>
        /// Get the registered client templates
        /// </summary>
        IEnumerable<ClientResource> GetClientTemplates();

        /// <summary>
        /// Gets the component initialization statements
        /// </summary>
        IEnumerable<string> GetInitializationStatements();

        /// <summary>
        /// Get the registered script resources
        /// </summary>
        IEnumerable<ClientResource> GetScripts();
    }
}