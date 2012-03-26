using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Web.Showcase.ActionFilters;
using DowJones.Web.Showcase.Areas.Factiva.ActionFilters;

namespace DowJones.Web.Showcase.BootstrapperTasks
{
    public class GlobalFilterRegistrationTask : IBootstrapperTask
    {
        private readonly NavigationMenuPopulationFilter _navigationFilter;

        public GlobalFilterRegistrationTask(NavigationMenuPopulationFilter navigationFilter)
        {
            _navigationFilter = navigationFilter;
        }

        public void Execute()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        internal void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            int order = 1;
            filters.Add(new LanguageSetter(), order++);
            filters.Add(new HandleErrorAttribute(), order++);
            filters.Add(new TrackExecutionTimeAttribute(), order++);
            filters.Add(new CurrentApartmentStateFilter(), order++);
            filters.Add(_navigationFilter, order++);
        }
    }
}