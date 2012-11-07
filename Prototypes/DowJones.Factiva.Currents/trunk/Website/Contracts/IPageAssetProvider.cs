using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	public interface IPageAssetProvider
	{
		IEnumerable<IModule> GetModulesForPage(string pageName);
	}
}