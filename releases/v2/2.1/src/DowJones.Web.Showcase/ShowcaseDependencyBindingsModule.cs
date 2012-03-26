using DowJones.Assemblers.Preferences;
using DowJones.Assemblers.Security;
using DowJones.Assemblers.Session;
using DowJones.DependencyInjection;
using DowJones.Infrastructure.Common;
using DowJones.Managers.RelatedConcept;
using DowJones.Managers.Sparkline;
using DowJones.Pages;
using DowJones.Preferences;
using DowJones.Security.Interfaces;
using DowJones.Session;
using DowJones.Web.Mvc.Search.UI.Components.Builders;
using DowJones.Web.Navigation;
using DowJones.Web.Showcase.Mocks;

namespace DowJones.Web.Showcase
{
    public class ShowcaseDependencyBindingsModule : DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            BindToFactory<IPrinciple, PrincipleFactory>().InRequestScope();
            BindToFactory<IControlData, ControlDataFactory>().InRequestScope();
            BindToFactory<IPreferences, PreferencesFactory>().InRequestScope();
            BindToFactory<IUserSession, DevelopmentSessionFactory>().InRequestScope();

//            RebindToFactory<IPreferenceService, CachedPreferenceServiceFactory>().InRequestScope();

            Bind<IMenuDataSource>().To<ShowcaseMenuDataSource>().InSingletonScope();

            Bind<IPageAssetsManager>().To<MockPageAssetsManager>().InRequestScope();

            // In a production app, this should be replaced with something meaningful rather than hard coded values
            Bind<Product>().ToConstant(new Product("CM", "Communicator", "Communicator")).InSingletonScope();

            Bind<ISparklineService>().To<SparklineService>();

            Bind<ISourceListService>().To<SourceListService>();

            Bind<IRelatedConceptService>().To<RelatedConceptService>();  
        }
    }
}
