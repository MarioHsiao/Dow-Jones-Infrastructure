using DowJones.Assemblers.Session;
using DowJones.Infrastructure.Common;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace $rootnamespace$
{
    public class Dependencies : DowJones.DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            BindToFactory<IControlData, ControlDataFactory>();
			/*
				The binding of DevelopmentSessionFactory is done to ensure that the home page runs on first load.
				The site will break when it is set to release mode...  
				Please set the binding to the UserSessionFactory and implement proper session management.
				BindToFactory<IUserSession, UserSessionFactory>();
			*/

			BindToFactory<IUserSession, DevelopmentSessionFactory>();
            Bind<IPreferences>().ToMethod(x => new DowJones.Preferences.Preferences("en"));
            Bind<IPrinciple>().ToMethod(x => new EntitlementsPrinciple(new GetUserAuthorizationsResponse()));
            Bind<Product>().ToConstant(new GlobalProduct());
        }
    }
}