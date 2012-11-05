using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	public interface IPageAssetProvider
	{
		int MapPageNameToId(string name);

		IEnumerable<IModule> GetModulesForPage(int pageId);

		IEnumerable<IModule> GetModulesForPage(string pageName);
	}
}