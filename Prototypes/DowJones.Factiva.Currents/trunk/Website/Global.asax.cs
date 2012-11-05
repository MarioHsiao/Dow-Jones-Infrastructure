using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
	}
}