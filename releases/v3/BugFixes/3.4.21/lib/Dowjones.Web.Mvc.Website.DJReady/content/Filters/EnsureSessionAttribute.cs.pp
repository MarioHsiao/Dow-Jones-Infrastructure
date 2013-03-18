using System;
using System.Web.Configuration;
using System.Web.Mvc;
using DowJones.Properties;
using DowJones.Web.Mvc.ActionResults;
using log4net;
using System.IO;

namespace $rootnamespace$.Filters
{
    public class EnsureSessionAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof (EnsureSessionAttribute));

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            _logger.Debug(string.Format("Thread Apartment State: {0}", System.Threading.Thread.CurrentThread.GetApartmentState()));
            _logger.Debug("Check for valid session");

            ////Check if session is invalid. If yes then redirect to return product url if it is present or else redirect for authentication
            //if (Is_InValid_Session)
            //{
            //    if (!String.IsNullOrEmpty(returnToProductUrl))
            //    {
            //        _logger.Debug("Redirect to caller product URL");
            //        filterContext.HttpContext.Response.Redirect(returnToProductUrl, true);
            //        return;
            //    }

            //    _logger.Debug("Invalid session - Redirect for authentication");
            //    ActionResult r = GetLoginRedirect(filterContext);
            //    filterContext.Result = r;
            //}
        }

        #endregion

        public static ActionResult GetLoginRedirect(ControllerContext ctx)
        {
            _logger.Debug("Build redirect view for authentication");

            string cycloneUrl = String.Empty;//Set the appropriate Cyclone url
            return new RedirectResult(String.Format("{0}?p=snp", cycloneUrl));
        }
    }
}