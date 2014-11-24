namespace EMG.widgets.ui.delegates.core
{
    /// <summary>
    /// Alert Headline Widget Definition class.
    /// </summary>
    public class AlertHeadlineWidgetDefinition : HeadlineWidgetDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlertHeadlineWidgetDefinition"/> class.
        /// </summary>
        public AlertHeadlineWidgetDefinition()
        {
            AuthenticationCredentials = new AuthenticationCredentials();
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] alertIds;
    }
}