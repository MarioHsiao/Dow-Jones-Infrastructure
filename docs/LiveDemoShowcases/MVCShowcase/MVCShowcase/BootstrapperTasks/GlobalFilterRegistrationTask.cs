using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.ActionFilters;

namespace DowJones.MvcShowcase.BootstrapperTasks
{
    public class GlobalFilterRegistrationTask : IBootstrapperTask
    {
        public void Execute()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        internal void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SetThreadCultureToInterfaceLanguageFilter());

			// elmah adds this one for us, so we don't need to use the default one
			//filters.Add(new HandleErrorAttribute());
        }
    }
}