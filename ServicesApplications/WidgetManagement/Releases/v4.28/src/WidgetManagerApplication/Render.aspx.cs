using System;
using System.Web.UI;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using factiva.nextgen;
using EMG.widgets.ui.page;

namespace EMG.widgets.ui
{
    /// <summary>
    /// 
    /// </summary>
    public class Render : BasePage
    {
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
            string testID = (Request["testID"]) ?? Request["testID"];

            if (!string.IsNullOrEmpty(testID))
            {
                testID = testID.Trim();
                WidgetManager widgetManager = new WidgetManager(SessionData.SessionBasedControlDataEx, SessionData.InterfaceLanguage);
                Widget widget = widgetManager.GetCachedWidgetById(testID);
                if (widget != null)
                {
                    ContentMain.Controls.Add(new LiteralControl("Success"));
                }
            }
            else ContentMain.Controls.Add(new LiteralControl("No Widget"));
        }
    }
}