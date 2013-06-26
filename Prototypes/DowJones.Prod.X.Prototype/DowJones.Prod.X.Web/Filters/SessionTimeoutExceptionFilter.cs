using System;
using System.Globalization;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Prod.X.Common.Exceptions;
using DowJones.Prod.X.Core.Interfaces;
using DowJones.Session;
using log4net;

namespace DowJones.Prod.X.Web.Filters
{
    public class SessionTimeoutExceptionFilter : HandleErrorAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SessionTimeoutExceptionFilter));
        private IUserSession _session;
        private IMembershipService _validateSessionService;

        [Inject("Constructor injection not available in Action Filter Attributes")]
        public IUserSession Session
        {
            get { return _session; }

            set
            {
                _session = value;
            }
        }

        [Inject("Constructor injection not available in Action Filter Attributes")] 
        public IMembershipService ValidateSessionService
        {
            get { return _validateSessionService; }

            set
            {
                _validateSessionService = value;
            }
        }
/*
        [Inject("Constructor injection not available in Action Filter Attributes")]
        public IClientStateCacheService ClientStateCacheService
        {
            get { return _clientStateCacheService; }

            set
            {
                _clientStateCacheService = value;
            }
        }*/

        public override void OnException(ExceptionContext filterContext)
        {
            var errorCode = GetErrorNumber(filterContext.Exception);
            //var dowJonesException = filterContext.Exception as DowJonesUtilitiesException;

            //if (dowJonesException == null)
            //{
            //    return;
            //}

            if (IsValidSession(ref errorCode))
            {
                return;
            }

            filterContext.Result = new RedirectResult(GetReturnToProductUrl(filterContext));
            filterContext.ExceptionHandled = true;
        }

        public string GetReturnToProductUrl(ExceptionContext filterContext)
        {
            //Get the return url from the client state cache
            /*var data = _clientStateCacheService.GetItem();
            if(data != null && !String.IsNullOrEmpty(data.ReturnUrl))
            {
                return data.ReturnUrl;
            }*/
            return string.Format("{0}{1}", (filterContext.HttpContext != null && filterContext.HttpContext.Request != null && filterContext.HttpContext.Request.Url != null) ? String.Format("{0}://", filterContext.HttpContext.Request.Url.Scheme) : "//", X.Common.Properties.Settings.Default.BaseDotComHost);
        }

        public static string GetErrorNumber(Exception exception)
        {
            var t = exception.GetType();
            var errorNumber = string.Empty;
            if (typeof(DowJonesUtilitiesException).IsAssignableFrom(t))
            {
                errorNumber = ((DowJonesUtilitiesException)exception).ReturnCode.ToString(CultureInfo.InvariantCulture);
            }

            if (errorNumber.IsNullOrEmpty())
            {
                errorNumber = "-1";
            }
            return errorNumber;
        }

        private bool IsValidSession(ref string errorCode)
        {
            if (errorCode.Equals(ApplicationExceptionConstants.ErrNoSession) || errorCode.Equals(ApplicationExceptionConstants.ErrInvalidSession))
            {
                Log.Debug(" Not valid Session ");
                return false;
            }

            if (Session == null || !Session.IsValid())
            {
                errorCode = ApplicationExceptionConstants.ErrNoSession;
                Log.Debug(" Empty Session ");
                return false;
            }

            var isValid = _validateSessionService.IsSessionValid();

            if (!isValid)
            {
                errorCode = _validateSessionService.LastError.ToString(CultureInfo.InvariantCulture);
                Log.Debug(" Not valid Session ");
            }
            return isValid;
        }
    }
}