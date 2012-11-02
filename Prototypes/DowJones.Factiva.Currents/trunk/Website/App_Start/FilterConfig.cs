using System.Web.Mvc;
using DowJones.Infrastructure;

namespace DowJones.Factiva.Currents.Website.App_Start
{
	public class FilterConfigTask : IBootstrapperTask
	{
		public void Execute()
		{
			RegisterGlobalFilters(GlobalFilters.Filters);
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}