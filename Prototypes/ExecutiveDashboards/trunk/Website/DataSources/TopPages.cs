namespace DowJones.Dash.Website.DataSources
{
    public class TopPages : ChartBeatDataSource
    {
        public TopPages() : base("/toppages")
        {
            Parameters.Add("limit", 15);
        }
    }
}