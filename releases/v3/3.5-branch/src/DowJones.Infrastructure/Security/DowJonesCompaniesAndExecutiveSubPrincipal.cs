using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace EMG.Utility.Security
{
    /// <summary>
    /// This uses the Interface piece of the Authorization Component
    /// </summary>
    public class DowJonesCoAndExecSubPrincipal : ISubPrincipal
    {
        
        #region ISubPrincipal Members

        internal DowJonesCoAndExecSubPrincipal()
        {
        }

        internal DowJonesCoAndExecSubPrincipal(MatrixInterfaceService service)
        {
            Initialize(service);
        }

        public void Initialize(AuthorizationComponent component)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
