using DowJones.Charting.Manager;

namespace DowJones.Charting.Core
{
    public interface IUriResponse
    {
        int Height { get;}
        int Width { get; }
        OutputChartType ChartType { get;}
        string Uri { get; }
    }
}
