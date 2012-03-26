using System.Web.Mvc;
using DowJones.Web.Mvc.Properties;

namespace DowJones.Web.Mvc.ActionResults
{
    public class AuthenticationServerLogoutRedirectActionResult : RedirectResult
    {
        public AuthenticationServerLogoutRedirectActionResult(string url) 
            : base(url ?? Settings.Default.AuthenticationServiceLogoutUrl, false)
        {
        }
    }
}