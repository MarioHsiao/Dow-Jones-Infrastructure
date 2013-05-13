using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DowJones.Exceptions;
using DowJones.Globalization;
using log4net;
using Newtonsoft.Json;

namespace $rootnamespace$.Filters
{
    public class HandleExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (HandleExceptionAttribute));


        public const string KEY_ERROR_CODE = "ErrorCode";

        public const string KEY_ERROR_INFO = "ErrorInfo";

        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            var logmessage = String.Format("{0} {1} {2}",
                filterContext.RouteData.GetRequiredString("controller"),
                filterContext.RouteData.GetRequiredString("action"), ex);


            _logger.Debug(logmessage);

            string errorCode = String.Empty;//Find the error number based on exception(ex)

            //bool isSessionInvalid = false;//Check if the session is invalid
            //if(isSessionInvalid){
                //errorCode = "";//If invalid session, then set the error code to appropriate invalid session error code
            //}
            
            HttpResponseBase response = filterContext.HttpContext.Response;

            //if (isSessionInvalid && !filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    //If we have the return Product Url then redirect to it
            //    if (!String.IsNullOrEmpty(returnToProductUrl))
            //    {
            //        filterContext.HttpContext.Response.Redirect(returnToProductUrl, true);
            //        return;
            //    }

            //    //If there is no session then redirect to Cyclone url with appropriate product id and cyclone prefirx
            //    if (errorCode.Equals(UiConstant.ERR_NO_SESSION))
            //    {
            //        string redirectUrl = string.Format("{0}?p={1}", CycloneRedirectionURL, "snp"),
            //            cyclonePrefix = "";//Get appropriate Cyclone prefix url
                    
            //        if (!string.IsNullOrEmpty(cyclonePrefix))
            //            redirectUrl += "&F=" + cyclonePrefix;

            //        filterContext.HttpContext.Response.Redirect(redirectUrl, true);

            //        return;
            //    }

            //    ActionResult logiRedirect = EnsureSessionAttribute.GetLoginRedirect(filterContext);
            //    response.Clear();
            //    logiRedirect.ExecuteResult(filterContext.Controller.ControllerContext);
            //    response.End();
            //    return;
            //}
            
            //ViewDataDictionary x = filterContext.Controller.ViewData;

            //if (x.ContainsKey(KEY_ERROR_CODE))
            //{
            //    errorCode = x[KEY_ERROR_CODE].ToString();
            //}

            //string errorMessage = "";//Get the translated error message from errorCode

            //if(!string.IsNullOrEmpty(errorMessage) && errorMessage.Trim().Substring(0, 2) == "${")
            //    errorMessage = "errorForUserMinus1";//Get the translated error message for errorForUserMinus1

            //filterContext.ExceptionHandled = true;
            
            //string returnURL="history.back();";

            //x[KEY_ERROR_INFO] = null;//Create and set the error info using errorCode, errorMessage, ex and returnUrl

            //if (filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            //    var jsonView = GetJsonResultForAnException(x[KEY_ERROR_INFO]);
            //    filterContext.Result = jsonView;
            //}
            //else
            //{
            //    filterContext.Result = new ViewResult {ViewName = "ShowError", ViewData = x };
            //}

            response.Clear();
        }

		//Helper methods to return JsonResult from error info
        //private static JsonResult GetJsonResultForAnException(ErrorInfo errorInfo)
        //{
        //    return new JsonResult
        //               {
        //                   Data = GetJsonError(errorInfo), 
        //                   ContentEncoding = Encoding.UTF8,
        //                   JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //               };
        //}

        //private static string GetJsonError(ErrorInfo errorInfo)
        //{
        //    var a = new {Number = errorInfo.ErrorCode, Description = errorInfo.ErrorMessage};
        //    return JsonConvert.SerializeObject(new { ERROR = a });
        //}

        #endregion
    }
}