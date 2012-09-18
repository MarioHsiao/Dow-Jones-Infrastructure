namespace DowJones.Dash.Website.DataSources
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