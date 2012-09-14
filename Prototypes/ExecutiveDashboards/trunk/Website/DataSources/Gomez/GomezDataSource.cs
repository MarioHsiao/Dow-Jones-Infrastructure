namespace DowJones.Dash.Website.DataSources
{
    public abstract class GomezDataSource : SqlDataSource
    {
        private static string GomezConnectionString
        {
            get { return "Data Source=10.0.64.14;Initial Catalog=master;User Id=webuser;Password=webpwd;Asynchronous Processing=True"; }
        }

        public GomezDataSource(string query = null)
            : base(GomezConnectionString, query)
        {
        }
    }
}