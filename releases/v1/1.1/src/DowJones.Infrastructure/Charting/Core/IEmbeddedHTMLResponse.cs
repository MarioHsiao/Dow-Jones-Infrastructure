using DowJones.Tools.Managers.Charting;

namespace DowJones.Tools.Charting
{
    public interface IEmbeddedHTMLResponse
    {
        int Height { get;}
        int Width { get; }
        OutputChartType ChartType { get;}
        string EmbeddingHTML { get; }
    }
}
