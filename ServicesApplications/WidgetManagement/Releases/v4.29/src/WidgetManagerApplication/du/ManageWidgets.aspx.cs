using System;
using factiva.nextgen;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.exception;
using EMG.widgets.ui.page;

namespace EMG.widgets.ui.du
{
    /// <summary>
    ///     Direct url processing page for creation of widgets:
    ///     <para>Base URL: host/du/createWidget.aspx</para> 
    ///     <list type="bullet">
    ///         <item><term>Author</term><description>David Da Costa</description></item>
    ///         <item><term>Base Url</term><description>host/du/manageWidgets.aspx</description></item>
    ///         <item><term>Usage</term><description>Used by various products as an entry point into the Widgets Management page.</description></item>
    ///     </list>
    ///	    <list type="table">
    ///         <listheader><term>Parameters:</term></listheader>
    ///         <item>
    ///             <term>SA_FROM</term>
    ///             <description>
    ///                 Code representation of cookies used for session authentication [optional].
    ///                 <list type="bullet">
    ///                     <listheader><term>Accepted Values:</term></listheader>
    ///                     <item><term>GL</term><description>global.factiva.com</description></item>
    ///                     <item><term>IF</term><description>preview.factiva.com</description></item>
    ///                 </list>
    ///                 <para>Default value: IF</para>
    ///             </description>
    ///         </item>
    ///     </list> 
    ///     <list type="table">
    ///         <listheader><term>Valid Urls</term></listheader>
    ///         <item>
    ///             <term>Production</term>
    ///             <description>http://widgets.factiva.com/du/manageWidgets.aspx?SA_FROM=GL</description>
    ///         </item>
    ///         <item>
    ///             <term>Beta</term>
    ///             <description>http://widgets.beta.factiva.com/du/manageWidgets.aspx?SA_FROM=GL	</description>
    ///         </item>
    ///         <item>
    ///             <term>Integration</term>
    ///             <description>http://widgets.int.factiva.com/du/manageWidgets.aspx?SA_FROM=GL	</description>
    ///         </item>
    ///         <item>
    ///             <term>Development</term>
    ///             <description>http://nevada.dev.us.factiva.com/widgets/du/manageWidgets.aspx?SA_FROM=GL</description>
    ///         </item>
    ///     </list>
    /// </summary>
    public class ManageWidgets : BasePage
    {

        private EmgWidgetsUIException m_Exception;
        /// <summary>
        /// Initializes a new instance of the <see cref="ManageWidgets"/> class.
        /// </summary>
        public ManageWidgets()
        {
            OverrideDefaultPrefix = true;
            // Do not validate let the next page do the work.
            base.ValidateSessionId = false;
        }

        
        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            WidgetManagementDTO widgetManagementDTO = (WidgetManagementDTO)FormState.Accept(typeof(WidgetManagementDTO), true);
            widgetManagementDTO.action = WidgetManagementAction.List;
            widgetManagementDTO.assetIds = null;

            SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;
            if (widgetManagementDTO.IsValid())
            {
                Response.Redirect(widgetManagementDTO.GetRedirectionUrl(), true);
            }
            else
            {
                m_Exception = new EmgWidgetsUIException(EmgWidgetsUIException.INVALID_DIRECT_URL_WIDGET_DTO);
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <exception cref="EmgWidgetsUIException">Thrown when and exception has been captured from the Page_Init event.</exception>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (m_Exception != null)
            {
                throw m_Exception;
            }
        }
    }
}
