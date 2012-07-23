using DowJones.Charting.Manager;

namespace DowJones.Charting.Core
{
    public interface IEmbeddedHTMLResponse
    {
        int Height { get;}
        int Width { get; }
        OutputChartType ChartType { get;}
        string EmbeddingHTML { get; }
    }
}
