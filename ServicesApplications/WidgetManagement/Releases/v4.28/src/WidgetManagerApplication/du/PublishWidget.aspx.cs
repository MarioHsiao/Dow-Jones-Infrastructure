using System;
using EMG.Utility.Exceptions;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.exception;
using EMG.widgets.ui.page;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using factiva.nextgen;

namespace EMG.widgets.ui.du
{
    /// <summary>
    /// 
    /// </summary>
    public class PublishWidget : BasePage
    {
        private WidgetManagementDTO m_WidgetManagementDTO;
        private EmgWidgetsUIException m_Exception = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="PublishWidget"/> class.
        /// </summary>
        public PublishWidget()
        {
            OverrideDefaultPrefix = true;
            // Do not validate let the next page do the work.
            base.ValidateSessionId = true;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            m_WidgetManagementDTO = (WidgetManagementDTO)FormState.Accept(typeof(WidgetManagementDTO), true);
            m_WidgetManagementDTO.action = WidgetManagementAction.Update;
            m_WidgetManagementDTO.secondaryAction = WidgetManagementSecondaryAction.Publish;

            UpdateWidgetManagementDTO();
            SessionData.SiteUserType = SiteUserType.FactivaWidgetBuilderUser;
            if (m_WidgetManagementDTO.IsValid())
            {
                Response.Redirect(m_WidgetManagementDTO.GetRedirectionUrl(), true);
            }
            else if (m_Exception == null)
            {
                m_Exception = new EmgWidgetsUIException(EmgWidgetsUIException.INVALID_DIRECT_URL_WIDGET_DTO);
            }
        }

        private void UpdateWidgetManagementDTO()
        {
            try
            {
                if (m_WidgetManagementDTO.assetIds != null &&
                    m_WidgetManagementDTO.assetIds.Length > 0 &&
                    m_WidgetManagementDTO.assetIds.Length == 1)
                {
                    var workspaceManager = new WorkspaceManager(SessionData.Instance().SessionBasedControlDataEx, SessionData.Instance().InterfaceLanguage);
                    // Find if there is a corresponding asset else perform a create
                    long temp;
                    if (workspaceManager.IsWorkspaceIdAssociatedWithDissemenatedWidget(m_WidgetManagementDTO.assetIds[0], out temp))
                    {
                        m_WidgetManagementDTO.widgetId = temp.ToString();
                        FormState.Remove(m_WidgetManagementDTO);
                        FormState.Add(m_WidgetManagementDTO);
                    }
                }
            }
            catch (FactivaBusinessLogicException fex)
            {
                m_Exception = new EmgWidgetsUIException(fex.ReturnCodeFromFactivaService);
            }
            catch (EmgUtilitiesException emgex)
            {
                m_Exception = new EmgWidgetsUIException(emgex.ReturnCode);
            }
            catch (Exception ex)
            {
                m_Exception = new EmgWidgetsUIException(ex, -1);
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
