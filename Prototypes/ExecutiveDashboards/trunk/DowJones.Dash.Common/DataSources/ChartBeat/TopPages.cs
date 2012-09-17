namespace DowJones.Dash.DataSources.ChartBeat
{
    public class TopPages : ChartBeatDataSource
    {
        public TopPages() : base("/toppages")
        {
            Parameters.Add("limit", 10);
        }
    }
}