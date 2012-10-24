using DowJones.Infrastructure;

namespace DowJones.Dash.Website.App_Start
{
    public class InitializationTask : IBootstrapperTask
    {
        private readonly HubClientConnection _clientConnection;

        public InitializationTask(HubClientConnection clientConnection)
        {
            _clientConnection = clientConnection;
        }

        #region IBootstrapperTask Members

        public void Execute()
        {
            _clientConnection.Start();
        }

        #endregion
    }
}