namespace DowJones.Dash.DataSources.ChartBeat
{
    public class HistorialTrafficSeriesWeekAgo : ChartBeatDataSource
    {
        public HistorialTrafficSeriesWeekAgo()
            : base("/historical/traffic/series/")
        {
            Parameters.Add("frequency", "5");
            Parameters.Add("days_ago", "7");
            Parameters.Add("limit", "288");
        }
    }
}