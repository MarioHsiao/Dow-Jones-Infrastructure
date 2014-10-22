using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using factiva.nextgen.ui;
using factiva.nextgen.ui.controls;
using EMG.widgets.ui.dto.request;

namespace EMG.widgets.ui.controls.navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class ExitWidgetBuilderControl : BaseControl
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
        public ExitWidgetBuilderControl()
        {
           Attributes.Add("class","exitWidgetBuilderCntr");
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (m_WidgetManagementDTO != null &&
                m_WidgetManagementDTO.IsValid() && 
                m_WidgetManagementDTO.IsValid(m_WidgetManagementDTO.doneUrl))
            {
                LinkButton linkButton = new LinkButton();
                linkButton.CssClass = "button";
                linkButton.Text = string.Format("<span>{0}</span>", ResourceText.GetInstance.GetString("exitWidgetBuilder"));
                linkButton.OnClientClick = string.Format("location.href = '{0}';return false;",m_WidgetManagementDTO.doneUrl.Replace("\"","&quot;"));
                Controls.Add(linkButton);

                base.Render(writer);
            }
        }
    }
}
