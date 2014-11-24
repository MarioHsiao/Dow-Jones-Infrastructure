using EMG.widgets.ui.delegates.core.discovery;
using Factiva.Gateway.Messages.Track.V1_0;
using EMG.widgets.ui.delegates.core;

namespace EMG.widgets.ui.delegates.core.alertHeadline
{
    /// <summary>
    /// Alert Information 
    /// </summary>
    public class AlertInfo
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
        /// external access token.
        /// </summary>
        public string ExternalAccessToken;

        /// <summary>
        /// headline count
        /// </summary>
        public int HeadlineCount;

        /// <summary>
        /// headlines of the alerts
        /// </summary>
        public AlertHeadlineInfo[] Headlines;

        /// <summary>
        /// Chart information for Companies
        /// </summary>
        public DiscoveryInfo CompaniesChart;

        /// <summary>
        /// Chart information for Industries
        /// </summary>
        public DiscoveryInfo IndustriesChart;

        /// <summary>
        /// Chart information for Subjects
        /// </summary>
        public DiscoveryInfo SubjectsChart;

        /// <summary>
        /// Chart information for Executives
        /// </summary>
        public DiscoveryInfo ExecutivesChart;

        /// <summary>
        /// Chart information for Regions
        /// </summary>
        public DiscoveryInfo RegionsChart;

        /// <summary>
        /// Status
        /// </summary>
        public Status Status;

        /// <summary>
        /// Status
        /// </summary>
        public ProductType Type;

        /// <summary>
        /// View All Uri
        /// </summary>
        public string ViewAllUri;

        /// <summary>
        /// Group folder flag
        /// </summary>
        public bool IsGroupFolder;
    }
}