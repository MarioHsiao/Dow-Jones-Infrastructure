using System;
using DowJones.Exceptions;
using DowJones.Security.Interfaces;

namespace DowJones.Assemblers.Security
{
    public class UninitializedPrinciple : IPrinciple
    {
        public IUserSubPrinciple UserServices
        {
            get { throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UninitializedPrincipleException); }
        }

        public ICoreServicesSubPrinciple CoreServices
        {
            get { throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UninitializedPrincipleException); }

        }

        public IRuleSet RuleSet
        {
            get { throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UninitializedPrincipleException); }

        }
    }
}