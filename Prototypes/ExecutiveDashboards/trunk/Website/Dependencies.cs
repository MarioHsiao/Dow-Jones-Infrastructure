using DowJones.Infrastructure.Common;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Dash.Website
{
    public class Dependencies : DowJones.DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<IControlData>().ToMethod(x => new ControlData()).InRequestScope();
            Bind<IUserSession>().ToMethod(x => new UserSession { SessionId = "12345" }).InRequestScope();
            Bind<IPreferences>().ToMethod(x => new DowJones.Preferences.Preferences("en")).InRequestScope();
            Bind<IPrinciple>().ToMethod(x => new EntitlementsPrinciple(new GetUserAuthorizationsResponse())).InRequestScope();
            Bind<Product>().ToConstant(new GlobalProduct()).InSingletonScope();

            AutoBind<DataSources.IDataSource>(null, x => x.InSingletonScope());
        }
    }
}