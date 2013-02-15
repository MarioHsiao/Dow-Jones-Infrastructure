using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using GitHubTfsSyncApp.Models.GitHub;
using Newtonsoft.Json;

namespace GitHubTfsSyncApp.Helpers
{
	public class WebHookResponseModelBinder : IModelBinder
	{
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			var payload = actionContext.Request.Content.ReadAsStringAsync().Result;	

			if (payload == null || !payload.Contains("payload="))
			{
				bindingContext.ModelState.AddModelError("payload", "payload not found");
				return false;
			}

			var decodedPayload = HttpUtility.UrlDecode(payload.Replace("payload=", ""));

			var settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
			bindingContext.Model = JsonConvert.DeserializeObject<WebHookResponse>(decodedPayload, settings);
			return true;
		}
	}

	public class WebHookResponseModelBinderProvider : ModelBinderProvider
	{
		public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
		{
			return new WebHookResponseModelBinder();
		}
	}
}