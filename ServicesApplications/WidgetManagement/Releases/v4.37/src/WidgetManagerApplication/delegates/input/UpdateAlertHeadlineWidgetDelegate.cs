using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using factiva.nextgen.ui.ajaxDelegates;

namespace EMG.widgets.ui.delegates.input
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateAlertHeadlineWidgetDelegate : UpdateAlertHeadlineWidgetDTO, IAjaxDelegate
    {
        #region IAjaxDelegate Members

        /// <summary>
        /// Gets or sets the return code.
        /// </summary>
        /// <value>The return code.</value>
        public long ReturnCode { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>The status message.</value>
        public string StatusMessage { get; set; }

        #endregion
    }
}
