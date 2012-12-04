using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DowJones.Factiva.Currents.Website
{
	public class GlobalErrorHandler
	{
		public static void HandleError(HttpContext context, Exception ex, Controller controller)
		{
			LogException(ex);

			context.Response.StatusCode = GetStatusCode(ex);
			context.ClearError();
			context.Response.Clear();
			context.Response.TrySkipIisCustomErrors = true;

			ReturnErrorView(context, ex, controller);
		}

		public static void LogException(Exception ex)
		{
			// log the exception
		}

		private static void ReturnErrorView(HttpContext context, Exception ex, Controller controller)
		{
			var routeData = new RouteData();
			routeData.Values["controller"] = "Error";
			routeData.Values["action"] = GetActionName(GetStatusCode(ex));

			controller.ViewData.Model = new HandleErrorInfo(ex, " ", " ");
			((IController)controller).Execute(new RequestContext(new HttpContextWrapper(context), routeData));
		}


		private static int GetStatusCode(Exception ex)
		{
			return ex is HttpException ? ((HttpException)ex).GetHttpCode() : (int)HttpStatusCode.InternalServerError;
		}


		private static string GetActionName(int statusCode)
		{
			switch (statusCode)
			{
				case 404:
					return "NotFound";
				default:
					return "GenericError";

			}
		}
	}
}