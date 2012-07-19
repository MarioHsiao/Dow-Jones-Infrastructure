using DowJones.Infrastructure.Common;
using DowJones.Preferences;
using DowJones.Security.Interfaces;
using DowJones.Session;

namespace DowJones.Web.ViewComponentRenderingService
{
    public class Bindings : DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IUserSession>().ToMethod(ctx => new UserSession());
            Bind<IControlData>().ToMethod(ctx => new ControlData());
            Bind<IPreferences>().ToConstant(new DowJones.Preferences.Preferences("en"));
            Bind<IPrinciple>().ToMethod(ctx => new DummyPrinciple());
            Bind<Product>().ToConstant(new GlobalProduct());
        }

        class DummyPrinciple : IPrinciple
        {
            public IUserSubPrinciple UserServices
            {
                get { return null; }
            }

            public ICoreServicesSubPrinciple CoreServices
            {
                get { return null; }
            }

            public IRuleSet RuleSet
            {
                get { return null; }
            }
        }
    }
}