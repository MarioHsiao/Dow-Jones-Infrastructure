
namespace DowJones.Dash.DataSources.ChartBeat
{
    public class HistorialTrafficSeries : ChartBeatDataSource
    {
        public HistorialTrafficSeries()
            : base("/historical/traffic/series/")
        {
            Parameters.Add("frequency", "20");
        }
    }
}