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

        [Inject("Constructor injection not available in Action Filter Attributes")]
        public IUserSession Session { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Log.Debug("Check for valid session");

            if (Session.Validate())
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
