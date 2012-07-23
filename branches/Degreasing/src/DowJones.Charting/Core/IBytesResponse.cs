using DowJones.Charting.Manager;

namespace DowJones.Charting.Core
{
    public interface IBytesResponse
    {
        int Height { get;}
        int Width { get; }
        OutputChartType ChartType { get;}
        byte[] Bytes { get; }
    }
}
