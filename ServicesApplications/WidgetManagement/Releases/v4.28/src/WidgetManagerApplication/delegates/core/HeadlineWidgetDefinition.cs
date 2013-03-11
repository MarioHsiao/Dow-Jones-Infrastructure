using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;

namespace EMG.widgets.ui.delegates.core
{
    /// <summary>
    /// Headline Widget Definition
    /// </summary>
    public class HeadlineWidgetDefinition : WidgetDefinition
    {
        /// <summary>
        /// Display Type for widget.
        /// </summary>
        public WidgetHeadlineDisplayType DisplayType = WidgetHeadlineDisplayType.HeadlinesWithSnippets;
        

        /// <summary>
        /// Number of headlines to show.
        /// </summary>
        public int NumOfHeadlines = 3;
    }
}