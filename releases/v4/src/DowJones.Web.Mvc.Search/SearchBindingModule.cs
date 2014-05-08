using DowJones.Web.Mvc.Search.Managers.Preferences;
using DowJones.Web.Mvc.Search.Requests.Common;
using DowJones.Web.Mvc.Search.UI.Components.Builders;
using Ninject.Web.Common;

namespace DowJones.Web.Mvc.Search
{
    public class SearchBindingModule : DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<ISearchBuilderFactory>().To<SearchBuilderFactory>().InRequestScope();

            Bind<ISearchRequestDefaultSettingFactory>().To<SearchRequestDefaultSettingFactory>().InRequestScope();

            Bind<ISearchBuilderPreferences>().To<SearchBuilderPreferences>().InRequestScope();

        }
    }
}
