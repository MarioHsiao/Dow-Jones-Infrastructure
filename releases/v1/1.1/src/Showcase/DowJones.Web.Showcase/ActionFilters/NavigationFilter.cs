using System.Web.Mvc;
using DowJones.Web.Navigation;

namespace DowJones.Web.Showcase.ActionFilters
{
    public class NavigationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var navigationMenu = new NavigationMenu(new[] {
                    new NavigationMenuNode("Controls", new[] {
                        new ControllerActionMenuNode("Portal Headline List", controller: "PortalHeadlineList"),
                    })
                });

            filterContext.Controller.ViewBag.MainNavigationMenu = navigationMenu;
        }
    }
}