using DowJones.Tools.Managers.Charting;

namespace DowJones.Tools.Charting
{
    public interface IUriResponse
    {
        int Height { get;}
        int Width { get; }
        OutputChartType ChartType { get;}
        string Uri { get; }
    }
}
