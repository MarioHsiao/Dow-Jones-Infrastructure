using System;

namespace DowJones.Dash.Caching
{
    public class DashboardErrorMessage : DashboardMessage
    {
        public DashboardErrorMessage(string dataSource, Exception exception = null)
            : base(dataSource, exception == null ? "Unknown error" : exception.Message)
        {
        }
    }
}