using DowJones.Charting.Manager;

namespace DowJones.Charting.Core.Response
{
    public class ChartUriResponse : IUriResponse
    {
        private readonly int _height;
        private readonly int _width;
        private readonly string _uri;
        private readonly OutputChartType _chartType = OutputChartType.FLASH;

        internal ChartUriResponse(int height, int width, string uri, OutputChartType chartType)
        {
            _height = height;
            _width = width;
            _uri = uri;
            _chartType = chartType;
        }

        #region IUriResponse Members

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

        public string Uri
        {
            get { return _uri; }
        }
        #endregion
    }
}
