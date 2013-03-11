using factiva.nextgen.ui.ajaxDelegates;

namespace EMG.widgets.ui.delegates.output
{
    /// <summary>
    /// 
    /// </summary>
    public class GetWidgetCodeResponseDelegate : AjaxDelegate
    {
        /// <summary>
        /// 
        /// </summary>
        public IntegrationEndPoint[] IntegrationEndPoints;

        /// <summary>
        /// 
        /// </summary>
        public IntegrationEndPoint[] RssIntegrationEndPoints;

        /// <summary>
        /// 
        /// </summary>
        public string RssIntegrationUrl; 

        /// <summary>
        /// 
        /// </summary>
        public string JavaSciptWidgetCodeSnippet;


        /// <summary>
        /// 
        /// </summary>
        public string JavaScriptCodeBaseUrl;

        /// <summary>
        /// 
        /// </summary>
        public string Token;
    }
}
