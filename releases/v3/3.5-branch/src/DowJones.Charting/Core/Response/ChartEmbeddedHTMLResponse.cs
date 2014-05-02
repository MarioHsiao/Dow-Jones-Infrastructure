using System;
using DowJones.Charting.Manager;

namespace DowJones.Charting.Core.Response
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class ChartEmbeddedHTMLResponse : IEmbeddedHTMLResponse
    {
        private readonly int _height;
        private readonly int _width;
        private readonly string _embeddingHtml;
        private readonly OutputChartType _chartType = OutputChartType.FLASH;

        internal ChartEmbeddedHTMLResponse(int height, int width, string embeddingHTML, OutputChartType chartType)
        {
            _height = height;
            _width = width;
            _embeddingHtml = embeddingHTML;
            _chartType = chartType;
        }

        #region IEmbeddedHTMLResponse Members

        public int Height
        {
            get { return _height; }
        }

        public int Width
        {
            get { return _width; }
        }

        public OutputChartType ChartType
        {
            get { return _chartType; }
        }

        public string EmbeddingHTML
        {
            get { return _embeddingHtml; }
        }
        #endregion
    }
}
