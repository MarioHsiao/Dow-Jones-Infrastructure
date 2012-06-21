using DowJones.Assemblers.Session;
using DowJones.Infrastructure.Common;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.MVCShowcase
{
    public class Dependencies : DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            BindToFactory<IControlData, ControlDataFactory>();
            BindToFactory<IUserSession, UserSessionFactory>();
            Bind<IPreferences>().ToMethod(x => new Preferences.Preferences("en"));
            Bind<IPrinciple>().ToMethod(x => new EntitlementsPrinciple(new GetUserAuthorizationsResponse()));
            Bind<Product>().ToConstant(new GlobalProduct());
        }
    }
}