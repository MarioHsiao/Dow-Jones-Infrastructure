using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Session;
using DowJones.Web.Mvc.ActionResults;
using log4net;

namespace DowJones.Web.Mvc.ActionFilters
{
    public class RequireAuthenticationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (RequireAuthenticationAttribute));
        private IUserSession _session;

        [Inject("Constructor injection not available in Action Filter Attributes")]
        public IUserSession Session
        {
            get { return _session; }

            set
            {
                if (_session == null)
                {
                    _session = value;
                }
            }
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Log.Debug("Check for valid session");
            Log.DebugFormat("ProxySession: {0}", Session.IsProxySession);
            Log.DebugFormat("SessionId: {0}", Session.SessionId);

            if (Session.IsValid())
            {
                Log.Debug("Valid session");
                return;
            }

            Log.Debug("Invalid session - redirecting to login server");

            filterContext.Result =
                new AuthServerLoginRedirectActionResult
                    {
                        InterfaceLanguage = Session.InterfaceLanguage.ToString()
                    };
        }
    }
}
