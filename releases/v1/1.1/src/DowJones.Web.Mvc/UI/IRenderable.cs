using System.Web.UI;

namespace DowJones.Web.Mvc.UI
{
    public interface IRenderable
    {
        /// <summary>
        /// Renders the component.
        /// </summary>
        void Render();

        /// <summary>
        /// Renders the component.
        /// </summary>
        void Render(HtmlTextWriter writer);
    }
}