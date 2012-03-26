using System.Web.Mvc;
using DowJones.Web.Mvc.ActionResults;

namespace DowJones.Web.Mvc.Controllers
{
    public abstract class AuthenticationControllerBase : ControllerBase
    {

        public ActionResult Login()
        {
            return new AuthServerLoginRedirectActionResult(ControllerContext);
        }

        public ActionResult Logout()
        {
            var logoutUrl = string.Format(
                "{0}?productname={1}", 
                    DowJones.Properties.Settings.Default.LogoutServerUrl,
                    DowJones.Properties.Settings.Default.DefaultProductName);

            return Redirect(logoutUrl);
        }

    }
}
