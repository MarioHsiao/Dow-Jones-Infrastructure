using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace GitHubTfsSyncApp.Helpers
{
	public class GithubModelBinder : DefaultModelBinder
	{
		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			var serializer = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
			
			return base.CreateModel(controllerContext, bindingContext, modelType);
		}
	}
}