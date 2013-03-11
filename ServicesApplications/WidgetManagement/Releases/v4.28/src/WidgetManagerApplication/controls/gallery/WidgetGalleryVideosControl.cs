using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.Properties;

namespace EMG.widgets.ui.controls.navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class WidgetGalleryVideosControl : BaseControl
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
        /// Initializes a new instance of the <see cref="WidgetGalleryVideosControl"/> class.
        /// </summary>
        public WidgetGalleryVideosControl()
        {
            ID = "widgetGalleryVideosCntrl";
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
                Controls.Add(new LiteralControl("<span class=\"widgetGalleryDesc\">" + ResourceText.GetInstance.GetString("widgetGalleryDesc") + "</span>"));

                // Create a Javascript FlashObjectDelegate
                FlashObjectDelegate flashObjectDelegate = new FlashObjectDelegate();
                flashObjectDelegate.source = Settings.Default.MarketingFlashMovie_Source;
                flashObjectDelegate.width = Settings.Default.MarketingFlashMovie_Width;
                flashObjectDelegate.height = Settings.Default.MarketingFlashMovie_Height;
                flashObjectDelegate.targetFlashPlayerVersion = Settings.Default.MarketingFlashMovie_TargetVersion;
                
                // Label 
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Controls.Add(new LiteralControl("<script type=\"text/javascript\"> var movieData = " + serializer.Serialize(flashObjectDelegate) + ";</script>"));

                // Date link
                HtmlAnchor anchor = new HtmlAnchor();
                anchor.ID = "movieAnchor";
                anchor.Title = anchor.InnerText = ResourceText.GetInstance.GetString("fceSalesworksLearnMore");
                anchor.HRef = "javascript:void(0)";
                anchor.Attributes.Add("onclick", "showMovie();return false;");
                Controls.Add(anchor);

                // Render controls
                base.Render(writer);
            }
        }

    }
}
