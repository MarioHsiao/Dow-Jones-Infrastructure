namespace DowJones.Dash.Caching
{
    public class DashboardMessage
    {
        public object Data { get; set; }
        public string DataSource { get; set; }

        public DashboardMessage(string dataSource = null, object data = null)
        {
            Data = data;
            DataSource = dataSource;
        }
    }
}