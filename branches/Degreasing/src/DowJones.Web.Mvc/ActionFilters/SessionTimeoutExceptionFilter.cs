using System;
using System.Web;
using System.Web.Mvc;
using DowJones.Exceptions;
using DowJones.Web.Mvc.ActionResults;

namespace DowJones.Web.Mvc.ActionFilters
{
    public class SessionTimeoutExceptionFilter : HandleErrorAttribute
    {
        private readonly string _redirectUrl;

        public SessionTimeoutExceptionFilter() : this(null)
        {
        }

        public SessionTimeoutExceptionFilter(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            var dowJonesException = filterContext.Exception as DowJonesUtilitiesException;

            if (dowJonesException == null)
                return;

            if (dowJonesException.ReturnCode != DowJonesUtilitiesException.ErrorInvalidSessionLong)
                return;

            // TODO: Check to see if SessionID is actually invalid or not

            string landingPage = null;
            if (!(String.IsNullOrEmpty(_redirectUrl)))
            {
                landingPage = string.Format("http://{0}{1}", filterContext.HttpContext.Request.Url.Authority,
                                            VirtualPathUtility.ToAbsolute(_redirectUrl));
            }

            filterContext.Result = new AuthServerLoginRedirectActionResult() { LandingPage = landingPage };
            filterContext.ExceptionHandled = true;
        }
    }
}