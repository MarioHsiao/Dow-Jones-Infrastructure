using System;
using System.Globalization;
using System.Net;
using System.Web;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using DowJones.Web.Configuration;

namespace DowJones.Web.Mvc.Infrastructure
{
	public class CoreJsHandler : ClientResourceHandler
	{
		private static readonly string[] JQueryAndRequire = new[] { "jquery", "require" };

		private static readonly string[] CoreLibraries = new[] {
                "underscore", "common", "dj-jquery-ext", "jquery-json",
                "pubsub", "composite-component", "jquery-ui", "jquery-ui-interactions"
            };

		public override void OnProcessRequest(HttpContextBase context)
		{
			var culture = SetRequestLanguage(context.Request["lang"]);

			if (ResourceHasNotBeenModified(context))
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotModified;
				return;
			}

			context.Response.Clear();
			context.Response.Buffer = true;

			RenderJQueryAndRequire(context, culture);

			RenderClientConfiguration(context);

			RenderCoreLibraries(context, culture);
		}

		private void RenderCoreLibraries(HttpContextBase context, CultureInfo culture)
		{
			RenderClientResources(context, CoreLibraries, culture);
		}

		private void RenderJQueryAndRequire(HttpContextBase context, CultureInfo culture)
		{
			// render scripts
			RenderClientResources(context, JQueryAndRequire, culture);
			
			// render config object
			var config = new RequireJsConfiguration { BaseUrl = GenerateRequireJsBaseUrl() };
			config.WriteTo(context.Response.Output);
		}

		private static bool ResourceHasNotBeenModified(HttpContextBase context)
		{
			// If client caching is disabled everything is always modified!
			if (!IsClientCachingEnabled(context))
				return false;

			var isModified = false;
			var ifModifiedSinceHeader = context.Request.Headers["If-Modified-Since"];
			DateTime ifModifiedSince;
			if (ifModifiedSinceHeader.HasValue() &&
				DateTime.TryParse(ifModifiedSinceHeader, out ifModifiedSince))
			{
				isModified = (ifModifiedSince.GetDay() <= LastModifiedCalculator(context));
			}
			return isModified;
		}

		private static void RenderClientConfiguration(HttpContextBase context)
		{
			var encrytedToken = context.Request["encryptedtoken"];
			var sessionId = context.Request["sessionid"];
			var language = context.Request["lang"];
			var debugEnabled = context.DebugEnabled();

			var clientConfiguration = new ClientConfiguration
			{
				Debug = debugEnabled,
				Credentials = new ClientCredentials { Token = encrytedToken, SessionId = sessionId },
				Preferences = new ClientPreferences { InterfaceLanguage = language },
			};

			clientConfiguration.WriteTo(context.Response.Output);
		}

	}
}
