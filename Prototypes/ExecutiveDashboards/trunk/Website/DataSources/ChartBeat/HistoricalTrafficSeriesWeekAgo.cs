namespace DowJones.Dash.Website.DataSources
{
    public class HistorialTrafficSeriesWeekAgo : ChartBeatDataSource
    {
        public HistorialTrafficSeriesWeekAgo()
            : base("/historical/traffic/series/")
        {
            Parameters.Add("frequency", "20");
            Parameters.Add("days_ago", "7");
            Parameters.Add("limit", "288");
        }
    }
}