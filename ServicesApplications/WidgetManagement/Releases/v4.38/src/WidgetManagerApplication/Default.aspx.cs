using System;
using factiva.nextgen;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.page;

namespace EMG.widgets.ui
{
    /// <summary>
    /// default page in current site. Sits at the root of the siate
    /// </summary>
    public class _Default : BasePage
    {
        /// <summary>
        /// Handles the PreInit event of the Pate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Pate_PreInit(object sender, EventArgs e)
        {
            SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;
        }

        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            WidgetManagementDTO widgetManagementDTO = (WidgetManagementDTO)FormState.Accept(typeof(WidgetManagementDTO), true);
           
            SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;
            if (widgetManagementDTO.IsValid())
            {
                Response.Redirect(widgetManagementDTO.GetRedirectionUrl(), true);
            }
            else
            {
                widgetManagementDTO.action = WidgetManagementAction.List;
                widgetManagementDTO.assetIds = null;
                Response.Redirect(widgetManagementDTO.GetRedirectionUrl(), true);
            }
        }
    }
}