using DowJones.Assemblers.Preferences;
using DowJones.Assemblers.Security;
using DowJones.Assemblers.Session;
using DowJones.Infrastructure.Common;
using DowJones.Preferences;
using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Web.Models;
using DowJones.Prod.X.Web.Models.Interfaces;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using MembershipService = DowJones.Prod.X.Core.Services.MembershipService;

namespace DowJones.Prod.X.Web
{
    public class Dependencies : DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            //BindToFactory<IControlData, ControlDataFactory>();
			/*
				The binding of DevelopmentSessionFactory is done to ensure that the home page runs on first load.
				The site will break when it is set to release mode...  
				Please set the binding to the UserSessionFactory and implement proper session management.
				BindToFactory<IUserSession, UserSessionFactory>();
			*/


            Unbind<IPreferences>();

            BindToFactory<IControlData, ControlDataFactory>().InRequestScope();
            BindToFactory<IUserSession, UserSessionFactory>().InRequestScope();/*
            BindToFactory<IPreferences, PreferencesFactory>().InRequestScope();
            BindToFactory<INewsletterPreferences, NewslettersPreferencesFactory>().InRequestScope();*/
            RebindToFactory<ReferringProduct, Common.ReferringProductFactory>().InRequestScope();
           /* BindToFactory<IPrinciple, PrincipleFactory>().InRequestScope();
            RebindToFactory<IPreferenceService, NewslettersCachedPreferenceServiceFactory>().InRequestScope();*/

            Bind<IUsageTrackingProperties>().To<UsageTrackingProperties>();
            Bind<IGenericSiteUrls>().To<GenericSiteUrls>();
            Bind<IActionProperties>().To<GenericActionProperties>();
            Bind<IBasicSiteRequestDto>().To<BasicSiteRequestDto>();
            Bind<Product>().ToConstant(new GlobalProduct());

            Bind<IPreferences>().ToMethod(x => new Preferences.Preferences("en"));
            //Bind<IPrinciple>().ToMethod(x => new EntitlementsPrinciple(new GetUserAuthorizationsResponse()));
            BindToFactory<IPrinciple, PrincipleFactory>().InRequestScope();

            // Services bindings
            Bind<Core.Interfaces.IMembershipService>().To<MembershipService>();
        }
    }
}