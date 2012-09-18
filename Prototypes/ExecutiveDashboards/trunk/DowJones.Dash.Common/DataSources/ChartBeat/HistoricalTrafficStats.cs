namespace DowJones.Dash.DataSources.ChartBeat
{
    public class HistoricalTrafficStats : ChartBeatDataSource
    {
        public HistoricalTrafficStats() : base("/historical/traffic/stats/")
        {
            Parameters.Add("fields", "srvload,people,srvload");
            Parameters.Add("properties", "min,max,avg");
        }
    }
}