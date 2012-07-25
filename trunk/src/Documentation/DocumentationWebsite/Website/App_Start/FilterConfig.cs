

using System.Web.Mvc;

namespace DowJones.Documentation.Website.App_Start
{
	public class FilterConfig
	{
		public void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			// elmah adds this one for us, so we don't need to use the default one
			//filters.Add(new HandleErrorAttribute());
		}
	}
}