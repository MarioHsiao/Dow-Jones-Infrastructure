
namespace EMG.widgets.ui.delegates.core.automaticWorkspace
{
    /// <summary>
    /// 
    /// </summary>
	public class AutomaticWorkspaceInfo
	{
        /// <summary>
        /// id of the alert.
        /// </summary>
        public long Id;

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
        public HeadlineInfo[] Headlines;

        /// <summary>
        /// Status
        /// </summary>
        public Status Status;

        /// <summary>
        /// View All Uri
        /// </summary>
        public string ViewAllUri;
	}
}
