using System.Web.Mvc;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.ActionFilters;

namespace DowJones.Dash.Website.App_Start
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
            filters.Add(new HandleErrorAttribute());
        }
    }
}