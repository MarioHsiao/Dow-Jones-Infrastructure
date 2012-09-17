namespace DowJones.Dash.DataSources.ChartBeat
{
    public class DashboardStats  : ChartBeatDataSource
    {
        public DashboardStats() : base("/dashapi/stats/")
        {
        }
    }
}