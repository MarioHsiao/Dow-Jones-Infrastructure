using System;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using EMG.Utility.Generators;
using factiva.nextgen;
using factiva.nextgen.ui.page;
using EMG.widgets.ui.dto.request;
using BasePage=EMG.widgets.ui.page.BasePage;

namespace EMG.widgets.ui.widgetManagement
{
    #if (DEBUG)
    /// <summary>
    /// 
    /// </summary>
    [ClientScript("~/js/designer.js", 0)]
#else
    /// <summary>
    /// 
    /// </summary>
    [ClientScript("~/jsa/designer.js", 0)]
#endif
    public partial class Manage : BasePage
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
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

    }
}
