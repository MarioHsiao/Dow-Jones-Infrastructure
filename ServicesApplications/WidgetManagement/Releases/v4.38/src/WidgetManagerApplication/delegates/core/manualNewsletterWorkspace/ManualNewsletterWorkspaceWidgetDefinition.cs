
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;

namespace EMG.widgets.ui.delegates.core.manualNewsletterWorkspace
{
    /// <summary>
    /// 
    /// </summary>
    public class ManualNewsletterWorkspaceWidgetDefinition : WidgetDefinition
    {
        /// <summary>
        /// Manual Workspace Id
        /// </summary>
        public int ManualWorkspaceId;

        /// <summary>
        /// Number of Items per section.
        /// </summary>
        public int NumItemsPerSection = 3;

        /// <summary>
        /// Display Type for widget.
        /// </summary>
        public WidgetHeadlineDisplayType DisplayType = WidgetHeadlineDisplayType.HeadlinesWithSnippets;

    }
}
