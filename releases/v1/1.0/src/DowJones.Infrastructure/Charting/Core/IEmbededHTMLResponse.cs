using EMG.Tools.Managers.Charting;

namespace EMG.Tools.Charting
{
    public interface IEmbededHTMLResponse
    {
        int Height { get;}
        int Width { get; }
        OutputChartType ChartType { get;}
        string EmbededHTML { get; }
    }
}
