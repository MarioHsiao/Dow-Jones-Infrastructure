using Factiva.Gateway.Messages.Assets.Common.V2_0;

namespace EMG.widgets.ui.delegates.core.manualNewsletterWorkspace
{
    /// <summary>
    /// 
    /// </summary>
    public class ManualNewsletterWorkspaceInfo
    {
        /// <summary>
        /// id of the alert.
        /// </summary>
        public int Id;

        /// <summary>
        /// name of alert
        /// </summary>
        public string Name;

        /// <summary>
        /// headline count
        /// </summary>
        public int Count;

        /// <summary>
        /// headlines
        /// </summary>
        public ManualNewsletterWorkspaceSection[] Sections;

        /// <summary>
        /// Status
        /// </summary>
        public Status Status;

        /// <summary>
        /// View All Uri
        /// </summary>
        public string ViewAllUri;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ManualNewsletterWorkspaceSection
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;

        /// <summary>
        /// 
        /// </summary>
        public int Id;


        /// <summary>
        /// 
        /// </summary>
        public HeadlineInfo[] Headlines;
    }
}
