using System.Web.UI;
using EMG.widgets.ui.dto.request;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;

namespace EMG.widgets.ui.controls.gallery
{
    /// <summary>
    /// 
    /// </summary>
    public class WidgetGalleryWelcomeControl : BaseControl
    {
        private WidgetManagementDTO m_WidgetManagementDTO = null;
        /// <summary>
        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return m_WidgetManagementDTO; }
            set { m_WidgetManagementDTO = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetGalleryWelcomeControl"/> class.
        /// </summary>
        public WidgetGalleryWelcomeControl()
        {
            ID = "widgetGalleryWelcomeCntrl";
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (m_WidgetManagementDTO != null &&
                m_WidgetManagementDTO.IsValid() &&
                m_WidgetManagementDTO.IsValid(m_WidgetManagementDTO.doneUrl))
            {
                // Label
                Controls.Add(new LiteralControl("<span class=\"widgetGalleryWelcome\">" + ResourceText.GetInstance.GetString("widgetGalleryWelcome") + "</span>"));

                // Render controls
                base.Render(writer);
            }
        }

    }
}
