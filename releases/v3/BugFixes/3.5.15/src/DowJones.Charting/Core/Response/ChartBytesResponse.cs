using System;
using DowJones.Charting.Manager;

namespace DowJones.Charting.Core.Response
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class ChartBytesResponse : IBytesResponse
    {
        private readonly int _height;
        private readonly int _width;
        private readonly byte[] _bytes;
        private readonly OutputChartType _chartType = OutputChartType.FLASH;

        internal ChartBytesResponse(int height, int width, byte[] bytes, OutputChartType chartType)
        {
            _height = height;
            _width = width;
            _bytes = bytes;
            _chartType = chartType;
        }

        #region DiscoveryChartBytesResponse Members

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

        public byte[] Bytes
        {
            get { return _bytes; }
        }
        #endregion
    }
}
