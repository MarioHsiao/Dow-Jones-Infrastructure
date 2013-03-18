using System;
using System.Web;
using System.Web.Mvc;

namespace DowJones.Web.Showcase.ActionFilters
{
    /// <summary>
    /// Measures the time it takes to execute an action
    /// </summary>
    public class TrackExecutionTimeAttribute : ActionFilterAttribute
    {
        public const string StopwatchKey = "Stopwatch";
        public const string ActionExecutionTimeKey = "ActionExecutionTime";
        public const string ResultExecutionElapsedTimeKey = "ResultExecutionElapsedTime";
        public const string ResultExecutionTimeKey = "ResultExecutionElapsedTime";

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            CaptureElapsedTime(filterContext, ActionExecutionTimeKey);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var totalElapsedTime = CaptureElapsedTime(filterContext, ResultExecutionElapsedTimeKey);
            var actionExecutionTime = (TimeSpan)filterContext.Controller.TempData[ActionExecutionTimeKey];

            var resultExecutionTime = totalElapsedTime - actionExecutionTime;
            filterContext.Controller.TempData[ResultExecutionTimeKey] = resultExecutionTime;
        }

        private static TimeSpan CaptureElapsedTime(ControllerContext filterContext, string key)
        {
            var elapsedTime = DateTime.Now - filterContext.HttpContext.Timestamp;
            filterContext.Controller.TempData[key] = elapsedTime;
            return elapsedTime;
        }


        public static string RenderElapsedTime(HttpContext httpContext)
        {
            httpContext = httpContext ?? HttpContext.Current;
            var elapsedTime = DateTime.Now - httpContext.Timestamp;
            return elapsedTime + "Boo ya!";
        }
    }
}