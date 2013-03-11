using System.Collections.Generic;
using factiva.nextgen;

namespace EMG.widgets.ui.delegates.interfaces
{
    internal interface IWidgetPreviewDelegate
    {
        /// <summary>
        /// Fills the preview for a specified list of assets
        /// </summary>
        /// <param name="assetIds">The asset ids.</param>
        void FillPreview(List<int> assetIds);

        /// <summary>
        /// Fills the preview for a specified widgetId.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        void FillPreview(string widgetId);
    }
}