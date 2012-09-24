using System;
using DowJones.Dash.DataSources;

namespace DowJones.Dash.Website
{
    public partial class MvcApplication
    {
        public static Lazy<string> Version =
            new Lazy<string>(typeof(MvcApplication).Assembly.GetName().Version.ToString);

        protected override void OnApplicationStopped()
        {
            // Clean up outstanding polling tasks
            PollingDataSource.GlobalCancellationTokenSource.Cancel(false);
        }
    }
}