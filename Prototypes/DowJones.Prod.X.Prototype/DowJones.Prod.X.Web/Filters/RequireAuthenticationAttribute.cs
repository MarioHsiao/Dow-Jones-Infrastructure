using System;
using System.Web.Mvc;
using DowJones.Assemblers.Security;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Prod.X.Common.Exceptions;
using DowJones.Prod.X.Common.Extentions;
using DowJones.Security.Interfaces;
using DowJones.Session;
using log4net;

namespace DowJones.Prod.X.Web.Filters
{
    public class RequireAuthenticationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequireAuthenticationAttribute));
        private IPrinciple _principle;

        [Inject("Constructor injection not available in Action Filter Attributes")]
        public IUserSession Session { get; set; }

        [Inject("Constructor injection not available in Action Filter Attributes")]
        public IPrinciple Principle
        {
            get { return _principle; }

            set
            {
                _principle = value;
            }
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Log.Debug("Check for valid session");
            Log.DebugFormat("ProxySession: {0}", Session.IsProxySession);
            Log.DebugFormat("SessionId: {0}", Session.SessionId);

            Log.Debug("Check for valid session");

            if (Session == null)
            {
                Log.Debug("Problem injecting Session");
                throw new DowJonesUtilitiesException(Convert.ToInt64(ApplicationExceptionConstants.ErrNoSession));
            }

            if (!Session.IsValid())
            {
                Log.Debug("Session ID is empty");
                throw new DowJonesUtilitiesException(Convert.ToInt64(ApplicationExceptionConstants.ErrNoSession));
            }

            var unInitializedPrinciple = _principle as UninitializedPrinciple;

            // Check for valid principle
            if (unInitializedPrinciple != null) return;
            
            //Check for access to newsletters
            if (_principle.HasAccessToProductX()) return;

            Log.Debug("No access to newsletters");
            throw new DowJonesUtilitiesException(ApplicationExceptionConstants.NoAccessToNewsletters);
        }

        
    }
}