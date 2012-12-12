using DowJones.Dash.Website.Connections;
using DowJones.Infrastructure;

namespace DowJones.Dash.Website.App_Start
{
    public class InitializationTask : IBootstrapperTask
    {
       #region IBootstrapperTask Members

        public void Execute()
        {
            DataSourcesServiceConnection.Instance.Start();
        }

        #endregion
    }
}