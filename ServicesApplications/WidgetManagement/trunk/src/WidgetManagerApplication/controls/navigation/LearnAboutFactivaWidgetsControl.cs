using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.Properties;

namespace EMG.widgets.ui.controls.navigation
{
    internal class FlashObjectDelegate
    {
        public string Source;
        public string Width;
        public string Height;
        public string TargetFlashPlayerVersion;
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
                                          Source = Settings.Default.MarketingFlashMovie_Source, 
                                          Width = Settings.Default.MarketingFlashMovie_Width, 
                                          Height = Settings.Default.MarketingFlashMovie_Height, 
                                          TargetFlashPlayerVersion = Settings.Default.MarketingFlashMovie_TargetVersion
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
