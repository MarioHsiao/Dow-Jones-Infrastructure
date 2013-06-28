using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.ActionFilters;
using HandleErrorAttribute = DowJones.Prod.X.Web.Filters.HandleErrorAttribute;

namespace DowJones.Prod.X.Web.BootstrapperTasks
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
            //filters.Add(new HandleErrorAttribute());
        }
    }
}