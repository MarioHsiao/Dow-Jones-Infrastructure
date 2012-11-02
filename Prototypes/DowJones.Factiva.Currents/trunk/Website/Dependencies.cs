using DowJones.Infrastructure.Common;
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
			Bind<IUserSession>().ToMethod(x => new UserSession { SessionId = "12345" }).InRequestScope();
			Bind<IPreferences>().ToMethod(x => new Preferences.Preferences("en")).InRequestScope();
			Bind<IPrinciple>().ToMethod(x => new EntitlementsPrinciple(new GetUserAuthorizationsResponse())).InRequestScope();
			Bind<Product>().ToConstant(new GlobalProduct()).InSingletonScope();

			Bind<JsonSerializerSettings>()
				.ToConstant(new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					ContractResolver = new CamelCasePropertyNamesContractResolver(),
					Converters = new[] { new StringEnumConverter() },
					TypeNameHandling = TypeNameHandling.Objects,
				});

		}
	}
}