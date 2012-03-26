using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

namespace DowJones.Web.Showcase.ActionFilters
{
    public class LogActivityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log("OnActionExecuting", filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext);
        }


        private static void Log(string methodName, ControllerContext context)
        {
            var routeData = context.RouteData;
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = string.Format("{0}:\t Controller:{1}\tAction:{2}", 
                                        methodName, controllerName, actionName);

            Debug.WriteLine(message, "Action Filter Log");

            WriteToTempData(context, message);
        }

        private static void WriteToTempData(ControllerContext context, string message)
        {
            var logMessages = context.Controller.TempData["LogMessages"] as IList<string>;

            if(logMessages == null)
            {
                logMessages = new List<string>();
                context.Controller.TempData["LogMessages"] = logMessages;
            }

            logMessages.Add(message);
        }
    }
}