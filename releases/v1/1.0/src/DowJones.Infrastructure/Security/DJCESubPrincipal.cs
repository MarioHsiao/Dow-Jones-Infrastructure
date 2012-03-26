using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Utilities.Security
{
    /// <summary>
    /// This uses the Interface piece of the Authorization Component
    /// </summary>
    public class DJCESubPrincipal : ISubPrincipal
    {
        public bool IsRadar { get; private set; }

        public bool IsLists { get; private set; }

        public bool IsSales { get; private set; }

        public bool IsResearchPlus { get; private set; }

        public bool IsResearch { get; private set; }

        internal DJCESubPrincipal()
        {
        }

        internal DJCESubPrincipal(MatrixInterfaceService service)
        {
            Initialize(service);
        }

        internal void Initialize(MatrixInterfaceService service)
        {

            if (service == null || service.ac3.Count  != 1)
            {
                return;
            }
            switch (service.ac3[0].ToUpper())
            {
                case "3":
                case "4":
                    IsResearch = true;
                    break;
                case "5":
                    IsRadar = true;
                    break;                
                case "6":
                    IsLists = true;
                    break;
                case "7":
                    IsSales = true;
                    break;
                case "8":
                    IsResearchPlus = true;
                    break;
            }
        }

        #region ISubPrincipal Members

        public void Initialize(AuthorizationComponent component)
        {
            Initialize((MatrixInterfaceService)component);
        }

        #endregion
    }
}
