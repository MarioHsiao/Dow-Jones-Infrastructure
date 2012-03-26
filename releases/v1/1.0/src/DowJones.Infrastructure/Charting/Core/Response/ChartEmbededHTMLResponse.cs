using EMG.Tools.Managers.Charting;

namespace EMG.Tools.Charting
{
    public class ChartEmbededHTMLResponse : IEmbededHTMLResponse
    {
        private readonly int height;
        private readonly int width;
        private readonly string embededHTML;
        private readonly OutputChartType chartType = OutputChartType.FLASH;

        internal ChartEmbededHTMLResponse(int height, int width, string embededHTML, OutputChartType chartType)
        {
            this.height = height;
            this.width = width;
            this.embededHTML = embededHTML;
            this.chartType = chartType;
        }

        #region IEmbededHTMLResponse Members

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public OutputChartType ChartType
        {
            get { return chartType; }
        }

        public string EmbededHTML
        {
            get { return embededHTML; }
        }
        #endregion
    }
}
