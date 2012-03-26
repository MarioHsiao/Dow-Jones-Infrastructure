using System.Web.Mvc;
using DowJones.Web.Navigation;

namespace DowJones.Web.Showcase.Areas.Factiva.ActionFilters
{
    public class NavigationMenuPopulationFilter : ActionFilterAttribute
    {
        private readonly IMenuDataSource _dataSource;

        public NavigationMenuPopulationFilter(IMenuDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IMenu mainMenu = _dataSource.GetMenu("factiva-main-menu");
            filterContext.Controller.ViewData["MainNavigationMenu"] = mainMenu;
        }

    }
}