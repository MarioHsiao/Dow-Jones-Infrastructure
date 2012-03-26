using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using DowJones.Web.Showcase.ActionFilters;

namespace DowJones.Web.Showcase.BootstrapperTasks
{
    public class GlobalActionFilterRegistration : IBootstrapperTask
    {
        [Inject]
        protected NavigationFilter NavigationFilter { get; set; }


        public void Execute()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
        }


        internal void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute(), 1);
            filters.Add(new AssemblyVersionActionFilter(), 2);
            filters.Add(NavigationFilter, 3);
        }
    }
}