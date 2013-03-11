using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using factiva.nextgen.ui.ajaxDelegates;

namespace EMG.widgets.ui.delegates.input
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateManaualNewsletterWorkspaceWidget : CreateWorkspaceWidgetDTO, IAjaxDelegate
    {
        private long m_ReturnCode;
        private string m_StatusMessage;

        #region IAjaxDelegate Members

        /// <summary>
        /// Gets or sets the return code.
        /// </summary>
        /// <value>The return code.</value>
        public long ReturnCode
        {
            get { return m_ReturnCode; }
            set { m_ReturnCode = value; }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>The status message.</value>
        public string StatusMessage
        {
            get { return m_StatusMessage; }
            set { m_StatusMessage = value; }
        }

        #endregion
    }
}