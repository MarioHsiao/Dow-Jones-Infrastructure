using System;
using System.Globalization;
using System.Net;
using System.Web;
using DowJones.Extensions;
using DowJones.Extensions.Web;
using DowJones.Infrastructure;
using DowJones.Web.Configuration;

namespace DowJones.Web.Mvc.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public class CoreJsHandler : ClientResourceHandler
	{
		//private static readonly string[] JQueryAndRequire = new[] { "jquery", "require" };
		private static readonly string[] CoreLibraries = new[] {
                "underscore", "common", "pubsub", "composite-component", "dj-jquery-ext", "jquery-json",
                  "jquery-ui", "jquery-ui-interactions", "tmpload"
            };

		/// <summary>
		/// Called when [process request].
		/// </summary>
		/// <param name="context">The context.</param>
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
			RenderClientResources(context, "jquery", culture);

			RenderScopedRequire(context, culture);

		}

		private void RenderScopedRequire(HttpContextBase context, CultureInfo culture)
		{
			context.Response.Write("var _djRequire = (function () {\n  ");
			// render config object
			var config = new RequireJsConfiguration { BaseUrl = GenerateRequireJsBaseUrl() };
			config.WriteVariableTo(context.Response.Output);

			RenderClientResources(context, "require", culture);

			context.Response.Write("\n  return { require: require, define: define};\n  }());\n");
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
				Preferences = new ClientPreferences { InterfaceLanguage = language },
			};

			if (sessionId.IsNotEmpty())
			{
				clientConfiguration.Credentials = new ClientCredentials { Token = sessionId, CredentialType = CredentialType.Session };
			}
			else if (encrytedToken.IsNotEmpty())
			{
				clientConfiguration.Credentials = new ClientCredentials { Token = encrytedToken, CredentialType = CredentialType.EncryptedToken };
			}

			clientConfiguration.WriteTo(context.Response.Output);
		}

	}
}
