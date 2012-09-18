namespace DowJones.Dash.Website.DataSources
{
    public class HistoricalTrafficValues : ChartBeatDataSource
    {
        public HistoricalTrafficValues()
            : base("/historical/traffic/values/")
        {
            Parameters.Add("days_ago", "0");
            Parameters.Add("limit", "1");
            Parameters.Add("fields", "internal,search,links,direct,social");
        }
    }
}