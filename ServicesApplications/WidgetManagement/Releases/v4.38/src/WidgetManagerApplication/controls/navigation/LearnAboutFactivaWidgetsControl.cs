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
    /// FlashObjectDelegate : Note this must always be lowercase
    /// </summary>
    internal class FlashObjectDelegate
    {
        public string source;
        public string width;
        public string height;
        public string targetFlashPlayerVersion;
    }

    /// <summary>
    /// 
    /// </summary>
    public class LearnAboutFactivaWidgetsControl : BaseControl
    {
        private WidgetManagementDTO _widgetManagementDTO;
        /// <summary>
        /// Gets or sets the widget management DTO.
        /// </summary>
        /// <value>The widget management DTO.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return _widgetManagementDTO; }
            set { _widgetManagementDTO = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitWidgetBuilderControl"/> class.
        /// </summary>
        public LearnAboutFactivaWidgetsControl()
        {
            base.ID = "learnAboutFactivaWidgetsCntrl";
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (_widgetManagementDTO == null || !_widgetManagementDTO.IsValid() || !_widgetManagementDTO.IsValid(_widgetManagementDTO.doneUrl))
            {
                return;
            }

            // Create a Javascript FlashObjectDelegate
            var flashObjectDelegate = new FlashObjectDelegate
                                      {
                                          source = Settings.Default.MarketingFlashMovie_Source, 
                                          width = Settings.Default.MarketingFlashMovie_Width, 
                                          height = Settings.Default.MarketingFlashMovie_Height, 
                                          targetFlashPlayerVersion = Settings.Default.MarketingFlashMovie_TargetVersion
                                      };
            // Label 
            var serializer = new JavaScriptSerializer();
            Controls.Add(new LiteralControl("<script type=\"text/javascript\"> var movieData = " + serializer.Serialize(flashObjectDelegate) + ";</script>"));

            // Date link
            var anchor = new HtmlAnchor
                         {
                             ID = "movieAnchor"
                         };
            anchor.Title = anchor.InnerText = ResourceText.GetInstance.GetString("learnMoreAboutWidgets");
            anchor.HRef = "javascript:void(0)";
            anchor.Attributes.Add("onclick", "showMovie();return false;");
            Controls.Add(anchor);
                
                
            // Render controls
            base.Render(writer);
        }
    }
}
