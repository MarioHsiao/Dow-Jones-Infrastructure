namespace DowJones.Dash.Website.DataSources
{
    public class QuickStats : ChartBeatDataSource
    {
        public QuickStats() : base("/live/quickstats/v3")
        {
        }
    }
}