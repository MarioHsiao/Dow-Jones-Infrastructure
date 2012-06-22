using System.Web.Mvc;

namespace DowJones.Documentation.Website.App_Start
{
	public class FilterConfig
	{
		public void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}