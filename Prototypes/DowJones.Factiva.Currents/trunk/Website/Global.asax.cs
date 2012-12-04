using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DowJones.Factiva.Currents.Website.Controllers;

namespace DowJones.Factiva.Currents.Website
{
	using App_Start;

	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : Web.Mvc.HttpApplication
	{
		protected new void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			var exception = Server.GetLastError();
			var httpException = exception as HttpException;
			// TODO: Log this exception with your logger

			GlobalErrorHandler.HandleError(((HttpApplication)sender).Context, Server.GetLastError(), new ErrorController());
		}
	}
}