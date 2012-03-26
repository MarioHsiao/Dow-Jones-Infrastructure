using DowJones.Tools.Managers.Charting;

namespace DowJones.Tools.Charting
{
    public interface IBytesResponse
    {
        int Height { get;}
        int Width { get; }
        OutputChartType ChartType { get;}
        byte[] Bytes { get; }
    }
}
