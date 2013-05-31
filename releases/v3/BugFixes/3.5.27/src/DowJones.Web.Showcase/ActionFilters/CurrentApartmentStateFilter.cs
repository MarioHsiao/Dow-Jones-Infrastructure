using System.Web.Mvc;

namespace DowJones.Web.Showcase.ActionFilters
{
    public class CurrentApartmentStateFilter : IActionFilter 
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var apartmentState = System.Threading.Thread.CurrentThread.GetApartmentState();
            filterContext.Controller.ViewData["ApartmentState"] = apartmentState;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            /* NOOP */
        }
    }
}