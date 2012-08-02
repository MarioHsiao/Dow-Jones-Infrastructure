using DowJones.Infrastructure.Common;
using DowJones.Pages;
using DowJones.Preferences;
using DowJones.Security.Interfaces;
using DowJones.Session;

namespace DowJones.DegreasedDashboards.Website
{
    public class Dependencies : DowJones.DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IControlData>().ToConstant(new ControlData());
            Bind<IUserSession>().ToConstant(new UserSession { SessionId = "1234567890" });
            Bind<IPreferences>().ToMethod(x => new DowJones.Preferences.Preferences("en"));
            Bind<IPrinciple>().ToConstant(new Principle());
            Bind<Product>().ToConstant(new GlobalProduct());

            Bind<IPageManager>().To<InMemoryPageManager>().InSingletonScope();
            Bind<IPageSubscriptionManager>().To<InMemoryPageSubscriptionManager>().InSingletonScope();
        }

        public class Principle : IPrinciple
        {
            public IUserSubPrinciple UserServices { get; private set; }
            public ICoreServicesSubPrinciple CoreServices { get; private set; }
            public IRuleSet RuleSet { get; private set; }
        }
    }
}