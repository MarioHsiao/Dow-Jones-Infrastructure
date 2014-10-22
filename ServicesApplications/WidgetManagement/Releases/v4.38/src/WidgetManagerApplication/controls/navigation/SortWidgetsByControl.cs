using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;
using EMG.widgets.ui.dto.request;

namespace EMG.widgets.ui.controls.navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class SortWidgetsByControl : BaseControl 
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
        /// Initializes a new instance of the <see cref="ExitWidgetBuilderControl"/> class.
        /// </summary>
        public SortWidgetsByControl()
        {
            ID = "sortWidgetsByCntr";
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
                Controls.Add(new LiteralControl("<script type=\"text/javascript\"> var w_sortBy = \"" + m_WidgetManagementDTO.widgetSortBy + "\";</script>"));

                // Label 
                Controls.Add(new LiteralControl("<span class=\"sortBy\">"  + ResourceText.GetInstance.GetString("sortBy") +  ":</span>"));

                // Date link
                var dateAnchor = new HtmlAnchor
                                 {
                                     ID = "dateAnchor"
                                 };
                dateAnchor.Title = dateAnchor.InnerText = ResourceText.GetInstance.GetString("date");
                dateAnchor.HRef = "javascript:void(0)";
                dateAnchor.Attributes.Add("onclick", "fireDateSort();return false;");
                Controls.Add(dateAnchor);
                
                // Pipe
                Controls.Add(new LiteralControl("<span class=\"pipe\">|</span>"));

                // name link
                var nameAnchor = new HtmlAnchor
                                 {
                                     ID = "nameAnchor"
                                 };
                nameAnchor.Title = nameAnchor.InnerText = ResourceText.GetInstance.GetString("name");
                nameAnchor.HRef = "javascript:void(0)";
                nameAnchor.Attributes.Add("onclick","fireNameSort();return false;");
                Controls.Add(nameAnchor);

                // Render controls
                base.Render(writer);
            }
        }
    }
}
