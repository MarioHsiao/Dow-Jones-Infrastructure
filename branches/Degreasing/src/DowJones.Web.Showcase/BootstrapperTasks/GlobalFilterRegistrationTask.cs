using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.ActionFilters;
using DowJones.Web.Showcase.ActionFilters;

namespace DowJones.Web.Showcase.BootstrapperTasks
{
    public class GlobalFilterRegistrationTask : IBootstrapperTask
    {
        public void Execute()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        internal void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var order = 1;
            filters.Add(new SetThreadCultureToInterfaceLanguageFilter(), order++);
            filters.Add(new HandleErrorAttribute(), order++);
            filters.Add(new TrackExecutionTimeAttribute(), order++);
            filters.Add(new CurrentApartmentStateFilter(), order);
        }
    }
}