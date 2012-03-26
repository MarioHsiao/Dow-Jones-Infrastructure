using System.Net;
using System.Web;
using System.Web.Mvc;
using Orchard.Security;
using Orchard.Themes;
using System.Configuration;
using System.Diagnostics;

namespace DowJones.Orchard.SingleSignOn
{
    [Themed]
    public class SingleSignOnController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly IAuthenticationService _authenticationService;

        public SingleSignOnController(
                IMembershipService membershipService,
                IAuthenticationService authenticationService
            )
        {
            _membershipService = membershipService;
            _authenticationService = authenticationService;
        }

        public ActionResult AccessDenied(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                return View("AccessDenied");
            }

            if (TryAuthenticateUser())
                return Redirect(ResolveRedirectUrl(returnUrl));
            else
                return LogOn(returnUrl);
        }

        public ActionResult Authenticate(string redirectUrl)
        {
            if (TryAuthenticateUser())
                return Redirect(ResolveRedirectUrl(redirectUrl));
            else
                return View("InvalidUser");
        }

        public ActionResult ChangePassword( )
        {
            return Redirect("~/");
        }

        public ActionResult LogOff(string returnUrl)
        {
            _authenticationService.SignOut();

            return Redirect(ConfigurationManager.AppSettings["LogoutUrl"]);
           // return Redirect(ResolveRedirectUrl(returnUrl));
        }

        public ActionResult LogOn(string redirectUrl)
        {
            //TextWriterTraceListener myListener = new TextWriterTraceListener("trace.log", "myListener");           
            //Trace.TraceInformation("enter the Login code");
            //Trace.Flush(); 
            
            if (TryAuthenticateUser())
            {
                return Redirect(ResolveRedirectUrl(redirectUrl));
            }

            var authenticateUrl = 
                Url.Action("Authenticate",
                           new { redirectUrl = ResolveRedirectUrl(redirectUrl) });

            var form = new LoginServerForm(Request, ResolveExternalUrl(authenticateUrl));

            return Content(form.ToHtmlString());
        }


        private bool TryAuthenticateUser()
        {
            if (User.Identity.IsAuthenticated)
                return true;

            return TryAuthenticateSsoToken();
        }

        private bool TryAuthenticateSsoToken()
        {
            var loginCookie = Request.Cookies["GSLogin"];
            //TextWriterTraceListener myListener = new TextWriterTraceListener("trace.log", "myListener");
            //Trace.TraceInformation("cookie" + loginCookie);
            //Trace.Flush(); 

            if (loginCookie == null)
            {
                HttpContext.Trace.Write("Authentication", "Invalid Gateway login cookie");
                return false;
            }

            string prefix = ConfigurationManager.AppSettings["DefaultProductPrefix"]; 
            var username = loginCookie[prefix + "%5FU"];
            var @namespace = loginCookie[prefix + "%5FN"];
            
            //Trace.TraceInformation("Login information username:" + username + "ns:" + @namespace);
            //Trace.Flush(); 
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(@namespace))
            {
                HttpContext.Trace.Write("Authentication", "Invalid username or namespace");
                //Trace.TraceInformation("Authentication", "Invalid username or namespace");
                //Trace.Flush();
                return false;
            }

            if (IsValidSession(loginCookie[prefix + "%5FS"]) == false)
            {
                HttpContext.Trace.Write("Authentication", "Invalid session");
                //Trace.TraceInformation("Authentication", "Invalid session" + loginCookie[prefix + "%5FS"]);
                //Trace.Flush();
                return false;
            }

            var userId = new FactivaUserId(username, @namespace);

            IUser user = _membershipService.GetUser(userId);

            // If there is no user registered for this user 
            // (i.e. it's the first time they've tried to access the site)
            // create a new user for them
            if(user == null)
                   user = _membershipService.CreateUser(new CreateFactivaUser(userId));

            _authenticationService.SignIn(user, false);

            return true;
        }

        private bool IsValidSession(string sessionId)
        {
            // TODO: Call gateway to validate session ID to avoid hacking!

            HttpContext.Trace.Write("Authentication", "Skipping validation check for session " + sessionId);
            //Trace.TraceInformation("Authentication", "Skipping validation check for session " + sessionId);
            //Trace.Flush();
            
            return true;
        }

        private string ResolveExternalUrl(string url)
        {
            if (!url.StartsWith("http"))
            {
                return string.Format("{0}://{1}{2}",
                                     Request.Url.Scheme,
                                     Request.Url.Authority,
                                     VirtualPathUtility.ToAbsolute(url));
            }

            return url;
        }

        private static string ResolveRedirectUrl(string redirectUrl)
        {
            return string.IsNullOrWhiteSpace(redirectUrl) ? "~/" : redirectUrl;
        }
    }
}