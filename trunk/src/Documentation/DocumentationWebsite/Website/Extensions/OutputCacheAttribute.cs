using System.Web;

namespace DowJones.Documentation.Website
{
    public class OutputCacheAttribute : System.Web.Mvc.OutputCacheAttribute
    {
        protected bool IsEnabled
        {
            get { return !HttpContext.Current.IsDebuggingEnabled; }
        }

        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (IsEnabled)
                base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            if(IsEnabled)
                base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
        {
            if (IsEnabled)
                base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(System.Web.Mvc.ResultExecutingContext filterContext)
        {
            if (IsEnabled)
                base.OnResultExecuting(filterContext);
        }
    }
}