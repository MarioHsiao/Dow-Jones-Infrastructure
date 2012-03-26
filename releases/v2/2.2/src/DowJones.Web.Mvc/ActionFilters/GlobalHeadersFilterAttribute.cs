using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.ActionFilters
{
    public class GlobalHeadersFilterAttribute : ActionFilterAttribute
    {
        private readonly GlobalHeaders headers;

        [Inject("X")]
        public GlobalHeaders GlobalHeaders { get; set; }

        public GlobalHeadersFilterAttribute()
        {
                
        }
        public GlobalHeadersFilterAttribute(GlobalHeaders headers)
        {
            this.headers = headers;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.GlobalHeaders = GlobalHeaders;
        }
    }
}
