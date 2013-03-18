using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.UI
{
    public interface IViewComponent : IRenderable, IHtmlString
    {
        IEnumerable<IViewComponent> Children { get; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        string ClientID { get; set; }

        /// <summary>
        /// The Client Resources (scripts and stylesheets) 
        /// registered with this component
        /// </summary>
        IEnumerable<ClientResource> ClientResources { get; }

        /// <summary>
        /// The component's CSS class (the "class" DOM property)
        /// </summary>
        string CssClass { get; set; }

        /// <summary>
        /// The Html Helper
        /// </summary>
        HtmlHelper Html { get; set; }

        /// <summary>
        /// Gets the HTML attributes.
        /// </summary>
        /// <value>The HTML attributes.</value>
        IDictionary<string, object> HtmlAttributes { get; }

        /// <summary>
        /// The component's model
        /// </summary>
        dynamic Model { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        ///  The component's parent component
        /// </summary>
        IViewComponent Parent { get; set; }

        /// <summary>
        ///  The component's view context
        /// </summary>
        ViewContext ViewContext { get; }

        void AddChild(IViewComponent child);

        void EnsureClientID();

        void RemoveChild(IViewComponent child);

        /// <summary>
        /// Writes the initialization script.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void WriteInitializationScript(TextWriter writer);

        /// <summary>
        /// Writes the cleanup script.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void WriteCleanupScript(TextWriter writer);
    }
}