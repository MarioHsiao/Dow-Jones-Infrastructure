using System.Web;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Factiva.Currents.Website.Mocks;
using DowJones.Factiva.Currents.Website.Providers;
using DowJones.Infrastructure.Common;
using DowJones.Pages.Modules.Modules.Summary;
using DowJones.Pages.Modules.Templates;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DowJones.Factiva.Currents.Website
{
	public class Dependencies : DependencyInjection.DependencyInjectionModule
	{
		protected override void OnLoad()
		{
			Bind<IControlData>().ToMethod(x => new ControlData()).InRequestScope();
			Bind<IUserSession>().ToMethod(x => new UserSession {SessionId = "12345"}).InRequestScope();
			Bind<IPreferences>().ToMethod(x => new Preferences.Preferences("en")).InRequestScope();
			Bind<IPrinciple>().ToMethod(x => new EntitlementsPrinciple(new GetUserAuthorizationsResponse())).InRequestScope();
			Bind<Product>().ToConstant(new GlobalProduct()).InSingletonScope();

			Bind<JsonSerializerSettings>()
				.ToConstant(new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore,
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
						ContractResolver = new CamelCasePropertyNamesContractResolver(),
						Converters = new[] {new StringEnumConverter()},
						TypeNameHandling = TypeNameHandling.Objects,
					});

			if (HttpContext.Current.IsDebuggingEnabled)
			{
				Bind<IPageAssetProvider>().To<PageAssetProvider>().InRequestScope();
			}
			else
			{
				Bind(typeof(ICacheProvider<>)).To(typeof(HttpContextCacheProvider<>)).InRequestScope();
				Bind<IPageAssetProvider>().To<CachedPageAssetProvider>().InRequestScope();
			}

			Bind<IScriptModuleTemplateManager>().To<MockScriptModuleTemplateManager>();

			Bind<IPageServiceResponseParser>()
				.To<PageServiceResponseParser>()
				.WithConstructorArgument("assemblies", 
											new[] { 
												typeof(PageServiceResponse).Assembly, 
												typeof(Pages.Tag).Assembly,
 												typeof(SummaryNewspageModule).Assembly  
											});

			Bind<ISearchContext>().To<SearchContextManager>();
		}
	}
}