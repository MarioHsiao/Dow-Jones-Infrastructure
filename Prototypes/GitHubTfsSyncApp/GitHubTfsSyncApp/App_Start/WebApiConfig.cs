using System.Web.Http;
using Newtonsoft.Json;

namespace GitHubTfsSyncApp
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			config.EnableQuerySupport();

			// To disable tracing in your application, please comment out or remove the following line of code
			// For more information, refer to: http://www.asp.net/web-api
			TraceConfig.Register(config);

			JsonFormatterConfig.Register(config);
		}
	}

	public class JsonFormatterConfig
	{
		public static void Register(HttpConfiguration config)
		{
			return;
			var jsonFormatter = config.Formatters.JsonFormatter;
			var jSettings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				DateParseHandling = DateParseHandling.DateTime,				
				//ContractResolver = new CamelCasePropertyNamesContractResolver(),
			};

			jsonFormatter.SerializerSettings = jSettings;
		}
	}
}
